BEGIN TRY 
 Drop Procedure [dbo].[M_TenzikaiShouhinName_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[M_TenzikaiShouhinName_Select]
	-- Add the parameters for the stored procedure here
	@tenzikainame as varchar(80)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 1 from M_TenzikaiShouhin where TenzikaiName=@tenzikainame and  DeleteFlg=0  
END
GO
