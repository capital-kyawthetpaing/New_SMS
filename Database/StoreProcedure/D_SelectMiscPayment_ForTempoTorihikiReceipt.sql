--  ======================================================================
--       éGèoã‡àÛç¸èÓïÒéÊìæ
--  ======================================================================
USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    ìXï‹éÊà¯ÉåÉVÅ[ÉgàÛç¸Å@ÉåÉVÅ[ÉgàÛç¸èoóÕ
--       Program ID      TempoTorihikiReceipt
--       Create date:    2020.02.24
--    ======================================================================
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

    -- ÉèÅ[ÉNÉeÅ[ÉuÉãÇ™écÇ¡ÇƒÇ¢ÇÈèÍçáÇÕçÌèú
    IF OBJECT_ID( N'#Temp_Sales', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#Temp_Sales];
    END

    -- ÅyîÃîÑÅzÉèÅ[ÉNÉeÅ[ÉuÉãçÏê¨
    SELECT * 
      INTO #Temp_Sales
      FROM (SELECT CONVERT(DATE, history.DepositDateTime) RegistDate  -- ìoò^ì˙
                  ,history.Number                                     -- ì`ï[î‘çÜ
                  ,sales.SalesNO                                      -- îÑè„î‘çÜ
                  ,history.DepositDateTime RegistDateTime             -- ìoò^ì˙éû
                  ,history.StoreCD                                    -- ìXï‹CD
                  ,1 DetailOrder                                      -- ñæç◊ï\é¶èá
                  ,history.JanCD                                      -- JanCD
                  ,sku.SKUShortName                                   -- è§ïiñº
                  ,history.DepositDateTime IssueDate                  -- î≠çsì˙éû
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesUnitPrice
                   END AS SalesUnitPrice                              -- íPâø
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesSU
                   END AS SalesSU                                     -- êîó 
                  ,history.SalesGaku                                  -- îÃîÑäz
                  ,history.SalesTax                                   -- ê≈äz
                  ,history.SalesTaxRate                               -- ê≈ó¶
                  ,history.TotalGaku                                  -- îÃîÑçáåväz
                  ,staff.ReceiptPrint StaffReceiptPrint               -- íSìñÉåÉVÅ[Égï\ãL
                  ,store.ReceiptPrint StoreReceiptPrint               -- ìXï‹ÉåÉVÅ[Égï\ãL
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
                                     ,TelphoneNO
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
          ,D.RegistDate                                                                    -- ìoò^ì˙
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DenominationName ELSE NULL END) Name1       -- éGéxï•ñº1
          ,MAX(CASE D.DepositNO WHEN  1 THEN D.DepositGaku      ELSE NULL END) Amount1     -- éGéxï•äz1
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DenominationName ELSE NULL END) Name2       -- éGéxï•ñº2
          ,MAX(CASE D.DepositNO WHEN  2 THEN D.DepositGaku      ELSE NULL END) Amount2     -- éGéxï•äz2
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DenominationName ELSE NULL END) Name3       -- éGéxï•ñº3
          ,MAX(CASE D.DepositNO WHEN  3 THEN D.DepositGaku      ELSE NULL END) Amount3     -- éGéxï•äz3
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DenominationName ELSE NULL END) Name4       -- éGéxï•ñº4
          ,MAX(CASE D.DepositNO WHEN  4 THEN D.DepositGaku      ELSE NULL END) Amount4     -- éGéxï•äz4
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DenominationName ELSE NULL END) Name5       -- éGéxï•ñº5
          ,MAX(CASE D.DepositNO WHEN  5 THEN D.DepositGaku      ELSE NULL END) Amount5     -- éGéxï•äz5
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DenominationName ELSE NULL END) Name6       -- éGéxï•ñº6
          ,MAX(CASE D.DepositNO WHEN  6 THEN D.DepositGaku      ELSE NULL END) Amount6     -- éGéxï•äz6
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DenominationName ELSE NULL END) Name7       -- éGéxï•ñº7
          ,MAX(CASE D.DepositNO WHEN  7 THEN D.DepositGaku      ELSE NULL END) Amount7     -- éGéxï•äz7
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DenominationName ELSE NULL END) Name8       -- éGéxï•ñº8
          ,MAX(CASE D.DepositNO WHEN  8 THEN D.DepositGaku      ELSE NULL END) Amount8     -- éGéxï•äz8
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DenominationName ELSE NULL END) Name9       -- éGéxï•ñº9
          ,MAX(CASE D.DepositNO WHEN  9 THEN D.DepositGaku      ELSE NULL END) Amount9     -- éGéxï•äz9
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DenominationName ELSE NULL END) Name10      -- éGéxï•ñº10
          ,MAX(CASE D.DepositNO WHEN 10 THEN D.DepositGaku      ELSE NULL END) Amount10    -- éGéxï•äz10
          ,tempSales.StaffReceiptPrint                                                     -- íSìñÉåÉVÅ[Égï\ãL
          ,tempSales.StoreReceiptPrint                                                     -- ìXï‹ÉåÉVÅ[Égï\ãL
          ,tempSales.StoreCD
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY history.DepositNO ORDER BY history.DepositDateTime DESC) as DepositNO
                  ,CONVERT(DATE, history.DepositDateTime) RegistDate
                  ,denominationKbn.DenominationName
                  ,history.DepositGaku
                  ,history.Number
              FROM D_DepositHistory history
              LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
              LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
             WHERE history.DepositNO = @DepositNO 
               AND history.DataKBN = 3
               AND history.DepositKBN = 3
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

