 BEGIN TRY 
 Drop Procedure dbo.[KehiNyuuryoku_Delete]
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
CREATE PROCEDURE [dbo].[KehiNyuuryoku_Delete] 
	-- Add the parameters for the stored procedure here
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
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Datetime as datetime = GETDATE();
   
	EXEC dbo.D_Cost_Delete @CostNOUpdate,@Datetime,@Operator,@Program,@PC,@OperateMode

	EXEC dbo.D_CostDetails_Delete @CostNOUpdate,@Datetime,@Operator,@Program,@PC,@OperateMode

	EXEC dbo.L_CostHistory_Insert 2,NULL,@CostNOUpdate,@PayeeCD,@RecordDate,@PayPlanDate,@StaffCD,@RegularFLG,@TotalGaku,NULL,NULL,NULL,NULL,@Operator,@Datetime

	EXEC dbo.L_CostDetailsHistory_Insert 2,NULL,@CostNOUpdate,@xml,NULL,NULL,NULL,NULL,@Operator,@Datetime

	EXEC dbo.D_PayPlan_Delete @CostNOUpdate,@Datetime,@Operator,@Program,@PC,@OperateMode
END


