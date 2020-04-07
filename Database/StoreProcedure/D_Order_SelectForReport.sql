 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectForReport]
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
CREATE PROCEDURE [dbo].[D_Order_SelectForReport]
	-- Add the parameters for the stored procedure here
	@TargetDateFrom as date,
	@TargetDateTo as date,
	@SoukoCD as varchar(30),
	@StoreCD as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @Date as date = getdate();
    -- Insert statements for procedure here
	Select 
	CONVERT(varchar, case when dp.PurchaseDate is not null then dp.PurchaseDate 
		 else do.LastArriveDate end,111) as 'Date', 
	case when dp.PurchaseNO is not null then dp.PurchaseNO 
		 else do.OrderNO end as OrderNo,
	case when dp.PurchaseNO is not null then '*'
		 else null end as Star,
    do.OrderCD,
	mv.VendorName,
	ms.StaffName,
	dod.SKUCD +'    '+dod.ItemName as product ,
    case when dp.PurchaseNO is not null then dpd.PurchaseGaku
	     else dod.OrderHontaiGaku end as Gaku

	From D_Order do
	Left Outer Join D_OrderDetails dod on dod.OrderNO = do.OrderNO 
	Left Outer Join D_PurchaseDetails dpd on dpd.OrderNO = dod.OrderNO and dpd.OrderRows = dod.OrderRows
	Left Outer Join D_Purchase dp on dp.PurchaseNO = dpd.PurchaseNO 
	Left Outer Join F_Staff(cast(@Date as varchar)) ms on ms.StaffCD = do.StaffCD
	--Left Outer Join M_Staff ms on ms.StaffCD =do.StaffCD and ms.ChangeDate <= do.OrderDate
	--Left Outer Join M_Vendor mv on mv.VendorCD = do.OrderCD and mv.ChangeDate <= do.OrderDate
	Left Outer Join F_Vendor(cast(@date as varchar)) mv on mv.VendorCD = do.OrderCD
	Where do.DeleteDateTime is null and 
	dod.DeleteDateTime is null and
	dpd.DeleteDateTime is null and 
	dp.DeleteDateTime is null and 
	ms.DeleteFlg = 0 and 
	mv.DeleteFlg = 0 and 
	mv.VendorFlg = 1 and
	do.DestinationSoukoCD = @SoukoCD and
	dod.JuchuuNO is null and
	(dp.PurchaseNO is not null or
	(@TargetDateFrom is null or (dp.PurchaseDate >=@TargetDateFrom))  and (@TargetDateTo is null or (dp.PurchaseDate <= @TargetDateTo)) and
	dp.StoreCD = @StoreCD) or 
	(dp.PurchaseNO is null or 
		(@TargetDateFrom is null or (do.LastArriveDate >= @TargetDateFrom)) and (@TargetDateTo is null or (do.LastArriveDate <= @TargetDateTo)) and
	do.StoreCD = @StoreCD)
	Order by case when dp.PurchaseNO is not null  then dp.PurchaseDate 
			      else do.LastArriveDate end asc,
			 case when dp.PurchaseNO is not null  then dp.PurchaseNO  
			      else do.OrderNO end asc,
				  dod.SKUCD asc
END

