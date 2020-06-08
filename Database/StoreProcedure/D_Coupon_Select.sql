SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ ポイント引換券印刷
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.22
--       Update date:    2020.06.06  ChangeDateが最新のデータを取得するように修正
--  ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_Coupon_Select')
  DROP PROCEDURE [dbo].[D_Coupon_Select]
GO


CREATE PROCEDURE [dbo].[D_Coupon_Select]
(
    @StoreCD varchar(4)
)AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT TicketUnit
          ,Print1
          ,Bold1
          ,Size1
          ,Print2
          ,Bold2
          ,Size2
          ,Print3
          ,Bold3
          ,Size3
          ,Print4
          ,Bold4
          ,Size4
          ,Print5
          ,Bold5
          ,Size5
          ,Print6
          ,Bold6
          ,Size6
          ,Print7
          ,Bold7
          ,Size7
          ,Print8
          ,Bold8
          ,Size8
          ,Print9
          ,Bold9
          ,Size9
          ,Print10
          ,Bold10
          ,Size10
          ,Print11
          ,Bold11
          ,Size11
          ,Print12
          ,Bold12
          ,Size12
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) RANK
                  ,TicketUnit
                  ,Print1
                  ,Bold1
                  ,Size1
                  ,Print2
                  ,Bold2
                  ,Size2
                  ,Print3
                  ,Bold3
                  ,Size3
                  ,Print4
                  ,Bold4
                  ,Size4
                  ,Print5
                  ,Bold5
                  ,Size5
                  ,Print6
                  ,Bold6
                  ,Size6
                  ,Print7
                  ,Bold7
                  ,Size7
                  ,Print8
                  ,Bold8
                  ,Size8
                  ,Print9
                  ,Bold9
                  ,Size9
                  ,Print10
                  ,Bold10
                  ,Size10
                  ,Print11
                  ,Bold11
                  ,Size11
                  ,Print12
                  ,Bold12
                  ,Size12
              FROM M_StorePoint
             WHERE StoreCD = @StoreCD
               AND ChangeDate <= CONVERT (date, SYSDATETIME())
               AND DeleteFlg = 0) storePoint
     WHERE storePoint.RANK = 1
         ;
END
