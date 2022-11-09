<?php

namespace App\Http\Controllers\AuthControllers;

use App\Http\Controllers\BaseController;
use App\Services\AuthService\AuthServiceInterface;
use Illuminate\Http\JsonResponse;

class EditController extends BaseController
{
    /**
     * Initialize class of service.
     * 
     * @param AuthService
     * @return void
     */
    public function __construct(AuthServiceInterface $service)
    {
        $this->service = $service;
    }

    public function auth(): JsonResponse
    {
        return $this->service->auth();
    }
}
