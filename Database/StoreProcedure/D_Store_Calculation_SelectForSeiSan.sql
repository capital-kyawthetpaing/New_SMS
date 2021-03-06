 BEGIN TRY 
 Drop Procedure dbo.[D_Store_Calculation_SelectForSeiSan]
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
CREATE PROCEDURE [dbo].[D_Store_Calculation_SelectForSeiSan]
	-- Add the parameters for the stored procedure here
	 @StoreCD as VarChar(6) ,
	 @Date as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	dsc.[10000yen] as Yen10000,
	dsc.[5000yen] as Yen5000,
	dsc.[2000yen] as Yen2000,
	dsc.[1000yen] as Yen1000,
	dsc.[500yen] as Yen500,
	dsc.[100yen] as Yen100,
	dsc.[50yen] as Yen50,
	dsc.[10yen] as Yen10,
	dsc.[5yen] as Yen5,
	dsc.[1yen] as Yen1,
	dsc.Etcyen as OtherYen,
	IsNull(FORMAT(Convert(Int,dsc.[10000yen] * 10000 + dsc.[5000yen] *5000 + dsc.[2000yen] * 2000 + dsc.[1000yen] * 1000 + dsc.[500yen] * 500 + dsc.[100yen] * 100 + dsc.[50yen] * 50 + dsc.[10yen] *10 + dsc.[5yen] * 5 + dsc.[1yen] * 1 + dsc.Etcyen), '#,#'),0) as TotalCash
	
	From D_StoreCalculation dsc
	Where dsc.CalculationDate = @Date and dsc.StoreCD = @StoreCD
END
