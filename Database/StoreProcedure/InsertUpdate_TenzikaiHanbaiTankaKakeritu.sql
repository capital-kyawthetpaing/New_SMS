 BEGIN TRY 
 Drop Procedure [dbo].[InsertUpdate_TenzikaiHanbaiTankaKakeritu]
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
Create PROCEDURE [dbo].[InsertUpdate_TenzikaiHanbaiTankaKakeritu]
	-- Add the parameters for the stored procedure here
			@TankaCD as Varchar(13),
			@BrandCD as VarChar(6),
			@SegmentCD as VarChar(6),
			@LastYearTerm as VarChar(6),
			@LastSeason as VarChar(6),
			@PriceOutTaxF as Varchar(10),
			@PriceOutTaxT as VarChar(10),
			@xml as Xml,
			@Operator varchar(10),
			@Program as varchar(100),
			@PC as varchar(30),
			@OperateMode as varchar(50),
			@KeyItem as varchar(100)
			

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @InsertDateTime datetime=getdate()
	Declare @DocHandle int

	DECLARE @DocDel int	
	EXEC sp_xml_preparedocument @DocDel OUTPUT, @xml

	SELECT *
	Into #tmpInser
	FROM OPENXML (@DocDel, '/NewDataSet/test',2)
	WITH(
	BrandCD VARCHAR(6),
	SegmentCD VARCHAR(6),
	LastYearTerm VARCHAR(6),
	LastSeason VARCHAR(6),
	Rate  decimal(5,2)
	)
	EXEC sp_xml_removedocument @DocDel; 

	Insert 
	Into   M_TenzikaiRate
	Select 
		@TankaCD as RankCD,
		BrandCD,
		SegmentCD,
		LastYearTerm,
		LastSeason,
		Rate,
		@Operator as InsertOperator,
		@InsertDateTime as InsertDateTime,
		@Operator as UpdateOperator,
		@InsertDateTime as UpdateDateTime
	From #tmpInser


	Update M_TenzikaiShouhin
	set UpdateOperator=@Operator,
		UpdateDateTime=@InsertDateTime,
		SalePriceOutTax =   
						Case 
						when  RoundKBN =1  then   Floor(m.joudaiTanka * tm.Rate) 
						when  RoundKBN =2  then   Round(m.joudaiTanka * tm.Rate,2)
						when  RoundKBN =3  then   CEILING(m.joudaiTanka * tm.Rate)
						end

	From		M_TenzikaiShouhin as m
	inner join F_TankaCD(getdate()) as mt on mt.TankaCD=@TankaCD
	inner join #tmpInser as tm	on	tm.BrandCD		=m.BrandCD
								and tm.SegmentCD	=m.SegmentCD
								and tm.LastYearTerm	=m.LastYearTerm
								and tm.LastSeason	=m.LastSeason
	where	m.DeleteFlg=0
			and m.TankaCD=@TankaCD
			and @BrandCD is NUll or m.BrandCD =tm.BrandCD
			and @SegmentCD is NULl or m.SegmentCD=tm.SegmentCD
			and @LastYearTerm is NUll or m.LastYearTerm = tm.LastYearTerm
			and @LastSeason is NUll or m.LastSeason = tm.LastSeaSon
			and @PriceOutTaxF is NUll or m.JOudaiTanka=@PriceOutTaxF
			and @PriceOutTaxT is NULL or m.JOudaiTanka=@PriceOutTaxT


			
	
	drop Table #tmpInser

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END
