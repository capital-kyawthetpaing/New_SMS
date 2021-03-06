
BEGIN TRY 
 Drop Procedure [dbo].[MasterTorikomi_SKU_Insert_Update]
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
CREATE PROCEDURE [dbo].[MasterTorikomi_SKU_Insert_Update]
	-- Add the parameters for the stored procedure here
	@xml as Xml,
	@type as int,
	@OperatorCD as varchar(10),
	@ProgramID as varchar(100),
	@PC as varchar(30),
	@KeyItem as varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		declare @AdminCounter as int = (select Max (AdminNo) from M_SKUCounter where MainKEY = 1);
		declare @AdminCounterJ as int = (select MAX(JanCount) from M_JANCounter Where MainKEY=1);
	if @type =1
		Begin
		Create table [#tmpSKU1](
	
				[AdminNO] [varchar](50) NOT NULL,
				[ChangeDate] [date] NOT NULL,
				[SKUCD] [varchar](40) NULL,
				[VariousFLG] [tinyint] NULL,
				[SKUName] [varchar](100) NULL,
				[KanaName] [varchar](50) NULL,
				[SKUShortName] [varchar](40) NULL,
				[EnglishName] [varchar](80) NULL,
				[ITEMCD] [varchar](30) NULL,
				[SizeNO] [int] NULL,
				[ColorNO] [int] NULL,
				[JanCD] [varchar](13) NULL,
				[SetKBN] [tinyint] NULL,
				[PresentKBN] [tinyint] NULL,
				[SampleKBN] [tinyint] NULL,
				[DiscountKBN] [tinyint] NULL,
				[SizeName] [varchar](20) NULL,
				[ColorName] [varchar](20) NULL,
				[WebFlg] [tinyint] NULL,
				[RealStoreFlg] [tinyint] NULL,
				[MainVendorCD] [varchar](13) NULL,
				[BrandCD] [varchar](6) NULL,
				[MakerItem] [varchar](50) NULL,
				[TaniCD] [varchar](2) NULL,
				[SportsCD] [varchar](6) NULL,
				[SegmentCD] [varchar](6) NULL,
				[ZaikoKBN] [tinyint] NULL,
				[Rack] [varchar](10) NULL,
				[VirtualFlg] [tinyint] NULL,
				[DirectFlg] [tinyint] NULL,
				[ReserveCD] [varchar](3) NULL,
				[NoticesCD] [varchar](3) NULL,
				[PostageCD] [varchar](3) NULL,
				[ManufactCD] [varchar](3) NULL,
				[ConfirmCD] [varchar](3) NULL,
				[WebStockFlg] [tinyint] NULL,
				[StopFlg] [tinyint] NULL,
				[DiscontinueFlg] [tinyint] NULL,
				[SoldOutFlg] [tinyint] NULL,
				[InventoryAddFlg] [tinyint] NULL,
				[MakerAddFlg] [tinyint] NULL,
				[StoreAddFlg] [tinyint] NULL,
				[NoNetOrderFlg] [tinyint] NULL,
				[EDIOrderFlg] [tinyint] NULL,
				[AutoOrderFlg] [tinyint] NULL,
				[CatalogFlg] [tinyint] NULL,
				[ParcelFlg] [tinyint] NULL,
				[TaxRateFLG] [tinyint] NULL,
				[CostingKBN] [tinyint] NULL,
				[SaleExcludedFlg] [tinyint] NULL,
				[NormalCost] [money] NOT NULL,
				[PriceWithTax] [money] NULL,
				[PriceOutTax] [money] NULL,
				[OrderPriceWithTax] [money] NULL,
				[OrderPriceWithoutTax] [money] NULL,
				[Rate] [decimal](5, 2) NULL,
				[SaleStartDate] [date] NULL,
				[WebStartDate] [date] NULL,
				[OrderAttentionCD] [varchar](3) NULL,
				[OrderAttentionNote] [varchar](100) NULL,
				[CommentInStore] [varchar](300) NULL,
				[CommentOutStore] [varchar](300) NULL,
				[LastYearTerm] [varchar](6) NULL,
				[LastSeason] [varchar](6) NULL,
				[LastCatalogNO] [varchar](20) NULL,
				[LastCatalogPage] [varchar](20) NULL,
				[LastCatalogNOLong] [varchar](2000) NULL,
				[LastCatalogPageLong] [varchar](2000) NULL,
				[LastCatalogText] [varchar](1000) NULL,
				[LastInstructionsNO] [varchar](1000) NULL,
				[LastInstructionsDate] [date] NULL,
				[WebAddress] [varchar](200) NULL,
				[SetAdminCD] [int] NULL,
				[SetItemCD] [varchar](30) NULL,
				[SetSKUCD] [varchar](30) NULL,
				[SetSU] [int] NULL,
				[ExhibitionSegmentCD] [varchar](5) NULL,
				[OrderLot] [int] NULL,
				[ExhibitionCommonCD] [varchar](30) NULL,
				[ApprovalDate] [date] NULL,
				[DeleteFlg] [tinyint] NULL,
				[TagName1] [varchar](20) NULL,
				[TagName2] [varchar](20) NULL,
				[TagName3] [varchar](20) NULL,
				[TagName4] [varchar](20) NULL,
				[TagName5] [varchar](20) NULL,
				[TagName6] [varchar](20) NULL,
				[TagName7] [varchar](20) NULL,
				[TagName8] [varchar](20) NULL,
				[TagName9] [varchar](20) NULL,
				[TagName10] [varchar](20) NULL
				)
				declare @DocHandle1 int

				exec sp_xml_preparedocument @DocHandle1 output, @xml
				INSERT Into #tmpSKU1
				select *  FROM OPENXML (@DocHandle1, '/NewDataSet/test',2)
						with
						(
				AdminNO varchar(50),
				改定日 date,
				SKUCD varchar(40) ,
				諸口区分 tinyint,
				商品名 varchar(100),
				カナ名 varchar(50),
				略名 varchar(40),
				英語名 varchar(80),
				ITEMCD varchar(30),
				サイズ枝番 int,
				カラー枝番 int,
				JANCD varchar(13),
				セット品区分 tinyint,
				プレゼント品区分 tinyint ,
				サンプル品区分 tinyint ,
				値引商品区分 tinyint,
				サイズ名 varchar(20) ,
				カラー名 varchar(20) ,
				Webストア取扱区分 tinyint ,
				実店舗取扱区分 tinyint ,
				主要仕入先CD varchar(13) ,
				ブランドCD varchar(6) ,
				メーカー商品CD varchar(50) ,
				単位CD varchar(2) ,
				競技CD varchar(6) ,
				商品分類CD varchar(6) ,
				在庫管理対象区分 tinyint ,
				棚番 varchar(10) ,
				架空商品区分 tinyint ,
				直送品区分 tinyint ,
				予約品区分 varchar(3) ,
				特記区分 varchar(3) ,
				送料区分 varchar(3) ,
				要加工品区分 varchar(3) ,
				要確認品区分 varchar(3) ,
				Web在庫連携区分 tinyint ,
				販売停止品区分 tinyint ,
				廃番品区分 tinyint ,
				完売品区分 tinyint ,
				自社在庫連携対象 tinyint ,
				メーカー在庫連携対象 tinyint ,
				店舗在庫連携対象 tinyint ,
				Net発注不可区分 tinyint ,
				EDI発注可能区分 tinyint ,
				自動発注対象区分 tinyint ,
				カタログ掲載有無区分 tinyint ,
				小包梱包可能区分 tinyint ,
				税率区分 tinyint ,
				原価計算方法 tinyint ,
				Sale対象外区分 tinyint ,
				標準原価 money  ,
				税抜定価 money ,
				税込定価 money ,
				発注税込価格 money ,
				発注税抜価格 money ,
				掛率 decimal(5, 2) ,
				発売開始日 date ,
				Web掲載開始日 date,
				発注注意区分 varchar(3) ,
				発注注意事項 varchar(100) ,
				管理用備考 varchar(300) ,
				表示用備考 varchar(300) ,
				年度 varchar(6) ,
				シーズン varchar(6) ,
				カタログ番号 varchar(20) ,
				カタログページ varchar(20) ,
				カタログ番号Long varchar(2000) ,
				カタログページLong varchar(2000) ,
				カタログ情報 varchar(1000),
				指示書番号 varchar(1000),
				指示書発行日 date,
				商品情報アドレス varchar(200),
				SetAdminCD int ,
				SetItemCD varchar(30),
				SetSKUCD varchar(30),
				構成数 int,
				ExhibitionSegmentCD varchar(5),
				発注ロット int ,
				ExhibitionCommonCD varchar(30),
				承認日 date ,
				削除 tinyint ,
				タグ1  varchar(6) ,
				タグ2  varchar(6) ,
				タグ3  varchar(20) ,
				タグ4 varchar(20) ,
				タグ5  varchar(20) ,
				タグ6  varchar(20) ,
				タグ7  varchar(20),
				タグ8 varchar(20),
				タグ9 varchar(20),
				タグ10 varchar(20)
				)
				exec sp_xml_removedocument @DocHandle1;
				
				Create table [#tmpSKU1_N](
				[LastNo][int] Not Null,
				[LastJanNo][varchar](13) NULL,
				[AdminNO] [varchar](50) NOT NULL,
				[ChangeDate] [date] NOT NULL,
				[SKUCD] [varchar](40) NULL,
				[VariousFLG] [tinyint] NULL,
				[SKUName] [varchar](100) NULL,
				[KanaName] [varchar](50) NULL,
				[SKUShortName] [varchar](40) NULL,
				[EnglishName] [varchar](80) NULL,
				[ITEMCD] [varchar](30) NULL,
				[SizeNO] [int] NULL,
				[ColorNO] [int] NULL,
				[JanCD] [varchar](13) NULL,
				[SetKBN] [tinyint] NULL,
				[PresentKBN] [tinyint] NULL,
				[SampleKBN] [tinyint] NULL,
				[DiscountKBN] [tinyint] NULL,
				[SizeName] [varchar](20) NULL,
				[ColorName] [varchar](20) NULL,
				[WebFlg] [tinyint] NULL,
				[RealStoreFlg] [tinyint] NULL,
				[MainVendorCD] [varchar](13) NULL,
				[BrandCD] [varchar](6) NULL,
				[MakerItem] [varchar](50) NULL,
				[TaniCD] [varchar](2) NULL,
				[SportsCD] [varchar](6) NULL,
				[SegmentCD] [varchar](6) NULL,
				[ZaikoKBN] [tinyint] NULL,
				[Rack] [varchar](10) NULL,
				[VirtualFlg] [tinyint] NULL,
				[DirectFlg] [tinyint] NULL,
				[ReserveCD] [varchar](3) NULL,
				[NoticesCD] [varchar](3) NULL,
				[PostageCD] [varchar](3) NULL,
				[ManufactCD] [varchar](3) NULL,
				[ConfirmCD] [varchar](3) NULL,
				[WebStockFlg] [tinyint] NULL,
				[StopFlg] [tinyint] NULL,
				[DiscontinueFlg] [tinyint] NULL,
				[SoldOutFlg] [tinyint] NULL,
				[InventoryAddFlg] [tinyint] NULL,
				[MakerAddFlg] [tinyint] NULL,
				[StoreAddFlg] [tinyint] NULL,
				[NoNetOrderFlg] [tinyint] NULL,
				[EDIOrderFlg] [tinyint] NULL,
				[AutoOrderFlg] [tinyint] NULL,
				[CatalogFlg] [tinyint] NULL,
				[ParcelFlg] [tinyint] NULL,
				[TaxRateFLG] [tinyint] NULL,
				[CostingKBN] [tinyint] NULL,
				[SaleExcludedFlg] [tinyint] NULL,
				[NormalCost] [money] NOT NULL,
				[PriceWithTax] [money] NULL,
				[PriceOutTax] [money] NULL,
				[OrderPriceWithTax] [money] NULL,
				[OrderPriceWithoutTax] [money] NULL,
				[Rate] [decimal](5, 2) NULL,
				[SaleStartDate] [date] NULL,
				[WebStartDate] [date] NULL,
				[OrderAttentionCD] [varchar](3) NULL,
				[OrderAttentionNote] [varchar](100) NULL,
				[CommentInStore] [varchar](300) NULL,
				[CommentOutStore] [varchar](300) NULL,
				[LastYearTerm] [varchar](6) NULL,
				[LastSeason] [varchar](6) NULL,
				[LastCatalogNO] [varchar](20) NULL,
				[LastCatalogPage] [varchar](20) NULL,
				[LastCatalogNOLong] [varchar](2000) NULL,
				[LastCatalogPageLong] [varchar](2000) NULL,
				[LastCatalogText] [varchar](1000) NULL,
				[LastInstructionsNO] [varchar](1000) NULL,
				[LastInstructionsDate] [date] NULL,
				[WebAddress] [varchar](200) NULL,
				[SetAdminCD] [int] NULL,
				[SetItemCD] [varchar](30) NULL,
				[SetSKUCD] [varchar](30) NULL,
				[SetSU] [int] NULL,
				[ExhibitionSegmentCD] [varchar](5) NULL,
				[OrderLot] [int] NULL,
				[ExhibitionCommonCD] [varchar](30) NULL,
				[ApprovalDate] [date] NULL,
				[DeleteFlg] [tinyint] NULL,
				[TagName1] [varchar](20) NULL,
				[TagName2] [varchar](20) NULL,
				[TagName3] [varchar](20) NULL,
				[TagName4] [varchar](20) NULL,
				[TagName5] [varchar](20) NULL,
				[TagName6] [varchar](20) NULL,
				[TagName7] [varchar](20) NULL,
				[TagName8] [varchar](20) NULL,
				[TagName9] [varchar](20) NULL,
				[TagName10] [varchar](20) NULL,
				[RowNo][varchar](20) Null
				)
	
							
							Insert into [#tmpSKU1_N]
							select (case when AdminNo = 'New' then @AdminCounter+ RowNo else AdminNo end ) as LastNo,
							(case when JanCD = 'Auto' then @AdminCounterJ+RowNo else JanCD end ) as LastJanNo, *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU1  ) f

								declare @LastNO1 as int=0
								declare @LastJanNo1 as int=0

							Set	@LastNO1=(SElect MAX(LastNo) from #tmpSKU1_N where AdminNO='New');
							Set	@LastJanNo1 =(SElect MAX(LastJanNo) from #tmpSKU1_N where JanCD='Auto');
							
							
								Update M_SKUCounter
								Set AdminNO=@LastNO1
								where @LastNO1 <> 0

								
								Update M_JANCounter
								Set JanCount=@LastJanNo1
								where @LastJanNo1 <> 0


			
			
						Insert 
						Into    M_SKU
						Select	
								LastNO,
								ChangeDate ,
								SKUCD ,
								IsNull(VariousFLG,0),
								IsNull(SKUName,'---'),
								SKUName,
								KanaName,
								IsNull(SKUShortName,LEFT(SKUName,40)),
								EnglishName ,
								IsNull(ITEMCD,'---') ,
								SizeNO,
								ColorNO ,
								LastJanNo ,
								SetKBN,
								PresentKBN,
								SampleKBN,
								DiscountKBN,
								SizeName,
								ColorName ,
								SizeName,
								ColorName ,
								WebFlg,
								RealStoreFlg,
								MainVendorCD ,
								MainVendorCD ,
								BrandCD,
								MakerItem,
								TaniCD,
								SportsCD,
								SegmentCD,
								ZaikoKBN,
								Rack ,
								VirtualFlg,
								DirectFlg,
								ts.ReserveCD,
								ts.NoticesCD,
								ts.PostageCD,
								ts.ManufactCD,
								ts.ConfirmCD,
								ts.WebStockFlg,
								ts.StopFlg,
								ts.DiscontinueFlg,
								ts.SoldOutFlg,
								ts.InventoryAddFlg,
								ts.MakerAddFlg,
								ts.StoreAddFlg,
								ts.NoNetOrderFlg,
								ts.EDIOrderFlg,
								ts.AutoOrderFlg,
								ts.CatalogFlg,
								ts.ParcelFlg,
								ts.TaxRateFLG,
								ts.CostingKBN,
								IsNull(NormalCost,0) as NormalCost,
								ts.SaleExcludedFlg,
								IsNull(PriceOutTax,0)as PriceOutTax ,
								IsNull(PriceWithTax,0) as PriceWithTax,
								IsNull(OrderPriceWithTax ,0)as OrderPriceWithTax,
								IsNull(OrderPriceWithoutTax,0)as OrderPriceWithoutTax,
								IsNull(Rate,0) as Rate,
								SaleStartDate ,
								WebStartDate,
								OrderAttentionCD ,
								OrderAttentionNote,
								CommentInStore,
								CommentOutStore ,
								ts.LastYearTerm,
								ts.LastSeason,
								ts.LastCatalogNO,
								ts.LastCatalogPage,
								ts.LastCatalogNOLong,
								ts.LastCatalogPageLong, 
								ts.LastCatalogText,
								ts.LastInstructionsNO,
								ts.LastInstructionsDate,
								IsNull(ts.WebAddress,ts.ITEMCD),
								0,
								Null,	
								Null,		--ts.SetSKUCD,
								0,   --ts.SetSU,
								ts.ExhibitionSegmentCD,
								IsNull(ts.OrderLot,1),
								Null,	--ts.ExhibitionCommonCD,
								ts.ApprovalDate,
								IsNull(ts.DeleteFlg,0),
								0,			--ts.UsedFlg,
								1,		--ts.SKSUpdateFlg,
								Null,		--ts.SKSUpdateDateTime,
								@OperatorCD,
								getdate(),
								@OperatorCD,
								getdate() 
						from #tmpSKU1_N as ts
						where not Exists(
											Select ms.AdminNO
											from M_SKU as ms
											where ms.AdminNO=ts.LastNO
											and ms.ChangeDate=ts.ChangeDate
										)
					
						Update M_SKU
						Set		
								AdminNo=ts.LastNO,
								ChangeDate=ts.ChangeDate ,
								SKUCD=ts.SKUCD ,
								VariousFLG=IsNull(ts.VariousFLG,0),
								SKUName=IsNull(ts.SKUName,'---'),
								SKUNameLong=ts.SKUName,
								KanaName=ts.KanaName,
								SKUShortName=IsNull(ts.SKUShortName,LEFT(ts.SKUName,40)),
								EnglishName=ts.EnglishName ,
								ITemCD=IsNull(ts.ITEMCD,'---') ,
								SizeNO=ts.SizeNO,
								ColorNO=ts.ColorNO ,
								JanCD=ts.LastJanNo ,
								SizeName=ts.SizeName,
								ColorName=ts.ColorName ,
								SizeNameLong=ts.SizeName,
								ColorNameLong=ts.ColorName ,
								SetKBN=ts.SetKBN,
								PresentKBN=ts.PresentKBN,
								DiscountKBN=ts.DiscountKBN,
								WebFlg=ts.WebFlg,
								RealStoreFlg=ts.RealStoreFlg,
								MainVendorCD=ts.MainVendorCD ,
								MakerVendorCD=ts.MainVendorCD ,
								BrandCD=ts.BrandCD,
								MakerItem=ts.MakerItem,
								TaniCD=ts.TaniCD,
								SportsCD=ts.SportsCD,
								SegmentCD=ts.SegmentCD,
								ZaikoKBN=ts.ZaikoKBN,
								Rack=ts.Rack ,
								VirtualFlg=ts.VirtualFlg,
								DirectFlg=ts.DirectFlg,
								ReserveCD=ts.ReserveCD,
								NoticesCD=ts.NoticesCD,
								PostageCD=ts.PostageCD,
								ManufactCD=ts.ManufactCD,
								ConfirmCD=ts.ConfirmCD,
								WebStockFlg=ts.WebStockFlg,
								StopFlg=ts.StopFlg,
								DiscontinueFlg=ts.DiscontinueFlg,
								SoldOutFlg=ts.SoldOutFlg,
								InventoryAddFlg=ts.InventoryAddFlg,
								MakerAddFlg=ts.MakerAddFlg,
								StoreAddFlg=ts.StoreAddFlg,
								NoNetOrderFlg=ts.NoNetOrderFlg,
								EDIOrderFlg=ts.EDIOrderFlg,
								AutoOrderFlg=ts.AutoOrderFlg,
								CatalogFlg=ts.CatalogFlg,
								ParcelFlg=ts.ParcelFlg,
								TaxRateFLG=ts.TaxRateFLG,
								CostingKBN=ts.CostingKBN,
								SaleExcludedFlg=ts.SaleExcludedFlg,
								NormalCost=IsNull(ts.NormalCost,0),
								PriceOutTax=IsNull(ts.PriceOutTax,0) ,
								PriceWithTax=IsNull(ts.PriceWithTax,0),
								OrderPriceWithTax=IsNull(ts.OrderPriceWithTax ,0),
								OrderPriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
								Rate=IsNull(ts.Rate,0),
								SaleStartDate=ts.SaleStartDate ,
								WebStartDate=ts.WebStartDate,
								OrderAttentionCD=ts.OrderAttentionCD ,
								OrderAttentionNote=ts.OrderAttentionNote,
								CommentInStore=ts.CommentInStore,
								CommentOutStore=ts.CommentOutStore ,
								LastYearTerm=ts.LastYearTerm,
								LastSeason=ts.LastSeason,
								LastCatalogNO=ts.LastCatalogNO,
								LastCatalogPage=ts.LastCatalogPage,
								LastCatalogNOLong=ts.LastCatalogNOLong,
								LastCatalogPageLong=ts.LastCatalogPageLong, 
								LastCatalogText=ts.LastCatalogText,
								LastInstructionsNO=ts.LastInstructionsNO,
								LastInstructionsDate=ts.LastInstructionsDate,
								WebAddress=	IsNull(ts.WebAddress,ts.ITEMCD),
								SetAdminCD=0, 
								SetItemCD=Null,	
								SetSKUCD=Null,		--ts.SetSKUCD,
								SetSU=0,   --ts.SetSU,
								ExhibitionSegmentCD=ts.ExhibitionSegmentCD,
								OrderLot=IsNull(ts.OrderLot,1),
								ExhibitionCommonCD=Null,	--ts.ExhibitionCommonCD,
								ApprovalDate=ts.ApprovalDate,
								DeleteFlg=IsNull(ts.DeleteFlg,0),
								UsedFlg=0,			--ts.UsedFlg,
								SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
								SKSUpdateDateTime=Null,		--ts.SKSUpdateDateTime,
								InsertOperator=@OperatorCD,
								InsertDateTime=getdate(),
								UpdateOperator=@OperatorCD,
								UpdateDateTime=getdate() 
								from M_SKU as ms
								inner join #tmpSKU1_N  as ts on ts.LastNO =ms.AdminNO
								where ms.ChangeDate=ts.ChangeDate

						Delete mtag
								from M_SKUTag as mtag
								inner join #tmpSKU1_N as ts on ts.LastNO=mtag.AdminNO
								Where mtag.AdminNO=ts.LastNO
								and mtag.ChangeDate=ts.ChangeDate

						Insert 
							Into	M_SKUTag
							SELECT LastNO,ChangeDate,ROW_NUMBER() OVER (ORDER BY LastNo),ColumnValue from #tmpSKU1_N as ts
								 Unpivot(ColumnValue For ColumnName IN (TagName1,TagName2,TagName3,TagName4,TagName5,TagName6,TagName7,TagName8,TagName9,TagName10)) AS H

						Delete mj
									from M_JANOrderPrice as mj
									Where mj.AdminNO IN 
									(
									Select ms.AdminNO 
									from M_SKU as ms
									Inner join #tmpSKU1_N as ts on ts.LastNO= ms.AdminNO
									where ms.ItemCD=ts.ITEMCD
									)
							
						Insert 
							Into	 M_JANOrderPrice
							Select
								    MainVendorCD,
									'0000',
									LastNO,
									ChangeDate,
									SKUCD,
									IsNull(Rate,0),
									IsNull(OrderPriceWithoutTax,0),
									Null,
									IsNull(DeleteFlg,0),
									0,
									@OperatorCD,
									getdate(),
									@OperatorCD,
									getdate()
									from #tmpSKU1_N 
									where MainVendorCD != Null

						Insert 
						Into M_JANOrderPrice
						Select
						'0000000000000',
						'0000',
						ts.LastNO,
						ts.ChangeDate,
						ts.SKUCD,
						IsNull(ts.Rate,0),
						IsNull(ts.OrderPriceWithoutTax,0),
						Null,
						IsNull(ts.DeleteFlg,0),
						0,
						@OperatorCD,
						getdate(),
						@OperatorCD,
						getdate()
						from #tmpSKU1_N as ts
						where not Exists(

							Select AdminNO
							from M_JANOrderPrice as mj
							where mj.AdminNO= ts.LastNO
							and mj.ChangeDate=ts.ChangeDate
							and mj.VendorCD='0000000000000'
						)

						Update M_JANOrderPrice
					SET
					
					VendorCD= '0000000000000',
					StoreCD=	'0000',
					AdminNO=ts.LastNO,
					ChangeDate=ts.ChangeDate,
					SKUCD=ts.SKUCD,
					Rate=IsNull(ts.Rate,0),
					PriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
					Remarks=NULL,
					DeleteFlg=IsNull(ts.DeleteFlg,0),
					UsedFlg=0,
					InsertOperator=@OperatorCD,
					InsertDateTime=getdate(),
					UpdateOperator=@OperatorCD,
					UpdateDateTime=getdate()
					from M_JANOrderPrice as mj
					inner join #tmpSKU1_N as ts on mj.AdminNO=ts.LastNO
					where mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'

						Insert into M_SKUInfo
					Select 
						ts.LastNO,
						ts.ChangeDate,
						1,
						ts.LastYearTerm,
						ts.LastSeason,
						ts.LastCatalogNO,
						ts.LastCatalogPage,
						ts.LastCatalogText,
						ts.LastInstructionsNO,
						ts.LastInstructionsDate,
						ts.DeleteFlg,
						@OperatorCD,
						getdate(),
						@OperatorCD,
						getdate()

						From #tmpSKU1_N as ts
						Where not Exists(
						                  Select AdminNO
										  from M_SKUInfo as mi
										  where mi.AdminNO=ts.LastNO
										  and mi.ChangeDate=ts.ChangeDate
										  and mi.SEQ=1
											)
				
						Update M_SKUInfo
						Set
						AdminNO =ts.LastNO,
						ChangeDate=ts.ChangeDate,
						SEQ=1,
						YearTerm=ts.LastYearTerm,
						Season=ts.LastSeason,
						CatalogNO=ts.LastCatalogNO,
						CatalogPage=ts.LastCatalogPage,
						CatalogText=ts.LastCatalogText,
						InstructionsNO=ts.LastInstructionsNO,
						InstructionsDate=ts.LastInstructionsDate,
						DeleteFlg=ts.DeleteFlg,
						InsertOperator=@OperatorCD,
						InsertDateTime=getdate(),
						UpdateOperator=@OperatorCD,
						UpdateDateTime=getdate()
						From M_SKUInfo as mI
						inner join #tmpSKU1_N as ts on ts.LastNO=mI.AdminNO
						where mI.AdminNO= ts.LastNO
						and mI.ChangeDate=ts.ChangeDate
						and mI.SEQ= 1
			
				drop table #tmpSKU1
				drop table #tmpSKU1_N
				

		End

	if @type =2
	  Begin
	   Create table [#tmpSKU2](
	
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[SKUCD] [varchar](40) NULL,
	[VariousFLG] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[KanaName] [varchar](50) NULL,
	[SKUShortName] [varchar](40) NULL,
	[EnglishName] [varchar](80) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[JanCD] [varchar](13) NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[MainVendorCD] [varchar](13) NULL,
	[VendorName] [varchar](50) NULL,
	[BrandCD] [varchar](6) NULL,
	[BrandName] [varchar](40) NULL,
	[MakerItem] [varchar](50) NULL,
	[TaniCD] [varchar](2) NULL,
	[TaniName] [varchar](100) NULL,
	[SportsCD] [varchar](6) NULL,
	[SportsName] [varchar](100) NULL,
	[SegmentCD] [varchar](6) NULL,
	[SegmentName] [varchar](100) NULL,
	[ZaikoKBN] [tinyint] NULL,
	[Rack] [varchar](10) NULL,
	[NormalCost] [money] NOT NULL,
	[PriceWithTax] [money] NULL,
	[PriceOutTax] [money] NULL,
	[OrderPriceWithTax] [money] NULL,
	[OrderPriceWithoutTax] [money] NULL,
	[Rate] [decimal](5, 2) NULL,
	[SaleStartDate] [date] NULL,
	[WebStartDate] [date] NULL,
	[OrderAttentionCD] [varchar](3) NULL,
	[OrderAttentionName] [varchar](100) NULL,
	[OrderAttentionNote] [varchar](100) NULL,
	[CommentInStore] [varchar](300) NULL,
	[CommentOutStore] [varchar](300) NULL,
	[SetSU] [int] NULL,
	[ExhibitionSegmentCD] [varchar](5) NULL,
	[ExhibitionSegmentName] [varchar](100) NULL,
	[OrderLot] [int] NULL,
	[ApprovalDate] [date] NULL,
	[DeleteFlg] [tinyint] NULL
	)

	 Create table [#tmpSKU2_N](
	[LastNO][int] NOT NUll,
	[LastJanNo][varchar](13) NULL,
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[SKUCD] [varchar](40) NULL,
	[VariousFLG] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[KanaName] [varchar](50) NULL,
	[SKUShortName] [varchar](40) NULL,
	[EnglishName] [varchar](80) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[JanCD] [varchar](13) NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[MainVendorCD] [varchar](13) NULL,
	[VendorName] [varchar](50) NULL,
	[BrandCD] [varchar](6) NULL,
	[BrandName] [varchar](40) NULL,
	[MakerItem] [varchar](50) NULL,
	[TaniCD] [varchar](2) NULL,
	[TaniName] [varchar](100) NULL,
	[SportsCD] [varchar](6) NULL,
	[SportsName] [varchar](100) NULL,
	[SegmentCD] [varchar](6) NULL,
	[SegmentName] [varchar](100) NULL,
	[ZaikoKBN] [tinyint] NULL,
	[Rack] [varchar](10) NULL,
	[NormalCost] [money] NOT NULL,
	[PriceWithTax] [money] NULL,
	[PriceOutTax] [money] NULL,
	[OrderPriceWithTax] [money] NULL,
	[OrderPriceWithoutTax] [money] NULL,
	[Rate] [decimal](5, 2) NULL,
	[SaleStartDate] [date] NULL,
	[WebStartDate] [date] NULL,
	[OrderAttentionCD] [varchar](3) NULL,
	[OrderAttentionName] [varchar](100) NULL,
	[OrderAttentionNote] [varchar](100) NULL,
	[CommentInStore] [varchar](300) NULL,
	[CommentOutStore] [varchar](300) NULL,
	[SetSU] [int] NULL,
	[ExhibitionSegmentCD] [varchar](5) NULL,
	[ExhibitionSegmentName] [varchar](100) NULL,
	[OrderLot] [int] NULL,
	[ApprovalDate] [date] NULL,
	[DeleteFlg] [tinyint] NULL,
	[RowNo][int] Null,
	)
	declare @DocHandle2 int

	exec sp_xml_preparedocument @DocHandle2 output, @xml
	insert into #tmpSKU2
	select *  FROM OPENXML (@DocHandle2, '/NewDataSet/test',2)
			with
			(
	AdminNO varchar(50),
	改定日 date,
	SKUCD varchar(40) ,
	諸口区分 tinyint,
	商品名 varchar(100),
	カナ名 varchar(50),
	略名 varchar(40),
	英語名 varchar(80),
	ITEMCD varchar(30),
	サイズ枝番 int,
	カラー枝番 int,
	JANCD varchar(13),
	サイズ名 varchar(20) ,
	カラー名 varchar(20) ,
	主要仕入先CD varchar(13) ,
	主要仕入先名 varchar(50),
	ブランドCD varchar(6) ,
	ブランド名 varchar(40),
	メーカー商品CD varchar(50) ,
	単位CD varchar(2) ,
	単位名 varchar(100),
	競技CD varchar(6) ,
	競技名 varchar(100),
	商品分類CD varchar(6) ,
	分類名 varchar(100),
	ZaikoKBN tinyint ,
	棚番 varchar(10) ,
	標準原価 money  ,
	税抜定価 money ,
	税込定価 money ,
	発注税込価格 money ,
	発注税抜価格 money ,
	掛率 decimal(5, 2) ,
	発売開始日 date ,
	Web掲載開始日 date,
	発注注意区分 varchar(3) ,
	発注注意区分名 varchar(100),
	発注注意事項 varchar(100) ,
	管理用備考 varchar(300) ,
	表示用備考 varchar(300) ,
	構成数 int,
	セグメントCD varchar(5),
	セグメント名 varchar(100),
	発注ロット int ,
	承認日 date ,
	削除 tinyint  
	)
	exec sp_xml_removedocument @DocHandle2;

							Insert into [#tmpSKU2_N]
							select (case when AdminNo = 'New' then @AdminCounter+ RowNo else AdminNo end ) as LastNo,
							(case when JanCD = 'Auto' then @AdminCounterJ+RowNo else JanCD end ) as LastJanNo, *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU2  ) f

							declare @LastNO2 as int=0;
							declare @LastJanNo2 as int=0;
							Set @LastNO2=(SElect MAX(LastNo) from #tmpSKU2_N where AdminNO='New');
							Set	@LastJanNo2 =(SElect MAX(LastJanNo) from #tmpSKU2_N where JanCD='Auto');
							
							
								Update M_SKUCounter
								Set AdminNO=@LastNO2
								Where @LastNO2 <> 0
								
								Update M_JANCounter
								Set JanCount=@LastJanNo2
								Where @LastJanNo2 <> 0

			
	
		Insert 
						Into    M_SKU
						Select	
								LastNO,
								ChangeDate ,
								SKUCD ,
								IsNull(VariousFLG,0),
								IsNull(SKUName,'---'),
								SKUName,
								KanaName,
								IsNull(SKUShortName,LEFT(SKUName,40)),
								EnglishName ,
								IsNull(ITEMCD,'---') ,
								SizeNO,
								ColorNO ,
								LastJanNo ,
								(SElect SetKBN from M_SKUInitial where MainKey=1),
								(SElect PresentKBN from M_SKUInitial where MainKey=1),
								(SElect SampleKBN from M_SKUInitial where MainKey=1),
								(SElect DiscountKBN from M_SKUInitial where MainKey=1),
								SizeName,
								ColorName ,
								SizeName,
								ColorName ,
								(SElect WebFlg from M_SKUInitial where MainKey=1),
								(SElect RealStoreFlg from M_SKUInitial where MainKey=1),
								MainVendorCD ,
								MainVendorCD ,
								BrandCD,
								MakerItem,
								TaniCD,
								SportsCD,
								SegmentCD,
								(SElect VirtualFlg from M_SKUInitial where MainKey=1)ZaikoKBN,
								Rack ,
								(SElect VirtualFlg from M_SKUInitial where MainKey=1),
								(SElect DirectFlg from M_SKUInitial where MainKey=1),
								(SElect ReserveCD from M_SKUInitial where MainKey=1),
								(SElect NoticesCD from M_SKUInitial where MainKey=1),
								(SElect PostageCD from M_SKUInitial where MainKey=1),
								(SElect ManufactCD from M_SKUInitial where MainKey=1),
								(SElect ConfirmCD from M_SKUInitial where MainKey=1),
								(SElect WebStockFlg from M_SKUInitial where MainKey=1),
								(SElect StopFlg from M_SKUInitial where MainKey=1),
								(SElect DiscontinueFlg from M_SKUInitial where MainKey=1),
								(SElect SoldOutFlg from M_SKUInitial where MainKey=1),
								(SElect InventoryAddFlg from M_SKUInitial where MainKey=1),
								(SElect MakerAddFlg from M_SKUInitial where MainKey=1),
								(SElect StoreAddFlg from M_SKUInitial where MainKey=1),
								(SElect NoNetOrderFlg from M_SKUInitial where MainKey=1),
								(SElect EDIOrderFlg from M_SKUInitial where MainKey=1),
								(SElect AutoOrderFlg from M_SKUInitial where MainKey=1),
								(SElect CatalogFlg from M_SKUInitial where MainKey=1),
								(SElect ParcelFlg from M_SKUInitial where MainKey=1),
								(SElect TaxRateFLG from M_SKUInitial where MainKey=1),
								(SElect CostingKBN from M_SKUInitial where MainKey=1),
								IsNull(NormalCost,0)as NormalCost,
								(SElect SaleExcludedFlg from M_SKUInitial where MainKey=1),
								IsNull(PriceOutTax,0)as PriceOutTax ,
								IsNull(PriceWithTax,0) as PriceWithTax,
								IsNull(OrderPriceWithTax ,0)as OrderPriceWithTax,
								IsNull(OrderPriceWithoutTax,0)as OrderPriceWithoutTax,
								IsNull(Rate,0) as Rate,
								SaleStartDate ,
								WebStartDate,
								OrderAttentionCD ,
								OrderAttentionNote,
								CommentInStore,
								CommentOutStore ,
								Null,
								Null,
								Null,
								Null,
								Null,
								Null, 
								Null,
								Null,
								Null,
								ts.ITEMCD,--WebAddress
								0,
								Null,	
								Null,		--ts.SetSKUCD,
								0,   --ts.SetSU,
								ts.ExhibitionSegmentCD,
								1,--OrderLost
								Null,	--ts.ExhibitionCommonCD,
								ts.ApprovalDate,
								IsNull(ts.DeleteFlg,0),
								0,			--ts.UsedFlg,
								1,		--ts.SKSUpdateFlg,
								Null,		--ts.SKSUpdateDateTime,
								@OperatorCD,
								getdate(),
								@OperatorCD,
								getdate() 
						from #tmpSKU2_N as ts
						where not Exists(
											Select ms.AdminNO
											from M_SKU as ms
											where ms.AdminNO=ts.LastNO
											and ms.ChangeDate=ts.ChangeDate
										)
					
	  	Update M_SKU
						Set		
								AdminNo=ts.LastNO,
								ChangeDate=ts.ChangeDate ,
								SKUCD=ts.SKUCD ,
								VariousFLG=IsNull(ts.VariousFLG,0),
								SKUName=IsNull(ts.SKUName,'---'),
								SKUNameLong=ts.SKUName,
								KanaName=ts.KanaName,
								SKUShortName=IsNull(ts.SKUShortName,LEFT(ts.SKUName,40)),
								EnglishName=ts.EnglishName ,
								ITemCD=IsNull(ts.ITEMCD,'---') ,
								SizeNO=ts.SizeNO,
								ColorNO=ts.ColorNO ,
								JanCD=ts.LastJanNo ,
								SizeName=ts.SizeName,
								ColorName=ts.ColorName ,
								SizeNameLong=ts.SizeName,
								ColorNameLong=ts.ColorName ,
								SetKBN=(SElect SetKBN from M_SKUInitial where MainKey=1),
								PresentKBN=(SElect PresentKBN from M_SKUInitial where MainKey=1),
								DiscountKBN=(SElect DiscountKBN from M_SKUInitial where MainKey=1),
								WebFlg=(SElect WebFlg from M_SKUInitial where MainKey=1),
								RealStoreFlg=(SElect RealStoreFlg from M_SKUInitial where MainKey=1),
								MainVendorCD=ts.MainVendorCD ,
								MakerVendorCD=ts.MainVendorCD ,
								BrandCD=ts.BrandCD,
								MakerItem=ts.MakerItem,
								TaniCD=ts.TaniCD,
								SportsCD=ts.SportsCD,
								SegmentCD=ts.SegmentCD,
								ZaikoKBN=ts.ZaikoKBN,
								Rack=ts.Rack ,
								VirtualFlg=(SElect VirtualFlg from M_SKUInitial where MainKey=1),
								DirectFlg=(SElect DirectFlg from M_SKUInitial where MainKey=1),
								ReserveCD=(SElect ReserveCD from M_SKUInitial where MainKey=1),
								NoticesCD=(SElect NoticesCD from M_SKUInitial where MainKey=1),
								PostageCD=(SElect PostageCD from M_SKUInitial where MainKey=1),
								ManufactCD=(SElect ManufactCD from M_SKUInitial where MainKey=1),
								ConfirmCD=(SElect ConfirmCD from M_SKUInitial where MainKey=1),
								WebStockFlg=(SElect WebStockFlg from M_SKUInitial where MainKey=1),
								StopFlg=(SElect StopFlg from M_SKUInitial where MainKey=1),
								DiscontinueFlg=(SElect DiscontinueFlg from M_SKUInitial where MainKey=1),
								SoldOutFlg=(SElect SoldOutFlg from M_SKUInitial where MainKey=1),
								InventoryAddFlg=(SElect InventoryAddFlg from M_SKUInitial where MainKey=1),
								MakerAddFlg=(SElect MakerAddFlg from M_SKUInitial where MainKey=1),
								StoreAddFlg=(SElect StoreAddFlg from M_SKUInitial where MainKey=1),
								NoNetOrderFlg=(SElect NoNetOrderFlg from M_SKUInitial where MainKey=1),
								EDIOrderFlg=(SElect EDIOrderFlg from M_SKUInitial where MainKey=1),
								AutoOrderFlg=(SElect AutoOrderFlg from M_SKUInitial where MainKey=1),
								CatalogFlg=(SElect CatalogFlg from M_SKUInitial where MainKey=1),
								ParcelFlg=(SElect ParcelFlg from M_SKUInitial where MainKey=1),
								TaxRateFLG=(SElect TaxRateFLG from M_SKUInitial where MainKey=1),
								CostingKBN=(SElect CostingKBN from M_SKUInitial where MainKey=1),
								SaleExcludedFlg=(SElect SaleExcludedFlg from M_SKUInitial where MainKey=1),
								NormalCost=IsNull(ts.NormalCost,0),
								PriceOutTax=IsNull(ts.PriceOutTax,0) ,
								PriceWithTax=IsNull(ts.PriceWithTax,0),
								OrderPriceWithTax=IsNull(ts.OrderPriceWithTax ,0),
								OrderPriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
								Rate=IsNull(ts.Rate,0),
								SaleStartDate=ts.SaleStartDate ,
								WebStartDate=ts.WebStartDate,
								OrderAttentionCD=ts.OrderAttentionCD ,
								OrderAttentionNote=ts.OrderAttentionNote,
								CommentInStore=ts.CommentInStore,
								CommentOutStore=ts.CommentOutStore ,
								WebAddress=ts.ITEMCD,
								SetAdminCD=0, 
								SetItemCD=Null,	
								SetSKUCD=Null,		--ts.SetSKUCD,
								SetSU=0,   --ts.SetSU,
								ExhibitionSegmentCD=ts.ExhibitionSegmentCD,
								OrderLot=1,
								ExhibitionCommonCD=Null,	--ts.ExhibitionCommonCD,
								ApprovalDate=ts.ApprovalDate,
								DeleteFlg=IsNull(ts.DeleteFlg,0),
								UsedFlg=0,			--ts.UsedFlg,
								SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
								SKSUpdateDateTime=Null,		--ts.SKSUpdateDateTime,
								InsertOperator=@OperatorCD,
								InsertDateTime=getdate(),
								UpdateOperator=@OperatorCD,
								UpdateDateTime=getdate() 
								from M_SKU as ms
								inner join #tmpSKU2_N  as ts on ts.LastNO =ms.AdminNO
								where ms.ChangeDate=ts.ChangeDate

		Delete mj
			from M_JANOrderPrice as mj
			
			Where mj.AdminNO In
			(
			Select ms.AdminNO 
			from M_SKU as ms
			Inner join #tmpSKU2_N as ts on ts.LastNO= ms.AdminNO
			where ms.ItemCD=ts.ITEMCD
			)

		Insert 
			Into M_JANOrderPrice
			Select
			MainVendorCD,
			'0000',
			LastNO,
			ChangeDate,
			SKUCD,
			IsNull(Rate,0),
			IsNull(OrderPriceWithoutTax,0),
			Null,
			IsNull(DeleteFlg,0),
			0,
			@OperatorCD,
			getdate(),
			@OperatorCD,
			getdate()
			from #tmpSKU2_N
			where MainVendorCD != Null

		Insert 
				Into M_JANOrderPrice
				Select
				'0000000000000',
				'0000',
				ts.LastNO,
				ts.ChangeDate,
				ts.SKUCD,
				IsNull(ts.Rate,0),
				IsNull(ts.OrderPriceWithoutTax,0),
				Null,
				IsNull(ts.DeleteFlg,0),
				0,
				@OperatorCD,
				getdate(),
				@OperatorCD,
				getdate()
				from #tmpSKU2_N as ts
				where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.LastNO
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)

		Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.LastNO,
			ChangeDate=ts.ChangeDate,
			SKUCD=ts.SKUCD,
			Rate=IsNull(ts.Rate,0),
			PriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
			Remarks=NULL,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_JANOrderPrice as mj
			inner join #tmpSKU2_N as ts on mj.AdminNO=ts.LastNO
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'


	drop table #tmpSKU2
	drop table #tmpSKU2_N

	  End 

	if @type =3
		Begin
		 Create table [#tmpSKU3](
			[AdminNO] [varchar](50) NOT NULL,
			[ChangeDate] [date] NOT NULL,
			[ApprovalDate] [date] NULL,
			[SKUCD] [varchar](40) NULL,
			[JanCD] [varchar](13) NULL,
			[DeleteFlg] [tinyint] NULL,
			[SKUName] [varchar](100) NULL,
			[ITEMCD] [varchar](30) NULL,
			[SizeNO] [int] NULL,
			[ColorNO] [int] NULL,
			[SizeName] [varchar](20) NULL,
			[ColorName] [varchar](20) NULL,
			[MainVendorCD] [varchar](13) NULL,
			[VendorName] [varchar](50) NULL,
			[SetKBN] [tinyint] NULL,
			[PresentKBN] [tinyint] NULL,
			[SampleKBN] [tinyint] NULL,
			[DiscountKBN] [tinyint] NULL,
			[WebFlg] [tinyint] NULL,
			[RealStoreFlg] [tinyint] NULL,
			[ZaikoKBN] [tinyint] NULL,
			[VirtualFlg] [tinyint] NULL,
			[DirectFlg] [tinyint] NULL,
			[ReserveCD] [varchar](3) NULL,
			[NoticesCD] [varchar](3) NULL,
			[PostageCD] [varchar](3) NULL,
			[ManufactCD] [varchar](3) NULL,
			[ConfirmCD] [varchar](3) NULL,
			[WebStockFlg] [tinyint] NULL,
			[StopFlg] [tinyint] NULL,
			[DiscontinueFlg] [tinyint] NULL,
			[SoldOutFlg] [tinyint] NULL,
			[InventoryAddFlg] [tinyint] NULL,
			[MakerAddFlg] [tinyint] NULL,
			[StoreAddFlg] [tinyint] NULL,
			[NoNetOrderFlg] [tinyint] NULL,
			[EDIOrderFlg] [tinyint] NULL,
			[AutoOrderFlg] [tinyint] NULL,
			[CatalogFlg] [tinyint] NULL,
			[ParcelFlg] [tinyint] NULL,
			[SaleExcludedFlg] [tinyint] NULL,
			[NormalCost] [money] NOT NULL,
			[PriceWithTax] [money] NULL,
			[PriceOutTax] [money] NULL,
			[OrderPriceWithTax] [money] NULL,
			[OrderPriceWithoutTax] [money] NULL,
			[Rate] [decimal](5, 2) NULL,
			)

			 Create table [#tmpSKU3_N](
			[LastNo][int] NOT NULL,
			[LastJanNo][varchar](13) NULL,
			[AdminNO] [varchar](50) NOT NULL,
			[ChangeDate] [date] NOT NULL,
			[ApprovalDate] [date] NULL,
			[SKUCD] [varchar](40) NULL,
			[JanCD] [varchar](13) NULL,
			[DeleteFlg] [tinyint] NULL,
			[SKUName] [varchar](100) NULL,
			[ITEMCD] [varchar](30) NULL,
			[SizeNO] [int] NULL,
			[ColorNO] [int] NULL,
			[SizeName] [varchar](20) NULL,
			[ColorName] [varchar](20) NULL,
			[MainVendorCD] [varchar](13) NULL,
			[VendorName] [varchar](50) NULL,
			[SetKBN] [tinyint] NULL,
			[PresentKBN] [tinyint] NULL,
			[SampleKBN] [tinyint] NULL,
			[DiscountKBN] [tinyint] NULL,
			[WebFlg] [tinyint] NULL,
			[RealStoreFlg] [tinyint] NULL,
			[ZaikoKBN] [tinyint] NULL,
			[VirtualFlg] [tinyint] NULL,
			[DirectFlg] [tinyint] NULL,
			[ReserveCD] [varchar](3) NULL,
			[NoticesCD] [varchar](3) NULL,
			[PostageCD] [varchar](3) NULL,
			[ManufactCD] [varchar](3) NULL,
			[ConfirmCD] [varchar](3) NULL,
			[WebStockFlg] [tinyint] NULL,
			[StopFlg] [tinyint] NULL,
			[DiscontinueFlg] [tinyint] NULL,
			[SoldOutFlg] [tinyint] NULL,
			[InventoryAddFlg] [tinyint] NULL,
			[MakerAddFlg] [tinyint] NULL,
			[StoreAddFlg] [tinyint] NULL,
			[NoNetOrderFlg] [tinyint] NULL,
			[EDIOrderFlg] [tinyint] NULL,
			[AutoOrderFlg] [tinyint] NULL,
			[CatalogFlg] [tinyint] NULL,
			[ParcelFlg] [tinyint] NULL,
			[SaleExcludedFlg] [tinyint] NULL,
			[NormalCost] [money] NOT NULL,
			[PriceWithTax] [money] NULL,
			[PriceOutTax] [money] NULL,
			[OrderPriceWithTax] [money] NULL,
			[OrderPriceWithoutTax] [money] NULL,
			[Rate] [decimal](5, 2) NULL,
			[RowNo][int] Null
			)

			declare @DocHandle3 int

			exec sp_xml_preparedocument @DocHandle3 output, @xml
			INSERT Into #tmpSKU3
			select *  FROM OPENXML (@DocHandle3, '/NewDataSet/test',2)
			with
			(
			AdminNO varchar(50),
			改定日 date,
			承認日 date ,
			SKUCD varchar(40) ,
			JANCD varchar(13),
			削除 tinyint ,
			商品名 varchar(100),
			ITEMCD varchar(30),
			サイズ枝番 int,
			カラー枝番 int,
			サイズ名 varchar(20) ,
			カラー名 varchar(20) ,
			主要仕入先CD varchar(13),
			主要仕入先名 varchar(50),
			セット品区分 tinyint,
			プレゼント品区分 tinyint ,
			サンプル品区分 tinyint ,
			値引商品区分 tinyint,
			Webストア取扱区分 tinyint ,
			実店舗取扱区分 tinyint ,
			在庫管理対象区分 tinyint ,
			架空商品区分 tinyint ,
			直送品区分 tinyint ,
			予約品区分 varchar(3) ,
			特記区分 varchar(3) ,
			送料区分 varchar(3) ,
			要加工品区分 varchar(3) ,
			要確認品区分 varchar(3) ,
			Web在庫連携区分 tinyint ,
			販売停止品区分 tinyint ,
			廃番品区分 tinyint ,
			完売品区分 tinyint ,
			自社在庫連携対象 tinyint ,
			メーカー在庫連携対象 tinyint ,
			店舗在庫連携対象 tinyint ,
			Net発注不可区分 tinyint ,
			EDI発注可能区分 tinyint ,
			自動発注対象区分 tinyint ,
			カタログ掲載有無区分 tinyint ,
			小包梱包可能区分 tinyint ,
			Sale対象外区分 tinyint ,
			標準原価 money  ,
			税抜定価 money ,
			税込定価 money ,
			発注税込価格 money ,
			発注税抜価格 money ,
			掛率 decimal(5, 2) 
			)
			exec sp_xml_removedocument @DocHandle3;

					Insert into [#tmpSKU3_N]
							select (case when AdminNo = 'New' then @AdminCounter+ RowNo else AdminNo end ) as LastNo,
							(case when JanCD = 'Auto' then @AdminCounterJ+RowNo else JanCD end ) as LastJanNo, *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU3  ) f
									declare @LastNO3 as int =0;
									declare @LastJanNo3 as int =0;
									SEt @LastNO3=(SElect MAX(LastNo) from #tmpSKU3_N where AdminNO='New');
								SEt @LastJanNo3=(SElect MAX(LastJanNo) from #tmpSKU3_N where JanCD='Auto');
							
							
								Update M_SKUCounter
								Set AdminNO=@LastNO3
								Where @LastNO3 <>0

								Update M_JANCounter
								Set JanCount=@LastJanNo3
								Where @LastJanNo3 <>0

			
			Update M_SKU
			Set		
					AdminNo=ts.LastNo,
					ChangeDate=ts.ChangeDate ,
					SKUCD=ts.SKUCD ,
					SKUName=ts.SKUName,
					SKUNameLong=ts.SKUName,
					ITemCD=ts.ITEMCD ,
					SizeNO=ts.SizeNO,
					ColorNO=ts.ColorNO ,
					JanCD=ts.LastJanNo ,
					SizeName=ts.SizeName,
					ColorName=ts.ColorName ,
					SizeNameLong=ts.SizeName,
					ColorNameLong=ts.ColorName ,
					SetKBN=ts.SetKBN,
					PresentKBN=ts.PresentKBN,
					DiscountKBN=ts.DiscountKBN,
					WebFlg=ts.WebFlg,
					RealStoreFlg=ts.RealStoreFlg,
					ZaikoKBN=ts.ZaikoKBN,
					VirtualFlg=ts.VirtualFlg,
					DirectFlg=ts.DirectFlg,
					ReserveCD=ts.ReserveCD,
					NoticesCD=ts.NoticesCD,
					PostageCD=ts.PostageCD,
					ManufactCD=ts.ManufactCD,
					ConfirmCD=ts.ConfirmCD,
					WebStockFlg=ts.WebStockFlg,
					StopFlg=ts.StopFlg,
					DiscontinueFlg=ts.DiscontinueFlg,
					SoldOutFlg=ts.SoldOutFlg,
					InventoryAddFlg=ts.InventoryAddFlg,
					MakerAddFlg=ts.MakerAddFlg,
					StoreAddFlg=ts.StoreAddFlg,
					NoNetOrderFlg=ts.NoNetOrderFlg,
					EDIOrderFlg=ts.EDIOrderFlg,
					AutoOrderFlg=ts.AutoOrderFlg,
					CatalogFlg=ts.CatalogFlg,
					ParcelFlg=ts.ParcelFlg,
					SaleExcludedFlg=ts.SaleExcludedFlg,
					NormalCost=IsNull(ts.NormalCost,0),
					PriceOutTax=IsNull(ts.PriceOutTax,0) ,
					PriceWithTax=IsNull(ts.PriceWithTax,0),
					OrderPriceWithTax=IsNull(ts.OrderPriceWithTax ,0),
					OrderPriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
					Rate=IsNull(ts.Rate,0),
					SetAdminCD=0, 
					SetItemCD=Null,	
					SetSKUCD=Null,		--ts.SetSKUCD,
					SetSU=0,   --ts.SetSU,
					ExhibitionCommonCD=Null,	--ts.ExhibitionCommonCD,
					ApprovalDate=ts.ApprovalDate,
					DeleteFlg=IsNull(ts.DeleteFlg,0),
					UsedFlg=0,			--ts.UsedFlg,
					SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
					SKSUpdateDateTime=Null,		--ts.SKSUpdateDateTime,
					InsertOperator=@OperatorCD,
					InsertDateTime=getdate(),
					UpdateOperator=@OperatorCD,
					UpdateDateTime=getdate() 
					from M_SKU as ms
					inner join #tmpSKU3_N  as ts on ts.LastNo =ms.AdminNO
					where ms.ChangeDate=ts.ChangeDate
			
			Delete mj
							from M_JANOrderPrice as mj
							
							Where mj.AdminNO In 
							(
							Select ms.AdminNO 
							from M_SKU as ms
							Inner join #tmpSKU3_N as ts on ts.LastNo= ms.AdminNO
							where ms.ItemCD=ts.ITEMCD
							)
			Insert 
					Into	 M_JANOrderPrice
					Select
						    MainVendorCD,
							'0000',
							LastNo,
							ChangeDate,
							SKUCD,
							IsNull(Rate,0),
							IsNull(OrderPriceWithoutTax,0),
							Null,
							IsNull(DeleteFlg,0),
							0,
							@OperatorCD,
							getdate(),
							@OperatorCD,
							getdate()
							from #tmpSKU3_N
							where MainVendorCD != Null
			Insert 
				Into M_JANOrderPrice
				Select
				'0000000000000',
				'0000',
				ts.LastNo,
				ts.ChangeDate,
				ts.SKUCD,
				IsNull(ts.Rate,0),
				IsNull(ts.OrderPriceWithoutTax,0),
				Null,
				IsNull(ts.DeleteFlg,0),
				0,
				@OperatorCD,
				getdate(),
				@OperatorCD,
				getdate()
				from #tmpSKU3_N as ts
				where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.LastNo
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)

			Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.LastNo,
			ChangeDate=ts.ChangeDate,
			SKUCD=ts.SKUCD,
			Rate=IsNull(ts.Rate,0),
			PriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
			Remarks=NULL,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_JANOrderPrice as mj
			inner join #tmpSKU3_N as ts on mj.AdminNO=ts.LastNo
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'

			drop table #tmpSKU3
			drop table #tmpSKU3_N
	End

	if @type =4
		Begin
			Create table [#tmpSKU4](
		[AdminNO] [varchar](50) NOT NULL,
		[ChangeDate] [date] NOT NULL,
		[ApprovalDate] [date] NULL,
		[SKUCD] [varchar](40) NULL,
		[JanCD] [varchar](13) NULL,
		[DeleteFlg] [tinyint] NULL,
		[SKUName] [varchar](100) NULL,
		[ITEMCD] [varchar](30) NULL,
		[SizeNO] [int] NULL,
		[ColorNO] [int] NULL,
		[SizeName] [varchar](20) NULL,
		[ColorName] [varchar](20) NULL,
		[MainVendorCD] [varchar](13) NULL,
		[VendorName] [varchar](50) NULL,
		[TaxRateFLG] [tinyint] NULL,
		[CostingKBN] [tinyint] NULL,
		[SaleExcludedFlg] [tinyint] NULL,
		[NormalCost] [money] NOT NULL,
		[PriceWithTax] [money] NULL,
		[PriceOutTax] [money] NULL,
		[OrderPriceWithTax] [money] NULL,
		[OrderPriceWithoutTax] [money] NULL,
		[Rate] [decimal](5, 2) NULL,
		)

			Create table [#tmpSKU4_N](
		[LastNo][int] Not Null,
		[AdminNO] [varchar](50) NOT NULL,
		[ChangeDate] [date] NOT NULL,
		[ApprovalDate] [date] NULL,
		[SKUCD] [varchar](40) NULL,
		[JanCD] [varchar](13) NULL,
		[DeleteFlg] [tinyint] NULL,
		[SKUName] [varchar](100) NULL,
		[ITEMCD] [varchar](30) NULL,
		[SizeNO] [int] NULL,
		[ColorNO] [int] NULL,
		[SizeName] [varchar](20) NULL,
		[ColorName] [varchar](20) NULL,
		[MainVendorCD] [varchar](13) NULL,
		[VendorName] [varchar](50) NULL,
		[TaxRateFLG] [tinyint] NULL,
		[CostingKBN] [tinyint] NULL,
		[SaleExcludedFlg] [tinyint] NULL,
		[NormalCost] [money] NOT NULL,
		[PriceWithTax] [money] NULL,
		[PriceOutTax] [money] NULL,
		[OrderPriceWithTax] [money] NULL,
		[OrderPriceWithoutTax] [money] NULL,
		[Rate] [decimal](5, 2) NULL,
		[RowNo][int] Null
		)

		declare @DocHandle4 int

		exec sp_xml_preparedocument @DocHandle4 output, @xml
		INSERT Into #tmpSKU4
		select *  FROM OPENXML (@DocHandle4, '/NewDataSet/test',2)
		with
		(
		AdminNO varchar(50),
		改定日 date,
		承認日 date ,
		SKUCD varchar(40) ,
		JANCD varchar(13),
		削除 tinyint ,
		商品名 varchar(100),
		ITEMCD varchar(30),
		サイズ枝番 int,
		カラー枝番 int,
		サイズ名 varchar(20) ,
		カラー名 varchar(20) ,
		主要仕入先CD varchar(13) ,
		主要仕入先名 varchar(50),
		税率区分 tinyint,
		原価計算方法 tinyint,
		Sale対象外区分 tinyint ,
		標準原価 money  ,
		税抜定価 money ,
		税込定価 money ,
		発注税込価格 money ,
		発注税抜価格 money ,
		掛率 decimal(5, 2) 
		)
		exec sp_xml_removedocument @DocHandle4;


		declare @AdminCounter4 as int = (select Max (AdminNo) from M_SKUCounter where MainKEY = 1);

								Insert into [#tmpSKU4_N]
								select (case when AdminNo = 'New' then @AdminCounter4+ RowNo else AdminNo end ) as LastNo , *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU4 ) f


									declare @LastNO4 as int =0;

								SEt @LastNO4=(SElect MAX(LastNo) from #tmpSKU4_N where AdminNO='New');
								Update M_SKUCounter
								Set AdminNO=@LastNO4
								where @LastNO4 <> 0
		
						Update M_SKU
		Set		
			AdminNO=ts.LastNo,
			ChangeDate=ts.ChangeDate,
			TaxRateFLG=ts.TaxRateFLG,
			CostingKBN=ts.CostingKBN,
			SaleExcludedFlg=ts.SaleExcludedFlg,
			NormalCost=IsNull(ts.NormalCost,0),
			PriceOutTax=IsNull(ts.PriceOutTax,0),
			PriceWithTax=IsNull(ts.PriceWithTax,0),
			OrderPriceWithTax=IsNull(ts.OrderPriceWithTax,0),
			OrderPriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
			Rate=IsNull(ts.Rate,0),
			ApprovalDate=ts.ApprovalDate,
			DeleteFlg=ts.DeleteFlg,
			UsedFlg=0,
			SKSUpdateFlg=1,
			SKSUpdateDateTime=Null,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_SKU as ms
			inner join #tmpSKU4_N  as ts on ts.LastNo =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate

						Delete mj
							from M_JANOrderPrice as mj
							
							Where mj.AdminNO  In 
							(
							Select ms.AdminNO 
							from M_SKU ms
							Inner join #tmpSKU4_N as ts on ts.LastNo= ms.AdminNO
							where ms.ItemCD=ts.ITEMCD
							)
						Insert 
					Into	 M_JANOrderPrice
					Select
						    MainVendorCD,
							'0000',
							LastNo,
							ChangeDate,
							SKUCD,
							IsNull(Rate,0),
							IsNull(OrderPriceWithoutTax,0),
							Null,
							IsNull(DeleteFlg,0),
							0,
							@OperatorCD,
							getdate(),
							@OperatorCD,
							getdate()
							from #tmpSKU4_N
							where MainVendorCD != Null


						Insert 
			Into M_JANOrderPrice
			Select
			'0000000000000',
			'0000',
			ts.LastNo,
			ts.ChangeDate,
			ts.SKUCD,
			IsNull(ts.Rate,0),
			IsNull(ts.OrderPriceWithoutTax,0),
			Null,
			IsNull(ts.DeleteFlg,0),
			0,
			@OperatorCD,
			getdate(),
			@OperatorCD,
			getdate()
			from #tmpSKU4_N as ts
			where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.LastNo
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)


						Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.LastNO,
			ChangeDate=ts.ChangeDate,
			SKUCD=ts.SKUCD,
			Rate=IsNull(ts.Rate,0),
			PriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
			Remarks=NULL,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_JANOrderPrice as mj
			inner join #tmpSKU4_N as ts on mj.AdminNO=ts.LastNo
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'


		drop table #tmpSKU4
		drop table #tmpSKU4_N




		End

	if @type =5
	Begin
		 Create table [#tmpSKU5](
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[ApprovalDate] [date] NULL,
	[SKUCD] [varchar](40) NULL,
	[JanCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[LastYearTerm] [varchar](6) NULL,
	[LastSeason] [varchar](6) NULL,
	[LastCatalogNO] [varchar](20) NULL,
	[LastCatalogPage] [varchar](20) NULL,
	[LastCatalogNOLong] [varchar](2000) NULL,
	[LastCatalogPageLong] [varchar](2000) NULL,
	[LastCatalogText] [varchar](1000) NULL,
	[LastInstructionsNO] [varchar](1000) NULL,
	[LastInstructionsDate] [date] NULL,
	) 

			Create table [#tmpSKU5_N](
	[LastNo][int] Not Null,
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[ApprovalDate] [date] NULL,
	[SKUCD] [varchar](40) NULL,
	[JanCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[LastYearTerm] [varchar](6) NULL,
	[LastSeason] [varchar](6) NULL,
	[LastCatalogNO] [varchar](20) NULL,
	[LastCatalogPage] [varchar](20) NULL,
	[LastCatalogNOLong] [varchar](2000) NULL,
	[LastCatalogPageLong] [varchar](2000) NULL,
	[LastCatalogText] [varchar](1000) NULL,
	[LastInstructionsNO] [varchar](1000) NULL,
	[LastInstructionsDate] [date] NULL,
	[RowNo][int] Null
	)


	declare @DocHandle5 int

	exec sp_xml_preparedocument @DocHandle5 output, @xml
	INSERT Into #tmpSKU5
	select *  FROM OPENXML (@DocHandle5, '/NewDataSet/test',2)
	with
	(
	AdminNO varchar(50),
	改定日 date,
	承認日 date ,
	SKUCD varchar(40) ,
	JANCD varchar(13),
	削除 tinyint ,
	商品名 varchar(100),
	ITEMCD varchar(30),
	サイズ枝番 int,
	カラー枝番 int,
	サイズ名 varchar(20) ,
	カラー名 varchar(20) ,
	年度 varchar(6) ,
	シーズン varchar(6) ,
	カタログ番号 varchar(20) ,
	カタログページ varchar(20) ,
	カタログ番号Long varchar(2000) ,
	カタログページLong varchar(2000) ,
	カタログ情報 varchar(1000),
	指示書番号 varchar(1000),
	指示書発行日 date
	)
	exec sp_xml_removedocument @DocHandle5;
		declare @AdminCounter5 as int = (select Max (AdminNo) from M_SKUCounter where MainKEY = 1);

								Insert into [#tmpSKU5_N]
								select (case when AdminNo = 'New' then @AdminCounter5+ RowNo else AdminNo end ) as LastNo , *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU5  ) f
							

								
									declare @LastNO5 as int =0;

								SEt @LastNO5=(SElect MAX(LastNo) from #tmpSKU5_N where AdminNO='New');
								Update M_SKUCounter
								Set AdminNO=@LastNO5
								where @LastNO5 <> 0
	
	
	Update M_SKU
	Set		
			AdminNO=ts.LastNo,
			ChangeDate=ts.ChangeDate,
			LastYearTerm=ts.LastYearTerm,
			LastSeason=ts.LastSeason,
			LastCatalogNO=ts.LastCatalogNO ,
			LastCatalogPage=ts.LastCatalogPage,
			LastCatalogNOLong=ts.LastCatalogNOLong,
			LastCatalogPageLong=ts.LastCatalogPageLong ,
			LastCatalogText=ts.LastCatalogText,
			LastInstructionsNO=ts.LastInstructionsNO,
			LastInstructionsDate=ts.LastInstructionsDate,
			ApprovalDate=ts.ApprovalDate,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,
			SKSUpdateFlg=1,
			SKSUpdateDateTime=Null,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_SKU as ms
			inner join #tmpSKU5_N as ts on ts.LastNo =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate
		

		
		Insert into M_SKUInfo
		Select 
		LastNo,
		ChangeDate,
		1,
		LastYearTerm,
		LastSeason,
		LastCatalogNO,
		LastCatalogPage,
		LastCatalogText,
		LastInstructionsNO,
		LastInstructionsDate,
		DeleteFlg,
		@OperatorCD,
		getdate(),
		@OperatorCD,
		getdate()
		From #tmpSKU5_N as ts
		Where not Exists(
					Select  AdminNO
					from M_SKUInfo as mi
					where mi.AdminNO=ts.LastNo
					and mi.ChangeDate= ts.ChangeDate
					and mi.SEQ=1
				)
		
		Update M_SKUInfo
		Set
		AdminNO=ts.LastNo,
		ChangeDate=ts.ChangeDate,
		SEQ=1,
		YearTerm=ts.LastYearTerm,
		Season=ts.LastSeason,
		CatalogNO=ts.LastCatalogNO,
		CatalogPage=ts.LastCatalogPage,
		CatalogText=ts.LastCatalogText,
		InstructionsNO=ts.LastInstructionsNO,
		InstructionsDate=ts.LastInstructionsDate,
		DeleteFlg=ts.DeleteFlg,
		InsertOperator=@OperatorCD,
		InsertDateTime=getdate(),
		UpdateOperator=@OperatorCD,
		UpdateDateTime=getdate()
		From M_SKUInfo as mI
		inner join #tmpSKU5_N as ts on ts.LastNo=mI.AdminNO
		where mI.AdminNO= ts.LastNo
		and mI.ChangeDate=ts.ChangeDate
		and mI.SEQ= 1

	drop table #tmpSKU5
	drop table #tmpSKU5_N

	End  

	if @type=6
	Begin 
	  Create table [#tmpSKU6](
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[ApprovalDate] [date] NULL,
	[SKUCD] [varchar](40) NULL,
	[JanCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[TagName1] [varchar](20) NULL,
	[TagName2] [varchar](20) NULL,
	[TagName3] [varchar](20) NULL,
	[TagName4] [varchar](20) NULL,
	[TagName5] [varchar](20) NULL,
	[TagName6] [varchar](20) NULL,
	[TagName7] [varchar](20) NULL,
	[TagName8] [varchar](20) NULL,
	[TagName9] [varchar](20) NULL,
	[TagName10] [varchar](20) NULL,
	)

 Create table [#tmpSKU6_N](
	[LastNo][int] NOT NULL,
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NOT NULL,
	[ApprovalDate] [date] NULL,
	[SKUCD] [varchar](40) NULL,
	[JanCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[TagName1] [varchar](20) NULL,
	[TagName2] [varchar](20) NULL,
	[TagName3] [varchar](20) NULL,
	[TagName4] [varchar](20) NULL,
	[TagName5] [varchar](20) NULL,
	[TagName6] [varchar](20) NULL,
	[TagName7] [varchar](20) NULL,
	[TagName8] [varchar](20) NULL,
	[TagName9] [varchar](20) NULL,
	[TagName10] [varchar](20) NULL,
	[RowNo][int] NUll,
	)
	declare @DocHandle6 int

	exec sp_xml_preparedocument @DocHandle6 output, @xml
	INSERT Into #tmpSKU6
	select *  FROM OPENXML (@DocHandle6, '/NewDataSet/test',2)
	with
	(
	AdminNO varchar(50),
	改定日 date,
	承認日 date ,
	SKUCD varchar(40) ,
	JANCD varchar(13),
	削除 tinyint ,
	商品名 varchar(100),
	ITEM varchar(30),
	サイズ枝番 int,
	カラー枝番 int,
	サイズ名 varchar(20) ,
	カラー名 varchar(20) ,
	タグ1  varchar(6) ,
	タグ2  varchar(6) ,
	タグ3  varchar(20) ,
	タグ4 varchar(20) ,
	タグ5  varchar(20) ,
	タグ6  varchar(20) ,
	タグ7  varchar(20),
	タグ8 varchar(20),
	タグ9 varchar(20),
	タグ10 varchar(20)
	)
	exec sp_xml_removedocument @DocHandle6;

	declare @AdminCounter6 as int = (select Max (AdminNo) from M_SKUCounter where MainKEY = 1);

								Insert into [#tmpSKU6_N]
								select (case when AdminNo = 'New' then @AdminCounter6+ RowNo else AdminNo end ) as LastNo , *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU6  ) f


							

							
									declare @LastNO6 as int =0;

								SEt @LastNO6=(SElect MAX(LastNo) from #tmpSKU6_N where AdminNO='New');
								Update M_SKUCounter
								Set AdminNO=@LastNO6
								where @LastNO6 <> 0
	
	
	Update M_SKU
	Set		
			AdminNO=ts.LastNo,
			ChangeDate=ts.ChangeDate,
			ApprovalDate=ts.ApprovalDate,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,
			SKSUpdateFlg=1,
			SKSUpdateDateTime=Null,
			InsertOperator=@OperatorCD,
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate()
			from M_SKU as ms
			inner join #tmpSKU6_N as ts on ts.LastNo =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate

		Delete mtag
		from M_SKUTag as mtag
		inner join #tmpSKU6_N as ts on ts.LastNo=mtag.AdminNO
		Where mtag.AdminNO=ts.LastNo
		and mtag.ChangeDate=ts.ChangeDate
		Insert 
		Into M_SKUTag
		SELECT LastNo,ChangeDate,ROW_NUMBER() OVER (ORDER BY LastNo),ColumnValue from #tmpSKU6_N as ts
		 Unpivot(ColumnValue For ColumnName IN (TagName1,TagName2,TagName3,TagName4,TagName5,TagName6,TagName7,TagName8,TagName9,TagName10)) AS H

	drop table #tmpSKU6
	drop table #tmpSKU6_N
	End

	if @type = 7
  begin
	CREATE TABLE [dbo].[#tmpSKU7](
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NULL,
	[ApprovalDate] [date]  NULL,
	[SKUCD] [varchar](40) NULL,
	[JANCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[KanaName] [varchar](50) NULL,
	[SKUShortName] [varchar](40) NULL,
	[EnglishName] [varchar](80) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL
	) 

	CREATE TABLE [dbo].[#tmpSKU7_N](
	[LastNo][int] NOT NUll,
	[LastJanNo][varchar](13)Null,
	[AdminNO] [varchar](50) NOT NULL,
	[ChangeDate] [date] NULL,
	[ApprovalDate] [date]  NULL,
	[SKUCD] [varchar](40) NULL,
	[JANCD] [varchar](13) NULL,
	[DeleteFlg] [tinyint] NULL,
	[SKUName] [varchar](100) NULL,
	[KanaName] [varchar](50) NULL,
	[SKUShortName] [varchar](40) NULL,
	[EnglishName] [varchar](80) NULL,
	[ITEMCD] [varchar](30) NULL,
	[SizeNO] [int] NULL,
	[ColorNO] [int] NULL,
	[SizeName] [varchar](20) NULL,
	[ColorName] [varchar](20) NULL,
	[RowNo][int] Null
	)

	declare @DocHandle7 int

	exec sp_xml_preparedocument @DocHandle7 output, @xml
	insert into #tmpSKU7
	select *  FROM OPENXML (@DocHandle7, '/NewDataSet/test',2)
	with
	(
	AdminNO varchar(50),
	改定日  date,
	承認日 date,
	SKUCD varchar(40),
	JANCD varchar(13),
	削除 tinyint,
	商品名 varchar(100),
	カナ名 varchar(50),
	略名 varchar(40),
	英語名 varchar(80),
	ITEMCD varchar(30),
	サイズ枝番 int,
	カラー枝番 int,
	サイズ名 varchar(20),
	カラー名 varchar(20)
	)
exec sp_xml_removedocument @DocHandle7;
		
		Insert into [#tmpSKU7_N]
							select (case when AdminNo = 'New' then @AdminCounter+ RowNo else AdminNo end ) as LastNo,
							(case when JanCD = 'Auto' then @AdminCounterJ+RowNo else JanCD end ) as LastJanNo, *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU7  ) f

								declare @LastNO7 as int=0;
								SEt @LastNO7 =(SElect MAX(LastNo) from #tmpSKU7_N where AdminNO='New');
								declare @LastJanNo7 as int=0;
							SEt	@LastJanNo7=(SElect MAX(LastJanNo) from #tmpSKU7_N where JanCD='Auto');
							
							
								Update M_SKUCounter
								Set AdminNO=@LastNO7
								Where @LastNO7 <> 0

								
								Update M_JANCounter
								Set JanCount=@LastJanNo7
								Where @LastJanNo7 <> 0

							
							

		
		Update	M_SKU
			SET	SKUName=ts.SKUName,
				SKUNameLong=ts.SKUName,
				KanaName=ts.KanaName,
				EnglishName=ts.EnglishName,
				SKUShortName=IsNull(ts.SKUShortName,LEFT(ts.SKUName,40)),
				JanCD=ts.LastJanNo,
				ITemCD=ts.ITEMCD,
				SizeNO=ts.SizeNO,
				ColorNO=ts.ColorNo,
				ApprovalDate=ts.ApprovalDate,--ts.ApprovalDate,
				DeleteFlg=IsNull(ts.DeleteFlg,0),
				UsedFlg=0,  --ts.UsedFlg,
				SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
				SKSUpdateDateTime=Null,			--ts.SKSUpdateDateTime,
				InsertOperator='0001' ,    --ts.InsertOperator,
				InsertDateTime=getdate(),			--ts.InsertDateTime,
				UpdateOperator='0001'	,		--ts.UpdateOperator,
				UpdateDateTime=getdate()	
		from M_SKU as ms
		inner join #tmpSKU7_N  as ts on ts.LastNo =ms.AdminNO
		where ms.ChangeDate=ts.ChangeDate

	drop table #tmpSKU7
	drop table #tmpSKU7_N

	end

	if @type =8
		Begin
			CREATE TABLE [dbo].[#tmpSKU8](
						[AdminNO] [varchar](50) NOT NULL,
						[ChangeDate] [date] NULL,
						[ApprovalDate] [date]  NULL,
						[SKUCD] [varchar](40) NULL,
						[JANCD] [varchar](13) NULL,
						[DeleteFlg] [tinyint] NULL,
						[SKUName] [varchar](100) NULL,
						[ITEMCD] [varchar](30) NULL,
						[SizeNO] [int] NULL,
						[ColorNO] [int] NULL,
						[SizeName] [varchar](20) NULL,
						[ColorName] [varchar](20) NULL,
						[APIKey] [varchar](20) NULL,
						[ShouhinCD] [varchar](20) NULL,
					)  

						CREATE TABLE [dbo].[#tmpSKU8_N](
						[LastNo][int] NOT NULL,
						[LastJanNo][varchar](13)Null,
						[AdminNO] [varchar](50) NOT NULL,
						[ChangeDate] [date] NULL,
						[ApprovalDate] [date]  NULL,
						[SKUCD] [varchar](40) NULL,
						[JANCD] [varchar](13) NULL,
						[DeleteFlg] [tinyint] NULL,
						[SKUName] [varchar](100) NULL,
						[ITEMCD] [varchar](30) NULL,
						[SizeNO] [int] NULL,
						[ColorNO] [int] NULL,
						[SizeName] [varchar](20) NULL,
						[ColorName] [varchar](20) NULL,
						[APIKey] [varchar](20) NULL,
						[ShouhinCD] [varchar](20) NULL,
						[RowNo][int] Null
					)

			declare @DocHandle8 int

				exec sp_xml_preparedocument @DocHandle8 output, @xml
				insert into #tmpSKU8
				select *  FROM OPENXML (@DocHandle8, '/NewDataSet/test',2)
				with
				(
				AdminNO varchar(50),
				ChangeDate  date,
				ApprovalDate date,
				SKUCD varchar(40),
				JANCD varchar(13),
				DeleteFlg tinyint,
				SKUName varchar(100),
				ITEMCD varchar(30),
				SizeNO int,
				ColorNO int,
				SizeName varchar(20),
				ColorName varchar(20),
				APIKey varchar(20),
				ShouhinCD varchar(20)
				)
			exec sp_xml_removedocument @DocHandle8;

		Insert into [#tmpSKU8_N]
							select (case when AdminNo = 'New' then @AdminCounter+ RowNo else AdminNo end ) as LastNo,
							(case when JanCD = 'Auto' then @AdminCounterJ+RowNo else JanCD end ) as LastJanNo, *  from 
								 (select  * ,   (ROW_NUMBER () Over (order by AdminNo desc)) as RowNo
								from #tmpSKU8  ) f

								declare @LastNO8 as int=0;
								declare @LastJanNo8 as int=0;
								Set @LastNO8=(SElect MAX(LastNo) from #tmpSKU8_N where AdminNO='New');
								Set  @LastJanNo8=(SElect MAX(LastJanNo) from #tmpSKU8_N where JanCD='Auto');
							
							
								Update M_SKUCounter
								Set AdminNO=@LastNO8
								Where @LastNO8 <> 0

								
								Update M_JANCounter
								Set JanCount=@LastJanNo8
								Where @LastJanNo8 <> 0 

			
	
		Update	M_SKU
			SET	
				ApprovalDate=ts.ApprovalDate,--ts.ApprovalDate,
				DeleteFlg=IsNull(ts.DeleteFlg,0),
				UsedFlg=0,  --ts.UsedFlg,
				SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
				SKSUpdateDateTime=Null,			--ts.SKSUpdateDateTime,
				InsertOperator=@OperatorCD ,    --ts.InsertOperator,
				InsertDateTime=getdate(),			--ts.InsertDateTime,
				UpdateOperator=@OperatorCD	,		--ts.UpdateOperator,
				UpdateDateTime=getdate()	
		from M_SKU as ms
		inner join #tmpSKU8_N  as ts on ts.LastNo =ms.AdminNO
		where ms.ChangeDate=ts.ChangeDate


	
				Insert into M_Site
				Select 
				ts.LastNo,
				ts.APIKey,
				ts.ShouhinCD,
				ts.ShouhinCD + '.html',
				@OperatorCD,
				getdate(),
				@OperatorCD,
				getdate()
				from #tmpSKU8_N as ts
				where not Exists(
								Select AdminNO
								from M_Site as ms
								inner join M_API as mp on mp.APIKey=ms.APIKey
								where ms.AdminNO=ts.LastNO
								and ms.APIKey=ts.APIKey
							)
			

				Update M_Site
				Set 
				AdminNO=ts.LastNo,
				APIKey=ts.APIKey,
				ShouhinCD=ts.ShouhinCD,
				SiteURL=mp.ShopURL +ts.ShouhinCD + '.html',
				InsertOperator=@OperatorCD,
				InsertDateTime=getdate(),
				UpdateOperator=@OperatorCD,
				UpdateDateTime=getdate()

				from M_Site as ms
				inner join #tmpSKU8_N as ts on ts.LastNo=ms.AdminNO
				inner join M_API as mp on mp.APIKey=ms.APIKey


			drop table #tmpSKU8
			drop table #tmpSKU8_N
	End
	
	  exec dbo.L_Log_Insert @OperatorCD,@ProgramID,@PC,Null,@KeyItem
	
END