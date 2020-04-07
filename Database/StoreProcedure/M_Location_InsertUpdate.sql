 BEGIN TRY 
 Drop Procedure dbo.[M_Location_InsertUpdate]
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
CREATE PROCEDURE [dbo].[M_Location_InsertUpdate] 
	-- Add the parameters for the stored procedure here
	@RackNo as varchar(11),
	@StockNo as varchar(11),
	@SoukoCD as varchar(6),
	@JanCD as varchar(13),
	@SKUCD as varchar(30),
	@StockSu  as int,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @InsertDateTime datetime=getdate()

	Exec dbo.D_Stock_Update @RackNo,@Operator,@InsertDateTime,@StockNo

	--Exec dbo.L_StockHistory_Insert @StockNo,@SoukoCD,@RackNo,@JanCD,@SKUCD,@StockSu,
	--@Operator,@InsertDateTime

	Exec dbo.L_StockHistory_Insert @Operator,@InsertDateTime

	Exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
    
	
END

