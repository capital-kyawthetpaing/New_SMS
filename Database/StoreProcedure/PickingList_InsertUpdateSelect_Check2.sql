 BEGIN TRY 
 Drop Procedure dbo.[PickingList_InsertUpdateSelect_Check2]
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
CREATE PROCEDURE [dbo].[PickingList_InsertUpdateSelect_Check2](
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@PickingNO varchar(11),
	@Operator varchar(10)
	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	declare
	@DateTime datetime=getdate()
	

		--Select Data for report
			select 
			dst.RackNO,
			ds.JanCD,
			fs.SKUName,
			ds.ShippingPossibleSu,
			mmp.Char1,
			ds.Number,
			ds.SKUCD,
			fs.ColorName,
			fs.SizeName,
			ds.ShippingPlanDate,
			mb.BrandName,
			Isnull(fst1.TagName,'') +' ' +Isnull(fst2.TagName,'')+' '+Isnull(fst3.TagName,'') as 'TagName',
			ddp.DeliveryName
			from D_PickingDetails as dpd
			left outer join D_Picking as dp on dpd.PickingNO=dp.PickingNO and dp.DeleteDateTime is null
			left outer join D_Reserve as ds on ds.ReserveNO=dpd.ReserveNO and ds.DeleteDateTime is null
			left outer join D_Stock as dst on dst.StockNO=ds.StockNO and ds.DeleteDateTime is null
			left outer join D_DeliveryPlan as ddp on ddp.Number=ds.Number
			left outer join F_SKU(@DateTime) as fs on fs.AdminNO=ds.AdminNO and fs.DeleteFlg=0
			left outer join M_Brand as mb on mb.BrandCD=fs.BrandCD
			left outer join F_ITEM(@DateTime) as fi on fi.ITemCD=fs.ITemCD and fi.DeleteFlg=0
			left outer join F_SKUTag(@DateTime,1) as fst1 on fst1.AdminNO=ds.AdminNO
			left outer join F_SKUTag(@DateTime,2) as fst2 on fst2.AdminNO=ds.AdminNO
			left outer join F_SKUTag(@DateTime,3) as fst3 on fst3.AdminNO=ds.AdminNO
			left outer join M_MultiPorpose as mmp on mmp.[Key]=fs.TaniCD and mmp.ID='201'
			where dpd.DeleteDateTime is null
			and dpd.PickingNO=@PickingNO


END

