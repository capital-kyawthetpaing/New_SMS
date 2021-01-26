
 BEGIN TRY 
 Drop Procedure dbo.[M_ItemSelectoutput]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    MasterSuutsuroku_Item
--       Program ID      M_ItemSelectoutput
--       Create date:    2019.5.27
--    ======================================================================
Create PROCEDURE M_ItemSelectoutput

		@ChangeDate as date,
	@VendorCD as varchar(13),
	@makerCD as varchar(13),
	@brandCD as varchar(6),
	@SKUName as varchar(100),
	@JanCD as varchar(13),
	@SkuCD as varchar(30),
	@MakerItem as varchar(50),
	@ItemCD as varchar(30),
	@CommentInStore as varchar(300),
	@ReserveCD as varchar(3),
	@NticesCD as varchar(3),
	@PostageCD as varchar(3),
	@OrderAttentionCD as varchar(3),
	@SportsCD as varchar(6),
	@InsertDateTimeT as datetime,
	@InsertDateTimeF as datetime,
	@UpdateDateTimeT as datetime,
	@UpdateDateTimeF as datetime,
	@ApprovalDateF as date,
	@ApprovalDateT as date,
	@YearTerm as varchar(6),
	@Season as varchar(6),
	@CatalogNo as varchar(20),
	@InstructionsNO as varchar(1000),
	@TagName1 as varchar(20),
	@TagName2 as varchar(20),
	@TagName3 as varchar(20),
	@TagName4 as varchar(20),
	@TagName5 as varchar(20),
	@type as tinyint,
	@chktype as tinyint,

	@SoukoCD as varchar(6),
	@RackNoF as varchar(11),
	@RackNoT as varchar(11),
	@chkUnapprove as tinyint
					, @InsertOperator    as varchar(50)
					,@Program         as varchar(50)
					,@PC              as varchar(50)
					,@OperateMode     as varchar(50)
					,@KeyItem as varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	--	@ChangeDate as date='2021-01-25',
	--@VendorCD as varchar(13)='200060',
	--@makerCD as varchar(13),
	--@brandCD as varchar(6),
	--@SKUName as varchar(100),
	--@JanCD as varchar(13),
	--@SkuCD as varchar(30),
	--@MakerItem as varchar(50),
	--@ItemCD as varchar(30),
	--@CommentInStore as varchar(300),
	--@ReserveCD as varchar(3)='-1',
	--@NticesCD as varchar(3)='-1',
	--@PostageCD as varchar(3)='-1',
	--@OrderAttentionCD as varchar(3)='-1',
	--@SportsCD as varchar(6),
	--@InsertDateTimeT as datetime,
	--@InsertDateTimeF as datetime,
	--@UpdateDateTimeT as datetime,
	--@UpdateDateTimeF as datetime,
	--@ApprovalDateF as date,
	--@ApprovalDateT as date,
	--@YearTerm as varchar(6)='-1',
	--@Season as varchar(6)='-1',
	--@CatalogNo as varchar(20),
	--@InstructionsNO as varchar(1000),
	--@TagName1 as varchar(20),
	--@TagName2 as varchar(20),
	--@TagName3 as varchar(20),
	--@TagName4 as varchar(20),
	--@TagName5 as varchar(20),
	--@type as tinyint='3',
	--@chktype as tinyint='0',

	--@SoukoCD as varchar(6)='000000',
	--@RackNoF as varchar(11),
	--@RackNoT as varchar(11),
	--@chkUnapprove as tinyint='0'
	--
	SET NOCOUNT ON;
		 IF (@SoukoCD = -1)
		SET @SoukoCD = null

	 IF (@YearTerm = -1)
		SET @YearTerm = null

	IF (@Season = -1)
		SET @Season = null

		IF (@ReserveCD = -1)
		SET @ReserveCD = null

	IF (@NticesCD = -1)
		SET @NticesCD = null

	IF (@PostageCD = -1)
		SET @PostageCD = null

	IF (@OrderAttentionCD = -1)
		SET @OrderAttentionCD = null

    
		select
		distinct
		'データ区分' as 'データ区分'
		,fim.ITemCD as ItemCD
		,convert(varchar, fim.ChangeDate, 111) as '改定日'
		--,fim.ChangeDate as '改定日'
		,convert(varchar, fim.ApprovalDate, 111) as '承認日'
		--,fim.ApprovalDate as '承認日'
		, COnvert(int,fim.DeleteFlg ) as '削除'
		,Cast (fim.VariousFLG as int) as '諸口区分'
		,fim.ITemName as '商品名'
		,fim.KanaName as 'カナ名'
		,fim.ITEMShortName as '略名'
		,fim.EnglishName as '英語名'
		,fim.MainVendorCD as '主要仕入先CD'
		,(select top 1 VendorName  from  F_Vendor(getdate()) fv where fv.VendorCD =@VendorCD  )  as '主要仕入先名' 
		,fim.BrandCD as 'ブランドCD'
		,(select top 1 BrandName  from   M_Brand mb where mb.BrandCD =@brandCD  )  as 'ブランド名' 
		,fim.makerItem as 'メーカー商品CD'
		,(fim.SizeCount ) as '展開サイズ数'
		,fim.ColorCount as '展開カラー数'
		,fim.TaniCD as '単位CD'
		, (select top 1 LEFT (Char1, 20) from M_MultiPorpose mp where mp.[ID] ='201') as '単位名'
		,fim.SportsCD as  '競技CD'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='202') as '競技名'
		,fim.SegmentCD as '商品分類CD'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='203') as '分類名'
		,fim._ExhibitionSegmentCD as 'セグメントCD'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='206') as 'セグメント名'
		,fim.SetKBN as  'セット品区分'
		, (case when fim.SetKBN ='1' then 'セット' else null end) as 'セット品区分名'
		,fim.PresentKBN as  'プレゼント品区分'
		, (case when fim.PresentKBN ='1' then 'プレゼント' else null end) as 'プレゼント品区分名'
		,fim.SampleKBN as 'サンプル品区分'
		, (case when fim.SampleKBN ='1' then 'サンプル' else null end) as 'サンプル品区分名'
		,fim.DiscountKBN as '値引商品区分'
		, (case when fim.DiscountKBN ='1' then '値引' else null end) as '値引商品区分名'
		,fim.WebFlg as 'Webストア取扱区分'
		, (case when fim.WebFlg ='1' then '対象' else null end) as 'Webストア取扱区分名'
		,fim.RealStoreFlg as '実店舗取扱区分'
		, (case when fim.RealStoreFlg ='1' then '対象' else null end) as '実店舗取扱区分名'
		,fim.ZaikoKBN as '在庫管理対象区分'
		, (case when fim.ZaikoKBN ='1' then '対象' else null end) as '在庫管理対象区分名'
		,fim.VirtualFlg as '架空商品区分'
		, (case when fim.VirtualFlg ='1' then '架空' else null end) as '架空商品区分名'
		,fim.DirectFlg as '直送品区分'
		, (case when fim.DirectFlg ='1' then '直送' else null end) as '直送品区分名'
		,fim.ReserveCD as '予約品区分'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='311') as '予約品区分名'
		,fim.NoticesCD as '特記区分'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='310') as '特記区分名'
		,fim.PostageCD as '送料区分'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='309') as '送料区分名'
		,fim.ManufactCD as '要加工品区分'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='312') as '要加工品区分名'
		,fim.ConfirmCD as '要確認品区分'
		, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='313') as '要確認品区分名'
		,fim.WebStockFlg as 'Web在庫連携区分'
		, (case when fim.WebStockFlg ='1' then '対象' else null end) as 'Web在庫連携区分名'
		,fim.StopFlg as '販売停止品区分'
		, (case when fim.StopFlg ='1' then '停止' else null end) as '販売停止品区分名'
		,fim.DiscontinueFlg as '廃番品区分'
		, (case when fim.DiscontinueFlg ='1' then '廃番' else null end) as '廃番品区分名'
		,fim._SoldOutflg as '完売品区分'
		, (case when fim._SoldOutflg =1 then '完売' else null end) as '完売品区分名'	
		,fim.InventoryAddFlg as '自社在庫連携対象'
		, (case when fim.InventoryAddFlg =1 then'対象' else null end) as '自社在庫連携対象名'
		,fim.MakerAddFlg as 'メーカー在庫連携対象'
		, (case when fim.MakerAddFlg ='1' then '対象' else null end) as 'メーカー在庫連携対象名'
		,fim.StoreAddFlg as '店舗在庫連携対象'
		, (case when fim.StoreAddFlg ='1' then '対象' else null end) as '店舗在庫連携対象名'
		,fim.NoNetOrderFlg as 'Net発注不可区分'
		, (case when fim.NoNetOrderFlg ='1' then '不可' else null end) as 'Net発注不可区分名'
		,fim.EDIOrderFlg as 'EDI発注可能区分'
		, (case when fim.EDIOrderFlg ='1' then '可能' else null end) as 'EDI発注可能区分名'
		,fim.AutoOrderFlg as '自動発注対象区分'
		, (case when fim.AutoOrderFlg ='1' then '対象' else null end) as '自動発注対象名'

		
		,fim.CatalogFlg as 'カタログ掲載有無'
		, (case when fim.CatalogFlg ='1' then '”あり' else null end) as 'カタログ掲載有無名'
		,fim.ParcelFlg as '小包梱包可能区分'
		, (case when fim.ParcelFlg ='1' then '可能' else null end) as '小包梱包可能名'
		, cast(fim.TaxRateFLG as int) as '税率区分'
		, (case when fim.TaxRateFLG =1 then '通常課税' 
		when fim.TaxRateFLG =2 then '軽減課税'
		else '非課税' end ) as '税率区分名'



		 , cast(fim.CostingKBN as int) as '原価計算方法'
		 , (case when fim.CostingKBN =1 then '標準原価' else '総平均原価' end) as '原価計算方法名'
		 , Cast (fim.SaleExcludedFlg as int) as 'Sale対象外区分'
		 ,(case when fim.SaleExcludedFlg =1 then '対象外' else null end) as 'Sale対象外区分名'
		,Cast(fim._NormalCost as int) as '標準原価'
	,Cast(fim.PriceWithTax as int) as '税込定価'
	,Cast(fim.PriceOutTax as int) as '税抜定価'
	,Cast(fim.OrderPriceWithTax as int) as '発注税込価格'
	,Cast(fim.OrderPriceWithoutTax as int)as '発注税抜価格'
	,Cast(fim.Rate as int) as '掛率'
	,convert(varchar, fim.SaleStartDate, 111) as '発売開始日'
	,convert(varchar, fim.WebStartDate, 111) as 'Web掲載開始日'
	,fim.OrderAttentionCD as '発注注意区分'
	, (select top 1 LEFT (Char1, 20)  from M_MultiPorpose mp where mp.[ID] ='319') as '発注注意区分名'
	,fim.OrderAttentionNote  as '発注注意事項'
	,fim.CommentInStore as '管理用備考'
	,fim.CommentOutStore as '表示用備考'
	,fim.Rack as '棚番'

	,fim.LastYearTerm as '年度'
	,fim.LastSeason as 'シーズン'
	,fim.LastCatalogNO as 'カタログ番号'
	,fim.LastCatalogPage as 'カタログページ'
	--,'fim.LastCatalogNOLong' as LastCatalogNOLong
	--,'fim.LastCatalogPageLong' as Lastcataloglong
	,fim.LastCatalogText as 'カタログ情報'
	,fim.LastInstructionsNO as '指示書番号'
	,convert(varchar, fim.LastInstructionsDate, 111) as '指示書発行日'
	,fim.WebAddress as '商品情報アドレス'
	, fim._orderlot as '発注ロット'
	,fim.TagName01 as 'ITEMタグ1'
	,fim.TagName02 as 'ITEMタグ2'
	,fim.TagName03 as 'ITEMタグ3'
	,fim.TagName04 as 'ITEMタグ4'
	,fim.TagName05 as 'ITEMタグ5'
	,fim.TagName06 as 'ITEMタグ6'
	,fim.TagName07 as 'ITEMタグ7'
	,fim.TagName08 as 'ITEMタグ8'
	,fim.TagName09 as 'ITEMタグ9'
	,fim.TagName10 as 'ITEMタグ10'
	
		from (
select msku.ITemCD, msku.AdminNO
					From	F_SKU(@ChangeDate) as msku
					Left outer join M_SKUInfo as msInfo on msku.AdminNO=msInfo.AdminNO
														and msku.ChangeDate = msInfo.ChangeDate 
														and msku.DeleteFlg=0
					Left outer join  M_SKUTag as mst on msku.AdminNO=mst.AdminNo
														and msku.ChangeDate =mst.ChangeDate
					--left outer join F_Vendor(gedate()) where  
					where 
					 (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem IN (Select * from SplitString(@MakerItem,','))))
					and (@ItemCD is Null or (msku.ITemCD  IN (Select * from SplitString(@ItemCD,','))))
					and (@CommentInStore is Null or(msku.CommentInStore  Like '%'  +@CommentInStore+ '%' ))
					and (@ReserveCD is Null or ( msku.ReserveCD=@ReserveCD))
					and (@NticesCD is Null or (msku.NoticesCD=@NticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttentionCD is Null or (msku.OrderAttentionCD=@OrderAttentionCD))
					and (@SportsCD is Null or (msku.SportsCD=@SportsCD))
					and (@InsertDateTimeF is Null or (msku.InsertDateTime >= @InsertDateTimeF))
					and (@InsertDateTimeT is Null or (msku.InsertDateTime <= @InsertDateTimeT))
					and (@UpdateDateTimeT is Null or (msku.UpdateDateTime >= @UpdateDateTimeT))
					and (@UpdateDateTimeF is Null or (msku.UpdateDateTime <= @UpdateDateTimeF))
					and ((((@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
							and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))) ) or 
							(@chkUnapprove = 1 and msku.ApprovalDate is null)
							) 
					and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and  (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@TagName1 is Null or  TagName =@TagName1)
					and (@TagName2 is Null or  TagName =@TagName2)
					and (@TagName3 is Null or  TagName =@TagName3)
					and (@TagName4 is Null or  TagName =@TagName4)
					and (@TagName5 is Null or  TagName =@TagName5) )  Main left Outer join 

					 F_ITEM(GETDATE()) fim on Main.ITemCD = fim.ITemCD
					 Order by fim.ITemCD asc

 -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			EXEC L_Log_Insert
					 @InsertOperator  
					,@Program        
					,@PC             
					,@OperateMode    
					,@KeyItem
END

