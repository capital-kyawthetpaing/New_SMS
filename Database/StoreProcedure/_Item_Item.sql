
 BEGIN TRY 
 Drop Procedure dbo.[_Item_Item]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[_Item_Item]
	-- Add the parameters for the stored procedure here
	  @xml as xml 
	 ,@opt as varchar(20)
	 ,@Date as datetime
	 ,@MainFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	Declare @DocHandle int,
	@_Date datetime =CONVERT(varchar,getdate(),120),
	--@Opt as varchar(10) = '0001',
	@Upt as int =1,
	@Ins as int = 1;

			create table [dbo].#tempItem
			(
			
				 ITemCD				varchar(100)												
				,ChangeDate			date														
				,ApprovalDate		date															
				,DeleteFlg			int														
				,VariousFLG			int														
				,ITemName			varchar(100) collate Japanese_CI_AS 														
				,KanaName			varchar(100) collate Japanese_CI_AS														
				,ITEMShortName		varchar(100) collate Japanese_CI_AS															
				,EnglishName			varchar(100) collate Japanese_CI_AS														
				,MainVendorCD		varchar(100) collate Japanese_CI_AS															
				,VendorName			varchar(100) collate Japanese_CI_AS														
				,BrandCD				varchar(100) collate Japanese_CI_AS													
				,BrandName			varchar(100) collate Japanese_CI_AS														
				,MakerItem			varchar(100) collate Japanese_CI_AS														
				,SizeCount			int														
				,ColorCount			int														
				,TaniCD				varchar(100) collate Japanese_CI_AS
				,TaniName			varchar(100) collate Japanese_CI_AS
				,SportsCD			varchar(100) collate Japanese_CI_AS
				,SportsName		 	varchar(100) collate Japanese_CI_AS																
				,SegmentCD			varchar(100) collate Japanese_CI_AS														
				,SegmentCDName 		varchar(100) collate Japanese_CI_AS															
				,ExhibitionSegmentCD	varchar(100) collate Japanese_CI_AS																
				,ExhibitionSegmentCDName varchar(100) collate Japanese_CI_AS																
				,SetKBN				varchar(100) collate Japanese_CI_AS													
				,SetKBNName			varchar(100) collate Japanese_CI_AS											
				,PresentKBN			varchar(100) collate Japanese_CI_AS		 								
				,PresentKBNName		varchar(100) collate Japanese_CI_AS			 					
				,SampleKBN			varchar(100) collate Japanese_CI_AS		 			
				,SampleKBNName		varchar(100) collate Japanese_CI_AS		 					
				,DiscountKBN			varchar(100) collate Japanese_CI_AS		 			
				,DiscountKBNName		varchar(100) collate Japanese_CI_AS		 				
				,WebFlg				varchar(100) collate Japanese_CI_AS		 		
				,WebFlgName			varchar(100) collate Japanese_CI_AS			 			
				,RealStoreFlg		varchar(100) collate Japanese_CI_AS		 				
				,RealStoreFlgName	varchar(100) collate Japanese_CI_AS			 					
				,ZaikoKBN			varchar(100) collate Japanese_CI_AS		 			
				,ZaikoKBNName		varchar(100) collate Japanese_CI_AS			 				
				,VirtualFlg			varchar(100) collate Japanese_CI_AS		 			
				,VirtualFlgName		varchar(100) collate Japanese_CI_AS			 				
				,DirectFlg			varchar(100) collate Japanese_CI_AS		 			
				,DirectFlgName		varchar(100) collate Japanese_CI_AS		 
				,ReserveCD			varchar(100) collate Japanese_CI_AS	 
				,ReserveCDName 		varchar(100) collate Japanese_CI_AS 
				,NoticesCD			varchar(100) collate Japanese_CI_AS	 
				,NoticesCDName 		varchar(100) collate Japanese_CI_AS
				,PostageCD			varchar(100) collate Japanese_CI_AS	 
				,PostageCDName  		varchar(100) collate Japanese_CI_AS
				,ManufactCD			varchar(100) collate Japanese_CI_AS	 
				,ManufactCDName  	varchar(100) collate Japanese_CI_AS	 
				,ConfirmCD			varchar(100) collate Japanese_CI_AS	 
				,ConfirmCDName  		varchar(100) collate Japanese_CI_AS
				,WebStockFlg			varchar(100) collate Japanese_CI_AS	 
				,WebStockFlgName		varchar(100) collate Japanese_CI_AS	 
				,StopFlg				varchar(100) collate Japanese_CI_AS	 
				,StopFlgName			varchar(100) collate Japanese_CI_AS		 	 					
				,DiscontinueFlg		varchar(100) collate Japanese_CI_AS			 
				,DiscontinueFlgName	varchar(100) collate Japanese_CI_AS				 
				,SoldOutFlg			varchar(100) collate Japanese_CI_AS			 
				,SoldOutFlgName		varchar(100) collate Japanese_CI_AS				 
				,InventoryAddFlg		varchar(100) collate Japanese_CI_AS			 
				,InventoryAddFlgName	varchar(100) collate Japanese_CI_AS				 
				,MakerAddFlg			varchar(100) collate Japanese_CI_AS			 
				,MakerAddFlgName		varchar(100) collate Japanese_CI_AS				 
				,StoreAddFlg			varchar(100) collate Japanese_CI_AS			 
				,StoreAddFlgName		varchar(100) collate Japanese_CI_AS				 
				,NoNetOrderFlg		varchar(100) collate Japanese_CI_AS			 
				,NoNetOrderFlgName	varchar(100) collate Japanese_CI_AS				 
				,EDIOrderFlg			varchar(100) collate Japanese_CI_AS			 
				,EDIOrderFlgName		varchar(100) collate Japanese_CI_AS				  					
				,AutoOrderFlg		varchar(100) collate Japanese_CI_AS															
				,AutoOrderFlgName	varchar(100) collate Japanese_CI_AS		 	
				,CatalogFlg			varchar(100) collate Japanese_CI_AS	 
				,CatalogFlgName		varchar(100) collate Japanese_CI_AS		 
				,ParcelFlg			varchar(100) collate Japanese_CI_AS	 
				,ParcelFlgName		varchar(100) collate Japanese_CI_AS		 
				,TaxRateFLG			varchar(100) collate Japanese_CI_AS	 
				,TaxRateFLGName		varchar(100) collate Japanese_CI_AS	 
				,CostingKBN			varchar(100) collate Japanese_CI_AS	 
				,CostingKBNName		varchar(100) collate Japanese_CI_AS		 	
				,SaleExcludedFlg		varchar(100) collate Japanese_CI_AS															
				,SaleExcludedFlgName	varchar(100) collate Japanese_CI_AS					 				
				,NormalCost			int														
				,PriceWithTax		money															
				,PriceOutTax			money														
				,OrderPriceWithTax	money																
				,OrderPriceWithoutTax money																	
				,Rate				decimal(5,2)													
				,SaleStartDate		date															
				,WebStartDate		date															
				,OrderAttentionCD	varchar(100) collate Japanese_CI_AS																	
				,OrderAttentionCDName varchar(100) collate Japanese_CI_AS																		
				,OrderAttentionNote	varchar(100) collate Japanese_CI_AS																	
				,CommentInStore		varchar(200) collate Japanese_CI_AS																
				,CommentOutStore		varchar(200) collate Japanese_CI_AS																
				,Rack				varchar(100) collate Japanese_CI_AS														
				,LastYearTerm		varchar(100) collate Japanese_CI_AS																
				,LastSeason			varchar(100) collate Japanese_CI_AS															
				,LastCatalogNO		varchar(100) collate Japanese_CI_AS																
				,LastCatalogPage		varchar(100) collate Japanese_CI_AS																
				,LastCatalogText		varchar(1000) collate Japanese_CI_AS																
				,LastInstructionsNO	varchar(100) collate Japanese_CI_AS																	
				,LastInstructionsDate date																
				,WebAddress			varchar(200) collate Japanese_CI_AS															
				,OrderLot			varchar(100) collate Japanese_CI_AS															
				,TagName01			varchar(100) collate Japanese_CI_AS															
				,TagName02			varchar(100) collate Japanese_CI_AS															
				,TagName03			varchar(100) collate Japanese_CI_AS															
				,TagName04			varchar(100) collate Japanese_CI_AS															
				,TagName05			varchar(100) collate Japanese_CI_AS															
				,TagName06			varchar(100) collate Japanese_CI_AS															
				,TagName07			varchar(100) collate Japanese_CI_AS															
				,TagName08			varchar(100) collate Japanese_CI_AS															
				,TagName09			varchar(100) collate Japanese_CI_AS															
				,TagName10			varchar(100) collate Japanese_CI_AS	
			)
			
--			SELECT COUNT(*) 
--FROM INFORMATION_SCHEMA.COLUMNS 
--WHERE 
--   TABLE_NAME = '#tempItem';
--			select *  from #tempItem

exec sp_xml_preparedocument @DocHandle output, @xml
insert into #tempItem
	select * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
 				 ITEMCD	varchar(100)	
				,改定日					varchar(100)	
				,承認日					varchar(100)	
				,削除					varchar(100)			
				,諸口区分				varchar(100)			   
				,商品名					varchar(100)		
				,カナ名					varchar(100)	
				,略名					varchar(100)	
				,英語名					varchar(100)	
				,主要仕入先CD			varchar(100)	
				,主要仕入先名				varchar(100)	
				,ブランドCD				varchar(100)	
				,ブランド名				varchar(100)	
				,メーカー商品CD			varchar(100)	
				,展開サイズ数				varchar(100)	
				,展開カラー数				varchar(100)	
				,単位CD					varchar(100)	
				,単位名					varchar(100)	
				,競技CD					varchar(100)	
				,競技名					varchar(100)	
				,商品分類CD				varchar(100)	
				,分類名					varchar(100)	
				,セグメントCD			varchar(100)	
				,セグメント名				varchar(100)	
				,セット品区分				varchar(100)	
				,セット品区分名			varchar(100)	
				,プレゼント品区分			varchar(100)
				,プレゼント品区分名   varchar(100)
				,サンプル品区分 varchar(100)
				,サンプル品区分名    varchar(100)
				,値引商品区分  varchar(100)
				,値引商品区分名 varchar(100)
				,Webストア取扱区分  varchar(100)
				,Webストア取扱区分名 varchar(100)
				,実店舗取扱区分 varchar(100)
				,実店舗取扱区分名    varchar(100)
				,在庫管理対象区分    varchar(100)
				,在庫管理対象区分名   varchar(100)
				,架空商品区分  varchar(100)
				,架空商品区分名 varchar(100)
				,直送品区分  varchar(100)
				,直送品区分名  varchar(100)
				,予約品区分   varchar(100)
				,予約品区分名  varchar(100)
				,特記区分    varchar(100)
				,特記区分名   varchar(100)
				,送料区分    varchar(100)
				,送料区分名   varchar(100)
				,要加工品区分  varchar(100)
				,要加工品区分名 varchar(100)
				,要確認品区分  varchar(100)
				,要確認品区分名 varchar(100)
				,Web在庫連携区分   varchar(100)
				,Web在庫連携区分名  varchar(100)
				,販売停止品区分 varchar(100)
				,販売停止品区分名  varchar(100)  
				,廃番品区分   varchar(100)
				,廃番品区分名  varchar(100)
				,完売品区分   varchar(100)
				,完売品区分名  varchar(100)
				,自社在庫連携対象    varchar(100)
				,自社在庫連携対象名   varchar(100)
				,メーカー在庫連携対象 varchar(100)
				,メーカー在庫連携対象名 varchar(100)
				,店舗在庫連携対象    varchar(100)
				,店舗在庫連携対象名   varchar(100)
				,Net発注不可区分   varchar(100)
				,Net発注不可区分名  varchar(100)
				,EDI発注可能区分   varchar(100)
				,EDI発注可能区分名 varchar(100)
				, 自動発注対象区分 varchar(100)
				, 自動発注対象名	varchar(100)
				, カタログ掲載有無  varchar(100)
				, カタログ掲載有無名 varchar(100)
				,小包梱包可能区分 varchar(100)
				, 小包梱包可能名 varchar(100)
				, 税率区分    varchar(100)
				,税率区分名   varchar(100)
				,原価計算方法  varchar(100)
				,原価計算方法名 varchar(100)
				,Sale対象外区分   varchar(100)
				,Sale対象外区分名  varchar(100)
				,標準原価    varchar(100)
				,税込定価    varchar(100)
				,税抜定価    varchar(100)
				,発注税込価格  varchar(100)
				,発注税抜価格  varchar(100)
				,掛率  varchar(100)
				,発売開始日  varchar(100)
				,Web掲載開始日    varchar(100)
				,発注注意区分 varchar(100)
				,発注注意区分名 varchar(100)
				,発注注意事項   varchar(100)
				,管理用備考   varchar(200)
				,表示用備考   varchar(200)
				,棚番  varchar(100)
				,年度  varchar(100)
				,シーズン   varchar(100)
				, カタログ番号  varchar(100)
				,カタログページ   varchar(100)
				,カタログ情報  varchar(1000)
				,指示書番号 varchar(100)
				,指示書発行日 varchar(100)
				,商品情報アドレス varchar(200)
				,発注ロット varchar(100)
				,ITEMタグ1 varchar(100)
				,ITEMタグ2 varchar(100)
				,ITEMタグ3 varchar(100)
				,ITEMタグ4 varchar(100)
				,ITEMタグ5 varchar(100)
				,ITEMタグ6 varchar(100)
				,ITEMタグ7 varchar(100)
				,ITEMタグ8 varchar(100)
				,ITEMタグ9 varchar(100)
				,ITEMタグ10 varchar(100)

			)
			exec sp_xml_removedocument @DocHandle;
			
			 WITH cte AS ( SELECT  ROW_NUMBER() OVER (PARTITION BY ItemCD,ChangeDate ORDER BY ItemCD,ChangeDate ) row_num FROM  #tempItem) delete FROM cte WHERE row_num > 1;
			 Update #tempItem set  TaniCD= null where TaniCD= ''
			--select *  from #tempItem

			--Merge [M_item] mi using [
			if (@MainFlg =1)
			Begin
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then @_Date else null end )	as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			 targ.VariousFLG									 =src.VariousFLG							
			,targ.ITemName									 =src.ITemName							
			,targ.KanaName									 =src.KanaName							
			,targ.ITEMShortName								 =src.ITEMShortName							
			,targ.EnglishName								 =src.EnglishName							
			,targ.SetKBN									 =src.SetKBN							
			,targ.PresentKBN								 =src.PresentKBN							
			,targ.SampleKBN									 =src.SampleKBN							
			,targ.DiscountKBN								 =src.DiscountKBN							
			,targ.SizeCount									 =src.SizeCount							
			,targ.ColorCount								 =src.ColorCount							
			,targ.SizeName									 =src.SizeName							
			,targ.ColorName									 =src.ColorName							
			,targ.WebFlg									 =src.WebFlg							
			,targ.RealStoreFlg								 =src.RealStoreFlg							
			,targ.MainVendorCD								 =src.MainVendorCD							
			,targ.MakerVendorCD								 =src.MakerVendorCD						
			,targ.BrandCD									 =src.BrandCD							
			,targ.MakerItem									 =src.MakerItem							
			,targ.TaniCD									 =src.TaniCD							
			,targ.SportsCD									 =src.SportsCD							
			,targ.SegmentCD									 =src.SegmentCD							
			,targ.ZaikoKBN									 =src.ZaikoKBN							
			,targ.Rack										 =src.Rack							
			,targ.VirtualFlg								 =src.VirtualFlg							
			,targ.DirectFlg									 =src.DirectFlg							
			,targ.ReserveCD									 =src.ReserveCD							
			,targ.NoticesCD									 =src.NoticesCD							
			,targ.PostageCD									 =src.PostageCD							
			,targ.ManufactCD								 =src.ManufactCD							
			,targ.ConfirmCD									 =src.ConfirmCD							
			,targ.StopFlg									 =src.StopFlg							
			,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			,targ.SoldOutFlg								 =src.SoldOutFlg							
			,targ.WebStockFlg								 =src.WebStockFlg							
			,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			,targ.MakerAddFlg								 =src.MakerAddFlg							
			,targ.StoreAddFlg								 =src.StoreAddFlg							
			,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			,targ.CatalogFlg								 =src.CatalogFlg							
			,targ.ParcelFlg									 =src.ParcelFlg							
			,targ.TaxRateFLG								 =src.TaxRateFLG							
			,targ.CostingKBN								 =src.CostingKBN							
			,targ.NormalCost								 =src.NormalCost							
			,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			,targ.PriceWithTax								 =src.PriceWithTax							
			,targ.PriceOutTax								 =src.PriceOutTax							
			,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			,targ.Rate										 =src.Rate							
			,targ.SaleStartDate								 =src.SaleStartDate							
			,targ.WebStartDate								 =src.WebStartDate							
			,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			,targ.CommentInStore							 =src.CommentInStore							
			,targ.CommentOutStore							 =src.CommentOutStore							
			,targ.LastYearTerm								 =src.LastYearTerm							
			,targ.LastSeason								 =src.LastSeason							
			,targ.LastCatalogNO								 =src.LastCatalogNO							
			,targ.LastCatalogPage							 =src.LastCatalogPage							
			,targ.LastCatalogText							 =src.LastCatalogText							
			,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			,targ.WebAddress								 =src.WebAddress							
			,targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )
			,targ.TagName01									 =src.TagName01							
			,targ.TagName02									 =src.TagName02							
			,targ.TagName03									 =src.TagName03							
			,targ.TagName04									 =src.TagName04							
			,targ.TagName05									 =src.TagName05							
			,targ.TagName06									 =src.TagName06							
			,targ.TagName07									 =src.TagName07							
			,targ.TagName08									 =src.TagName08							
			,targ.TagName09									 =src.TagName09							
			,targ.TagName10									 =src.TagName10							
			,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			,targ.UsedFlg									 =src.UsedFlg							
			,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			,targ.InsertOperator							 =src.InsertOperator							
			,targ.InsertDateTime							 =src.InsertDateTime							
			,targ.UpdateOperator							 =src.UpdateOperator							
			,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
			if (@MainFlg =2)
			Begin	
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				--,SetKBN							
				--,PresentKBN							
				--,SampleKBN							
				--,DiscountKBN							
				,SizeCount							
				,ColorCount							
				--,null as SizeName							
				--,null as ColorName							
				--,WebFlg							
				--,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				--,ZaikoKBN							
				,Rack							
				--,VirtualFlg							
				--,DirectFlg							
				--,ReserveCD							
				--,NoticesCD							
				--,PostageCD							
				--,ManufactCD							
				--,ConfirmCD							
				--,StopFlg							
				--,DiscontinueFlg							
				--,SoldOutFlg							
				--,WebStockFlg							
				--,InventoryAddFlg							
				--,MakerAddFlg							
				--,StoreAddFlg							
				--,NoNetOrderFlg							
				--,EDIOrderFlg							
				--,AutoOrderFlg							
				--,CatalogFlg							
				--,ParcelFlg							
				--,TaxRateFLG							
				--,CostingKBN							
				,NormalCost							
				--,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				--,LastYearTerm							
				--,LastSeason							
				--,LastCatalogNO							
				--,LastCatalogPage							
				--,LastCatalogText							
				--,LastInstructionsNO							
				--,LastInstructionsDate							
				--,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	as ApprovalDateTime		
				--,TagName01							
				--,TagName02							
				--,TagName03							
				--,TagName04							
				--,TagName05							
				--,TagName06							
				--,TagName07							
				--,TagName08							
				--,TagName09							
				--,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				--,0 as UsedFlg							
				--,1 as SKSUpdateFlg							
				--,null as SKSUpdateDateTime							
				--,@Opt as InsertOperator							
				--, @Date as InsertDateTime							
				--, @Opt as UpdateOperator							
				--, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			 targ.VariousFLG								 =src.VariousFLG							
			,targ.ITemName									 =src.ITemName							
			,targ.KanaName									 =src.KanaName							
			,targ.ITEMShortName								 =src.ITEMShortName							
			,targ.EnglishName								 =src.EnglishName							
			--,targ.SetKBN									 =src.SetKBN							
			--,targ.PresentKBN								 =src.PresentKBN							
			--,targ.SampleKBN									 =src.SampleKBN							
			--,targ.DiscountKBN								 =src.DiscountKBN							
			,targ.SizeCount									 =src.SizeCount							
			,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			--,targ.WebFlg									 =src.WebFlg							
			--,targ.RealStoreFlg								 =src.RealStoreFlg							
			,targ.MainVendorCD								 =src.MainVendorCD							
			,targ.MakerVendorCD								 =src.MakerVendorCD						
			,targ.BrandCD									 =src.BrandCD							
			,targ.MakerItem									 =src.MakerItem							
			,targ.TaniCD									 =src.TaniCD							
			,targ.SportsCD									 =src.SportsCD							
			,targ.SegmentCD									 =src.SegmentCD							
			--,targ.ZaikoKBN									 =src.ZaikoKBN							
			,targ.Rack										 =src.Rack							
			--,targ.VirtualFlg								 =src.VirtualFlg							
			--,targ.DirectFlg									 =src.DirectFlg							
			--,targ.ReserveCD									 =src.ReserveCD							
			--,targ.NoticesCD									 =src.NoticesCD							
			--,targ.PostageCD									 =src.PostageCD							
			--,targ.ManufactCD								 =src.ManufactCD							
			--,targ.ConfirmCD									 =src.ConfirmCD							
			--,targ.StopFlg									 =src.StopFlg							
			--,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			--,targ.SoldOutFlg								 =src.SoldOutFlg							
			--,targ.WebStockFlg								 =src.WebStockFlg							
			--,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			--,targ.MakerAddFlg								 =src.MakerAddFlg							
			--,targ.StoreAddFlg								 =src.StoreAddFlg							
			--,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			--,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			--,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			--,targ.CatalogFlg								 =src.CatalogFlg							
			--,targ.ParcelFlg									 =src.ParcelFlg							
			--,targ.TaxRateFLG								 =src.TaxRateFLG							
			--,targ.CostingKBN								 =src.CostingKBN							
			,targ.NormalCost								 =src.NormalCost							
			--,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			,targ.PriceWithTax								 =src.PriceWithTax							
			,targ.PriceOutTax								 =src.PriceOutTax							
			,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			,targ.Rate										 =src.Rate							
			,targ.SaleStartDate								 =src.SaleStartDate							
			,targ.WebStartDate								 =src.WebStartDate							
			,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			,targ.CommentInStore							 =src.CommentInStore							
			,targ.CommentOutStore							 =src.CommentOutStore							
			--,targ.LastYearTerm								 =src.LastYearTerm							
			--,targ.LastSeason								 =src.LastSeason							
			--,targ.LastCatalogNO								 =src.LastCatalogNO							
			--,targ.LastCatalogPage							 =src.LastCatalogPage							
			--,targ.LastCatalogText							 =src.LastCatalogText							
			--,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			--,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			,targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )		
			--,targ.TagName01									 =src.TagName01							
			--,targ.TagName02									 =src.TagName02							
			--,targ.TagName03									 =src.TagName03							
			--,targ.TagName04									 =src.TagName04							
			--,targ.TagName05									 =src.TagName05							
			--,targ.TagName06									 =src.TagName06							
			--,targ.TagName07									 =src.TagName07							
			--,targ.TagName08									 =src.TagName08							
			--,targ.TagName09									 =src.TagName09							
			--,targ.TagName10									 =src.TagName10							
			,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			--,targ.UpdateOperator							 =src.UpdateOperator							
			--,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			--,SetKBN					
			--,PresentKBN				
			--,SampleKBN					
			--,DiscountKBN				
			,SizeCount					
			,ColorCount				
			--,SizeName					
			--,ColorName					
			--,WebFlg					
			--,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			--,ZaikoKBN					
			,Rack						
			--,VirtualFlg				
			--,DirectFlg					
			--,ReserveCD					
			--,NoticesCD					
			--,PostageCD					
			--,ManufactCD				
			--,ConfirmCD					
			--,StopFlg					
			--,DiscontinueFlg			
			--,SoldOutFlg				
			--,WebStockFlg				
			--,InventoryAddFlg			
			--,MakerAddFlg				
			--,StoreAddFlg				
			--,NoNetOrderFlg				
			--,EDIOrderFlg				
			--,AutoOrderFlg				
			--,CatalogFlg				
			--,ParcelFlg					
			--,TaxRateFLG				
			--,CostingKBN				
			,NormalCost				
			--,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			--,LastYearTerm				
			--,LastSeason				
			--,LastCatalogNO				
			--,LastCatalogPage			
			--,LastCatalogText			
			--,LastInstructionsNO		
			--,LastInstructionsDate		
			--,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			--,TagName01					
			--,TagName02					
			--,TagName03					
			--,TagName04					
			--,TagName05					
			--,TagName06					
			--,TagName07					
			--,TagName08					
			--,TagName09					
			--,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			--,UsedFlg					
			--,SKSUpdateFlg				
			--,SKSUpdateDateTime			
			--,InsertOperator			
			--,InsertDateTime			
			--,UpdateOperator			
			--,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			--,src.SetKBN					
			--,src.PresentKBN				
			--,src.SampleKBN					
			--,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			--,src.SizeName					
			--,src.ColorName					
			--,src.WebFlg					
			--,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			--,src.ZaikoKBN					
			,src.Rack						
			--,src.VirtualFlg				
			--,src.DirectFlg					
			--,src.ReserveCD					
			--,src.NoticesCD					
			--,src.PostageCD					
			--,src.ManufactCD				
			--,src.ConfirmCD					
			--,src.StopFlg					
			--,src.DiscontinueFlg			
			--,src.SoldOutFlg				
			--,src.WebStockFlg				
			--,src.InventoryAddFlg			
			--,src.MakerAddFlg				
			--,src.StoreAddFlg				
			--,src.NoNetOrderFlg				
			--,src.EDIOrderFlg				
			--,src.AutoOrderFlg				
			--,src.CatalogFlg				
			--,src.ParcelFlg					
			--,src.TaxRateFLG				
			--,src.CostingKBN				
			,src.NormalCost				
			--,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			--,src.LastYearTerm				
			--,src.LastSeason				
			--,src.LastCatalogNO				
			--,src.LastCatalogPage			
			--,src.LastCatalogText			
			--,src.LastInstructionsNO		
			--,src.LastInstructionsDate		
			--,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			--,src.TagName01					
			--,src.TagName02					
			--,src.TagName03					
			--,src.TagName04					
			--,src.TagName05					
			--,src.TagName06					
			--,src.TagName07					
			--,src.TagName08					
			--,src.TagName09					
			--,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			--,src.UsedFlg					
			--,src.SKSUpdateFlg				
			--,src.SKSUpdateDateTime			
			--,src.InsertOperator			
			--,src.InsertDateTime			
			--,src.UpdateOperator			
			--,src.UpdateDateTime			
			);
			End
			if (@MainFlg =3)
			Begin
			set @Ins =0;
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.VariousFLG									 =src.VariousFLG							
			--,targ.ITemName									 =src.ITemName							
			--,targ.KanaName									 =src.KanaName							
			--,targ.ITEMShortName								 =src.ITEMShortName							
			--,targ.EnglishName								 =src.EnglishName							
			 targ.SetKBN									 =src.SetKBN							
			,targ.PresentKBN								 =src.PresentKBN							
			,targ.SampleKBN									 =src.SampleKBN							
			,targ.DiscountKBN								 =src.DiscountKBN							
			--,targ.SizeCount									 =src.SizeCount							
			--,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			,targ.WebFlg									 =src.WebFlg							
			,targ.RealStoreFlg								 =src.RealStoreFlg							
			--,targ.MainVendorCD								 =src.MainVendorCD							
			--,targ.MakerVendorCD								 =src.MakerVendorCD						
			--,targ.BrandCD									 =src.BrandCD							
			--,targ.MakerItem									 =src.MakerItem							
			--,targ.TaniCD									 =src.TaniCD							
			--,targ.SportsCD									 =src.SportsCD							
			--,targ.SegmentCD									 =src.SegmentCD							
			,targ.ZaikoKBN									 =src.ZaikoKBN							
			--,targ.Rack										 =src.Rack							
			,targ.VirtualFlg								 =src.VirtualFlg							
			,targ.DirectFlg									 =src.DirectFlg							
			,targ.ReserveCD									 =src.ReserveCD							
			,targ.NoticesCD									 =src.NoticesCD							
			,targ.PostageCD									 =src.PostageCD							
			,targ.ManufactCD								 =src.ManufactCD							
			,targ.ConfirmCD									 =src.ConfirmCD							
			,targ.StopFlg									 =src.StopFlg							
			,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			,targ.SoldOutFlg								 =src.SoldOutFlg							
			,targ.WebStockFlg								 =src.WebStockFlg							
			,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			,targ.MakerAddFlg								 =src.MakerAddFlg							
			,targ.StoreAddFlg								 =src.StoreAddFlg							
			,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			,targ.CatalogFlg								 =src.CatalogFlg							
			,targ.ParcelFlg									 =src.ParcelFlg							
			--,targ.TaxRateFLG								 =src.TaxRateFLG							
			--,targ.CostingKBN								 =src.CostingKBN							
			--,targ.NormalCost								 =src.NormalCost							
			,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			,targ.PriceWithTax								 =src.PriceWithTax							
			,targ.PriceOutTax								 =src.PriceOutTax							
			,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			,targ.Rate										 =src.Rate							
			--,targ.SaleStartDate								 =src.SaleStartDate							
			--,targ.WebStartDate								 =src.WebStartDate							
			--,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			--,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			--,targ.CommentInStore							 =src.CommentInStore							
			--,targ.CommentOutStore							 =src.CommentOutStore							
			--,targ.LastYearTerm								 =src.LastYearTerm							
			--,targ.LastSeason								 =src.LastSeason							
			--,targ.LastCatalogNO								 =src.LastCatalogNO							
			--,targ.LastCatalogPage							 =src.LastCatalogPage							
			--,targ.LastCatalogText							 =src.LastCatalogText							
			--,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			--,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			,targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )		
			--,targ.TagName01									 =src.TagName01							
			--,targ.TagName02									 =src.TagName02							
			--,targ.TagName03									 =src.TagName03							
			--,targ.TagName04									 =src.TagName04							
			--,targ.TagName05									 =src.TagName05							
			--,targ.TagName06									 =src.TagName06							
			--,targ.TagName07									 =src.TagName07							
			--,targ.TagName08									 =src.TagName08							
			--,targ.TagName09									 =src.TagName09							
			--,targ.TagName10									 =src.TagName10							
			--,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			--,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			,targ.UpdateOperator							 =src.UpdateOperator							
			,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
			if (@MainFlg =4)
			Begin
			set @Ins =0;
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.VariousFLG									 =src.VariousFLG							
			--,targ.ITemName									 =src.ITemName							
			--,targ.KanaName									 =src.KanaName							
			--,targ.ITEMShortName								 =src.ITEMShortName							
			--,targ.EnglishName								 =src.EnglishName							
			--,targ.SetKBN									 =src.SetKBN							
			--,targ.PresentKBN								 =src.PresentKBN							
			--,targ.SampleKBN									 =src.SampleKBN							
			--,targ.DiscountKBN								 =src.DiscountKBN							
			--,targ.SizeCount									 =src.SizeCount							
			--,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			--,targ.WebFlg									 =src.WebFlg							
			--,targ.RealStoreFlg								 =src.RealStoreFlg							
			--,targ.MainVendorCD								 =src.MainVendorCD							
			--,targ.MakerVendorCD								 =src.MakerVendorCD						
			--,targ.BrandCD									 =src.BrandCD							
			--,targ.MakerItem									 =src.MakerItem							
			--,targ.TaniCD									 =src.TaniCD							
			--,targ.SportsCD									 =src.SportsCD							
			--,targ.SegmentCD									 =src.SegmentCD							
			--,targ.ZaikoKBN									 =src.ZaikoKBN							
			--,targ.Rack										 =src.Rack							
			--,targ.VirtualFlg								 =src.VirtualFlg							
			--,targ.DirectFlg									 =src.DirectFlg							
			--,targ.ReserveCD									 =src.ReserveCD							
			--,targ.NoticesCD									 =src.NoticesCD							
			--,targ.PostageCD									 =src.PostageCD							
			--,targ.ManufactCD								 =src.ManufactCD							
			--,targ.ConfirmCD									 =src.ConfirmCD							
			--,targ.StopFlg									 =src.StopFlg							
			--,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			--,targ.SoldOutFlg								 =src.SoldOutFlg							
			--,targ.WebStockFlg								 =src.WebStockFlg							
			--,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			--,targ.MakerAddFlg								 =src.MakerAddFlg							
			--,targ.StoreAddFlg								 =src.StoreAddFlg							
			--,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			--,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			--,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			--,targ.CatalogFlg								 =src.CatalogFlg							
			--,targ.ParcelFlg									 =src.ParcelFlg							
			targ.TaxRateFLG								 =src.TaxRateFLG							
			,targ.CostingKBN								 =src.CostingKBN							
			,targ.NormalCost								 =src.NormalCost							
			,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			,targ.PriceWithTax								 =src.PriceWithTax							
			,targ.PriceOutTax								 =src.PriceOutTax							
			,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			,targ.Rate										 =src.Rate							
			--,targ.SaleStartDate								 =src.SaleStartDate							
			--,targ.WebStartDate								 =src.WebStartDate							
			--,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			--,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			--,targ.CommentInStore							 =src.CommentInStore							
			--,targ.CommentOutStore							 =src.CommentOutStore							
			--,targ.LastYearTerm								 =src.LastYearTerm							
			--,targ.LastSeason								 =src.LastSeason							
			--,targ.LastCatalogNO								 =src.LastCatalogNO							
			--,targ.LastCatalogPage							 =src.LastCatalogPage							
			--,targ.LastCatalogText							 =src.LastCatalogText							
			--,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			--,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			,targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	
			--,targ.TagName01									 =src.TagName01							
			--,targ.TagName02									 =src.TagName02							
			--,targ.TagName03									 =src.TagName03							
			--,targ.TagName04									 =src.TagName04							
			--,targ.TagName05									 =src.TagName05							
			--,targ.TagName06									 =src.TagName06							
			--,targ.TagName07									 =src.TagName07							
			--,targ.TagName08									 =src.TagName08							
			--,targ.TagName09									 =src.TagName09							
			--,targ.TagName10									 =src.TagName10							
			--,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			--,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			,targ.UpdateOperator							 =src.UpdateOperator							
			,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
			if (@MainFlg =5)
			Begin
			set @Ins =0;
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.VariousFLG									 =src.VariousFLG							
			--,targ.ITemName									 =src.ITemName							
			--,targ.KanaName									 =src.KanaName							
			--,targ.ITEMShortName								 =src.ITEMShortName							
			--,targ.EnglishName								 =src.EnglishName							
			--,targ.SetKBN									 =src.SetKBN							
			--,targ.PresentKBN								 =src.PresentKBN							
			--,targ.SampleKBN									 =src.SampleKBN							
			--,targ.DiscountKBN								 =src.DiscountKBN							
			--,targ.SizeCount									 =src.SizeCount							
			--,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			--,targ.WebFlg									 =src.WebFlg							
			--,targ.RealStoreFlg								 =src.RealStoreFlg							
			--,targ.MainVendorCD								 =src.MainVendorCD							
			--,targ.MakerVendorCD								 =src.MakerVendorCD						
			--,targ.BrandCD									 =src.BrandCD							
			--,targ.MakerItem									 =src.MakerItem							
			--,targ.TaniCD									 =src.TaniCD							
			--,targ.SportsCD									 =src.SportsCD							
			--,targ.SegmentCD									 =src.SegmentCD							
			--,targ.ZaikoKBN									 =src.ZaikoKBN							
			--,targ.Rack										 =src.Rack							
			--,targ.VirtualFlg								 =src.VirtualFlg							
			--,targ.DirectFlg									 =src.DirectFlg							
			--,targ.ReserveCD									 =src.ReserveCD							
			--,targ.NoticesCD									 =src.NoticesCD							
			--,targ.PostageCD									 =src.PostageCD							
			--,targ.ManufactCD								 =src.ManufactCD							
			--,targ.ConfirmCD									 =src.ConfirmCD							
			--,targ.StopFlg									 =src.StopFlg							
			--,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			--,targ.SoldOutFlg								 =src.SoldOutFlg							
			--,targ.WebStockFlg								 =src.WebStockFlg							
			--,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			--,targ.MakerAddFlg								 =src.MakerAddFlg							
			--,targ.StoreAddFlg								 =src.StoreAddFlg							
			--,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			--,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			--,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			--,targ.CatalogFlg								 =src.CatalogFlg							
			--,targ.ParcelFlg									 =src.ParcelFlg							
			--,targ.TaxRateFLG								 =src.TaxRateFLG							
			--,targ.CostingKBN								 =src.CostingKBN							
			--,targ.NormalCost								 =src.NormalCost							
			--,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			--,targ.PriceWithTax								 =src.PriceWithTax							
			--,targ.PriceOutTax								 =src.PriceOutTax							
			--,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			--,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			--,targ.Rate										 =src.Rate							
			--,targ.SaleStartDate								 =src.SaleStartDate							
			--,targ.WebStartDate								 =src.WebStartDate							
			--,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			--,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			--,targ.CommentInStore							 =src.CommentInStore							
			--,targ.CommentOutStore							 =src.CommentOutStore							
			 targ.LastYearTerm								 =src.LastYearTerm							
			,targ.LastSeason								 =src.LastSeason							
			,targ.LastCatalogNO								 =src.LastCatalogNO							
			,targ.LastCatalogPage							 =src.LastCatalogPage							
			,targ.LastCatalogText							 =src.LastCatalogText							
			,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			,targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )		
			--,targ.TagName01									 =src.TagName01							
			--,targ.TagName02									 =src.TagName02							
			--,targ.TagName03									 =src.TagName03							
			--,targ.TagName04									 =src.TagName04							
			--,targ.TagName05									 =src.TagName05							
			--,targ.TagName06									 =src.TagName06							
			--,targ.TagName07									 =src.TagName07							
			--,targ.TagName08									 =src.TagName08							
			--,targ.TagName09									 =src.TagName09							
			--,targ.TagName10									 =src.TagName10							
			--,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			--,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			--,targ.UpdateOperator							 =src.UpdateOperator							
			--,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
			if (@MainFlg =6)
			Begin
			set @Ins =0;
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.VariousFLG									 =src.VariousFLG							
			--,targ.ITemName									 =src.ITemName							
			--,targ.KanaName									 =src.KanaName							
			--,targ.ITEMShortName								 =src.ITEMShortName							
			--,targ.EnglishName								 =src.EnglishName							
			--,targ.SetKBN									 =src.SetKBN							
			--,targ.PresentKBN								 =src.PresentKBN							
			--,targ.SampleKBN									 =src.SampleKBN							
			--,targ.DiscountKBN								 =src.DiscountKBN							
			--,targ.SizeCount									 =src.SizeCount							
			--,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			--,targ.WebFlg									 =src.WebFlg							
			--,targ.RealStoreFlg								 =src.RealStoreFlg							
			--,targ.MainVendorCD								 =src.MainVendorCD							
			--,targ.MakerVendorCD								 =src.MakerVendorCD						
			--,targ.BrandCD									 =src.BrandCD							
			--,targ.MakerItem									 =src.MakerItem							
			--,targ.TaniCD									 =src.TaniCD							
			--,targ.SportsCD									 =src.SportsCD							
			--,targ.SegmentCD									 =src.SegmentCD							
			--,targ.ZaikoKBN									 =src.ZaikoKBN							
			--,targ.Rack										 =src.Rack							
			--,targ.VirtualFlg								 =src.VirtualFlg							
			--,targ.DirectFlg									 =src.DirectFlg							
			--,targ.ReserveCD									 =src.ReserveCD							
			--,targ.NoticesCD									 =src.NoticesCD							
			--,targ.PostageCD									 =src.PostageCD							
			--,targ.ManufactCD								 =src.ManufactCD							
			--,targ.ConfirmCD									 =src.ConfirmCD							
			--,targ.StopFlg									 =src.StopFlg							
			--,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			--,targ.SoldOutFlg								 =src.SoldOutFlg							
			--,targ.WebStockFlg								 =src.WebStockFlg							
			--,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			--,targ.MakerAddFlg								 =src.MakerAddFlg							
			--,targ.StoreAddFlg								 =src.StoreAddFlg							
			--,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			--,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			--,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			--,targ.CatalogFlg								 =src.CatalogFlg							
			--,targ.ParcelFlg									 =src.ParcelFlg							
			--,targ.TaxRateFLG								 =src.TaxRateFLG							
			--,targ.CostingKBN								 =src.CostingKBN							
			--,targ.NormalCost								 =src.NormalCost							
			--,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			--,targ.PriceWithTax								 =src.PriceWithTax							
			--,targ.PriceOutTax								 =src.PriceOutTax							
			--,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			--,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			--,targ.Rate										 =src.Rate							
			--,targ.SaleStartDate								 =src.SaleStartDate							
			--,targ.WebStartDate								 =src.WebStartDate							
			--,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			--,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			--,targ.CommentInStore							 =src.CommentInStore							
			--,targ.CommentOutStore							 =src.CommentOutStore							
			--,targ.LastYearTerm								 =src.LastYearTerm							
			--,targ.LastSeason								 =src.LastSeason							
			--,targ.LastCatalogNO								 =src.LastCatalogNO							
			--,targ.LastCatalogPage							 =src.LastCatalogPage							
			--,targ.LastCatalogText							 =src.LastCatalogText							
			--,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			--,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	
			,targ.TagName01									 =src.TagName01							
			,targ.TagName02									 =src.TagName02							
			,targ.TagName03									 =src.TagName03							
			,targ.TagName04									 =src.TagName04							
			,targ.TagName05									 =src.TagName05							
			,targ.TagName06									 =src.TagName06							
			,targ.TagName07									 =src.TagName07							
			,targ.TagName08									 =src.TagName08							
			,targ.TagName09									 =src.TagName09							
			,targ.TagName10									 =src.TagName10							
			--,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			--,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			--,targ.UpdateOperator							 =src.UpdateOperator							
			--,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
			if (@MainFlg =8)
			Begin
			set @Ins =0;
			Merge [M_item] targ using ( select *  from	(select 
				ITemCD							
				,ChangeDate							
				,VariousFLG							
				,ITemName							
				,KanaName							
				,ITEMShortName							
				,EnglishName							
				,SetKBN							
				,PresentKBN							
				,SampleKBN							
				,DiscountKBN							
				,SizeCount							
				,ColorCount							
				,null as SizeName							
				,null as ColorName							
				,WebFlg							
				,RealStoreFlg							
				,MainVendorCD							
				,MainVendorCD as MakerVendorCD							
				,BrandCD							
				,MakerItem							
				,TaniCD							
				,SportsCD							
				,SegmentCD							
				,ZaikoKBN							
				,Rack							
				,VirtualFlg							
				,DirectFlg							
				,ReserveCD							
				,NoticesCD							
				,PostageCD							
				,ManufactCD							
				,ConfirmCD							
				,StopFlg							
				,DiscontinueFlg							
				,SoldOutFlg							
				,WebStockFlg							
				,InventoryAddFlg							
				,MakerAddFlg							
				,StoreAddFlg							
				,NoNetOrderFlg							
				,EDIOrderFlg							
				,AutoOrderFlg							
				,CatalogFlg							
				,ParcelFlg							
				,TaxRateFLG							
				,CostingKBN							
				,NormalCost							
				,SaleExcludedFlg							
				,PriceWithTax							
				,PriceOutTax							
				,OrderPriceWithTax							
				,OrderPriceWithoutTax							
				,Rate							
				,SaleStartDate							
				,WebStartDate							
				,OrderAttentionCD							
				,OrderAttentionNote							
				,CommentInStore							
				,CommentOutStore							
				,LastYearTerm							
				,LastSeason							
				,LastCatalogNO							
				,LastCatalogPage							
				,LastCatalogText							
				,LastInstructionsNO							
				,LastInstructionsDate							
				,WebAddress							
				,ApprovalDate							
				,(case when ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end ) as ApprovalDateTime		
				,TagName01							
				,TagName02							
				,TagName03							
				,TagName04							
				,TagName05							
				,TagName06							
				,TagName07							
				,TagName08							
				,TagName09							
				,TagName10							
				,ExhibitionSegmentCD							
				,OrderLot							
				,DeleteFlg							
				,0 as UsedFlg							
				,1 as SKSUpdateFlg							
				,null as SKSUpdateDateTime							
				,@Opt as InsertOperator							
				, @Date as InsertDateTime							
				, @Opt as UpdateOperator							
				, @Date as UpdateDateTime							
				
			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.VariousFLG									 =src.VariousFLG							
			--,targ.ITemName									 =src.ITemName							
			--,targ.KanaName									 =src.KanaName							
			--,targ.ITEMShortName								 =src.ITEMShortName							
			--,targ.EnglishName								 =src.EnglishName							
			--,targ.SetKBN									 =src.SetKBN							
			--,targ.PresentKBN								 =src.PresentKBN							
			--,targ.SampleKBN									 =src.SampleKBN							
			--,targ.DiscountKBN								 =src.DiscountKBN							
			--,targ.SizeCount									 =src.SizeCount							
			--,targ.ColorCount								 =src.ColorCount							
			--,targ.SizeName									 =src.SizeName							
			--,targ.ColorName									 =src.ColorName							
			--,targ.WebFlg									 =src.WebFlg							
			--,targ.RealStoreFlg								 =src.RealStoreFlg							
			--,targ.MainVendorCD								 =src.MainVendorCD							
			--,targ.MakerVendorCD								 =src.MakerVendorCD						
			--,targ.BrandCD									 =src.BrandCD							
			--,targ.MakerItem									 =src.MakerItem							
			--,targ.TaniCD									 =src.TaniCD							
			--,targ.SportsCD									 =src.SportsCD							
			--,targ.SegmentCD									 =src.SegmentCD							
			--,targ.ZaikoKBN									 =src.ZaikoKBN							
			--,targ.Rack										 =src.Rack							
			--,targ.VirtualFlg								 =src.VirtualFlg							
			--,targ.DirectFlg									 =src.DirectFlg							
			--,targ.ReserveCD									 =src.ReserveCD							
			--,targ.NoticesCD									 =src.NoticesCD							
			--,targ.PostageCD									 =src.PostageCD							
			--,targ.ManufactCD								 =src.ManufactCD							
			--,targ.ConfirmCD									 =src.ConfirmCD							
			--,targ.StopFlg									 =src.StopFlg							
			--,targ.DiscontinueFlg							 =src.DiscontinueFlg							
			--,targ.SoldOutFlg								 =src.SoldOutFlg							
			--,targ.WebStockFlg								 =src.WebStockFlg							
			--,targ.InventoryAddFlg							 =src.InventoryAddFlg							
			--,targ.MakerAddFlg								 =src.MakerAddFlg							
			--,targ.StoreAddFlg								 =src.StoreAddFlg							
			--,targ.NoNetOrderFlg								 =src.NoNetOrderFlg							
			--,targ.EDIOrderFlg								 =src.EDIOrderFlg							
			--,targ.AutoOrderFlg								 =src.AutoOrderFlg							
			--,targ.CatalogFlg								 =src.CatalogFlg							
			--,targ.ParcelFlg									 =src.ParcelFlg							
			--,targ.TaxRateFLG								 =src.TaxRateFLG							
			--,targ.CostingKBN								 =src.CostingKBN							
			--,targ.NormalCost								 =src.NormalCost							
			--,targ.SaleExcludedFlg							 =src.SaleExcludedFlg							
			--,targ.PriceWithTax								 =src.PriceWithTax							
			--,targ.PriceOutTax								 =src.PriceOutTax							
			--,targ.OrderPriceWithTax							 =src.OrderPriceWithTax							
			--,targ.OrderPriceWithoutTax						 =src.OrderPriceWithoutTax							
			--,targ.Rate										 =src.Rate							
			--,targ.SaleStartDate								 =src.SaleStartDate							
			--,targ.WebStartDate								 =src.WebStartDate							
			--,targ.OrderAttentionCD							 =src.OrderAttentionCD							
			--,targ.OrderAttentionNote						 =src.OrderAttentionNote							
			--,targ.CommentInStore							 =src.CommentInStore							
			--,targ.CommentOutStore							 =src.CommentOutStore							
			--,targ.LastYearTerm								 =src.LastYearTerm							
			--,targ.LastSeason								 =src.LastSeason							
			--,targ.LastCatalogNO								 =src.LastCatalogNO							
			--,targ.LastCatalogPage							 =src.LastCatalogPage							
			--,targ.LastCatalogText							 =src.LastCatalogText							
			--,targ.LastInstructionsNO						 =src.LastInstructionsNO							
			--,targ.LastInstructionsDate						 =src.LastInstructionsDate							
			--,targ.WebAddress								 =src.WebAddress							
			targ.ApprovalDate								 =src.ApprovalDate							
			,targ.ApprovalDateTime							 =(case when src.ApprovalDate is not null then CONVERT(varchar,@_Date,120) else null end )	
			--,targ.TagName01									 =src.TagName01							
			--,targ.TagName02									 =src.TagName02							
			--,targ.TagName03									 =src.TagName03							
			--,targ.TagName04									 =src.TagName04							
			--,targ.TagName05									 =src.TagName05							
			--,targ.TagName06									 =src.TagName06							
			--,targ.TagName07									 =src.TagName07							
			--,targ.TagName08									 =src.TagName08							
			--,targ.TagName09									 =src.TagName09							
			--,targ.TagName10									 =src.TagName10							
			--,targ.ExhibitionSegmentCD						 =src.ExhibitionSegmentCD							
			--,targ.OrderLot									 =src.OrderLot							
			,targ.DeleteFlg									 =src.DeleteFlg							
			--,targ.UsedFlg									 =src.UsedFlg							
			--,targ.SKSUpdateFlg								 =src.SKSUpdateFlg							
			--,targ.SKSUpdateDateTime							 =src.SKSUpdateDateTime							
			--,targ.InsertOperator							 =src.InsertOperator							
			--,targ.InsertDateTime							 =src.InsertDateTime							
			--,targ.UpdateOperator							 =src.UpdateOperator							
			--,targ.UpdateDateTime							 =src.UpdateDateTime		
			

			when not matched by target and @Ins = 1 then insert
			(
			ItemCD
			,ChangeDate
			,VariousFLG				
			,ITemName					
			,KanaName					
			,ITEMShortName				
			,EnglishName				
			,SetKBN					
			,PresentKBN				
			,SampleKBN					
			,DiscountKBN				
			,SizeCount					
			,ColorCount				
			,SizeName					
			,ColorName					
			,WebFlg					
			,RealStoreFlg				
			,MainVendorCD				
			,MakerVendorCD				
			,BrandCD					
			,MakerItem					
			,TaniCD					
			,SportsCD					
			,SegmentCD					
			,ZaikoKBN					
			,Rack						
			,VirtualFlg				
			,DirectFlg					
			,ReserveCD					
			,NoticesCD					
			,PostageCD					
			,ManufactCD				
			,ConfirmCD					
			,StopFlg					
			,DiscontinueFlg			
			,SoldOutFlg				
			,WebStockFlg				
			,InventoryAddFlg			
			,MakerAddFlg				
			,StoreAddFlg				
			,NoNetOrderFlg				
			,EDIOrderFlg				
			,AutoOrderFlg				
			,CatalogFlg				
			,ParcelFlg					
			,TaxRateFLG				
			,CostingKBN				
			,NormalCost				
			,SaleExcludedFlg			
			,PriceWithTax				
			,PriceOutTax				
			,OrderPriceWithTax			
			,OrderPriceWithoutTax		
			,Rate						
			,SaleStartDate				
			,WebStartDate				
			,OrderAttentionCD			
			,OrderAttentionNote		
			,CommentInStore			
			,CommentOutStore			
			,LastYearTerm				
			,LastSeason				
			,LastCatalogNO				
			,LastCatalogPage			
			,LastCatalogText			
			,LastInstructionsNO		
			,LastInstructionsDate		
			,WebAddress				
			,ApprovalDate				
			,ApprovalDateTime			
			,TagName01					
			,TagName02					
			,TagName03					
			,TagName04					
			,TagName05					
			,TagName06					
			,TagName07					
			,TagName08					
			,TagName09					
			,TagName10					
			,ExhibitionSegmentCD		
			,OrderLot					
			,DeleteFlg					
			,UsedFlg					
			,SKSUpdateFlg				
			,SKSUpdateDateTime			
			,InsertOperator			
			,InsertDateTime			
			,UpdateOperator			
			,UpdateDateTime			
			)
			values
			(
			ItemCD
			,src.ChangeDate
			,src.VariousFLG				
			,src.ITemName					
			,src.KanaName					
			,src.ITEMShortName				
			,src.EnglishName				
			,src.SetKBN					
			,src.PresentKBN				
			,src.SampleKBN					
			,src.DiscountKBN				
			,src.SizeCount					
			,src.ColorCount				
			,src.SizeName					
			,src.ColorName					
			,src.WebFlg					
			,src.RealStoreFlg				
			,src.MainVendorCD				
			,src.MakerVendorCD				
			,src.BrandCD					
			,src.MakerItem					
			,src.TaniCD					
			,src.SportsCD					
			,src.SegmentCD					
			,src.ZaikoKBN					
			,src.Rack						
			,src.VirtualFlg				
			,src.DirectFlg					
			,src.ReserveCD					
			,src.NoticesCD					
			,src.PostageCD					
			,src.ManufactCD				
			,src.ConfirmCD					
			,src.StopFlg					
			,src.DiscontinueFlg			
			,src.SoldOutFlg				
			,src.WebStockFlg				
			,src.InventoryAddFlg			
			,src.MakerAddFlg				
			,src.StoreAddFlg				
			,src.NoNetOrderFlg				
			,src.EDIOrderFlg				
			,src.AutoOrderFlg				
			,src.CatalogFlg				
			,src.ParcelFlg					
			,src.TaxRateFLG				
			,src.CostingKBN				
			,src.NormalCost				
			,src.SaleExcludedFlg			
			,src.PriceWithTax				
			,src.PriceOutTax				
			,src.OrderPriceWithTax			
			,src.OrderPriceWithoutTax		
			,src.Rate						
			,src.SaleStartDate				
			,src.WebStartDate				
			,src.OrderAttentionCD			
			,src.OrderAttentionNote		
			,src.CommentInStore			
			,src.CommentOutStore			
			,src.LastYearTerm				
			,src.LastSeason				
			,src.LastCatalogNO				
			,src.LastCatalogPage			
			,src.LastCatalogText			
			,src.LastInstructionsNO		
			,src.LastInstructionsDate		
			,src.WebAddress				
			,src.ApprovalDate				
			,src.ApprovalDateTime			
			,src.TagName01					
			,src.TagName02					
			,src.TagName03					
			,src.TagName04					
			,src.TagName05					
			,src.TagName06					
			,src.TagName07					
			,src.TagName08					
			,src.TagName09					
			,src.TagName10					
			,src.ExhibitionSegmentCD		
			,src.OrderLot					
			,src.DeleteFlg					
			,src.UsedFlg					
			,src.SKSUpdateFlg				
			,src.SKSUpdateDateTime			
			,src.InsertOperator			
			,src.InsertDateTime			
			,src.UpdateOperator			
			,src.UpdateDateTime			
			);
			End
		--	select * from M_ITEM where ITemCD = 'ac223'


			--select *  from #tempItem
			drop table dbo.#tempItem
	 
END

