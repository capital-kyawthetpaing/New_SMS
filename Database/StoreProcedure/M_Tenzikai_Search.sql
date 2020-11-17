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
@VendorCD as varchar(13),
@TenzikaiName as varchar(80),
@VendorCDFrom as varchar(13),
@VendorCDTo as varchar(13),
@LastYearTerm as varchar(6),
@LastSeason as varchar(6),
@NStartDate as date,
@NEndDate as date,
@LStartDate as date,
@LEndDate as date
AS
BEGIN
Select
mt.TenzikaiName,
max(mv.VendorName) as VendorName,
mt.LastYearTerm,
mt.LastSeason
from M_TenzikaiShouhin mt 
left outer join F_Vendor(getdate()) as mv on mv.VendorCD=mt.VendorCD and mt.DeleteFlg=0
where 
 (@TenzikaiName IS NULL OR (mt.TenzikaiName LIKE '%'+@TenzikaiName+'%'))
AND (@VendorCDFrom IS NULL OR (mt.VendorCD>=@VendorCDFrom))
AND (@VendorCDTo IS NULL OR (mt.VendorCD<=@VendorCDTo))
AND  mt.LastYearTerm=@LastYearTerm
AND  mt.LastSeason=@LastSeason
AND (@NStartDate IS NULL OR(mt.InsertDateTime>=@NStartDate))
AND (@NEndDate IS NULL OR(mt.InsertDateTime<=@NEndDate))
AND (@LStartDate IS NULL OR(mt.UpdateDateTime>=@LStartDate))
AND (@LEndDate IS NULL OR(mt.UpdateDateTime<=@LEndDate))
Group by
mt.TenzikaiName,mt.VendorCD,mt.LastYearTerm,mt.LastSeason
Order by mt.LastYearTerm,mt.LastSeason DESC,mt.TenzikaiName,mt.VendorCD ASC
END

GO

