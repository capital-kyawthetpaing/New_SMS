 BEGIN TRY 
 Drop Procedure dbo.[ZaikoKanriHyou_Export]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[ZaikoKanriHyou_Export]
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
		--CONVERT(varchar(10), GETDATE(),111) AS Today,
		--dms.YYYYMM,
		--dms.SoukoCD,
		--fsouko.SoukoName,
		dms.JanCD 'JANCD',
		dms.SKUCD 'SKUCD',
		fsku.SKUName '商品名',
		fsku.ColorName 'カラー',
		fsku.SizeName 'サイズ',
		format(dms.LastMonthQuantity, '#,##0') '前月会計残数',
		format(dms.LastMonthAmount, '#,##0') '前月会計残額',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残数',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残額',
		format(dms.ThisMonthArrivalQ, '#,##0') '発注入庫数',																	
		format(dms.ThisMonthPurchaseQ, '#,##0') '仕入数',																
		format(dms.ThisMonthPurchaseA, '#,##0') '仕入額',																	
		format(dms.ThisMonthSalesQ, '#,##0') '出庫数',																	
		format(dms.ThisMonthSalesA, '#,##0') '売上額',																	
		format(dms.ThisMonthMoveInQ , '#,##0')'店舗間入数',																	
		format(dms.ThisMonthMoveOutQ, '#,##0') '店舗間出数',
		format(dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ, '#,##0') as 'その他入出数',
		format(tmpdp.TotalPurchaseGaku , '#,##0')'MD金額',
		format(dms.ThisMonthQuantity, '#,##0') '当月会計残数',
		format(dms.ThisMonthAmount, '#,##0') '当月会計残額',
		format(dms.LastMonthCost, '#,##0') '前月評価単価',
		format(dms.ThisMonthCost, '#,##0') '当月評価単価',
		format(tmpdp.AveragePurchaseGaku , '#,##0')'MD単価',
		format(dms.ThisMonthInventry, '#,##0') '当月在庫残数', 
		format(dms.ThisMonthInventryAmount, '#,##0') '当月在庫残額',
		format(dms.ThisMonth01Q , '#,##0')'入荷数	',
		format(dms.ThisMonth01A , '#,##0') '入荷額',
		format(dms.ThisMonth01QB, '#,##0') '入荷仕入数',
		format(dms.ThisMonth01AB, '#,##0') '入荷仕入額',
		format(dms.ThisMonth30Q, '#,##0')  '仕入数',
		format(dms.ThisMonth30A, '#,##0')  '仕入額',
		format(dms.ThisMonth03Q, '#,##0')  '出荷売上数',
		format(dms.ThisMonth03A, '#,##0')  '出荷売上額',
		format(dms.ThisMonth11Q, '#,##0')  '倉庫移動入数',
		format(dms.ThisMonth11A, '#,##0')  '倉庫移動入額',

		format(dms.ThisMonth12Q, '#,##0')  '倉庫移動入取消数',
		format(dms.ThisMonth12A, '#,##0')  '倉庫移動入取消額',
		format(dms.ThisMonth13Q, '#,##0')  '倉庫移動出数',
		format(dms.ThisMonth13A, '#,##0')  '倉庫移動出額',
		format(dms.ThisMonth14Q, '#,##0')  '倉庫移動出取消数',
		format(dms.ThisMonth14A, '#,##0')   '倉庫移動出取消額',
		format(dms.ThisMonth41Q, '#,##0')   '店舗間移動入数',
		format(dms.ThisMonth41A, '#,##0')   '店舗間移動入額',
		format(dms.ThisMonth42Q, '#,##0')   '店舗間移動入取消数',
		format(dms.ThisMonth42A, '#,##0')   '店舗間移動入取消額',

		format(dms.ThisMonth43Q, '#,##0')   '店舗間移動出数',
		format(dms.ThisMonth43A, '#,##0')   '店舗間移動出額',
		format(dms.ThisMonth44Q, '#,##0')   '店舗間移動出取消数',
		format(dms.ThisMonth44A, '#,##0')   '店舗間移動出取消額',
		format(dms.ThisMonth16Q, '#,##0')   '返品対象出数',
		format(dms.ThisMonth16A, '#,##0')   '返品対象出額',
		format(dms.ThisMonth21Q, '#,##0')   '返品対象入数',
		format(dms.ThisMonth21A, '#,##0')   '返品対象入額	',
		format(dms.ThisMonth17Q, '#,##0')   '棚卸加算数',
		format(dms.ThisMonth17A, '#,##0')   '棚卸加算額',

		format(dms.ThisMonth18Q, '#,##0')   '棚卸減数',
		format(dms.ThisMonth18A, '#,##0')   '棚卸減額',
		format(dms.ThisMonth19Q, '#,##0')   '調整追加数',
		format(dms.ThisMonth19A, '#,##0')   '調整追加額',
		format(dms.ThisMonth20Q, '#,##0')   '調整削除数',
		format(dms.ThisMonth20A, '#,##0')   '調整削除額',
		format(dms.ThisMonth22Q, '#,##0')   '棚番変更数',
		format(dms.ThisMonth22A, '#,##0')   '棚番変更額',
		format(dms.ThisMonth25Q, '#,##0')   '店頭売上数',
		format(dms.ThisMonth25A, '#,##0')   '店頭売上額',

		format(dms.ThisMonth24Q, '#,##0')   '店頭変更数',
		format(dms.ThisMonth24A, '#,##0')   '店頭変更額',
		format(dms.ThisMonth23Q, '#,##0')   '店頭返品数',
		format(dms.ThisMonth23A, '#,##0')   '店頭返品額',
		format(dms.ThisMonth31Q, '#,##0')   '商品CD付替数',
		format(dms.ThisMonth31A, '#,##0')   '商品CD付替額',
		format(dms.ThisMonth32Q, '#,##0')   '商品CD付替取消数',
		format(dms.ThisMonth32A, '#,##0')   '商品CD付替取消額'
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
		dms.JanCD 'JANCD',
		dms.SKUCD 'SKUCD',
		fsku.SKUName '商品名',
		fsku.ColorName 'カラー',
		fsku.SizeName 'サイズ',
		format(dms.LastMonthQuantity, '#,##0') '前月会計残数',
		format(dms.LastMonthAmount, '#,##0') '前月会計残額',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残数',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残額',
		format(dms.ThisMonthArrivalQ, '#,##0') '発注入庫数',																	
		format(dms.ThisMonthPurchaseQ, '#,##0') '仕入数',																
		format(dms.ThisMonthPurchaseA, '#,##0') '仕入額',																	
		format(dms.ThisMonthSalesQ, '#,##0') '出庫数',																	
		format(dms.ThisMonthSalesA, '#,##0') '売上額',																	
		format(dms.ThisMonthMoveInQ , '#,##0')'店舗間入数',																	
		format(dms.ThisMonthMoveOutQ, '#,##0') '店舗間出数',
		format(dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ, '#,##0') as 'その他入出数',
		format(tmpdp.TotalPurchaseGaku , '#,##0')'MD金額',
		format(dms.ThisMonthQuantity, '#,##0') '当月会計残数',
		format(dms.ThisMonthAmount, '#,##0') '当月会計残額',
		format(dms.LastMonthCost, '#,##0') '前月評価単価',
		format(dms.ThisMonthCost, '#,##0') '当月評価単価',
		format(tmpdp.AveragePurchaseGaku , '#,##0')'MD単価',
		format(dms.ThisMonthInventry, '#,##0') '当月在庫残数', 
		format(dms.ThisMonthInventryAmount, '#,##0') '当月在庫残額',
		format(dms.ThisMonth01Q , '#,##0')'入荷数	',
		format(dms.ThisMonth01A , '#,##0') '入荷額',
		format(dms.ThisMonth01QB, '#,##0') '入荷仕入数',
		format(dms.ThisMonth01AB, '#,##0') '入荷仕入額',
		format(dms.ThisMonth30Q, '#,##0')  '仕入数',
		format(dms.ThisMonth30A, '#,##0')  '仕入額',
		format(dms.ThisMonth03Q, '#,##0')  '出荷売上数',
		format(dms.ThisMonth03A, '#,##0')  '出荷売上額',
		format(dms.ThisMonth11Q, '#,##0')  '倉庫移動入数',
		format(dms.ThisMonth11A, '#,##0')  '倉庫移動入額',

		format(dms.ThisMonth12Q, '#,##0')  '倉庫移動入取消数',
		format(dms.ThisMonth12A, '#,##0')  '倉庫移動入取消額',
		format(dms.ThisMonth13Q, '#,##0')  '倉庫移動出数',
		format(dms.ThisMonth13A, '#,##0')  '倉庫移動出額',
		format(dms.ThisMonth14Q, '#,##0')  '倉庫移動出取消数',
		format(dms.ThisMonth14A, '#,##0')   '倉庫移動出取消額',
		format(dms.ThisMonth41Q, '#,##0')   '店舗間移動入数',
		format(dms.ThisMonth41A, '#,##0')   '店舗間移動入額',
		format(dms.ThisMonth42Q, '#,##0')   '店舗間移動入取消数',
		format(dms.ThisMonth42A, '#,##0')   '店舗間移動入取消額',

		format(dms.ThisMonth43Q, '#,##0')   '店舗間移動出数',
		format(dms.ThisMonth43A, '#,##0')   '店舗間移動出額',
		format(dms.ThisMonth44Q, '#,##0')   '店舗間移動出取消数',
		format(dms.ThisMonth44A, '#,##0')   '店舗間移動出取消額',
		format(dms.ThisMonth16Q, '#,##0')   '返品対象出数',
		format(dms.ThisMonth16A, '#,##0')   '返品対象出額',
		format(dms.ThisMonth21Q, '#,##0')   '返品対象入数',
		format(dms.ThisMonth21A, '#,##0')   '返品対象入額	',
		format(dms.ThisMonth17Q, '#,##0')   '棚卸加算数',
		format(dms.ThisMonth17A, '#,##0')   '棚卸加算額',

		format(dms.ThisMonth18Q, '#,##0')   '棚卸減数',
		format(dms.ThisMonth18A, '#,##0')   '棚卸減額',
		format(dms.ThisMonth19Q, '#,##0')   '調整追加数',
		format(dms.ThisMonth19A, '#,##0')   '調整追加額',
		format(dms.ThisMonth20Q, '#,##0')   '調整削除数',
		format(dms.ThisMonth20A, '#,##0')   '調整削除額',
		format(dms.ThisMonth22Q, '#,##0')   '棚番変更数',
		format(dms.ThisMonth22A, '#,##0')   '棚番変更額',
		format(dms.ThisMonth25Q, '#,##0')   '店頭売上数',
		format(dms.ThisMonth25A, '#,##0')   '店頭売上額',

		format(dms.ThisMonth24Q, '#,##0')   '店頭変更数',
		format(dms.ThisMonth24A, '#,##0')   '店頭変更額',
		format(dms.ThisMonth23Q, '#,##0')   '店頭返品数',
		format(dms.ThisMonth23A, '#,##0')   '店頭返品額',
		format(dms.ThisMonth31Q, '#,##0')   '商品CD付替数',
		format(dms.ThisMonth31A, '#,##0')   '商品CD付替額',
		format(dms.ThisMonth32Q, '#,##0')   '商品CD付替取消数',
		format(dms.ThisMonth32A, '#,##0')   '商品CD付替取消額'
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
		dms.JanCD 'JANCD',
		dms.SKUCD 'SKUCD',
		fsku.SKUName '商品名',
		fsku.ColorName 'カラー',
		fsku.SizeName 'サイズ',
		format(dms.LastMonthQuantity, '#,##0') '前月会計残数',
		format(dms.LastMonthAmount, '#,##0') '前月会計残額',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残数',
		format(dms.LastMonthInventry, '#,##0') '前月在庫残額',
		format(dms.ThisMonthArrivalQ, '#,##0') '発注入庫数',																	
		format(dms.ThisMonthPurchaseQ, '#,##0') '仕入数',																
		format(dms.ThisMonthPurchaseA, '#,##0') '仕入額',																	
		format(dms.ThisMonthSalesQ, '#,##0') '出庫数',																	
		format(dms.ThisMonthSalesA, '#,##0') '売上額',																	
		format(dms.ThisMonthMoveInQ , '#,##0')'店舗間入数',																	
		format(dms.ThisMonthMoveOutQ, '#,##0') '店舗間出数',
		format(dms.ThisMonthAnyInQ - dms.ThisMonthAnyOutQ, '#,##0') as 'その他入出数',
		format(tmpdp.TotalPurchaseGaku , '#,##0')'MD金額',
		format(dms.ThisMonthQuantity, '#,##0') '当月会計残数',
		format(dms.ThisMonthAmount, '#,##0') '当月会計残額',
		format(dms.LastMonthCost, '#,##0') '前月評価単価',
		format(dms.ThisMonthCost, '#,##0') '当月評価単価',
		format(tmpdp.AveragePurchaseGaku , '#,##0')'MD単価',
		format(dms.ThisMonthInventry, '#,##0') '当月在庫残数', 
		format(dms.ThisMonthInventryAmount, '#,##0') '当月在庫残額',
		format(dms.ThisMonth01Q , '#,##0')'入荷数	',
		format(dms.ThisMonth01A , '#,##0') '入荷額',
		format(dms.ThisMonth01QB, '#,##0') '入荷仕入数',
		format(dms.ThisMonth01AB, '#,##0') '入荷仕入額',
		format(dms.ThisMonth30Q, '#,##0')  '仕入数',
		format(dms.ThisMonth30A, '#,##0')  '仕入額',
		format(dms.ThisMonth03Q, '#,##0')  '出荷売上数',
		format(dms.ThisMonth03A, '#,##0')  '出荷売上額',
		format(dms.ThisMonth11Q, '#,##0')  '倉庫移動入数',
		format(dms.ThisMonth11A, '#,##0')  '倉庫移動入額',

		format(dms.ThisMonth12Q, '#,##0')  '倉庫移動入取消数',
		format(dms.ThisMonth12A, '#,##0')  '倉庫移動入取消額',
		format(dms.ThisMonth13Q, '#,##0')  '倉庫移動出数',
		format(dms.ThisMonth13A, '#,##0')  '倉庫移動出額',
		format(dms.ThisMonth14Q, '#,##0')  '倉庫移動出取消数',
		format(dms.ThisMonth14A, '#,##0')   '倉庫移動出取消額',
		format(dms.ThisMonth41Q, '#,##0')   '店舗間移動入数',
		format(dms.ThisMonth41A, '#,##0')   '店舗間移動入額',
		format(dms.ThisMonth42Q, '#,##0')   '店舗間移動入取消数',
		format(dms.ThisMonth42A, '#,##0')   '店舗間移動入取消額',

		format(dms.ThisMonth43Q, '#,##0')   '店舗間移動出数',
		format(dms.ThisMonth43A, '#,##0')   '店舗間移動出額',
		format(dms.ThisMonth44Q, '#,##0')   '店舗間移動出取消数',
		format(dms.ThisMonth44A, '#,##0')   '店舗間移動出取消額',
		format(dms.ThisMonth16Q, '#,##0')   '返品対象出数',
		format(dms.ThisMonth16A, '#,##0')   '返品対象出額',
		format(dms.ThisMonth21Q, '#,##0')   '返品対象入数',
		format(dms.ThisMonth21A, '#,##0')   '返品対象入額	',
		format(dms.ThisMonth17Q, '#,##0')   '棚卸加算数',
		format(dms.ThisMonth17A, '#,##0')   '棚卸加算額',

		format(dms.ThisMonth18Q, '#,##0')   '棚卸減数',
		format(dms.ThisMonth18A, '#,##0')   '棚卸減額',
		format(dms.ThisMonth19Q, '#,##0')   '調整追加数',
		format(dms.ThisMonth19A, '#,##0')   '調整追加額',
		format(dms.ThisMonth20Q, '#,##0')   '調整削除数',
		format(dms.ThisMonth20A, '#,##0')   '調整削除額',
		format(dms.ThisMonth22Q, '#,##0')   '棚番変更数',
		format(dms.ThisMonth22A, '#,##0')   '棚番変更額',
		format(dms.ThisMonth25Q, '#,##0')   '店頭売上数',
		format(dms.ThisMonth25A, '#,##0')   '店頭売上額',

		format(dms.ThisMonth24Q, '#,##0')   '店頭変更数',
		format(dms.ThisMonth24A, '#,##0')   '店頭変更額',
		format(dms.ThisMonth23Q, '#,##0')   '店頭返品数',
		format(dms.ThisMonth23A, '#,##0')   '店頭返品額',
		format(dms.ThisMonth31Q, '#,##0')   '商品CD付替数',
		format(dms.ThisMonth31A, '#,##0')   '商品CD付替額',
		format(dms.ThisMonth32Q, '#,##0')   '商品CD付替取消数',
		format(dms.ThisMonth32A, '#,##0')   '商品CD付替取消額'
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



