 BEGIN TRY 
 Drop Procedure dbo.[D_Stock_Update]
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
CREATE PROCEDURE [dbo].[D_Stock_Update]
	-- Add the parameters for the stored procedure here
	@RackNo as varchar(11),
	@Operator varchar(10),
	@InsertDateTime datetime,
	@StockNo as varchar(11)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--Insert 
	--Into D_Stock(RackNO ,UpdateOperator,UpdateDateTime)
	--Values (@RackNo ,@Operator ,@InsertDateTime)

	UPDATE D_Stock
	SET RackNO = @RackNo ,UpdateOperator = @Operator ,UpdateDateTime = @InsertDateTime
	WHERE StockNO = @StockNo
END

