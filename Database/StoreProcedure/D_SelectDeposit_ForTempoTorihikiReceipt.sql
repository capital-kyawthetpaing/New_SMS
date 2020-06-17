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
    @StoreCD          varchar(4),
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.Number
          ,D.RegistDate                                                                    -- ìoò^ì˙
          ,MAX(D.CustomerCD) CustomerCD                                                    -- ì¸ã‡å≥CD
          ,MAX(D.CustomerName) CustomerName                                                -- ì¸ã‡å≥ñº
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName ELSE NULL END) Name1       -- ì¸ã‡ñº1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku      ELSE NULL END) Amount1     -- ì¸ã‡äz1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName ELSE NULL END) Name2       -- ì¸ã‡ñº2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku      ELSE NULL END) Amount2     -- ì¸ã‡äz2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName ELSE NULL END) Name3       -- ì¸ã‡ñº3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku      ELSE NULL END) Amount3     -- ì¸ã‡äz3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName ELSE NULL END) Name4       -- ì¸ã‡ñº4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku      ELSE NULL END) Amount4     -- ì¸ã‡äz4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName ELSE NULL END) Name5       -- ì¸ã‡ñº5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku      ELSE NULL END) Amount5     -- ì¸ã‡äz5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName ELSE NULL END) Name6       -- ì¸ã‡ñº6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku      ELSE NULL END) Amount6     -- ì¸ã‡äz6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName ELSE NULL END) Name7       -- ì¸ã‡ñº7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku      ELSE NULL END) Amount7     -- ì¸ã‡äz7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName ELSE NULL END) Name8       -- ì¸ã‡ñº8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku      ELSE NULL END) Amount8     -- ì¸ã‡äz8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName ELSE NULL END) Name9       -- ì¸ã‡ñº9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku      ELSE NULL END) Amount9     -- ì¸ã‡äz9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName ELSE NULL END) Name10      -- ì¸ã‡ñº10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku      ELSE NULL END) Amount10    -- ì¸ã‡äz10
          ,D.StaffReceiptPrint                                                             -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                             -- ìXï‹ÉåÉVÅ[Égï\ãL
      FROM (
            SELECT ROW_NUMBER() OVER(PARTITION BY history.DepositNO ORDER BY history.DepositDateTime DESC) as DepositNO
                  ,CONVERT(DATE, history.DepositDateTime) RegistDate
                  ,customer.CustomerCD
                  ,customer.CustomerName
                  ,denominationKbn.DenominationName
                  ,history.DepositGaku
                  ,history.Number
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
                                        AND CONVERT(varchar, customer.ChangeDate, 111) <= CONVERT(varchar, history.DepositDateTime, 111)
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
               AND history.DepositKBN = 2
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
               AND history.CustomerCD IS NOT NULL
               AND customer.DeleteFlg = 0
           ) D 
     GROUP BY D.Number
             ,D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
        ;
END

GO

