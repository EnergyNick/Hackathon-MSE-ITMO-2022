<?php

namespace App\Http\Requests;

use Anik\Form\FormRequest;
use Illuminate\Http\JsonResponse;

abstract class BaseRequest extends FormRequest
{
    protected function errorResponse(): ?JsonResponse
    {
        return response()->json([
            'success' => false,
            'message' => $this->errorMessage(),
            'errors' => $this->validator->errors()->messages(),
        ], $this->statusCode());
    }
}
