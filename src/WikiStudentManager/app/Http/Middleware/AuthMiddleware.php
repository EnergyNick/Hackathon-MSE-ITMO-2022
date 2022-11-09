<?php

namespace App\Http\Middleware;

use App\Helpers\Auth;
use App\Models\KeyValue;
use Closure;

class AuthMiddleware
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
        $key = config('wiki_auth.auth');
        
        if (is_null(KeyValue::getOfKey($key))) {
            $response = Auth::authUser();
            $cookies = $response->cookies()->toArray();
            $cookie = '';
            foreach ($cookies as $c) {
                if (array_key_exists('Name', $c) && array_key_exists('Value', $c)) {
                    $cookie .= $c['Name'] . '=' . $c['Value'] . '; ';
                }
            }
            KeyValue::setOfKey($key, $cookie);
        }
        
        $cookie = KeyValue::getOfKeyOrFail($key);
        
        $request->request->add(['Cookie' => $cookie]);

        return $next($request);
    }
}
