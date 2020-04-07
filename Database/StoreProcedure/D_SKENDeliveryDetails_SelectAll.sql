 BEGIN TRY 
 Drop Procedure dbo.[D_SKENDeliveryDetails_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[D_SKENDeliveryDetails_SelectAll]
	-- Add the parameters for the stored procedure here
	@SKENNouhinshoNO  varchar(10),
	@ChkFlg varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	dskenDetail.SKENNouhinshoNO,
	dskenDetail.SKENHacchuu,
	dskenDetail.SKENNouhinHinban,
	dskenDetail.SKENJanCD,
	fsku.SKUName,
	fsku.ColorName,
	fsku.SizeName,
	dskenDetail.SKENNouhinSuu,
	dskenDetail.ErrorText
	from D_SKENDelivery as dskend
	inner join D_SKENDeliveryDetails  as dskenDetail on dskend.SKENNouhinshoNO=dskenDetail.SKENNouhinshoNO
	cross apply F_SKU(dskend.SKENJuchuuDate) fsku 
	where fsku.AdminNO=dskenDetail.AdminNO
	and dskend.SKENNouhinshoNO=@SKENNouhinshoNO
	and ((@Chkflg is null or ( (@Chkflg = 1 or (dskend.ErrorSu <> 0)) ))
	or (@Chkflg is null or ( (@Chkflg = 0 or (dskend.ErrorSu = 0)) )))

	order by dskenDetail.SKENBangouB desc

END

