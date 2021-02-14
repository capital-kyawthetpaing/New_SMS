IF OBJECT_ID ( 'M_SKU_SelectForShiireTanka', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_SKU_SelectForShiireTanka]
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
--            SKUマスタチェック           --
--                                        --
--****************************************--
CREATE PROCEDURE M_SKU_SelectForShiireTanka(
    @MakerItem varchar(30),
    @AdminNO int,
    @ChangeDate varchar(10)
)AS
BEGIN
    SET NOCOUNT ON;

    SELECT MS.AdminNO
          ,MS.SKUCD
          ,MS.SizeName
          ,MS.ColorName
          ,MS.MakerItem
          ,MS.BrandCD
          ,MS.SportsCD
          ,MS.SegmentCD
          ,MS.LastYearTerm      
          ,MS.LastSeason  
          ,MS.ITemCD
          ,MI.ItemName
          ,MI.PriceOutTax
    FROM F_SKU(CONVERT(DATE, @ChangeDate)) MS
    LEFT JOIN F_ITEM(CONVERT(DATE, @ChangeDate)) MI ON MS.ITEMCD = MI.ITEMCD
    WHERE ISNULL(MS.MakerItem,'') = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE ISNULL(MS.MakerItem,'') END)
      AND MS.AdminNO = (CASE WHEN @AdminNO <> 0 THEN @AdminNO ELSE MS.AdminNO END)
      AND MS.DeleteFlg = 0
      AND MI.DeleteFlg = 0
    ;
END

