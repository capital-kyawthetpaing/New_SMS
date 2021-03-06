BEGIN TRY 
 Drop Procedure dbo.[D_StockReplica_Insert]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[D_StockReplica_Insert]    Script Date: 2020/05/08 2:15:20 PM ******/

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[D_StockReplica_Insert]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare

		@savedate as date= getdate(),
		@num as int
    -- Insert statements for procedure here
	SET @num=(Select Num1 from M_multiporpose where ID=225and [Key] =1)

	SELECT @savedate=DATEADD(Day, -@num, @savedate)

	Delete from D_StockReplica
	where ReplicaDate < @savedate



	Insert Into D_StockReplica
	SELECT
		(Select			ReplicaNO+1						
		From			D_StockReplicaNO					
		Where			KeyNo=1	),	
	CONVERT(VARCHAR(10), getdate(), 111),
	getdate(),
	ds.Stockno,
	ds.SoukoCD,
	ds.RackNO,
	ds.ArrivalPlanNO,
	ds.SKUCD,
	ds.AdminNO,
	ds.JanCD,
	ds.ArrivalYetFLG,
	ds.ArrivalPlanKBN,
	ds.ArrivalPlanDate,
	ds.ArrivalDate,
	ds.StockSu,
	ds.PlanSu,
	ds.AllowableSu,
	ds.AnotherStoreAllowableSu,
	ds.ReserveSu,
	ds.InstructionSu,
	ds.ShippingSu,
	ds.OriginalStockNo,
	ds.ExpectReturnDate,
	ds.ReturnPlanSu,
	ds.VendorCD,
	ds.ReturnDate,
	ds.ReturnSu,
	msku.LastCost,	
	ds.InsertOperator,
	ds.InsertDateTime,
	ds.UpdateOperator,
	ds.UpdateDateTime,
	ds.DeleteOperator,
	ds.DeleteDateTime

	
	From D_Stock as ds
	left outer join M_SKULastCost as msku 
	on msku.AdminNo=ds.AdminNO						
	and msku.SoukoCD=ds.SoukoCD
	order by ds.StockNO

	Update D_StockReplicaNO
	set ReplicaNO=ReplicaNO +1
	where KeyNO=1
END
