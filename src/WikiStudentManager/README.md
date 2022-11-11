# Запуск приложения

Для запуска приложения необходимо иметь `docker`, `docker-compose`

Для работы приложения перед запуском необходимо создать файл `data_wiki.php` в папке `config`

Файл `data_wiki.php`:
```
<?php

return [
    'username' => '<Login>',
    'password' => '<Password>',
    'lgname' => '<Login>',
    'lgpassword' => '<LGpassword>',
];
```

`<Login>` - имя учетной записи пользователя на CSC Wiki

`<Password>` - пароль от учетной записи пользователя на CSC Wiki

`<LGpassword>` - пароль бота данного пользователя на CSC Wiki

Для запуска приложения необходимо в директории, где находится `docker-compose.yaml`, в консоли ввести команду:
```
docker-compose up
```
