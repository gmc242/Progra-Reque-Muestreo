CREATE SCHEMA test
GO;

USE test

CREATE TABLE usuario(
	identificador varchar(20),
	nombre varchar(40) NOT NULL,
	sal binary(32) NOT NULL,
	pass_hash binary(64) NOT NULL,
	primary key(identificador)
);

/* Cuando se inicializa el programa se debe inicializar el usuario Admin, para que use pass admin*/

CREATE TABLE proyecto(
	id_proyecto integer,
	nombre varchar(50) NOT NULL,
	tiempo_muestreo integer NOT NULL,
	tamano_muestreo integer NOT NULL,
	tiempo_entre_muestreos integer NOT NULL,
	fecha_inicio Date NOT NULL,
	fecha_fin Date NOT NULL,
	lider_id varchar(20) NOT NULL,
	descripcion varchar(1000),
	primary key(id_proyecto),
	foreign key(lider_id) references usuario
);

CREATE TABLE asistentes_por_proyecto(
	id_asistente varchar(20),
	id_proyecto integer NOT NULL,
	foreign key(id_asistente) references usuario,
	foreign key(id_proyecto) references proyecto
);

CREATE TABLE sujetos_de_prueba(
	id_sujeto integer,
	nombre varchar(40) NOT NULL,
	id_proyecto integer NOT NULL,
	primary key(id_sujeto),
	foreign key(id_proyecto) references proyecto
);

CREATE TABLE actividad(
	id_actividad integer,
	nombre varchar(20) NOT NULL,
	descripcion varchar(200),
	id_proyecto integer NOT NULL,
	primary key (id_actividad),
	foreign key (id_proyecto) references proyecto
);

CREATE TABLE tarea(
	id_tarea integer,
	nombre varchar(20) NOT NULL,
	descripcion varchar(200),
	id_actividad integer NOT NULL,
	primary key (id_tarea),
	foreign key (id_actividad) references actividad
);

CREATE TABLE observacion_de_tarea(
	id_observacion integer,
	id_sujeto integer,
	id_actividad integer NOT NULL,
	fecha_hora datetime NOT NULL,
	categoria varchar(2) NOT NULL,
	observacion varchar(200),
	primary key(id_observacion),
	foreign key(id_sujeto) references sujetos_de_prueba,
	foreign key(id_actividad) references actividad,
	constraint cate check (categoria IN ('TP','TC','TI'))
);

CREATE TABLE ronda_de_observacion(
	id_ronda integer,
	id_actividad integer NOT NULL,
	fecha_hora datetime NOT NULL,
	humedad integer NOT NULL,
	temperatura integer NOT NULL,
	descripcion varchar(200),
	primary key(id_ronda),
	foreign key(id_actividad) references actividad
);

