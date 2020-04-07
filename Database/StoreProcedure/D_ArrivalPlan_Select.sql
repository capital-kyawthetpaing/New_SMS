 BEGIN TRY 
 Drop Procedure dbo.[D_ArrivalPlan_Select]
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
CREATE PROCEDURE [dbo].[D_ArrivalPlan_Select]
	-- Add the parameters for the stored procedure here
	@SoukoCD as Varchar(30), 
	@CalcuArrivalPlanDate1 as date,
	@CalcuArrivalPlanDate2 as date,
	@FrmSoukoCD as varchar(6), 
	@ITEMCD as varchar(200),
	@JanCD as varchar(200), 
	@SKUCD as varchar(200),
	
	@ArrivalDate1 as date,
	@ArrivalDate2 as date, 
	@PurchaseSu as int, 
	@VendorDeliveryNo as varchar(15),
	
	@PurchaseDateFrom as date,
	@PurchaseDateTo as date,
	@VendorCD as varchar(13),
	@StatusFlg as tinyint,
	@DisplayFlg as tinyint


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @currentdate as date = getdate()
    -- Insert statements for procedure here
	Select 
	da.ArrivalDate,
	dap.CalcuArrivalPlanDate,
	dp.PurchaseDate,
	(CASE 
		  WHEN dap.ArrivalPlanKBN = 1 or dap.ArrivalPlanKBN = 2 THEN N'発注'
		  Else N'店舗移動'		  
	END) AS Goods,
	dap.SKUCD ,
	dap.JanCD,
	sku.SKUName,
	sku.ColorName,
	sku.SizeName,
	dap.ArrivalPlanSu,
	da.ArrivalSu,
	mv.VendorName,
	ms.SoukoName,
	(CASE 
		  WHEN sku.DirectFlg = 1 THEN N'〇'
		  Else null		  
	END) AS Directdelivery,
	(CASE 
		  WHEN dr.ReserveKBN = 1 THEN dr.Number  
	END) AS ReserveNumber,
	dap.Number,
	da.ArrivalNO,
	dp.PurchaseNO,
	da.VendorDeliveryNo

	From F_ArrivalPlan(cast(@currentdate as varchar(10))) dap
	Left Outer Join D_ArrivalDetails dad on dad.ArrivalPlanNO = dap.ArrivalPlanNO
	Left Outer Join F_Arrival(cast(@currentdate as varchar(10))) da on da.ArrivalNO = dad.ArrivalNO 
	Left Outer Join D_PurchaseDetails dpd on dpd.ArrivalNO = da.ArrivalNO
	Left Outer Join F_Purchase(cast(@currentdate as varchar(10)))  dp on dp.PurchaseNO = dpd.PurchaseNO
	Left Outer Join D_Stock ds on ds.ArrivalPlanNO = dap.ArrivalPlanNO 
	Left Outer Join D_Reserve dr on dr.StockNO = ds.StockNO 
	Left Outer Join M_SKU sku on sku.AdminNO = da.AdminNO and sku.ChangeDate <= da.ArrivalDate
	Left Outer Join M_Vendor mv on mv.VendorCD = dp.VendorCD and mv.ChangeDate <= dp.PurchaseDate
	Left Outer Join M_Souko ms on ms.SoukoCD = dap.FromSoukoCD and ms.ChangeDate <= dap.CalcuArrivalPlanDate
	Where dap.DeleteDateTime is null and 
	dad.DeleteDateTime is null and
	da.DeleteDateTime is null and 
	dpd.DeleteDateTime is null and 
	dp.DeleteDateTime is null and 
	ds.DeleteDateTime is null and 
	dr.DeleteDateTime is null and 
	sku.DeleteFlg = 0 and 
	mv.DeleteFlg = 0 and 
	ms.DeleteFlg = 0 and 
	mv.VendorFlg = 1 and 
	dap.SoukoCD = @SoukoCD and 
    (@ArrivalDate1 is null or	da.ArrivalDate >= @ArrivalDate1) and ( @ArrivalDate2 is null or da.ArrivalDate <= @ArrivalDate2) and 
	(@CalcuArrivalPlanDate1 is null or dap.CalcuArrivalPlanDate >= @CalcuArrivalPlanDate1) and ( @CalcuArrivalPlanDate2 is null or dap.CalcuArrivalPlanDate <= @CalcuArrivalPlanDate2) and
	--da.ArrivalSu = da.PurchaseSu and da.PurchaseSu = 0 and 
	(@StatusFlg is null or ((@StatusFlg = 0 or(da.ArrivalSu= da.PurchaseSu)) )) and 
	(@StatusFlg is null or ((@StatusFlg = 1 or (da.PurchaseSu= 0)) )) and
	
	(@PurchaseDateFrom is null or dp.PurchaseDate >= @PurchaseDateFrom) and (@PurchaseDateTo is null or dp.PurchaseDate <= @PurchaseDateTo) and 
	(@VendorDeliveryNo is null or da.VendorDeliveryNo = @VendorDeliveryNo) and 
	--(@VendorCD is null or dp.VendorCD = @VendorCD) and 
	--(@FrmSoukoCD is null or dap.FromSoukoCD = @FrmSoukoCD) and
	(@DisplayFlg is null or ((@DisplayFlg = 0 or (dp.VendorCD = @VendorCD)) )) and 
	(@DisplayFlg is null or ((@DisplayFlg = 1 or (dap.FromSoukoCD = @FrmSoukoCD)) )) and 
	(@ITEMCD is null or dap.SKUCD in (Select SKUCD from M_SKU where ITemCD in (select Item from SplitString(@ITEMCD,','))))and 
	--dap.JanCD in (@JanCD) and 
	--dap.SKUCD in (@SKUCD)
	(@JanCD is null or (dap.JanCD in  (select Item from SplitString(@JanCD,','))))and
	( @SKUCD is null  or( dap.SKUCD in (select Item from SplitString(@SKUCD,','))))
	Order by dap.CalcuArrivalPlanDate asc,da.ArrivalDate asc,dp.PurchaseDate asc,da.SKUCD asc
	
END


