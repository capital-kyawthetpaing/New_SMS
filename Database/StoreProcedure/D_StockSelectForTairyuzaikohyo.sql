 BEGIN TRY 
 Drop Procedure dbo.[D_StockSelectForTairyuzaikohyo]
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
CREATE PROCEDURE [dbo].[D_StockSelectForTairyuzaikohyo]
	-- Add the parameters for the stored procedure here
	@AdminNO int,
	@SoukoCD varchar(6),
	@RackNOFrom varchar(11),
	@RackNOTo varchar(11),
	@MainVendorCD varchar(13),
	@BrandCD varchar(6),
	@SKUName varchar(100),
	@JanCD varchar(13),
	@SKUCD varchar(30),
	@YearTerm varchar(6),
	@Season varchar(6),
	@CatalogNO varchar(20),
	@InsturctionsNO varchar(1000),
	@ItemCD varchar(30),
	@MakerItem varchar(50),
	@SportsCD varchar(6),
	@TagName1 varchar(20),
	@TagName2 varchar(20),
	@TagName3 varchar(20),
	@TagName4 varchar(20),
	@TagName5 varchar(20),
	@ReserveCD varchar(3),
	@NoticesCD varchar(3),
	@PostageCD varchar(3),
	@OrderAttentionCD varchar(3),
	--@InsertDateTime datetime,
	--@UpdateDateTime datetime,
	--@ApprovalDate1 date,
	--@ApprovalDate2 date,
	@keyword1 as varchar(80),
	@keyword2 as varchar(80),
	@keyword3 as varchar(80),
	@Type as tinyint,
	@checkFlag as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#Tmp_D_StockSelect](
	[SoukoCD][varchar](6) collate Japanese_CI_AS,
	[AdminNO][int],
	[SKUCD][varchar](30) collate Japanese_CI_AS,
	[JanCD][varchar](13) collate Japanese_CI_AS,
	[SKUName][varchar](100) collate Japanese_CI_AS,
	[ColorName][varchar](20) collate Japanese_CI_AS,
	[SizeName][varchar](20) collate Japanese_CI_AS,
	[BrandName][varchar](40) collate Japanese_CI_AS,
	[Char1][varchar](100) collate Japanese_CI_AS,
	[StockSu][int],
	[ArrivalDate][date],
	[ShippingDate][date]
	--[DaysCalculation][int]
	)

	if @Type=1
	begin
		insert into #Tmp_D_StockSelect
	select
	Max(ds.SoukoCD) as SoukoCD,
	Max(ds.AdminNO) as AdminNO,
	Max(ds.SKUCD) as SKUCD,
	Max(ds.JanCD) as JanCD,
	Max(msku.SKUName) as SKUName,
	Max(msku.ColorName) as ColorName,
	Max(msku.SizeName)as SizeName,
	Max(mb.BrandName) as BrandName,
	(Select Char1 From M_MultiPorpose mmp where mmp.ID = '202' and mmp.[Key] = max(msku.SportsCD)) as Char1,
	sum(ds.StockSu) as StockSu,
	Max(ds.ArrivalDate) as ArrivalDate,
	case
	when max(dship.ShippingDate) is null then max(ds.ArrivalDate)
	else Max(dship.ShippingDate)
	end as ShippingDate
	--DATEDIFF(day,max(ShippingDate),Convert(date,getdate())) as DaysCalculation
	From D_Stock ds
	Left Outer Join F_SKU(getdate()) msku on msku.AdminNO = ds.AdminNO 
	Left Outer Join F_Souko(getdate()) msk on msk.SoukoCD = ds.SoukoCD
	Left Outer Join F_SKUInfo(getdate(),0) finfo on finfo.AdminNO = ds.AdminNO
	Left Outer Join F_SKUTag(getdate(),0) ftag on ftag.AdminNO = ds.AdminNO
	Left Outer Join M_Brand mb on mb.BrandCD = msku.BrandCD
	Left Outer Join D_Reserve dr on dr.StockNO = ds.StockNO
	Left Outer Join D_InstructionDetails did on did.ReserveNO = dr.ReserveNO
	Left Outer Join D_Instruction di on di.InstructionNO = did.InstructionNO 
	Left Outer Join D_Shipping dship on dship.InstructionNO = di.InstructionNO
	
	Where (@AdminNO is null or ds.AdminNO = @AdminNO )
	and (@SoukoCD is null or  ds.SoukoCD = @SoukoCD) 
	and (@RackNOFrom is null or ds.RackNO >= @RackNOFrom) 
	and (@RackNOTo is null or ds.RackNO <= @RackNOTo)
	and (@MainVendorCD is null or msku.MainVendorCD = @MainVendorCD)
	and (@BrandCD is null or msku.BrandCD = @BrandCD)
	and (@SKUName is null or msku.SKUName like @SKUName) 
	and(@JanCD is null or  msku.JanCD = @JanCD) 
	and (@SKUCD is null or msku.SKUCD = @SKUCD) 
	
	and (@ItemCD is null or (@checkFlag=1 and @ItemCD is not null and Exists(select * from dbo.SplitString(@ItemCD,',')as P where msku.ITemCD LIKE '%'+ p.Item+ '%') ))
	and (@MakerItem is null or (@checkFlag=2 and @MakerItem is not null and Exists(select * from dbo.SplitString(@MakerItem,',')as P where msku.MakerItem LIKE '%'+ p.Item+ '%') ))

	and (@YearTerm is null or finfo.YearTerm = @YearTerm)
	and (@Season is null or finfo.Season = @Season)
	and (@CatalogNO is null or finfo.CatalogNO = @CatalogNO)
	and (@InsturctionsNO is null or finfo.InstructionsNO = @InsturctionsNO)
	
	and(@SportsCD is null or msku.SportsCD = @SportsCD)
	and((@TagName1 is null or ftag.TagName = @TagName1) 
	or (@TagName2 is null or ftag.TagName = @TagName2) 
	or (@TagName3 is null or ftag.TagName = @TagName3) 
	or (@TagName4 is null or ftag.TagName = @TagName4) 
	or (@TagName5 is null or ftag.TagName = @TagName5))
	and(@ReserveCD is null or msku.ReserveCD = @ReserveCD )
	and(@NoticesCD is null or  msku.NoticesCD = @NoticesCD) 
	and (@PostageCD is null or msku.PostageCD = @PostageCD )
	and (@OrderAttentionCD is null or msku.OrderAttentionCD = @OrderAttentionCD) 
	and((@keyword1 is null 
	and @keyword2 is null 
	and @keyword3 is null)
	or ((msku.CommentInStore like '%' + @keyword1 + '%')
	or (msku.CommentInStore like '%' + @keyword2 + '%')
	or (msku.CommentInStore like '%' + @keyword3 + '%')))
	and ds.ArrivalYetFLG = 0 
	and ds.StockSu > 0
	Group by ds.AdminNO
	end

	else if @Type=2
	begin
	insert into #Tmp_D_StockSelect
	select
	Max(ds.SoukoCD) as SoukoCD,
	Max(ds.AdminNO) as AdminNO,
	Max(ds.SKUCD) as SKUCD,
	Max(ds.JanCD) as JanCD,
	Max(msku.SKUName) as SKUName,
	Max(msku.ColorName) as ColorName,
	Max(msku.SizeName)as SizeName,
	Max(mb.BrandName) as BrandName,
	(Select Char1 From M_MultiPorpose mmp where mmp.ID = '202' and mmp.[Key] = max(msku.SportsCD)) as Char1,
	sum(ds.StockSu) as StockSu,
	Max(ds.ArrivalDate) as ArrivalDate,
	case
	when max(dship.ShippingDate) is null then max(ds.ArrivalDate)
	else Max(dship.ShippingDate)
	end as ShippingDate
	--DATEDIFF(day,max(ShippingDate),Convert(date,getdate())) as DaysCalculation
	From D_Stock ds
	Left Outer Join F_SKU(getdate()) msku on msku.AdminNO = ds.AdminNO 
	Left Outer Join F_Souko(getdate()) msk on msk.SoukoCD = ds.SoukoCD
	Left Outer Join F_SKUInfo(getdate(),0) finfo on finfo.AdminNO = ds.AdminNO
	Left Outer Join F_SKUTag(getdate(),0) ftag on ftag.AdminNO = ds.AdminNO
	Left Outer Join D_Reserve dr on dr.StockNO = ds.StockNO
	Left Outer Join D_InstructionDetails did on did.ReserveNO = dr.ReserveNO
	Left Outer Join D_Instruction di on di.InstructionNO = did.InstructionNO 
	Left Outer Join D_Shipping dship on dship.InstructionNO = di.InstructionNO
	Left Outer Join M_Brand mb on mb.BrandCD = msku.BrandCD
	Where(@AdminNO is null or ds.AdminNO = @AdminNO )
	and (@SoukoCD is null or  ds.SoukoCD = @SoukoCD) 
	and (@RackNOFrom is null or ds.RackNO >= @RackNOFrom) 
	and (@RackNOTo is null or ds.RackNO <= @RackNOTo)
	and (@MainVendorCD is null or msku.MainVendorCD = @MainVendorCD)
	and (@BrandCD is null or msku.BrandCD = @BrandCD)
	and (@SKUName is null or msku.SKUName like @SKUName) 
	and(@JanCD is null or  msku.JanCD = @JanCD) 
	and (@SKUCD is null or msku.SKUCD = @SKUCD) 
	
	and (@YearTerm is null or finfo.YearTerm = @YearTerm)
	and (@Season is null or finfo.Season = @Season)
	and (@CatalogNO is null or finfo.CatalogNO = @CatalogNO)
	and (@InsturctionsNO is null or finfo.InstructionsNO = @InsturctionsNO)

	--and (@ItemCD is null or (@checkFlag=1 and @ItemCD is not null and msku.ITemCD LIKE '%' + (select * from SplitString(@ItemCD,',')) + '%') )
	--and (@MakerItem is null or (@checkflag=2 and @MakerItem is not null and msku.ITemCD LIKE '%' + (select * from SplitString(@MakerItem,',')) + '%'))
	
	and (@ItemCD is null or (@checkFlag=1 and @ItemCD is not null and Exists(select * from dbo.SplitString(@ItemCD,',')as P where msku.ITemCD LIKE '%'+ p.Item+ '%') ))
	and (@MakerItem is null or (@checkFlag=2 and @MakerItem is not null and Exists(select * from dbo.SplitString(@MakerItem,',')as P where msku.MakerItem LIKE '%'+ p.Item+ '%') ))

	and(@SportsCD is null or msku.SportsCD = @SportsCD)
	and((@TagName1 is null or ftag.TagName = @TagName1) 
	or (@TagName2 is null or ftag.TagName = @TagName2) 
	or (@TagName3 is null or ftag.TagName = @TagName3) 
	or (@TagName4 is null or ftag.TagName = @TagName4) 
	or (@TagName5 is null or ftag.TagName = @TagName5))
	and(@ReserveCD is null or msku.ReserveCD = @ReserveCD )
	and(@NoticesCD is null or  msku.NoticesCD = @NoticesCD) 
	and (@PostageCD is null or msku.PostageCD = @PostageCD )
	and (@OrderAttentionCD is null or msku.OrderAttentionCD = @OrderAttentionCD) 
	and (@keyword1 is null or (msku.CommentInStore like '%' + @keyword1 + '%'))
	and (@keyword2 is null or (msku.CommentInStore like '%' + @keyword2 + '%'))
	and (@keyword3 is null or (msku.CommentInStore like '%' + @keyword3 + '%'))
	and ds.ArrivalYetFLG = 0 
	and ds.StockSu > 0
	--and dr.StockNO = ds.StockNO 
	--and did.ReserveNO = dr.ReserveNO
	--and di.InstructionNO = did.InstructionNO
	--and dship.InstructionNO = di.InstructionNO
	--Group by ds.SoukoCD,ds.AdminNO,ds.SKUCD,ds.JanCD,msku.SKUName,msku.ColorName,msku.SizeName,ds.ArrivalDate,dship.ShippingDate,mb.BrandName,msku.SportsCD 
	Group by ds.AdminNO
	end
	
	select DATEDIFF(day,tds.ShippingDate,Convert(date,getdate())) DaysCalculation,
	tds.SKUCD,
	tds.JanCD,
	tds.SKUName,
	tds.ColorName,
	tds.SizeName,
	tds.BrandName,
	tds.Char1,
	convert(varchar,tds.ArrivalDate,120) as ArrivalDate,
	convert(varchar,tds.ShippingDate,120) as ShippingDate,
	tds.StockSu
	from #Tmp_D_StockSelect tds
	where (@AdminNO is null or DATEDIFF(day,tds.ShippingDate,Convert(date,getdate())) >= @AdminNO)
	drop table #Tmp_D_StockSelect
	
END

