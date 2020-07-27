SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹ÉåÉW éÊà¯ÉåÉVÅ[ÉgèoóÕÅ@éGì¸ã‡àÛç¸èÓïÒéÊìæ
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ÉçÉWÉbÉNïœçX
--                  :    2020.07.11  à¯êîÇDepositNOÇ…ïœçX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectMiscDeposit_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectMiscDeposit_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectMiscDeposit_ForTempoTorihikiReceipt]
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

    SELECT D.RegistDate                                                                          -- ìoò^ì˙
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate1       -- éGì¸ã‡ì˙1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) MiscDepositName1       -- éGì¸ã‡ñº1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount1     -- éGì¸ã‡äz1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate2       -- éGì¸ã‡ì˙2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) MiscDepositName2       -- éGì¸ã‡ñº2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount2     -- éGì¸ã‡äz2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate3       -- éGì¸ã‡ì˙3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) MiscDepositName3       -- éGì¸ã‡ñº3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount3     -- éGì¸ã‡äz3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate4       -- éGì¸ã‡ì˙4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) MiscDepositName4       -- éGì¸ã‡ñº4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount4     -- éGì¸ã‡äz4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate5       -- éGì¸ã‡ì˙5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) MiscDepositName5       -- éGì¸ã‡ñº5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount5     -- éGì¸ã‡äz5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate6       -- éGì¸ã‡ì˙6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) MiscDepositName6       -- éGì¸ã‡ñº6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount6     -- éGì¸ã‡äz6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate7       -- éGì¸ã‡ì˙7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) MiscDepositName7       -- éGì¸ã‡ñº7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount7     -- éGì¸ã‡äz7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate8       -- éGì¸ã‡ì˙8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) MiscDepositName8       -- éGì¸ã‡ñº8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount8     -- éGì¸ã‡äz8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate9       -- éGì¸ã‡ì˙9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) MiscDepositName9       -- éGì¸ã‡ñº9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount9     -- éGì¸ã‡äz9
          ,MAX(CASE D.RANK WHEN  10 THEN D.DepositDateTime ELSE NULL END) MiscDepositDate10      -- éGì¸ã‡ì˙10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) MiscDepositName10      -- éGì¸ã‡ñº10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount10    -- éGì¸ã‡äz10
          ,D.StaffReceiptPrint                                                                   -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                                   -- ìXï‹ÉåÉVÅ[Égï\ãL
      FROM (
            SELECT H.RegistDate
                  ,H.DepositDateTime
                  ,H.DenominationName
                  ,H.DepositGaku
                  ,ROW_NUMBER() OVER(PARTITION BY H.Number ORDER BY H.DepositDateTime ASC) as RANK
                  ,H.StaffReceiptPrint
                  ,H.StoreReceiptPrint
              FROM (
                    SELECT CONVERT(Date, history.DepositDateTime) RegistDate
                          ,FORMAT(history.DepositDateTime,  'yyyy/MM/dd HH:mm') DepositDateTime
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
                                             AND staff.StaffCD = @StaffCD
                                             AND staff.DeleteFlg <> 1
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 2
                       AND history.DepositNO = @DepositNO
                       AND history.CancelKBN = 0
                       AND history.CustomerCD IS NULL
                   ) H
             WHERE H.RANK BETWEEN 1 AND 10
           ) D 
     GROUP BY D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
           ;
END

GO

