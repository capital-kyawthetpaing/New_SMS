BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikaishouhin_InsertUpdate]
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
CREATE PROCEDURE [dbo].[M_Tenzikaishouhin_InsertUpdate]
	-- Add the parameters for the stored procedure here
	@xml as xml,
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
	@KeyItem as varchar(100),
	@type tinyInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Declare
	@InsertDateTime as datetime=getdate()

	CREATE TABLE [dbo].[#tempSKUPrice]
	(
				chkflg int,
				JanCD varchar(13),
				SKUCD varchar(30),
				SKUName varchar(100),
				ColorCD varchar(10),
				ColorName varchar(20),
				SizeCD varchar(10),
				SizeName varchar(20),
				HanbaiDateMonth varchar(2),
				HanbaiDate varchar(10),
				Shiiretanka money,
				JoudaiTanka money,
				SalePriceOutTax money,
				--Rank1 money,
				--Rank2 money,
				--Rank3 money,
				--Rank4 money,
				--Rank5 money,
				BrandCD varchar(6),
				ExhibitionSegmentCD varchar(5),
				TaniCD varchar(3),
				TaxRateFlg tinyInt,
				Remark varchar(500),
				ExhibitioinCommonCD varchar(30),
				TankaCD  varchar(13),

	)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @xml
	insert into #tempSKUPrice
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				chkflg int,
				JanCD varchar(13),
				SKUCD varchar(30),
				SKUName varchar(100),
				ColorCD varchar(10),
				ColorName varchar(20),
				SizeCD varchar(10),
				SizeName varchar(20),
				HanbaiDateMonth varchar(2),
				HanbaiDate varchar(10),
				Shiiretanka money,
				JoudaiTanka money,
				SalePriceOutTax money,
				--Rank1 money ,
				--Rank2 money,
				--Rank3 money,
				--Rank4 money,
				--Rank5 money,
				BrandCD varchar(6),
				ExhibitionSegmentCD varchar(5),
				TaniCD varchar(3),
				TaxRateFlg tinyInt,
				Remark varchar(500),
				ExhibitioinCommonCD varchar(30),
				TankaCD  varchar(13)
			)
			exec sp_xml_removedocument @DocHandle;




	IF @type=2
	Begin
		Delete 
		From M_TenzikaiShouhin
		where TenzikaiName=@tenzikainame
		and VendorCD=@vendorcd
		and LastYearTerm=@year
		and LastSeason=@season
		and(@BrandCD is null OR BrandCD=@BrandCD)
		and (@SegmentCD is null OR SegmentCD=@SegmentCD)

	End

		Insert into M_TenzikaiShouhin(
					TankaCD
					,TenzikaiName
					,VendorCD
					,LastYearTerm
					,LastSeason
					,JANCD
					,SKUCD
					,BrandCD
					,SegmentCD
					,SKUName
					,SizeNO
					,ColorNO
					,SizeName
					,ColorName
					,HanbaiYoteiDateMonth
					,HanbaiYoteiDate
					,SiireTanka
					,JoudaiTanka
					,SalePriceOutTax
					,TaniCD
					,TaxRateFLG
					,Remarks
					,ExhibitionCommonCD
					,DeleteFlg
					,InsertOperator
					,InsertDateTime
					,UpdateOperator
					,UpdateDateTime

					)

			select  
				--case when tskup.SalePriceOutTax <> 0 then 0
				--	when tskup.Rank1 <> 0 then 1
				--	when tskup.Rank2 <> 0 then 2
				--	when tskup.Rank3 <> 0 then 3
				--	when tskup.Rank4 <> 0 then 4
				--	when tskup.Rank5 <> 0 then 5
				--	else 0 end,
				TankaCD,
				@tenzikainame,
				@vendorcd,
				@year,
				@season,
				tskup.JanCD,
				tskup.SKUCD,
				tskup.BrandCD,
				tskup.ExhibitionSegmentCD,
				tskup.SKUName,
				tskup.SizeCD,
				tskup.ColorCD,
				tskup.SizeName,
				tskup.ColorName,
				tskup.HanbaiDateMonth,
				tskup.HanbaiDate,
				tskup.Shiiretanka,
				tskup.JoudaiTanka,
				tskup.SalePriceOutTax,
				--case when tskup.SalePriceOutTax <> 0 then tskup.SalePriceOutTax
				--	when tskup.Rank1 <> 0 then tskup.Rank1
				--	when tskup.Rank2 <> 0 then tskup.Rank2
				--	when tskup.Rank3 <> 0 then tskup.Rank3
				--	when tskup.Rank4 <> 0 then tskup.Rank4
				--	when tskup.Rank5 <> 0 then tskup.Rank5 
				--	else 0 end,
				tskup.TaniCD,
				tskup.TaxRateFlg,
				case when tskup.Remark<>'' then tskup.Remark else null end,
				isnull(@year,'') + isnull(@season,'') + isnull(tskup.JanCD,''),
				0,
				@OperatorCD,
				@InsertDateTime,
				@OperatorCD,
				@InsertDateTime
				From #tempSKUPrice as tskup


		exec dbo.L_Log_Insert @OperatorCD,@Program,@PC,@OperateMode,@KeyItem

		drop table #tempSKUPrice

END




