<?php

namespace App\Services\AuthService;

use Illuminate\Http\JsonResponse;

interface AuthServiceInterface
{
    public function auth(): JsonResponse;
}
