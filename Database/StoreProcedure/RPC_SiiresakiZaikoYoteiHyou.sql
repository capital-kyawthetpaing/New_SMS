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
   CONVERT(varchar(10), GETDATE(),111)+ '  '  + LEFT(CONVERT (varchar, GETDATE(), 108),5)AS Today,
   --CONVERT(varchar(10),GETDATE(),111) AS Today,
   dmp.VendorCD,
   MAX(fv.VendorName) AS VendorName,
   (CAST(CONVERT(VARCHAR,CAST(SUM(dmp.LastmonthQuantity)AS MONEY),1)AS VARCHAR))AS LastMonthQuantity,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.LastMonthAmount) AS MONEY), 1) AS VARCHAR)) AS LastMonthAmount,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPurchaseQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPurchaseQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPurchaseA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPurchaseA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthCustPurchaseQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthCustPurchaseQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthCustPurchaseA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthCustPurchaseA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPurchasePlanQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPurchasePlanQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPurchasePlanA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPurchasePlanA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthSalesQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthSalesQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthSalesA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthSalesA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthCustSalesQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthCustSalesQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthCustSalesA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthCustSalesA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthSalesPlanQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthSalesPlanQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthSalesPlanA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthSalesPlanA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthReturnsQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthReturnsQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthReturnsA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthReturnsA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthReturnsPlanQ) AS MONEY), 1) AS VARCHAR)) AS ThisMonthReturnsPlanQ,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthReturnsPlanA) AS MONEY), 1) AS VARCHAR)) AS ThisMonthReturnsPlanA,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPlanQuantity) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPlanQuantity,
   (CAST(CONVERT(VARCHAR, CAST( SUM(dmp.ThisMonthPlanAmount) AS MONEY), 1) AS VARCHAR)) AS ThisMonthPlanAmount
   
  
   --SUM(dmp.ThisMonthReturnsPlanQ) AS ThisMonthReturnsPlanQ,
   --SUM(dmp.ThisMonthReturnsPlanA) AS ThisMonthReturnsPlanA,
   --SUM(dmp.ThisMonthPlanQuantity) AS ThisMonthPlanQuantity,
   --SUM(dmp.ThisMonthPlanAmount) AS ThisMonthPlanAmount
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
