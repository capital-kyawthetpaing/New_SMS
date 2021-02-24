IF OBJECT_ID ( 'M_ItemOrderPrice_SelectFromSKU', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_ItemOrderPrice_SelectFromSKU]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    仕入先別発注単価マスタ
--       Program ID      MasterTouroku_HacchuuPrice
--       Create date:    2020.05.11
--    ======================================================================

--****************************************--
--                                        --
--            データ抽出(SKUより）        --
--                                        --
--****************************************--
CREATE PROCEDURE M_ItemOrderPrice_SelectFromSKU
    (@VendorCD              varchar(13),
     @StoreCD               varchar(4),
     @DispKbn               tinyint,
     @BaseDate              varchar(10),
     @BrandCD               varchar(6),
     @SportsCD              varchar(6),
     @SegmentCD             varchar(6),
     @LastYearTerm          varchar(6),
     @LastSeason            varchar(6),
     @ChangeDate            varchar(10),
     @MakerItem             varchar(30)
    )AS
BEGIN

    SET NOCOUNT ON;
    
    IF @DispKbn = 1
    BEGIN
    	WITH PRICE AS (
            SELECT VendorCD, StoreCD, AdminNO, MAX(ChangeDate) AS ChangeDate
              FROM M_JANOrderPrice MJP
             WHERE VendorCD = @VendorCD
               AND StoreCD = @StoreCD
               AND ChangeDate <= @BaseDate
             GROUP BY VendorCD, StoreCD, AdminNO
        )
       ,SKU AS (
            SELECT PR.AdminNO, MAX(PR.ChangeDate) AS ChangeDate, MAX(MS.ChangeDate) AS SKUChangeDate
              FROM PRICE PR
             INNER JOIN M_SKU MS ON PR.AdminNO = MS.AdminNO
                                AND PR.ChangeDate >= MS.ChangeDate
             GROUP BY PR.AdminNO
        )        
        SELECT 0 AS Chk
              ,MJP.VendorCD
              ,MJP.StoreCD
              ,MS.ITEMCD
              ,( SELECT TOP 1 ITemName
                   FROM M_ITEM X 
                  WHERE X.ITEMCD = MS.ITEMCD
                    AND X.ChangeDate <= MJP.ChangeDate
                   ORDER BY X.ChangeDate DESC
                ) AS ITemName
              ,MJP.AdminNO
              ,MJP.SKUCD
              ,MS.SizeName
              ,MS.ColorName
              ,MS.MakerItem
              ,MS.BrandCD
              ,MS.SportsCD
              ,MS.SegmentCD
              ,MS.LastYearTerm
              ,MS.LastSeason
              ,CONVERT(varchar,MJP.ChangeDate,111) AS ChangeDate
              ,MJP.Rate
              ,( SELECT TOP 1 PriceOutTax
                   FROM M_ITEM X 
                  WHERE X.ITEMCD = MS.ITEMCD
                    AND X.ChangeDate <= MJP.ChangeDate
                   ORDER BY X.ChangeDate DESC
                ) AS PriceOutTax
              ,MJP.PriceWithoutTax
              ,MJP.InsertOperator
              ,MJP.InsertDateTime
              ,MJP.UpdateOperator
              ,MJP.UpdateDateTime
              ,MJP.Rate AS OldRate
              ,'0' AS DelFlg
              ,ROW_NUMBER() OVER(ORDER BY MS.BrandCD,MS.SportsCD,MS.SegmentCD,MS.LastYearTerm,MS.LastSeason,MS.MakerItem,MS.ITEMCD,MJP.AdminNO,MJP.ChangeDate) AS TempKey
        FROM PRICE
        INNER JOIN M_JANOrderPrice MJP ON PRICE.VendorCD = MJP.VendorCD
                                      AND PRICE.StoreCD = MJP.StoreCD
                                      AND PRICE.AdminNO = MJP.AdminNO
                                      AND PRICE.ChangeDate = MJP.ChangeDate
        INNER JOIN SKU ON MJP.AdminNO = SKU.AdminNO
                      AND MJP.ChangeDate = SKU.ChangeDate
        INNER JOIN M_SKU MS ON MS.AdminNO = SKU.AdminNO
                           AND MS.ChangeDate = SKU.SKUChangeDate
        WHERE ISNULL(MS.BrandCD,'') = (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MS.BrandCD,'') END)
          AND ISNULL(MS.SportsCD,'') = (CASE WHEN @SportsCD <> '' THEN @SportsCD ELSE ISNULL(MS.SportsCD,'') END)
          AND ISNULL(MS.SegmentCD,'') = (CASE WHEN @SegmentCD <> '' THEN @SegmentCD ELSE ISNULL(MS.SegmentCD,'') END)
          AND ISNULL(MS.LastYearTerm,'') = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE ISNULL(MS.LastYearTerm,'') END)
          AND ISNULL(MS.LastSeason,'') = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE ISNULL(MS.LastSeason,'') END)
          AND MJP.ChangeDate = (CASE WHEN @ChangeDate <> '' THEN @ChangeDate ELSE MJP.ChangeDate END)
          AND ISNULL(MS.MakerItem,'') = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE ISNULL(MS.MakerItem,'') END)
        ORDER BY MS.BrandCD
                ,MS.SportsCD
                ,MS.SegmentCD
                ,MS.LastYearTerm
                ,MS.LastSeason
                ,MS.MakerItem
                ,MS.ITEMCD
                ,MJP.AdminNO
                ,MJP.ChangeDate
        ;    
    END
    ELSE
    BEGIN
        WITH SKU AS (
            SELECT MJP.AdminNO, MJP.ChangeDate, MAX(MS.ChangeDate) AS SKUChangeDate
              FROM M_JANOrderPrice MJP
             INNER JOIN M_SKU MS ON MJP.AdminNO = MS.AdminNO
                                AND MJP.ChangeDate >= MS.ChangeDate
             WHERE VendorCD = @VendorCD
               AND StoreCD = @StoreCD
             GROUP BY MJP.AdminNO, MJP.ChangeDate
        )
        SELECT 0 AS Chk
              ,MJP.VendorCD
              ,MJP.StoreCD
              ,MS.ITEMCD
              ,( SELECT TOP 1 ITemName
                   FROM M_ITEM X 
                  WHERE X.ITEMCD = MS.ITEMCD
                    AND X.ChangeDate <= MJP.ChangeDate
                   ORDER BY X.ChangeDate DESC
                ) AS ITemName
              ,MJP.AdminNO
              ,MJP.SKUCD
              ,MS.SizeName
              ,MS.ColorName
              ,MS.MakerItem
              ,MS.BrandCD
              ,MS.SportsCD
              ,MS.SegmentCD
              ,MS.LastYearTerm
              ,MS.LastSeason
              ,CONVERT(varchar,MJP.ChangeDate,111) AS ChangeDate
              ,MJP.Rate
              ,( SELECT TOP 1 PriceOutTax
                   FROM M_ITEM X 
                  WHERE X.ITEMCD = MS.ITEMCD
                    AND X.ChangeDate <= MJP.ChangeDate
                   ORDER BY X.ChangeDate DESC
                ) AS PriceOutTax
              ,MJP.PriceWithoutTax
              ,MJP.InsertOperator
              ,MJP.InsertDateTime
              ,MJP.UpdateOperator
              ,MJP.UpdateDateTime
              ,MJP.Rate AS OldRate
              ,'0' AS DelFlg
              ,ROW_NUMBER() OVER(ORDER BY MS.BrandCD,MS.SportsCD,MS.SegmentCD,MS.LastYearTerm,MS.LastSeason,MS.MakerItem,MS.ITEMCD,MJP.AdminNO,MJP.ChangeDate) AS TempKey
        FROM M_JANOrderPrice MJP
        INNER JOIN SKU ON MJP.AdminNO = SKU.AdminNO
                      AND MJP.ChangeDate = SKU.ChangeDate
        INNER JOIN M_SKU MS ON MS.AdminNO = SKU.AdminNO
                           AND MS.ChangeDate = SKU.SKUChangeDate
        WHERE MJP.VendorCD = @VendorCD
          AND MJP.StoreCD = @StoreCD
          AND ISNULL(MS.BrandCD,'') = (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MS.BrandCD,'') END)
          AND ISNULL(MS.SportsCD,'') = (CASE WHEN @SportsCD <> '' THEN @SportsCD ELSE ISNULL(MS.SportsCD,'') END)
          AND ISNULL(MS.SegmentCD,'') = (CASE WHEN @SegmentCD <> '' THEN @SegmentCD ELSE ISNULL(MS.SegmentCD,'') END)
          AND ISNULL(MS.LastYearTerm,'') = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE ISNULL(MS.LastYearTerm,'') END)
          AND ISNULL(MS.LastSeason,'') = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE ISNULL(MS.LastSeason,'') END)
          AND MJP.ChangeDate = (CASE WHEN @ChangeDate <> '' THEN @ChangeDate ELSE MJP.ChangeDate END)
          AND ISNULL(MS.MakerItem,'') = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE ISNULL(MS.MakerItem,'') END)
        ORDER BY MS.BrandCD
                ,MS.SportsCD
                ,MS.SegmentCD
                ,MS.LastYearTerm
                ,MS.LastSeason
                ,MS.MakerItem
                ,MS.ITEMCD
                ,MJP.AdminNO
                ,MJP.ChangeDate
        ;
    END
    
END

