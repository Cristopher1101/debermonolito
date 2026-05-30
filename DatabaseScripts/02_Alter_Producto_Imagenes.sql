USE [Monolito4am]
GO

-- 1. Crear la tabla para las imágenes de los productos
CREATE TABLE [dbo].[tbl_producto_imagen](
	[pimg_id] [int] IDENTITY(1,1) PRIMARY KEY,
	[pro_id] [int] NOT NULL,
	[pimg_path] [varchar](255) NOT NULL,
	[pimg_es_principal] [bit] DEFAULT 0,
    FOREIGN KEY ([pro_id]) REFERENCES [dbo].[tbl_producto] ([pro_id]) ON DELETE CASCADE
)
GO

-- 2. Hacer que la columna prov_id en la tabla de productos acepte Nulos (NULL)
-- (Si esta línea te da un error de "constraint" o "llave foránea", ve a tu SQL Server, 
--  despliega la tabla tbl_producto -> Keys, elimina la llave foránea, corre esta línea y vuelve a crear la relación).
ALTER TABLE [dbo].[tbl_producto] ALTER COLUMN [prov_id] [int] NULL
GO
