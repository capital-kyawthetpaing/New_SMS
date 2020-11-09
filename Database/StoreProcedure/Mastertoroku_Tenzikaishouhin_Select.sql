 BEGIN TRY 
 Drop Procedure dbo.[Mastertoroku_Tenzikaishouhin_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Mastertoroku_Tenzikaishouhin_Select]
	-- Add the parameters for the stored procedure here
		 @tenzikainame as varchar(80)
		,@vendorcd as varchar(13)
		,@lastyear as varchar(6)
		,@lastseason as varchar(6)
		,@brandcd as varchar(6)
		,@segment as varchar(6)
		


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
	[ColorNO] [int] NULL,
	[ColorName] [varchar](20) NULL,
	[SizeNO] [int] NULL ,
	[SizeName] [varchar](20) NULL,
	[HanbaiYoteiDateMonth] [int]  NULL ,
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
		MAX(Case 
				When mt.TaxRateFlG=0 Then N''
				When mt.TaxRateFlG=1 Then N''
				Else N''
			End) as TaxRateFlg,
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
		TaxRateFlg as 税率区分,
		Remarks as 備考,
		ExhibitionCommonCD

		from #tmpTenzishouhin
		order by JanCD asc
		--,SKUCD asc
	

		

drop Table #tmpTenzishouhin
END
