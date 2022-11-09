<?php

namespace App\Services\AuthService;

use Illuminate\Http\JsonResponse;

interface EditServiceInterface
{
    public function edit(): JsonResponse;
}
