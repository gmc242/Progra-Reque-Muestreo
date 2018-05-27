CREATE DATABASE Monitoreo
GO

USE Monitoreo

CREATE TABLE usuario(
	identificador varchar(20),
	nombre varchar(40) NOT NULL,
	sal binary(32) NOT NULL,
	pass_hash binary(32) NOT NULL,
	primary key(identificador)
);

/* En algun momento se debe inicializar el usuario Admin, para que use pass admin*/

CREATE TABLE proyecto(
	id_proyecto integer identity(1,1),
	nombre varchar(50) NOT NULL,
	tiempo_muestreo integer NOT NULL,
	tamano_muestreo integer NOT NULL,
	tiempo_entre_muestreos integer NOT NULL,
	fecha_inicio Date NOT NULL,
	fecha_fin Date NOT NULL,
	lider_id varchar(20) NOT NULL,
	descripcion varchar(1000),
	tipo_muestreo varchar(2),
	primary key(id_proyecto),
	foreign key(lider_id) references usuario,
	constraint tipo check (tipo_muestreo IN ('MP','MD', 'MF'))
);

CREATE TABLE asistentes_por_proyecto(
	id_asistente varchar(20),
	id_proyecto integer NOT NULL,
	foreign key(id_asistente) references usuario,
	foreign key(id_proyecto) references proyecto
);

CREATE TABLE sujetos_de_prueba(
	id_sujeto integer identity(1,1),
	nombre varchar(40) NOT NULL,
	id_proyecto integer NOT NULL,
	primary key(id_sujeto),
	foreign key(id_proyecto) references proyecto
);

CREATE TABLE actividad(
	id_actividad integer identity(1,1),
	nombre varchar(20) NOT NULL,
	descripcion varchar(200),
	id_proyecto integer NOT NULL,
	primary key (id_actividad),
	foreign key (id_proyecto) references proyecto
);

CREATE TABLE tarea(
	id_tarea integer identity(1,1),
	nombre varchar(20) NOT NULL,
	descripcion varchar(200),
	id_actividad integer NOT NULL,
	categoria varchar(2) NOT NULL,
	primary key (id_tarea),
	foreign key (id_actividad) references actividad,
	constraint cate check (categoria IN ('TP','TC','TI'))
);

CREATE TABLE observacion(
	id_observacion integer identity(1,1),
	id_actividad integer NOT NULL,
	dia date NOT NULL,
	descripcion Varchar(200),
	primary key(id_observacion),
	foreign key(id_actividad) references actividad
);

CREATE TABLE ronda_de_observacion(
	id_ronda integer identity(1,1),
	id_observacion integer NOT NULL,
	fecha_hora datetime NOT NULL,
	humedad float NOT NULL,
	temperatura float NOT NULL,
	descripcion varchar(200),
	primary key(id_ronda),
	foreign key(id_observacion) references observacion
);

CREATE TABLE observacion_de_tarea(
	id_observacion_tarea integer identity(1,1),
	id_ronda integer NOT NULL,
	id_sujeto integer NOT NULL,
	id_tarea integer NOT NULL,
	primary key(id_observacion_tarea),
	foreign key(id_sujeto) references sujetos_de_prueba,
	foreign key(id_tarea) references tarea,
	foreign key(id_ronda) references ronda_de_observacion
);

CREATE TABLE asistentes_por_actividad(
	id_asistente varchar(20) NOT NULL,
	id_actividad int NOT NULL,
	foreign key(id_asistente) references usuario,
	foreign key(id_actividad) references actividad
);

CREATE TABLE usuarios_por_actividad(
	id_actividad int NOT NULL,
	id_usuario varchar(20) NOT NULL,
	foreign key(id_actividad) references actividad,
	foreign key(id_usuario) references usuario
);

CREATE TABLE sujetos_por_actividad(
	id_actividad int NOT NULL,
	id_sujeto int NOT NULL,
	foreign key (id_actividad) references actividad,
	foreign key (id_sujeto) references sujetos_de_prueba
)



