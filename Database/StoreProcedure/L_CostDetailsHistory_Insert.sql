 BEGIN TRY 
 Drop Procedure dbo.[L_CostDetailsHistory_Insert]
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
CREATE PROCEDURE [dbo].[L_CostDetailsHistory_Insert]
	
	@mode as int,
	@CostNO as varchar(11),
	@CostNOUpdate as varchar(11),
	@xml as XML,
	@InsertOperator as varchar(10),
	@InsertDateTime as Datetime,
	@UpdateOperator as varchar(10),
	@UpdateDateTime as Datetime,
	@DeleteOperator as varchar(10),
	@DeleteDateTime as Datetime
	
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

		DECLARE @CostNum AS VARCHAR(11);
	
		IF @mode = 1
		BEGIN
			SET @CostNum = @CostNO
		END
		ELSE
		BEGIN
			SET @CostNum = @CostNOUpdate
		END

		INSERT INTO L_CostDetailsHistory
			(
			HistorySEQ,
			HistorySEQRow,
			CostNO,
			CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime,
			DeleteOperator,
			DeleteDateTime  
			)
			SELECT
			IDENT_CURRENT('L_CostHistory') AS HistorySEQ,
			ROW_NUMBER() OVER (ORDER BY(SELECT NULL)) AS HistorySEQRow,
			@CostNum,
			ROW_NUMBER() OVER (ORDER BY(SELECT NULL)) AS CostRows,
			CostCD,
			Summary,
			DepartmentCD,
			CostGaku,
			@InsertOperator,
			@InsertDateTime,
			@UpdateOperator,
			@UpdateDateTime,
			@DeleteOperator,
			@DeleteDateTime
			FROM #Temp

			DROP TABLE #Temp
END


