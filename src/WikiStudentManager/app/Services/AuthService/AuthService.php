<?php

namespace App\Services\AuthService;

use App\Helpers\GetTokenHelper;
use App\Responses\SuccessResponse;
use Illuminate\Http\JsonResponse;
use Illuminate\Support\Facades\Http;

class AuthService implements AuthServiceInterface
{
    public function auth(): JsonResponse
    {
        // $tokenAuth = GetToken::login();
        $tokenCsrf = GetTokenHelper::csrf();

        return SuccessResponse::response($tokenCsrf, ['ggg' => 'fff'], 200);
    }
}
