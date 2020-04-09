 BEGIN TRY 
 Drop Procedure dbo.[D_DepositHistory_Insert]
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
CREATE PROCEDURE [dbo].[D_DepositHistory_Insert]
	-- Add the parameters for the stored procedure here
	
	@StoreCD as varchar(6),
	@DataKBN as tinyint,
	@DepositKBN as tinyint,
	@DepositKBN1 as tinyint,
	@CancelKBN as tinyint,
	@RecoredKBN as tinyint,
	@DenominationCD as varchar(3),
	@Remark as varchar(200),
	@ExchangeMoney as money,
	@ExchangeDenomination as varchar(3),
	@ExchangeCount as int,
	@Rows as tinyint,
	@SalesSU as money,
	@SalesUnitPrice as money,
	@SalesGaku as money,
	@SalesTax as money,
	@SalesTaxRate as int,
	@TotalGaku as money,
	@Refund as money,
	@ProperGaku as money,
	@DiscountGaku as money,
	@CustomerCD as varchar(13),
	@IsIssued as tinyint,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)


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
		DepositDateTime,
		DataKBN,
		DepositKBN,
		CancelKBN,
		RecoredKBN,
		DenominationCD,
		DepositGaku,
		Remark,
		AccountingDate,
		Number,
		[Rows],
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
		SalesTaxRate,
		TotalGaku,
		Refund,
		ProperGaku,
		DiscountGaku,
		CustomerCD,
		IsIssued,
		Program,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime )

	Values 
	(@StoreCD,
	 @currentdate,
	 @DataKBN ,
	 @DepositKBN ,
	 @CancelKBN,
	 @RecoredKBN,
	 @DenominationCD,
	 @ExchangeMoney,
	 @Remark,
	 @currentdate,
	 Null,
	 @Rows,
	 @ExchangeMoney,
	 0,
	 0,
	 NULL,
	 Null,
	 Null,
	 @SalesSU ,
	 @SalesUnitPrice,
	 @SalesGaku,
	 @SalesTax,
	 @SalesTaxRate,
	 @TotalGaku,
	 @Refund,
	 @ProperGaku,
	 @DiscountGaku,
	 @CustomerCD,
	 @IsIssued,
	 @Program,
	 @Operator,
	 @currentdate,
	 @Operator,
	 @currentdate),

	 (
	 @StoreCD,
	 @currentdate,
	 @DataKBN ,
	 @DepositKBN1 ,
	 @CancelKBN,
	 @RecoredKBN,
	 @DenominationCD,
	 (-1)*@ExchangeMoney,
	 @Remark,
	 @currentdate,
	 Null,
	 @Rows,
	 @ExchangeMoney,
	 @ExchangeDenomination,
	 (-1)*@ExchangeCount,
	 NULL,
	 Null,
	 Null,
	 @SalesSU ,
	 @SalesUnitPrice,
	 @SalesGaku,
	 @SalesTax,
	 @SalesTaxRate,
	 @TotalGaku,
	 @Refund,
	 @ProperGaku,
	 @DiscountGaku,
	 @CustomerCD,
	 @IsIssued,
	 @Program,
	 @Operator,
	 @currentdate,
	 @Operator,
	 @currentdate)

 exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

