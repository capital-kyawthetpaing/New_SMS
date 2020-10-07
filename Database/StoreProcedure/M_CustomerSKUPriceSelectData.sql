 BEGIN TRY 
 Drop Procedure dbo.[M_CustomerSKUPriceSelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE M_CustomerSKUPriceSelectData
	-- Add the parameters for the stored procedure here
	@TekiyouKaisiDate_From varchar(10),
	@TekiyouKaisiDate_To varchar(10),
	@CustomerCD_From	 varchar(13),
	@CustomerCD_To		 varchar(13),
	@SKUCD_From			 varchar(30),
	@SKUCD_To			 varchar(30),
	@SKUName			 varchar(100),
	@DisplayKBN tinyInt
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @DisplayKBN = 1
	Begin

		select CustomerCD
		,CustomerName
		,CONVERT(VARCHAR(10), TekiyouKaisiDate , 111) as TekiyouKaisiDate
		,CONVERT(VARCHAR(10), TekiyouShuuryouDate , 111) as TekiyouShuuryouDate
		,AdminNO
		,JanCD
		,SKUCD
		,SKUName
		,SalePriceOutTax
		,Remarks

		from M_CustomerSKUPrice
		where (@TekiyouKaisiDate_From is null OR TekiyouKaisiDate>=@TekiyouKaisiDate_From)
		and (@TekiyouKaisiDate_To is null OR TekiyouKaisiDate<=@TekiyouKaisiDate_To)
		and (@CustomerCD_From is null OR CustomerCD>=@CustomerCD_From)
		and (@CustomerCD_To is null OR CustomerCD<=@CustomerCD_To)
		and (@SKUCD_From is null OR SKUCD>=@SKUCD_From)
		and (@SKUCD_To is null OR SKUCD<=@SKUCD_To)
		and (@SKUName is null OR SKUName like '%'+ @TekiyouKaisiDate_To+'%' )
		and DeleteFlg=0

	End
	Else
	Begin
		select CustomerCD
		,CustomerName
		,CONVERT(VARCHAR(10), TekiyouKaisiDate , 111) as TekiyouKaisiDate
		,CONVERT(VARCHAR(10), TekiyouShuuryouDate , 111) as TekiyouShuuryouDate
		,AdminNO
		,JanCD
		,SKUCD
		,SKUName
		,SalePriceOutTax
		,Remarks

		from F_CustomerSKUPrice(Getdate())
		where (@TekiyouKaisiDate_From is null OR TekiyouKaisiDate>=@TekiyouKaisiDate_From)
		and (@TekiyouKaisiDate_To is null OR TekiyouKaisiDate<=@TekiyouKaisiDate_To)
		and (@CustomerCD_From is null OR CustomerCD>=@CustomerCD_From)
		and (@CustomerCD_To is null OR CustomerCD<=@CustomerCD_To)
		and (@SKUCD_From is null OR SKUCD>=@SKUCD_From)
		and (@SKUCD_To is null OR SKUCD<=@SKUCD_To)
		and (@SKUName is null OR SKUName like '%'+ @TekiyouKaisiDate_To+'%' )
		and DeleteFlg=0
	End
END
GO
