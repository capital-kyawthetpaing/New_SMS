 BEGIN TRY 
 Drop Procedure dbo.[RPC_SiiresakiZaikoYoteiHyou]
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
CREATE PROCEDURE [dbo].[RPC_SiiresakiZaikoYoteiHyou]
@StoreCD as varchar(4),
@TargetSDate as int,
@TargetEDate as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @chdate as date;
	set @chdate=
   CAST(
      CAST(subString(CONVERT(varchar, @TargetSDate),0,5) AS VARCHAR(4)) +
      CAST(subString(CONVERT(varchar, @TargetSDate),5,3) AS VARCHAR(2))
       + CAST('01' AS VARCHAR(2))
   AS date)
   Select 
   --CONVERT(varchar(10), GETDATE(),111)+ '  '  + LEFT(CONVERT (varchar, GETDATE(), 108),5)AS Today,
   --CONVERT(varchar(10),GETDATE(),111) AS Today,
    dmp.VendorCD,
   MAX(fv.VendorName) AS VendorName,
   --(CAST(CONVERT(VARCHAR,CAST(SUM(dmp.LastmonthQuantity)AS MONEY),1)AS VARCHAR))AS LastMonthQuantity,
   SUM(dmp.LastMonthQuantity)AS LastMonthQuantity,
   SUM(dmp.LastMonthAmount)AS LastMonthAmount,
   SUM(dmp.ThisMonthPurchaseQ)AS ThisMonthPurchaseQ,
   SUM(dmp.ThisMonthPurchaseA)AS ThisMonthPurchaseA,
   SUM(dmp.ThisMonthCustPurchaseQ)AS ThisMonthCustPurchaseQ,
   SUM(dmp.ThisMonthCustPurchaseA)AS ThisMonthCustPurchaseA,
   SUM(dmp.ThisMonthPurchasePlanQ)AS ThisMonthPurchasePlanQ,
   SUM(dmp.ThisMonthPurchasePlanA)AS ThisMonthPurchasePlanA,
   SUM(dmp.ThisMonthSalesQ)AS ThisMonthSalesQ,
   SUM(dmp.ThisMonthSalesA)AS ThisMonthSalesA,
   SUM(dmp.ThisMonthCustSalesQ)AS ThisMonthCustSalesQ,
   SUM(dmp.ThisMonthCustSalesA)AS ThisMonthCustSalesA,
   SUM(dmp.ThisMonthSalesPlanQ)AS ThisMonthSalesPlanQ,
   SUM(dmp.ThisMonthSalesPlanA)AS ThisMonthSalesPlanA,
   SUM(dmp.ThisMonthReturnsQ)AS ThisMonthReturnsQ,
   SUM(dmp.ThisMonthReturnsA)AS ThisMonthReturnsA,
   SUM(dmp.ThisMonthReturnsPlanQ) AS ThisMonthReturnsPlanQ,
   SUM(dmp.ThisMonthReturnsPlanA) AS ThisMonthReturnsPlanA,
   SUM(dmp.ThisMonthPlanQuantity) AS ThisMonthPlanQuantity,
   SUM(dmp.ThisMonthPlanAmount) AS ThisMonthPlanAmount
   From  D_MonthlyPurchase dmp 
   left outer join F_Vendor(getdate()) fv on fv.VendorCD=dmp.VendorCD 
   where fv.DeleteFlg=0
   and fv.VendorFlg=1
   and (@StoreCD IS NULL OR  ( dmp.StoreCD=@StoreCD))
   and (@TargetSDate IS NULL OR(dmp.YYYYMM>=@TargetSDate))
   and (@TargetEDate IS NULL OR(dmp.YYYYMM<=@TargetEDate))
   Group by dmp.VendorCD
   order by dmp.VendorCD Asc

END
