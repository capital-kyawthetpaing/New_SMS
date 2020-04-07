 BEGIN TRY 
 Drop Procedure dbo.[D_StockHistory_Insert]
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
CREATE PROCEDURE [dbo].[D_StockHistory_Insert]
	-- Add the parameters for the stored procedure here
	@RackNo as varchar(11),
	@StockNo as varchar(11),
	@SoukoCD as varchar(6),
	@JanCD as varchar(13),
	@SKUCD as varchar(30),
	@StockSu  as int,
	@Operator varchar(10),
	@InsertDateTime datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT 
	INTO D_StockHistory(StockNO,SoukoCD,RackNO ,JanCD,SKUCD,
	StockSu,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	VALUES (@StockNo ,@SoukoCD,@RackNo,@JanCD,@SKUCD,
	@StockSu,@Operator,@InsertDateTime,@Operator,@InsertDateTime)

END

