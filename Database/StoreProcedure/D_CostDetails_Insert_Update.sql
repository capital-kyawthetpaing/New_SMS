 BEGIN TRY 
 Drop Procedure dbo.[D_CostDetails_Insert_Update]
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
CREATE PROCEDURE [dbo].[D_CostDetails_Insert_Update]
	-- Add the parameters for the stored procedure here
	@mode as int,
	@CostNO as varchar(11),
	@CostNOUpdate as varchar(11),
	@xml as XML,
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
	
		CREATE TABLE #Temp ( CostCD varchar(5), Summary varchar(100), DepartmentCD varchar(5), CostGaku money)

		DECLARE @DocHandle int
		
		EXEC sp_xml_preparedocument @DocHandle OUTPUT, @Xml
		
		INSERT INTO #Temp select * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
		WITH(CostCD varchar(5), Summary varchar(100), DepartmentCD varchar(5), CostGaku money) 
		EXEC sp_xml_removedocument @DocHandle; 

		DELETE #Temp WHERE CostCD IS NULL AND DepartmentCD IS NULL
		UPDATE #Temp SET CostGaku = 0 WHERE CostGaku IS NULL

		IF @mode = 1 
		BEGIN
			INSERT INTO D_CostDetails
			(
			CostNO,
			CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime 
			)
			SELECT
			@CostNO,
			ROW_NUMBER() OVER (ORDER BY(SELECT NULL)) AS CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			@Operator,
			@Datetime,
			@Operator,
			@Datetime
			FROM #Temp

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNO

		END
		ELSE IF @mode = 2
		BEGIN
			DELETE D_CostDetails WHERE CostNO = @CostNOUpdate

			INSERT INTO D_CostDetails
			(
			CostNO,
			CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime 
			)
			SELECT
			@CostNOUpdate,
			ROW_NUMBER() OVER (ORDER BY(SELECT NULL)) AS CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			@Operator,
			@Datetime,
			@Operator,
			@Datetime
			FROM #Temp

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@CostNOUpdate
		END

		


		DROP TABLE #Temp
END


