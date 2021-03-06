BEGIN TRY 
 Drop Procedure dbo.[RPC_UriageMotochou_PrintSelect]
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
CREATE PROCEDURE[dbo].[RPC_UriageMotochou_PrintSelect]
	-- Add the parameters for the stored procedure here
	@FromYYYYMM as int,
	--@ToYYYYMM as int,
	@FromDate as date,
	--@ToDate as date,
	@CustomerCD as varchar(13),
	@StoreCD as varchar(6),
	@chkValue as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/* Create temp table for Tmp_D_MonthlyCalims1 */
	CREATE TABLE [dbo].[#Tmp_D_MonthlyCalims](
	[YYYYMM] [int] NOT NULL,
	[Date][date],
	[MonthlyKBN][varchar](20) collate Japanese_CI_AS,
	[SalesNo][varchar](11) collate Japanese_CI_AS,
	[Info][varchar](100) collate Japanese_CI_AS,
	[SalesGaku][money],
	[CollectAmount][money],
	[LastBalanceGaku][money],
	[DisplayOrder][int],
	[CustomerCD][varchar](13) collate Japanese_CI_AS)


	--CONSTRAINT [PK_Tmp_D_MonthlyCalims2] PRIMARY KEY CLUSTERED
	--(
	--[YYYYMM] ASC
	--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	--) ON [PRIMARY] 

	INSERT INTO #Tmp_D_MonthlyCalims
	(
		YYYYMM,
		[Date],
		MonthlyKBN,
		SalesNo,
		Info,
		SalesGaku,
		CollectAmount,
		LastBalanceGaku,
		DisplayOrder,
		CustomerCD
	)
	select
		@FromYYYYMM,
		@FromDate,
		'残',
		NULL,
		NULL,
		NULL,
		NULL,
		LastBalanceGaku,
		0,
		CustomerCD
	from D_MonthlyClaims 
	where DeleteDateTime is null
	and YYYYMM=@FromYYYYMM--YYYYMM
	and (@CustomerCD is null or (CustomerCD=@CustomerCD))
	and StoreCD=@StoreCD

	/* Create temp table for Tmp_D_SalesTran */
	CREATE TABLE [dbo].[#Tmp_D_SalesTran](
	[YYYYMM] [int] NOT NULL,
	[Date][date],
	[MonthlyKBN][varchar](20) collate Japanese_CI_AS,
	[SalesNo][varchar](11) collate Japanese_CI_AS,
	[Info][varchar](100) collate Japanese_CI_AS,
	[SalesGaku][money],
	[CollectAmount][money],
	[LastBalanceGaku][money],
	[DisplayOrder][int],
	[CustomerCD][varchar](13) collate Japanese_CI_AS)

	INSERT INTO #Tmp_D_SalesTran
	(
		YYYYMM,
		[Date],
		MonthlyKBN,
		SalesNo,
		Info,
		SalesGaku,
		CollectAmount,
		LastBalanceGaku,
		DisplayOrder,
		CustomerCD
	)
	select
		CONVERT(VARCHAR(6), dst.SalesDate, 112),
		SalesDate,
		'売上',
		dst.SalesNO,
		ISNULL(dsdt.JuchuuNO,'') + ' ' + ISNULL(dsdt.SKUName,''),
		dst.SalesGaku,
		NULL,
		NULL,
		1,
		dst.CustomerCD
	from D_SalesTran dst
	left outer join D_SalesDetailsTran dsdt on dsdt.DataNo=dst.DataNo and dsdt.SalesRows = 1
	where dsdt.DeleteDateTime is null
	--and dst.DeleteDateTime is null
	--and dst.SalesDate =  @FromDate
	and CONVERT(VARCHAR(6), dst.SalesDate, 112) =  @FromYYYYMM
	--and (@ToDate is null or (dst.SalesDate<= @ToDate))
	and (@CustomerCD is null or (dst.CustomerCD=@CustomerCD))
	and dst.StoreCD=@StoreCD

	/* Create temp table for Tmp_D_SalesTran */
	CREATE TABLE [dbo].[#Tmp_D_Collect1](
	[YYYYMM] [int] NOT NULL,
	[Date][date],
	[MonthlyKBN][varchar](20) collate Japanese_CI_AS,
	[SalesNo][varchar](11) collate Japanese_CI_AS,
	[Info][varchar](100) collate Japanese_CI_AS,
	[SalesGaku][money],
	[CollectAmount][money],
	[LastBalanceGaku][money],
	[DisplayOrder][int],
	[CustomerCD][varchar](13) collate Japanese_CI_AS)

	INSERT INTO #Tmp_D_Collect1
	(
		YYYYMM,
		[Date],
		MonthlyKBN,
		SalesNo,
		Info,
		SalesGaku,
		CollectAmount,
		LastBalanceGaku,
		DisplayOrder,
		CustomerCD
	)
	select
		CONVERT(VARCHAR(6), dpc.CollectClearDate, 112),
		dpc.CollectClearDate,
		'入金',
		dpc.CollectNO,
		ISNULL(dmn.DenominationName,'') + ' ' + ISNULL(fk.KouzaName,'') + ' ' + ISNULL(fk.KouzaNO,''),
		NULL,
		dcbd.CollectAmount,
		NULL,
		1,
		ds.CustomerCD
	from D_PaymentConfirm dpc
	left outer join D_Collect dc on dc.CollectNO = dpc.CollectNO
	left outer join 
		( select ConfirmNO,CollectPlanNO,SUM(CollectAmount) as CollectAmount from D_CollectBillingDetails group by ConfirmNO, CollectPlanNO ) dcbd
		on dcbd.ConfirmNO = dpc.ConfirmNO
	left outer join D_CollectPlan dcp on dcp.CollectPlanNO = dcbd.CollectPlanNO
	inner join D_Sales ds on ds.SalesNO = dcp.SalesNO--ses change left join to inner join
	left outer join M_DenominationKBN dmn on dmn.DenominationCD = dc.PaymentMethodCD
	left outer join F_Kouza(getdate()) fk on fk.KouzaCD=dc.KouzaCD
	where dpc.DeleteDateTime is null
	and dc.DeleteDateTime is null
	and dcp.DeleteDateTime is null
	and ds.DeleteDateTime is null
	--2021/05/31 Y.Nishikawa CHG 条件が不正。振込で無いときに必ずデータ取得できなくなる↓↓
	--and fk.DeleteFlg = 0
	and (fk.DeleteFlg is null or (fk.DeleteFlg is not null and fk.DeleteFlg = 0))
	--2021/05/31 Y.Nishikawa CHG 条件が不正。振込で無いときに必ずデータ取得できなくなる↑↑
	and CONVERT(VARCHAR(6), dpc.CollectClearDate, 112)= @FromYYYYMM--YYYYMMfrom
	--and (@ToYYYYMM is null or (CONVERT(VARCHAR(6), dpc.CollectClearDate, 112)<= @ToYYYYMM))--YYYYMMto
	and(@CustomerCD is null or ( ds.CustomerCD=@CustomerCD))
	and dc.StoreCD=@StoreCD

--	select * from
--	(
--	select YYYYMM,CONVERT(VARCHAR(7),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD from #Tmp_D_MonthlyCalims tmc
--	union 
--	select YYYYMM,CONVERT(VARCHAR(10),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD  from #Tmp_D_SalesTran tst
--	union
--	select YYYYMM,CONVERT(VARCHAR(10),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD from #Tmp_D_Collect1 tc1
--	union
--	select 
--	NULL,
--	NULL,
--	'残',
--	NULL,
--	NULL,
--	NULL,
--	NULL,
--	(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1),
--	2,
--	NULL) t1
--	inner join F_Customer(getdate()) fs on fs.CustomerCD=t1.CustomerCD--ses
--	where  (@chkValue is null
-- OR (@chkValue=1 AND(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1)<>0)
--OR  (@chkValue=2 AND(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1)=0)
--) 
--	--2021/05/31 Y.Nishikawa CHG 表示順修正↓↓
--	--order by DisplayOrder,MonthlyKBN
--	order by DisplayOrder,[Date],MonthlyKBN
	--2021/05/31 Y.Nishikawa CHG 表示順修正↑↑
	
	select tmp.*
, fc.CustomerName
from
(select * from (
select YYYYMM,CONVERT(VARCHAR(7),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD from #Tmp_D_MonthlyCalims tmc
union
select YYYYMM,CONVERT(VARCHAR(10),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD from #Tmp_D_SalesTran tst
union
select YYYYMM,CONVERT(VARCHAR(10),[Date] , 111) as [Date],MonthlyKBN,SalesNo,Info,SalesGaku,CollectAmount,LastBalanceGaku,DisplayOrder,CustomerCD from #Tmp_D_Collect1 tc1
union
select
NULL,
NULL,
'残',
NULL,
NULL,
NULL,
NULL,
--fs.CustomerName,
(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1),
2,
NULL) t1

where (@chkValue is null
OR (@chkValue=1 AND(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1)<>0)
OR (@chkValue=2 AND(select sum(LastBalanceGaku) from #Tmp_D_MonthlyCalims) + (select sum(SalesGaku) from #Tmp_D_SalesTran) - (select sum(CollectAmount) from #Tmp_D_Collect1)=0
)) ) tmp inner join F_Customer(getdate()) fc on tmp.CustomerCD = fc.CustomerCD

order by tmp.DisplayOrder,tmp.[Date],tmp.MonthlyKBN ASC

	drop table #Tmp_D_MonthlyCalims
	drop table #Tmp_D_SalesTran
	drop table #Tmp_D_Collect1

END

