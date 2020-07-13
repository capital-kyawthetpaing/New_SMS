SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹ÉåÉW éÊà¯ÉåÉVÅ[ÉgèoóÕÅ@éGèoã‡àÛç¸èÓïÒéÊìæ
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ÉçÉWÉbÉNïœçX
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
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                                                           -- ìoò^ì˙
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime1    -- éGéxï•ì˙1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) MiscPaymentName1        -- éGéxï•ñº1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount1      -- éGéxï•äz1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime2    -- éGéxï•ì˙2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) MiscPaymentName2        -- éGéxï•ñº2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount2      -- éGéxï•äz2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime3    -- éGéxï•ì˙3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) MiscPaymentName3        -- éGéxï•ñº3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount3      -- éGéxï•äz3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime4    -- éGéxï•ì˙4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) MiscPaymentName4        -- éGéxï•ñº4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount4      -- éGéxï•äz4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime5    -- éGéxï•ì˙5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) MiscPaymentName5        -- éGéxï•ñº5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount5      -- éGéxï•äz5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime6    -- éGéxï•ì˙6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) MiscPaymentName6        -- éGéxï•ñº6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount6      -- éGéxï•äz6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime7    -- éGéxï•ì˙7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) MiscPaymentName7        -- éGéxï•ñº7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount7      -- éGéxï•äz7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime8    -- éGéxï•ì˙8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) MiscPaymentName8        -- éGéxï•ñº8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount8      -- éGéxï•äz8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime9    -- éGéxï•ì˙9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) MiscPaymentName9        -- éGéxï•ñº9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount9      -- éGéxï•äz9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDateTime10   -- éGéxï•ì˙10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) MiscPaymentName10       -- éGéxï•ñº10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount10     -- éGéxï•äz10
          ,D.StaffReceiptPrint                                                                    -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                                    -- ìXï‹ÉåÉVÅ[Égï\ãL
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

