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
CREATE PROCEDURE [dbo].[RPC_UrikakekinTairyuuHyou_PrintSelect]
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

		set @query='select CustomerCD,CustomerName,SaleA,'+@cols2+'
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


		--select @query
		EXEC SP_EXECUTESQL @query

		Drop table #temp

END

