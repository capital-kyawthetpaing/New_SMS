
 BEGIN TRY 
 Drop Procedure dbo.[_Item_ItemPrice]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[_Item_ItemPrice]
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
	@Upt as int =1,
	@Ins as int = 1;
	if @MainFlg = 1 or @MainFlg  =2 or @MainFlg =3 or @MainFlg =4
		Begin
  

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

			--select *  from #tempItem
			--Merge [M_item] mi using [
			 WITH cte AS ( SELECT  ROW_NUMBER() OVER (PARTITION BY ItemCD,ChangeDate ORDER BY ItemCD,ChangeDate ) row_num FROM  #tempItem) delete FROM cte WHERE row_num > 1;
			Merge [M_ItemPrice] targ using ( select *  from	(select 
							ITemCD							
							,ChangeDate							
							,PriceWithTax 							
							,PriceOutTax	as PriceWithoutTax						
							,100 as GeneralRate							
							,PriceWithTax	as	GeneralPriceWithTax					
							,PriceOutTax	as 	GeneralPriceOutTax					
							,100 as MemberRate						
							,PriceWithTax	as 	MemberPriceWithTax					
							,PriceOutTax	as MemberPriceOutTax						
							,100 as ClientRate							
							,PriceWithTax	as 	ClientPriceWithTax					
							,PriceOutTax	as 	ClientPriceOutTax					
							,100 as WebRate						
							,PriceWithTax	as 	WebPriceWithTax					
							,PriceOutTax	as WebPriceOutTax						
							,100 as SaleRate							
							,PriceWithTax	as SalePriceWithTax						
							,PriceOutTax	as SalePriceOutTax						
							,null as Remarks							
							,0		as DeleteFlg					
							,0		as UsedFlg					
							,@opt	as InsertOpt					
							,@date	as Insertdt					
							,@opt	as updateOpt					
							,@date	as Updatedt	


			from #tempItem ) a ) src  	on targ.ItemCD = src.ItemCD and targ.ChangeDate = src.ChangeDate
			when matched  and @Upt =1 then
			Update set
			-- targ.ITemCD										=src.ITemCD				
			--,targ.ChangeDate									=src.ChangeDate			
			 targ.PriceWithTax									=src.PriceWithTax		
			,targ.PriceWithoutTax								=src.PriceWithoutTax		
			,targ.GeneralRate									=src.GeneralRate			
			--,targ.GeneralPriceWithTax							=src.PriceWithTax	
			--,targ.GeneralPriceOutTax							=src.PriceOutTax	
			--,targ.MemberRate									=src.MemberRate			
			--,targ.MemberPriceWithTax							=src.PriceWithTax	
			--,targ.MemberPriceOutTax								=src.PriceOutTax	
			--,targ.ClientRate									=src.ClientRate			
			--,targ.ClientPriceWithTax							=src.PriceWithTax	
			--,targ.ClientPriceOutTax								=src.PriceOutTax	
			--,targ.SaleRate										=src.SaleRate			
			--,targ.SalePriceWithTax								=src.PriceWithTax	
			--,targ.SalePriceOutTax								=src.PriceOutTax		
			--,targ.WebRate										=src.WebRate				
			--,targ.WebPriceWithTax								=src.PriceWithTax		
			--,targ.WebPriceOutTax								=src.PriceOutTax		
			--,targ.Remarks										=src.Remarks				
			--,targ.DeleteFlg										=src.DeleteFlg			
			--,targ.UsedFlg										=src.UsedFlg				
			--,targ.InsertOperator								=src.InsertOpt		
			--,targ.InsertDateTime								=src.Insertdt		
			,targ.UpdateOperator								=src.Updateopt		
			,targ.UpdateDateTime								=src.Updatedt		
			when not matched by target and @Ins = 1 then insert
			(
				ITemCD				
				,ChangeDate			
				,PriceWithTax			
				,PriceWithoutTax		
				,GeneralRate			
				,GeneralPriceWithTax	
				,GeneralPriceOutTax	
				,MemberRate			
				,MemberPriceWithTax	
				,MemberPriceOutTax		
				,ClientRate			
				,ClientPriceWithTax	
				,ClientPriceOutTax		
				,SaleRate				
				,SalePriceWithTax		
				,SalePriceOutTax		
				,WebRate				
				,WebPriceWithTax		
				,WebPriceOutTax		
				,Remarks				
				,DeleteFlg				
				,UsedFlg				
				,InsertOperator		
				,InsertDateTime		
				,UpdateOperator		
				,UpdateDateTime			
			)
			values
			(
			 	src.ITemCD			
				,src.ChangeDate		
				,src.PriceWithTax	
				,src.PriceWithoutTax	
				,src.GeneralRate	
				,src.PriceWithTax	
				,src.PriceWithoutTax	
				,src.MemberRate		
				,src.PriceWithTax	
				,src.PriceWithoutTax
				,src.ClientRate		
				,src.PriceWithTax	
				,src.PriceWithoutTax
				,src.SaleRate	
				,src.PriceWithTax
				,src.PriceWithoutTax	
				,src.WebRate		
				,src.PriceWithTax	
				,src.PriceWithoutTax	
				,src.Remarks		
				,src.DeleteFlg	
				,src.UsedFlg		
				,src.InsertOpt		
				,src.Insertdt		
				,src.Updateopt		
				,src.Updatedt				
			
			);
		--	select * from M_ITEM where ITemCD = 'ac223'


			--select *  from #tempItem
			drop table dbo.#tempItem
			End

END
