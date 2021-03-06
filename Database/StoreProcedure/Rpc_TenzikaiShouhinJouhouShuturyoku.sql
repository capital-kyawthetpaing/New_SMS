 BEGIN TRY 
 Drop Procedure dbo.[Rpc_TenzikaiShouhinJouhouShuturyoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Rpc_TenzikaiShouhinJouhouShuturyoku]
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
	JANCD AS '仮JANCD',
	SKUCD AS 'SKUCD',
	SKUName AS '商品名',
	SizeName AS 'サイズ',
	ColorName AS 'カラー',
	--HanbaiYoteiDate AS '販売予定日',
	Concat(ISNULL(max(HanbaiYoteiDateMonth), '')  , N'月' , ISNULL(max(HanbaiYoteiDate), '') )as  '販売予定日',
	'' AS '即納数',
	'' AS '希望日1',
	'' AS '希望日2',
	(ISNULL(max(LastYearTerm), '') + ISNULL(max(LastSeason), '') + ISNULL(max(JanCD), ''))As  '消さないでください',
	Max(TenzikaiName) as TenzikaiName
	from M_TenzikaiShouhin
	where  DeleteFlg=0
	AND    VendorCD=@VendorCD
	AND    LastYearTerm=@LastYearTerm
	AND    LastSeason=@LastSeason
	AND    (@BrandCDFrom IS NULL OR (BrandCD>=@BrandCDFrom))
    AND    (@BrandCDTo IS NULL OR (BrandCD<=@BrandCDTo))
	AND    (@SegmentCDFrom IS NULL OR (SegmentCD>=@SegmentCDFrom))
    AND    (@SegmentCDTo IS NULL OR (SegmentCD<=@SegmentCDTo))
	AND    (@TenzikaiName IS NULL OR   (TenzikaiName LIKE '%'+@TenzikaiName+'%'))
	Group BY
	JANCD,
	SKUCD,
	SKUName,
	SizeName,
	ColorName,
	HanbaiYoteiDate
	Order by
	JANCD,SKUCD,SKUName,SizeName,ColorName,HanbaiYoteiDate  ASC

END

