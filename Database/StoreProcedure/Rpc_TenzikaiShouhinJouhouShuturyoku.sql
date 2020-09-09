BEGIN TRY 
 Drop Procedure [dbo].[Rpc_TenzikaiShouhinJouhouShuturyoku]
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
CREATE PROCEDURE [dbo].[Rpc_TenzikaiShouhinJouhouShuturyoku]
@VendorCD as varchar(13),
@LastYearTerm as varchar(6),
@LastSeason as varchar(6),
@BrandCDFrom as varchar(6),
@BrandCDTo as varchar(6),
@SegmentCDFrom as varchar(6),
@SegmentCDTo as varchar(6),
@TenzikaiName as varchar(80),
@JANCD as varchar(13),
@HanbaiYoteiDateMonth as int,
@HanbaiYoteiDate as varchar(8)
AS
BEGIN
	Select 
	mt.JANCD AS 仮JANCD,
	mt.SKUCD AS SKUCD,
	mt.SKUName AS 商品名,
	mt.SizeName AS サイズ,
	mt.Colorname AS カラー,
	mt.HanbaiYoteiDate AS 販売予定日,
	Concat(ISNULL(@HanbaiYoteiDateMonth, '')  , N'月' , ISNULL(@HanbaiYoteiDate, '') )as  '販売予定日',
	'' AS 即納数,
	'' AS 希望日1,
	'' AS 希望日2,
	(ISNULL(@LastYearTerm, '') + ISNULL(@LastSeason, '') + ISNULL(@JANCD, ''))As  '消さないでください'
	from M_TenzikaiShouhin  mt
	where  DeleteFlg=0
	AND    mt.VendorCD=@VendorCD
	AND    mt.LastYearTerm=@LastYearTerm
	AND    mt.LastSeason=@LastSeason
	AND    (@BrandCDFrom IS NULL OR (mt.BrandCD>=@BrandCDFrom))
    AND    (@BrandCDTo IS NULL OR (mt.BrandCD>=@BrandCDTo))
	AND    (@SegmentCDFrom IS NULL OR (mt.SegmentCD>=@SegmentCDFrom))
    AND    (@SegmentCDTo IS NULL OR (mt.SegmentCD>=@SegmentCDTo))
	AND    (@TenzikaiName IS NULL OR   (mt.TenzikaiName LIKE '%'+@TenzikaiName+'%'))
	Group BY
	mt.JANCD,
	mt.SKUCD,
	mt.SKUName,
	mt.SizeName,
	mt.ColorName,
	mt.HanbaiYoteiDate
	Order by
	mt.JANCD,mt.SKUCD,mt.SKUName,mt.SizeName,mt.ColorName,mt.HanbaiYoteiDate  ASC

END

GO

