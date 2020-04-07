 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_FeeSelect]
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
CREATE PROCEDURE [dbo].[M_Kouza_FeeSelect]
	-- Add the parameters for the stored procedure here
	@KouzaCD varchar(3),
	@BankCD varchar(4),
	@BranchCD varchar(3),
	@Amount money

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Isnull(dbo.F_GetKouzaFee(@BankCD,@BranchCD,Isnull(@Amount,0),@KouzaCD),0) as 'Fee'
	
	

	
END

