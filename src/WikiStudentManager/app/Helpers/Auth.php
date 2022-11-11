<?php

namespace App\Helpers;

use Illuminate\Support\Facades\Http;
use \Illuminate\Http\Client\Response;

class Auth
{
    /**
     * Auth user.
     * @param string $logintoken
     * @return Response
     */
    public static function authUser(): Response
    {
        $tokenAuth = GetTokenHelper::login();

        $headers = [
            'Cookie' => 'sewiki_session=' . GetTokenHelper::getCookies()[0]['Value']
        ];
        
        $options = [
            'logintoken' => $tokenAuth,
            'loginreturnurl' => 'https://wiki.compscicenter.ru/',
            'username' => config('data_wiki.username'),
            'password' => config('data_wiki.password')
        ];

        $response = Http::asForm()->withHeaders($headers)
            ->post(config('wiki.auth_user'), $options);

        return $response;
    }

    /**
     * Auth bot.
     * @param string $logintoken
     * @return Response
     */
    public static function authBot(): Response
    {
        $tokenAuth = GetTokenHelper::login();

        $headers = [
            'Cookie' => 'sewiki_session=' . GetTokenHelper::getCookies()[0]['Value']
        ];

        $options = [
            'lgtoken' => $tokenAuth,
            'lgname' => config('data_wiki.username'),
            'lgpassword' => config('data_wiki.password')
        ];
        
        $response = Http::asForm()->withHeaders($headers)
            ->post(config('wiki.auth_bot'), $options);

        return $response;
    }
}
