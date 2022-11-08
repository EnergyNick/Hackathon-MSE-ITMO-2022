<?php

return [
    'url' => 'https://wiki.compscicenter.ru/api.php',
    'token_auth' => '?action=query&meta=tokens&type=login&format=json',
    'token_edit' => '?action=query&meta=tokens&format=json',
    'auth_user' => 'https://wiki.compscicenter.ru/api.php?action=clientlogin&format=json',
    'auth_bot' => 'https://wiki.compscicenter.ru/api.php?action=login&format=json',
    'loginreturnurl' => 'https://wiki.compscicenter.ru/'
];
