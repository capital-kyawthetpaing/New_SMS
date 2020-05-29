BEGIN TRY 
 Drop Procedure [dbo].[M_ITem_ItemNandPriceoutTax_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





 Create PROCEDURE [dbo].[M_ITem_ItemNandPriceoutTax_Select]
	-- Add the parameters for the stored procedure here
	  @ITemCD as varchar(30),
    @AddDate  as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	SELECT ItemName,PriceOutTax
	FROM M_Item
	WHERE ItemCD=@ITemCD
	AND ChangeDate <= @AddDate
END
GO
