<?php

namespace App\Helpers;

use App\Exceptions\IntegrationException;
use Illuminate\Support\Facades\Http;

class GetToken
{
    /**
     * Get token for auth.
     * @return string|null
     */
    public static function login(): string|null
    {
        $url = config('wiki.url') . config('wiki.token_auth');
        $request = Http::get($url);
        $json = $request->json() ?? [];
        if (
            array_key_exists('query', $json) &&
            array_key_exists('tokens', $json['query']) &&
            array_key_exists('logintoken', $json['query']['tokens'])
        ) {
            return $json['query']['tokens']['logintoken'];
        } else {
            throw new IntegrationException('request to get token failed', null, 408);
        }
    }

    /**
     * Get token for edit.
     * @return string|null
     */
    public static function csrf(): string|null
    {
        $url = config('wiki.url') . config('wiki.token_edit');
        $request = Http::get($url);
        $json = $request->json() ?? [];
        if (
            array_key_exists('query', $json) &&
            array_key_exists('tokens', $json['query']) &&
            array_key_exists('csrftoken', $json['query']['tokens'])
        ) {
            $token = $json['query']['tokens']['csrftoken'];
            throw_if($token = "+\\", new IntegrationException('empty token received', null, 408));
            return $token;
        } else {
            throw new IntegrationException('request to get token failed', null, 408);
        }
    }
}
