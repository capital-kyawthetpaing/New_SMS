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
	select 
		SKUCD,
		SKUName,
		ColorNo,
		ColorName,
		SizeNo,
		SizeName,
		(
		Select IsNull(PriceWithoutTax,0)
		from F_JANOrderPrice(getdate())
		where VendorCD=@vendorcd
		and StoreCD=@Storecd
		) as  SiireTanka,
		PriceOutTax as JoudaiTanka
		
	from F_SKU(getdate())
	where JANCD=@JanCD
	AND SetKBN='0'
END
