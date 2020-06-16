SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    �X�܃��W ������V�[�g�o�́@���ֈ�����擾
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ���W�b�N�ύX
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
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.Number
          ,D.RegistDate                                                                              -- �o�^��
          ,COUNT(*) ExchangeCount                                                                    -- ���։�
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName     ELSE NULL END) Name1             -- ���֖�1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku          ELSE NULL END) Amount1           -- ���֊z1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) Denomination1     -- ���֎���1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeCount        ELSE NULL END) Count1            -- ���֖���1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName     ELSE NULL END) Name2             -- ���֖�2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku          ELSE NULL END) Amount2           -- ���֊z2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) Denomination2     -- ���֎���2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeCount        ELSE NULL END) Count2            -- ���֖���2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName     ELSE NULL END) Name3             -- ���֖�3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku          ELSE NULL END) Amount3           -- ���֊z3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) Denomination3     -- ���֎���3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeCount        ELSE NULL END) Count3            -- ���֖���3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName     ELSE NULL END) Name4             -- ���֖�4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku          ELSE NULL END) Amount4           -- ���֊z4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) Denomination4     -- ���֎���4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeCount        ELSE NULL END) Count4            -- ���֖���4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName     ELSE NULL END) Name5             -- ���֖�5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku          ELSE NULL END) Amount5           -- ���֊z5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) Denomination5     -- ���֎���5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeCount        ELSE NULL END) Count5            -- ���֖���5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName     ELSE NULL END) Name6             -- ���֖�6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku          ELSE NULL END) Amount6           -- ���֊z6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) Denomination6     -- ���֎���6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeCount        ELSE NULL END) Count6            -- ���֖���6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName     ELSE NULL END) Name7             -- ���֖�7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku          ELSE NULL END) Amount7           -- ���֊z7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) Denomination7     -- ���֎���7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeCount        ELSE NULL END) Count7            -- ���֖���7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName     ELSE NULL END) Name8             -- ���֖�8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku          ELSE NULL END) Amount8           -- ���֊z8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) Denomination8     -- ���֎���8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeCount        ELSE NULL END) Count8            -- ���֖���8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName     ELSE NULL END) Name9             -- ���֖�9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku          ELSE NULL END) Amount9           -- ���֊z9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) Denomination9     -- ���֎���9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeCount        ELSE NULL END) Count9            -- ���֖���9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName     ELSE NULL END) Name10            -- ���֖�10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku          ELSE NULL END) Amount10          -- ���֊z10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) Denomination10    -- ���֎���10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeCount        ELSE NULL END) Count10           -- ���֖���10
          ,D.StaffReceiptPrint                                                                       -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint                                                                       -- �X�܃��V�[�g�\�L
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
