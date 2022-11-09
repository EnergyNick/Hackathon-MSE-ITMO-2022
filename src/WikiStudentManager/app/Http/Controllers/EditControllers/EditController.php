<?php

namespace App\Http\Controllers\EditControllers;

use App\Http\Controllers\BaseController;
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
     * Edit page.
     */
    public function edit(): JsonResponse
    {
        return $this->service->edit();
    }
}
