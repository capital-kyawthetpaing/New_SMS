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
    @DepositNO        int
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT D.RegistDate                                                                                 -- �o�^��
          ,COUNT(*) ExchangeCount                                                                       -- ���։�
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime1         -- ���֓�1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName     ELSE NULL END) ExchangeName1             -- ���֖�1
          ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount1           -- ���֊z1
          ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination1     -- ���֎���1
          ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount1            -- ���֖���1
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime2         -- ���֓�2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName     ELSE NULL END) ExchangeName2             -- ���֖�2
          ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount2           -- ���֊z2
          ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination2     -- ���֎���2
          ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount2            -- ���֖���2
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime3         -- ���֓�3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName     ELSE NULL END) ExchangeName3             -- ���֖�3
          ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount3           -- ���֊z3
          ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination3     -- ���֎���3
          ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount3            -- ���֖���3
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime4         -- ���֓�4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName     ELSE NULL END) ExchangeName4             -- ���֖�4
          ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount4           -- ���֊z4
          ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination4     -- ���֎���4
          ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount4            -- ���֖���4
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime5         -- ���֓�5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName     ELSE NULL END) ExchangeName5             -- ���֖�5
          ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount5           -- ���֊z5
          ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination5     -- ���֎���5
          ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount5            -- ���֖���5
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime6         -- ���֓�6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName     ELSE NULL END) ExchangeName6             -- ���֖�6
          ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount6           -- ���֊z6
          ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination6     -- ���֎���6
          ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount6            -- ���֖���6
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime7         -- ���֓�7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName     ELSE NULL END) ExchangeName7             -- ���֖�7
          ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount7           -- ���֊z7
          ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination7     -- ���֎���7
          ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount7            -- ���֖���7
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime8         -- ���֓�8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName     ELSE NULL END) ExchangeName8             -- ���֖�8
          ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount8           -- ���֊z8
          ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination8     -- ���֎���8
          ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount8            -- ���֖���8
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime9         -- ���֓�9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName     ELSE NULL END) ExchangeName9             -- ���֖�9
          ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount9           -- ���֊z9
          ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination9     -- ���֎���9
          ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount9            -- ���֖���9
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime      ELSE NULL END) ExchangeDateTime10        -- ���֓�10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName     ELSE NULL END) ExchangeName10            -- ���֖�10
          ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount10          -- ���֊z10
          ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination10    -- ���֎���10
          ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount10           -- ���֖���10
          ,D.StaffReceiptPrint                                                                          -- �S�����V�[�g�\�L
          ,D.StoreReceiptPrint                                                                          -- �X�܃��V�[�g�\�L
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
