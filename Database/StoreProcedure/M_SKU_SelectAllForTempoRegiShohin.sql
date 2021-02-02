/****** Object:  StoredProcedure [dbo].[M_SKU_SelectAllForTempoRegiShohin]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[M_SKU_SelectAllForTempoRegiShohin]
GO

/****** Object:  StoredProcedure [dbo].[M_SKU_SelectAllForTempoRegiShohin]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_SKU_SelectAllForTempoRegiShohin]    */

CREATE PROCEDURE [dbo].[M_SKU_SelectAllForTempoRegiShohin](
    @BrandCD varchar(20),
	@SportsCD varchar(20),
    @SKUName varchar(80),
    @JanCD varchar(13),
	@VendorCD varchar(20),
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
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
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
          ,MB.BrandKana
          ,MB.BrandName

    from F_SKU(@ChangeDate) MS
    /*INNER JOIN(SELECT M.AdminNO, MAX(M.ChangeDate) AS ChangeDate
        FROM M_SKU M
        WHERE --M.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE M.JanCD END)
        --AND M.SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE M.SKUName END) + '%'
        --AND 
        	M.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND M.DeleteFlg = 0
        GROUP BY M.AdminNO)M
        ON M.AdminNO = MS.AdminNO AND M.ChangeDate = MS.ChangeDate
        */
    LEFT OUTER JOIN M_Brand AS MB
    ON  MB.BrandCD= MS.BrandCD
    
    WHERE /*(MS.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE MS.JanCD END)
    AND MS.SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE MS.SKUName END) + '%'
    AND ISNULL(MB.BrandKana,'') LIKE '%' + (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MB.BrandKana,'') END) + '%'
    AND */
    MS.DeleteFlg = 0
    --)
    --OR
    AND EXISTS (SELECT A.ITemCD FROM F_SKU(@ChangeDate) AS A
                        /*LEFT OUTER JOIN M_Brand AS MB  
                        --ON  MB.BrandCD= A.BrandCD  */ --close for task 2522
                         WHERE (@BrandCD is null  OR (A.BrandCD=@BrandCD))
							And (@VendorCD is null  OR (A.MainVendorCD=@VendorCD))
							And (@SportsCD is null  OR (A.SportsCD=@SportsCD))
							And (A.JanCD is null or A.JanCD = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE A.JanCD END))
							AND (A.SKUName is null or A.SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE A.SKUName END) + '%')
							--AND ISNULL(MB.BrandKana,'') LIKE '%' + (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MB.BrandKana,'') END) + '%'  --close for task 2522
							AND A.DeleteFlg = 0
							AND A.ITemCD = MS.ITemCD
                         )
    ORDER BY MS.SKUName, MS.JANCD
    ;
    
END


GO

