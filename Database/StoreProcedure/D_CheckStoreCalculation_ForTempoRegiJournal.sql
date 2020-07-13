SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ ジャーナル印刷 店舗積算データ有無チェック
--       Program ID      TempoRegiJournal
--       Create date:    2020.06.29
--  ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_CheckStoreCalculation_ForTempoRegiJournal')
  DROP PROCEDURE [dbo].[D_CheckStoreCalculation_ForTempoRegiJournal]
GO


CREATE PROCEDURE [dbo].[D_CheckStoreCalculation_ForTempoRegiJournal]
(
    @StoreCD   varchar(4),
    @DateFrom  varchar(10),
    @DateTo    varchar(10)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) DataNum
      FROM D_StoreCalculation
     WHERE StoreCD = @StoreCD
       AND CONVERT(varchar, CalculationDate, 111) >= @DateFrom
       AND CONVERT(varchar, CalculationDate, 111) <= @DateTo
     ;
END
