BEGIN TRY 
 Drop Procedure [dbo].[M_Tenzikaishouhin_DeleteUpdate]
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
Create PROCEDURE [dbo].[M_Tenzikaishouhin_DeleteUpdate]
	-- Add the parameters for the stored procedure here
		@xml as xml,
		@year varchar(6),
		@season varchar(6),
		@startDate varchar(10),
		@OperatorCD varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
				BrandCD varchar(6),
				ExhibitionSegmentCD varchar(5),
				TaniCD varchar(3),
				TaxRateFlg tinyInt,
				Remark varchar(500),
				ExhibitioinCommonCD varchar(30),
				TankaCD  varchar(13)

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
				BrandCD varchar(6),
				ExhibitionSegmentCD varchar(5),
				TaniCD varchar(3),
				TaxRateFlg tinyInt,
				Remark varchar(500),
				ExhibitioinCommonCD varchar(30),
				TankaCD  varchar(13)
			)
			exec sp_xml_removedocument @DocHandle;

	Delete mskup
	from M_SKUPrice as mskup 
	inner join F_SKU(@startDate) as fsku on mskup.AdminNO=fsku.AdminNO 
											and mskup.DeleteFlg=0
											and fsku.DeleteFlg=0
	inner join #tempSKUPrice as tskup on fsku.ExhibitionCommonCD=tskup.ExhibitioinCommonCD and tskup.chkflg=1
	where mskup.StoreCD='0000'
	and mskup.ChangeDate=@startDate


	---テーブル転送仕様Ｃ
	INSERT into M_SKUPrice(
			TankaCD
			,StoreCD
			,AdminNO
			,SKUCD
			,ChangeDate
			,TekiyouShuuryouDate
			,PriceWithTax
			,PriceWithoutTax
			,GeneralRate
			,GeneralPriceWithTax
			,GeneralPriceOutTax
			,MemberRate
			,MemberPriceWithTax
			,MemberPriceOutTax
			,ClientRate
			,ClientPriceWithTax
			,ClientPriceOutTax
			,SaleRate
			,SalePriceWithTax
			,SalePriceOutTax
			,WebRate
			,WebPriceWithTax
			,WebPriceOutTax
			,Remarks
			,DeleteFlg
			,UsedFlg
			,InsertOperator
			,InsertDateTime
			,UpdateOperator
			,UpdateDateTime

	)
	select 
	tskup.TankaCD,
	'0000',
	fsku.AdminNO,
	fsku.SKUCD,
	@startDate,
	null,
	0,
	tskup.JoudaiTanka,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	tskup.SalePriceOutTax,
	0,
	0,
	0,
	tskup.Remark,
	0,
	0,
	@OperatorCD,
	@InsertDateTime,
	@OperatorCD,
	@InsertDateTime

	from F_SKU(@startDate) as fsku
	inner join #tempSKUPrice as tskup on fsku.ExhibitionCommonCD=tskup.ExhibitioinCommonCD
										--fsku.JanCD=tskup.JanCD
										--and fsku.SKUName=tskup.SKUName
										--and fsku.ColorName=tskup.ColorName
										--and fsku.SizeName=tskup.SizeName
										
	where DeleteFlg=0 and
	(
	(fsku.ExhibitionSegmentCD is null and fsku.JanCD = tskup.JanCD
										OR (fsku.SKUName=tskup.SKUName
											and fsku.ColorName=tskup.ColorName
											and fsku.SizeName=tskup.SizeName
											and fsku.LastYearTerm=@year
											and fsku.LastSeason=@season)
											)
	OR
	(fsku.ExhibitionSegmentCD is not null and fsku.ExhibitionSegmentCD=tskup.ExhibitionSegmentCD and tskup.chkflg=1)
	)

	--テーブル転送仕様Ｄ
	Update fsku
	set ExhibitionSegmentCD=tskup.ExhibitionSegmentCD,
	UpdateOperator=@OperatorCD,
	UpdateDateTime=@InsertDateTime
	from M_SKU as fsku 
	inner join #tempSKUPrice as tskup on fsku.ExhibitionSegmentCD=tskup.ExhibitionSegmentCD and fsku.DeleteFlg=0
	and fsku.ChangeDate<=@startDate

   

  drop table #tempSKUPrice;
END
