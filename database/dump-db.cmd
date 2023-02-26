@echo off
set file_path=.\dumps
set host="localhost"
set port=5432
set username="sys"
set db_name="nwd-atomskills-2023"

for /r %file_path%  %%I in (nwd-atomskills-2023.tar) do set file_time=%%~tI

if exist %file_path%\nwd-atomskills-2023.tar rename %file_path%\nwd-atomskills-2023.tar %file_time:~6,4%%file_time:~3,2%%file_time:~0,2%-%file_time:~11,2%%file_time:~14,2%.tar

set file_name=%file_path%\nwd-atomskills-2023.tar

pg_dump -v -F t -f %file_name% -O -h %host% -p %port% -d %db_name% -U %username%