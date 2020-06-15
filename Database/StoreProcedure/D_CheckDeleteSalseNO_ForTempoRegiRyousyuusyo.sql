SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 領収書印刷　削除済みお買上番号チェック
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2020.06.13
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo')
  DROP PROCEDURE [dbo].[D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo]
GO

CREATE PROCEDURE [dbo].[D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo]
    (
     @SalesNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(sales.SalesNo) DeleteSalesNoCount
      FROM D_Sales sales
     WHERE sales.SalesNo = @SalesNO
       AND sales.DeleteDateTime IS NOT NULL
         ;
END
