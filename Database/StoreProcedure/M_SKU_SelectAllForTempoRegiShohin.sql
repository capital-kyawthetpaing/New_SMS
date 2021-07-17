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
    IF @JanCD IS NOT NULL
    BEGIN
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
              ,MPt.Char1 AS TaniName
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

        LEFT OUTER JOIN M_Brand AS MB
        ON  MB.BrandCD= MS.BrandCD
 
        LEFT OUTER JOIN M_MultiPorpose MPt
        ON  MPt.ID = 201
        AND MPt.[KEY] = MS.TaniCD

        WHERE MS.DeleteFlg = 0

        AND EXISTS (SELECT A.ITemCD FROM F_SKU(@ChangeDate) AS A
                    WHERE A.DeleteFlg = 0
                    AND   A.ITemCD = MS.ITemCD
				    AND   A.JanCD = @JanCD

                    AND   (@BrandCD is null  OR A.BrandCD=@BrandCD)
				    AND   (@VendorCD is null  OR A.MainVendorCD=@VendorCD)
				    AND   (@SportsCD is null  OR A.SportsCD=@SportsCD)
				    AND   (@SKUName is null or A.SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE A.SKUName END) + '%')
                    )
        ORDER BY MS.SKUName, MS.JANCD
    END
    ELSE
    BEGIN
        ---------------------------
        -- create temp table
        ---------------------------
        CREATE TABLE #tmp_SelectAllForTempoRegiShohin (ItemCD varchar(30)collate database_default)
        CREATE INDEX IX_#tmp_SelectAllForTempoRegiShohin on #tmp_SelectAllForTempoRegiShohin (ItemCD)

        IF @BrandCD IS NOT NULL
        BEGIN
            INSERT INTO #tmp_SelectAllForTempoRegiShohin
            SELECT DISTINCT ITemCD
            FROM F_SKU(@ChangeDate)
            WHERE DeleteFlg = 0
            AND   ChangeDate <= CONVERT(date, @ChangeDate)
            AND   BrandCD=@BrandCD

            AND   (@JanCD is null OR JanCD = @JanCD)
			AND   (@VendorCD is null OR MainVendorCD=@VendorCD)
			AND   (@SportsCD is null OR SportsCD=@SportsCD)
			AND   (@SKUName is null OR SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE SKUName END) + '%')
        END
        ELSE
        BEGIN
            INSERT INTO #tmp_SelectAllForTempoRegiShohin
            SELECT DISTINCT ITemCD
            FROM F_SKU(@ChangeDate)
            WHERE DeleteFlg = 0
            AND   ChangeDate <= CONVERT(date, @ChangeDate)

            AND   (@BrandCD is null OR BrandCD=@BrandCD)
            AND   (@JanCD is null OR JanCD = @JanCD)
			AND   (@VendorCD is null OR MainVendorCD=@VendorCD)
			AND   (@SportsCD is null OR SportsCD=@SportsCD)
			AND   (@SKUName is null OR SKUName LIKE '%' + (CASE WHEN @SKUName <> '' THEN @SKUName ELSE SKUName END) + '%')
        END
--select * from #tmp_SelectAllForTempoRegiShohin
--return
        ---------------------------
        -- select data
        ---------------------------
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
                ,MPt.Char1 AS TaniName
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

        FROM F_SKU(@ChangeDate) MS

        INNER JOIN #tmp_SelectAllForTempoRegiShohin tmp
        ON tmp.ItemCD = MS.ITemCD
        AND MS.DeleteFlg = 0
    
        LEFT OUTER JOIN M_Brand AS MB
        ON  MB.BrandCD= MS.BrandCD

        LEFT OUTER JOIN M_MultiPorpose MPt
        ON  MPt.ID = 201
        AND MPt.[KEY] = MS.TaniCD

        WHERE MS.DeleteFlg = 0

        ORDER BY MS.SKUName, MS.JANCD
    END
END


GO

