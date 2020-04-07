 BEGIN TRY 
 Drop Procedure dbo.[D_CostDetails_Delete]
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
CREATE PROCEDURE [dbo].[D_CostDetails_Delete]
	-- Add the parameters for the stored procedure here
	@CostNOUpdate as varchar(11),
	@Datetime as datetime,
	@Operator as varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE D_CostDetails 
	SET 
		DeleteOperator = @Operator, 
		DeleteDateTime = @Datetime 
	WHERE CostNO = @CostNOUpdate

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNOUpdate
END


