 BEGIN TRY 
 Drop Procedure dbo.[M_SoukoWarehouse_Select]
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
CREATE PROCEDURE [dbo].[M_SoukoWarehouse_Select]
	-- Add the parameters for the stored procedure here
	@DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * From M_Souko 
	Where DeleteFlg = @DeleteFlg
	Order by SoukoCD
END

