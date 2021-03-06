 BEGIN TRY 
 Drop Procedure dbo.[RPC_UrikakekinTairyuuHyou_PrintSelect]
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
Create PROCEDURE [dbo].[RPC_UrikakekinTairyuuHyou_PrintSelect]
	-- Add the parameters for the stored procedure here
	@FiscalYYYYMM int,
	@StoreCD varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Declare

	@cols NVARCHAR (MAX),
	 @cols1 NVARCHAR (MAX),
	 @cols2 NVARCHAR (MAX),
	 @query NVARCHAR(MAX)

	 declare @month int=11
	while(@month>=0)
	begin
		set @cols= COALESCE (@cols + ',ISNULL(['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+'],0) as '+'['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+']' , 'ISNULL(['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+'],0) as '+ '['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+']')

		set @cols2= COALESCE (@cols2 + ',ISNULL(['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+'],0) as '+'['+Convert(varchar,@month)+']' , 'ISNULL(['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+'],0) as '+ '['+Convert(varchar,@month)+']')
		
		set @cols1= COALESCE (@cols1 + ',['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+']', '['+Convert(varchar,(select resultdate from F_DecreaseMonth(@FiscalYYYYMM,@month)))+']')
		set @month=@month-1
	end

	--select @cols
	--select @cols1
	--select @cols2

	Select * into #temp
	from 
	((
		SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,11))
		and dmc.StoreCD=@StoreCD)

		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,10))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,9))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,8))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,7))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,6))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,5))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,4))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,3))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,2))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,1))
		and dmc.StoreCD=@StoreCD)
		
		Union all

		(SELECT dmc.CustomerCD,
		fc.CustomerName,
		dmc.SalesHontaiGaku + dmc.SalesTax 'Sale',
		dmc.CollectGaku,
		dmc.BalanceGaku,
		'0.00' 'Avg1',
		dmc.YYYYMM
		from D_MonthlyClaims dmc 
		inner join F_Store(getdate()) fs on dmc.StoreCD=fs.StoreCD 
		inner join F_Customer(getdate()) fc on dmc.CustomerCD=fc.CustomerCD
		where dmc.DeleteDateTime is null
		and YYYYMM = (select resultdate from F_DecreaseMonth(@FiscalYYYYMM,0))
		and dmc.StoreCD=@StoreCD))bb

	CREATE TABLE [dbo].TempCalculation
		(
		CustomerCD varchar(13),
		CustomerName varchar(200) collate Japanese_CI_AS,
		SaleA varchar(10) collate Japanese_CI_AS,
		[11] money,
		[10] money,
		[9] money,
		[8] money,
		[7] money,
		[6] money,
		[5] money,
		[4] money,
		[3] money,
		[2] money,
		[1] money,
		[0] money
		)



     set @query='insert into TempCalculation(
		CustomerCD ,
		CustomerName,
		SaleA ,
		[11] ,
		[10],
		[9],
		[8],
		[7],
		[6],
		[5],
		[4],
		[3],
		[2],
		[1],
		[0]
		)
		select CustomerCD,CustomerName,SaleA,'+@cols2+'
		from
		((
		Select CustomerCD,CustomerName,''売上'' ''SaleA'','+@cols+'
		from
		(
		select YYYYMM,CustomerCD,CustomerName,Sale
		from #temp t
		)p
		PiVOT
		(
		max(Sale)
		for [YYYYMM] IN ('+@cols1+')
		)x)

		union all

		(Select CustomerCD,CustomerName,''入金'' ''SaleA'','+@cols+'
		from
		(
		select YYYYMM,CustomerCD,CustomerName,CollectGaku
		from #temp t
		)p
		PiVOT
		(
		max(CollectGaku)
		for [YYYYMM] IN ('+@cols1+')
		)x )

		union all

		(Select CustomerCD,CustomerName,''残'' ''SaleA'','+@cols+'
		from
		(
		select YYYYMM,CustomerCD,CustomerName,BalanceGaku
		from #temp t
		)p
		PiVOT
		(
		max(BalanceGaku)
		for [YYYYMM] IN ('+@cols1+')
		)x ))bbv
		order by CustomerCD asc,SaleA desc'



     EXEC SP_EXECUTESQL @query


    select tc.* ,
	case when tc.SaleA <> '売上' then convert(varchar,(0.0),10)
	else
	case when tc.[0] + tc.[1] > t1.[0] then convert(varchar,(((t1.[0] - tc.[0])/tc.[1]) + 1),10)
	when tc.[0] + tc.[1] + tc.[2] > t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]))/tc.[2] + 2),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] > t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2]))/tc.[3] + 3),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] > t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3]))/tc.[4] + 4),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] > t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4]))/tc.[5] + 5),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5]))/tc.[6] + 6),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6]))/tc.[7] + 7),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7]))/tc.[8] + 8),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8] + tc.[9]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8]))/tc.[9] + 9),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8] + tc.[9] + tc.[10]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8] + tc.[9]))/tc.[10] + 10),10)
	when tc.[0] + tc.[1] + tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8] + tc.[9] + tc.[10] + tc.[11]> t1.[0] then convert(varchar,((t1.[0] - (tc.[0]+tc.[1]+ tc.[2] + tc.[3] + tc.[4] + tc.[5] + tc.[6] + tc.[7] + tc.[8] + tc.[9] + tc.[10]))/tc.[11] + 11),10)
	else convert(varchar,(0.0),10) end
	end Result
	from TempCalculation tc
	left join (select CustomerCD,SaleA,[0] from TempCalculation where SaleA = '残') as t1 on tc.CustomerCD = t1.CustomerCD
	order by tc.CustomerCD desc,tc.SaleA desc



	Drop table TempCalculation
	Drop table #temp

END


