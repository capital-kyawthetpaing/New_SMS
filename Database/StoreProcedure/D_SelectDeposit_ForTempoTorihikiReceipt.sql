SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹ÉåÉW éÊà¯ÉåÉVÅ[ÉgèoóÕÅ@ì¸ã‡àÛç¸èÓïÒéÊìæ
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ÉçÉWÉbÉNïœçX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectDeposit_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
(
    @DepositNO        int,
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                                                      -- ìoò^ì˙
          ,MAX(D.CustomerCD) CustomerCD                                                      -- ì¸ã‡å≥CD
          ,MAX(D.CustomerName) CustomerName                                                  -- ì¸ã‡å≥ñº
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) DepositDate1       -- ì¸ã‡ì˙1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) DepositName1       -- ì¸ã‡ñº1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) DepositAmount1     -- ì¸ã‡äz1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) DepositDate2       -- ì¸ã‡ì˙2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) DepositName2       -- ì¸ã‡ñº2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) DepositAmount2     -- ì¸ã‡äz2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) DepositDate3       -- ì¸ã‡ì˙3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) DepositName3       -- ì¸ã‡ñº3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) DepositAmount3     -- ì¸ã‡äz3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) DepositDate4       -- ì¸ã‡ì˙4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) DepositName4       -- ì¸ã‡ñº4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) DepositAmount4     -- ì¸ã‡äz4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) DepositDate5       -- ì¸ã‡ì˙5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) DepositName5       -- ì¸ã‡ñº5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) DepositAmount5     -- ì¸ã‡äz5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) DepositDate6       -- ì¸ã‡ì˙6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) DepositName6       -- ì¸ã‡ñº6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) DepositAmount6     -- ì¸ã‡äz6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) DepositDate7       -- ì¸ã‡ì˙7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) DepositName7       -- ì¸ã‡ñº7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) DepositAmount7     -- ì¸ã‡äz7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) DepositDate8       -- ì¸ã‡ì˙8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) DepositName8       -- ì¸ã‡ñº8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) DepositAmount8     -- ì¸ã‡äz8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) DepositDate9       -- ì¸ã‡ì˙9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) DepositName9       -- ì¸ã‡ñº9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) DepositAmount9     -- ì¸ã‡äz9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) DepositDate10      -- ì¸ã‡ì˙10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) DepositName10      -- ì¸ã‡ñº10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) DepositAmount10    -- ì¸ã‡äz10
          ,D.StaffReceiptPrint                                                               -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                               -- ìXï‹ÉåÉVÅ[Égï\ãL
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
                                             AND staff.StaffCD = @StaffCD
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

