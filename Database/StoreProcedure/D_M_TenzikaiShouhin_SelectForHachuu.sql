 BEGIN TRY 
 Drop Procedure dbo.[M_TenzikaiShouhin_SelectForHachuu]
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
CREATE PROCEDURE [dbo].[M_TenzikaiShouhin_SelectForHachuu] 
	-- Add the parameters for the stored procedure here
	@TenzikaiName as varchar(80)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
Select
	mt.VendorCD,
	mt.TenzikaiName,
	max(mv.VendorName) as VendorName,
	mt.LastYearTerm,
	mt.LastSeason
	from M_TenzikaiShouhin mt 
	left outer join F_Vendor(getdate()) as mv on mv.VendorCD=mt.VendorCD and mt.DeleteFlg=0
	where mt.TenzikaiName = @TenzikaiName
	
	Group by
    mt.TenzikaiName,mt.VendorCD,mt.LastYearTerm,mt.LastSeason
	END

GO
