 BEGIN TRY 
 Drop Procedure dbo.[D_APIRireki_YahooCount_Insert]
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
CREATE PROCEDURE [dbo].[D_APIRireki_YahooCount_Insert]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(50),
	@APIKey as tinyint , 
	@Status as tinyint,
	@Count_NewOrder int,
	@Count_NewReserve int,
	@Count_WaitPayment int,
	@Count_WaitShipping int,
	@Count_Shipping int,
	@Count_Reserve int,
	@Count_Holding int,
	@Count_WaitDone int,
	@Count_Suspect int,
	@Count_SettleError int,
	@Count_Refund int,
	@Count_AutoDone int,
	@Count_AutoWorking int,
	@Count_Release int,
	@Count_NoPayNumber int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	Declare @val as int,
	@val2 as int,
	@DateTime as Datetime;
	set @val = (select Max(IsNull(InportSEQ,0))+1 from D_APIRireki);
	set @val2= (select Max(IsNull(InportSEQ,0))+1 from D_yahooCount);
	if (@val is null)
	Begin
	set @val=1;
	End
		if (@val2 is null)
	Begin
	set @val2=1;
	End
	set @DateTime = getdate();


	----insert into D_APIRireki(InportSEQ,StoreCD,APIKey,InsertOperator, InsertDateTime) values(@val,@StoreCD, @APIKey,Null,@DateTime)
	--select  *from  D_yahooCOunt
	insert into D_YahooCount (

	InportSEQ,
	StoreCD,
	APIKey,
	OperateDate,
	OperateTime
	,
	[Status],
	Count_NewOrder,						
    Count_NewReserve,						
    Count_WaitPayment,						
    Count_WaitShipping	,					
    Count_Shipping,						
    Count_Reserve	,					
    Count_Holding,						
    Count_WaitDone	,					
    Count_Suspect	,					
    Count_SettleError	,					
    Count_Refund		,				
    Count_AutoDone		,				
    Count_AutoWorking	,					
    Count_Release	,					
    Count_NoPayNumber,						
    InsertOperator		,				
    InsertDateTime		,				
    UpdateOperator	,					
    UpdateDateTime						

	)
	values (
	 @val2,
	 @StoreCD,
	 @APIKey,
	 (SELECT convert(varchar, @DateTime, 111)),
	 (SELECT convert(varchar, @DateTime, 108))
	 ,
	 @Status,
     @Count_NewOrder,						
     @Count_NewReserve,						
     @Count_WaitPayment,						
     @Count_WaitShipping,						
     @Count_Shipping	,					
     @Count_Reserve	,					
     @Count_Holding	,					
     @Count_WaitDone,						
     @Count_Suspect		,				
     @Count_SettleError	,					
     @Count_Refund		,				
     @Count_AutoDone	,					
     @Count_AutoWorking,						
     @Count_Release	,				
     @Count_NoPayNumber	,					
     Null,					
     @DateTime	,					
     Null	,					
     @DateTime
	)
	--SELECT convert(varchar, getdate(), 108) 
	--(SELECT convert(varchar, @DateTime, 111)
	--select * from D_APIRireki
	SET IDENTITY_INSERT D_APIRireki ON

	insert into D_APIRireki(InportSEQ,StoreCD,APIKey,InsertOperator, InsertDateTime) values(@val,@StoreCD, @APIKey,Null,@DateTime)
		SET IDENTITY_INSERT D_APIRireki off

END

