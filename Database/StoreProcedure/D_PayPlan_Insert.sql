 BEGIN TRY 
 Drop Procedure dbo.[D_PayPlan_Insert]
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
CREATE PROCEDURE [dbo].[D_PayPlan_Insert] 
	-- Add the parameters for the stored procedure here
	@mode as int,
	@CostNO as varchar(11),
	@CostNOUpdate as varchar(11),
	@StoreCD as varchar(4),
	@PayeeCD as varchar(13),
	@RecordDate as date,
	@PayPlanDate as date,
	@TotalGaku as money,
	@Operator as varchar(10),
	@Datetime as datetime,
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		DECLARE @CostNum AS VARCHAR(11);
	
		IF @mode = 1
		BEGIN
			SET @CostNum = @CostNO
		END
		ELSE
		BEGIN
			SET @CostNum = @CostNOUpdate
		END

    -- Insert statements for procedure here
	INSERT INTO D_PayPlan(
	PayPlanKBN,
	Number,
	StoreCD,
	PayeeCD,
	RecordedDate,
	PayPlanDate,
	HontaiGaku8,
	HontaiGaku10,
	TaxGaku8,
	TaxGaku10,
	PayPlanGaku,
	PayConfirmGaku,
	PayConfirmFinishedKBN,
	PayCloseDate,
	PayCloseNO,
	Program,
	PayConfirmFinishedDate,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime,
	DeleteOperator,
	DeleteDateTime )
	VALUES
	(
	2,
	@CostNum,
	@StoreCD,
	@PayeeCD,
	@RecordDate,
	@PayPlanDate,
	0,
	ROUND((@TotalGaku*100)/110,0,1),
	0,
	@TotalGaku-ROUND((@TotalGaku*100)/110,0,1),
	@TotalGaku,
	0,
	0,
	NULL,
	NULL,
	@Program,
	NULL,
	@Operator,
	@Datetime,
	@Operator,
	@Datetime,
	NULL,
	NULL )

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNum
END


