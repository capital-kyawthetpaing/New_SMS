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
	@Operator as varchar(10),
	@MotoKouzaCD as varchar(3),
	@PayDate as  varchar(10),
	@Flg as tinyint
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
	From D_FBControl dfb
	INNER JOIN  D_Pay dp ON dp.FBCreateNO= dfb.FBPayNO 
	WHERE dp.DeleteDateTime is null 
	And	dp.MotoKouzaCD = @MotoKouzaCD					
	And	dp.PayDate	= @PayDate						
	And	dp.TransferGaku	> 0					
	And	dp.FBCreateDate	is not null 						
	And	dfb.DeleteDateTime	is  null						


	Update D_FBData ---D_FBDataUpdate
	Set 		
	DeleteOperator = @Operator,			
	DeleteDateTime = @DateTime
	from D_FBData fbdata
	INNER JOIN  D_Pay dp ON dp.FBCreateNO= fbdata.FBPayNO
	WHERE dp.DeleteDateTime is null 
	And	dp.MotoKouzaCD = @MotoKouzaCD					
	And	dp.PayDate	= @PayDate						
	And	dp.TransferGaku	> 0					
	And	dp.FBCreateDate	is not null 						
	And	fbdata.DeleteDateTime is  null

	Update D_Pay  ---D_PayUpdate
	Set 
	FBCreateNO = Null,
	FBCreateDate = Null,					
	UpdateOperator =@Operator,				
	UpdateDateTime = @DateTime	
	WHERE DeleteDateTime is null 
	And	MotoKouzaCD = @MotoKouzaCD					
	And	PayDate	= @PayDate						
	And	TransferGaku	> 0					
	and ((@Flg = 0 and FBCreateDate is null) 
	or ((@Flg = 1  or @Flg = 2 )and FBCreateDate is not null))	
	
	

	
END