<?php

namespace App\Http\Controllers\AuthControllers;

use App\Http\Controllers\BaseController;
use App\Services\AuthService\AuthServiceInterface;

class AuthController extends BaseController
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

    public function test()
    {
        return response()->json(['text' => 'Hello world', 200]);
    }
}
