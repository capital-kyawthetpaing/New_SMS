 BEGIN TRY 
 Drop Procedure dbo.[D_Picking_SelectToCheckPickingKBN]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE D_Picking_SelectToCheckPickingKBN
	-- Add the parameters for the stored procedure here
	@Code varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select PickingNO,PickingKBN from D_Picking where PickingNO=@Code and DeleteDateTime is null
END
GO
