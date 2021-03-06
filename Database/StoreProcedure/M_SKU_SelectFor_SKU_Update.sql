BEGIN TRY 
 Drop Procedure [dbo].[M_SKU_SelectFor_SKU_Update]
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
Create PROCEDURE [dbo].[M_SKU_SelectFor_SKU_Update]
	-- Add the parameters for the stored procedure here
	
		
	@itemtb as Xml,
	@skutb as Xml,
	@date as varchar(10),
	@type as varchar,
	@itemcd as varchar(30)
	AS
BEGIN
	
		SET NOCOUNT ON;
	Declare @date1 as date=getdate()
   
  
   if @type =1  -- for copy
   begin
	DECLARE @DocHandle int	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @itemtb
	 SELECT * 
	 into #tmpitem1
	 FROM OPENXML (@DocHandle, '/NewDataSet/test',3)
	WITH(
	CheckBox  Varchar,

	ITemCD Varchar(30),
	ITemName varchar(100),
	MakerItem VARCHAR(30),
	ChangeDate date,
	PriceOutTax money,
	PriceWithoutTax money
	)
	EXEC sp_xml_removedocument @DocHandle; 
	
		Select 
			tm.ITemCD
			,
			tm.ITemName,
			ts.AdminNO,
			ts.SKUCD,
			ts.SizeName,
			ts.ColorName,
			ts.MakerItem,
			ts.BrandCD,
			ts.SportsCD,
			ts.SegmentCD,
			ts.LastYearTerm,
			ts.LastSeason,
			IsNUll(tm.PriceOutTax,0) as PriceOutTax,
			CONVERT(VARCHAR(10),ts.ChangeDate, 111) as ChangeDate,
			IsNUll(ts.Rate,0) as Rate,
			tm.PriceWithoutTax,
			tm.CheckBox
		from #tmpitem1 as tm
		inner join F_SKU(@date1) as ts 
		on tm.ITemCD=ts.ITemCD 
		and ts.SetKBN=0 and ts.ItemCD not like '%-k'
		and DeleteFlg=0
		where tm.CheckBox=1
  drop Table #tmpitem1
  end  

   if @type =2  --for update
   begin
	
	DECLARE @Docitem int	
	EXEC sp_xml_preparedocument @Docitem OUTPUT, @itemtb
	 SELECT * 
	 into #tmitem
	 FROM OPENXML (@Docitem, '/NewDataSet/test',3)
	WITH(
	CheckBox  Varchar,
	LastYearTerm varchar(6),
	LastSeason varchar(6),
	BrandCD varchar(6),
	SportsCD varchar(6),
	SegmentCD varchar(6),
	ITemCD Varchar(30),
	MakerItem VARCHAR(30),
	Rate decimal,
	ChangeDate date,
	PriceOutTax money,
	PriceWithoutTax money
	)
	EXEC sp_xml_removedocument @Docitem; 



	DECLARE @Docsku int	
	EXEC sp_xml_preparedocument @Docsku OUTPUT, @skutb
	
	 SELECT * 
	 into #tmsku
	 FROM OPENXML (@Docsku, '/NewDataSet/test',3)
	WITH(
	AdminNO int,
	SKUCD varchar(40),
	CheckBox  Varchar,
	SizeName Varchar(20),
	ColorName Varchar(20),
	ITemCD Varchar(30),
	MakerItem VARCHAR(30),
	Rate decimal,
	ChangeDate date,
	PriceOutTax money,
	PriceWithoutTax money
	)
	EXEC sp_xml_removedocument @Docsku; 
		
		Select  
		
			tm.ITemCD,
			ts.AdminNO,
			ts.SKUCD,
			ts.SizeName,
			ts.ColorName,
			ts.MakerItem,
			tm.BrandCD,
			tm.SportsCD
			,
			tm.SegmentCD,
			tm.LastYearTerm
			,
			tm.LastSeason,
			IsNUll(tm.PriceOutTax,0) as PriceOutTax,
			CONVERT(VARCHAR(10),ts.ChangeDate, 111) as ChangeDate,
			IsNUll(tm.Rate,0) as Rate,
			tm.PriceWithoutTax,
			tm.CheckBox
		from #tmitem as tm
		left outer  join #tmsku as ts
		on tm.ITemCD=ts.ITemCD
		and ts.ChangeDate <= @date
		where tm.CheckBox=1

		

  drop Table #tmitem
   drop Table #tmsku


  end

  if @type =3  -- for add 
 
   begin
		Select  SKUCD,
				AdminNO,
				ITemCD,
				SizeName,
				ColorName,
				MakerItem,
				BrandCD,
				SportsCD,
				SegmentCD,
				LastYearTerm,
				LastSeason,
				CONVERT(VARCHAR(10),ChangeDate, 111) as ChangeDate,
				isNUll(Rate,0) as Rate
				,IsNUll(PriceOutTax,0) as PriceOutTax,
				InsertOperator,
				InsertDateTime,
				UpdateOperator,
				UpdateDateTime


			
		from M_SKU 
		where M_SKU.ITemCD= @itemcd
		and SetKBN=0 and ItemCD not like '%-k'
		and ChangeDate <= @date

  end

END