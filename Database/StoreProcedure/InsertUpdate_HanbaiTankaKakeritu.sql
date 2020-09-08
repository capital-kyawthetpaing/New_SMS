 BEGIN TRY 
 Drop Procedure dbo.InsertUpdate_HanbaiTankaKakeritu
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InsertUpdate_HanbaiTankaKakeritu
	@xml as xml,
	@StartDate varchar(10),
	@EndDate varchar(10),
	@TankaCD varchar(13),
	@BrandCD varchar(6),
	@ExhibitionSegmentCD varchar(5),
	@LastYearTerm varchar(6),
	@LastSeason varchar(6),
	@PriceOutTaxFrom money,
	@PriceOutTaxTo money,
	@Mode tinyint,
	@OperateMode as varchar(10),
	@ProgramID varchar(100),
	@Operator varchar(10),
	@PC varchar(30),
	@KeyItem as varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @InsertDateTime datetime=getdate()

	CREATE TABLE [dbo].[#tempSalesRate]
	(
			TankaCD varchar(13),
			TankaName varchar(30),
			BrandCD varchar(6),
			BrandName varchar(30),
			ExhibitionSegmentCD	varchar(5),
			ExhibitionSegmentName varchar(30),
			LastYearTerm varchar(6),
			LastSeason	varchar(6),
			StartDate  varchar(10),
			EndDate varchar(10),
			Rate	varchar(10)
	)
	
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @xml
	insert into #tempSalesRate
	select * 
	--into #tempJuChuu
	FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				TankaCD varchar(13),
				TankaName varchar(30),
				BrandCD varchar(6),
				BrandName varchar(30),
				ExhibitionSegmentCD	varchar(5),
				ExhibitionSegmentName varchar(30),
				LastYearTerm varchar(6),
				LastSeason	varchar(6),
				StartDate  varchar(10),
				EndDate varchar(10),
				Rate	varchar(10)
				
			)
			exec sp_xml_removedocument @DocHandle;

		Insert into M_SalesRate(
			ApplicationStartDate,
			ApplicationEndDate,
			RankCD,
			BrandCD,
			SegmentCD,
			LastYearTerm,
			LastSeason,
			Rate,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime)
		select @StartDate,
			@EndDate,
			TankaCD,
			BrandCD,
			ExhibitionSegmentCD,
			LastYearTerm,
			LastSeason,
			Rate,
			@Operator,
			@InsertDateTime,
			@Operator,
			@InsertDateTime
		from #tempSalesRate

		If @Mode=1
		Begin
			Insert into M_SKUPrice(
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
			select @TankaCD,
				'0000',
				fs.AdminNO,
				fs.SKUCD,
				@StartDate,
				@EndDate,
				0,
				Isnull(fs.PriceOutTax,0),
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
				case 
					when ftcd.RoundKBN=1 then Isnull(floor(fs.PriceOutTax * tsr.Rate),0)
					when ftcd.RoundKBN=2 then Isnull(ceiling(fs.PriceOutTax * tsr.Rate),0)
					else Isnull(round((fs.PriceOutTax * tsr.Rate),2),0) end,
				0,
				0,
				0,
				NULL,
				0,
				0,
				@Operator,
				@InsertDateTime,
				@Operator,
				@InsertDateTime
			From F_SKU(@StartDate) as fs
			left outer join #tempSalesRate as tsr on tsr.BrandCD=fs.BrandCD and fs.DeleteFlg=0
													and tsr.ExhibitionSegmentCD=fs.ExhibitionSegmentCD
													and tsr.LastYearTerm=fs.LastYearTerm
													and tsr.LastSeason=fs.LastSeason
			left outer join F_TankaCD(@StartDate) as ftcd on ftcd.TankaCD=@TankaCD
			Where (@BrandCD is null or fs.BrandCD=@BrandCD)
			and (@ExhibitionSegmentCD is null or fs.ExhibitionSegmentCD=@ExhibitionSegmentCD)
			and (@LastYearTerm is null or fs.LastYearTerm=@LastYearTerm)
			and (@LastSeason is null or fs.LastSeason=@LastSeason)
			and (@PriceOutTaxFrom is null or fs.PriceOutTax<=@PriceOutTaxFrom)
			and (@PriceOutTaxTo is null or fs.PriceOutTax>=@PriceOutTaxTo)
			and NOT Exists(Select * 
							from M_SKUPrice as msp
							where msp.DeleteFlg=0
							and msp.TankaCD=@TankaCD
							and msp.StoreCD='0000'
							and msp.AdminNO=fs.AdminNO
							and msp.ChangeDate=@StartDate)
		End

		Else
		Begin 
			
			Update mskup
			Set TekiyouShuuryouDate=@EndDate,
			SalePriceOutTax=case 
								when ftcd.RoundKBN=1 then Isnull(floor(fs.PriceOutTax * tsr.Rate),0)
								when ftcd.RoundKBN=2 then Isnull(ceiling(fs.PriceOutTax * tsr.Rate),0)
								else Isnull(round((fs.PriceOutTax * tsr.Rate),2),0) end
			From M_SKUPrice as mskup 
			left outer join F_SKU(@StartDate) as fs on fs.AdminNO=mskup.AdminNO 
														and fs.DeleteFlg=0
														and mskup.DeleteFlg=0
			left outer join #tempSalesRate as tsr on  tsr.BrandCD=fs.BrandCD 
														and tsr.ExhibitionSegmentCD=fs.ExhibitionSegmentCD
														and tsr.LastYearTerm=fs.LastYearTerm
														and tsr.LastSeason=fs.LastSeason
			left outer join F_TankaCD(@StartDate) as ftcd on ftcd.TankaCD=@TankaCD
			Where mskup.TankaCD=@TankaCD
			and mskup.StoreCD='0000'
			and mskup.ChangeDate=@StartDate
			and Exists(Select * 
						from M_SKUPrice as msp
						where msp.DeleteFlg=0
						and msp.TankaCD=@TankaCD
						and msp.StoreCD='0000'
						and msp.AdminNO=fs.AdminNO
						and msp.ChangeDate=@StartDate)

		End


			exec dbo.L_Log_Insert @Operator,@ProgramID,@PC,@OperateMode,@KeyItem

		Drop table #tempSalesRate
END
GO
