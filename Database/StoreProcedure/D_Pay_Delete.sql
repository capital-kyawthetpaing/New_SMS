 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Delete]
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
CREATE PROCEDURE [dbo].[D_Pay_Delete]
	-- Add the parameters for the stored procedure here
	@PayNO varchar(11),
	@Operator varchar(10),
	@LargePayNO varchar(11),
	@Program as varchar(20),
	@PC as varchar(10),
	@OperateMode as Varchar(10)
	--@KeyItem as varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @Datetime datetime=getdate()


--テーブル転送仕様Ａ②
	Update D_Pay
	set UpdateOperator= @Operator,
	UpdateDateTime=@Datetime,
	DeleteOperator=@Operator,
	DeleteDateTime=@Datetime
	where PayNO=@PayNO


--テーブル転送仕様Ｂ②
	Update D_PayDetails
	set UpdateOperator=@Operator,
	UpdateDateTime=@Datetime,
	DeleteOperator=@Operator,
	DeleteDateTime=@Datetime
	--from D_PayDetails as dpd 
	--inner join D_Pay as dp on dpd.PayNO=dp.PayNO and dpd.DeleteDateTime is null
	where PayNO=@PayNO

---sheetC,D
	exec dbo.L_PayHistory_Insert @PayNO,@LargePayNO

----SheetE

	Update D_PayPlan
	set UpdateOperator = @Operator,
	UpdateDateTime = @Datetime
    from D_PayPlan as dpp inner join D_PayDetails dpd
					on dpd.PayPlanNO =  dpp.PayPlanNO
	Where dpd.PayNO = @PayNO
		
----SheetF
	Update D_PayCloseHistory
	Set ProcessingKBN = 1,
	UpdateOperator = @Operator,
	UpdateDateTime = @Datetime
	From D_PayCloseHistory dpch inner join D_Pay dp 
					on dp.PayCloseNO = dpch.PayCloseNO
	Where dp.PayNO = @PayNO


----SheetY
		  IF NOT EXISTS ( Select * from D_Exclusive Where DataKBN = '9' and Number = @PayNO )
		      BEGIN
		      	insert into D_Exclusive(
		      	DataKBN,
		      	Number,
		      	OperateDataTime,
		      	Operator,
		      	Program,
		      	PC)
		      	values(
		      	9,
		      	@PayNO,
		      	@DateTime,
		      	@Operator,
		      	@Program,
		      	@PC)
		      END
		  Else
		  	BEGIN
		  		Delete From D_Exclusive Where DataKBN = '9' and Number = @PayNO
		  		insert into D_Exclusive(
		      	DataKBN,
		      	Number,
		      	OperateDataTime,
		      	Operator,
		      	Program,
		      	PC)
		      	values(
		      	9,
		      	@PayNO,
		      	@DateTime,
		      	@Operator,
		      	@Program,
		      	@PC)
		  	END	


---sheetZ
	Insert into L_Log(
				--SEQ,
				OperateDate,
				OperateTime,
				InsertOperator,
				Program,
				PC,
				OperateMode,
				KeyItem)
				select 
				--IDENT_CURRENT('L_Log') AS SEQ,
				CONVERT(VARCHAR(10), getdate(), 111),
				CONVERT(VARCHAR(10), getdate(), 108),
				@Operator,
				@Program,
				@PC,
				@OperateMode,
				@PayNO
	
	
END

