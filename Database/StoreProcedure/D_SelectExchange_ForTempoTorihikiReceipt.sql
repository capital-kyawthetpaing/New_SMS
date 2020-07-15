SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹ÉåÉW éÊà¯ÉåÉVÅ[ÉgèoóÕÅ@óºë÷àÛç¸èÓïÒéÊìæ
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ÉçÉWÉbÉNïœçX
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectExchange_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
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

    SELECT D.RegistDate                                                                                 -- ìoò^ì˙
          ,COUNT(*) ExchangeCount                                                                       -- óºë÷âÒêî
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime1         -- óºë÷ì˙1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName     ELSE NULL END) ExchangeName1             -- óºë÷ñº1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount1           -- óºë÷äz1
          ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination1     -- óºë÷éÜïº1
          ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount1            -- óºë÷ñáêî1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime2         -- óºë÷ì˙2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName     ELSE NULL END) ExchangeName2             -- óºë÷ñº2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount2           -- óºë÷äz2
          ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination2     -- óºë÷éÜïº2
          ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount2            -- óºë÷ñáêî2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime3         -- óºë÷ì˙3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName     ELSE NULL END) ExchangeName3             -- óºë÷ñº3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount3           -- óºë÷äz3
          ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination3     -- óºë÷éÜïº3
          ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount3            -- óºë÷ñáêî3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime4         -- óºë÷ì˙4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName     ELSE NULL END) ExchangeName4             -- óºë÷ñº4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount4           -- óºë÷äz4
          ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination4     -- óºë÷éÜïº4
          ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount4            -- óºë÷ñáêî4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime5         -- óºë÷ì˙5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName     ELSE NULL END) ExchangeName5             -- óºë÷ñº5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount5           -- óºë÷äz5
          ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination5     -- óºë÷éÜïº5
          ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount5            -- óºë÷ñáêî5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime6         -- óºë÷ì˙6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName     ELSE NULL END) ExchangeName6             -- óºë÷ñº6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount6           -- óºë÷äz6
          ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination6     -- óºë÷éÜïº6
          ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount6            -- óºë÷ñáêî6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime7         -- óºë÷ì˙7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName     ELSE NULL END) ExchangeName7             -- óºë÷ñº7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount7           -- óºë÷äz7
          ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination7     -- óºë÷éÜïº7
          ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount7            -- óºë÷ñáêî7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime8         -- óºë÷ì˙8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName     ELSE NULL END) ExchangeName8             -- óºë÷ñº8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount8           -- óºë÷äz8
          ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination8     -- óºë÷éÜïº8
          ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount8            -- óºë÷ñáêî8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime9         -- óºë÷ì˙9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName     ELSE NULL END) ExchangeName9             -- óºë÷ñº9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount9           -- óºë÷äz9
          ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination9     -- óºë÷éÜïº9
          ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount9            -- óºë÷ñáêî9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime10        -- óºë÷ì˙10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName     ELSE NULL END) ExchangeName10            -- óºë÷ñº10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount10          -- óºë÷äz10
          ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination10    -- óºë÷éÜïº10
          ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount10           -- óºë÷ñáêî10
          ,D.StaffReceiptPrint                                                                          -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                                          -- ìXï‹ÉåÉVÅ[Égï\ãL
      FROM (
            SELECT H.RegistDate
                  ,H.DepositDateTime
                  ,H.DenominationCD
                  ,H.DenominationName
                  ,H.DepositGaku
                  ,H.ExchangeDenomination
                  ,H.ExchangeCount
                  ,ROW_NUMBER() OVER(PARTITION BY H.Number ORDER BY H.DepositDateTime ASC) as RANK
                  ,H.StaffReceiptPrint
                  ,H.StoreReceiptPrint
              FROM (
                    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') DepositDateTime
                          ,history.DenominationCD
                          ,denominationKbn.DenominationName
                          ,history.DepositGaku
                          ,history.ExchangeDenomination
                          ,history.ExchangeCount
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
                       AND history.DepositKBN = 4
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
