SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    �X�܃��W ������V�[�g�o�́@���ֈ�����擾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ���W�b�N�ύX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectExchange_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
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

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                           -- �o�^��
          ,ABS(history.ExchangeCount) ExchangeCount                                    -- ���։�
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') ExchangeDateTime        -- ���֓�
          ,denominationKbn.DenominationName ExchangeName                               -- ���֖�
          ,ABS(history.DepositGaku) ExchangeAmount                                     -- ���֊z
          ,history.ExchangeDenomination                                                -- ���֎���
          ,history.Remark ExchangeRemark                                               -- ���֔��l
          ,staff.ReceiptPrint StaffReceiptPrint                                        -- �S�����V�[�g�\�L
          ,store.ReceiptPrint StoreReceiptPrint                                        -- �X�܃��V�[�g�\�L
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
      WHERE history.DepositNO = @DepositNO
        AND history.DataKBN = 3
        AND history.DepositKBN = 5
        AND history.CancelKBN = 0
        AND store.DeleteFlg <> 1
        AND staff.DeleteFlg <> 1
           ;
END

GO
