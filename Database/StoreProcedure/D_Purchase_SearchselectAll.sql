 BEGIN TRY 
 Drop Procedure dbo.[D_Purchase_SearchselectAll]
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
CREATE PROCEDURE [dbo].[D_Purchase_SearchselectAll]
	-- Add the parameters for the stored procedure here
	@VendorCD as varchar(13),
	@JanCD as  varchar(150),
	@SKUCD as varchar(320),
	@ItemCD as varchar(350),
	@ItemName as varchar(80),	--searchcase
	@MakerItemCD as varchar(30),--searchcase
	@PurchaseSDate as date,
	@PurchaseEDate as date,
	@PlanSDate as date,
	@PlanEDate as date,
	@OrderSDate as date,
	@OrderEDate as date,
	@ChkValue as tinyint,
	--@ChkSumi as tinyint,
	--@ChkMi as tinyint,
	@StaffCD as varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECTdp.PurchaseNO,CONVERT(VARCHAR(10),dp.PurchaseDate , 111) as PurchaseDate,			(dp.VendorCD+'  '+fv.VendorName) as PurchaseCDName,			dpd.SKUCD,						dpd.JanCD ,			fsku.MakerItem,			dpd.ItemName,			(dpd.ColorName+' '+dpd.SizeName) as ColorSize,			dpd.Remark,			--fsku.ITemCD,			IsNull(FORMAT(Convert(Int,dpd.PurchaseSu), '#,#') ,0)as  PurchaseSu,			IsNull(FORMAT(Convert(Int,dpd.PurchaserUnitPrice), '#,#') ,0)as  PurchaserUnitPrice,			IsNull(FORMAT(Convert(Int,dpd.PurchaseGaku), '#,#') ,0)as  PurchaseGaku,			IsNull(FORMAT(Convert(Int,dod.OrderSu), '#,#') ,0)as  OrderSu,			IsNull(FORMAT(Convert(Int,dpd.OrderUnitPrice), '#,#') ,0)as  OrderUnitPrice,		   IsNull(FORMAT(Convert(Int, dod.OrderHontaiGaku), '#,#') ,0)as   OrderHontaiGaku,			CONVERT(VARCHAR(10), do.OrderDate , 111) as OrderDate,			dpd.OrderNO,			CONVERT(VARCHAR(10),dod.ArrivePlanDate , 111) as ArrivePlanDate,			do.DestinationName,			fst.StoreName,			fstaff.StaffName,			CONVERT(VARCHAR(10), dp.PaymentPlanDate , 111) as PaymentPlanDate,			CONVERT(VARCHAR(10), dpp.PayConfirmFinishedDate , 111) as PayConfirmFinishedDate,			dpd.DeliveryNo			 from D_Purchase dpleft outer join D_PurchaseDetails dpd on dp.PurchaseNO = dpd.PurchaseNO left outer join F_Vendor(getdate()) fv on fv.VendorCD = dp.VendorCDleft outer join D_OrderDetails dod on dod.OrderNO = dpd.OrderNOleft outer join D_Order do on do.OrderNO = dod.OrderNOleft outer join F_SKU(getdate()) fsku on dpd.AdminNO = fsku.AdminNOleft outer join D_PayPlan dpp on dpp.Number = dp.PurchaseNOleft outer join F_Store(getdate()) fst on fst.StoreCD=dp.StoreCDleft outer join F_Staff(getdate()) fstaff on fstaff.StaffCD=dp.StaffCDWHERE dpd.DeleteDateTime is nullAND (@JanCD IS NULL OR  (dpd.JanCD in  (select Item from SplitString(@JanCD,','))))AND( @SKUCD IS NULL OR ( dpd.SKUCD in (select Item from SplitString(@SKUCD,','))))AND (@ItemName IS NULL OR  (dpd.ItemName like '%'+@ItemName+'%'))--D_PurchaseDetails(dpd) and fv.ChangeDate<=dp.PurchaseDateand (@VendorCD IS NULL OR  (fv.PayeeCD = @VendorCD))--M_Vendorand dod.OrderRows=dpd.OrderRowsand dod.DeleteDateTime is null  --D_OrderDetails(dod)and do.DeleteDateTime is null	and (@PlanSDate IS NULL OR (do.LastArriveDate>=@PlanSDate))	and (@PlanEDate IS NULL OR (do.LastArriveDate<=@PlanEDate))	and (@OrderSDate IS NULL OR (do.OrderDate>=@OrderSDate))	and (@OrderEDate IS NULL OR (do.OrderDate<=@OrderEDate))		--D_OrderAND fsku.ChangeDate<=dp.PurchaseDateAND (@ItemCD IS NULL OR  (fsku.ITemCD in  (select Item from SplitString(@ItemCD,','))))--Multiple ItemCDAND (@makeritemCD IS NULL OR   (fsku.MakerItem LIKE '%'+@makeritemCD+'%'))--M_SKU	AND dpp.DeleteDateTime is null		AND (@ChkValue IS NULL OR  (dpp.PayConfirmFinishedKBN=@ChkValue))--Check ONOFF caseAND dp.DeleteDateTime is nullAND (@PurchaseSDate IS NULL OR  (dp.PurchaseDate>=@PurchaseSDate))AND(@PurchaseEDate IS NULL OR ( dp.PurchaseDate<=@PurchaseEDate))AND (@VendorCD IS NULL OR  ( dp.VendorCD=@VendorCD))--残すHiddenPayeeFlg=0AND (@StaffCD IS NULL OR  ( dp.StaffCD=@StaffCD))ORDER BY dp.PurchaseNO ,dpd.DisplayRows ASC


--select 
--dp.PurchaseNO,
--CONVERT(VARCHAR(10),dp.PurchaseDate,111)as PurchaseDate,
--(dp.VendorCD+'  '+fv.VendorName) as PurchaseCDName,
--dpd.SKUCD,
--dpd.JanCD,
--dpd.ItemName,
--(dpd.ColorName+' '+dpd.SizeName) as ColorSize,--dpd.Remark,--IsNull(FORMAT(Convert(Int,dpd.PurchaseSu), '#,#') ,0)as  PurchaseSu,--			IsNull(FORMAT(Convert(Int,dpd.PurchaserUnitPrice), '#,#') ,0)as  PurchaserUnitPrice,--			IsNull(FORMAT(Convert(Int,dpd.PurchaseGaku), '#,#') ,0)as  PurchaseGaku,--			IsNull(FORMAT(Convert(Int,dod.OrderSu), '#,#') ,0)as  OrderSu,--			IsNull(FORMAT(Convert(Int,dpd.OrderUnitPrice), '#,#') ,0)as  OrderUnitPrice,--		    IsNull(FORMAT(Convert(Int, dod.OrderHontaiGaku), '#,#') ,0)as   OrderHontaiGaku,--			CONVERT(VARCHAR(10), do.OrderDate , 111) as OrderDate,
--dpd.OrderNO,
--CONVERT(VARCHAR(10),dod.ArrivePlanDate , 111) as ArrivePlanDate,--do.DestinationName,--fst.StoreName,--fstaff.StaffName,
--CONVERT(VARCHAR(10), dp.PaymentPlanDate , 111) as PaymentPlanDate,--CONVERT(VARCHAR(10), dpp.PayConfirmFinishedDate , 111) as PayConfirmFinishedDate,
--dpd.DeliveryNo
--from D_Purchase dp
--left outer join D_PurchaseDetails dpd on dpd.PurchaseNO=dp.PurchaseNO
--left outer join F_Vendor(getdate())fv on fv.VendorCD=dp.VendorCD
--left outer join D_OrderDetails dod on dod.OrderNO=dpd.OrderNO
--left outer join D_Order do on do.OrderNO=dod.OrderNO
--left outer join F_SKU(getdate()) fsku on dpd.AdminNO=fsku.AdminNO
--left outer join D_PayPlan dpp on dpp.Number=dp.PurchaseNO
--left outer join F_Store(getdate())fst on fst.StoreCD=dp.StoreCD
--left outer join F_Staff(getdate())fstaff on fstaff.StaffCD=dp.StaffCD


--where dpd.DeleteDateTime is null
--AND (@JanCD IS NULL OR  (dpd.JanCD in  (select Item from SplitString(@JanCD,','))))--AND( @SKUCD IS NULL OR ( dpd.SKUCD in (select Item from SplitString(@SKUCD,','))))--AND (@ItemName IS NULL OR  (dpd.ItemName like '%'+@ItemName+'%'))---dpd

--and fv.ChangeDate<=dp.PurchaseDate--and (@VendorCD IS NULL OR  (fv.PayeeCD = @VendorCD))--M_Vendor

--and do.DeleteDateTime is null--	and (@PlanSDate IS NULL OR (do.LastArriveDate>=@PlanSDate))--	and (@PlanEDate IS NULL OR (do.LastArriveDate<=@PlanEDate))--	and (@OrderSDate IS NULL OR (do.OrderDate>=@OrderSDate))--	and (@OrderEDate IS NULL OR (do.OrderDate<=@OrderEDate))---do


--and dod.OrderRows=dpd.OrderRows--and dod.DeleteDateTime is null---dod--AND fsku.ChangeDate<=dp.PurchaseDate--AND (@ItemCD IS NULL OR  (fsku.ITemCD in  (select Item from SplitString(@ItemCD,','))))--Multiple ItemCD--AND (@makeritemCD IS NULL OR   (fsku.MakerItem LIKE '%'+@makeritemCD+'%'))--M_SKU--AND dpp.DeleteDateTime is null	--AND (@ChkValue IS NULL OR  (dpp.PayConfirmFinishedKBN=@ChkValue))--Check ONOFF case--AND dp.DeleteDateTime is null--AND (@PurchaseSDate IS NULL OR  (dp.PurchaseDate>=@PurchaseSDate))--AND(@PurchaseEDate IS NULL OR ( dp.PurchaseDate<=@PurchaseEDate))--AND (@VendorCD IS NULL OR  ( dp.VendorCD=@VendorCD))--AND (@StaffCD IS NULL OR (dp.StaffCD=@StaffCD))--ORDER BY dp.PurchaseNO ,dpd.DisplayRows ASC

END

