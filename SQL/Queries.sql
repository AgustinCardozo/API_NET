USE [GD2015C1]
GO
/****** Object:  StoredProcedure [dbo].[SYS_GetClientes]    Script Date: 23/2/2023 00:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC dbo.SYS_GetClientes '0000024324'
--EXEC dbo.SYS_GetClientes null
CREATE OR ALTER PROCEDURE [dbo].[SYS_GetClientes]
@id_cliente CHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	BEGIN
		IF(@id_cliente IS NULL)
			SELECT ltrim(rtrim(clie_codigo)) 'clie_codigo', ltrim(rtrim(clie_razon_social)) 'clie_razon_social', 
				ltrim(rtrim(clie_telefono)) 'clie_telefono', ltrim(rtrim(clie_domicilio)) 'clie_domicilio', clie_limite_credito, clie_vendedor
			FROM dbo.Cliente c
		ELSE 
			SELECT ltrim(rtrim(clie_codigo)) 'clie_codigo', ltrim(rtrim(clie_razon_social)) 'clie_razon_social', 
				ltrim(rtrim(clie_telefono)) 'clie_telefono', ltrim(rtrim(clie_domicilio)) 'clie_domicilio', clie_limite_credito, clie_vendedor			
			FROM dbo.Cliente c WHERE c.clie_codigo = @id_cliente
	END
	
END

GO
/****** Object:  StoredProcedure [dbo].[SYS_CreateCliente]    Script Date: 27/4/2022 12:56:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC dbo.SYS_CreateCliente '000008', 'LALA SA', '48484848', 'Darwin 706', 6000.0, 3
CREATE OR ALTER PROCEDURE [dbo].[SYS_CreateCliente]
@id_cliente CHAR(6), @razon_social CHAR(100), @telefono CHAR(100), @domicilio CHAR(100), @limite DECIMAL(12,2), @id_vendedor NUMERIC(6,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @idCliente INT = 0;

	BEGIN
		IF NOT EXISTS (SELECT c.clie_codigo FROM dbo.Cliente c WHERE c.clie_codigo = @id_cliente)
			BEGIN
				INSERT INTO dbo.Cliente
					(clie_codigo, clie_razon_social, clie_telefono, clie_domicilio, clie_limite_credito, clie_vendedor)
				VALUES (@id_cliente, @razon_social, @telefono, @domicilio, @limite, @id_vendedor)

				SET @idCliente = SCOPE_IDENTITY()
				SELECT @id_cliente;
			END
		ELSE
			SELECT 'ERROR'
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SYS_DeleteCliente]    Script Date: 27/4/2022 14:19:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SYS_DeleteCliente]
@id_cliente CHAR(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	BEGIN
		IF EXISTS (SELECT c.clie_codigo FROM dbo.Cliente c WHERE c.clie_codigo = @id_cliente)
			BEGIN
				DELETE FROM dbo.Cliente WHERE clie_codigo = @id_cliente
				SELECT 'OK'
			END
		ELSE 
			SELECT 'ERROR'
	END
	
END

GO
/****** Object:  StoredProcedure [dbo].[SYS_UpdateCliente]    Script Date: 27/4/2022 18:06:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC dbo.SYS_UpdateCliente '000004', 'LALA SA', '48484848', 'Darwin 706', 5000, 1
CREATE OR ALTER PROCEDURE [dbo].[SYS_UpdateCliente]
@id_cliente CHAR(6), @razon_social CHAR(100) = NULL, @telefono CHAR(100) = NULL, @domicilio CHAR(100) = NULL, @limite NUMERIC(6,0) = NULL, @id_vendedor NUMERIC(6,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
		IF EXISTS (SELECT c.clie_codigo FROM dbo.Cliente c WHERE c.clie_codigo = @id_cliente)
			BEGIN
				UPDATE dbo.Cliente
				SET clie_razon_social = @razon_social,
				clie_telefono = @telefono,
				clie_domicilio = @domicilio,
				clie_limite_credito = @limite,
				clie_vendedor = @id_vendedor
				WHERE clie_codigo = @id_cliente
				
				SELECT 'OK'
			END
		ELSE
			SELECT 'ERROR'
	END
END

GO

SELECT * FROM dbo.Cliente WHERE clie_codigo LIKE '00000%'

GO 

IF OBJECT_ID('GD2015C1.dbo.Usuarios') IS NOT NULL
	DROP TABLE dbo.Usuarios

CREATE TABLE GD2015C1.dbo.Usuarios(
	[id] [int] IDENTITY(1,1) NOT NULL,    
	[usuario] [nvarchar](100) NOT NULL,
	[password] [nvarchar](250) NOT NULL,
	[mail] [nvarchar](250) NOT NULL,
	[nombre] [nvarchar](100) NOT NULL,
	[rol][nvarchar](50) NULL,
	[createdAt][datetime] NOT NULL, 
	[updatedAt][datetime] NULL, 
	[deletedAt][datetime] NULL,
	PRIMARY KEY(id)
);


GO

select * from GD2015C1.dbo.Usuarios

GO

truncate table GD2015C1.dbo.Usuarios

GO