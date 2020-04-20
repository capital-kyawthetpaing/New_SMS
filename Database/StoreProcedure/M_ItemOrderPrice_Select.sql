 BEGIN TRY 
 Drop Procedure dbo.[M_ItemOrderPrice_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_ItemOrderPrice_Select]    */
CREATE PROCEDURE M_ItemOrderPrice_Select(
    -- Add the parameters for the stored procedure here
    @MakerItem varchar(30),
    @VendorCD varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 MS.MakerItem
          ,MS.VendorCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.Rate
          ,MS.PriceWithoutTax
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime

    from M_ItemOrderPrice MS
    
    WHERE MS.MakerItem = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE MS.MakerItem END)
    AND MS.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE MS.VendorCD END)
    AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)     
    ORDER BY MS.ChangeDate desc
    ;
END

