 BEGIN TRY 
 Drop Procedure dbo.[KehiNyuuryoku_Insert_Update]
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
CREATE PROCEDURE [dbo].[KehiNyuuryoku_Insert_Update]
	-- Add the parameters for the stored procedure here
	@mode as int,
	@CostNOUpdate as varchar(11),
	@PayeeCD as varchar(13),
	@RecordDate as date,
	@PayPlanDate as date,
	@StaffCD as varchar(10),
	@RegularFLG as tinyint,
	@TotalGaku as money,
	@Operator as varchar(10),
	@StoreCD as varchar(4),
	@xml as XML,
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10)
	--@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Datetime as datetime = GETDATE(),@Changedate as date, @CostNO as varchar(11), @Output as varchar(11);
    SET @Changedate = CONVERT(VARCHAR(10), @RecordDate , 111);

	execute @CostNO = dbo.Fnc_GetNumber 16,@RecordDate,@StoreCD,@Operator,@OutNo = @Output

	EXEC dbo.D_Cost_Insert_Update @mode,@CostNO,@CostNOUpdate,@PayeeCD,@RecordDate,@PayPlanDate,@StaffCD,@RegularFLG,@TotalGaku,@Operator,@Datetime,@Program,@PC,@OperateMode

	EXEC dbo.D_CostDetails_Insert_Update @mode,@CostNO,@CostNOUpdate,@xml,@Datetime,@Operator,@Program,@PC,@OperateMode

	EXEC dbo.L_CostHistory_Insert @mode,@CostNO,@CostNOUpdate,@PayeeCD,@RecordDate,@PayPlanDate,@StaffCD,@RegularFLG,@TotalGaku,@Operator,@Datetime,@Operator,@Datetime,NULL,NULL

	EXEC dbo.L_CostDetailsHistory_Insert @mode,@CostNO,@CostNOUpdate,@xml,@Operator,@Datetime,@Operator,@Datetime,NULL,NULL

	EXEC dbo.D_PayPlan_Insert @mode,@CostNO,@CostNOUpdate,@StoreCD,@PayeeCD,@RecordDate,@PayPlanDate,@TotalGaku,@Operator,@Datetime,@Program,@PC,@OperateMode,@CostNO
END


