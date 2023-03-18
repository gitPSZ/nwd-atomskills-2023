-- DROP SCHEMA atom;

CREATE SCHEMA atom AUTHORIZATION sys;


-- Drop table

-- DROP TABLE atom.contractor;

CREATE TABLE atom.contractor (
	id int4 NOT NULL,
	inn varchar NULL,
	caption varchar NULL,
	CONSTRAINT contractor_pk PRIMARY KEY (id)
);

-- Drop table

-- DROP TABLE atom.machine_state;

CREATE TABLE atom.machine_state (
	id int4 NOT NULL,
	code varchar NULL,
	caption varchar NULL,
	CONSTRAINT machine_status_pk PRIMARY KEY (id)
);

-- Drop table

-- DROP TABLE atom.navigation_buttons;

CREATE TABLE atom.navigation_buttons (
	id int4 NOT NULL,
	routerlink varchar NULL,
	iconclass varchar NULL,
	caption varchar NULL,
	role_id int4 NULL,
	CONSTRAINT navigation_buttons_pk PRIMARY KEY (id)
);

-- Drop table

-- DROP TABLE atom.person;

CREATE TABLE atom.person (
	id serial NOT NULL ,
	nameclient varchar NULL,
	login varchar NULL,
	"password" varchar NULL,
	email varchar NULL,
	role_id int4 NULL,
	salt varchar NULL
);

-- Drop table

-- DROP TABLE atom.product;

CREATE TABLE atom.product (
	id int4 NOT NULL,
	code varchar NULL,
	caption varchar NULL,
	milling_time int4 NULL,
	lathe_time int4 NULL,
	CONSTRAINT product_pk PRIMARY KEY (id)
);

-- Drop table

-- DROP TABLE atom."role";

CREATE TABLE atom."role" (
	id int4 NULL,
	"RoleName" varchar NULL,
	rolecaption varchar NULL
);

-- Drop table

-- DROP TABLE atom.machine;

CREATE TABLE atom.machine (
	id varchar NOT NULL,
	machine_type varchar NULL,
	port int4 NULL,
	is_deleted varchar NULL DEFAULT 1,
	id_state int4 NULL,
	machine_type_caption varchar NULL,
	CONSTRAINT machines_pk PRIMARY KEY (id),
	CONSTRAINT machine_fk FOREIGN KEY (id_state) REFERENCES atom.machine_state(id)
);

-- Drop table

-- DROP TABLE atom.request;

CREATE TABLE atom.request (
	id int4 NOT NULL,
	"number" varchar NULL,
	create_date timestamp NULL,
	release_date timestamp NULL,
	description varchar NULL,
	id_contractor int4 NULL,
	state_code varchar NULL,
	state_caption varchar NULL,
	priority int4 NULL DEFAULT 3,
	CONSTRAINT request_pk PRIMARY KEY (id),
	CONSTRAINT request_fk FOREIGN KEY (id_contractor) REFERENCES atom.contractor(id)
);

-- Drop table

-- DROP TABLE atom.request_position;

CREATE TABLE atom.request_position (
	id int4 NOT NULL,
	product_id int4 NULL,
	request_id int4 NULL,
	quantity int4 NULL,
	quantity_exec int4 NULL,
	CONSTRAINT request_position_pk PRIMARY KEY (id),
	CONSTRAINT request_position_fk FOREIGN KEY (request_id) REFERENCES atom.request(id),
	CONSTRAINT request_position_fk_1 FOREIGN KEY (product_id) REFERENCES atom.product(id)
);

-- Drop table

-- DROP TABLE atom.machine_request;

CREATE TABLE atom.machine_request (
	id_machine varchar NOT NULL,
	id_request int4 NOT NULL,
	id int4 NOT NULL,
	CONSTRAINT machine_request_pk PRIMARY KEY (id),
	CONSTRAINT newtable_fk FOREIGN KEY (id_machine) REFERENCES atom.machine(id),
	CONSTRAINT newtable_fk_1 FOREIGN KEY (id_request) REFERENCES atom.request(id)
);
