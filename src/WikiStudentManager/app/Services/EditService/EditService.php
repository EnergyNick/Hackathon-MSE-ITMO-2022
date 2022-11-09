<?php

namespace App\Services\EditService;

use App\Exceptions\IntegrationException;
use App\Helpers\GetTokenHelper;
use App\Http\Requests\EditRequests\FileEditRequest;
use App\Responses\SuccessResponse;
use GuzzleHttp\Psr7\Utils;
use Illuminate\Http\JsonResponse;
use Illuminate\Support\Facades\Http;

class EditService implements EditServiceInterface
{
    public function edit(): JsonResponse
    {
        dd(1);

        return response()->json(['d' => 's'], 200);
    }

    public function upload(FileEditRequest $request): JsonResponse
    {
        $data = $request->validated();

        $name = $data['filename'];

        $url = 'https://wiki.compscicenter.ru/api.php?action=upload&' . 'filename=' . $name . '&format=json';

        $headers = ['Cookie' => $request->get('Cookie')] + ['Content-Type: multipart/form-data'];

        $file = fopen($_FILES['file']['tmp_name'], 'r+');

        $token = GetTokenHelper::csrf();

        $body = [
            'token' => $token,
            'file' => $file,
        ];

        $response = Http::withHeaders($headers)
            ->attach($_FILES['file']['name'], $file, $_FILES['file']['name'])
            ->post($url, $body)->json();

        if (array_key_exists('upload', $response) && array_key_exists('result', $response['upload']) && $response['upload']['result'] == 'Warning') {
            throw new IntegrationException('error upload file', null, 400);
        }

        return SuccessResponse::response('file upload', ['name' => $name . $data['file']->extension()], 200);
    }
}
