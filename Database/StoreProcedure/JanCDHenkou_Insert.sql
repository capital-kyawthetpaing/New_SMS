 BEGIN TRY 
 Drop Procedure [dbo].[JanCDHenkou_Insert]
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
CREATE PROCEDURE [dbo].[JanCDHenkou_Insert]
	-- Add the parameters for the stored procedure here
	@xml as XML,
	@Operator as varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @Datetime as datetime = GETDATE();

	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	
	EXEC dbo.JanCDHenkou_D_JANUpdate_Insert @xml, @Datetime, @Operator

	EXEC dbo.JanCDHenkou_M_SKU_Update @xml, @Datetime, @Operator
	
--	EXEC dbo.JanCDHenkou_M_JANOrderPrice_Update @xml, @Datetime, @Operator
		 
	EXEC dbo.JanCDHenkou_M_StoreButtonDetails_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_DepositHistory_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_SalesDetails_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_SalesDetailsTran_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_Inventory_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_InventoryHistory_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_MoveRequestDetailes_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_MoveDetails_Update @xml, @Datetime, @Operator
		 
	EXEC dbo.JanCDHenkou_D_MonthlyStock_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_MonthlyPurchase_Update @xml, @Datetime, @Operator
		 
	EXEC dbo.JanCDHenkou_D_Warehousing_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_MinimumInventory_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_ShippingDetails_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_InstructionDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_Reserve_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_StockForReserve_Update @xml, @Datetime, @Operator
		 
	EXEC dbo.JanCDHenkou_D_MakerStock_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_Stock_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_SKENDeliveryDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_Delivery_Update @xml, @Datetime, @Operator

	EXEC dbo.JanCDHenkou_D_Arrival_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_ArrivalPlan_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_D_EDIOrderDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_OrderDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_PurchaseDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_PurchaseDetailsHistory_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_JuchuuDetails_Update @xml, @Datetime, @Operator
		
	EXEC dbo.JanCDHenkou_L_JuchuuDetailsHistory_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_D_MitsumoriDetails_Update @xml, @Datetime, @Operator
	
	EXEC dbo.JanCDHenkou_L_MitsumoriDetailsHistory_Update @xml, @Datetime, @Operator
	
	EXEC dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem			
	
END
