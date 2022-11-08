<?php

namespace App\Http\Requests\AuthRequests;

use App\Http\Requests\BaseRequest;

class AuthUserRequest extends BaseRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    protected function authorize(): bool
    {
        return true;
    }

    /**
     * Get the message of error.
     * 
     * @return array
     */
    protected function messages(): array
    {
        return [
            'username.required' => 'field must not be empty',
            'username.max' => 'max length 255',
            'username.string' => 'field must be string',
            'password.required' => 'field must not be empty',
            'password.max' => 'max length 255',
            'password.string' => 'field must be string',
        ];
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    protected function rules(): array
    {
        return [
            'username' => 'required|string|max:255',
            'password' => 'required|string|max:255',
        ];
    }
}
