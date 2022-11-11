<?php

namespace App\Services\EditService;

use App\Http\Requests\EditRequests\AppendFileEditRequest;
use App\Http\Requests\EditRequests\AppendLinkEditRequest;
use App\Http\Requests\EditRequests\FileEditRequest;
use Illuminate\Http\JsonResponse;

interface EditServiceInterface
{
    public function upload(FileEditRequest $request): JsonResponse;
    public function appendFile(AppendFileEditRequest $request):JsonResponse;
    public function appendLink(AppendLinkEditRequest $request):JsonResponse;
}
