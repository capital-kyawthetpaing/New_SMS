use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.D_TeniSelectbyTaniCD
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[D_TeniSelectbyTaniCD]
	-- Add the parameters for the stored procedure here
	@TeniCD as  varchar(11)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Char1, [Key] From M_MultiPorpose Where M_MultiPorpose.ID='201' And	M_MultiPorpose.[Key]= @TeniCD
END