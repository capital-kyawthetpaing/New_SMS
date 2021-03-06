BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikaishouhin_Delete] 
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
Create PROCEDURE [dbo].[M_Tenzikaishouhin_Delete] 
	-- Add the parameters for the stored procedure here
	@year varchar(6),
	@season varchar(6),
	@tenzikainame varchar(80),
	@vendorcd varchar(13),
	@BrandCD varchar(6),
	@SegmentCD varchar(6),
	@OperatorCD varchar(10),
	@Program as varchar(30),
	@Pc as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Delete 
	From M_TenzikaiShouhin
	where TenzikaiName=@tenzikainame
		and VendorCD=@vendorcd
		and LastYearTerm=@year
		and LastSeason=@season
		and(@BrandCD is null OR BrandCD=@BrandCD)
		and (@SegmentCD is null OR SegmentCD=@SegmentCD)

	exec dbo.L_Log_Insert @OperatorCD,@Program,@PC,@OperateMode,@KeyItem
END
