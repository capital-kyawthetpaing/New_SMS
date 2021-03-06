 BEGIN TRY 
 Drop Procedure [dbo].[M_TenzikaiShouhin_Select]
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
CREATE PROCEDURE [dbo].[M_TenzikaiShouhin_Select]
	-- Add the parameters for the stored procedure here
		@TankaCD as Varchar(13),
		@BrandCD as VarChar(6),
		@SegmentCD as VarChar(6),
		@LastYearTerm as VarChar(6),
		@LastSeason as VarChar(6),
		@PriceOutTaxF as Varchar(10),
		@PriceOutTaxT as VarChar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @LastYearTerm ='-1'
		Set @LastYearTerm= null

	IF @LastSeason ='-1'
		Set @LastSeason=null
    -- Insert statements for procedure here
	select 
		m.BrandCD
		,MAX(mb.BrandName) as BrandName
		,m.SegmentCD
		,MAX(mp.Char1) as SegmentName
		,m.LastYearTerm
		,m.LastSeason
		,MAX(IsNUll(mt.Rate,100)) as Rate
from  M_TenzikaiShouhin as m
left outer join M_Brand as mb on mb.BrandCD=m.BrandCD
left outer join M_Multiporpose as mp on mp.ID='226'
									and mp.[Key]=m.SegmentCD
left outer join M_TenzikaiRate as mt on mt.RankCD=m.TankaCD
									and mt.BrandCD=m.BrandCD
									and mt.SegmentCD=m.SegmentCD
									and mt.LastYearTerm=m.LastYearTerm
									and mt.LastSeason =m.LastSeason

where m.DeleteFlg=0
and m.TankaCD=@TankaCD
and (@BrandCD is NUll or m.BrandCD =@BrandCD)
and (@SegmentCD is NULl or m.SegmentCD=@SegmentCD)
and (@LastYearTerm is NUll or m.LastYearTerm =@LastYearTerm)
and (@LastSeason is NUll or m.LastSeason =@LastSeaSon)
and (@PriceOutTaxF is NUll or m.SalePriceOutTax >=@PriceOutTaxF)
and (@PriceOutTaxT is NULL or m.SalePriceOutTax <=@PriceOutTaxT)

Group By m.BrandCD
			,m.SegmentCD
			,m.LastYearTerm,
			m.LastSeason
Order By  m.BrandCD  Asc
			,m.SegmentCD  Asc
			,m.LastYearTerm  Asc,
			m.LastSeason Asc
END
