<?php

namespace App\Services\EditService;

use Illuminate\Http\JsonResponse;

interface EditServiceInterface
{
    public function edit(): JsonResponse;
}
