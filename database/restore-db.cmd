@echo off

set file_path=.\dumps
set host=127.0.0.1
set port=5432
set username=sys

pg_restore -v -c -C -h %host% -p %port% -U %username% -W -d postgres %file_path%\nwd-atomskills-2023.tar