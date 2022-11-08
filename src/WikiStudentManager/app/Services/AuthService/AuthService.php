<?php

namespace App\Services\AuthService;

use App\Helpers\Auth;
use App\Helpers\GetTokenHelper;
use App\Responses\SuccessResponse;
use Illuminate\Http\JsonResponse;
use Illuminate\Support\Facades\Http;

class AuthService implements AuthServiceInterface
{
    public function auth(): JsonResponse
    {
        $response = Auth::authBot();
        return response()->json($response->json(), 200);
    }
}
