 BEGIN TRY 
 Drop Procedure dbo.[M_Tenzikaishouhin_Search]
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
Create PROCEDURE [dbo].[M_Tenzikaishouhin_Search]
	-- Add the parameters for the stored procedure here
	@tenzikainame as Varchar(80),
	@vendorcd as varchar(13),
	@skuname as varchar(100),
	@year as varchar(6),
	@season as varchar(6),
	@brand as varchar(6),
	@segment as varchar(6),
	@insertdateF as datetime,
	@insertdateT as datetime,
	@updatedateF as datetime,
	@updatedateT as datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  
			TankaCD
			,TenzikaiName
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
			,(
				Select VendorName
				From  F_Vendor(getdate())
				where VendorCD=mt.VendorCD
			)as VendorName
			,ColorName
			,SizeName
			,Remarks
			,InsertDateTime
			,UpdateDateTime
	from M_Tenzikaishouhin as mt
	where DeleteFlg =0
	and ( @tenzikainame is null or TenzikaiName Like '%' +@tenzikainame +'%')
	and (@vendorcd is null or VendorCD=@vendorcd)
	and LastYearTerm =@year
	and LastSeason =@season
	and (@skuname is null or SKUName Like '%'+@skuname + '%')
	and	(@brand is null or BrandCD=@brand)
	and( @segment is null or SegmentCD=@segment)
	and (@insertdateF is null or InsertDateTime >=@insertdateF)
	and (@insertdateT is null or InsertDateTime <=@insertdateT)
	and	(@updatedateF is null or UpdateDateTime >=@updatedateF)
	and (@updatedateT is null or UpdateDateTime <=@updatedateT)
END
