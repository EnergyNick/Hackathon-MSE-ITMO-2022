<?php

namespace App\Http\Middleware;

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
            dd(null);
        }
        dd(1);
        return $next($request);
    }
}
