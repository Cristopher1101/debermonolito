/* Monolito4am — columna OTP compatible con cifrado (más de 10 caracteres).
   Ejecute UNA de las opciones según cómo tenga la base ahora.

   OPCIÓN A (recomendada si la columna sigue siendo VARCHAR como en el script original):
   Amplía la columna para guardar el cifrado en hexadecimal (hasta 512 caracteres).
*/
USE [Monolito4am];
GO
IF EXISTS (SELECT 1 FROM sys.columns c
           INNER JOIN sys.tables t ON t.object_id = c.object_id
           WHERE t.name = 'tbl_usuario' AND c.name = 'usu_codigo_OTP'
             AND TYPE_NAME(c.user_type_id) IN ('varchar', 'nvarchar', 'char'))
BEGIN
    ALTER TABLE dbo.tbl_usuario ALTER COLUMN usu_codigo_OTP VARCHAR(512) NULL;
END
GO

/*
   OPCIÓN B (solo si ya convirtió la columna a VARBINARY y el modelo LINQ usa Binary):
   ALTER TABLE dbo.tbl_usuario ALTER COLUMN usu_codigo_OTP VARBINARY(MAX) NULL;
*/
