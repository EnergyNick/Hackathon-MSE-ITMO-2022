<?php

namespace App\Http\Middleware;

use App\Helpers\Auth;
use App\Models\KeyValue;
use Carbon\Carbon;
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

        if (
            is_null(KeyValue::getOfKey($key)) ||
            KeyValue::getOfKey($key)->updated_at->diffInSeconds(Carbon::now()) > unserialize(KeyValue::getOfKeyOrFail($key))['Expires']
        ) {
            $response = Auth::authUser();
            $cookies = $response->cookies()->toArray();
            $expires = $cookies[1]['Expires'];
            $cookie = '';
            foreach ($cookies as $c) {
                if (array_key_exists('Name', $c) && array_key_exists('Value', $c)) {
                    $cookie .= $c['Name'] . '=' . $c['Value'] . '; ';
                }
            }
            $cookie = substr($cookie, 0, -2);
            $newCookie = serialize(['Cookie' => $cookie, 'Expires' => $expires]);
            KeyValue::setOfKey($key, $newCookie);
        }

        $cookie = unserialize(KeyValue::getOfKeyOrFail($key))['Cookie'];

        $request->request->add(['Cookie' => $cookie]);

        return $next($request);
    }
}
