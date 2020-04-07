 BEGIN TRY 
 Drop Procedure dbo.[D_Exclusive_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Exclusive_SelectAll]    */

CREATE PROCEDURE [dbo].[D_Exclusive_SelectAll](
    @SKUCD varchar(30),
    @JanCD varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
/*
    -- Insert statements for procedure here
    SELECT MS.AdminNO
    	  ,MS.SKUCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.VariousFLG
          ,MS.SKUName
          ,MS.SKUShortName
          ,MS.ITemCD
          ,MS.ColorNO
          ,MS.SizeNO
          ,MS.JanCD
          ,MS.SetKBN
          ,MS.PresentKBN
          ,MS.SampleKBN
          ,MS.DiscountKBN
          ,MS.ColorName
          ,MS.SizeName
          ,MS.MainVendorCD
          ,MS.MakerVendorCD
          ,MS.BrandCD
          ,MS.MakerItem
          ,MS.TaniCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.[ID]= 201 AND A.[KEY]= MS.TaniCD) AS TaniName
          ,MS.SportsCD
          ,MS.ClassificationA
          ,MS.ClassificationB
          ,MS.ClassificationC
          ,MS.ZaikoKBN
          ,MS.Rack
          ,MS.TaxRateFLG
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
          ,MS.CommentInStore
          ,MS.CommentOutStore
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
          ,M.CNT AS CountSKU

    from M_SKU MS
    INNER JOIN(SELECT M.SKUCD, MAX(M.ChangeDate) AS ChangeDate, COUNT(*) OVER() CNT
        FROM M_SKU M
        WHERE M.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE M.JanCD END)
        AND M.ChangeDate <= CONVERT(DATE, @ChangeDate)      --一致データのみ
        GROUP BY M.SKUCD)M
        ON M.SKUCD = MS.SKUCD AND M.ChangeDate = MS.ChangeDate

    WHERE MS.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE MS.JanCD END)
    AND MS.SKUCD = (CASE WHEN @SKUCD <> '' THEN @SKUCD ELSE MS.SKUCD END)
        
    ORDER BY MS.SKUCD
    ;
    */
END


