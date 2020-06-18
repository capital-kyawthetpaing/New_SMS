SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    “X•ÜƒŒƒW æˆøƒŒƒV[ƒgo—Í@’Ş‘K€”õî•ñæ“¾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ƒƒWƒbƒN•ÏX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectChangePreparation_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectChangePreparation_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectChangePreparation_ForTempoTorihikiReceipt]
(
    @StoreCD          varchar(4),
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 ˆ—ŠJn                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.Number
          ,D.RegistDate             -- “o˜^“ú
          ,'Œ»‹à' Name1             -- ’Ş‘K€”õ–¼1
          ,D.DepositGaku Amount1    -- ’Ş‘K€”õŠz1
          ,NULL Name2               -- ’Ş‘K€”õ–¼2
          ,NULL Amount2             -- ’Ş‘K€”õŠz2
          ,NULL Name3               -- ’Ş‘K€”õ–¼3
          ,NULL Amount3             -- ’Ş‘K€”õŠz3
          ,NULL Name4               -- ’Ş‘K€”õ–¼4
          ,NULL Amount4             -- ’Ş‘K€”õŠz4
          ,NULL Name5               -- ’Ş‘K€”õ–¼5
          ,NULL Amount5             -- ’Ş‘K€”õŠz5
          ,NULL Name6               -- ’Ş‘K€”õ–¼6
          ,NULL Amount6             -- ’Ş‘K€”õŠz6
          ,NULL Name7               -- ’Ş‘K€”õ–¼7
          ,NULL Amount7             -- ’Ş‘K€”õŠz7
          ,NULL Name8               -- ’Ş‘K€”õ–¼8
          ,NULL Amount8             -- ’Ş‘K€”õŠz8
          ,NULL Name9               -- ’Ş‘K€”õ–¼9
          ,NULL Amount9             -- ’Ş‘K€”õŠz9
          ,NULL Name10              -- ’Ş‘K€”õ–¼10
          ,NULL Amount10            -- ’Ş‘K€”õŠz10
          ,D.StaffReceiptPrint      -- ’S“–ƒŒƒV[ƒg•\‹L
          ,D.StoreReceiptPrint      -- “X•ÜƒŒƒV[ƒg•\‹L
      FROM (
            SELECT ROW_NUMBER() OVER(PARTITION BY history.DepositNO ORDER BY history.AccountingDate DESC) as DepositNO
                  ,CONVERT(DATE, history.DepositDateTime) RegistDate
                  ,denominationKbn.DenominationName
                  ,history.DepositGaku
                  ,history.Number
                  ,staff.ReceiptPrint StaffReceiptPrint
                  ,store.ReceiptPrint StoreReceiptPrint
              FROM D_DepositHistory history
              LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
              LEFT OUTER JOIN (
                               SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) as RANK
                                     ,StoreCD
                                     ,ReceiptPrint
                                     ,DeleteFlg 
                                 FROM M_Store 
                              ) store ON store.RANK = 1
                                     AND store.StoreCD = history.StoreCD
                                     AND store.DeleteFlg <> 1
              LEFT OUTER JOIN (
                               SELECT ROW_NUMBER() OVER(PARTITION BY StaffCD ORDER BY ChangeDate DESC) AS RANK
                                     ,StaffCD
                                     ,StoreCD
                                     ,ReceiptPrint
                                     ,DeleteFlg
                                 FROM M_Staff
                              ) staff ON staff.RANK = 1
                                     AND staff.StoreCD = history.StoreCD
                                     AND staff.StaffCD = @StaffCD
                                     AND staff.DeleteFlg <> 1
             WHERE history.StoreCD = @StoreCD
               AND history.DataKBN = 3
               AND history.DepositKBN = 6
               AND history.CancelKBN = 0
           ) D
        ;
END

GO
