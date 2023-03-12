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

После установки БД, вам нужно зайти в папку `/environment` там будет два файла `as-environment-service-1.0.0.jar` и `run-env.cmd`

Смотрим внутрь файла `run-environment.cmd`, если запускаете не на том же устройстве, что и кластер БД, меняете настройки и запускаете его. Если запустилось - вы все сделали правильно.

```cmd
@echo off
set NWC_SRV_PORT=1040
set NWC_SRV_MILLING_PORTS={'mm-1':'1041','mm-2':'1042','mm-3':'1043','mm-4':'1045','mm-5':'1047'}
set NWC_SRV_LATHE_PORTS={'lm-1':'1051','lm-2':'1052','lm-3':'1054','lm-4':'1056'}
set NWC_DB_HOST=localhost
set NWC_DB_PORT=5432
set CRM_MAX_EXECUTING_REQUESTS=7
set NWC_MNF_MIN_REPAIRING_DURATION=20
set NWC_MNF_MAX_REPAIRING_DURATION=240

java -jar as-environment-service-1.0.0.jar
```

- **NWC_SRV_PORT** - Основной порт приложения
- **NWC_SRV_MILLING_PORTS** - Имена и порты фрезерных станков
- **NWC_SRV_LATHE_PORTS** - Имена и порты токарных станков
- **NWC_DB_HOST** - Хост СУБД
- **NWC_DB_PORT** - Порт СУБД
- **CRM_MAX_EXECUTING_REQUESTS** - Максимальное количество формирующихся и исполняющихся заявок
- **NWC_MNF_MIN_REPAIRING_DURATION** - Минимальное время в секундах для починки станка
- **NWC_MNF_MAX_REPAIRING_DURATION** - Максимальное время в секундах для починки станка

Обязательно посмотрите [**документацию по взаимодействию с api**](./api_doc.md).
