<?php

namespace App\Responses;

use Illuminate\Http\JsonResponse;

interface ResponseInterface
{
    public static function baseResponse(string|null $message, array|null $data, int $code, bool $status): JsonResponse;
}
