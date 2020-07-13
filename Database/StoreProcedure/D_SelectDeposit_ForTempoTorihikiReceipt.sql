SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    �X�܃��W ������V�[�g�o�́@����������擾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ���W�b�N�ύX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectDeposit_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
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

    SELECT D.RegistDate                                                                      -- �o�^��
          ,MAX(D.CustomerCD) CustomerCD                                                      -- ������CD
          ,MAX(D.CustomerName) CustomerName                                                  -- ��������
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) DepositDate1       -- ������1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) DepositName1       -- ������1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) DepositAmount1     -- �����z1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) DepositDate2       -- ������2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) DepositName2       -- ������2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) DepositAmount2     -- �����z2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) DepositDate3       -- ������3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) DepositName3       -- ������3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) DepositAmount3     -- �����z3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) DepositDate4       -- ������4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) DepositName4       -- ������4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) DepositAmount4     -- �����z4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) DepositDate5       -- ������5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) DepositName5       -- ������5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) DepositAmount5     -- �����z5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) DepositDate6       -- ������6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) DepositName6       -- ������6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) DepositAmount6     -- �����z6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) DepositDate7       -- ������7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) DepositName7       -- ������7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) DepositAmount7     -- �����z7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) DepositDate8       -- ������8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) DepositName8       -- ������8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) DepositAmount8     -- �����z8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) DepositDate9       -- ������9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) DepositName9       -- ������9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) DepositAmount9     -- �����z9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) DepositDate10      -- ������10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) DepositName10      -- ������10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) DepositAmount10    -- �����z10
          ,D.StaffReceiptPrint                                                               -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint                                                               -- �X�܃��V�[�g�\�L
      FROM (
            SELECT H.RegistDate
                  ,H.DepositDateTime
                  ,H.CustomerCD
                  ,H.CustomerName
                  ,H.DenominationCD
                  ,H.DenominationName
                  ,H.DepositGaku
                  ,ROW_NUMBER() OVER(PARTITION BY H.Number ORDER BY H.DepositDateTime ASC) as RANK
                  ,H.StaffReceiptPrint
                  ,H.StoreReceiptPrint
              FROM (
                    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') DepositDateTime
                          ,customer.CustomerCD
                          ,customer.CustomerName
                          ,denominationKbn.DenominationName
                          ,denominationKbn.DenominationCD
                          ,history.DepositGaku
                          ,history.Number
                          ,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime DESC) as RANK
                          ,staff.ReceiptPrint StaffReceiptPrint
                          ,store.ReceiptPrint StoreReceiptPrint
                      FROM D_DepositHistory history
                      LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
                      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                      LEFT OUTER JOIN (
                                       SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) AS RANK
                                             ,CustomerCD
                                             ,CustomerName
                                             ,ChangeDate
                                             ,DeleteFlg
                                         FROM M_Customer
                                      ) customer ON customer.RANK = 1
                                                AND customer.CustomerCD = history.CustomerCD
                                                AND customer.ChangeDate <= history.AccountingDate
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
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 2
                       AND history.DepositNO = @DepositNO
                       AND history.CancelKBN = 0
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                       AND history.CustomerCD IS NOT NULL
                       AND customer.DeleteFlg = 0
                   ) H
             WHERE H.RANK BETWEEN 1 AND 10
           ) D 
     GROUP BY D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
        ;
END

GO

