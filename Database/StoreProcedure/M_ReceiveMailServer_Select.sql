
 BEGIN TRY 
 Drop Procedure dbo.[M_ReceiveMailServer_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE M_ReceiveMailServer_Select
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ReceiveAddress,
	ReceiveServer,
	Account,
	[Password],
	BackUpAddress
	From M_ReceiveMailServer
	order by ProcessingOrder
END
GO
