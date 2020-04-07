 BEGIN TRY 
 Drop Procedure dbo.[M_MakerBrand_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [M_MakerBrand_Select]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_MakerBrand_Select]
	-- Add the parameters for the stored procedure here
	@BrandCD varchar(6),
	@MakerCD varchar(13),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  top 1 MS.BrandCD
    	  ,MS.MakerCD
          ,CONVERT(varchar, MS.ChangeDate,111) AS ChangeDate
          ,(SELECT M.BrandName FROM M_Brand M WHERE M.BrandCD = MS.BrandCD) AS BrandName
          ,MS.IrregularKBN
          ,MS.DataSourseMakerCD
          ,MS.PatternCD
          ,MS.DeleteFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    FROM M_MakerBrand MS
    WHERE MS.BrandCD = @BrandCD
    AND MS.MakerCD = CASE WHEN @MakerCD <> '' THEN @MakerCD ELSE MS.MakerCD END
    AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    ORDER BY MS.ChangeDate desc
END



