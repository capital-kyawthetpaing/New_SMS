BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikai_Search]
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
CREATE PROCEDURE [dbo].[M_Tenzikai_Search]
	@ChangeDate as varchar(10),
	@VendorCD as varchar(13),
	@TenzikaiName as varchar(80),
	@VendorCDFrom as varchar(13),
	@VendorCDTo as varchar(13),
	@LastYearTerm as varchar(6),
	@LastSeason as varchar(6),
	@NStartDate as varchar(10),
	@NEndDate as varchar(10),
	@LStartDate as varchar(10),
	@LEndDate as varchar(10)
AS
BEGIN
	Select
	mt.VendorCD,
	mt.TenzikaiName,
	max(mv.VendorName) as VendorName,
	mt.LastYearTerm,
	mt.LastSeason
	from M_TenzikaiShouhin mt 
	left outer join F_Vendor(@ChangeDate) as mv on mv.VendorCD=mt.VendorCD and mt.DeleteFlg=0
	where 
	 (@TenzikaiName IS NULL OR (mt.TenzikaiName LIKE '%'+@TenzikaiName+'%'))
	AND (@VendorCDFrom IS NULL OR (mt.VendorCD>=@VendorCDFrom))
	AND (@VendorCDTo IS NULL OR (mt.VendorCD<=@VendorCDTo))
	AND  mt.LastYearTerm=@LastYearTerm
	AND  mt.LastSeason=@LastSeason
	AND (@NStartDate IS NULL OR( CONVERT(VARCHAR(10), mt.InsertDateTime , 111)>=@NStartDate))
	AND (@NEndDate IS NULL OR(  CONVERT(VARCHAR(10), mt.InsertDateTime , 111)<=@NEndDate))
	AND (@LStartDate IS NULL OR(  CONVERT(VARCHAR(10), mt.UpdateDateTime , 111)>=@LStartDate))
	AND (@LEndDate IS NULL OR( CONVERT(VARCHAR(10), mt.UpdateDateTime , 111)<=@LEndDate))
	Group by
	mt.TenzikaiName,mt.VendorCD,mt.LastYearTerm,mt.LastSeason
	Order by mt.LastYearTerm,mt.LastSeason DESC,mt.TenzikaiName,mt.VendorCD ASC
END

