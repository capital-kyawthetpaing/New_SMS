BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikaishouhin_SelectForJancd]
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
Create PROCEDURE [dbo].[M_Tenzikaishouhin_SelectForJancd]
	-- Add the parameters for the stored procedure here
		 @tenzikainame as varchar(80)
		,@vendorcd as varchar(13)
		,@lastyear as varchar(6)
		,@lastseason as varchar(6)
		,@brandcd as varchar(6)
		,@segment as varchar(6)
		,@jancd as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			select  
		JANCD,
		SKUCD,
		SKUName,
		ColorNo,
		ColorName,
		SizeNo,
		SizeName,
		HanbaiYoteiDateMonth,
		HanbaiYoteiDate,
		SiireTanka,
		JoudaiTanka,
		Case	When TankaCD='0' Then SalePriceOutTax Else 0 End as SalePriceOutTax,
		Case	When TankaCD='1' Then SalePriceOutTax Else 0 End as TankKa1,
		Case	When TankaCD='2' Then SalePriceOutTax Else 0 End as TankKa2,
		Case	When TankaCD='3' Then SalePriceOutTax Else 0 End	as TankKa3,
		Case	When TankaCD='4' Then SalePriceOutTax Else 0 End	as TankKa4,
		Case	When TankaCD='5' Then SalePriceOutTax Else 0 End	as TankKa5, 		
		BrandCD,
		SegmentCD,
		TaniCD,
		Case
		 
		When TaxRateFlG=0 Then N''
		When TaxRateFlG=1 Then N''
		Else N''
		 End as TaxRateFlg,
		Remarks,
		NUll as ExhibitionCommonCD
		from M_Tenzikaishouhin
		where TenzikaiName= @tenzikainame
		and VendorCD = @vendorcd
		And LastYearTerm =@lastyear
		AND LastSeason=@lastseason
		AND BrandCD=@brandcd
		--AND SegmentCD=@segment
		AND JanCD =@jancd
		AND DeleteFlg=0
END
