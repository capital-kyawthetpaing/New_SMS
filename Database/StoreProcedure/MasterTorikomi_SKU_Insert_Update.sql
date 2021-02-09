
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
Create PROCEDURE [dbo].[MasterTorikomi_SKU_Insert_Update]
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
	if @type =1
		Begin
		Create table [#tmpSKU1](
	
				[AdminNO] [int] NOT NULL,
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
				[MakerVendorCD] [varchar](13) NULL,
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
				[NormalCost] [money] NOT NULL,
				[SaleExcludedFlg] [tinyint] NULL,
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
				AdminNO int,
				ChangeDate date,
				SKUCD varchar(40) ,
				VariousFLG tinyint,
				SKUName varchar(100),
				KanaName varchar(50),
				SKUShortName varchar(40),
				EnglishName varchar(80),
				ITEM varchar(30),
				SizeNO int,
				ColorNO int,
				JanCD varchar(13),
				SetKBN tinyint,
				PresentKBN tinyint ,
				SampleKBN tinyint ,
				DiscountKBN tinyint,
				SizeName varchar(20) ,
				ColorName varchar(20) ,
				WebFlg tinyint ,
				RealStoreFlg tinyint ,
				MainVendorCD varchar(13) ,
				MakerVendorCD varchar(13) ,
				BrandCD varchar(6) ,
				MakerItem varchar(50) ,
				TaniCD varchar(2) ,
				SportsCD varchar(6) ,
				SegmentCD varchar(6) ,
				ZaikoKBN tinyint ,
				Rack varchar(10) ,
				VirtualFlg tinyint ,
				DirectFlg tinyint ,
				ReserveCD varchar(3) ,
				NoticesCD varchar(3) ,
				PostageCD varchar(3) ,
				ManufactCD varchar(3) ,
				ConfirmCD varchar(3) ,
				WebStockFlg tinyint ,
				StopFlg tinyint ,
				DiscontinueFlg tinyint ,
				SoldOutFlg tinyint ,
				InventoryAddFlg tinyint ,
				MakerAddFlg tinyint ,
				StoreAddFlg tinyint ,
				NoNetOrderFlg tinyint ,
				EDIOrderFlg tinyint ,
				AutoOrderFlg tinyint ,
				CatalogFlg tinyint ,
				ParcelFlg tinyint ,
				TaxRateFLG tinyint ,
				CostingKBN tinyint ,
				NormalCost money  ,
				SaleExcludedFlg tinyint ,
				PriceWithTax money ,
				PriceOutTax money ,
				OrderPriceWithTax money ,
				OrderPriceWithoutTax money ,
				Rate decimal(5, 2) ,
				SaleStartDate date ,
				WebStartDate date,
				OrderAttentionCD varchar(3) ,
				OrderAttentionNote varchar(100) ,
				CommentInStore varchar(300) ,
				CommentOutStore varchar(300) ,
				LastYearTerm varchar(6) ,
				LastSeason varchar(6) ,
				LastCatalogNO varchar(20) ,
				LastCatalogPage varchar(20) ,
				LastCatalogNOLong varchar(2000) ,
				LastCatalogPageLong varchar(2000) ,
				LastCatalogText varchar(1000),
				LastInstructionsNO varchar(1000),
				LastInstructionsDate date,
				WebAddress varchar(200),
				SetAdminCD int ,
				SetItemCD varchar(30),
				SetSKUCD varchar(30),
				SetSU int,
				ExhibitionSegmentCD varchar(5),
				OrderLot int ,
				ExhibitionCommonCD varchar(30),
				ApprovalDate date ,
				DeleteFlg tinyint ,
				TagName1  varchar(6) ,
				TagName2  varchar(6) ,
				TagName3  varchar(20) ,
				TagName4 varchar(20) ,
				TagName5  varchar(20) ,
				TagName6  varchar(20) ,
				TagName7  varchar(20),
				TagName8 varchar(20),
				TagName9 varchar(20),
				TagName10 varchar(20)
				)
				exec sp_xml_removedocument @DocHandle1;
				
				Insert 
				Into    M_SKU
				Select	
						AdminNO,
						ChangeDate ,
						SKUCD ,
						IsNull(VariousFLG,0),
						SKUName,
						SKUName,
						KanaName,
						IsNull(SKUShortName,SUBSTRING(SKUName,0,20) ),
						EnglishName ,
						ITEMCD ,
						SizeNO,
						ColorNO ,
						JanCD ,
						SizeName,
						ColorName ,
						SizeName,
						ColorName ,
						SetKBN,
						PresentKBN,
						DiscountKBN,
						WebFlg,
						RealStoreFlg,
						MainVendorCD ,
						MakerVendorCD ,
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
						ts.SaleExcludedFlg,
						IsNull(NormalCost,0),
						IsNull(PriceOutTax,0) ,
						IsNull(PriceWithTax,0),
						IsNull(OrderPriceWithTax ,0),
						IsNull(OrderPriceWithoutTax,0),
						IsNull(Rate,0),
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
				from #tmpSKU1 as ts
				where not Exists(
									Select ms.AdminNO
									from M_SKU as ms
									where ms.AdminNO=ts.AdminNO
									and ms.ChangeDate=ts.ChangeDate
								)
				
				
				Update M_SKU
				Set		
						AdminNo=ts.AdminNO,
						ChangeDate=ts.ChangeDate ,
						SKUCD=ts.SKUCD ,
						VariousFLG=IsNull(ts.VariousFLG,0),
						SKUName=ts.SKUName,
						SKUNameLong=ts.SKUName,
						KanaName=ts.KanaName,
						SKUShortName=IsNull(ts.SKUShortName,SUBSTRING(ts.SKUName,0,20) ),
						EnglishName=ts.EnglishName ,
						ITemCD=ts.ITEMCD ,
						SizeNO=ts.SizeNO,
						ColorNO=ts.ColorNO ,
						JanCD=ts.JanCD ,
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
						MakerVendorCD=ts.MakerVendorCD ,
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
						inner join #tmpSKU1  as ts on ts.AdminNO =ms.AdminNO
						where ms.ChangeDate=ts.ChangeDate

					Delete mtag
						from M_SKUTag as mtag
						inner join #tmpSKU1 as ts on ts.AdminNO=mtag.AdminNO
						Where mtag.AdminNO=ts.AdminNo
						and mtag.ChangeDate=ts.ChangeDate

					Insert 
					Into	M_SKUTag
					SELECT AdminNO,ChangeDate,ROW_NUMBER() OVER (ORDER BY AdminNO),ColumnValue from #tmpSKU1 as ts
						 Unpivot(ColumnValue For ColumnName IN (TagName1,TagName2,TagName3,TagName4,TagName5,TagName6,TagName7,TagName8,TagName9,TagName10)) AS H

						Delete mj
							from M_JANOrderPrice as mj
							Where mj.AdminNO IN 
							(
							Select ms.AdminNO 
							from M_SKU as ms
							Inner join #tmpSKU1 as ts on ts.AdminNO= ms.AdminNO
							where ms.ItemCD=ts.ITEMCD
							)
						--	and mj.ChangeDate= ts.ChangeDate
					
					Insert 
					Into	 M_JANOrderPrice
					Select
						    MainVendorCD,
							'0000',
							AdminNO,
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
							from #tmpSKU1

				Insert 
				Into M_JANOrderPrice
				Select
				'0000000000000',
				'0000',
				ts.AdminNO,
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
				from #tmpSKU1 as ts
				where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.AdminNO
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)

			Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU1 as ts on mj.AdminNO=ts.AdminNO
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'

					


						Insert into M_SKUInfo
						Select 
						ts.AdminNO,
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

						From #tmpSKU1 as ts
						Where not Exists(
						                  Select AdminNO
										  from M_SKUInfo as mi
										  where mi.AdminNO=ts.AdminNO
										  and mi.ChangeDate=ts.ChangeDate
										  and mi.SEQ=1
											)
				
						Update M_SKUInfo
						Set
						AdminNO=ts.AdminNO,
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
						inner join #tmpSKU1 as ts on ts.AdminNO=mI.AdminNO
						where mI.AdminNO= ts.AdminNO
						and mI.ChangeDate=ts.ChangeDate
						and mI.SEQ= 1

				drop table #tmpSKU1


		End

	if @type =2
	  Begin
	   Create table [#tmpSKU2](
	
	[AdminNO] [int] NOT NULL,
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
	[MakerVendorCD] [varchar](13) NULL,
	[MakerVendorName] [varchar](50) NULL,
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

	declare @DocHandle2 int

	exec sp_xml_preparedocument @DocHandle2 output, @xml
	insert into #tmpSKU2
	select *  FROM OPENXML (@DocHandle2, '/NewDataSet/test',2)
			with
			(
	AdminNO int,
	ChangeDate date,
	SKUCD varchar(40) ,
	VariousFLG tinyint,
	SKUName varchar(100),
	KanaName varchar(50),
	SKUShortName varchar(40),
	EnglishName varchar(80),
	ITEMCD varchar(30),
	SizeNO int,
	ColorNO int,
	JanCD varchar(13),
	SizeName varchar(20) ,
	ColorName varchar(20) ,
	MainVendorCD varchar(13) ,
	VendorName varchar(50),
	MakerVendorCD varchar(13) ,
	MakerVendorName varchar(50),
	BrandCD varchar(6) ,
	BrandName varchar(40),
	MakerItem varchar(50) ,
	TaniCD varchar(2) ,
	TaniName varchar(100),
	SportsCD varchar(6) ,
	SportsName varchar(100),
	SegmentCD varchar(6) ,
	SegmentName varchar(100),
	ZaikoKBN tinyint ,
	Rack varchar(10) ,
	NormalCost money  ,
	PriceWithTax money ,
	PriceOutTax money ,
	OrderPriceWithTax money ,
	OrderPriceWithoutTax money ,
	Rate decimal(5, 2) ,
	SaleStartDate date ,
	WebStartDate date,
	OrderAttentionCD varchar(3) ,
	OrderAttentionName varchar(100),
	OrderAttentionNote varchar(100) ,
	CommentInStore varchar(300) ,
	CommentOutStore varchar(300) ,
	SetSU int,
	ExhibitionSegmentCD varchar(5),
	ExhibitionSegmentName varchar(100),
	OrderLot int ,
	ApprovalDate date ,
	DeleteFlg tinyint  

	)
	exec sp_xml_removedocument @DocHandle2;


	
	Insert 
	Into M_SKU
		(AdminNO
		,ChangeDate
		,SKUCD
		,VariousFLG
		,SKUName
		,KanaName
		,SKUShortName
		,EnglishName
		,ITEMCD
		,SizeNO
		,ColorNO,
		JanCD
		,SizeName
		,ColorName
		,MainVendorCD
		,MakerVendorCD
		,BrandCD
		,MakerItem
		,TaniCD
		,SportsCD
		,SegmentCD
		,Rack
		,NormalCost
		,PriceOutTax
		,PriceWithTax
		,OrderPriceWithTax
		,OrderPriceWithoutTax
		,Rate
		,SaleStartDate
		,WebStartDate
		,OrderAttentionCD
		,OrderAttentionNote
		,CommentInStore
		,CommentOutStore
		,SetAdminCD
		,SetItemCD
		,SetSKUCD
		,SetSU
		,ExhibitionSegmentCD
		,OrderLot
		,ExhibitionCommonCD
		,ApprovalDate
		,DeleteFlg
		,UsedFlg
		,SKSUpdateFlg
		,SKSUpdateDateTime
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		)
	Select
		ts.AdminNO ,
		ts.ChangeDate ,
		ts.SKUCD ,
		IsNull(ts.VariousFLG,0) ,
		ts.SKUName ,
		ts.KanaName ,
		IsNull(ts.SKUShortName,SUBSTRING(ts.SKUName,0,20) ) ,
		ts.EnglishName ,
		ts.ITEMCD ,
		ts.SizeNO,
		ts.ColorNO ,
		ts.JanCD ,
		ts.SizeName  ,
		ts.ColorName ,
		ts.MainVendorCD ,
		ts.MakerVendorCD ,
		ts.BrandCD ,
		ts.MakerItem  ,
		ts.TaniCD ,
		ts.SportsCD ,
		ts.SegmentCD ,
		ts.Rack ,
		IsNull(ts.NormalCost,0)  ,
		IsNull(ts.PriceOutTax,0)  ,
		IsNull(ts.PriceWithTax,0)  ,
		IsNull(ts.OrderPriceWithTax ,0) ,
		IsNull(ts.OrderPriceWithoutTax,0)  ,
		IsNull(ts.Rate,0),
		ts.SaleStartDate  ,
		ts.WebStartDate ,
		ts.OrderAttentionCD  ,
		ts.OrderAttentionNote ,
		ts.CommentInStore,
		ts.CommentOutStore ,
		0,	--	ts.SetAdminCD,
		Null,	--ts.SetItemCD,
		Null,		--ts.SetSKUCD,
		0, --ts.SetSU,
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
	from #tmpSKU2 as ts
	where not Exists(

					Select AdminNO
					from M_SKU as ms
					where ms.AdminNO=ts.AdminNO
					and ms.ChangeDate=ts.ChangeDate
				)
	
	
	Update M_SKU
	Set		AdminNO=ts.AdminNO ,
			ChangeDate=ts.ChangeDate ,
			SKUCD=ts.SKUCD ,
			VariousFLG=ts.VariousFLG ,
			SKUName=ts.SKUName ,
			KanaName=ts.KanaName ,
			SKUShortName=IsNull(ts.SKUShortName,SUBSTRING(ts.SKUName,0,20) ),
			EnglishName=ts.EnglishName ,
			ITemCD=ts.ITEMCD ,
			SizeNO=ts.SizeNO,
			ColorNO=ts.ColorNO ,
			JanCD=ts.JanCD ,
			SizeName=ts.SizeName  ,
			ColorName=ts.ColorName ,
			MainVendorCD=ts.MainVendorCD ,
			MakerVendorCD=ts.MakerVendorCD ,
			BrandCD=ts.BrandCD ,
			MakerItem=ts.MakerItem  ,
			TaniCD=ts.TaniCD ,
			SportsCD=ts.SportsCD ,
			SegmentCD=ts.SegmentCD ,
			Rack=ts.Rack ,
			NormalCost=IsNull(ts.NormalCost,0),
			PriceOutTax=IsNull(ts.PriceOutTax,0)  ,
			PriceWithTax=IsNull(ts.PriceWithTax ,0) ,
			OrderPriceWithTax=IsNull(ts.OrderPriceWithTax,0),
			OrderPriceWithoutTax=IsNull(ts.OrderPriceWithoutTax,0),
			Rate=IsNull(ts.Rate,0),
			SaleStartDate=ts.SaleStartDate  ,
			WebStartDate=ts.WebStartDate ,
			OrderAttentionCD=ts.OrderAttentionCD  ,
			OrderAttentionNote=ts.OrderAttentionNote ,
			CommentInStore=ts.CommentInStore,
			CommentOutStore=ts.CommentOutStore ,
			SetAdminCD=0,	--	ts.SetAdminCD,
			SetItemCD=Null,	--ts.SetItemCD,
			SetSKUCD=Null,		--ts.SetSKUCD,
			SetSU=0, --ts.SetSU,
			ExhibitionSegmentCD=ts.ExhibitionSegmentCD,
			OrderLot=IsNull(ts.OrderLot,1),
			ExhibitionCommonCD=Null,	--ts.ExhibitionCommonCD,
			ApprovalDate='2021-01-27' , --ts.ApprovalDate,
			DeleteFlg=IsNull(ts.DeleteFlg,0),
			UsedFlg=0,			--ts.UsedFlg,
			SKSUpdateFlg=1,		--ts.SKSUpdateFlg,
			SKSUpdateDateTime=Null,		--ts.SKSUpdateDateTime,
			InsertOperator='0001',
			InsertDateTime=getdate(),
			UpdateOperator=@OperatorCD,
			UpdateDateTime=getdate() 
			from M_SKU as ms
			inner join #tmpSKU2  as ts on ts.AdminNO =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate
		Delete mj
			from M_JANOrderPrice as mj
			
			Where mj.AdminNO In
			(
			Select ms.AdminNO 
			from M_SKU as ms
			Inner join #tmpSKU2 as ts on ts.AdminNO= ms.AdminNO
			where ms.ItemCD=ts.ITEMCD
			)
		--	and mj.ChangeDate= ts.ChangeDate

			Insert 
			Into M_JANOrderPrice
			Select
			MainVendorCD,
			'0000',
			AdminNO,
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
			from #tmpSKU2 

				Insert 
				Into M_JANOrderPrice
				Select
				'0000000000000',
				'0000',
				ts.AdminNO,
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
				from #tmpSKU2 as ts
				where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.AdminNO
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)



		

			Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU2 as ts on mj.AdminNO=ts.AdminNO
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'


	drop table #tmpSKU2

	  End 

	if @type =3
		Begin
		 Create table [#tmpSKU3](
			[AdminNO] [int] NOT NULL,
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
			[NormalCost] [money] NOT NULL,
			[SaleExcludedFlg] [tinyint] NULL,
			[PriceWithTax] [money] NULL,
			[PriceOutTax] [money] NULL,
			[OrderPriceWithTax] [money] NULL,
			[OrderPriceWithoutTax] [money] NULL,
			[Rate] [decimal](5, 2) NULL,
			)

			declare @DocHandle3 int

			exec sp_xml_preparedocument @DocHandle3 output, @xml
			INSERT Into #tmpSKU3
			select *  FROM OPENXML (@DocHandle3, '/NewDataSet/test',2)
			with
			(
			AdminNO int,
			ChangeDate date,
			ApprovalDate date ,
			SKUCD varchar(40) ,
			JanCD varchar(13),
			DeleteFlg tinyint ,
			SKUName varchar(100),
			ITEMCD varchar(30),
			SizeNO int,
			ColorNO int,
			SizeName varchar(20) ,
			ColorName varchar(20) ,
			MainVendorCD varchar(13),
			VendorName varchar(50),
			SetKBN tinyint,
			PresentKBN tinyint ,
			SampleKBN tinyint ,
			DiscountKBN tinyint,
			WebFlg tinyint ,
			RealStoreFlg tinyint ,
			ZaikoKBN tinyint ,
			VirtualFlg tinyint ,
			DirectFlg tinyint ,
			ReserveCD varchar(3) ,
			NoticesCD varchar(3) ,
			PostageCD varchar(3) ,
			ManufactCD varchar(3) ,
			ConfirmCD varchar(3) ,
			WebStockFlg tinyint ,
			StopFlg tinyint ,
			DiscontinueFlg tinyint ,
			SoldOutFlg tinyint ,
			InventoryAddFlg tinyint ,
			MakerAddFlg tinyint ,
			StoreAddFlg tinyint ,
			NoNetOrderFlg tinyint ,
			EDIOrderFlg tinyint ,
			AutoOrderFlg tinyint ,
			CatalogFlg tinyint ,
			ParcelFlg tinyint ,
			NormalCost money  ,
			SaleExcludedFlg tinyint ,
			PriceWithTax money ,
			PriceOutTax money ,
			OrderPriceWithTax money ,
			OrderPriceWithoutTax money ,
			Rate decimal(5, 2) 
			)
			exec sp_xml_removedocument @DocHandle3;
			
			Update M_SKU
			Set		
					AdminNo=ts.AdminNO,
					ChangeDate=ts.ChangeDate ,
					SKUCD=ts.SKUCD ,
					SKUName=ts.SKUName,
					SKUNameLong=ts.SKUName,
					ITemCD=ts.ITEMCD ,
					SizeNO=ts.SizeNO,
					ColorNO=ts.ColorNO ,
					JanCD=ts.JanCD ,
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
					inner join #tmpSKU3  as ts on ts.AdminNO =ms.AdminNO
					where ms.ChangeDate=ts.ChangeDate


			
							Delete mj
							from M_JANOrderPrice as mj
							
							Where mj.AdminNO In 
							(
							Select ms.AdminNO 
							from M_SKU as ms
							Inner join #tmpSKU3 as ts on ts.AdminNO= ms.AdminNO
							where ms.ItemCD=ts.ITEMCD
							)
						
					
					Insert 
					Into	 M_JANOrderPrice
					Select
						    MainVendorCD,
							'0000',
							AdminNO,
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
							from #tmpSKU3


				Insert 
				Into M_JANOrderPrice
				Select
				'0000000000000',
				'0000',
				ts.AdminNO,
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
				from #tmpSKU3 as ts
				where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.AdminNO
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)



		

			Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU3 as ts on mj.AdminNO=ts.AdminNO
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'

			drop table #tmpSKU3
	End

	if @type =4
		Begin
			Create table [#tmpSKU4](
		[AdminNO] [int] NOT NULL,
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

		declare @DocHandle4 int

		exec sp_xml_preparedocument @DocHandle4 output, @xml
		INSERT Into #tmpSKU4
		select *  FROM OPENXML (@DocHandle4, '/NewDataSet/test',2)
		with
		(
		AdminNO int,
		ChangeDate date,
		ApprovalDate date ,
		SKUCD varchar(40) ,
		JanCD varchar(13),
		DeleteFlg tinyint ,
		SKUName varchar(100),
		ITEMCD varchar(30),
		SizeNO int,
		ColorNO int,
		SizeName varchar(20) ,
		ColorName varchar(20) ,
		MainVendorCD varchar(13) ,
		VendorName varchar(50),
		TaxRateFLG tinyint,
		CostingKBN tinyint,
		SaleExcludedFlg tinyint ,
		NormalCost money  ,
		PriceWithTax money ,
		PriceOutTax money ,
		OrderPriceWithTax money ,
		OrderPriceWithoutTax money ,
		Rate decimal(5, 2) 
		)
		exec sp_xml_removedocument @DocHandle4;

		Update M_SKU
		Set		
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU4  as ts on ts.AdminNO =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate
	

							Delete mj
							from M_JANOrderPrice as mj
							
							Where mj.AdminNO  In 
							(
							Select ms.AdminNO 
							from M_SKU ms
							Inner join #tmpSKU4 as ts on ts.AdminNO= ms.AdminNO
							where ms.ItemCD=ts.ITEMCD
							)
							
					
					Insert 
					Into	 M_JANOrderPrice
					Select
						    MainVendorCD,
							'0000',
							AdminNO,
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
							from #tmpSKU4


			Insert 
			Into M_JANOrderPrice
			Select
			'0000000000000',
			'0000',
			ts.AdminNO,
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
			from #tmpSKU4 as ts
			where not Exists(

					Select AdminNO
					from M_JANOrderPrice as mj
					where mj.AdminNO= ts.AdminNO
					and mj.ChangeDate=ts.ChangeDate
					and mj.VendorCD='0000000000000'
				)


			Update M_JANOrderPrice
			SET
			
			VendorCD= '0000000000000',
			StoreCD=	'0000',
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU4 as ts on mj.AdminNO=ts.AdminNO
			where mj.ChangeDate=ts.ChangeDate
			and mj.VendorCD='0000000000000'
		drop table #tmpSKU4




		End

	if @type =5
	Begin
		 Create table [#tmpSKU5](
	[AdminNO] [int] NOT NULL,
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

	declare @DocHandle5 int

	exec sp_xml_preparedocument @DocHandle5 output, @xml
	INSERT Into #tmpSKU5
	select *  FROM OPENXML (@DocHandle5, '/NewDataSet/test',2)
	with
	(
	AdminNO int,
	ChangeDate date,
	ApprovalDate date ,
	SKUCD varchar(40) ,
	JanCD varchar(13),
	DeleteFlg tinyint ,
	SKUName varchar(100),
	ITEMCD varchar(30),
	SizeNO int,
	ColorNO int,
	SizeName varchar(20) ,
	ColorName varchar(20) ,
	LastYearTerm varchar(6) ,
	LastSeason varchar(6) ,
	LastCatalogNO varchar(20) ,
	LastCatalogPage varchar(20) ,
	LastCatalogNOLong varchar(2000) ,
	LastCatalogPageLong varchar(2000) ,
	LastCatalogText varchar(1000),
	LastInstructionsNO varchar(1000),
	LastInstructionsDate date
	)
	exec sp_xml_removedocument @DocHandle5;

	
	
	Update M_SKU
	Set		
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU5 as ts on ts.AdminNO =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate
		

		
		Insert into M_SKUInfo
		Select 
		AdminNO,
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
		From #tmpSKU5 as ts
		Where not Exists(
					Select  AdminNO
					from M_SKUInfo as mi
					where mi.AdminNO=ts.AdminNO
					and mi.ChangeDate= ts.ChangeDate
					and mi.SEQ=1
				)
		
		Update M_SKUInfo
		Set
		AdminNO=ts.AdminNO,
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
		inner join #tmpSKU5 as ts on ts.AdminNO=mI.AdminNO
		where mI.AdminNO= ts.AdminNO
		and mI.ChangeDate=ts.ChangeDate
		and mI.SEQ= 1
	drop table #tmpSKU5
	End  

	if @type=6
	Begin 
	  Create table [#tmpSKU6](
	[AdminNO] [int] NOT NULL,
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

	declare @DocHandle6 int

	exec sp_xml_preparedocument @DocHandle6 output, @xml
	INSERT Into #tmpSKU6
	select *  FROM OPENXML (@DocHandle6, '/NewDataSet/test',2)
	with
	(
	AdminNO int,
	ChangeDate date,
	ApprovalDate date ,
	SKUCD varchar(40) ,
	JanCD varchar(13),
	DeleteFlg tinyint ,
	SKUName varchar(100),
	ITEM varchar(30),
	SizeNO int,
	ColorNO int,
	SizeName varchar(20) ,
	ColorName varchar(20) ,
	TagName1  varchar(6) ,
	TagName2  varchar(6) ,
	TagName3  varchar(20) ,
	TagName4 varchar(20) ,
	TagName5  varchar(20) ,
	TagName6  varchar(20) ,
	TagName7  varchar(20),
	TagName8 varchar(20),
	TagName9 varchar(20),
	TagName10 varchar(20)
	)
	exec sp_xml_removedocument @DocHandle6;

	
	
	Update M_SKU
	Set		
			AdminNO=ts.AdminNO,
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
			inner join #tmpSKU6 as ts on ts.AdminNO =ms.AdminNO
			where ms.ChangeDate=ts.ChangeDate

		Delete mtag
		from M_SKUTag as mtag
		inner join #tmpSKU6 as ts on ts.AdminNO=mtag.AdminNO
		Where mtag.AdminNO=ts.AdminNo
		and mtag.ChangeDate=ts.ChangeDate
		Insert 
		Into M_SKUTag
		SELECT AdminNO,ChangeDate,ROW_NUMBER() OVER (ORDER BY AdminNO),ColumnValue from #tmpSKU6 as ts
		 Unpivot(ColumnValue For ColumnName IN (TagName1,TagName2,TagName3,TagName4,TagName5,TagName6,TagName7,TagName8,TagName9,TagName10)) AS H

	drop table #tmpSKU6
	End

	if @type = 7
  begin
	CREATE TABLE [dbo].[#tmpSKU7](
	[AdminNO] [int] NOT NULL,
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
	[APIKey] [varchar](20) NULL,
	[ShouhinCD] [varchar](20) NULL,
	)

	declare @DocHandle7 int

	exec sp_xml_preparedocument @DocHandle7 output, @xml
	insert into #tmpSKU7
	select *  FROM OPENXML (@DocHandle7, '/NewDataSet/test',2)
	with
	(
	AdminNO int,
	ChangeDate  date,
	ApprovalDate date,
	SKUCD varchar(40),
	JANCD varchar(13),
	DeleteFlg tinyint,
	SKUName varchar(100),
	KanaName varchar(50),
	SKUShortName varchar(40),
	EnglishName varchar(80),
	ITEMCD varchar(30),
	SizeNO int,
	ColorNO int,
	SizeName varchar(20),
	ColorName varchar(20),
	APIKey varchar(20),
	ShouhinCD varchar(20)
	)
exec sp_xml_removedocument @DocHandle7;

		Update	M_SKU
			SET	SKUName=ts.SKUName,
				SKUNameLong=ts.SKUName,
				KanaName=ts.KanaName,
				EnglishName=ts.EnglishName,
				SKUShortName=IsNull(ts.SKUShortName,SUBSTRING(ts.SKUName,0,20) ),
				JanCD=ts.JanCD,
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
		inner join #tmpSKU7  as ts on ts.AdminNO =ms.AdminNO
		where ms.ChangeDate=ts.ChangeDate

	drop table #tmpSKU7

	end

	if @type =8
		Begin
			CREATE TABLE [dbo].[#tmpSKU8](
						[AdminNO] [int] NOT NULL,
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

			declare @DocHandle8 int

				exec sp_xml_preparedocument @DocHandle8 output, @xml
				insert into #tmpSKU8
				select *  FROM OPENXML (@DocHandle8, '/NewDataSet/test',2)
				with
				(
				AdminNO int,
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
		inner join #tmpSKU8  as ts on ts.AdminNO =ms.AdminNO
		where ms.ChangeDate=ts.ChangeDate


	
				Insert into M_Site
				Select 
				ts.AdminNO,
				ts.APIKey,
				ts.ShouhinCD,
				ts.ShouhinCD + '.html',
				@OperatorCD,
				getdate(),
				@OperatorCD,
				getdate()
				from #tmpSKU8 as ts
				where not Exists(
								Select AdminNO
								from M_Site as ms
								inner join M_API as mp on mp.APIKey=ms.APIKey
								where ms.AdminNO=ts.AdminNO
								and ms.APIKey=ts.APIKey
							)
			

				Update M_Site
				Set 
				AdminNO=ts.AdminNO,
				APIKey=ts.APIKey,
				ShouhinCD=ts.ShouhinCD,
				SiteURL=mp.ShopURL +ts.ShouhinCD + '.html',
				InsertOperator=@OperatorCD,
				InsertDateTime=getdate(),
				UpdateOperator=@OperatorCD,
				UpdateDateTime=getdate()

				from M_Site as ms
				inner join #tmpSKU8 as ts on ts.AdminNO=ms.AdminNO
				inner join M_API as mp on mp.APIKey=ms.APIKey


			drop table #tmpSKU8
	End
	
	  exec dbo.L_Log_Insert @OperatorCD,@ProgramID,@PC,Null,@KeyItem
	
END
