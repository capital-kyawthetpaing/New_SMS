 BEGIN TRY 
 Drop Procedure dbo.[RPC_ZaikoKanriHyou]
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
CREATE PROCEDURE [dbo].[RPC_ZaikoKanriHyou]
	-- Add the parameters for the stored procedure here
	@targetdate as date,
	@soukoCD as varchar(6),
	@YYYYMM as int,
	@itemcd as varchar(30) ,
	@sku as varchar(32),
	@jan as varchar(13) ,
	@makeritem as varchar(50),
	--@skuName as varchar(100),
	@itemName as varchar(80),
	@purchaseStartDate as Date,
	@purchaseEndDate as Date,
	@related as tinyint  --1 item, 2 maker,3 normal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

CREATE TABLE [dbo].[#Temp](
	[AdminNO] [int]  NOT NULL,
	[SoukoCD] [varchar] (6) collate Japanese_CI_AS  NULL,
	[AveragePurchaseGaku] [money] NULL,
	[TotalPurchaseGaku] [money] NULL
	CONSTRAINT [PK_Tmp_D_Purchase] PRIMARY KEY CLUSTERED 
	(
		[AdminNo] ASC
		--[SoukoCD] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
if @related = 1----item
	begin 
		insert into #Temp
		select 
		dpd.AdminNO,
		da.SoukoCD,
		isnull(sum(dpd.TotalPurchaseGaku/nullif(dpd.PurchaseSu,0)),0),
		isnull(sum(dpd.TotalPurchaseGaku),0)
		from D_Purchase dp
		left outer join D_PurchaseDetails dpd on dp.PurchaseNO = dpd.PurchaseNO
		left outer join D_Arrival da on da.ArrivalNO = dpd.ArrivalNO
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dpd.AdminNO  and fsku.DeleteFlg=0
		Where dp.DeleteDateTime is null
		and dpd.DeleteDateTime is null
		and da.DeleteDateTime is null
		and dp.ProcessKBN=4
		and PurchaseDate>=@purchaseStartDate
		and PurchaseDate<=@purchaseEndDate
		and da.SoukoCD=@soukoCD
		AND fsku.AdminNO in
		(
			select AdminNO from M_SKU where ITemCD in
				(
					select ITemCD from D_PurchaseDetails dpd
					left outer join F_SKU(@targetdate) fsku on dpd.AdminNO = fsku.AdminNO
					where (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
					and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
					and (@sku is null or (dpd.SKUCD in (select * from SplitString(@sku,','))))
					and (@jan is null or(dpd.JanCD in (select * from SplitString(@jan,','))))
					and (@itemName is null or(dpd.ItemName like '%' + @itemName + '%'))
				)
		)
		group by dpd.AdminNO,da.SoukoCD	

		select 
		CONVERT(varchar(10), GETDATE(),111) AS Today,
		dms.YYYYMM,
		dms.SoukoCD,
		fsouko.SoukoName,
		dms.SKUCD,
		dms.JanCD,
		fsku.SKUName,
		isnull(fsku.ColorName,'') +N'  '+isnull(fsku.SizeName,'') as ColorSize,
		dms.LastMonthQuantity,
		dms.LastMonthAmount,
		dms.ThisMonthArrivalQ,																	
		dms.ThisMonthPurchaseQ,																
		dms.ThisMonthPurchaseA,																	
		dms.ThisMonthSalesQ,																	
		dms.ThisMonthSalesA,																	
		dms.ThisMonthMoveInQ,																	
		dms.ThisMonthMoveOutQ,
		dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ as ThisMonthAnyQ,
		dms.ThisMonthAnyInA - dms.ThisMonthAnyOutA as ThisMonthAnyA,
		tmpdp.TotalPurchaseGaku,
		dms.ThisMonthQuantity,
		--dms.ThisMonthQuantity * dms.ThisMonthCost as Expense,
		dms.ThisMonthCost as Expense,
		dms.ThisMonthAmount,
		dms.LastMonthCost,
		dms.ThisMonthCost,
		dms.ThisMonthShippingQ,
		tmpdp.AveragePurchaseGaku
		from D_MonthlyStock dms
		left outer join #Temp tmpdp on dms.AdminNO = tmpdp.AdminNO and dms.SoukoCD = tmpdp.SoukoCD
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dms.AdminNO and fsku.DeleteFlg=0
		left outer join F_Souko(@targetdate) fsouko on fsouko.SoukoCD = dms.SoukoCD and fsouko.DeleteFlg=0
		where dms.YYYYMM = @YYYYMM
		and dms.SoukoCD = @soukoCD
		and fsku.ITemCD in
		(
			select ITemCD from M_SKU where ITemCD in
				(
					select ITemCD from D_PurchaseDetails dpd
					left outer join F_SKU(@targetdate) fsku on dpd.AdminNO = fsku.AdminNO
					where (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
					and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
					and (@sku is null or (dpd.SKUCD in (select * from SplitString(@sku,','))))
					and (@jan is null or(dpd.JanCD in (select * from SplitString(@jan,','))))
					and (@itemName is null or(dpd.ItemName like '%' + @itemName + '%'))
				)
		)
		Order by dms.SoukoCD,dms.SKUCD,dms.JanCD  --ses
	end
else if @related = 2 --maker
	begin 
		insert into #Temp
		select 
		dpd.AdminNO,
		da.SoukoCD,
		isnull(sum(dpd.TotalPurchaseGaku/nullif(dpd.PurchaseSu,0)),0) as AveragePurchaseGaku,
		isnull(sum(dpd.TotalPurchaseGaku),0) as TotalPurchaseGaku
		from D_Purchase dp
		left outer join D_PurchaseDetails dpd on dp.PurchaseNO = dpd.PurchaseNO
		left outer join D_Arrival da on da.ArrivalNO = dpd.ArrivalNO
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dpd.AdminNO and fsku.DeleteFlg=0
		Where dp.DeleteDateTime is null
		and dpd.DeleteDateTime is null
		and da.DeleteDateTime is null
		and dp.ProcessKBN=4
		and PurchaseDate>=@purchaseStartDate
		and PurchaseDate<=@purchaseEndDate
		and da.SoukoCD=@soukoCD
		and fsku.AdminNO in
		(
			select AdminNO from M_SKU where MakerItem in
				(
					select fsku.MakerItem from D_PurchaseDetails dpd
					left outer join F_SKU(@targetdate) fsku on dpd.AdminNO = fsku.AdminNO
					where (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
					and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
					and (@sku is null or (dpd.SKUCD in (select * from SplitString(@sku,','))))
					and (@jan is null or(dpd.JanCD in (select * from SplitString(@jan,','))))
					and (@itemName is null or(dpd.ItemName like '%' + @itemName + '%'))
				)
		)
		group by dpd.AdminNO,da.SoukoCD	

		select 
		CONVERT(varchar(10), GETDATE(),111) AS Today,
		dms.YYYYMM,
		dms.SoukoCD,
		fsouko.SoukoName,
		dms.SKUCD,
		dms.JanCD,
		fsku.SKUName,
		isnull(fsku.ColorName,'') +N'  '+isnull(fsku.SizeName,'') as ColorSize,
		dms.LastMonthQuantity,
		dms.LastMonthAmount,
		dms.ThisMonthArrivalQ,																	
		dms.ThisMonthPurchaseQ,																
		dms.ThisMonthPurchaseA,																	
		dms.ThisMonthSalesQ,																	
		dms.ThisMonthSalesA,																	
		dms.ThisMonthMoveInQ,																	
		dms.ThisMonthMoveOutQ,
		dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ as ThisMonthAnyQ,
		dms.ThisMonthAnyInA - dms.ThisMonthAnyOutA as ThisMonthAnyA,
		tmpdp.TotalPurchaseGaku,
		dms.ThisMonthQuantity,
		--dms.ThisMonthQuantity * dms.ThisMonthCost as Expense,
		dms.ThisMonthCost as Expense,
		dms.ThisMonthAmount,
		dms.LastMonthCost,
		dms.ThisMonthCost,
		dms.ThisMonthShippingQ,
		tmpdp.AveragePurchaseGaku,
		dms.ThisMonthShippingQ
		from D_MonthlyStock dms
		left outer join #Temp tmpdp on dms.AdminNO = tmpdp.AdminNO and dms.SoukoCD = tmpdp.SoukoCD
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dms.AdminNO and fsku.DeleteFlg = 0 
		left outer join F_Souko(@targetdate) fsouko on fsouko.SoukoCD = dms.SoukoCD  and fsouko.DeleteFlg = 0
		where dms.YYYYMM = @YYYYMM
		and dms.SoukoCD = @soukoCD
		and dms.AdminNO in
		(
			select AdminNO from M_SKU where MakerItem in
				(
					select fsku.MakerItem from D_PurchaseDetails dpd
					left outer join F_SKU(@targetdate) fsku on dpd.AdminNO = fsku.AdminNO
					where (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
					and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
					and (@sku is null or (dpd.SKUCD in (select * from SplitString(@sku,','))))
					and (@jan is null or(dpd.JanCD in (select * from SplitString(@jan,','))))
					and (@itemName is null or(dpd.ItemName like '%' + @itemName + '%'))
				)
		)
		Order by dms.SoukoCD,dms.SKUCD,dms.JanCD
	end
else if @related = 3 -- normal
	begin
		insert into #Temp
		select 
		dpd.AdminNO,
		da.SoukoCD,
		isnull(sum(dpd.TotalPurchaseGaku/nullif(dpd.PurchaseSu,0)),0) as AveragePurchaseGaku,
		isnull(sum(dpd.TotalPurchaseGaku),0) as TotalPurchaseGaku
		from D_Purchase dp
		left outer join D_PurchaseDetails dpd on dp.PurchaseNO = dpd.PurchaseNO
		left outer join D_Arrival da on da.ArrivalNO = dpd.ArrivalNO
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dpd.AdminNO  and fsku.DeleteFlg=0
		Where dp.DeleteDateTime is null
		and dpd.DeleteDateTime is null
		and da.DeleteDateTime is null
		and dp.ProcessKBN=4
		and PurchaseDate>=@purchaseStartDate
		and PurchaseDate<=@purchaseEndDate
		and da.SoukoCD=@soukoCD
		AND (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
		and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
		and (@sku is null or (dpd.SKUCD in (select * from SplitString(@sku,','))))
		and (@jan is null or(dpd.JanCD in (select * from SplitString(@jan,','))))
		and (@itemName is null or(dpd.ItemName like '%' + @itemName + '%'))
		and dpd.AdminNO is not null--ssa
		group by dpd.AdminNO,da.SoukoCD	

		select 
		CONVERT(varchar(10), GETDATE(),111) AS Today,
		dms.YYYYMM,
		dms.SoukoCD,
		fsouko.SoukoName,
		dms.SKUCD,
		dms.JanCD,
		fsku.SKUName,
		isnull(fsku.ColorName,'') +N'  '+isnull(fsku.SizeName,'') as ColorSize,
		dms.LastMonthQuantity,
		dms.LastMonthAmount,
		dms.ThisMonthArrivalQ,																	
		dms.ThisMonthPurchaseQ,																
		dms.ThisMonthPurchaseA,																	
		dms.ThisMonthSalesQ,																	
		dms.ThisMonthSalesA,																	
		dms.ThisMonthMoveInQ,																	
		dms.ThisMonthMoveOutQ,
		dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ as ThisMonthAnyQ,
		dms.ThisMonthAnyInA - dms.ThisMonthAnyOutA as ThisMonthAnyA,
		tmpdp.TotalPurchaseGaku,
		dms.ThisMonthQuantity,
		--dms.ThisMonthQuantity * dms.ThisMonthCost as Expense,
		dms.ThisMonthCost as Expense,
		dms.ThisMonthAmount,
		dms.LastMonthCost,
		dms.ThisMonthCost,
		dms.ThisMonthShippingQ,
		tmpdp.AveragePurchaseGaku
		from D_MonthlyStock dms
		left outer join #Temp tmpdp on dms.AdminNO = tmpdp.AdminNO and dms.SoukoCD = tmpdp.SoukoCD
		left outer join F_SKU(@targetdate) fsku on fsku.AdminNO = dms.AdminNO  and fsku.DeleteFlg = 0 
		left outer join F_Souko(@targetdate) fsouko on fsouko.SoukoCD = dms.SoukoCD  and fsouko.DeleteFlg = 0
		where dms.YYYYMM = @YYYYMM
		and dms.SoukoCD = @soukoCD
		and (@makeritem is null or (fsku.MakerItem in (select * from SplitString(@makeritem,','))))
		and (@itemcd is null or(fsku.ITemCD in (select * from SplitString(@itemcd,','))))
		and (@sku is null or (dms.SKUCD in (select * from SplitString(@sku,','))))
		and (@jan is null or(dms.JanCD in (select * from SplitString(@jan,','))))
		and (@itemName is null or(fsku.SKUName like '%' + @itemName + '%'))
		Order by dms.SoukoCD,dms.SKUCD,dms.JanCD
	end

drop table #Temp
END



