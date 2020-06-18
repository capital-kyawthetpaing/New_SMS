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
    @StoreCD          varchar(4),
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.Number
          ,D.RegistDate             -- �o�^��
          ,'����' Name1             -- �ޑK������1
          ,D.DepositGaku Amount1    -- �ޑK�����z1
          ,NULL Name2               -- �ޑK������2
          ,NULL Amount2             -- �ޑK�����z2
          ,NULL Name3               -- �ޑK������3
          ,NULL Amount3             -- �ޑK�����z3
          ,NULL Name4               -- �ޑK������4
          ,NULL Amount4             -- �ޑK�����z4
          ,NULL Name5               -- �ޑK������5
          ,NULL Amount5             -- �ޑK�����z5
          ,NULL Name6               -- �ޑK������6
          ,NULL Amount6             -- �ޑK�����z6
          ,NULL Name7               -- �ޑK������7
          ,NULL Amount7             -- �ޑK�����z7
          ,NULL Name8               -- �ޑK������8
          ,NULL Amount8             -- �ޑK�����z8
          ,NULL Name9               -- �ޑK������9
          ,NULL Amount9             -- �ޑK�����z9
          ,NULL Name10              -- �ޑK������10
          ,NULL Amount10            -- �ޑK�����z10
          ,D.StaffReceiptPrint      -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint      -- �X�܃��V�[�g�\�L
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
