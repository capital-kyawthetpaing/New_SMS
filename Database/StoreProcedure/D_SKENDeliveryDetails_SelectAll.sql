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
	@SKENBangouA  int,
	@ChkFlg tinyInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	dskenDetail.SKENNouhinshoNO,
	left(dskenDetail.SKENSyouhinmei,11) as SKENSyouhinmei,
	dskenDetail.SKENNouhinHinban,
	dskenDetail.SKENJanCD,
	fsku.SKUName,
	fsku.ColorName,
	fsku.SizeName,
	dskenDetail.SKENNouhinSuu,
	dskenDetail.ErrorText,
	dskend.ErrorSu,
	dskenDetail.ErrorKBN,
	SKENBangouB
	into #tempSKEN
	from D_SKENDelivery as dskend
	inner join D_SKENDeliveryDetails  as dskenDetail on dskend.SKENNouhinshoNO=dskenDetail.SKENNouhinshoNO
														and dskend.ImportDateTime=dskenDetail.ImportDateTime
	left outer join F_SKU(getdate()) fsku on fsku.AdminNO=dskenDetail.SKENAdminNO
	where dskend.SKENBangouA=@SKENBangouA
	and  ( (@Chkflg = 1 and (dskend.ErrorSu <> 0 and dskenDetail.ErrorKBN<>0)) 
		OR
		 (@Chkflg = 0 and (dskend.ErrorSu = 0))
		 OR
		 (@Chkflg = 2 and (dskend.ErrorSu = 0  or (dskend.ErrorSu <> 0 and dskenDetail.ErrorKBN<>0)))
	)

	If @ChkFlg=2
	begin
		select SKENNouhinshoNO,
		SKENSyouhinmei,
		SKENNouhinHinban,
		SKENJanCD,
		SKUName,
		ColorName,
		SizeName,
		SKENNouhinSuu,
		ErrorText
		from #tempSKEN
		where (ErrorSu = 0  or (ErrorSu <> 0 and ErrorKBN<>0))
		order by SKENBangouB desc
	end

	If @ChkFlgE=1
	begin
		select SKENNouhinshoNO,
		SKENSyouhinmei,
		SKENNouhinHinban,
		SKENJanCD,
		SKUName,
		ColorName,
		SizeName,
		SKENNouhinSuu,
		ErrorText
		from #tempSKEN
		where (@Chkflg = 1 and (ErrorSu <> 0 and ErrorKBN<>0))
		order by SKENBangouB desc
	end

	If @ChkFlg=0
	begin
		select SKENNouhinshoNO,
		SKENSyouhinmei,
		SKENNouhinHinban,
		SKENJanCD,
		SKUName,
		ColorName,
		SizeName,
		SKENNouhinSuu,
		ErrorText
		from #tempSKEN
		where (@Chkflg = 0 and (ErrorSu = 0 and ErrorKBN=0))
		order by SKENBangouB desc
	end


	drop table #tempSKEN


END


