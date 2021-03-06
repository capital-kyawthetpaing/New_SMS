BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikaishouhin_Search]
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
Create  PROCEDURE [dbo].[M_Tenzikaishouhin_Search]
	-- Add the parameters for the stored procedure here
	@tenzikainame as varchar(80),
	@vendorcd as varchar(13),
	@skuname as varchar(100),
	@year as varchar(6),
	@season as varchar(6),
	@brand as varchar(6),
	@segment as varchar(6),
	@insertdateF as varchar(10),
	@insertdateT as varchar(10),
	@updatedateF as varchar(10),
	@updatedateT as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  
			Distinct
			TenzikaiName
			,SKUCD
			,JANCD
			,SKUName
			,	(Select Char1
					from M_MultiPorpose as mp
					where ID='226'
					and [Key]=mt.SegmentCD
				)as SegmentName
			,
			(Select BrandName
					from M_Brand as mb
					where BrandCD=mt.BrandCD
			)as BrandName
			--,(
			--	Select VendorName
			--	From  F_Vendor(getdate())
			--	where VendorCD=mt.VendorCD
			--)as VendorName
			,fv.VendorCD
			,fv.VendorName
			,ColorName
			,SizeName
			,Remarks
			,mt.InsertDateTime
			,mt.UpdateDateTime
			,mt.LastYearTerm
			,mt.LastSeason
	from M_Tenzikaishouhin as mt 
	left outer join F_Vendor(GETDATE()) as fv on fv.VendorCD=mt.VendorCD and mt.DeleteFlg=0
	where 
	 ( @tenzikainame is null or TenzikaiName Like '%' +@tenzikainame +'%')
	and (@vendorcd is null or mt.VendorCD=@vendorcd)
	and LastYearTerm =@year
	and LastSeason =@season
	and (@skuname is null or SKUName Like '%'+@skuname + '%')
	and	(@brand is null or BrandCD=@brand)
	and( @segment is null or SegmentCD=@segment)
	and (@insertdateF is null or  CONVERT(VARCHAR(10), mt.InsertDateTime , 111) >=@insertdateF)
	and (@insertdateT is null or CONVERT(VARCHAR(10), mt.InsertDateTime , 111) <=@insertdateT)
	and	(@updatedateF is null or CONVERT(VARCHAR(10), mt.UpdateDateTime , 111) >=@updatedateF)
	and (@updatedateT is null or CONVERT(VARCHAR(10), mt.UpdateDateTime , 111) <=@updatedateT)
	order by 
		LastYearTerm DESC,
		LastSeason DESC,
		TenzikaiName ASC,
		SKUCD ASC,
		JanCD ASC



END
