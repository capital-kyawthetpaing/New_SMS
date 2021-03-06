 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectByJanCD_ForTenzikaishouhin]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_SKU_SelectByJanCD_ForTenzikaishouhin]
	-- Add the parameters for the stored procedure here
	@JanCD as varchar(13),
	@vendorcd as varchar(13),
	@storecd as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	
		Fs.SKUCD,
		Fs.SKUName,
		Fs.ColorNo,
		Fs.ColorName,
		Fs.SizeNo,
		Fs.SizeName,
		IsNull(Fj.PriceWithoutTax,0)
		as  SiireTanka,
		Fs.PriceOutTax as JoudaiTanka
		
	from F_SKU(getdate()) as Fs
	Left outer Join F_JANOrderPrice(getdate()) as Fj
	ON Fj.AdminNO =Fs.AdminNO
	AND VendorCD=@vendorcd
	WHERE Fs.JanCD=@JanCD
	AND SetKBN='0'
END
