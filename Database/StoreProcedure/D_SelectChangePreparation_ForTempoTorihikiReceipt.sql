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
    @DepositNO        int,
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                                -- �o�^��
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') ChangePreparationDate        -- �ޑK������
          ,'����' ChangePreparationName                                                     -- �ޑK������
          ,history.DepositGaku ChangePreparationAmount                                      -- �ޑK�����z
          ,history.Remark ChangePreparationRemark                                           -- �ޑK�����l
          ,staff.ReceiptPrint AS StaffReceiptPrint                                             -- �S�����V�[�g�\�L
          ,store.ReceiptPrint AS StoreReceiptPrint                                          -- �X�܃��V�[�g�\�L
      FROM D_DepositHistory history
      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
      LEFT OUTER JOIN F_Store(CONVERT(DATE, GETDATE())) AS store
        ON store.StoreCD = history.StoreCD
       AND store.DeleteFlg = 0
      LEFT OUTER JOIN F_Staff(CONVERT(DATE, GETDATE())) AS staff
        ON staff.StoreCD = history.StoreCD
       AND staff.StaffCD = @StaffCD
       AND staff.DeleteFlg = 0

     WHERE history.DepositNO = @DepositNO
       AND history.DataKBN = 3
       AND history.DepositKBN = 6
       AND history.CancelKBN = 0
        ;
END

GO
