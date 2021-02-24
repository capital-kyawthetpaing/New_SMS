IF OBJECT_ID ( 'M_ITEM_SelectForShiireTanka', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_ITEM_SelectForShiireTanka]
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
--            ITEMマスタチェック          --
--                                        --
--****************************************--
CREATE PROCEDURE M_ITEM_SelectForShiireTanka(
    @ITemCD varchar(30),
    @ChangeDate varchar(10),
    @DeleteFlg varchar(1)
)AS
BEGIN
    SET NOCOUNT ON;

    SELECT Top 1 MI.ITemCD
          ,CONVERT(varchar,MI.ChangeDate,111) AS ChangeDate
          ,MI.ItemName
          ,MI.BrandCD
          ,BR.BrandName
          ,MI.MakerItem
          ,MI.SportsCD
          ,SP.Char1 AS SportsName
          ,MI.SegmentCD
          ,SG.Char1 AS SegmentName
          ,MI.PriceOutTax
          ,MI.LastYearTerm
          ,MI.LastSeason
          ,MI.DeleteFlg       
    FROM M_ITEM MI
    LEFT JOIN M_Brand BR ON MI.BrandCD = BR.BrandCD
    LEFT JOIN M_MultiPorpose SP ON MI.SportsCD = SP.[Key]
                               AND '202' = SP.ID
    LEFT JOIN M_MultiPorpose SG ON MI.SegmentCD = SG.[Key]
                               AND '203' = SG.ID
    WHERE MI.ITemCD = (CASE WHEN @ITemCD <> '' THEN @ITemCD ELSE MI.ITemCD END)
      AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
      AND (@DeleteFlg IS NULL OR MI.DeleteFlg = @DeleteFlg)
    ORDER BY MI.ChangeDate DESC
    ;
END

