 BEGIN TRY 
 Drop Procedure dbo.[D_Purchase_Select]
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
Create PROCEDURE [dbo].[D_Purchase_Select]
	-- Add the parameters for the stored procedure here
	@DeliveryNo as varchar(15),
	@VendorCD as varchar(10),
	@PayeeFLg as tinyint,
	@ArrivalDateFrom as date,
	@ArrivalDateTo as date,
	@PurchaseDateFrom as date,
	@PurchaseDateTo as date,
	@PaymentDueDateFrom as date,
	@PaymentDueDateTo as date,
	@StaffCD as varchar(10),
	@ChkValue as tinyint,
	@StoreCD as varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	
      select   DP.PurchaseNO,
		CONVERT(VARCHAR(10),DP.PurchaseDate , 111) as PurchaseDate,	 
		DP.VendorCD+' '+(select VendorName from F_Vendor(DP.PurchaseDate) where VendorCD = DP.VendorCD) as VendorName,
		--IsNull(FORMAT(Convert(Int,DP.PurchaseGaku), '#,#') ,0)as  PurchaseGaku,
		--IsNull(FORMAT(Convert(Int,DP.PurchaseTax), '#,#') ,0)as  PurchaseTax,
		--IsNull(FORMAT(Convert(Int,DP.TotalPurchaseGaku), '#,#') ,0)as  TotalPurchaseGaku,
		DP.PurchaseGaku,
		DP.PurchaseTax,
		DP.TotalPurchaseGaku,
		DP.CommentInStore,
		CONVERT(VARCHAR(10),DP.PaymentPlanDate , 111) as PaymentPlanDate,
		CONVERT(VARCHAR(10),DPP.PayConfirmFinishedDate , 111) as PayConfirmFinishedDate,
		DPD.DeliveryNo,
		(select StoreName from F_Store(DP.PurchaseDate) where StoreCD = DP.StoreCD) as StoreName,
		(select StaffName from F_Staff(DP.PurchaseDate) where StaffCD = DP.StaffCD) as StaffName

	FROM D_Purchase as DP

		Left Outer Join D_PurchaseDetails AS DPD
		ON DPD.PurchaseNO=DP.PurchaseNO
		AND DPD.DeleteDateTime is NULL		--DPD
		AND DPD.DisplayRows = (select Min(DisplayRows) from D_PurchaseDetails )

		Left Outer Join M_Vendor as FV
		ON FV.VendorCD=dp.VendorCD
		AND FV.ChangeDate <= getdate()
		

		Left Outer Join  D_OrderDetails AS DOrderD
		ON DOrderD.OrderNO=DPD.OrderNO
		AND DOrderD.OrderRows=DPD.OrderRows
		AND DOrderD.DeleteDateTime is Null
		

		Left Outer Join D_Order AS DO
		ON DO.OrderNO=DOrderD.OrderNO
		AND DO.DeleteDateTime is NUll
		
		Left Outer Join D_PayPlan AS DPP
		ON DPP.Number=DP.PurchaseNO
		
		Left Outer Join D_Arrival as DA
		ON DA.ArrivalNO=DPD.ArrivalNO
		AND DA.DeleteDateTime is NULL

		WHERE DP.DeleteDateTime IS NULL
		AND (@PurchaseDateFrom is null or (DP.PurchaseDate>=CONVERT(DATE, @PurchaseDateFrom)))
		AND (@PurchaseDateTo is null or  (DP.PurchaseDate<=CONVERT(DATE, @PurchaseDateTo)))
		AND (@PaymentDueDateFrom is null or (DP.PaymentPlanDate>=CONVERT(DATETIME, @PaymentDueDateFrom)))
		AND (@PaymentDueDateTo is null or  (DP.PaymentPlanDate<=CONVERT(DATETIME, @PaymentDueDateTo)))
		AND (@StaffCD is null or  (DP.StaffCD=@StaffCD))
		AND (@VendorCD is null or (DP.VendorCD=@VendorCD))
		AND DP.StoreCD =@StoreCD 
		AND (@ChkValue IS NULL OR  (dpp.PayConfirmFinishedKBN=@ChkValue))
		AND (@ArrivalDateFrom is null or (DOrderD.ArrivePlanDate >= @ArrivalDateFrom))
		AND (@ArrivalDateTo is null or  (DOrderD.ArrivePlanDate <= @ArrivalDateTo))
		AND (@DeliveryNo is Null or (DA.VendorDeliveryNo =@DeliveryNo))
		AND (@VendorCD IS NULL or(FV.PayeeCD=@VendorCD))
		Order By
		DP.PurchaseNO ASC
END



