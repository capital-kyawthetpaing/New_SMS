BEGIN TRY 
 Drop Procedure [dbo].[M_ShiireKakeritsu_Select]
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
CREATE PROCEDURE [dbo].[M_ShiireKakeritsu_Select]
@VendorCD as varchar(13),
@StoreCD as varchar(4)
AS
BEGIN
	select '0' as Column1
		   ,[VendorCD]
		  ,[StoreCD]
	      ,[BrandCD]
	      ,[SportsCD]
		  ,[SegmentCD]
		  ,[LastYearTerm]
		  ,[LastSeason]
		  ,[ChangeDate]
	      ,[Rate]
	      
	  FROM M_OrderRate
	  where VendorCD=@VendorCD
	  and StoreCD=@StoreCD
	  Order by VendorCD,StoreCD,ChangeDate,BrandCD,SportsCD,SegmentCD,LastYearTerm,LastSeason Desc
END
