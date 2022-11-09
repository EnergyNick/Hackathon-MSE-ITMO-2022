<?php

namespace App\Services\EditService;

use Illuminate\Http\JsonResponse;

class EditService implements EditServiceInterface
{
    public function edit(): JsonResponse
    {
        dd(1);

        return response()->json(['d' => 's'], 200);
    }
}
