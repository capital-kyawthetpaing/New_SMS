SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹ÉåÉW éÊà¯ÉåÉVÅ[ÉgèoóÕÅ@éGì¸ã‡àÛç¸èÓïÒéÊìæ
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ÉçÉWÉbÉNïœçX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectMiscDeposit_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectMiscDeposit_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectMiscDeposit_ForTempoTorihikiReceipt]
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
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName ELSE NULL END) Name1       -- éGì¸ã‡ñº1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku      ELSE NULL END) Amount1     -- éGì¸ã‡äz1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName ELSE NULL END) Name2       -- éGì¸ã‡ñº2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku      ELSE NULL END) Amount2     -- éGì¸ã‡äz2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName ELSE NULL END) Name3       -- éGì¸ã‡ñº3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku      ELSE NULL END) Amount3     -- éGì¸ã‡äz3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName ELSE NULL END) Name4       -- éGì¸ã‡ñº4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku      ELSE NULL END) Amount4     -- éGì¸ã‡äz4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName ELSE NULL END) Name5       -- éGì¸ã‡ñº5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku      ELSE NULL END) Amount5     -- éGì¸ã‡äz5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName ELSE NULL END) Name6       -- éGì¸ã‡ñº6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku      ELSE NULL END) Amount6     -- éGì¸ã‡äz6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName ELSE NULL END) Name7       -- éGì¸ã‡ñº7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku      ELSE NULL END) Amount7     -- éGì¸ã‡äz7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName ELSE NULL END) Name8       -- éGì¸ã‡ñº8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku      ELSE NULL END) Amount8     -- éGì¸ã‡äz8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName ELSE NULL END) Name9       -- éGì¸ã‡ñº9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku      ELSE NULL END) Amount9     -- éGì¸ã‡äz9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName ELSE NULL END) Name10      -- éGì¸ã‡ñº10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku      ELSE NULL END) Amount10    -- éGì¸ã‡äz10
          ,D.StaffReceiptPrint                                                             -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                             -- ìXï‹ÉåÉVÅ[Égï\ãL
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
               AND history.DepositKBN = 2
               AND history.CancelKBN = 0
               AND history.CustomerCD IS NULL
           ) D 
     GROUP BY D.Number
             ,D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
        ;
END

GO

