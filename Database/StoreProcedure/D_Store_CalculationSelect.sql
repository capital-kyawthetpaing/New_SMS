 BEGIN TRY 
 Drop Procedure dbo.[D_Store_CalculationSelect]
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
CREATE PROCEDURE [dbo].[D_Store_CalculationSelect]
	-- Add the parameters for the stored procedure here
    @StoreCD as VarChar(6) ,
	@Date as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- D_DepositHistory

	Create table #tmp_DepositＨistory
	(
		CashSale  Money,
		Gift  Money,
		CashDeposit Money,
		CashPayment Money,
		DepositTransfer Money,
		DepositCash Money,
		DepositCheck Money,
		DepositBill Money,
		DepositOffset Money,
		DepositAdjustment Money,
		DepositReturns Money,
		DepositDiscount Money,
		DepositCancel Money,
		PaymentTransfer Money,
		PaymentCash Money,
		Paymentcheck Money,
		PaymentBill Money,
		PaymentOffset Money,
		PaymentAdjustment Money,
		DepositDateTime datetime
	)

	insert into #tmp_DepositＨistory(CashSale,Gift ,CashDeposit ,CashPayment ,DepositTransfer ,DepositCash ,DepositCheck ,DepositBill ,DepositOffset ,DepositAdjustment ,DepositReturns ,DepositDiscount ,DepositCancel ,
	PaymentTransfer ,PaymentCash ,Paymentcheck ,PaymentBill ,PaymentOffset,PaymentAdjustment ,DepositDateTime )
	SELECT 
		Sum(CASE 
		  WHEN ddh.DepositKBN = 1 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		  END) AS CashSale,

		Sum(CASE 
		  WHEN ddh.DepositKBN = 7 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		  END) AS Gift,

		Sum(CASE 
		  WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		  END) AS CashDeposit,	
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		   END) AS CashPayment,
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 5 THEN ddh.DepositGaku
		    END) AS DepositTransfer,

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		    END) AS DepositCash,

		Sum(CASE 
		  WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 6 THEN ddh.DepositGaku
		  END) AS DepositCheck,	
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 8 THEN ddh.DepositGaku
		   END) AS DepositBill,
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 7 THEN ddh.DepositGaku
		    END) AS DepositOffset,

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn .SystemKBN = 9 THEN ddh.DepositGaku
		    END) AS DepositAdjustment,

		Sum(CASE 
		  WHEN ddh.DepositKBN = 9  THEN ddh.DepositGaku
		  END) AS DepositReturns ,

		Sum(CASE 
		  WHEN ddh.DepositKBN = 2 and mdkbn .SystemKBN = 10 THEN ddh.DepositGaku
		  END) AS DepositDiscount,
 
		Sum(CASE 
		  WHEN ddh.DepositKBN = 8  THEN ddh.DepositGaku
		  END) AS DepositCancel,

		Sum(CASE 
		  WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 5 THEN ddh.DepositGaku
		  END) AS PaymentTransfer,	
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5 )and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
		   END) AS PaymentCash,
		
		Sum(CASE
		   WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 6 THEN ddh.DepositGaku
		    END) AS Paymentcheck,

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 8 THEN ddh.DepositGaku
		    END) AS PaymentBill,

		Sum(CASE 
		  WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 7 THEN ddh.DepositGaku
		  END) AS PaymentOffset,	
		
		Sum(CASE 
		   WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 9 THEN ddh.DepositGaku
		   END) AS PaymentAdjustment,

		 CAST (ddh.DepositDateTime as Date) as DepositDateTime

	FROM D_DepositＨistory ddh
	Left Outer Join M_DenominationKBN mdkbn on mdkbn.DenominationCD = ddh.DenominationCD
	Left Outer Join F_Store(cast(@Date as varchar(10))) fs on fs.StoreCD = ddh.StoreCD and fs.ChangeDate <= ddh.AccountingDate
	--Left Outer Join M_Store ms on ms.StoreCD = ddh.StoreCD and ms.ChangeDate <= ddh.DepositDateTime 
	Where CAST(ddh.DepositDateTime as Date) = @Date 
	and ddh.StoreCD = @StoreCD 
	and fs.DeleteFlg = 0
	and fs.StoreKBN = 1
	Group By CAST (ddh.DepositDateTime as Date)



	-- D_Sales
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
 
	Insert Into #tmp_Sales(SlipNum,TotalSales,Amount8 ,Amonunt10 ,TaxAmount ,SalesExcludingTax ,Foreigntax8 ,Foreigntax10 ,Consumpitontax,TaxIncludeSales ,Cash ,Hanging ,VISA ,JCB ,SalesDate) 
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
		Left Outer Join D_Collect dc on dc.CollectNO = dcb.CollectPlanNO
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



	-- D_JuChuu

	Create table #tmp_JuChuu
	(
		NumOfCustomer varchar(11),
		JuchuuDate date
	)

	Insert Into #tmp_JuChuu(NumOfCustomer,JuchuuDate)
	SELECT	 
	Count(djc.JuchuuNo) as NumOfCustomer,
	djc.JuchuuDate
	FROM F_Sales(cast(@Date as varchar(10))) fs
	Left Outer Join D_CollectPlan dcp on dcp.SalesNO = fs.SalesNO
	Left Outer Join D_Juchuu djc on djc.JuchuuNo = dcp.JuchuuNO
	--Left Outer Join M_Store ms on ms.StoreCD = ds.StoreCD and ms.ChangeDate <= ds.SalesDate
	Left Outer Join F_Store (cast(@Date as varchar(10))) fstore on fstore.StoreCD = fs.StoreCD and fstore.ChangeDate <= fs.SalesDate
	WHERE fs.DeleteDateTime is null 
	and fs.SalesDate = @Date
	and fs.StoreCD = @StoreCD 
	and dcp.DeleteDateTime is null 
	and dcp.InvalidFLG = 0
	and djc.JuchuuKBN in (2,3)
	and fstore.DeleteFlg = 0
	and fstore.StoreKBN = 1
	GROUP BY djc.JuchuuDate

	Select 

	IsNull( FORMAT(Convert(Int,dsc.[10000yen]), '#,#'),0) as Yen10000,
	IsNull(FORMAT(Convert(Int,dsc.[5000yen]), '#,#'),0) as Yen5000,
	IsNull(FORMAT(Convert(Int,dsc.[2000yen]), '#,#'),0) as Yen2000,
	IsNull(FORMAT(Convert(Int,dsc.[1000yen]), '#,#'),0) as Yen1000,
	IsNull(FORMAT(Convert(Int,dsc.[500yen]), '#,#'),0) as Yen500,
	IsNull(FORMAT(Convert(Int,dsc.[100yen]), '#,#'),0) as Yen100,
	IsNull(FORMAT(Convert(Int,dsc.[50yen]), '#,#'),0) as Yen50,
	IsNull(FORMAT(Convert(Int,dsc.[10yen]), '#,#'),0) as Yen10,
	IsNull(FORMAT(Convert(Int,dsc.[5yen]), '#,#'),0) as Yen5,
	IsNull(FORMAT(Convert(Int,dsc.[1yen]), '#,#'),0) as Yen1,
	IsNull(FORMAT(Convert(Int,dsc.Etcyen), '#,#'),0) as OtherYen,
	IsNull(FORMAT(Convert(Int,dsc.[10000yen] * 10000 + dsc.[5000yen] *5000 + dsc.[2000yen] * 2000 + dsc.[1000yen] * 1000 + dsc.[500yen] * 500 + dsc.[100yen] * 100 + dsc.[50yen] * 50 + dsc.[10yen] *10 + dsc.[5yen] * 5 + dsc.[1yen] * 1 + dsc.Etcyen), '#,#'),0) as TotalCash,
	dsc.Change as Change,	
	IsNull(FORMAT(Convert(Int,tmpddh.CashSale),'#,#'),0) as CashSale,
	IsNull(FORMAT(Convert(Int,tmpddh.Gift),'#,#'),0) as Gift,
	IsNull(FORMAT(Convert(Int,tmpddh.CashDeposit),'#,#'),0) as CashDeposit,
	IsNull(FORMAT(Convert(Int,tmpddh.CashPayment),'#,#'),0) as CashPayment,
	--dsc.Change  + IsNull(FORMAT(Convert(Int, tmpddh.CashSale - tmpddh.Gift + tmpddh.CashDeposit - tmpddh.CashPayment),'#,#'),0) as CashTotal,
	FORMAT(dsc.Change+ IsNull(tmpddh.CashSale,0)  -  IsNull(tmpddh.Gift,0) + IsNull(tmpddh.CashDeposit,0) - IsNull(tmpddh.CashPayment,0),'#,#') as CashTotal,


	IsNull(FORMAT(Convert(Int,dsc.[10000yen] * 10000 + dsc.[5000yen] *5000 + dsc.[2000yen] * 2000 + dsc.[1000yen] * 1000 + dsc.[500yen] * 500 + dsc.[100yen] * 100 + dsc.[50yen] * 50 + dsc.[10yen] *10 + dsc.[5yen] * 5 + dsc.[1yen] * 1 + dsc.Etcyen -
	(dsc.Change + IsNull(tmpddh.CashSale,0) - IsNull(tmpddh.Gift,0) + IsNull(tmpddh.CashDeposit,0) - IsNull(tmpddh.CashPayment,0))), '#,#'),0) as CashStorage,

	IsNull(FORMAT(Convert(Int,tmps.SlipNum),'#,#'),0) as SlipNum,
	IsNull(FORMAT(Convert(Int,tmpjc.NumOfCustomer),'#,#'),0) as NumOfCustomer,
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
	IsNull(FORMAT(Convert(Int,tmps.JCB),'#,#'),0)as JCB,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositTransfer),'#,#'),0)as DepositTransfer,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositCash),'#,#'),0)as DepositCash,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositCheck),'#,#'),0)as DepositCheck,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositBill),'#,#'),0)as DepositBill ,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositOffset),'#,#'),0)as DepositOffset,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositAdjustment),'#,#'),0)as DepositAdjustment,
	--IsNull(FORMAT(Convert(Int, IsNull(tmpddh.DepositTransfer,0) + IsNull(tmpddh.DepositCash,0)+ IsNull(tmpddh.DepositCheck,0)+ IsNull(tmpddh.DepositBill,0) + IsNull(tmpddh.DepositOffset,0) + IsNull(tmpddh.DepositAdjustment,0)),'#,#'),0) as DepositTotal,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositTransfer + tmpddh.DepositCash + tmpddh.DepositCheck + tmpddh.DepositBill + tmpddh.DepositOffset + tmpddh.DepositAdjustment),'#,#'),0) as DepositTotal,
	
   IsNull(FORMAT(Convert(Int, tmpddh.DepositTransfer + tmpddh.DepositCash ),'#,#'),0) as total,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositReturns),'#,#'),0)as DepositReturns ,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositDiscount),'#,#'),0)as DepositDiscount,
	IsNull(FORMAT(Convert(Int,tmpddh.DepositCancel),'#,#'),0)as DepositCancel,
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentTransfer),'#,#'),0)as PaymentTransfer,
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentCash),'#,#'),0)as PaymentCash,
	IsNull(FORMAT(Convert(Int,tmpddh.Paymentcheck),'#,#'),0)as PaymentCheck,
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentBill),'#,#'),0)as PaymentBill,
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentOffset),'#,#'),0)as PaymentOffset,
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentAdjustment),'#,#'),0) as PaymentAdjustment ,
    --IsNull(FORMAT(Convert(Int,IsNull(tmpddh.PaymentTransfer,0) +IsNull(tmpddh.PaymentCash,0) + IsNull(tmpddh.Paymentcheck,0) + IsNull(tmpddh.PaymentBill,0) + IsNull(tmpddh.PaymentOffset,0) + IsNull(tmpddh.PaymentAdjustment,0)),'#,#'),0) as TotalPayment
	IsNull(FORMAT(Convert(Int,tmpddh.PaymentTransfer + tmpddh.PaymentCash + tmpddh.Paymentcheck + tmpddh.PaymentBill + tmpddh.PaymentOffset + tmpddh.PaymentAdjustment ),'#,#'),0) as TotalPayment

	

	From D_StoreCalculation dsc
	--Left Outer Join D_DepositＨistory ddh on ddh.DepositDateTime  = dsc.CalculationDate
	--Left Outer Join D_Sales ds on ds.SalesDate = dsc.CalculationDate
	--Left Outer Join D_Juchuu djc on djc.JuchuuDate = dsc.CalculationDate
	--Where dsc.CalculationDate = @Date and dsc.StoreCD = @StoreCD 
	Left Outer Join #tmp_DepositＨistory tmpddh on tmpddh.DepositDateTime = dsc.CalculationDate
	Left Outer Join #tmp_Sales tmps on tmps.SalesDate = dsc.CalculationDate
	Left Outer Join #tmp_JuChuu tmpjc on tmpjc.JuchuuDate = dsc.CalculationDate
	Where dsc.CalculationDate = @Date and dsc.StoreCD = @StoreCD

	drop table #tmp_DepositＨistory
	drop table #tmp_JuChuu
	drop table #tmp_Sales

END

