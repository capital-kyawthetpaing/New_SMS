BEGIN TRY 
 Drop Procedure [dbo].[M_SKU_SelectByJanCD_ForTenzikaishouhin]
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
CREATE PROCEDURE [dbo].[M_SKU_SelectByJanCD_ForTenzikaishouhin]
	-- Add the parameters for the stored procedure here
	@JanCD as varchar(13),
	@vendorcd as varchar(13)
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
		PriceWithTax as SiireTanka,
		--(
		--Select PriceWithoutTax
		--from M_JANOrderPrice
		--where VendorCD=@vendorcd
		--and ChangeDate <= getdate()
		
		--) as  SiireTanka,
		PriceOutTax as JoudaiTanka
		
	from M_SkU
	where JANCD=@JanCD
	AND SetKBN='0'
	AND ChangeDate <=getdate()
END
