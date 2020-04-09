 BEGIN TRY 
 Drop Procedure dbo.[L_StockHistory_Insert]
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
CREATE PROCEDURE [dbo].[L_StockHistory_Insert]
	-- Add the parameters for the stored procedure here
	--@RackNo as varchar(11),
	--@StockNo as varchar(11),
	--@SoukoCD as varchar(6),
	--@JanCD as varchar(13),
	--@SKUCD as varchar(30),
	--@ArrivalYetFLG as tinyint,
	--@ArrivalPlanKBN as tinyint,
	--@ArrivalPlanDate as date,
	--@ArrivalDate as date,
	--@StockSu  as int,
	--@PlanSu as int,
	--@AllowableSu as int,
	--@AnotherStoreAllowableSu as int,
	--@ReserveSu as int,
	--@DeleteOperator as varchar(10),
	--@DeleteDateTime as datetime,
	@Operator varchar(10),
	@InsertDateTime datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
 --   INSERT 
	--INTO L_StockHistory(StockNO,SoukoCD,RackNO ,JanCD,SKUCD,
	--StockSu,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	--VALUES (@StockNo ,@SoukoCD,@RackNo,@JanCD,@SKUCD,
	--@StockSu,@Operator,@InsertDateTime,@Operator,@InsertDateTime)


	INSERT INTO L_StockHistory(StockNO,SoukoCD,RackNO ,JanCD,AdminNO,SKUCD,ArrivalYetFLG,ArrivalPlanKBN,ArrivalPlanDate,ArrivalDate,
	StockSu,PlanSu,AllowableSu,AnotherStoreAllowableSu,ReserveSu,InstructionSu,ShippingSu,OriginalStockNO,ExpectReturnDate,
	ReturnPlanSu,VendorCD,ReturnDate,ReturnSu,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime,DeleteOperator,DeleteDateTime)
	Select StockNO,SoukoCD,RackNO ,JanCD,AdminNO,SKUCD,ArrivalYetFLG,ArrivalPlanKBN,ArrivalPlanDate,ArrivalDate,
	StockSu,PlanSu,AllowableSu,AnotherStoreAllowableSu,ReserveSu,InstructionSu,ShippingSu,OriginalStockNO,ExpectReturnDate,
	ReturnPlanSu,VendorCD,ReturnDate,ReturnSu,@Operator,@InsertDateTime,@Operator,@InsertDateTime,DeleteOperator,DeleteDateTime
	From D_Stock

END

