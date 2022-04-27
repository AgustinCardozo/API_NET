USE [GD2015C1]
GO
/****** Object:  StoredProcedure [dbo].[GetClientes]    Script Date: 26/4/2022 12:49:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cardozo Agustin
-- Create date: 25/04/2022
-- Description:	Muestras los clientes del sistema
-- =============================================
CREATE PROCEDURE [dbo].[GetClientes]
@id_cliente CHAR(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	BEGIN
		IF(@id_cliente IS NULL)
			SELECT c.clie_codigo, c.clie_razon_social, c.clie_domicilio, c.clie_limite_credito 
			FROM GD2015C1.dbo.Cliente c
		ELSE 
			SELECT c.clie_codigo, c.clie_razon_social, c.clie_domicilio, c.clie_limite_credito 
			FROM GD2015C1.dbo.Cliente c WHERE c.clie_codigo = @id_cliente
	END
	
END