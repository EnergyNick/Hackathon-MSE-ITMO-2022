<?php

namespace App\Services\AuthService;

use App\Helpers\Auth;
use Illuminate\Http\JsonResponse;

class AuthService implements AuthServiceInterface
{
    const SESSION = 'session';

    public function auth(): JsonResponse
    {
        $response = Auth::authUser();

        return response()->json($response->json(), 200);
    }
}
