 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_SKU_SelectAll]    */

CREATE PROCEDURE M_SKU_SelectAll(
    @AdminNO int,
    @SKUCD varchar(30),
    @JanCD varchar(13),
    @SetKBN varchar(1),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

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
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.[ID]= 210 AND A.[KEY]= MS.BrandCD) AS BrandName
          ,MS.MakerItem
          ,MS.TaniCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.[ID]= 201 AND A.[KEY]= MS.TaniCD) AS TaniName
          ,MS.SportsCD
          ,MS.ZaikoKBN
          ,MS.Rack
          ,MS.VirtualFlg
          ,MS.DirectFlg
          ,MS.ReserveCD
          ,MS.NoticesCD
          ,MS.PostageCD
          ,MS.ManufactCD
          ,MS.ConfirmCD
          ,MS.WebStockFlg
          ,MS.StopFlg
          ,MS.DiscontinueFlg
          ,MS.InventoryAddFlg
          ,MS.MakerAddFlg
          ,MS.StoreAddFlg
          ,MS.NoNetOrderFlg
          ,MS.EDIOrderFlg
          ,MS.CatalogFlg
          ,MS.ParcelFlg
          ,MS.AutoOrderFlg
          ,MS.TaxRateFLG
          ,MS.CostingKBN
          ,MS.SaleExcludedFlg
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
          ,MS.Rate
          ,MS.SaleStartDate
          ,MS.WebStartDate
          ,MS.OrderAttentionCD
          ,MS.OrderAttentionNote
          ,MS.CommentInStore
          ,MS.CommentOutStore
          ,MS.LastYearTerm
          ,MS.LastSeason
          ,MS.LastCatalogNO
          ,MS.LastCatalogPage
          ,MS.LastCatalogText
          ,MS.LastInstructionsNO
          ,MS.LastInstructionsDate
          ,MS.WebAddress
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
        AND M.AdminNO = (CASE WHEN @AdminNO IS NULL THEN M.AdminNO ELSE @AdminNO END)
        AND M.SetKBN = (CASE WHEN ISNULL(@SetKBN,'') = '' THEN M.SetKBN ELSE CONVERT(tinyint,@SetKBN) END)
        AND M.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND M.DeleteFlg = 0
        GROUP BY M.SKUCD)M
        ON M.SKUCD = MS.SKUCD AND M.ChangeDate = MS.ChangeDate

    WHERE MS.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE MS.JanCD END)
    AND MS.SKUCD = (CASE WHEN @SKUCD <> '' THEN @SKUCD ELSE MS.SKUCD END)
    AND MS.AdminNO = (CASE WHEN @AdminNO IS NULL THEN MS.AdminNO ELSE @AdminNO END)
    AND MS.SetKBN = (CASE WHEN ISNULL(@SetKBN,'') = '' THEN MS.SetKBN ELSE CONVERT(tinyint,@SetKBN) END)
    AND MS.DeleteFlg = 0
    ORDER BY MS.SKUCD, MS.AdminNO
    ;
    
END


