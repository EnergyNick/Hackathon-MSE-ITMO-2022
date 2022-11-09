<?php

namespace App\Http\Requests\EditRequests;

use App\Http\Requests\BaseRequest;

class FileEditRequest extends BaseRequest
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
            'filename.required' => 'field must not be empty',
            'filename.max' => 'max length 255',
            'filename.string' => 'field must be string',
            'tags.filled' => 'field must not be empty if exists',
            'tags.max' => 'max length 255',
            'tags.string' => 'field must be string',
            'text.filled' => 'field must not be empty if exists',
            'text.max' => 'max length 255',
            'text.string' => 'field must be string',
            'comment.filled' => 'field must not be empty if exists',
            'comment.string' => 'field must be string',
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
            'file' => 'required|file',
            'filename' => 'required|string|max:255',
            'comment' => 'filled|string',
            'tags' => 'filled|string|max:255',
            'text' => 'filled|string|max:255',
        ];
    }
}
