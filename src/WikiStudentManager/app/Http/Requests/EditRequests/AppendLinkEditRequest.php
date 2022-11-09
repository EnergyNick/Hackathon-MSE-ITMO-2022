<?php

namespace App\Http\Requests\EditRequests;

use App\Http\Requests\BaseRequest;

class AppendLinkEditRequest extends BaseRequest
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
            'section.required' => 'field must not be empty',
            'section.integer' => 'field must be integer',
            'link.required' => 'field must not be empty if exists',
            'link.string' => 'field must be string',
            'tag.required' => 'field must not be empty',
            'tag.max' => 'max length 255',
            'tag.string' => 'field must be string',
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
            'section' => 'required|integer',
            'link' => 'required|string',
            'tag' => 'required|string|max:255',
        ];
    }
}
