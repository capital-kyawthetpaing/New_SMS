 BEGIN TRY 
 Drop Procedure dbo.[M_JANOrderPrice_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_JANOrderPrice_Select]    */
CREATE PROCEDURE [dbo].[M_JANOrderPrice_Select](
    -- Add the parameters for the stored procedure here
    @JanCD varchar(13),
    @VendorCD varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 MS.JanCD
          ,MS.VendorCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.Rate
          ,MS.PriceWithoutTax
          ,MS.Remarks
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime

    from M_JANOrderPrice MS
    
    WHERE MS.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE MS.JanCD END)
    AND MS.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE MS.VendorCD END)
    AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)     
    ORDER BY MS.ChangeDate desc
    ;
END


