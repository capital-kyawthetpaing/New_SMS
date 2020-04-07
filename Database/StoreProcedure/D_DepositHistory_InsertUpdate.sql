 BEGIN TRY 
 Drop Procedure dbo.[D_DepositHistory_InsertUpdate]
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
CREATE PROCEDURE [dbo].[D_DepositHistory_InsertUpdate]
	-- Add the parameters for the stored procedure here
	
	@StoreCD as varchar(6),
	@CustomerCD as varchar(13),
	@DiscountGaku as money,
	@ProperGaku as money,
	@DataKBN as tinyint,
	@DenominationCD as varchar(3),
	@DepositKBN as tinyint,
	@DepositGaku as money,
	@Remark as varchar(200),
	@ExchangeMoney as money,
	@ExchangeDenomination as varchar(3),
	@ExchangeCount as int,
	@AdminNO as int,
	@SalesSU as money,
	@SalesUnitPrice as money,
	@SalesGaku as money,
	@SalesTax as money,
	@TotalGaku as money,
	@Refund as money,
	@IsIssued as tinyint,
	@Operator as varchar(10),
	@Program as varchar(100),
	@PC as varchar(30),
	@KeyItem as varchar(100),
	@OperateMode as varchar(10),
	@CancelKBN as tinyint,
	@RecordKBN as tinyint,
	@AccountingDate as date,
	@Rows as tinyint,
	@SalesTaxRate as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @currentdate as datetime = GETDATE();

    -- Insert statements for procedure here
	Insert
	Into D_DepositＨistory
		(StoreCD,
		CustomerCD,
		DiscountGaku,
		ProperGaku,
		DepositDateTime,
		DataKBN,
		DepositKBN,
		DenominationCD,
		DepositGaku,
		Remark,
		Number,
		ExchangeMoney,
		ExchangeDenomination,
		ExchangeCount,
		AdminNO,
		SKUCD,
		JanCD,
		SalesSU,
		SalesUnitPrice,
		SalesGaku,
		SalesTax,
		TotalGaku,
		Refund,
		IsIssued,
		Program,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime,
		CancelKBN,
		RecoredKBN,
		AccountingDate,
		[Rows],
		SalesTaxRate )

	Values 
	(@StoreCD,
	@CustomerCD,
	@DiscountGaku,
	@ProperGaku,
	 @currentdate,
	 @DataKBN,
	 @DepositKBN, --'3',
	 @DenominationCD,
	 @DepositGaku,
	 @Remark,
	 Null,
	 @ExchangeMoney,
	 @ExchangeDenomination,
	 @ExchangeCount,
	 @AdminNO,
	 Null,
	 Null,
	 @SalesSU,
	 @SalesUnitPrice,
	 @SalesGaku,
	 @SalesTax,
	 @TotalGaku,
	 @Refund,
	 @IsIssued,
	 @Program,
	 @Operator,
	 @currentdate,
	 @Operator,
	 @currentdate,
	 @CancelKBN,
	 @RecordKBN,
	 @AccountingDate,
	 @Rows,
	 @SalesTaxRate)

	 Exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

