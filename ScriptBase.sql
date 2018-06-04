	CREATE TABLE ronda_de_observacion(
	id_ronda integer identity(1,1),
	id_observacion integer NOT NULL,
	hora time NOT NULL,
	humedad float NOT NULL,
	temperatura float NOT NULL,
	descripcion varchar(200),
	primary key(id_ronda),
	foreign key(id_observacion) references observacion
	);
	
	create proc getOperaciones
	as
	begin 
	select nombre from actividad
	end

	create proc getTareasXoperacion 
	@fecha date,
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', s.nombre as 'Colaborador', r.hora as 'Hora', r.humedad as 'Humedad', r.temperatura as 'Temperatura'
	from tarea t, sujetos_de_prueba s
	inner join observacion o
	on o.id_actividad = @idOperacion and o.dia = @fecha
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and ot.id_sujeto = s.id_sujeto
	end

	create proc getFechas 
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select o.dia
	from observacion o
	inner join actividad a
	on o.id_actividad = @idOperacion
	group by o.dia
	end

	create proc getObservacionesXdiaXoperacion
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion

	select o.dia as 'Dia', count(t.id_tarea) as 'Numero de Observaciones'
	from tarea t
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	group by o.dia
	end

	create proc getTareaXoperacion2
	@fecha date,
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', count(*) as CantObs
	from tarea t, sujetos_de_prueba s
	inner join observacion o
	on o.id_actividad = @idOperacion and o.dia = @fecha
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and ot.id_sujeto = s.id_sujeto
	Group by t.categoria,t.nombre
	having COUNT(*)>0
	end

	create proc getTareaXoperacion3
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', count(*) as CantObs
	from tarea t, sujetos_de_prueba s
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and ot.id_sujeto = s.id_sujeto
	Group by t.categoria,t.nombre
	having COUNT(*)>0
	end

	create proc getTareaXoperacionTP
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', count(*) as CantObs
	from tarea t
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and t.categoria = 'TP'
	Group by t.categoria,t.nombre
	having COUNT(*)>0
	end

	create proc getTareaXoperacionTI
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', count(*) as CantObs
	from tarea t
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and t.categoria = 'TI'
	Group by t.categoria,t.nombre
	having COUNT(*)>0
	end

	create proc getTareaXoperacionTC
	@nombreOperacion varchar(20)
	as 
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion	
	select t.categoria as 'Categoria', t.nombre as 'Nombre', count(*) as CantObs
	from tarea t
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and t.categoria = 'TC'
	Group by t.categoria,t.nombre
	having COUNT(*)>0
	end

	create proc productivasXoperacion
	@fecha date,
	@nombreOperacion varchar(20)
	as
	begin
	declare @idOperacion int
	select @idOperacion = id_actividad from actividad a where a.nombre = @nombreOperacion
	select o.dia as 'Dia',count(t.id_tarea) as 'Productivas'
	from tarea t
	inner join observacion o
	on o.id_actividad = @idOperacion
	inner join ronda_de_observacion r
	on r.id_observacion = o.id_observacion
	inner join observacion_de_tarea ot
	on ot.id_ronda = r.id_ronda
	where ot.id_tarea = t.id_tarea
	and t.categoria = 'TP'
	group by o.dia
	end

	insert into usuario values ('1','Bryan',1,1,1)
	select* from usuario

	insert into proyecto values('proyEjem',2,30,4,GETDATE(),GETDATE(),'1','ProyectoEjamplo','MP')
	select* from proyecto
	

	insert into actividad values ('Encofrado', 'Desc11',1)
	insert into actividad values ('Operacion2', 'Desc22',1)
	select* from actividad
	

	insert into sujetos_de_prueba values('SujetoEj1',1)
	insert into sujetos_de_prueba values('SujetoEj2',1)
	insert into sujetos_de_prueba values('SujetoEj3',1)
	insert into sujetos_de_prueba values('SujetoEj4',1)
	

	insert into tarea values('TareaEjemplo1','dt1','TP')
	insert into tarea values('TareaEjemplo2','dt2','TP')
	insert into tarea values('TareaEjemplo3','dt3','TP')
	insert into tarea values('TareaEjemplo4','dt4','TP')
	insert into tarea values('TareaEjemplo5','dt5','TI')
	insert into tarea values('TareaEjemplo6','dt6','TI')
	insert into tarea values('TareaEjemplo7','dt7','TI')
	insert into tarea values('TareaEjemplo8','dt8','TI')
	insert into tarea values('TareaEjemplo9','dt9','TC')
	insert into tarea values('TareaEjemplo10','dt10','TC')
	insert into tarea values('TareaEjemplo11','dt11','TC')
	insert into tarea values('TareaEjemplo12','dt12','TC')
	

	insert into observacion values(1,GETDATE(),'O1A1')
	insert into observacion values(1,'2018-06-03','O1A1')
	insert into observacion values(1,'2018-06-04','O1A1')
	insert into observacion values(2,GETDATE(),'O1A2')
	insert into observacion values(2,'2018-06-03','O2A2')
	insert into observacion values(2,'2018-06-04','O3A2')
	

	insert into ronda_de_observacion values(1,GETDATE(),20,11,'r1o1')
	insert into ronda_de_observacion values(1,GETDATE(),21,12,'r2o1')
	insert into ronda_de_observacion values(1,GETDATE(),22,13,'r3o1')
	insert into ronda_de_observacion values(2,GETDATE(),23,14,'r1o2')
	insert into ronda_de_observacion values(2,GETDATE(),24,15,'r2o2')
	insert into ronda_de_observacion values(2,GETDATE(),25,16,'r3o2')
	insert into ronda_de_observacion values(3,GETDATE(),26,17,'r1o3')
	insert into ronda_de_observacion values(3,GETDATE(),27,18,'r2o3')
	insert into ronda_de_observacion values(3,GETDATE(),28,19,'r3o3')
	insert into ronda_de_observacion values(4,GETDATE(),29,11,'r1o4')
	insert into ronda_de_observacion values(4,GETDATE(),21,12,'r2o4')
	insert into ronda_de_observacion values(4,GETDATE(),22,13,'r3o4')
	insert into ronda_de_observacion values(5,GETDATE(),23,14,'r1o5')
	insert into ronda_de_observacion values(5,GETDATE(),24,15,'r2o5')
	insert into ronda_de_observacion values(5,GETDATE(),25,16,'r3o5')
	insert into ronda_de_observacion values(6,GETDATE(),26,17,'r1o6')
	insert into ronda_de_observacion values(6,GETDATE(),27,18,'r2o6')
	insert into ronda_de_observacion values(6,GETDATE(),28,19,'r3o6')
	

	insert into observacion_de_tarea values(1,1,1)
	insert into observacion_de_tarea values(1,2,2)
	insert into observacion_de_tarea values(2,3,5)
	insert into observacion_de_tarea values(2,4,6)
	insert into observacion_de_tarea values(3,1,9)
	insert into observacion_de_tarea values(3,2,10)
	insert into observacion_de_tarea values(4,3,1)
	insert into observacion_de_tarea values(4,4,2)
	insert into observacion_de_tarea values(5,1,5)
	insert into observacion_de_tarea values(5,2,6)
	insert into observacion_de_tarea values(6,3,9)
	insert into observacion_de_tarea values(6,4,10)
	insert into observacion_de_tarea values(7,1,1)
	insert into observacion_de_tarea values(7,2,2)
	insert into observacion_de_tarea values(8,3,5)
	insert into observacion_de_tarea values(8,4,6)
	insert into observacion_de_tarea values(9,1,9)
	insert into observacion_de_tarea values(9,2,10)
	insert into observacion_de_tarea values(10,3,3)
	insert into observacion_de_tarea values(10,4,4)
	insert into observacion_de_tarea values(11,1,7)
	insert into observacion_de_tarea values(11,2,8)
	insert into observacion_de_tarea values(12,3,11)
	insert into observacion_de_tarea values(12,4,12)
	insert into observacion_de_tarea values(13,1,3)
	insert into observacion_de_tarea values(13,2,4)
	insert into observacion_de_tarea values(14,3,7)
	insert into observacion_de_tarea values(14,4,8)
	insert into observacion_de_tarea values(15,1,11)
	insert into observacion_de_tarea values(15,2,12)
	insert into observacion_de_tarea values(16,3,3)
	insert into observacion_de_tarea values(16,4,4)
	insert into observacion_de_tarea values(17,1,7)
	insert into observacion_de_tarea values(17,2,8)
	insert into observacion_de_tarea values(18,3,11)
	insert into observacion_de_tarea values(18,4,12)
