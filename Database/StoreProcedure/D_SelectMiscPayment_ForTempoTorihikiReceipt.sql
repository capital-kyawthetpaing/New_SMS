SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    �X�܃��W ������V�[�g�o�́@�G�o��������擾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ���W�b�N�ύX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectMiscPayment_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectMiscPayment_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectMiscPayment_ForTempoTorihikiReceipt]
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

    SELECT D.RegistDate                                                                           -- �o�^��
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime1    -- �G�x����1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) MiscPaymentName1        -- �G�x����1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount1      -- �G�x���z1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime2    -- �G�x����2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) MiscPaymentName2        -- �G�x����2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount2      -- �G�x���z2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime3    -- �G�x����3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) MiscPaymentName3        -- �G�x����3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount3      -- �G�x���z3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime4    -- �G�x����4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) MiscPaymentName4        -- �G�x����4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount4      -- �G�x���z4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime5    -- �G�x����5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) MiscPaymentName5        -- �G�x����5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount5      -- �G�x���z5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime6    -- �G�x����6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) MiscPaymentName6        -- �G�x����6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount6      -- �G�x���z6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime7    -- �G�x����7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) MiscPaymentName7        -- �G�x����7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount7      -- �G�x���z7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime8    -- �G�x����8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) MiscPaymentName8        -- �G�x����8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount8      -- �G�x���z8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime9    -- �G�x����9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) MiscPaymentName9        -- �G�x����9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount9      -- �G�x���z9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime10   -- �G�x����10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) MiscPaymentName10       -- �G�x����10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount10     -- �G�x���z10
          ,D.StaffReceiptPrint                                                                    -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint                                                                    -- �X�܃��V�[�g�\�L
      FROM (
            SELECT H.RegistDate
                  ,H.DepositDateTime
                  ,H.DenominationCD
                  ,H.DenominationName
                  ,H.DepositGaku
                  ,ROW_NUMBER() OVER(PARTITION BY H.Number ORDER BY H.DepositDateTime ASC) as RANK
                  ,H.StaffReceiptPrint
                  ,H.StoreReceiptPrint
              FROM (
                    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') DepositDateTime
                          ,history.DenominationCD
                          ,denominationKbn.DenominationName
                          ,history.DepositGaku
                          ,history.Number
                          ,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime DESC) as RANK
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
                       AND history.DepositKBN = 3
                       AND history.CancelKBN = 0
                   ) H
             WHERE H.RANK BETWEEN 1 AND 10
           ) D
     GROUP BY D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
           ;
END

GO

