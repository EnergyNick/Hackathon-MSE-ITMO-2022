<?php

namespace App\Responses;

use Illuminate\Http\JsonResponse;

class SuccessResponse extends AbstractResponse
{
    /**
     * Return json response.
     * 
     * @param string|null $message
     * @param array|null $data
     * @param int $code
     * @return JsonResponse
     */
    public static function response(string|null $message, array|null $data, int $code): JsonResponse
    {
        return parent::baseResponse($message, $data, $code, true);
    }
}
