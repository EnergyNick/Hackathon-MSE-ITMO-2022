<?php

namespace App\Services\EditService;

use App\Http\Requests\EditRequests\AppendFileEditRequest;
use App\Http\Requests\EditRequests\FileEditRequest;
use Illuminate\Http\JsonResponse;

interface EditServiceInterface
{
    public function edit(): JsonResponse;
    public function appendFile(AppendFileEditRequest $request):JsonResponse;
    public function upload(FileEditRequest $request): JsonResponse;
}
