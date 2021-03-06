 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectForSKSMasterUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_SKU_SelectForSKSMasterUpdate] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select 
	ms.AdminNO as Item_AdminCode,
	ms.ITemCD as Item_Code,
	ms.ColorName as Color_Name,
	ms.SizeName as Size_Name,
	ColorNO as Color_Code,
	SizeNO as Size_Code,
	'NULL' as Color_Name_Official,
	'NULL' as Size_Name_Official,
	JanCD as JAN_Code

	from M_SKU as ms 
	Left outer join M_ITem as mi on ms.ITemCD=mi.ITemCD
	where  mi.SKSUpdateFlg=1
		OR ms.SKSUpdateFlg=1
	order by ms.SizeNO,ms.ColorNO

END
