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
          ,D.RegistDate                                                                              -- ìoò^ì˙
          ,COUNT(*) ExchangeCount                                                                    -- óºë÷âÒêî
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName     ELSE NULL END) Name1             -- óºë÷ñº1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku          ELSE NULL END) Amount1           -- óºë÷äz1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) Denomination1     -- óºë÷éÜïº1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeCount        ELSE NULL END) Count1            -- óºë÷ñáêî1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName     ELSE NULL END) Name2             -- óºë÷ñº2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku          ELSE NULL END) Amount2           -- óºë÷äz2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) Denomination2     -- óºë÷éÜïº2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeCount        ELSE NULL END) Count2            -- óºë÷ñáêî2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName     ELSE NULL END) Name3             -- óºë÷ñº3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku          ELSE NULL END) Amount3           -- óºë÷äz3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) Denomination3     -- óºë÷éÜïº3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeCount        ELSE NULL END) Count3            -- óºë÷ñáêî3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName     ELSE NULL END) Name4             -- óºë÷ñº4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku          ELSE NULL END) Amount4           -- óºë÷äz4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) Denomination4     -- óºë÷éÜïº4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeCount        ELSE NULL END) Count4            -- óºë÷ñáêî4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName     ELSE NULL END) Name5             -- óºë÷ñº5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku          ELSE NULL END) Amount5           -- óºë÷äz5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) Denomination5     -- óºë÷éÜïº5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeCount        ELSE NULL END) Count5            -- óºë÷ñáêî5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName     ELSE NULL END) Name6             -- óºë÷ñº6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku          ELSE NULL END) Amount6           -- óºë÷äz6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) Denomination6     -- óºë÷éÜïº6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeCount        ELSE NULL END) Count6            -- óºë÷ñáêî6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName     ELSE NULL END) Name7             -- óºë÷ñº7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku          ELSE NULL END) Amount7           -- óºë÷äz7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) Denomination7     -- óºë÷éÜïº7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeCount        ELSE NULL END) Count7            -- óºë÷ñáêî7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName     ELSE NULL END) Name8             -- óºë÷ñº8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku          ELSE NULL END) Amount8           -- óºë÷äz8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) Denomination8     -- óºë÷éÜïº8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeCount        ELSE NULL END) Count8            -- óºë÷ñáêî8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName     ELSE NULL END) Name9             -- óºë÷ñº9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku          ELSE NULL END) Amount9           -- óºë÷äz9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) Denomination9     -- óºë÷éÜïº9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeCount        ELSE NULL END) Count9            -- óºë÷ñáêî9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName     ELSE NULL END) Name10            -- óºë÷ñº10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku          ELSE NULL END) Amount10          -- óºë÷äz10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) Denomination10    -- óºë÷éÜïº10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeCount        ELSE NULL END) Count10           -- óºë÷ñáêî10
          ,D.StaffReceiptPrint                                                                       -- íSìñÉåÉVÅ[Égï\ãL
          ,D.StoreReceiptPrint                                                                       -- ìXï‹ÉåÉVÅ[Égï\ãL
      FROM (
            SELECT ROW_NUMBER() OVER(PARTITION BY history.DepositNO ORDER BY history.AccountingDate DESC) as DepositNO
                  ,CONVERT(DATE, history.DepositDateTime) RegistDate
                  ,denominationKbn.DenominationName
                  ,history.DepositGaku
                  ,history.ExchangeDenomination
                  ,history.ExchangeCount
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
               AND history.DepositKBN = 4
               AND history.CancelKBN = 0
           ) D
     GROUP BY D.Number
             ,D.RegistDate
             ,D.StaffReceiptPrint
             ,D.StoreReceiptPrint
        ;
END

GO
