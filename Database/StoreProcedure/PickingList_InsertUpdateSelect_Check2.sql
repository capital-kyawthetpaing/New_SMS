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
			dpd.PickingNO as 'PickingNO',
			dst.RackNO,
			ds.JanCD,
			fs.SKUName,
			convert(varchar(30),ds.ShippingPossibleSu) +' ' +mmp.Char1 as 'ShippingPossibleSu',
			mmp.Char1,
			ds.Number,
			ds.SKUCD,
			fs.ColorName,
			fs.SizeName,
			Replace(CONVERT(char(10), ds.ShippingPlanDate,23),'-','/') as 'ShippingPlanDate',
			mb.BrandName,
			Isnull(fst1.TagName,'') +' ' +Isnull(fst2.TagName,'')+' '+Isnull(fst3.TagName,'') as 'TagName',
			case when ddp.DeliveryKBN<>2 then ddp.DeliveryName
					when ddp.DeliveryKBN=2 then fsouko.SoukoName 
			End as 'DeliveryName'
			from D_PickingDetails as dpd
			left outer join D_Picking as dp on dpd.PickingNO=dp.PickingNO 
											and dp.DeleteDateTime is null
			left outer join D_Reserve as ds on ds.ReserveNO=dpd.ReserveNO 
											and ds.DeleteDateTime is null
			left outer join D_Stock as dst on dst.StockNO=ds.StockNO 
											and ds.DeleteDateTime is null
			left outer join D_DeliveryPlan as ddp on ddp.Number=ds.Number
			left outer join F_Souko(@DateTime) as fsk on fsk.SoukoCD=ds.SoukoCD 
														and fsk.DeleteFlg=0
														and fsk.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKU(@DateTime) as fs on fs.AdminNO=ds.AdminNO 
													and fs.DeleteFlg=0
													and fs.ChangeDate<= ds.ShippingPlanDate
			left outer join M_Brand as mb on mb.BrandCD=fs.BrandCD
			left outer join F_ITEM(@DateTime) as fi on fi.ITemCD=fs.ITemCD 
													and fi.DeleteFlg=0
													and fi.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,1) as fst1 on fst1.AdminNO=ds.AdminNO
															and fst1.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,2) as fst2 on fst2.AdminNO=ds.AdminNO
															and fst2.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,3) as fst3 on fst3.AdminNO=ds.AdminNO
															and fst3.ChangeDate<= ds.ShippingPlanDate
			left outer join M_MultiPorpose as mmp on mmp.[Key]=fs.TaniCD 
												and mmp.ID='201'
			left outer join F_Souko(@DateTime) as fsouko on fsouko.SoukoCD=ddp.DeliverySoukoCD
														and fsouko.DeleteFlg=0
														and fsouko.ChangeDate<=ddp.DeliveryPlanDate
			where dpd.DeleteDateTime is null
			and dpd.PickingNO=@PickingNO
			order by dpd.PickingNO,dpd.PickingRows

END

