BEGIN TRY 
 Drop Procedure dbo.[M_ITEM_SelectBy_ItemCD]
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
Create PROCEDURE [dbo].[M_ITEM_SelectBy_ItemCD]
	-- Add the parameters for the stored procedure here
	@itemcd as varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT  
					mi.MakerItem,
					mi.BrandCD,
					 (
						Select BrandName   
						from M_Brand as mb
						where mb.BrandCD=mi.BrandCD
					 ) as BrandName,
					 mi.SportsCD,
					 (
						Select Char1   
						from M_Multiporpose as mp


						where mp.ID=202
						and mp.[Key]=mi.SportsCD
					 ) as Char1,
					 mi.SegmentCD,
					 (
						Select Char1   
						from M_Multiporpose as mp
						where mp.ID=203
						and mp.[Key]=mi.SegmentCD
					 ) as SegmentCDName,
					mi.LastYearTerm,
					mi.LastSeason,
					mi.ITemName,
					IsNUll(mi.PriceOutTax,0) as PriceOutTax
	
	 from M_Item as mi
	  where mi.ItemCD =@itemcd
	   and SetKBN=0 and ITemCD not like '%-k'

END
