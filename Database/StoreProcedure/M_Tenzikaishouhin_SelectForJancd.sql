 BEGIN TRY 
 Drop Procedure dbo.[M_Tenzikaishouhin_SelectForJancd]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_Tenzikaishouhin_SelectForJancd]
	-- Add the parameters for the stored procedure here
		 @tenzikainame as varchar(80)
		,@vendorcd as varchar(13)
		,@lastyear as varchar(6)
		,@lastseason as varchar(6)
		,@brandcd as varchar(6)
		,@segment as varchar(6)
		,@jancd as varchar(13)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select  1
		
		from M_Tenzikaishouhin
		where TenzikaiName= @tenzikainame
		and VendorCD = @vendorcd
		And LastYearTerm =@lastyear
		AND LastSeason=@lastseason
		AND (@brandcd is null or BrandCD=@brandcd)
		AND (@segment is null or SegmentCD=@segment)
		AND JanCD =@jancd
		AND DeleteFlg=0
END
