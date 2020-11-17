BEGIN TRY 
 Drop Procedure dbo.[M_TenzikaiShouhin_Check]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_TenzikaiShouhin_Check]
	-- Add the parameters for the stored procedure here
	@Tenzikainame as varchar(80),
	@VendorCD as varchar(13),
	@LastYear as varchar(6),
	@LastSeason as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 1
	From M_TenzikaiShouhin
	Where TenzikaiName =@Tenzikainame
	AND VendorCD = @VendorCD
	AND LastYearTerm =@LastYear
	AND LastSeason = @LastSeason
END
GO
