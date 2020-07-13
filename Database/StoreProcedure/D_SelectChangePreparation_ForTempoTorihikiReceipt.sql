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
    @DepositNO        int
)AS

--********************************************--
--                                            --
--                 ˆ—ŠJn                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                -- “o˜^“ú
          ,D.DepositDateTime ChangePreparationDate1    -- ’Ş‘K€”õ“ú1
          ,'Œ»‹à' ChangePreparationName1               -- ’Ş‘K€”õ–¼1
          ,D.DepositGaku ChangePreparationAmount1      -- ’Ş‘K€”õŠz1
          ,NULL ChangePreparationDate2                 -- ’Ş‘K€”õ“ú2
          ,NULL ChangePreparationName2                 -- ’Ş‘K€”õ–¼2
          ,NULL ChangePreparationAmount2               -- ’Ş‘K€”õŠz2
          ,NULL ChangePreparationDate3                 -- ’Ş‘K€”õ“ú3
          ,NULL ChangePreparationName3                 -- ’Ş‘K€”õ–¼3
          ,NULL ChangePreparationAmount3               -- ’Ş‘K€”õŠz3
          ,NULL ChangePreparationDate4                 -- ’Ş‘K€”õ“ú4
          ,NULL ChangePreparationName4                 -- ’Ş‘K€”õ–¼4
          ,NULL ChangePreparationAmount4               -- ’Ş‘K€”õŠz4
          ,NULL ChangePreparationDate5                 -- ’Ş‘K€”õ“ú5
          ,NULL ChangePreparationName5                 -- ’Ş‘K€”õ–¼5
          ,NULL ChangePreparationAmount5               -- ’Ş‘K€”õŠz5
          ,NULL ChangePreparationDate6                 -- ’Ş‘K€”õ“ú6
          ,NULL ChangePreparationName6                 -- ’Ş‘K€”õ–¼6
          ,NULL ChangePreparationAmount6               -- ’Ş‘K€”õŠz6
          ,NULL ChangePreparationDate7                 -- ’Ş‘K€”õ“ú7
          ,NULL ChangePreparationName7                 -- ’Ş‘K€”õ–¼7
          ,NULL ChangePreparationAmount7               -- ’Ş‘K€”õŠz7
          ,NULL ChangePreparationDate8                 -- ’Ş‘K€”õ“ú8
          ,NULL ChangePreparationName8                 -- ’Ş‘K€”õ–¼8
          ,NULL ChangePreparationAmount8               -- ’Ş‘K€”õŠz8
          ,NULL ChangePreparationDate9                 -- ’Ş‘K€”õ“ú9
          ,NULL ChangePreparationName9                 -- ’Ş‘K€”õ–¼9
          ,NULL ChangePreparationAmount9               -- ’Ş‘K€”õŠz9
          ,NULL ChangePreparationDate10                -- ’Ş‘K€”õ“ú10
          ,NULL ChangePreparationName10                -- ’Ş‘K€”õ–¼10
          ,NULL ChangePreparationAmount10              -- ’Ş‘K€”õŠz10
          ,D.StaffReceiptPrint                         -- ’S“–ƒŒƒV[ƒg•\‹L
          ,D.StoreReceiptPrint                         -- “X•ÜƒŒƒV[ƒg•\‹L
      FROM (
            SELECT H.RegistDate
                  ,H.DepositDateTime
                  ,H.DenominationName
                  ,H.DepositGaku
                  ,H.StaffReceiptPrint
                  ,H.StoreReceiptPrint
              FROM (
                    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') DepositDateTime
                          ,denominationKbn.DenominationName
                          ,history.DepositGaku
                          ,history.Number
                          ,ROW_NUMBER() OVER(PARTITION BY history.AccountingDate ORDER BY history.DepositDateTime DESC) as RANK
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
                                             AND staff.DeleteFlg <> 1
                     WHERE history.DepositNO = @DepositNO
                       AND history.DataKBN = 3
                       AND history.DepositKBN = 6
                       AND history.CancelKBN = 0
                   ) H
             WHERE H.RANK = 1
           ) D
        ;
END

GO
