 BEGIN TRY 
 Drop Procedure dbo.[D_TenzikaiJuchuu_SelectForExcel]
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
CREATE PROCEDURE [dbo].[D_TenzikaiJuchuu_SelectForExcel]
	-- Add the parameters for the stored procedure here
	@VendorCD as varchar(12),
	@Year as varchar(6),
	@Season as varchar(6),
	@CustomerCDFrom as varchar(13),
	@CustomerCDTo as varchar(13),
	@BrandCD as varchar(6),
	@SegmentCD as varchar(6),
	@TenzikaiName as varchar(80),
	@Chk as int,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#temp_tenzikai](	[JanCD]	[varchar](13) NOT NULL,											
	[VendorCD] [varchar](13) NOT NULL,												
	[LastYearTerm] [varchar](6) NOT NULL,											
	[LastSeason] [varchar](6) NOT NULL,	
	[BrandCD] [varchar](6) collate Japanese_CI_AS,
	[SegmentCD] [varchar](6) collate Japanese_CI_AS,
	[TenzikaiName] [varchar](80) NOT NULL )
	
	INSERT INTO #temp_tenzikai	(		JanCD,		VendorCD,		LastYearTerm,		LastSeason,		BrandCD,		SegmentCD,		TenzikaiName		)	select		mts.JanCD,												
		mts.VendorCD,												
		mts.LastYearTerm,												
		mts.LastSeason,												
		MAX(mts.BrandCD) as BrandCD,												
		MAX(mts.SegmentCD)as SegmentCD,												
		MAX(mts.TenzikaiName)as TenzikaiName													from M_TenzikaiShouhin mts	where mts.VendorCD = @VendorCD
	and mts.LastYearTerm = @Year
	and mts.lastSeason = @Season
	and (@BrandCD is null or  mts.BrandCD = @BrandCD)
	and (@SegmentCD is null or mts.SegmentCD = @SegmentCD) 
	and (@TenzikaiName is null or mts.TenzikaiName LIKE '%' + @TenzikaiName + '%')
	and mts.DeleteFLG = 0
	Group by mts.JanCD,mts.VendorCD,mts.LastYearTerm,mts.LastSeason
				
	
	if @Chk = 1 
	BEGIN 
		Select dtz.CustomerCD,
		Max(dtz.CustomerName)as CustomerName,
		Max(dtz.Address1 + dtz.Address2) as [Address],
		dtzd.SKUName,
		SUM(dtzd.JuchuuSuu) as JuchuuSuu,
		Max(temp.TenzikaiName) as TenzikaiName
		From D_TenzikaiJuchuu dtz
		inner join D_TenzikaiJuchuuDetails dtzd on dtzd.TenzikaiJuchuuNO = dtz.TenzikaiJuchuuNO
		inner join #temp_tenzikai temp on temp.JanCD = dtzd.JanCD and temp.VendorCD = dtz.VendorCD 
		and temp.LastYearTerm = dtz.LastYearTerm and temp.LastSeason = dtz.LastSeason 
		where dtz.DeleteDateTime is null 
		and dtzd.DeleteDateTime is null 
		and dtz.VendorCD = @VendorCD 
		and dtz.LastYearTerm = @Year
		and dtz.LastSeason = @Season
		and (@CustomerCDFrom is null or dtz.CustomerCD >= @CustomerCDFrom) 
		and (@CustomerCDTo is null or dtz.CustomerCD <= @CustomerCDTo)
		Group by 
		dtz.CustomerCD,dtzd.SKUName
		Order by 
		dtz.CustomerCD,Max(dtz.CustomerName),
		[Address],
		dtzd.SKUName asc

	END
	ELSE 
	BEGIN
		Select dtzd.SKUName,
		Sum(dtzd.JuchuuSuu) as JuchuuSuu,
		Max(temp.TenzikaiName) as TenzikaiName
		From D_TenzikaiJuchuu dtz
		inner join D_TenzikaiJuchuuDetails dtzd on dtzd.TenzikaiJuchuuNO = dtz.TenzikaiJuchuuNO
		left outer join #temp_tenzikai temp on temp.JanCD = dtzd.JanCD and temp.VendorCD = dtz.VendorCD 
		and temp.LastYearTerm = dtz.LastYearTerm and temp.LastSeason = dtz.LastSeason 
		where dtz.DeleteDateTime is null 
		and dtzd.DeleteDateTime is null 
		and dtz.VendorCD = @VendorCD 
		and dtz.LastYearTerm = @Year
		and dtz.LastSeason = @Season
		and (@CustomerCDFrom is null or dtz.CustomerCD >= @CustomerCDFrom) 
		and (@CustomerCDTo is null or dtz.CustomerCD <= @CustomerCDTo)
		Group by 
		dtzd.SKUName
		Order by 
		dtzd.SKUName asc
	END

	drop table #temp_tenzikai

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END


GO
