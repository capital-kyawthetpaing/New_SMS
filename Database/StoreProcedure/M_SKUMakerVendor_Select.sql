 BEGIN TRY 
 Drop Procedure dbo.[M_SKUMakerVendor_Select]
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
CREATE PROCEDURE [dbo].[M_SKUMakerVendor_Select]
	-- Add the parameters for the stored procedure here
	@JanCD as varchar(13),
	@StoreCD as varchar(4),
	@DataCheck as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare
	@todayDate as date=convert(Date,Getdate())

	--if @DataCheck = 0
	--begin
	--	--select msku.MakerVendorCD,
	--	--mvd.VendorName,
	--	--msku.JanCD,
	--	--msku.ColorName +' '+msku.SizeName as 'ColorSizeName',
	--	select fsku.MakerVendorCD +'
	--	'+fv.VendorName as 'MakerVendorCD' ,
	--	fsku.JanCD +' '+ fsku.ColorName +' '+fsku.SizeName as 'JanCD',
	--	IsNull(FORMAT(Convert(Int,fsp.PriceWithoutTax), '#,#') ,0)as 'PriceWithoutTax',
	--	IsNull(FORMAT(Convert(Int,fsp.GeneralPriceOutTax), '#,#'),0) as 'GeneralPriceOutTax',
	--	IsNull(FORMAT(Convert(Int,mslc.LastCost), '#,#'),0) as 'LastCost',
	--	Convert(varchar,(mslc.LastCost/fsp.GeneralPriceOutTax)*100) +'%' as 'CostRate',
	--	Convert(varchar,100-((mslc.LastCost/fsp.GeneralPriceOutTax) *100)) +'%' as 'ProfitRate'
	--	from F_SKU(CAST(@todayDate as varchar(10))) fsku 
	--	left outer join F_Vendor(cast(@todayDate as varchar(10))) fv  on fsku.MakerVendorCD=fv.VendorCD 
	--	left outer join F_SKUPrice(cast(@todayDate as varchar(10))) fsp on fsku.SKUCD=fsp.SKUCD and fsp.TankaCD='0000000000000' and fsp.StoreCD=@StoreCD 
	--	left outer join M_SKULastCost as mslc on fsku.SKUCD=mslc.SKUCD

	--	where fsku.DeleteFlg=0 
	--	and fv.DeleteFlg=0
	--	and fsp.DeleteFlg=0
	--	and fsku.JanCD=@JanCD
	--	and fsku.ChangeDate<=@todayDate

	--	order by fv.VendorCD,fsku.JanCD,fsku.ColorNO,fsku.SizeNO,fsp.PriceWithoutTax,fsp.GeneralPriceOutTax,mslc.LastCost asc

	--End
	--Else
	--begin
	--	--select msku.MakerVendorCD,
	--	--mvd.VendorName,
	--	--msku.JanCD,
	--	--msku.ColorName +' '+msku.SizeName as 'ColorSizeName',
	--	select fsku.MakerVendorCD +'
	--	'+fv.VendorName as 'MakerVendorCD' ,
	--	fsku.JanCD +' '+ fsku.ColorName +' '+fsku.SizeName as 'JanCD',
	--	IsNull(FORMAT(Convert(Int,fsp.PriceWithoutTax), '#,#') ,0)as 'PriceWithoutTax',
	--	IsNull(FORMAT(Convert(Int,fsp.GeneralPriceOutTax), '#,#'),0) as 'GeneralPriceOutTax',
	--	IsNull(FORMAT(Convert(Int,mslc.LastCost), '#,#'),0) as 'LastCost',
	--	Convert(varchar,(mslc.LastCost/fsp.GeneralPriceOutTax)*100) +'%' as 'CostRate',
	--	Convert(varchar,100-((mslc.LastCost/fsp.GeneralPriceOutTax) *100)) +'%' as 'ProfitRate'
	--	from F_SKU(CAST(@todayDate as varchar(10))) fsku
	--	left outer join F_Vendor(cast(@todayDate as varchar(10))) fv  on fsku.MakerVendorCD=fv.VendorCD 
	--	left outer join F_SKUPrice(cast(@todayDate as varchar(10))) fsp on fsku.SKUCD=fsp.SKUCD and fsp.TankaCD='0000000000000' and fsp.StoreCD=@StoreCD 
	--	left outer join M_SKULastCost as mslc on fsku.SKUCD=mslc.SKUCD

	--	where fsku.DeleteFlg=0 
	--	and fv.DeleteFlg=0
	--	and fsp.DeleteFlg=0
	--	and fsku.ITemCD in (select distinct ITemCD  From M_SKU ms Where ms.DeleteFlg = 0 AND ms.JanCD = @JanCD ) 
	--	and fsku.ChangeDate<=@todayDate

	--	order by fv.VendorCD,fsku.JanCD,fsku.ColorNO,fsku.SizeNO,fsp.PriceWithoutTax,fsp.GeneralPriceOutTax,mslc.LastCost asc

	--end


	select fsku.MakerVendorCD +'
		'+fv.VendorName as 'MakerVendorCD' ,
		fsku.JanCD +' '+ fsku.ColorName +' '+fsku.SizeName as 'JanCD',
		IsNull(FORMAT(Convert(Int,fsp.PriceWithoutTax), '#,#') ,0)as 'PriceWithoutTax',
		IsNull(FORMAT(Convert(Int,fsp.GeneralPriceOutTax), '#,#'),0) as 'GeneralPriceOutTax',
		IsNull(FORMAT(Convert(Int,mslc.LastCost), '#,#'),0) as 'LastCost',
		IsNull(Convert(varchar,(mslc.LastCost/fsp.GeneralPriceOutTax)*100),0)  +'%' as 'CostRate',
		IsNull(Convert(varchar,100-((mslc.LastCost/fsp.GeneralPriceOutTax) *100)),0)  +'%' as 'ProfitRate'
		from F_SKU(CAST(@todayDate as varchar(10))) fsku
		left outer join F_Vendor(cast(@todayDate as varchar(10))) fv  on fsku.MakerVendorCD=fv.VendorCD  and fv.DeleteFlg=0
		left outer join F_SKUPrice(cast(@todayDate as varchar(10))) fsp on fsku.SKUCD=fsp.SKUCD and fsp.TankaCD='0000000000000' and fsp.StoreCD=@StoreCD and fsp.DeleteFlg=0
		left outer join M_SKULastCost as mslc on fsku.SKUCD=mslc.SKUCD 

		where fsku.DeleteFlg=0 
		and (@DataCheck = 1 or (fsku.JanCD = @JanCD))
		and (@DataCheck = 0 or (fsku.ITemCD in (select distinct ItemCD from M_SKU where DeleteFlg = 0 and JanCD = @JanCD)))
		and fsku.ChangeDate<=@todayDate

		order by fv.VendorCD,fsku.JanCD,fsku.ColorNO,fsku.SizeNO,fsp.PriceWithoutTax,fsp.GeneralPriceOutTax,mslc.LastCost asc
END

