SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    �X�܃��W ������V�[�g�o�́@�ޑK�������擾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ���W�b�N�ύX
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
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                -- �o�^��
          ,D.DepositDateTime ChangePreparationDate1    -- �ޑK������1
          ,'����' ChangePreparationName1               -- �ޑK������1
          ,D.DepositGaku ChangePreparationAmount1      -- �ޑK�����z1
          ,NULL ChangePreparationDate2                 -- �ޑK������2
          ,NULL ChangePreparationName2                 -- �ޑK������2
          ,NULL ChangePreparationAmount2               -- �ޑK�����z2
          ,NULL ChangePreparationDate3                 -- �ޑK������3
          ,NULL ChangePreparationName3                 -- �ޑK������3
          ,NULL ChangePreparationAmount3               -- �ޑK�����z3
          ,NULL ChangePreparationDate4                 -- �ޑK������4
          ,NULL ChangePreparationName4                 -- �ޑK������4
          ,NULL ChangePreparationAmount4               -- �ޑK�����z4
          ,NULL ChangePreparationDate5                 -- �ޑK������5
          ,NULL ChangePreparationName5                 -- �ޑK������5
          ,NULL ChangePreparationAmount5               -- �ޑK�����z5
          ,NULL ChangePreparationDate6                 -- �ޑK������6
          ,NULL ChangePreparationName6                 -- �ޑK������6
          ,NULL ChangePreparationAmount6               -- �ޑK�����z6
          ,NULL ChangePreparationDate7                 -- �ޑK������7
          ,NULL ChangePreparationName7                 -- �ޑK������7
          ,NULL ChangePreparationAmount7               -- �ޑK�����z7
          ,NULL ChangePreparationDate8                 -- �ޑK������8
          ,NULL ChangePreparationName8                 -- �ޑK������8
          ,NULL ChangePreparationAmount8               -- �ޑK�����z8
          ,NULL ChangePreparationDate9                 -- �ޑK������9
          ,NULL ChangePreparationName9                 -- �ޑK������9
          ,NULL ChangePreparationAmount9               -- �ޑK�����z9
          ,NULL ChangePreparationDate10                -- �ޑK������10
          ,NULL ChangePreparationName10                -- �ޑK������10
          ,NULL ChangePreparationAmount10              -- �ޑK�����z10
          ,D.StaffReceiptPrint                         -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint                         -- �X�܃��V�[�g�\�L
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
