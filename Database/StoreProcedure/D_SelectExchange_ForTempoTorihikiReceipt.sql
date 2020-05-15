 BEGIN TRY 
 Drop Procedure dbo.[D_SelectExchange_ForTempoTorihikiReceipt]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗取引レシート印刷　レシート印刷出力
--       Program ID      TempoTorihikiReceipt
--       Create date:    2020.02.24
--    ======================================================================
CREATE PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
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

    -- ワークテーブルが残っている場合は削除
    IF OBJECT_ID( N'#Temp_Sales', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#Temp_Sales];
    END

    -- 【販売】ワークテーブル作成
    SELECT * 
      INTO #Temp_Sales
      FROM (SELECT CONVERT(DATE, history.DepositDateTime) RegistDate  -- 登録日
                  ,history.Number                                     -- 伝票番号
                  ,sales.SalesNO                                      -- 売上番号
                  ,history.DepositDateTime RegistDateTime             -- 登録日時
                  ,history.StoreCD                                    -- 店舗CD
                  ,1 DetailOrder                                      -- 明細表示順
                  ,history.JanCD                                      -- JanCD
                  ,sku.SKUShortName                                   -- 商品名
                  ,history.DepositDateTime IssueDate                  -- 発行日時
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesUnitPrice
                   END AS SalesUnitPrice                              -- 単価
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesSU
                   END AS SalesSU                                     -- 数量
                  ,history.SalesGaku                                  -- 販売額
                  ,history.SalesTax                                   -- 税額
                  ,history.SalesTaxRate                               -- 税率
                  ,history.TotalGaku                                  -- 販売合計額
                  ,staff.ReceiptPrint StaffReceiptPrint               -- 担当レシート表記
                  ,store.ReceiptPrint StoreReceiptPrint               -- 店舗レシート表記
              FROM D_DepositHistory history
              LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
              LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY AdminNO ORDER BY ChangeDate DESC) as RANK
                                     ,AdminNO
                                     ,SKUCD
                                     ,JanCD
                                     ,ChangeDate
                                     ,SKUShortName
                                     ,DeleteFlg
                                 FROM M_SKU 
                              ) sku ON sku.RANK = 1
                                   AND sku.SKUCD = history.SKUCD
                                   AND sku.JanCD = history.JanCD
                                   AND sku.ChangeDate <= history.AccountingDate
              LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY StaffCD ORDER BY ChangeDate DESC) AS RANK
                                     ,StaffCD
                                     ,ChangeDate
                                     ,ReceiptPrint
                                     ,DeleteFlg
                                 FROM M_Staff
                              ) staff ON staff.RANK = 1
                                     AND staff.StaffCD = sales.StaffCD
                                     AND staff.ChangeDate <= sales.SalesDate
              LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) as RANK
                                     ,StoreCD
                                     ,StoreName
                                     ,Address1
                                     ,Address2
                                     ,TelephoneNO
                                     ,ChangeDate
                                     ,ReceiptPrint
                                     ,DeleteFlg 
                                 FROM M_Store 
                              ) store ON store.RANK = 1
                                     AND store.StoreCD = sales.StoreCD
                                     AND store.ChangeDate <= sales.SalesDate
             WHERE history.DataKBN = 2
               AND history.DepositKBN = 1
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
               AND sku.DeleteFlg = 0
               AND staff.DeleteFlg = 0
               AND store.DeleteFlg = 0
           ) sales;

    SELECT D.Number
          ,D.RegistDate                                                                              -- 登録日
          ,COUNT(*) ExchangeCount                                                                    -- 両替回数
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName     ELSE NULL END) Name1             -- 両替名1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku          ELSE NULL END) Amount1           -- 両替額1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) Denomination1     -- 両替紙幣1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.ExchangeCount        ELSE NULL END) Count1            -- 両替枚数1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName     ELSE NULL END) Name2             -- 両替名2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku          ELSE NULL END) Amount2           -- 両替額2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) Denomination2     -- 両替紙幣2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.ExchangeCount        ELSE NULL END) Count2            -- 両替枚数2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName     ELSE NULL END) Name3             -- 両替名3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku          ELSE NULL END) Amount3           -- 両替額3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) Denomination3     -- 両替紙幣3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.ExchangeCount        ELSE NULL END) Count3            -- 両替枚数3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName     ELSE NULL END) Name4             -- 両替名4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku          ELSE NULL END) Amount4           -- 両替額4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) Denomination4     -- 両替紙幣4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.ExchangeCount        ELSE NULL END) Count4            -- 両替枚数4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName     ELSE NULL END) Name5             -- 両替名5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku          ELSE NULL END) Amount5           -- 両替額5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) Denomination5     -- 両替紙幣5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.ExchangeCount        ELSE NULL END) Count5            -- 両替枚数5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName     ELSE NULL END) Name6             -- 両替名6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku          ELSE NULL END) Amount6           -- 両替額6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) Denomination6     -- 両替紙幣6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.ExchangeCount        ELSE NULL END) Count6            -- 両替枚数6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName     ELSE NULL END) Name7             -- 両替名7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku          ELSE NULL END) Amount7           -- 両替額7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) Denomination7     -- 両替紙幣7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.ExchangeCount        ELSE NULL END) Count7            -- 両替枚数7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName     ELSE NULL END) Name8             -- 両替名8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku          ELSE NULL END) Amount8           -- 両替額8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) Denomination8     -- 両替紙幣8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.ExchangeCount        ELSE NULL END) Count8            -- 両替枚数8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName     ELSE NULL END) Name9             -- 両替名9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku          ELSE NULL END) Amount9           -- 両替額9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) Denomination9     -- 両替紙幣9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.ExchangeCount        ELSE NULL END) Count9            -- 両替枚数9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName     ELSE NULL END) Name10            -- 両替名10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku          ELSE NULL END) Amount10          -- 両替額10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) Denomination10    -- 両替紙幣10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.ExchangeCount        ELSE NULL END) Count10           -- 両替枚数10
          ,tempSales.StaffReceiptPrint                                                               -- 担当レシート表記
          ,tempSales.StoreReceiptPrint                                                               -- 店舗レシート表記
          ,tempSales.StoreCD
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY history.DepositNO ORDER BY history.DepositDateTime DESC) as DepositNO
                  ,CONVERT(DATE, history.DepositDateTime) RegistDate
                  ,denominationKbn.DenominationName
                  ,history.DepositGaku
                  ,history.ExchangeDenomination
                  ,history.ExchangeCount
                  ,history.Number
              FROM D_DepositHistory history
              LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
              LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
             WHERE history.DepositNO = @DepositNO 
               AND history.DataKBN = 3
               AND history.DepositKBN = 4
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
           ) D
      LEFT OUTER JOIN #Temp_Sales tempSales ON tempSales.RegistDate = D.RegistDate 
                                           AND tempSales.Number = D.Number
     GROUP BY D.Number
             ,D.RegistDate
             ,tempSales.StaffReceiptPrint
             ,tempSales.StoreReceiptPrint
             ,tempSales.StoreCD
        ;
END

GO
