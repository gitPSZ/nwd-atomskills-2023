create role uni_user inherit nologin;
create role uni_admin inherit nologin;
grant uni_user to uni_admin;

create role "camunda-admin" with login;
grant uni_admin TO "camunda-admin";
alter role "camunda-admin" with password 'camunda-admin';

create role "nwd-atomskills-2023" with login;
grant uni_admin TO "nwd-atomskills-2023";
alter role "nwd-atomskills-2023" with password 'nwd-atomskills-2023';