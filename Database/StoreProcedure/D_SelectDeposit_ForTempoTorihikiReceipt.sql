SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 取引レシート出力　入金印刷情報取得
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ロジック変更
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
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                                                      -- 登録日
          ,MAX(D.CustomerCD) CustomerCD                                                      -- 入金元CD
          ,MAX(D.CustomerName) CustomerName                                                  -- 入金元名
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) DepositDate1       -- 入金日1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) DepositName1       -- 入金名1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) DepositAmount1     -- 入金額1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) DepositDate2       -- 入金日2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) DepositName2       -- 入金名2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) DepositAmount2     -- 入金額2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) DepositDate3       -- 入金日3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) DepositName3       -- 入金名3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) DepositAmount3     -- 入金額3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) DepositDate4       -- 入金日4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) DepositName4       -- 入金名4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) DepositAmount4     -- 入金額4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) DepositDate5       -- 入金日5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) DepositName5       -- 入金名5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) DepositAmount5     -- 入金額5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) DepositDate6       -- 入金日6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) DepositName6       -- 入金名6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) DepositAmount6     -- 入金額6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) DepositDate7       -- 入金日7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) DepositName7       -- 入金名7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) DepositAmount7     -- 入金額7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) DepositDate8       -- 入金日8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) DepositName8       -- 入金名8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) DepositAmount8     -- 入金額8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) DepositDate9       -- 入金日9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) DepositName9       -- 入金名9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) DepositAmount9     -- 入金額9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) DepositDate10      -- 入金日10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) DepositName10      -- 入金名10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) DepositAmount10    -- 入金額10
          ,D.StaffReceiptPrint                                                               -- 担当レシート表記
          ,D.StoreReceiptPrint                                                               -- 店舗レシート表記
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

