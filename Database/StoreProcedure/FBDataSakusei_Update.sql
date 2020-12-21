 BEGIN TRY 
 Drop Procedure dbo.[FBDataSakusei_Update]
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
CREATE PROCEDURE [dbo].[FBDataSakusei_Update]
	-- Add the parameters for the stored procedure here
	@Operator as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @DateTime datetime=getdate()

	Update D_FBControl  ---D_FBControlUpdate
	Set 		
	DeleteOperator = @Operator, 			
	DeleteDateTime = @DateTime


	Update D_FBData ---D_FBDataUpdate
	Set 		
	DeleteOperator = @Operator,			
	DeleteDateTime = @DateTime

	Update D_Pay  ---D_PayUpdate
	Set 
	FBCreateDate = Null,					
	UpdateOperator =@Operator,				
	UpdateDateTime = @DateTime	
	
	

	
END