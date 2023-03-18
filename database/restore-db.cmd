@echo off

set file_path=.\dumps
set host=localhost
set port=5434
set username=sys2

pg_restore -v -c -C -h %host% -p %port% -U %username% -W -d postgres %file_path%\nwd-atomskills-2023.tar