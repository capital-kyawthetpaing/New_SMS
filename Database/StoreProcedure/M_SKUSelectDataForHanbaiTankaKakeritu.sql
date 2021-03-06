 BEGIN TRY 
 Drop Procedure dbo.[M_SKUSelectDataForHanbaiTankaKakeritu]
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
Create PROCEDURE [dbo].[M_SKUSelectDataForHanbaiTankaKakeritu]
@StartDate varchar(10),
@EndDate varchar(10),
@DateCopy varchar(10),
@TankaCD varchar(13),
@TankaName varchar(20),
@TankaCDCopy varchar(13),
@BrandCD varchar(6),
@BrandCDCopy varchar(6),
@ExhibitionSegmentCD varchar(5),
@ExhibitionSegmentCDCopy varchar(5),
@LastYearTerm  varchar(6),
@LastYearTermCopy varchar(6),
@LastSeason varchar(6),
@LastSeasonCopy varchar(6),
@PriceOutTaxFrom money,
@PriceOutTaxTo money,
@Option varchar(1)
AS
BEGIN

If @LastYearTerm='-1'
	set @LastYearTerm=null

 If @LastYearTermCopy='-1'
	set @LastYearTermCopy=null

If @LastSeason='-1'
	set @LastSeason=null

If @LastSeasonCopy='-1'
	set @LastSeasonCopy=null
 
	 IF @Option='1'
	 Begin
		select @TankaCD as TankaCD,
		@TankaName as TankaName,
		fs.BrandCD,
		Max(mb.BrandName) as BrandName,
		fs.ExhibitionSegmentCD,
		Max(mmp.Char1) as ExhibitionSegmentName,
		fs.LastYearTerm,
		fs.LastSeason,
		@StartDate as StartDate,
		@EndDate as EndDate,
		'100' as Rate

		from F_SKU(@StartDate) as fs
		left outer join M_Brand as mb on fs.BrandCD=mb.BrandCD and fs.DeleteFlg=0
		left outer join M_Multiporpose as mmp on mmp.ID='226' 
												and mmp.[KEY]=fs.ExhibitionSegmentCD
		where (@BrandCD is null or fs.BrandCD=@BrandCD)
		and (@ExhibitionSegmentCD is null or fs.ExhibitionSegmentCD=@ExhibitionSegmentCD)
		and (@LastYearTerm is null or fs.LastYearTerm=@LastYearTerm)
		and (@LastSeason is null or fs.LastSeason=@LastSeason)
		and (@PriceOutTaxFrom is null or fs.PriceOutTax<=@PriceOutTaxFrom)
		and (@PriceOutTaxTo is null or fs.PriceOutTax>=@PriceOutTaxTo)
		and NOT Exists(Select * 
						from M_SKUPrice as msp
						where msp.DeleteFlg=0
						and msp.TankaCD=@TankaCD
						and msp.StoreCD='0000'
						and msp.AdminNO=fs.AdminNO
						and msp.ChangeDate=@StartDate)

		Group By fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason
		Order by fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason

	End
	Else IF @Option='2'
	Begin


		CREATE TABLE [dbo].[#tempSalesRate]
		(   
			RankCD varchar(13),
			BrandCD varchar(6),
			SegmentCD	varchar(5),
			LastYearTerm varchar(6),
			LastSeason	varchar(6),
			ApplicationStartDate varchar(10),
			Rate	varchar(10)
		
		
		)
		Insert into #tempSalesRate
		select RankCD,
		BrandCD,
		SegmentCD,
		LastYearTerm,
		LastSeason,
		ApplicationStartDate,
		isnull(Max(Rate),0.00) as Rate
		--Into #tempSaleRate
		from M_SalesRate
		where ApplicationStartDate=@StartDate
		and RankCD=@TankaCD
		and (@BrandCDCopy is null or BrandCD=@BrandCDCopy)
		and (@ExhibitionSegmentCDCopy is null or SegmentCD=@ExhibitionSegmentCDCopy)
		and (@LastYearTermCopy is null or LastYearTerm=@LastYearTermCopy)
		and (@LastSeasonCopy is null or LastSeason=@LastSeasonCopy)
		Group by ApplicationStartDate,RankCD,BrandCD,SegmentCD,LastYearTerm,LastSeason


		select @TankaCD as TankaCD,
		@TankaName as TankaName,
		fs.BrandCD,
		Max(mb.BrandName) as BrandName,
		fs.ExhibitionSegmentCD,
		Max(mmp.Char1) as ExhibitionSegmentName,
		fs.LastYearTerm,
		fs.LastSeason,
		@StartDate as StartDate,
		@EndDate as EndDate,
		tsr.Rate

		from F_SKU(@StartDate) as fs
		left outer join M_Brand as mb on fs.BrandCD=mb.BrandCD and fs.DeleteFlg=0
		left outer join M_Multiporpose as mmp on mmp.ID='226' 
												and mmp.[KEY]=fs.ExhibitionSegmentCD
		left outer join #tempSalesRate as tsr on tsr.BrandCD=fs.BrandCD
												and tsr.SegmentCd=fs.ExhibitionSegmentCD
												and tsr.LastYearTerm=fs.LastYearTerm
												and tsr.LastSeason=fs.LastSeason
		where (@BrandCD is null or fs.BrandCD=@BrandCD)
		and (@ExhibitionSegmentCD is null or fs.ExhibitionSegmentCD=@ExhibitionSegmentCD)
		and (@LastYearTerm is null or fs.LastYearTerm=@LastYearTerm)
		and (@LastSeason is null or fs.LastSeason=@LastSeason)
		and (@PriceOutTaxFrom is null or fs.PriceOutTax<=@PriceOutTaxFrom)
		and (@PriceOutTaxTo is null or fs.PriceOutTax>=@PriceOutTaxTo)
		and Exists(select *
					from M_SalesRate
					Where ApplicationStartDate=@StartDate
						and RankCD=@TankaCD
						and (@BrandCDCopy is null or BrandCD=@BrandCDCopy)
						and (@ExhibitionSegmentCDCopy is null or ExhibitionSegmentCD=@ExhibitionSegmentCDCopy)
						and (@LastYearTermCopy is null or fs.LastYearTerm=@LastYearTermCopy)
						and (@LastSeasonCopy is null or fs.LastSeason=@LastSeasonCopy)
						Group by ApplicationStartDate,RankCD,BrandCD,SegmentCD,LastYearTerm,LastSeason )
		and NOT Exists(Select * 
						from M_SKUPrice as msp
						where msp.DeleteFlg=0
						and msp.TankaCD=@TankaCD
						and msp.StoreCD='0000'
						and msp.AdminNO=fs.AdminNO
						and msp.ChangeDate=@StartDate)

		Group By fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason,tsr.Rate
		Order by fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason

		Drop table #tempSalesRate
		
	End

	Else If @Option='3'
	Begin

	CREATE TABLE [dbo].[#tempSalesRate2]
		(   
			RankCD varchar(13),
			BrandCD varchar(6),
			SegmentCD	varchar(5),
			LastYearTerm varchar(6),
			LastSeason	varchar(6),
			ApplicationStartDate varchar(10),
			Rate	varchar(10)
		
		)

		Insert into #tempSalesRate2
		select RankCD,
		BrandCD,
		SegmentCD,
		LastYearTerm,
		LastSeason,
		ApplicationStartDate,
		Isnull(Max(Rate),0.00) as Rate
		--Into #tempSaleRate2
		from M_SalesRate
		where ApplicationStartDate=@StartDate
		and RankCD=@TankaCD
		and (@BrandCDCopy is null or BrandCD=@BrandCD)
		and (@ExhibitionSegmentCDCopy is null or SegmentCD=@ExhibitionSegmentCD)
		and (@LastYearTermCopy is null or LastYearTerm=@LastYearTerm)
		and (@LastSeasonCopy is null or LastSeason=@LastSeason)
		Group by ApplicationStartDate,RankCD,BrandCD,SegmentCD,LastYearTerm,LastSeason


		select @TankaCD as TankaCD,
		@TankaName as TankaName,
		fs.BrandCD,
		Max(mb.BrandName) as BrandName,
		fs.ExhibitionSegmentCD,
		Max(mmp.Char1) as ExhibitionSegmentName,
		fs.LastYearTerm,
		fs.LastSeason,
		@StartDate as StartDate,
		@EndDate as EndDate,
		tsr.Rate

		from F_SKU(@StartDate) as fs
		left outer join M_Brand as mb on fs.BrandCD=mb.BrandCD and fs.DeleteFlg=0
		left outer join M_Multiporpose as mmp on mmp.ID='226' 
												and mmp.[KEY]=fs.ExhibitionSegmentCD
		left outer join #tempSalesRate2 as tsr on tsr.BrandCD=fs.BrandCD
												and tsr.SegmentCd=fs.ExhibitionSegmentCD
												and tsr.LastYearTerm=fs.LastYearTerm
												and tsr.LastSeason=fs.LastSeason
		where (@BrandCD is null or fs.BrandCD=@BrandCD)
		and (@ExhibitionSegmentCD is null or fs.ExhibitionSegmentCD=@ExhibitionSegmentCD)
		and (@LastYearTerm is null or fs.LastYearTerm=@LastYearTerm)
		and (@LastSeason is null or fs.LastSeason=@LastSeason)
		and (@PriceOutTaxFrom is null or fs.PriceOutTax<=@PriceOutTaxFrom)
		and (@PriceOutTaxTo is null or fs.PriceOutTax>=@PriceOutTaxTo)
		and  Exists(Select * 
						from M_SKUPrice as msp
						where msp.DeleteFlg=0
						and msp.TankaCD=@TankaCD
						and msp.StoreCD='0000'
						and msp.AdminNO=fs.AdminNO
						and msp.ChangeDate=@StartDate)

		Group By fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason,tsr.Rate
		Order by fs.BrandCd,fs.ExhibitionSegmentCD,fs.LastYearTerm,fs.LastSeason


		Drop table #tempSalesRate2
		
	End

END