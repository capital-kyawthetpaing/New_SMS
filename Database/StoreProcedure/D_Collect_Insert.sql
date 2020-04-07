 BEGIN TRY 
 Drop Procedure dbo.[D_Collect_Insert]
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
CREATE PROCEDURE [dbo].[D_Collect_Insert]
	-- Add the parameters for the stored procedure here
	
	@InputKBN tinyint,
	@StoreCD varchar(6),
	@StaffCD varchar(10),
	--@InputDatetime datetime,
	--@WebCollectNO varchar(11),
	--@WebCollectType varchar(3),
	@CollectCustomerCD varchar(13),
	--@CollectDate date,
	@PaymentMethodCD varchar(3),
	--@KouzaCD varchar(3),
	--@BillDate date,
	@CollectAmount money,
	@FeeDeduction money,
	@Deduction1 money,
	@Deduction2 money,
	@DeductionConfirm money,
	@ConfirmSource money,
	@ConfirmAmount money,
	@Remark varchar(200),
	@AdvanceFLG tinyint,
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
	declare @currentdate as datetime = GETDATE(),@Today as date, @CollectNO as varchar(11), @Output as varchar(11);
	SET @Today = CONVERT(VARCHAR(10), @currentdate , 111);

	--execute @CollectNO = dbo.Fnc_GetNumber 7,@Today,@StoreCD,@Operator,@OutNo = @Output

		EXEC Fnc_GetNumber
            7,-------------in伝票種別 2
            @Today,----in基準日
            @StoreCD,-------in店舗CD
            @Operator,
            @Output OUTPUT
            ;

	INSERT INTO D_Collect 
	(
		CollectNO,
		InputKBN,
		StoreCD,
		StaffCD,
		InputDatetime,
		WebCollectNO,
		WebCollectType,
		CollectCustomerCD,
		CollectDate,
		PaymentMethodCD,
		KouzaCD,
		BillDate,
		CollectAmount,
		FeeDeduction,
		Deduction1,
		Deduction2,
		DeductionConfirm,
		ConfirmSource,
		ConfirmAmount,
		Remark,
		AdvanceFLG,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime
		
	)
	VALUES
	(
		@Output,
		@InputKBN,
		@StoreCD,
		@StaffCD,
		@currentdate,
		NULL,
		NULL,
		@CollectCustomerCD,
		@currentdate,
		@PaymentMethodCD,
		NULL,
		NULL,
		@CollectAmount,
		@FeeDeduction,
		@Deduction1,
		@Deduction2,
		@DeductionConfirm,
		@ConfirmSource,
		@ConfirmAmount,
		@Remark,
		@AdvanceFLG,
		@Operator,
		@currentdate,
		@Operator,
		@currentdate
	)


	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

