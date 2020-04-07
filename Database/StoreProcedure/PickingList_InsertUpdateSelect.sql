 BEGIN TRY 
 Drop Procedure dbo.[PickingList_InsertUpdateSelect]
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
CREATE PROCEDURE [dbo].[PickingList_InsertUpdateSelect]
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@ShippingPlanDateFrom1 date,
	@ShippingPlanDateTo1 date,
	@ShippingPlanDateFrom2 date,
	@ShippingPlanDateTo2 date,
	@ShippingDate date,
	@Check1 varchar(2),
	@Check3 varchar(2),
	@Operator varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @Output1 as varchar(11),@Output2 as varchar(11),
	@DateTime datetime=getdate()
	
		EXEC Fnc_GetNumber
            26,-------------in伝票種別 
            @ShippingPlanDateTo1,----in基準日
            null,
            @Operator,
            @Output1 OUTPUT
            ;

		IF @Check1='1'
		begin
			Exec dbo.PickingList_InsertUpdate_Check1 @SoukoCD,@ShippingPlanDateFrom1,@ShippingPlanDateTo1,@ShippingDate,@Output1,@Operator,@DateTime

		end

		IF @Check3='1'
		begin
			Exec dbo.PickingList_InsertUpdate_Check1 @SoukoCD,@ShippingPlanDateFrom2,@ShippingPlanDateTo2,@ShippingDate,@Output2,@Operator,@DateTime
		end

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
			--fst1.TagName as 'TagName1',
			--fst2.TagName as 'TagName2',
			--fst3.TagName as 'TagName3',
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
			and dpd.PickingNO= (case when @Check1=1 then  @Output1 
									when @Check3=1 then @Output2 end)

	
END
