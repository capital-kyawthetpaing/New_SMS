 BEGIN TRY 
 Drop Procedure dbo.[_Item_SKU]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[_Item_SKU]
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

    -- Insert statements for procedure here
	--	@Date datetime =CONVERT(varchar,getdate(),120),
	--@Opt as varchar(10) = '0001',
	declare
	@DocHandle as int,
	@Upt as int =1,
	@Ins as int = 1;
	DECLARE @Counter INT ;
	DECLARE @SC INT;
	DECLARE @CC int;
	Declare @ItmCD  varchar(50);
	declare  @tempAdmin as int =0;
			
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
			--select *  from #tempItem where ITemCD = 'AAAA'
			
			 WITH cte AS ( SELECT  ROW_NUMBER() OVER (PARTITION BY ItemCD,ChangeDate ORDER BY ItemCD,ChangeDate ) row_num FROM  #tempItem) delete FROM cte WHERE row_num > 1;
			Update #tempItem set SizeCount = 1 , ColorCount = 1 where SizeCount= 0 and ColorCount =0
	if @MainFlg =1 or @MainFlg =2

	Begin
	


			Create table dbo.tempSKU(
																						AdminNO				int						
																						,ChangeDate			date				
																						,SKUCD				varchar(100)	
																						,VariousFLG			int					
																						,SKUName			 varchar(200)					
																						,SKUNameLong		 varchar(2000)					
																						,KanaName			 varchar(200)						
																						,SKUShortName			 varchar(200)						
																						,EnglishName		 varchar(200)							
																						,ITemCD				 varchar(200)					
																						,SizeNO				int			
																						,ColorNO				int			
																						,JanCD				 varchar(200)					
																						,SetKBN				int			
																						,PresentKBN				int			
																						,SampleKBN			int				
																						,DiscountKBN		int					
																						,SizeName			 varchar(200)						
																						,ColorName			 varchar(200)						
																						,SizeNameLong		 varchar(200)							
																						,ColorNameLong		 varchar(200)							
																						,WebFlg					int		
																						,RealStoreFlg			int				
																						,MainVendorCD			int				
																						,MakerVendorCD		 varchar(200)							
																						,BrandCD			 varchar(200)						
																						,MakerItem			 varchar(200)						
																						,TaniCD				 varchar(200)					
																						,SportsCD			 varchar(200)						
																						,SegmentCD			 varchar(200)						
																						,ZaikoKBN			int				
																						,Rack				 varchar(200)					
																						,VirtualFlg			int				
																						,DirectFlg			int				
																						,ReserveCD			 varchar(200)						
																						,NoticesCD			 varchar(200)						
																						,PostageCD			 varchar(200)						
																						,ManufactCD			 varchar(200)						
																						,ConfirmCD			 varchar(200)						
																						,WebStockFlg		int					
																						,StopFlg			int				
																						,DiscontinueFlg		int					
																						,SoldOutFlg			int				
																						,InventoryAddFlg	int						
																						,MakerAddFlg		int					
																						,StoreAddFlg		int					
																						,NoNetOrderFlg		int					
																						,EDIOrderFlg		int					
																						,AutoOrderFlg		int					
																						,CatalogFlg			int				
																						,ParcelFlg			int				
																						,TaxRateFLG			int				
																						,CostingKBN			int				
																						,NormalCost			money				
																						,SaleExcludedFlg	int						
																						,PriceWithTax		money					
																						,PriceOutTax		money					
																						,OrderPriceWithTax	money						
																						,OrderPriceWithoutTax money							
																						,Rate					decimal(5,2)		
																						,SaleStartDate					date		
																						,WebStartDate			date				
																						,OrderAttentionCD		 varchar(200)							
																						,OrderAttentionNote		 varchar(200)							
																						,CommentInStore			 varchar(300)						
																						,CommentOutStore		 varchar(300)							
																						,LastYearTerm			 varchar(200)						
																						,LastSeason				 varchar(200)					
																						,LastCatalogNO			 varchar(200)						
																						,LastCatalogPage		 varchar(200)							
																						,LastCatalogNOLong		 varchar(2000)							
																						,LastCatalogPageLong	 varchar(2000)								
																						,LastCatalogText		 varchar(1000)							
																						,LastInstructionsNO		 varchar(1000)							
																						,LastInstructionsDate	date						
																						,WebAddress				 varchar(200)					
																						,SetAdminCD				 varchar(200)					
																						,SetItemCD				 varchar(200)					
																						,SetSKUCD				 varchar(200)					
																						,SetSU					int		
																						,ExhibitionSegmentCD	 varchar(200)								
																						,OrderLot				int			
																						,ExhibitionCommonCD		 varchar(200)							
																						,ApprovalDate			date				
																						,DeleteFlg				int			
																						,UsedFlg				int			
																						,SKSUpdateFlg			int				
																						,SKSUpdateDateTime		datetime					
																						,InsertOperator				 varchar(200)					
																						,InsertDateTime				datetime			
																						,UpdateOperator				 varchar(200)					
																						,UpdateDateTime				datetime			
																						)
			Create table dbo.CountSetting (EmpRows int);
								
									SET @Counter=0
									WHILE ( @Counter <= 50)
									BEGIN
									   
									    SET @Counter  = @Counter  + 1

										insert into  CountSetting(Emprows)
										values (@Counter)
									END
								
								--DECLARE @SC INT;
								--DECLARE @CC int;
								--Declare @ItmCD  varchar(50);
								--declare  @tempAdmin as int =0;
								DECLARE CUR_TEST CURSOR FAST_FORWARD FOR
								    SELECT ItemCD, SizeCount ,ColorCount
								    FROM   #tempItem
								    ORDER BY SizeCount;
								 
								OPEN CUR_TEST
								FETCH NEXT FROM CUR_TEST INTO @ItmCD, @SC , @CC

								WHILE @@FETCH_STATUS = 0
								BEGIN
								set @tempAdmin= (select max(AdminNo) from M_SKUCounter );
								--select @tempAdmin
								insert into tempSKU(ITemCD,SKUCD ,AdminNO )
							select  ItemCD, SKUCD , ( ROW_NUMBER () Over (order by SKUCD desc) + @tempAdmin ) from (
								select @ItmCD as ItemCD, (@ItmCD + Cast(SizeCount as varchar) + cast(ColorCount as varchar) ) as SKUCD 
								   from (  select * from (select  right('0000' +  Cast (EmpRows as varchar), 4) as SizeCount  from CountSetting where EmpRows  <= @SC) a) c
								  cross join 
								 ( select *  from (select  right('0000' +  Cast (EmpRows as varchar), 4) as ColorCount  from CountSetting where EmpRows  <= @CC ) a) d  
								  ) abc
								  set @tempAdmin= (select coalesce(MAX(AdminNo ), 0) from tempSKU );
							    -- delete M_SKUCounter where MainKEY =1
								-- insert M_SKUCounter (AdminNO, MainKEY, UpdateOperator,UpdateDateTime) values (@tempAdmin, 1,@Opt,@Upt)  
								 Update M_SKUCounter  set AdminNo = @tempAdmin+1 where mainkey =1 
								-- insert M_SKUCounter (AdminNO, MainKEY, UpdateOperator,UpdateDateTime) values (1400000, 1,0001,getdate())
								   FETCH NEXT FROM CUR_TEST INTO @ItmCD, @SC , @CC
								
								END
								CLOSE CUR_TEST
								DEALLOCATE CUR_TEST
								 
								 Update  tsku
								 set 
								-- tsku.AdminNo= titem.Adm
								 tsku.ChangeDate												  = titem.ChangeDate
								 ,tsku.VariousFLG													=titem.VariousFLG
								 ,tsku.SKUName													  =titem.ITemName			
								 ,tsku.SKUNameLong												  =titem.ITemName		
								 ,tsku.KanaName													  =titem.KanaName			
								 ,tsku.SKUShortName												  =titem.ITEMShortName		
								 ,tsku.EnglishName												  =titem.EnglishName		
								 ,tsku.ITemCD													  =titem.ITemCD				
								 ,tsku.SizeNO													  =titem.SizeCount				
								 ,tsku.ColorNO													  =titem.ColorCount			
								 ,tsku.JanCD													  = Null
								 ,tsku.SetKBN													  =titem.SetKBN				
								 ,tsku.PresentKBN												  =titem.PresentKBN			
								 ,tsku.SampleKBN												  =titem.SampleKBN			
								 ,tsku.DiscountKBN												  =titem.DiscountKBN		
								 ,tsku.SizeName													  = null			
								 ,tsku.ColorName												  = null				
								 ,tsku.SizeNameLong												  =null		
								 ,tsku.ColorNameLong											  =null		
								 ,tsku.WebFlg													  =titem.WebFlg				
								 ,tsku.RealStoreFlg												  =titem.RealStoreFlg		
								 ,tsku.MainVendorCD												  =titem.MainVendorCD		
								 ,tsku.MakerVendorCD											  =titem.MainVendorCD		
								 ,tsku.BrandCD													  =titem.BrandCD			
								 ,tsku.MakerItem												  =titem.MakerItem			
								 ,tsku.TaniCD													  =titem.TaniCD				
								 ,tsku.SportsCD													  =titem.SportsCD			
								 ,tsku.SegmentCD												  =titem.SegmentCD			
								 ,tsku.ZaikoKBN													  =titem.ZaikoKBN			
								 ,tsku.Rack														  =titem.Rack				
								 ,tsku.VirtualFlg												  =titem.VirtualFlg			
								 ,tsku.DirectFlg												  =titem.DirectFlg			
								 ,tsku.ReserveCD												  =titem.ReserveCD			
								 ,tsku.NoticesCD												  =titem.NoticesCD			
								 ,tsku.PostageCD												  =titem.PostageCD			
								 ,tsku.ManufactCD												  =titem.ManufactCD			
								 ,tsku.ConfirmCD												  =titem.ConfirmCD			
								 ,tsku.WebStockFlg												  =titem.WebStockFlg		
								 ,tsku.StopFlg													  =titem.StopFlg			
								 ,tsku.DiscontinueFlg											  =titem.DiscontinueFlg		
								 ,tsku.SoldOutFlg												  =titem.SoldOutFlg			
								 ,tsku.InventoryAddFlg											  =titem.InventoryAddFlg	
								 ,tsku.MakerAddFlg												  =titem.MakerAddFlg		
								 ,tsku.StoreAddFlg												  =titem.StoreAddFlg		
								 ,tsku.NoNetOrderFlg											  =titem.NoNetOrderFlg		
								 ,tsku.EDIOrderFlg												  =titem.EDIOrderFlg		
								 ,tsku.AutoOrderFlg												  =titem.AutoOrderFlg		
								 ,tsku.CatalogFlg												  =titem.CatalogFlg			
								 ,tsku.ParcelFlg												  =titem.ParcelFlg			
								 ,tsku.TaxRateFLG												  =titem.TaxRateFLG			
								 ,tsku.CostingKBN												  =titem.CostingKBN			
								 ,tsku.NormalCost												  =titem.NormalCost			
								 ,tsku.SaleExcludedFlg											  =titem.SaleExcludedFlg	
								 ,tsku.PriceWithTax												  =titem.PriceWithTax		
								 ,tsku.PriceOutTax												  =titem.PriceOutTax		
								 ,tsku.OrderPriceWithTax										  =titem.OrderPriceWithTax	
								 ,tsku.OrderPriceWithoutTax										  =titem.OrderPriceWithoutTax
								 ,tsku.Rate														  =titem.Rate				
								 ,tsku.SaleStartDate											  =titem.SaleStartDate		
								 ,tsku.WebStartDate												  =titem.WebStartDate		
								 ,tsku.OrderAttentionCD											  =titem.OrderAttentionCD	
								 ,tsku.OrderAttentionNote										  =titem.OrderAttentionNote	
								 ,tsku.CommentInStore											  =titem.CommentInStore		
								 ,tsku.CommentOutStore											  =titem.CommentOutStore	
								 ,tsku.LastYearTerm												  =titem.LastYearTerm		
								 ,tsku.LastSeason												  =titem.LastSeason			
								 ,tsku.LastCatalogNO											  =titem.LastCatalogNO		
								 ,tsku.LastCatalogPage											  =titem.LastCatalogPage	
								 --,tsku.LastCatalogNOLong										  =titem.LastCatalogNOLong	
								 --,tsku.LastCatalogPageLong										  =titem.LastCatalogPageLong
								 ,tsku.LastCatalogText											  =titem.LastCatalogText	
								 ,tsku.LastInstructionsNO										  =titem.LastInstructionsNO	
								 ,tsku.LastInstructionsDate										  =titem.LastInstructionsDate
								 ,tsku.WebAddress												  =titem.WebAddress			
								 ,tsku.SetAdminCD												  = '0'			
								 ,tsku.SetItemCD												  =null		
								 ,tsku.SetSKUCD													  =null			
								 ,tsku.SetSU													  =0		
								 ,tsku.ExhibitionSegmentCD										  =titem.ExhibitionSegmentCD
								 ,tsku.OrderLot													  =titem.OrderLot			
								 ,tsku.ExhibitionCommonCD										  = null	
								 ,tsku.ApprovalDate												  =titem.ApprovalDate		
								 ,tsku.DeleteFlg												  =titem.DeleteFlg			
								 ,tsku.UsedFlg													  = 0			
								 ,tsku.SKSUpdateFlg												  =1		
								 ,tsku.SKSUpdateDateTime										  =null
								 ,tsku.InsertOperator											  =@Opt		
								 ,tsku.InsertDateTime											  =@Date		
								 ,tsku.UpdateOperator											  =@Opt		
								 ,tsku.UpdateDateTime											  =@Date		

								 from tempSKU tsku
								 inner join #tempItem titem
								 on 
								 tsku.ItemCD = titem.ItemCD

								 Update tempSKU set SizeNo = Cast (  Left(RIGHT(SKUCD, 8), 4)  as int) , ColorNo = Cast (  RIGHT(SKUCD, 4)  as int)
							     insert into M_SKU
								 select * from tempSKU order by AdminNo,SKUCD asc
																										
								--	Delete  from M_SKU where AdminNO =1

				--SELECT COUNT(*) 
				--FROM INFORMATION_SCHEMA.COLUMNS WHERE 
				--TABLE_NAME = 'tempSKU';
			 -- select * from M_SKUCounter
			 --select Max(ExhibitionSegmentCD) from tempSKU
			
			
			drop table tempSKU			
			drop table CountSetting		
	End
	if @MainFlg =6
	Begin

	  Delete from M_SKU where ITemCD in (
	  select fsk.ItemCd  from F_SKU(GETDATE()) fsk 
	  inner join #tempItem tim on fsk.ITemCD = tim.ITemCD and fsk.ChangeDate = tim.ChangeDate and fsk.DeleteFlg = 0)

	  	Create table dbo.tempSKU(
																						AdminNO				int						
																						,ChangeDate			date				
																						,SKUCD				varchar(100)	
																						,VariousFLG			int					
																						,SKUName			 varchar(200)					
																						,SKUNameLong		 varchar(2000)					
																						,KanaName			 varchar(200)						
																						,SKUShortName			 varchar(200)						
																						,EnglishName		 varchar(200)							
																						,ITemCD				 varchar(200)					
																						,SizeNO				int			
																						,ColorNO				int			
																						,JanCD				 varchar(200)					
																						,SetKBN				int			
																						,PresentKBN				int			
																						,SampleKBN			int				
																						,DiscountKBN		int					
																						,SizeName			 varchar(200)						
																						,ColorName			 varchar(200)						
																						,SizeNameLong		 varchar(200)							
																						,ColorNameLong		 varchar(200)							
																						,WebFlg					int		
																						,RealStoreFlg			int				
																						,MainVendorCD			int				
																						,MakerVendorCD		 varchar(200)							
																						,BrandCD			 varchar(200)						
																						,MakerItem			 varchar(200)						
																						,TaniCD				 varchar(200)					
																						,SportsCD			 varchar(200)						
																						,SegmentCD			 varchar(200)						
																						,ZaikoKBN			int				
																						,Rack				 varchar(200)					
																						,VirtualFlg			int				
																						,DirectFlg			int				
																						,ReserveCD			 varchar(200)						
																						,NoticesCD			 varchar(200)						
																						,PostageCD			 varchar(200)						
																						,ManufactCD			 varchar(200)						
																						,ConfirmCD			 varchar(200)						
																						,WebStockFlg		int					
																						,StopFlg			int				
																						,DiscontinueFlg		int					
																						,SoldOutFlg			int				
																						,InventoryAddFlg	int						
																						,MakerAddFlg		int					
																						,StoreAddFlg		int					
																						,NoNetOrderFlg		int					
																						,EDIOrderFlg		int					
																						,AutoOrderFlg		int					
																						,CatalogFlg			int				
																						,ParcelFlg			int				
																						,TaxRateFLG			int				
																						,CostingKBN			int				
																						,NormalCost			money				
																						,SaleExcludedFlg	int						
																						,PriceWithTax		money					
																						,PriceOutTax		money					
																						,OrderPriceWithTax	money						
																						,OrderPriceWithoutTax money							
																						,Rate					decimal(5,2)		
																						,SaleStartDate					date		
																						,WebStartDate			date				
																						,OrderAttentionCD		 varchar(200)							
																						,OrderAttentionNote		 varchar(200)							
																						,CommentInStore			 varchar(300)						
																						,CommentOutStore		 varchar(300)							
																						,LastYearTerm			 varchar(200)						
																						,LastSeason				 varchar(200)					
																						,LastCatalogNO			 varchar(200)						
																						,LastCatalogPage		 varchar(200)							
																						,LastCatalogNOLong		 varchar(2000)							
																						,LastCatalogPageLong	 varchar(2000)								
																						,LastCatalogText		 varchar(1000)							
																						,LastInstructionsNO		 varchar(1000)							
																						,LastInstructionsDate	date						
																						,WebAddress				 varchar(200)					
																						,SetAdminCD				 varchar(200)					
																						,SetItemCD				 varchar(200)					
																						,SetSKUCD				 varchar(200)					
																						,SetSU					int		
																						,ExhibitionSegmentCD	 varchar(200)								
																						,OrderLot				int			
																						,ExhibitionCommonCD		 varchar(200)							
																						,ApprovalDate			date				
																						,DeleteFlg				int			
																						,UsedFlg				int			
																						,SKSUpdateFlg			int				
																						,SKSUpdateDateTime		datetime					
																						,InsertOperator				 varchar(200)					
																						,InsertDateTime				datetime			
																						,UpdateOperator				 varchar(200)					
																						,UpdateDateTime				datetime			
																						)
		Create table dbo.CountSetting (EmpRows int);
							--	DECLARE @Counter INT ;
									SET @Counter=0
									WHILE ( @Counter <= 50)
									BEGIN
									   
									    SET @Counter  = @Counter  + 1

										insert into  CountSetting(Emprows)
										values (@Counter)
									END
								
						
								DECLARE CUR_TEST CURSOR FAST_FORWARD FOR
								    SELECT ItemCD, SizeCount ,ColorCount
								    FROM   #tempItem
								    ORDER BY SizeCount;
								 
								OPEN CUR_TEST
								FETCH NEXT FROM CUR_TEST INTO @ItmCD, @SC , @CC

								WHILE @@FETCH_STATUS = 0
								BEGIN
								set @tempAdmin= (select max(AdminNo) from M_SKUCounter );
								--select @tempAdmin
								insert into tempSKU(ITemCD,SKUCD ,AdminNO )
							select  ItemCD, SKUCD , ( ROW_NUMBER () Over (order by SKUCD desc) + @tempAdmin ) from (
								select @ItmCD as ItemCD, (@ItmCD + Cast(SizeCount as varchar) + cast(ColorCount as varchar) ) as SKUCD 
								   from (  select * from (select  right('0000' +  Cast (EmpRows as varchar), 4) as SizeCount  from CountSetting where EmpRows  <= @SC) a) c
								  cross join 
								 ( select *  from (select  right('0000' +  Cast (EmpRows as varchar), 4) as ColorCount  from CountSetting where EmpRows  <= @CC ) a) d  
								  ) abc
								  set @tempAdmin= (select coalesce(MAX(AdminNo ), 0) from tempSKU );
							    -- delete M_SKUCounter where MainKEY =1
								-- insert M_SKUCounter (AdminNO, MainKEY, UpdateOperator,UpdateDateTime) values (@tempAdmin, 1,@Opt,@Upt)  
								 Update M_SKUCounter  set AdminNo = @tempAdmin+1 where mainkey =1 
								-- insert M_SKUCounter (AdminNO, MainKEY, UpdateOperator,UpdateDateTime) values (1400000, 1,0001,getdate())
								   FETCH NEXT FROM CUR_TEST INTO @ItmCD, @SC , @CC
								
								END
								CLOSE CUR_TEST
								DEALLOCATE CUR_TEST
								 
								 Update  tsku
								 set 
								-- tsku.AdminNo= titem.Adm
								 tsku.ChangeDate												  = titem.ChangeDate
								 ,tsku.VariousFLG													=titem.VariousFLG
								 ,tsku.SKUName													  =titem.ITemName			
								 ,tsku.SKUNameLong												  =titem.ITemName		
								 ,tsku.KanaName													  =titem.KanaName			
								 ,tsku.SKUShortName												  =titem.ITEMShortName		
								 ,tsku.EnglishName												  =titem.EnglishName		
								 ,tsku.ITemCD													  =titem.ITemCD				
								 ,tsku.SizeNO													  =titem.SizeCount				
								 ,tsku.ColorNO													  =titem.ColorCount			
								 ,tsku.JanCD													  = Null
								 ,tsku.SetKBN													  =titem.SetKBN				
								 ,tsku.PresentKBN												  =titem.PresentKBN			
								 ,tsku.SampleKBN												  =titem.SampleKBN			
								 ,tsku.DiscountKBN												  =titem.DiscountKBN		
								 ,tsku.SizeName													  = null			
								 ,tsku.ColorName												  = null				
								 ,tsku.SizeNameLong												  =null		
								 ,tsku.ColorNameLong											  =null		
								 ,tsku.WebFlg													  =titem.WebFlg				
								 ,tsku.RealStoreFlg												  =titem.RealStoreFlg		
								 ,tsku.MainVendorCD												  =titem.MainVendorCD		
								 ,tsku.MakerVendorCD											  =titem.MainVendorCD		
								 ,tsku.BrandCD													  =titem.BrandCD			
								 ,tsku.MakerItem												  =titem.MakerItem			
								 ,tsku.TaniCD													  =titem.TaniCD				
								 ,tsku.SportsCD													  =titem.SportsCD			
								 ,tsku.SegmentCD												  =titem.SegmentCD			
								 ,tsku.ZaikoKBN													  =titem.ZaikoKBN			
								 ,tsku.Rack														  =titem.Rack				
								 ,tsku.VirtualFlg												  =titem.VirtualFlg			
								 ,tsku.DirectFlg												  =titem.DirectFlg			
								 ,tsku.ReserveCD												  =titem.ReserveCD			
								 ,tsku.NoticesCD												  =titem.NoticesCD			
								 ,tsku.PostageCD												  =titem.PostageCD			
								 ,tsku.ManufactCD												  =titem.ManufactCD			
								 ,tsku.ConfirmCD												  =titem.ConfirmCD			
								 ,tsku.WebStockFlg												  =titem.WebStockFlg		
								 ,tsku.StopFlg													  =titem.StopFlg			
								 ,tsku.DiscontinueFlg											  =titem.DiscontinueFlg		
								 ,tsku.SoldOutFlg												  =titem.SoldOutFlg			
								 ,tsku.InventoryAddFlg											  =titem.InventoryAddFlg	
								 ,tsku.MakerAddFlg												  =titem.MakerAddFlg		
								 ,tsku.StoreAddFlg												  =titem.StoreAddFlg		
								 ,tsku.NoNetOrderFlg											  =titem.NoNetOrderFlg		
								 ,tsku.EDIOrderFlg												  =titem.EDIOrderFlg		
								 ,tsku.AutoOrderFlg												  =titem.AutoOrderFlg		
								 ,tsku.CatalogFlg												  =titem.CatalogFlg			
								 ,tsku.ParcelFlg												  =titem.ParcelFlg			
								 ,tsku.TaxRateFLG												  =titem.TaxRateFLG			
								 ,tsku.CostingKBN												  =titem.CostingKBN			
								 ,tsku.NormalCost												  =titem.NormalCost			
								 ,tsku.SaleExcludedFlg											  =titem.SaleExcludedFlg	
								 ,tsku.PriceWithTax												  =titem.PriceWithTax		
								 ,tsku.PriceOutTax												  =titem.PriceOutTax		
								 ,tsku.OrderPriceWithTax										  =titem.OrderPriceWithTax	
								 ,tsku.OrderPriceWithoutTax										  =titem.OrderPriceWithoutTax
								 ,tsku.Rate														  =titem.Rate				
								 ,tsku.SaleStartDate											  =titem.SaleStartDate		
								 ,tsku.WebStartDate												  =titem.WebStartDate		
								 ,tsku.OrderAttentionCD											  =titem.OrderAttentionCD	
								 ,tsku.OrderAttentionNote										  =titem.OrderAttentionNote	
								 ,tsku.CommentInStore											  =titem.CommentInStore		
								 ,tsku.CommentOutStore											  =titem.CommentOutStore	
								 ,tsku.LastYearTerm												  =titem.LastYearTerm		
								 ,tsku.LastSeason												  =titem.LastSeason			
								 ,tsku.LastCatalogNO											  =titem.LastCatalogNO		
								 ,tsku.LastCatalogPage											  =titem.LastCatalogPage	
								 --,tsku.LastCatalogNOLong										  =titem.LastCatalogNOLong	
								 --,tsku.LastCatalogPageLong										  =titem.LastCatalogPageLong
								 ,tsku.LastCatalogText											  =titem.LastCatalogText	
								 ,tsku.LastInstructionsNO										  =titem.LastInstructionsNO	
								 ,tsku.LastInstructionsDate										  =titem.LastInstructionsDate
								 ,tsku.WebAddress												  =titem.WebAddress			
								 ,tsku.SetAdminCD												  = '0'			
								 ,tsku.SetItemCD												  =null		
								 ,tsku.SetSKUCD													  =null			
								 ,tsku.SetSU													  =0		
								 ,tsku.ExhibitionSegmentCD										  =titem.ExhibitionSegmentCD
								 ,tsku.OrderLot													  =titem.OrderLot			
								 ,tsku.ExhibitionCommonCD										  = null	
								 ,tsku.ApprovalDate												  =titem.ApprovalDate		
								 ,tsku.DeleteFlg												  =titem.DeleteFlg			
								 ,tsku.UsedFlg													  = 0			
								 ,tsku.SKSUpdateFlg												  =1		
								 ,tsku.SKSUpdateDateTime										  =null
								 ,tsku.InsertOperator											  =@Opt		
								 ,tsku.InsertDateTime											  =@Date		
								 ,tsku.UpdateOperator											  =@Opt		
								 ,tsku.UpdateDateTime											  =@Date		

								 from tempSKU tsku
								 inner join #tempItem titem
								 on 
								 tsku.ItemCD = titem.ItemCD

								 
							     insert into M_SKU
								 select * from tempSKU order by AdminNo,SKUCD asc


	  drop table tempSKU			
			drop table CountSetting	
	End

	drop table dbo.#tempItem
END