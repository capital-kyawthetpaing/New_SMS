 BEGIN TRY 
 Drop Procedure dbo.[Mastertoroku_Tenzikaishouhin_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Mastertoroku_Tenzikaishouhin_Select]
	-- Add the parameters for the stored procedure here
		 @tenzikainame as varchar(80)
		,@vendorcd as varchar(13)
		,@lastyear as varchar(6)
		,@lastseason as varchar(6)
		,@brandcd as varchar(6)
		,@segment as varchar(6)
		,@mode as tinyint
		


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#tmpTenzishouhin]
	(
	[JANCD] [varchar](13)  NULL,
	[SKUCD] [varchar](30) NULL,
	[SKUName] [varchar](100) NULL,
	[ColorNO] [varchar](10) NULL,
	[ColorName] [varchar](20) NULL,
	[SizeNO] [varchar](10) NULL ,
	[SizeName] [varchar](20) NULL,
	[HanbaiYoteiDateMonth] [varchar](2)  NULL ,
	[HanbaiYoteiDate] [varchar](8) NULL,
	[SiireTanka] [money]  NULL ,
	[JoudaiTanka] [money]  NULL ,
	[SalePriceOutTax] [money]  NULL ,
	[TankKa1] [money]  NULL ,
	[TankKa2] [money]  NULL ,
	[TankKa3] [money]  NULL ,
	[TankKa4] [money]  NULL ,
	[TankKa5] [money]  NULL ,
	[BrandCD] [varchar](6) NULL,
	[SegmentCD] [varchar](6) NULL,
	[TaniCD] [varchar](50) NULL,
	[TaxRateFLG] [tinyint] NULL ,
	[Remarks] [varchar](500) NULL,
	[ExhibitionCommonCD] [varchar](30) NULL)
	


	
	if @mode= 1
	Begin
	
		
		insert into #tmpTenzishouhin
		select  
		mt.JANCD,
		MAX(mt.SKUCD) as SKUCD,
		MAX(mt.SKUName) as SKUName,
		MAX(mt.ColorNo) as ColorNo,
		MAX(mt.ColorName) as ColorName,
		MAX(mt.SizeNo) as SizeNO,
		MAX(mt.SizeName) as SizeName,
		MAX(mt.HanbaiYoteiDateMonth) as HanbaiYoteiDateMonth,
		MAX(mt.HanbaiYoteiDate) as HanbaiYoteiDate,
		MAX(mt.SiireTanka) as SiireTanka,
		MAX(mt.JoudaiTanka) as JoudaiTanka,
		MAX(Case	When mt.TankaCD='0' Then mt.SalePriceOutTax Else 0 End) as SalePriceOutTax,
		MAX(Case	When mt.TankaCD='1' Then mt.SalePriceOutTax Else 0 End) as TankKa1,
		MAX(Case	When mt.TankaCD='2' Then mt.SalePriceOutTax Else 0 End) as TankKa2,
		MAX(Case	When mt.TankaCD='3' Then mt.SalePriceOutTax Else 0 End)	as TankKa3,
		MAX(Case	When mt.TankaCD='4' Then mt.SalePriceOutTax Else 0 End)	as TankKa4,
		MAX(Case	When mt.TankaCD='5' Then mt.SalePriceOutTax Else 0 End)	as TankKa5, 		
		MAX(mt.BrandCD) as BrandCD,
		MAX(mt.SegmentCD) as SegmentCD,
		MAX(mt.TaniCD) as TaniCD,
		MAX(mt.TaxRateFlG) as TaxRateFlg,
		--MAX(Case 
		--		When mt.TaxRateFlG=0 Then N'非課税'
		--		When mt.TaxRateFlG=1 Then N'通常課税'
		--		Else N'軽減課税'
		--	End) as TaxRateFlg,
		MAX(mt.Remarks)as Remarks,
		
		NUll as ExhibitionCommonCD

	--into #tmpTenzishouhin
	from M_TenzikaiShouhin as mt
	left outer join M_Brand as mb on mb.BrandCD=mt.BrandCD
	left outer join M_Multiporpose as mm on mm.ID='226'
										and [Key]=mt.SegmentCD
	
	where mt.TenzikaiName =@tenzikainame
	and mt.VendorCD=@vendorcd
	and mt.LastYearTerm =@lastyear
	and mt.LastSeason=@lastseason
	and (@brandcd is Null or mt.BrandCD =@brandcd)
	and (@segment is NUll or mt.SegmentCD=@segment)
	and mt.DeleteFlg='0'
	Group By mt.JanCD
	End
	
	if @mode =2
	Begin 

	insert into #tmpTenzishouhin
		select  
		mt.JANCD,
		MAX(mt.SKUCD) as SKUCD,
		MAX(mt.SKUName) as SKUName,
		MAX(mt.ColorNo) as ColorNo,
		MAX(mt.ColorName) as ColorName,
		MAX(mt.SizeNo) as SizeNO,
		MAX(mt.SizeName) as SizeName,
		MAX(mt.HanbaiYoteiDateMonth) as HanbaiYoteiDateMonth,
		MAX(mt.HanbaiYoteiDate) as HanbaiYoteiDate,
		MAX(mt.SiireTanka) as SiireTanka,
		MAX(mt.JoudaiTanka) as JoudaiTanka,
		MAX(Case	When mt.TankaCD='0' Then mt.SalePriceOutTax Else 0 End) as SalePriceOutTax,
		MAX(Case	When mt.TankaCD='1' Then mt.SalePriceOutTax Else 0 End) as TankKa1,
		MAX(Case	When mt.TankaCD='2' Then mt.SalePriceOutTax Else 0 End) as TankKa2,
		MAX(Case	When mt.TankaCD='3' Then mt.SalePriceOutTax Else 0 End)	as TankKa3,
		MAX(Case	When mt.TankaCD='4' Then mt.SalePriceOutTax Else 0 End)	as TankKa4,
		MAX(Case	When mt.TankaCD='5' Then mt.SalePriceOutTax Else 0 End)	as TankKa5, 		
		MAX(mt.BrandCD) as BrandCD,
		MAX(mt.SegmentCD) as SegmentCD,
		MAX(mt.TaniCD) as TaniCD,
		MAX(mt.TaxRateFlG) as TaxRateFlg,
		--MAX(Case 
		--		When mt.TaxRateFlG=0 Then N'非課税'
		--		When mt.TaxRateFlG=1 Then N'通常課税'
		--		Else N'軽減課税'
		--	End) as TaxRateFlg,
		MAX(mt.Remarks)as Remarks,
		
		MAX(mt.ExhibitionCommonCD) as ExhibitionCommonCD

	--into #tmpTenzishouhin
	from M_TenzikaiShouhin as mt
	left outer join M_Brand as mb on mb.BrandCD=mt.BrandCD
	left outer join M_Multiporpose as mm on mm.ID='226'
										and [Key]=mt.SegmentCD
	
	where mt.TenzikaiName =@tenzikainame
	and mt.VendorCD=@vendorcd
	and mt.LastYearTerm =@lastyear
	and mt.LastSeason=@lastseason
	and (@brandcd is Null or mt.BrandCD =@brandcd)
	and (@segment is NUll or mt.SegmentCD=@segment)
	and mt.DeleteFlg='0'
	Group By mt.JanCD

	End

	Select

		JANCD,
		SKUCD,
		SKUName as 商品名,
		ColorNo as カラーNO,
		ColorName as カラー名,
		SizeNO as サイズNO,
		SizeName as サイズ名,
		HanbaiYoteiDateMonth as '販売予定日(月)',
		HanbaiYoteiDate as 販売予定日,
		IsNull(SiireTanka,'0') as 仕入単価,
		IsNUll(JoudaiTanka,'0') as 上代単価,
		SalePriceOutTax as 標準売上単価,
		TankKa1  as ランク１単価,
		TankKa2  as ランク２単価,
		TankKa3  as ランク３単価,
		TankKa4  as ランク４単価,
		TankKa5  as ランク５単価, 		
		BrandCD		as ブランドCD,
		SegmentCD	as セグメントCD,
		TaniCD		as 単位CD,
		(Case 
			When TaxRateFlG=0 Then N'非課税'
			When TaxRateFlG=1 Then N'通常課税'
			Else N'軽減課税'
		End) as TaxRateFlg,
		TaxRateFlg as 税率区分,
		Remarks as 備考,
		ExhibitionCommonCD

		from #tmpTenzishouhin
		order by JanCD asc
		--,SKUCD asc
	


		

drop Table #tmpTenzishouhin
END
