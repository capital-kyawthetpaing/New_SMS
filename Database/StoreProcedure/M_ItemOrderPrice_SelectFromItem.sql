IF OBJECT_ID ( 'M_ItemOrderPrice_SelectFromItem', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_ItemOrderPrice_SelectFromItem]
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
--            データ抽出(ITEMより）       --
--                                        --
--****************************************--
CREATE PROCEDURE M_ItemOrderPrice_SelectFromItem
    (@VendorCD              varchar(13),
     @StoreCD               varchar(4),
     @DispKbn               tinyint,      -- 1:現状 2:履歴
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
            SELECT VendorCD, StoreCD, MakerItem, MAX(ChangeDate) AS ChangeDate
              FROM M_ItemOrderPrice
             WHERE VendorCD = @VendorCD
               AND StoreCD = @StoreCD
               AND ChangeDate <= @BaseDate
             GROUP BY VendorCD, StoreCD, MakerItem
        )
        ,ITM AS (
                SELECT MakerItem, ChangeDate, ItemChangeDate, ItemCD
                FROM (
                        SELECT PR.MakerItem, PR.ChangeDate, IT.ChangeDate AS ItemChangeDate, IT.ItemCD
                              , ROW_NUMBER() OVER(PARTITION BY PR.MakerItem, PR.ChangeDate ORDER BY IT.ChangeDate DESC , IT.ItemCD) AS NUM
                          FROM PRICE PR
                          INNER JOIN M_ITEM IT ON PR.MakerItem = IT.MakerItem
                                              AND PR.ChangeDate >= IT.ChangeDate
                     ) SUB
                WHERE NUM = 1
        )
        SELECT 0 AS Chk
              ,MIP.VendorCD
              ,MIP.StoreCD
              ,MI.ITEMCD
              ,MI.ITemName
              ,MIP.MakerItem
              ,MI.BrandCD
              ,BR.BrandName
              ,MI.SportsCD
              ,SP.Char1 AS SportsName
              ,MI.SegmentCD
              ,SG.Char1 AS SegmentName
              ,MI.LastYearTerm
              ,MI.LastSeason
              ,CONVERT(varchar,MIP.ChangeDate,111) AS ChangeDate
              ,MIP.Rate
              ,MI.PriceOutTax
              ,MIP.PriceWithoutTax
              ,MIP.InsertOperator
              ,MIP.InsertDateTime
              ,MIP.UpdateOperator
              ,MIP.UpdateDateTime
              ,MIP.Rate AS OldRate
              ,'0' AS DelFlg
              ,ROW_NUMBER() OVER(ORDER BY MI.BrandCD,MI.SportsCD,MI.SegmentCD,MI.LastYearTerm,MI.LastSeason,MIP.MakerItem,MI.ITEMCD,MIP.ChangeDate) AS TempKey
        FROM PRICE
        INNER JOIN M_ItemOrderPrice MIP ON PRICE.VendorCD = MIP.VendorCD
                                       AND PRICE.StoreCD = MIP.StoreCD
                                       AND PRICE.MakerItem = MIP.MakerItem
                                       AND PRICE.ChangeDate = MIP.ChangeDate
        INNER JOIN ITM ON MIP.MakerItem = ITM.MakerItem
                      AND MIP.ChangeDate = ITM.ChangeDate
        INNER JOIN M_ITEM MI ON MI.MakerItem = ITM.MakerItem
                            AND MI.ITEMCD = ITM.ITEMCD
                            AND MI.ChangeDate = ITM.ItemChangeDate
        LEFT JOIN M_Brand BR ON MI.BrandCD = BR.BrandCD
        LEFT JOIN M_MultiPorpose SP ON MI.SportsCD = SP.[Key]
                                   AND '202' = SP.ID
        LEFT JOIN M_MultiPorpose SG ON MI.SegmentCD = SG.[Key]
                                   AND '203' = SG.ID
        WHERE ISNULL(MI.BrandCD,'') = (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MI.BrandCD,'') END)
          AND ISNULL(MI.SportsCD,'') = (CASE WHEN @SportsCD <> '' THEN @SportsCD ELSE ISNULL(MI.SportsCD,'') END)
          AND ISNULL(MI.SegmentCD,'') = (CASE WHEN @SegmentCD <> '' THEN @SegmentCD ELSE ISNULL(MI.SegmentCD,'') END)
          AND ISNULL(MI.LastYearTerm,'') = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE ISNULL(MI.LastYearTerm,'') END)
          AND ISNULL(MI.LastSeason,'') = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE ISNULL(MI.LastSeason,'') END)
          AND MIP.ChangeDate = (CASE WHEN @ChangeDate <> '' THEN @ChangeDate ELSE MIP.ChangeDate END)
          AND ISNULL(MIP.MakerItem,'') = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE ISNULL(MIP.MakerItem,'') END)          
        ORDER BY MI.BrandCD
                ,MI.SportsCD
                ,MI.SegmentCD
                ,MI.LastYearTerm
                ,MI.LastSeason
                ,MIP.MakerItem
                ,MI.ITEMCD
                ,MIP.ChangeDate
        ;
    
    END
    ELSE
    BEGIN
        
        WITH ITM AS (
                   SELECT MakerItem, ChangeDate, ItemChangeDate, ItemCD
                     FROM (
                        SELECT PR.MakerItem, PR.ChangeDate, IT.ChangeDate AS ItemChangeDate, IT.ItemCD
                             , ROW_NUMBER() OVER(PARTITION BY PR.MakerItem, PR.ChangeDate ORDER BY IT.ChangeDate DESC , IT.ItemCD) AS NUM
                        FROM M_ItemOrderPrice PR
                        INNER JOIN M_ITEM IT ON PR.MakerItem = IT.MakerItem
                                            AND PR.ChangeDate >= IT.ChangeDate
                        WHERE VendorCD = @VendorCD
                          AND StoreCD = @StoreCD
                          ) SUB 
                    WHERE NUM = 1
        )
        SELECT 0 AS Chk
              ,MIP.VendorCD
              ,MIP.StoreCD
              ,MI.ITEMCD
              ,MI.ITemName
              ,MIP.MakerItem
              ,MI.BrandCD
              ,BR.BrandName
              ,MI.SportsCD
              ,SP.Char1 AS SportsName
              ,MI.SegmentCD
              ,SG.Char1 AS SegmentName
              ,MI.LastYearTerm
              ,MI.LastSeason
              ,CONVERT(varchar,MIP.ChangeDate,111) AS ChangeDate
              ,MIP.Rate
              ,MI.PriceOutTax
              ,MIP.PriceWithoutTax
              ,MIP.InsertOperator
              ,MIP.InsertDateTime
              ,MIP.UpdateOperator
              ,MIP.UpdateDateTime
              ,MIP.Rate AS OldRate
              ,'0' AS DelFlg
              ,ROW_NUMBER() OVER(ORDER BY MI.BrandCD,MI.SportsCD,MI.SegmentCD,MI.LastYearTerm,MI.LastSeason,MIP.MakerItem,MI.ITEMCD,MIP.ChangeDate) AS TempKey
        FROM M_ItemOrderPrice MIP
        INNER JOIN ITM ON MIP.MakerItem = ITM.MakerItem
                      AND MIP.ChangeDate = ITM.ChangeDate
        INNER JOIN M_ITEM MI ON MI.MakerItem = ITM.MakerItem
                            AND MI.ItemCD = ITM.ItemCD
                            AND MI.ChangeDate = ITM.ItemChangeDate
        LEFT JOIN M_Brand BR ON MI.BrandCD = BR.BrandCD
        LEFT JOIN M_MultiPorpose SP ON MI.SportsCD = SP.[Key]
                                   AND '202' = SP.ID
        LEFT JOIN M_MultiPorpose SG ON MI.SegmentCD = SG.[Key]
                                   AND '203' = SG.ID
        WHERE MIP.VendorCD = @VendorCD
          AND MIP.StoreCD = @StoreCD
          AND ISNULL(MI.BrandCD,'') = (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MI.BrandCD,'') END)
          AND ISNULL(MI.SportsCD,'') = (CASE WHEN @SportsCD <> '' THEN @SportsCD ELSE ISNULL(MI.SportsCD,'') END)
          AND ISNULL(MI.SegmentCD,'') = (CASE WHEN @SegmentCD <> '' THEN @SegmentCD ELSE ISNULL(MI.SegmentCD,'') END)
          AND ISNULL(MI.LastYearTerm,'') = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE ISNULL(MI.LastYearTerm,'') END)
          AND ISNULL(MI.LastSeason,'') = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE ISNULL(MI.LastSeason,'') END)
          AND MIP.ChangeDate = (CASE WHEN @ChangeDate <> '' THEN @ChangeDate ELSE MIP.ChangeDate END)
          AND ISNULL(MIP.MakerItem,'') = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE ISNULL(MIP.MakerItem,'') END)          
        ORDER BY MI.BrandCD
                ,MI.SportsCD
                ,MI.SegmentCD
                ,MI.LastYearTerm
                ,MI.LastSeason
                ,MIP.MakerItem
                ,MI.ITEMCD
                ,MIP.ChangeDate
        ;
    END

END

