 BEGIN TRY 
 Drop Procedure dbo.[D_Cost_Insert_Update]
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
CREATE PROCEDURE [dbo].[D_Cost_Insert_Update]
	-- Add the parameters for the stored procedure here
	@mode as int,
	@CostNO as varchar(11),
	@CostNOUpdate as varchar(11),
	@PayeeCD as varchar(13),
	@RecordDate as date,
	@PayPlanDate as date,
	@StaffCD as varchar(10),
	@RegularFLG as tinyint,
	@TotalGaku as money,
	@Operator as varchar(10),
	@Datetime as Datetime,
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @mode = 1 
	BEGIN 
		INSERT INTO D_Cost(
		CostNO,
		PayeeCD,
		RecordedDate,
		PayPlanDate,
		InputDateTime,
		StaffCD,
		RegularlyFLG,
		TotalGaku,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime )
		VALUES (
		@CostNO,
		@PayeeCD,
		@RecordDate,
		@PayPlanDate,
		@Datetime,
		@StaffCD,
		@RegularFLG,
		@TotalGaku,
		@Operator,
		@Datetime,
		@Operator,
		@Datetime )

		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNO
	END
	ELSE 
	BEGIN
		UPDATE D_Cost 
		SET
		PayeeCD = @PayeeCD,
		RecordedDate = @RecordDate,
		PayPlanDate = @PayPlanDate,
		InputDateTime = @Datetime,
		StaffCD = @StaffCD,
		RegularlyFLG = @RegularFLG,
		TotalGaku = @TotalGaku,
		UpdateOperator = @Operator,
		UpdateDateTime = @Datetime
		WHERE CostNO = @CostNOUpdate

		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNOUpdate
	END

	
END


