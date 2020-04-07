 BEGIN TRY 
 Drop Procedure dbo.[L_CostHistory_Insert]
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
CREATE PROCEDURE [dbo].[L_CostHistory_Insert]
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
	INSERT INTO L_CostHistory(
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
		UpdateDateTime,
		DeleteOperator,
		DeleteDateTime )
		VALUES (
		@CostNum,
		@PayeeCD,
		@RecordDate,
		@PayPlanDate,
		(select InputDateTime from D_Cost where CostNO = @CostNum),
		@StaffCD,
		@RegularFLG,
		@TotalGaku,
		@InsertOperator,
		@InsertDateTime,
		@UpdateOperator,
		@UpdateDateTime, 
		@DeleteOperator,
		@DeleteDateTime)
END


