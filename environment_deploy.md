# Развертывание окружения

Окружение написано на kotlin. Чтобы начать работать с сервисом - вам нужны:

- java 17 + jdk
- PostgresPro 12

После скачивания и установки java+sdk, postgres.

заходим в папку `/database`
выполняем файл `before-restore.sql`

После выполнения файла, в постгресе должны появится роли: `uni_user`,`uni_admin`,`nwd-atomskills-2023`,`camunda-admin`
так-же `uni_user` будет включена в `uni_admin`,
а `uni_admin` будет назначена в `camunda-admin`,`nwd-atomskills-2023`

далее заходим в файл `restore-db.cmd` и видим нечто такое

```cmd
@echo off

set file_path=.\dumps
set host=127.0.0.1
set port=5432
set username=sys

pg_restore -v -c -C -h %host% -p %port% -U %username% -W -d postgres %file_path%\nwd-atomskills-2023.tar
```

- **host** - хост до вашей базы
- **port** - порт до вашей базы
- **username** - пользователь, под которым произведется развертывание базы (желательно `postgres` или его аналог, в моём случае был `sys`)

Если вы сделали все правильно, в кластере постгреса появится БД `nwd-atomskills-2023`

После установки БД, вам нужно зайти в папку `/environment` там будет два файла `env-app.jar` и `run-environment.cmd`

Смотрим внутрь файла `run-environment.cmd`, если запускаете не на том же устройстве, что и кластер БД, меняете настройки и запускаете его. Если запустилось - вы все сделали правильно.

Обязательно посмотрите [**документацию по взаимодействию с api**](./api_doc.md).
