<?php

namespace App\Services\AuthService;

use App\Responses\SuccessResponse;
use Illuminate\Http\JsonResponse;

class AuthService implements AuthServiceInterface
{
    public function auth(): JsonResponse
    {
        return SuccessResponse::response(config('wiki.url'), ['ggg' => 'fff'], 200);
    }
}
