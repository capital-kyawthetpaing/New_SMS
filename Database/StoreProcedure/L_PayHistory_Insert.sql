 BEGIN TRY 
 Drop Procedure dbo.[L_PayHistory_Insert]
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
CREATE PROCEDURE  [dbo].[L_PayHistory_Insert]
	-- Add the parameters for the stored procedure here
	@PayNO varchar(11),
	@LargePayNO varchar(11)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare
	 @ID int

	Insert into L_PayHistory(
		PayNO
		,LargePayNO
		,PayCloseNO
		,PayCloseDate
		,PayeeCD
		,InputDateTime
		,StaffCD
		,PayDate
		,PayPlanDate
		,HontaiGaku8
		,HontaiGaku10
		,TaxGaku8
		,TaxGaku10
		,PayGaku
		,NotPaidGaku
		,TransferGaku
		,TransferFeeGaku
		,FeeKBN
		,MotoKouzaCD
		,BankCD
		,BranchCD
		,KouzaKBN
		,KouzaNO
		,KouzaMeigi
		,CashGaku
		,BillGaku
		,BillDate
		,BillNO
		,ERMCGaku
		,ERMCDate
		,ERMCNO
		,CardGaku
		,OffsetGaku
		,OtherGaku1
		,Account1
		,SubAccount1
		,OtherGaku2
		,Account2
		,SubAccount2
		,FBCreateDate
		,FBCreateNO
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		,DeleteOperator
		,DeleteDateTime

	)

	SELECT PayNO
		,LargePayNO
		,PayCloseNO
		,PayCloseDate
		,PayeeCD
		,InputDateTime
		,StaffCD
		,PayDate
		,PayPlanDate
		,HontaiGaku8
		,HontaiGaku10
		,TaxGaku8
		,TaxGaku10
		,PayGaku
		,NotPaidGaku
		,TransferGaku
		,TransferFeeGaku
		,FeeKBN
		,MotoKouzaCD
		,BankCD
		,BranchCD
		,KouzaKBN
		,KouzaNO
		,KouzaMeigi
		,CashGaku
		,BillGaku
		,BillDate
		,BillNO
		,ERMCGaku
		,ERMCDate
		,ERMCNO
		,CardGaku
		,OffsetGaku
		,OtherGaku1
		,Account1
		,SubAccount1
		,OtherGaku2
		,Account2
		,SubAccount2
		,FBCreateDate
		,FBCreateNO
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		,DeleteOperator
		,DeleteDateTime

	From D_Pay
	where PayNO=@PayNO
	and LargePayNO=@LargePayNO

	set @ID=SCOPE_IDENTITY()

	Insert into L_PayDetailsHistory(
	HistorySEQ,
	HistorySEQRows,
	PayNO,
	PayNORows,
	PayPlanNO,
	PayGaku,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime,
	DeleteOperator,
	DeleteDateTime)
	Select
	@ID,
	row_number() over (order by (select NULL)),
	PayNO,
	PayNORows,
	PayPlanNO,
	PayGaku,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime,
	DeleteOperator,
	DeleteDateTime
	from D_PayDetails
	where PayNO=@PayNO

END

