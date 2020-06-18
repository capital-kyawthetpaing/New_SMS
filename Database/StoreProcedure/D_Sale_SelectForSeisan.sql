 BEGIN TRY 
 Drop Procedure dbo.[D_Sale_SelectForSeisan]
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
CREATE PROCEDURE [dbo].[D_Sale_SelectForSeisan]
	-- Add the parameters for the stored procedure here
	@StoreCD as VarChar(6) ,
	@Date as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create table #tmp_Sales
	(
		SlipNum varchar(11),
		TotalSales Money,
		Amount8 Money,
		Amonunt10 Money,
		TaxAmount Money,
		SalesExcludingTax Money,
		Foreigntax8 Money,
		Foreigntax10 Money,
		Consumpitontax Money,
		TaxIncludeSales Money,
		Cash Money,
		Hanging Money,
		VISA Money,
		JCB Money,
		SalesDate date
	)
 
	Insert Into #tmp_Sales(SlipNum,TotalSales,Amount8 ,Amonunt10 ,TaxAmount ,SalesExcludingTax ,Foreigntax8 ,Foreigntax10 ,Consumpitontax,TaxIncludeSales ,Cash ,Hanging ,VISA ,JCB,SalesDate ) 
	SELECT 
		Count(fs.SalesNO) as SlipNum,
		Sum(fs.SalesGaku) as TotalSales,
		Sum(fs.SalesHontaiGaku8 + fs.SalesTax8) as Amount8 ,
		Sum(fs.SalesHontaiGaku10 + fs.SalesTax10) as Amonunt10,
		Sum(fs.SalesHontaiGaku0) as TaxAmount,
		Sum(fs.SalesHontaiGaku8 + fs.SalesHontaiGaku10 + fs.SalesHontaiGaku0)  as SalesExcludingTax,
		Sum(fs.SalesTax8) as Foreigntax8,
		Sum(fs.SalesTax10) as Foreigntax10,
		Sum(fs.SalesTax8 + fs.SalesTax10) as Consumpitontax,
		Sum(fs.SalesHontaiGaku8 + fs.SalesHontaiGaku10 + fs.SalesHontaiGaku0 +fs.SalesTax8 + fs.SalesTax10) as TaxIncludeSales,
		Sum(CASE 
			WHEN dcp.PaymentProgressKBN = 9 and mdkbn .SystemKBN = 1 THEN dcb.CollectAmount
			END) AS Cash,
		Sum(CASE 
			WHEN dcp.PaymentProgressKBN != 9 THEN dcb.CollectAmount
			END) AS Hanging,
		Sum(CASE 
			WHEN dcp.PaymentProgressKBN = 9 and mdkbn .SystemKBN = 2 and mmp.Num1 = 1 THEN dcb.CollectAmount
			END) AS VISA,
		Sum(CASE 
			WHEN dcp.PaymentProgressKBN = 9 and  mmp.Num1 = 2 THEN dcb.CollectAmount
			END) AS JCB,

		fs.SalesDate
	 FROM F_Sales(cast(@Date as varchar(10))) fs
	 Left Outer Join D_CollectPlan dcp on dcp.SalesNO = fs.SalesNO
	 Left Outer Join D_CollectBilling dcb on dcb.CollectPlanNO = dcp.CollectPlanNO
	 Left Outer Join D_Collect dc on dc.CollectNO = dcb.ConfirmNo
	 Left Outer Join D_Juchuu djc on djc.JuchuuNo = dcp.JuchuuNO
	 --Left Outer Join M_Store ms on ms.StoreCD = fs.StoreCD and ms.ChangeDate <= fs.SalesDate
	 Left Outer Join F_Store (cast(@Date as varchar(10))) fstore on fstore.StoreCD = fs.StoreCD and fstore.ChangeDate <= fs.SalesDate
	 Left Outer Join M_DenominationKBN mdkbn on mdkbn.DenominationCD = dc.PaymentMethodCD
	 Left Outer Join M_MultiPorpose mmp on mmp.ID = '303' and mmp.[Key] = mdkbn.CardCompany
	 
	 Where fs.DeleteDateTime is null 
	 and fs.SalesDate = @Date
	 and fs.StoreCD = @StoreCD 
	 and dcp.DeleteDateTime is null 
	 and dcp.InvalidFLG = 0
	 and dcb.DeleteDateTime is null 
	 and dc.DeleteDateTime is null
	 and djc.JuchuuKBN in (2,3)
	 and fstore.DeleteFlg = 0
	 and fstore.StoreKBN = 1	
	 Group by fs.SalesDate
	
	Select
	IsNull(FORMAT(Convert(Int,tmps.SlipNum),'#,#'),0) as SlipNum,
	--IsNull(FORMAT(Convert(Int,tmpjc.NumOfCustomer),'#,#'),0) as NumOfCustomer,
	IsNull(FORMAT(Convert(Int,tmps.TotalSales),'#,#'),0) as TotalSales,
	IsNull(FORMAT(Convert(Int,tmps.Amount8),'#,#'),0) as Amount8,
	IsNull(FORMAT(Convert(Int,tmps.Amonunt10),'#,#'),0) as Amount10 ,
	IsNull(FORMAT(Convert(Int,tmps.TaxAmount),'#,#'),0) as TaxAmount,
	IsNull(FORMAT(Convert(Int,tmps.SalesExcludingTax),'#,#'),0) as SalesExcludingTax,
	IsNull(FORMAT(Convert(Int,tmps.Foreigntax8),'#,#'),0)as Foreigntax8,
	IsNull(FORMAT(Convert(Int,tmps.Foreigntax10),'#,#'),0) as Foreigntax10,
	IsNull(FORMAT(Convert(Int,tmps.Consumpitontax),'#,#'),0) as Consumpitontax ,
	IsNull(FORMAT(Convert(Int,tmps.TaxIncludeSales),'#,#'),0) as TaxIncludeSales,
	IsNull(FORMAT(Convert(Int,tmps.Cash),'#,#'),0)as Cash,
	IsNull(FORMAT(Convert(Int,tmps.Hanging),'#,#'),0)as Hanging,
	IsNull(FORMAT(Convert(Int,tmps.VISA),'#,#'),0)as VISA ,
	IsNull(FORMAT(Convert(Int,tmps.JCB),'#,#'),0)as JCB
	From #tmp_Sales tmps 
	Where tmps.SalesDate = @Date

	drop table #tmp_Sales
END
