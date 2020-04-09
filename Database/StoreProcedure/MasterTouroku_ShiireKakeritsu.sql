 BEGIN TRY 
 Drop Procedure dbo.[MasterTouroku_ShiireKakeritsu]
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
CREATE PROCEDURE [dbo].[MasterTouroku_ShiireKakeritsu]
@VendorCD as varchar(13)
AS
BEGIN
	declare @currentDate as datetime = getdate();
	CREATE TABLE [dbo].[#Tmp_M_OrderRate](
	[VendorCD][varchar](13),
	[BrandCD][varchar](6),
	[SportsCD][varchar](6),
	[SegmentCD][varchar](6),
	[LastSeason][varchar](6),
	[ChangeDate][date],
	[Rate][decimal]NOT NULL
	)
	INSERT INTO #Tmp_M_OrderRate
	(
		VendorCD,
		BrandCD,
		SportsCD,
		SegmentCD,
		LastSeason,
		ChangeDate,
		Rate,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime
	)
	select 
	  vendorCD,
	  BrandCD,
	  SportsCD,
	  SegmentCD,
	  LastSeason,
	  ChangeDate,
	  Rate,
	  InsertOperator,
	  InsertDateTime,
	  UpdateOperator,
	  UpdateDateTime
	  from M_OrderRate
	  where VendorCD=@VendorCD
	  order by VendorCD,ChangeDate,BrandCD,SportsCD,SegmentCD,LastSeason

--drop table #Tmp_M_OrderRate

END
