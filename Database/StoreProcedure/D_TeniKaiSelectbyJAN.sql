use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.D_TeniKaiSelectbyJAN
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[D_TeniKaiSelectbyJAN]
	-- Add the parameters for the stored procedure here
	@TenjiCD as  varchar(11)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from D_TenzikaiJuchuu where TenzikaiJuchuuNO =@TenjiCD
END
