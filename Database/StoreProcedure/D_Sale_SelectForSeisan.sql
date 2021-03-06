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
		Other Money,
		SalesDate date
	)
 
	Insert Into #tmp_Sales(SlipNum,TotalSales,Amount8 ,Amonunt10 ,TaxAmount ,SalesExcludingTax ,Foreigntax8 ,Foreigntax10 ,Consumpitontax,TaxIncludeSales ,Cash ,Hanging ,VISA ,JCB,Other,SalesDate ) 	SELECT 		Count(fs.SalesNO) as SlipNum,		Sum(fs.SalesGaku) as TotalSales,		Sum(fs.SalesHontaiGaku8 + fs.SalesTax8) as Amount8 ,		Sum(fs.SalesHontaiGaku10 + fs.SalesTax10) as Amonunt10,		Sum(fs.SalesHontaiGaku0) as TaxAmount,		Sum(fs.SalesHontaiGaku8 + fs.SalesHontaiGaku10 + fs.SalesHontaiGaku0)  as SalesExcludingTax,		Sum(fs.SalesTax8) as Foreigntax8,		Sum(fs.SalesTax10) as Foreigntax10,		Sum(fs.SalesTax8 + fs.SalesTax10) as Consumpitontax,		Sum(fs.SalesHontaiGaku8 + fs.SalesHontaiGaku10 + fs.SalesHontaiGaku0 +fs.SalesTax8 + fs.SalesTax10) as TaxIncludeSales,				Sum( Cash),		SUM( Hanging),		SUM( VISA),		SUM( JCB),		SUM( Other),		fs.SalesDate	 FROM F_Sales(cast(@Date as varchar(10))) fs	 left outer join	 (		select Number,		Sum(CASE			WHEN mdkbn.SystemKBN = 1 THEN dph.DepositGaku			ELSE 0			END) AS Cash,		sum(CASE 
		WHEN mdkbn.SystemKBN = 9 THEN dph.DepositGaku
		END)AS Hanging,
		sum(CASE
		WHEN mdkbn.SystemKBN = 2 and mmp.Num1 = 1 THEN dph.DepositGaku
		END) AS VISA,
		sum(CASE
		WHEN mdkbn.SystemKBN = 2 and mmp.Num1 = 2 THEN dph.DepositGaku
		END) AS JCB,
		sum(CASE
			WHEN mdkbn.SystemKBN = 3 or mdkbn.SystemKBN = 4 or mdkbn.SystemKBN = 5 or mdkbn.SystemKBN = 6 or mdkbn.SystemKBN = 7 or mdkbn.SystemKBN = 8 or mdkbn.SystemKBN = 10 or
			mdkbn.SystemKBN = 11 or mdkbn.SystemKBN = 12 or mdkbn.SystemKBN = 13 THEN dph.DepositGaku
			END) As Other		from D_DepositHistory dph		left outer join M_DenominationKBN mdkbn on mdkbn.DenominationCD = dph.DenominationCD		left outer join M_MultiPorpose mmp on mmp.ID = '303' and mmp.[Key] = mdkbn.CardCompany		--where DeleteDateTime is null	 --and JuchuuKBN in (2,3)	 --and InvalidFLG = 0	 where dph.DepositKBN = 1	 and dph.DenominationCD is not null	 and dph.DataKBN = 3	 group by Number	 ) dph on fs.SalesNO = dph.Number	 Left Outer Join F_Store (cast(@Date as varchar(10))) fstore on fstore.StoreCD = fs.StoreCD and fstore.ChangeDate <= fs.SalesDate	 --Left Outer Join M_DenominationKBN mdkbn on mdkbn.DenominationCD = dcp.PaymentMethodCD	 --Left Outer Join M_MultiPorpose mmp on mmp.ID = '303' and mmp.[Key] = mdkbn.CardCompany	 	 Where fs.DeleteDateTime is null 	 and fs.SalesDate = @Date	 and fs.StoreCD = @StoreCD 	 --and dcp.DeleteDateTime is null 	 --and dcp.InvalidFLG = 0	 --and dcp.JuchuuKBN in (2,3)	 and fstore.DeleteFlg = 0	 and fstore.StoreKBN = 1		 Group by fs.SalesDate		Select		IsNull(Cast(tmps.SlipNum as int),0)as SlipNum,	IsNull(Cast(tmps.TotalSales as int),0) as TotalSales,	IsNull(Cast(tmps.Amount8 as int),0) as Amount8,	IsNull(Cast(tmps.Amonunt10 as int),0) as Amount10 ,	IsNull(Cast(tmps.TaxAmount as int),0) as TaxAmount,	IsNull(Cast(tmps.SalesExcludingTax as int),0) as SalesExcludingTax,	IsNull(Cast(tmps.Foreigntax8 as int),0) as Foreigntax8,	IsNull(Cast(tmps.Foreigntax10 as int),0)  as Foreigntax10,	IsNull(Cast(tmps.Consumpitontax as int),0) as Consumpitontax,	IsNull(Cast(tmps.TaxIncludeSales as int),0) as TaxIncludeSales,	IsNull(Cast(tmps.Cash as int),0)as Cash,	IsNull(Cast(tmps.Hanging as int),0)as Hanging,	IsNull(Cast(tmps.VISA as int),0)as VISA ,	IsNull(Cast(tmps.JCB as int),0)as JCB,	IsNull(Cast(tmps.Other as int),0)as Other	From #tmp_Sales tmps 	Where tmps.SalesDate = @Date	drop table #tmp_Sales


END
