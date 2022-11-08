<?php

namespace App\Responses;

use App\Responses\ResponseInterface;
use Illuminate\Http\JsonResponse;

abstract class AbstractResponse implements ResponseInterface
{
    /**
     * Return json response.
     * 
     * @param string|null $message
     * @param array|null $data
     * @param int $code
     * @param bool $status
     * @return JsonResponse
     */
    public static function baseResponse(string|null $message, array|null $data, int $code, bool $status): JsonResponse
    {
        return new JsonResponse(['success' => $status, 'message' => $message, 'data' => $data], $code);
    }
}
