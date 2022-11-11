<?php

/** @var \Laravel\Lumen\Routing\Router $router */

use Illuminate\Support\Facades\Http;

/*
|--------------------------------------------------------------------------
| Application Routes
|--------------------------------------------------------------------------
|
| Here is where you can register all of the routes for an application.
| It is a breeze. Simply tell Lumen the URIs it should respond to
| and give it the Closure to call when that URI is requested.
|
*/

$router->get('/', function () use ($router) {
    return $router->app->version();
});

$router->group(['middleware' => 'auth'], function () use ($router) {
    $router->group(['namespace' => 'AuthControllers'], function () use ($router) {
        $router->get('/test', 'AuthController@auth');
    });

    $router->group(['namespace' => 'EditControllers'], function () use ($router) {
        $router->post('/append-file', 'EditController@appendFile');
    });

    $router->group(['namespace' => 'EditControllers'], function () use ($router) {
        $router->post('/append-link', 'EditController@appendLink');
    });

    $router->group(['namespace' => 'EditControllers'], function () use ($router) {
        $router->post('/upload', 'EditController@upload');
    });
});
