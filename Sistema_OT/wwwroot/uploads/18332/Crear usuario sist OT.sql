--select top 10 * from Ordenes_Trabajo order by FechaSolicitud desc
--select * from Usuarios order by Usuario


BEGIN TRAN 
insert into Usuarios 
(Usuario,
Descripcion,
FechaAlta,
UserID,
CambiarContrasenia,
FechaUltimoIngreso,
FechaCaducidadContrasenia,
DadoDeBaja,
Contrasenia,	
Administrador,
Nivel,
GrupoUsuario,
Apellidos,
Nombres,
NuncaCaducaContrasenia,
UserIDAutorizado
)
values ( 50,
'RGRAFF',
getdate(),
'RGRAFF',
'N',
NULL,
NULL,
'N',
'Desarrollo',
'S',
1,
'OPERADOR',
'Graff',
'Rocio Gisel',
'S',
NULL
)
--COMMIT
ROLLBACK