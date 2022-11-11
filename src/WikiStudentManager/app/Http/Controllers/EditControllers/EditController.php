<?php

namespace App\Http\Controllers\EditControllers;

use App\Http\Controllers\BaseController;
use App\Http\Requests\EditRequests\AppendFileEditRequest;
use App\Http\Requests\EditRequests\AppendLinkEditRequest;
use App\Http\Requests\EditRequests\FileEditRequest;
use App\Services\EditService\EditServiceInterface;
use Illuminate\Http\JsonResponse;

class EditController extends BaseController
{
    /**
     * Initialize class of service.
     * 
     * @param AuthService
     * @return void
     */
    public function __construct(EditServiceInterface $service)
    {
        $this->service = $service;
    }

    /**
     * Append file to section.
     * @param AppendFileEditRequest
     * @return JsonResponse
     */
    public function appendFile(AppendFileEditRequest $request): JsonResponse
    {
        return $this->service->appendFile($request);
    }

    /**
     * Append link to section.
     * @param AppendFileEditRequest
     * @return JsonResponse
     */
    public function appendLink(AppendLinkEditRequest $request): JsonResponse
    {
        return $this->service->appendLink($request);
    }

    /**
     * Upload file.
     * 
     * @param FileEditRequest $request
     * @return JsonResponse
     */
    public function upload(FileEditRequest $request): JsonResponse
    {
        return $this->service->upload($request);
    }
}
