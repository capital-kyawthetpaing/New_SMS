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
	@RackNO1 varchar(11),
	@RackNO2 varchar(11),
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
	@InsertDateTime datetime,
	@UpdateDateTime datetime,
	@ApprovalDate1 date,
	@ApprovalDate2 date,
	@keyword1 as varchar(80),
	@keyword2 as varchar(80),
	@keyword3 as varchar(80),
	@Type as tinyint
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
	[ShippingDate][date],
	[DaysCalculation][int]
	)
	if @Type=1
	begin
	insert into #Tmp_D_StockSelect
	select 
	ds.SoukoCD,
	ds.AdminNO,
	ds.SKUCD,
	ds.JanCD,
	msku.SKUName,
	msku.ColorName,
	msku.SizeName,
	mb.BrandName,
	Char1 = (Select Char1 From M_MultiPorpose mmp where mmp.ID = '202' and mmp.[Key] = msku.SportsCD),
	sum(ds.StockSu),
	ds.ArrivalDate,
	case
		when dship.ShippingDate is null then ds.ArrivalDate
		else Max(dship.ShippingDate)
		end as ShippingDate,
	DATEDIFF(day,ArrivalDate,Convert(date,getdate())) as DaysCalculation
	From D_Stock ds
	Inner Join F_SKU(getdate()) msku on msku.AdminNO = ds.AdminNO 
	Inner Join F_Souko(getdate()) msk on msk.SoukoCD = ds.SoukoCD
	Inner Join F_SKUInfo(getdate(),0) finfo on finfo.AdminNO = ds.AdminNO
	Inner Join F_SKUTag(getdate(),0) ftag on ftag.AdminNO = ds.AdminNO
	Inner Join D_Reserve dr on dr.StockNO = ds.StockNO
	Inner Join D_InstructionDetails did on did.ReserveNO = dr.ReserveNO
	Inner Join D_Instruction di on di.InstructionNO = did.InstructionNO 
	Inner Join D_Shipping dship on dship.InstructionNO = di.InstructionNO
	Left Outer Join M_Brand mb on mb.BrandCD = msku.BrandCD
	Where ds.AdminNO = @AdminNO 
	and ds.SoukoCD = @SoukoCD 
	and ds.RackNO >= @RackNO1 
	and ds.RackNO <= @RackNO2 
	and msku.MainVendorCD = @MainVendorCD
	and msku.BrandCD = @BrandCD
	and msku.SKUName like @SKUName 
	and msku.JanCD = @JanCD 
	and msku.SKUCD = @SKUCD 
	and msku.ITemCD like @ItemCD 
	and msku.MakerItem like @MakerItem
	and finfo.YearTerm = @YearTerm
	and finfo.Season = @Season
	and finfo.CatalogNO = @CatalogNO
	and finfo.InstructionsNO = @InsturctionsNO
	and msku.ITemCD like @ItemCD 
	and msku.MakerVendorCD like @MakerItem
	and msku.SportsCD = @SportsCD
	and (ftag.TagName = @TagName1 or ftag.TagName = @TagName2 or ftag.TagName = @TagName3 or ftag.TagName = @TagName4 or ftag.TagName = @TagName5)
	and msku.ReserveCD = @ReserveCD 
	and msku.NoticesCD = @NoticesCD 
	and msku.PostageCD = @PostageCD 
	and msku.OrderAttentionCD = @OrderAttentionCD 
	and msku.ApprovalDate = Null
	and((@keyword1 is null and @keyword2 is null and @keyword3 is null)
	or ((msku.CommentInStore like '%' + @keyword1 + '%')
	or (msku.CommentInStore like '%' + @keyword2 + '%')
	or (msku.CommentInStore like '%' + @keyword3 + '%'))
	or ((msku.CommentInStore like '%' + @keyword1 + '%')
	or (msku.CommentInStore like '%' + @keyword2 + '%')
	or (msku.CommentInStore like '%' + @keyword3 + '%')))
	and ds.ArrivalYetFLG = 0 
	and ds.StockSu > 0
	and dr.StockNO = ds.StockNO 
	and did.ReserveNO = dr.ReserveNO
	and di.InstructionNO = did.InstructionNO
	and dship.InstructionNO = di.InstructionNO
	Group by ds.SoukoCD,ds.AdminNO,ds.SKUCD,ds.JanCD,msku.SKUName,msku.ColorName,msku.SizeName,ds.ArrivalDate,dship.ShippingDate,mb.BrandName,msku.SportsCD 
	end

	else if @Type=2
	begin
	insert into #Tmp_D_StockSelect
	select 
	ds.SoukoCD,
	ds.AdminNO,
	ds.SKUCD,
	ds.JanCD,
	msku.SKUName,
	msku.ColorName,
	msku.SizeName,
	mb.BrandName,
	Char1 = (Select Char1 From M_MultiPorpose mmp where mmp.ID = '202' and mmp.[Key] = msku.SportsCD),
	sum(ds.StockSu),
	ds.ArrivalDate,
	case
		when dship.ShippingDate is null then ds.ArrivalDate
		else Max(dship.ShippingDate)
		end as ShippingDate,
	DATEDIFF(day,ArrivalDate,Convert(date,getdate())) as DaysCalculation
	From D_Stock ds
	Inner Join F_SKU(getdate()) msku on msku.AdminNO = ds.AdminNO 
	Inner Join F_Souko(getdate()) msk on msk.SoukoCD = ds.SoukoCD
	Inner Join F_SKUInfo(getdate(),0) finfo on finfo.AdminNO = ds.AdminNO
	Inner Join F_SKUTag(getdate(),0) ftag on ftag.AdminNO = ds.AdminNO
	Inner Join D_Reserve dr on dr.StockNO = ds.StockNO
	Inner Join D_InstructionDetails did on did.ReserveNO = dr.ReserveNO
	Inner Join D_Instruction di on di.InstructionNO = did.InstructionNO 
	Inner Join D_Shipping dship on dship.InstructionNO = di.InstructionNO
	Left Outer Join M_Brand mb on mb.BrandCD = msku.BrandCD
	Where ds.AdminNO = @AdminNO 
	and ds.SoukoCD = @SoukoCD 
	and ds.RackNO >= @RackNO1 
	and ds.RackNO <= @RackNO2 
	and msku.MainVendorCD = @MainVendorCD
	and msku.BrandCD = @BrandCD
	and msku.SKUName like @SKUName 
	and msku.JanCD = @JanCD 
	and msku.SKUCD = @SKUCD 
	and msku.ITemCD like @ItemCD 
	and msku.MakerItem like @MakerItem
	and finfo.YearTerm = @YearTerm
	and finfo.Season = @Season
	and finfo.CatalogNO = @CatalogNO
	and finfo.InstructionsNO = @InsturctionsNO
	and msku.ITemCD like @ItemCD 
	and msku.MakerVendorCD like @MakerItem
	and msku.SportsCD = @SportsCD
	and (ftag.TagName = @TagName1 or ftag.TagName = @TagName2 or ftag.TagName = @TagName3 or ftag.TagName = @TagName4 or ftag.TagName = @TagName5)
	and msku.ReserveCD = @ReserveCD 
	and msku.NoticesCD = @NoticesCD 
	and msku.PostageCD = @PostageCD 
	and msku.OrderAttentionCD = @OrderAttentionCD 
	and msku.ApprovalDate = Null
	and((@keyword1 is null and @keyword2 is null and @keyword3 is null)
	and ((msku.CommentInStore like '%' + @keyword1 + '%')
	and (msku.CommentInStore like '%' + @keyword2 + '%')
	and (msku.CommentInStore like '%' + @keyword3 + '%'))
	and ((msku.CommentInStore like '%' + @keyword1 + '%')
	and (msku.CommentInStore like '%' + @keyword2 + '%')
	and (msku.CommentInStore like '%' + @keyword3 + '%')))
	and ds.ArrivalYetFLG = 0 
	and ds.StockSu > 0
	and dr.StockNO = ds.StockNO 
	and did.ReserveNO = dr.ReserveNO
	and di.InstructionNO = did.InstructionNO
	and dship.InstructionNO = di.InstructionNO
	Group by ds.SoukoCD,ds.AdminNO,ds.SKUCD,ds.JanCD,msku.SKUName,msku.ColorName,msku.SizeName,ds.ArrivalDate,dship.ShippingDate,mb.BrandName,msku.SportsCD 
	end
	
END

