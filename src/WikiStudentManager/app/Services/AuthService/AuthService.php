<?php

namespace App\Services\AuthService;

class AuthService implements AuthServiceInterface
{
    public function auth(): string
    {
        return 'Hello world';
    }
}
