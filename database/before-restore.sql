create role uni_user inherit nologin;

create role uni_admin inherit nologin;

grant uni_user to uni_admin;