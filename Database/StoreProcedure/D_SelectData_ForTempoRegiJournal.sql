SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    ìXï‹ÉåÉW ÉWÉÉÅ[ÉiÉãàÛç¸
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.22
--       Update date:    2020.06.06  éGì¸ã‡ÅAéGéxï•ÅAóºë÷édólïœçX
--                       2020.07.17  åèêîÇ™ëùÇ¶ÇÈÇèCê≥
--  ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectData_ForTempoRegiJournal')
  DROP PROCEDURE [dbo].[D_SelectData_ForTempoRegiJournal]
GO


CREATE PROCEDURE [dbo].[D_SelectData_ForTempoRegiJournal]
(
    @StoreCD   varchar(4),
    @DateFrom  varchar(10),
    @DateTo    varchar(10)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    -- ÅyìXï‹ê∏éZÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPçÏê¨
    SELECT * 
      INTO #Temp_D_StoreCalculation1
      FROM (
            SELECT CalculationDate                  -- ê∏éZì˙
                  ,[10000yen] [10000yenNum]         -- åªã‡écçÇ10,000ñáêî
                  ,[5000yen] [5000yenNum]           -- åªã‡écçÇ5,000ñáêî
                  ,[2000yen] [2000yenNum]           -- åªã‡écçÇ2,000ñáêî
                  ,[1000yen] [1000yenNum]           -- åªã‡écçÇ1,000ñáêî
                  ,[500yen] [500yenNum]             -- åªã‡écçÇ500ñáêî
                  ,[100yen] [100yenNum]             -- åªã‡écçÇ100ñáêî
                  ,[50yen] [50yenNum]               -- åªã‡écçÇ50ñáêî
                  ,[10yen] [10yenNum]               -- åªã‡écçÇ10ñáêî
                  ,[5yen] [5yenNum]                 -- åªã‡écçÇ5ñáêî
                  ,[1yen] [1yenNum]                 -- åªã‡écçÇ1ñáêî
                  ,[10000yen]*10000 [10000yenGaku]  -- åªã‡écçÇ10,000ã‡äz
                  ,[5000yen]*5000 [5000yenGaku]     -- åªã‡écçÇ5,000ã‡äz
                  ,[2000yen]*2000 [2000yenGaku]     -- åªã‡écçÇ2,000ã‡äz
                  ,[1000yen]*1000 [1000yenGaku]     -- åªã‡écçÇ1,000ã‡äz
                  ,[500yen]*500 [500yenGaku]        -- åªã‡écçÇ500ã‡äz
                  ,[100yen]*100 [100yenGaku]        -- åªã‡écçÇ100ã‡äz
                  ,[50yen]*50 [50yenGaku]           -- åªã‡écçÇ50ã‡äz
                  ,[10yen]*10 [10yenGaku]           -- åªã‡écçÇ10ã‡äz
                  ,[5yen]*5 [5yenGaku]              -- åªã‡écçÇ5ã‡äz
                  ,[1yen]*1 [1yenGaku]              -- åªã‡écçÇ1ã‡äz
                  ,Change                           -- íﬁëKèÄîıã‡
                  ,Etcyen                           -- ÇªÇÃëºã‡äz
              FROM D_StoreCalculation
             WHERE StoreCD = @StoreCD
               AND CalculationDate >= convert(date, @DateFrom)
               AND CalculationDate <= convert(date, @DateTo)
           ) S1;

    SELECT *
      INTO #Temp_D_DepositHistory0
      FROM (
            SELECT DepositDateTime                  -- ìoò^ì˙
                  ,Number                           -- ì`ï[î‘çÜ
                  ,StoreCD                          -- ìXï‹CD
                  ,SKUCD                            -- 
                  ,JanCD                            -- JanCD
                  ,AdminNO
                  ,SalesSU                          -- 
                  ,SalesUnitPrice                   -- 
                  ,TotalGaku                        -- âøäi
                  ,SalesTax                         -- ê≈äz
                  ,SalesTaxRate                     -- ê≈ó¶
                  ,DataKBN                          -- 
                  ,DepositKBN                       -- 
                  ,CancelKBN                        -- 
                  ,DenominationCD                   -- 
                  ,DepositGaku                      -- 
                  ,Refund                           -- 
                  ,DepositNO                        -- 
                  ,DiscountGaku                     -- 
                  ,CustomerCD                       -- 
                  ,ExchangeDenomination             -- 
                  ,ExchangeCount                    -- 
                  ,[Rows]                           -- 
                  ,AccountingDate                   -- 
                  ,InsertOperator
                  ,Remark
              FROM D_DepositHistory
             WHERE StoreCD = @StoreCD
               AND AccountingDate >= convert(date, @DateFrom)
               AND AccountingDate <= convert(date, @DateTo)
               AND DataKBN IN (2,3)
               AND DepositKBN IN (1,2,3,5,6)
           ) H1;
--Temp_D_DepositHistory0Ç…IndexÅiNumberÅjÇÇ¬ÇØÇΩÇŸÇ§Ç™ÇÊÇ¢ÅHÅö
    -- ÉCÉìÉfÉbÉNÉXindex1çÏê¨
    CREATE CLUSTERED INDEX index_D_DepositHistory0 on [#Temp_D_DepositHistory0] ([Number]);
    
    -- ÅyîÃîÑÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory1
      FROM (
            SELECT distinct history.DepositDateTime RegistDate                  -- ìoò^ì˙éû
                  ,CONVERT(Date, history.DepositDateTime) DepositDate           -- ìoò^ì˙
                  ,history.Number SalesNO                                       -- ì`ï[î‘çÜ
                  ,history.StoreCD                                              -- ìXï‹CD
                  ,history.DepositNO                                            -- SortÇÃÇΩÇﬂÇ…éÊìæ
                  ,history.JanCD                                                -- JanCD
                  ,(SELECT top 1 sku.SKUShortName
                      FROM M_SKU AS sku
                     WHERE sku.AdminNO = history.AdminNO
                       AND sku.DeleteFlg = 0
                       AND sku.ChangeDate <= history.AccountingDate
                     ORDER BY sku.ChangeDate DESC
                   ) As SKUShortName                                            -- è§ïiñº
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesUnitPrice
                   END AS SalesUnitPrice                                        -- íPâø
                  ,CASE
                     WHEN history.SalesSU = 1 THEN NULL
                     ELSE history.SalesSU
                   END AS SalesSU                                               -- êîó 
                  ,history.TotalGaku Kakaku                                     -- âøäi
                  ,history.SalesTax                                             -- ê≈äz
                  ,history.SalesTaxRate                                         -- ê≈ó¶
                  ,history.TotalGaku                                            -- îÃîÑçáåväz
                  ,sales.SalesHontaiGaku8 + sales.SalesTax8 TargetAmount8       -- 8ÅìëŒè€äz
                  ,sales.SalesHontaiGaku10 + sales.SalesTax10 TargetAmount10    -- 10ÅìëŒè€äz
                  ,sales.SalesTax8                                              -- äOê≈8Åì
                  ,sales.SalesTax10                                             -- äOê≈10Åì
                  ,(SELECT top 1 staff.ReceiptPrint
                      FROM M_Staff AS staff
                     WHERE  staff.StaffCD = sales.StaffCD
                       AND staff.DeleteFlg = 0
                       AND staff.ChangeDate <= sales.SalesDate
                     ORDER BY staff.ChangeDate DESC
                    ) AS StaffReceiptPrint                                      -- íSìñÉåÉVÅ[Égï\ãL
                  ,(SELECT top 1 store.ReceiptPrint
                      FROM M_Store AS store
                     WHERE store.StoreCD = sales.StoreCD
                       AND store.DeleteFlg = 0
                       AND store.ChangeDate <= sales.SalesDate
                     ORDER BY store.ChangeDate DESC
                    ) AS StoreReceiptPrint                                      -- ìXï‹ÉåÉVÅ[Égï\ãL
                  ,history.AccountingDate
              FROM #Temp_D_DepositHistory0 AS history
              LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number

             WHERE history.DataKBN = 2
               AND history.DepositKBN = 1
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
           ) D1;

    -- ÅyîÃîÑÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇQçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory2
      FROM (
            SELECT D.SalesNO                                                                     -- ì`ï[î‘çÜ
                  ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) PaymentName1   -- éxï•ï˚ñ@ñº1
                  ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) AmountPay1     -- éxï•ï˚ñ@äz1
                  ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) PaymentName2   -- éxï•ï˚ñ@ñº2
                  ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) AmountPay2     -- éxï•ï˚ñ@äz2
                  ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) PaymentName3   -- éxï•ï˚ñ@ñº3
                  ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) AmountPay3     -- éxï•ï˚ñ@äz3
                  ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) PaymentName4   -- éxï•ï˚ñ@ñº4
                  ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) AmountPay4     -- éxï•ï˚ñ@äz4
                  ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) PaymentName5   -- éxï•ï˚ñ@ñº5
                  ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) AmountPay5     -- éxï•ï˚ñ@äz5
                  ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) PaymentName6   -- éxï•ï˚ñ@ñº6
                  ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) AmountPay6     -- éxï•ï˚ñ@äz6
                  ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) PaymentName7   -- éxï•ï˚ñ@ñº7
                  ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) AmountPay7     -- éxï•ï˚ñ@äz7
                  ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) PaymentName8   -- éxï•ï˚ñ@ñº8
                  ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) AmountPay8     -- éxï•ï˚ñ@äz8
                  ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) PaymentName9   -- éxï•ï˚ñ@ñº9
                  ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) AmountPay9     -- éxï•ï˚ñ@äz9
                  ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) PaymentName10  -- éxï•ï˚ñ@ñº10
                  ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) AmountPay10    -- éxï•ï˚ñ@äz10
              FROM (
                    SELECT history.Number SalesNO
                          ,history.DenominationCD
                          ,denominationKbn.DenominationName
                          ,history.DepositGaku + history.Refund DepositGaku
                          ,history.DepositDateTime
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime ASC) as RANK	Åö
                          ,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositNO ASC) as RANK
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
                      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 1
                       AND history.CancelKBN = 0
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                   ) D
             GROUP BY D.SalesNO
           ) D2;

    -- ÅyîÃîÑÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇRçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory3
      FROM (
            SELECT history.Number  SalesNO                   -- ì`ï[î‘çÜ
                  ,SUM(history.Refund) Refund                -- íﬁëK
                  ,SUM(history.DiscountGaku) DiscountGaku    -- ílà¯äz
              FROM #Temp_D_DepositHistory0 AS history
              LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number
             WHERE history.DataKBN = 3 
               AND history.DepositKBN = 1
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
             GROUP BY history.Number
           ) D3;

    -- ÅyíﬁëKèÄîıÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇSçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory4
      FROM (
            SELECT CONVERT(Date, D.DepositDateTime) RegistDate                             -- ìoò^ì˙
                  ,D.DepositDateTime ChangePreparationDate1                                -- íﬁëKèÄîıì˙1
                  ,'åªã‡' ChangePreparationName1                                           -- íﬁëKèÄîıñº1
                  ,D.DepositGaku ChangePreparationAmount1                                  -- íﬁëKèÄîıäz1
                  ,NULL ChangePreparationDate2                                             -- íﬁëKèÄîıì˙2
                  ,NULL ChangePreparationName2                                             -- íﬁëKèÄîıñº2
                  ,NULL ChangePreparationAmount2                                           -- íﬁëKèÄîıäz2
                  ,NULL ChangePreparationDate3                                             -- íﬁëKèÄîıì˙3
                  ,NULL ChangePreparationName3                                             -- íﬁëKèÄîıñº3
                  ,NULL ChangePreparationAmount3                                           -- íﬁëKèÄîıäz3
                  ,NULL ChangePreparationDate4                                             -- íﬁëKèÄîıì˙4
                  ,NULL ChangePreparationName4                                             -- íﬁëKèÄîıñº4
                  ,NULL ChangePreparationAmount4                                           -- íﬁëKèÄîıäz4
                  ,NULL ChangePreparationDate5                                             -- íﬁëKèÄîıì˙5
                  ,NULL ChangePreparationName5                                             -- íﬁëKèÄîıñº5
                  ,NULL ChangePreparationAmount5                                           -- íﬁëKèÄîıäz5
                  ,NULL ChangePreparationDate6                                             -- íﬁëKèÄîıì˙6
                  ,NULL ChangePreparationName6                                             -- íﬁëKèÄîıñº6
                  ,NULL ChangePreparationAmount6                                           -- íﬁëKèÄîıäz6
                  ,NULL ChangePreparationDate7                                             -- íﬁëKèÄîıì˙7
                  ,NULL ChangePreparationName7                                             -- íﬁëKèÄîıñº7
                  ,NULL ChangePreparationAmount7                                           -- íﬁëKèÄîıäz7
                  ,NULL ChangePreparationDate8                                             -- íﬁëKèÄîıì˙8
                  ,NULL ChangePreparationName8                                             -- íﬁëKèÄîıñº8
                  ,NULL ChangePreparationAmount8                                           -- íﬁëKèÄîıäz8
                  ,NULL ChangePreparationDate9                                             -- íﬁëKèÄîıì˙9
                  ,NULL ChangePreparationName9                                             -- íﬁëKèÄîıñº9
                  ,NULL ChangePreparationAmount9                                           -- íﬁëKèÄîıäz9
                  ,NULL ChangePreparationDate10                                            -- íﬁëKèÄîıì˙10
                  ,NULL ChangePreparationName10                                            -- íﬁëKèÄîıñº10
                  ,NULL ChangePreparationAmount10                                          -- íﬁëKèÄîıäz10
                  ,D.Remark ChangePreparationRemark                                        -- íﬁëKèÄîıîıçl
                  ,D.DepositNO
              FROM #Temp_D_DepositHistory0 D
              INNER JOIN (
                                   SELECT MAX(history.DepositNO) DepositNO
                                     FROM #Temp_D_DepositHistory0 history
                                    WHERE history.DataKBN = 3
                                      AND history.DepositKBN = 6
                                      AND history.CancelKBN = 0
                                    GROUP BY history.AccountingDate
                                  ) AS DD
              ON DD.DepositNO = D.DepositNO
           ) D4;

    -- ÅyéGì¸ã‡ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇTçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory5
      FROM (
           -- SELECT D.RegistDate                                                                          -- ìoò^ì˙
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate1       -- éGì¸ã‡ì˙1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) MiscDepositName1       -- éGì¸ã‡ñº1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount1     -- éGì¸ã‡äz1
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate2       -- éGì¸ã‡ì˙2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) MiscDepositName2       -- éGì¸ã‡ñº2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount2     -- éGì¸ã‡äz2
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate3       -- éGì¸ã‡ì˙3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) MiscDepositName3       -- éGì¸ã‡ñº3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount3     -- éGì¸ã‡äz3
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate4       -- éGì¸ã‡ì˙4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) MiscDepositName4       -- éGì¸ã‡ñº4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount4     -- éGì¸ã‡äz4
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate5       -- éGì¸ã‡ì˙5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) MiscDepositName5       -- éGì¸ã‡ñº5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount5     -- éGì¸ã‡äz5
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate6       -- éGì¸ã‡ì˙6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) MiscDepositName6       -- éGì¸ã‡ñº6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount6     -- éGì¸ã‡äz6
            --      ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate7       -- éGì¸ã‡ì˙7
            --      ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) MiscDepositName7       -- éGì¸ã‡ñº7
            --      ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount7     -- éGì¸ã‡äz7
            --      ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate8       -- éGì¸ã‡ì˙8
            --      ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) MiscDepositName8       -- éGì¸ã‡ñº8
            --      ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount8     -- éGì¸ã‡äz8
            --      ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate9       -- éGì¸ã‡ì˙9
            --      ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) MiscDepositName9       -- éGì¸ã‡ñº9
            --      ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount9     -- éGì¸ã‡äz9
            --      ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) MiscDepositDate10      -- éGì¸ã‡ì˙10
            --      ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) MiscDepositName10      -- éGì¸ã‡ñº10
            --      ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) MiscDepositAmount10    -- éGì¸ã‡äz10
            --      ,MAX(D.Remark) MiscDepositRemark                                                       -- éGì¸ã‡îıçl
            --  FROM (
                    SELECT CONVERT(Date, history.DepositDateTime)  AS RegistDate             -- ìoò^ì˙
                          ,history.DepositDateTime                 AS MiscDepositDate1       -- éGì¸ã‡ì˙1
                          ,denominationKbn.DenominationName        AS MiscDepositName1       -- éGì¸ã‡ñº1
                          ,history.DepositGaku                     AS MiscDepositAmount1     -- éGì¸ã‡äz1
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime ASC) as RANK	Åö
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number,history.DepositNO ORDER BY history.DepositNO ASC) as RANK
                          ,history.DepositNO	                   --2020.11.20 add
                          ,history.InsertOperator                  AS StaffReceiptPrint
                          ,history.Remark                          AS MiscDepositRemark      -- éGì¸ã‡îıçl
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 2
                       AND history.CancelKBN = 0
                       AND history.CustomerCD IS NULL
             --      ) D
             --GROUP BY D.RegistDate, D.DepositNO		--2020.11.20 add(DepositNO)ñ{ìñÇÕèWåvÇ∑ÇÈïKóvÇ»Çµ
           ) D5;

    -- Åyì¸ã‡ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇTÇPçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory51
      FROM (
           -- SELECT D.RegistDate                                                                     -- ìoò^ì˙
           --       ,MAX(CustomerCD) CustomerCD                                                       -- ì¸ã‡å≥CD
           --       ,MAX(CustomerName) CustomerName                                                   -- ì¸ã‡å≥ñº
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) DepositDate1      -- ì¸ã‡ì˙1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) DepositName1      -- ì¸ã‡ñº1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) DepositAmount1    -- ì¸ã‡äz1
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) DepositDate2      -- ì¸ã‡ì˙2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) DepositName2      -- ì¸ã‡ñº2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) DepositAmount2    -- ì¸ã‡äz2
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) DepositDate3      -- ì¸ã‡ì˙3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) DepositName3      -- ì¸ã‡ñº3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) DepositAmount3    -- ì¸ã‡äz3
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) DepositDate4      -- ì¸ã‡ì˙4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) DepositName4      -- ì¸ã‡ñº4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) DepositAmount4    -- ì¸ã‡äz4
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) DepositDate5      -- ì¸ã‡ì˙5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) DepositName5      -- ì¸ã‡ñº5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) DepositAmount5    -- ì¸ã‡äz5
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) DepositDate6      -- ì¸ã‡ì˙6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) DepositName6      -- ì¸ã‡ñº6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) DepositAmount6    -- ì¸ã‡äz6
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) DepositDate7      -- ì¸ã‡ì˙7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) DepositName7      -- ì¸ã‡ñº7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) DepositAmount7    -- ì¸ã‡äz7
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) DepositDate8      -- ì¸ã‡ì˙8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) DepositName8      -- ì¸ã‡ñº8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) DepositAmount8    -- ì¸ã‡äz8
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) DepositDate9      -- ì¸ã‡ì˙9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) DepositName9      -- ì¸ã‡ñº9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) DepositAmount9    -- ì¸ã‡äz9
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) DepositDate10     -- ì¸ã‡ì˙10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) DepositName10     -- ì¸ã‡ñº10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) DepositAmount10   -- ì¸ã‡äz10
           --       ,MAX(D.Remark) DepositRemark                                                      -- ì¸ã‡îıçl
           --   FROM (
                    SELECT CONVERT(Date, history.DepositDateTime) AS RegistDate
                          ,history.DepositDateTime                AS DepositDate1
                          ,history.CustomerCD
                          ,(SELECT top 1 customer.CustomerName
                            FROM M_Customer AS customer
                            WHERE customer.CustomerCD = history.CustomerCD             --DeleteFlgÇÕÇ†Ç¶Çƒïsóv
                            AND customer.ChangeDate <= history.DepositDateTime
                            ORDER BY customer.ChangeDate DESC
                            ) AS CustomerName
                          ,denominationKbn.DenominationName       AS DepositName1
                          ,history.DenominationCD 
                          ,history.DepositGaku                    AS DepositAmount1
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime ASC) as RANK	Åö
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number,history.DepositNO ORDER BY history.DepositNO ASC) as RANK
                          ,history.DepositNO	                  --2020.11.20 add
                          ,history.InsertOperator                 AS StaffReceiptPrint
                          ,history.Remark                         AS DepositRemark
                     FROM #Temp_D_DepositHistory0 history
                     LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD

                    WHERE history.DataKBN = 3
                      AND history.DepositKBN = 2
                      AND history.CancelKBN = 0
                      AND history.CustomerCD IS NOT NULL
           --        ) D
           --  GROUP BY D.RegistDate, D.DepositNO		--2020.11.20 add(DepositNO)ñ{ìñÇÕèWåvÇ∑ÇÈïKóvÇ»Çµ
           ) D51;

    -- ÅyéGéxï•ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇUçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory6
      FROM (
           -- SELECT D.RegistDate                                                                            -- ìoò^ì˙
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate1         -- éGéxï•ì˙1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) MiscPaymentName1         -- éGéxï•ñº1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount1       -- éGéxï•äz1
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate2         -- éGéxï•ì˙2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) MiscPaymentName2         -- éGéxï•ñº2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount2       -- éGéxï•äz2
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate3         -- éGéxï•ì˙3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) MiscPaymentName3         -- éGéxï•ñº3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount3       -- éGéxï•äz3
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate4         -- éGéxï•ì˙4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) MiscPaymentName4         -- éGéxï•ñº4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount4       -- éGéxï•äz4
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate5         -- éGéxï•ì˙5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) MiscPaymentName5         -- éGéxï•ñº5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount5       -- éGéxï•äz5
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate6         -- éGéxï•ì˙6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) MiscPaymentName6         -- éGéxï•ñº6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount6       -- éGéxï•äz6
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate7         -- éGéxï•ì˙7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) MiscPaymentName7         -- éGéxï•ñº7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount7       -- éGéxï•äz7
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate8         -- éGéxï•ì˙8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) MiscPaymentName8         -- éGéxï•ñº8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount8       -- éGéxï•äz8
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate9         -- éGéxï•ì˙9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) MiscPaymentName9         -- éGéxï•ñº9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount9       -- éGéxï•äz9
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime  ELSE NULL END) MiscPaymentDate10        -- éGéxï•ì˙10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) MiscPaymentName10        -- éGéxï•ñº10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) MiscPaymentAmount10      -- éGéxï•äz10
           --       ,MAX(D.Remark) MiscPaymentRemark                                                         -- éGéxï•îıçl
           --   FROM (
                    SELECT CONVERT(Date, history.DepositDateTime) AS RegistDate
                          ,history.DepositDateTime                AS MiscPaymentDate1
                          ,history.DenominationCD
                          ,denominationKbn.DenominationName       AS MiscPaymentName1
                          ,history.DepositGaku                    AS MiscPaymentAmount1
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime ASC) as RANK	Åö
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number,history.DepositNO ORDER BY history.DepositNO ASC) as RANK
                          ,history.Remark                         AS MiscPaymentRemark
                          ,history.DepositNO
                          ,history.AccountingDate
                          ,history.InsertOperator                 AS StaffReceiptPrint
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 3
                       AND history.CancelKBN = 0
            --       ) D
            -- GROUP BY D.RegistDate, D.DepositNO		--2020.11.20 add(DepositNO)ñ{ìñÇÕèWåvÇ∑ÇÈïKóvÇ»Çµ
           ) D6;

    -- Åyóºë÷ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇVçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory7
      FROM (
           -- SELECT D.RegistDate                                                                               -- ìoò^ì˙
           --       ,COUNT(*) ExchangeCount                                                                     -- óºë÷âÒêî
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate1           -- óºë÷ì˙1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName     ELSE NULL END) ExchangeName1           -- óºë÷ñº1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount1         -- óºë÷äz1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination1   -- óºë÷éÜïº1
           --       ,MAX(CASE D.RANK WHEN  1 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount1          -- óºë÷ñáêî1
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate2           -- óºë÷ì˙2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName     ELSE NULL END) ExchangeName2           -- óºë÷ñº2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount2         -- óºë÷äz2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination2   -- óºë÷éÜïº2
           --       ,MAX(CASE D.RANK WHEN  2 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount2          -- óºë÷ñáêî2
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate3           -- óºë÷ì˙3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName     ELSE NULL END) ExchangeName3           -- óºë÷ñº3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount3         -- óºë÷äz3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination3   -- óºë÷éÜïº3
           --       ,MAX(CASE D.RANK WHEN  3 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount3          -- óºë÷ñáêî3
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate4           -- óºë÷ì˙4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName     ELSE NULL END) ExchangeName4           -- óºë÷ñº4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount4         -- óºë÷äz4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination4   -- óºë÷éÜïº4
           --       ,MAX(CASE D.RANK WHEN  4 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount4          -- óºë÷ñáêî4
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate5           -- óºë÷ì˙5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName     ELSE NULL END) ExchangeName5           -- óºë÷ñº5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount5         -- óºë÷äz5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination5   -- óºë÷éÜïº5
           --       ,MAX(CASE D.RANK WHEN  5 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount5          -- óºë÷ñáêî5
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate6           -- óºë÷ì˙6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName     ELSE NULL END) ExchangeName6           -- óºë÷ñº6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount6         -- óºë÷äz6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination6   -- óºë÷éÜïº6
           --       ,MAX(CASE D.RANK WHEN  6 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount6          -- óºë÷ñáêî6
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate7           -- óºë÷ì˙7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName     ELSE NULL END) ExchangeName7           -- óºë÷ñº7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount7         -- óºë÷äz7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination7   -- óºë÷éÜïº7
           --       ,MAX(CASE D.RANK WHEN  7 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount7          -- óºë÷ñáêî7
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate8           -- óºë÷ì˙8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName     ELSE NULL END) ExchangeName8           -- óºë÷ñº8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount8         -- óºë÷äz8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination8   -- óºë÷éÜïº8
           --       ,MAX(CASE D.RANK WHEN  8 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount8          -- óºë÷ñáêî8
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate9           -- óºë÷ì˙9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName     ELSE NULL END) ExchangeName9           -- óºë÷ñº9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount9         -- óºë÷äz9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination9   -- óºë÷éÜïº9
           --       ,MAX(CASE D.RANK WHEN  9 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount9          -- óºë÷ñáêî9
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositDateTime      ELSE NULL END) ExchangeDate10          -- óºë÷ì˙10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName     ELSE NULL END) ExchangeName10          -- óºë÷ñº10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku          ELSE NULL END) ExchangeAmount10        -- óºë÷äz10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeDenomination ELSE NULL END) ExchangeDenomination10  -- óºë÷éÜïº10
           --       ,MAX(CASE D.RANK WHEN 10 THEN D.ExchangeCount        ELSE NULL END) ExchangeCount10         -- óºë÷ñáêî10
           --       ,MAX(D.Remark) ExchangeRemark                                                               -- óºë÷îıçl
           --   FROM (
                    SELECT CONVERT(Date, history.DepositDateTime) AS RegistDate
                          ,COUNT(*) OVER(partition by CONVERT(Date, history.DepositDateTime)) AS ExchangeCount
                          ,history.DepositDateTime                AS ExchangeDate1
                          ,denominationKbn.DenominationName       AS ExchangeName1
                          ,ABS(history.DepositGaku)               AS ExchangeAmount1
                          ,history.ExchangeDenomination           AS ExchangeDenomination1
                          ,ABS(history.ExchangeCount)             AS ExchangeCount1
                          --,ROW_NUMBER() OVER (PARTITION BY  history.Number ORDER BY history.DepositDateTime) AS RANK	Åö
                          --,ROW_NUMBER() OVER(PARTITION BY history.Number,history.DepositNO ORDER BY history.DepositNO ASC) as RANK
                          ,history.DepositNO
                          ,history.InsertOperator                 AS StaffReceiptPrint
                          ,history.Remark                         AS ExchangeRemark
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 5
                       AND history.CancelKBN = 0
         --          ) D
         --    GROUP BY D.RegistDate,D.DepositNO
           ) D7;

    -- Åyê∏éZèàóùÅFåªã‡îÑè„(+)ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇXçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory9
      FROM (
            SELECT D.RegistDate                                   -- ìoò^ì˙
                  ,SUM(D.DepositGaku) DepositGaku                 -- åªã‡îÑè„(+)
              FROM (
                    SELECT history.DepositNO
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,history.DepositGaku
                      FROM #Temp_D_DepositHistory0 AS history
                      LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number
                      LEFT OUTER JOIN M_DenominationKBN AS denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 1
                       AND history.CancelKBN = 0
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                       AND denominationKbn.SystemKBN = 1
                   ) D
             GROUP BY D.RegistDate
           ) D9;

    -- Åyê∏éZèàóùÅFåªã‡ì¸ã‡(+)ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇOçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory10
      FROM (
            SELECT D.RegistDate                                   -- ìoò^ì˙
                  ,SUM(D.DepositGaku) DepositGaku                 -- åªã‡îÑè„(+)
              FROM (
                    SELECT history.DepositNO
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,history.DepositGaku
                      FROM #Temp_D_DepositHistory0 AS history
                     INNER JOIN M_DenominationKBN AS denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 2
                       AND history.CancelKBN = 0
                       AND denominationKbn.SystemKBN = 1
                   ) D
             GROUP BY D.RegistDate
           ) D10;

    -- Åyê∏éZèàóùÅFåªã‡éxï•(-)ÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇPçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory11
      FROM (
            SELECT D.RegistDate                                   -- ìoò^ì˙
                  ,SUM(D.DepositGaku) DepositGaku                 -- åªã‡éxï•(-)
              FROM (
                    SELECT history.DepositNO
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,history.DepositGaku
                      FROM #Temp_D_DepositHistory0 AS history
                     INNER JOIN M_DenominationKBN AS denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 3
                       AND history.CancelKBN = 0
                       AND denominationKbn.SystemKBN = 1
                   ) D
             GROUP BY D.RegistDate
           ) D11;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇQçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory12
      FROM (
            SELECT CONVERT(DATE, history.DepositDateTime) RegistDate 
                  ,COUNT(DISTINCT sales.SalesNO) SalesNOCount
                  ,COUNT(DISTINCT sales.CustomerCD) CustomerCDCount
                  ,SUM(history.SalesSU) SalesSUSum
                  ,SUM(history.TotalGaku) TotalGakuSum
                  ,SUM(history.DiscountGaku) DiscountGaku
              FROM #Temp_D_DepositHistory0 AS history
              LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number
             WHERE history.DataKBN = 2
               AND history.DepositKBN = 1
               AND history.CancelKBN = 0
               AND sales.DeleteDateTime IS NULL
               AND sales.BillingType = 1
             GROUP BY CONVERT(DATE, history.DepositDateTime)
           ) D12;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇRçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory13
      FROM (
            SELECT D.RegistDate                                                 -- ìoò^ì˙
                  ,SUM(D.TaxableAmount) TaxableAmount                           -- ì‡ê≈ï™îÃîÑäzÇÃçáåv
                  ,SUM(D.ForeignTaxableAmount) ForeignTaxableAmount             -- äOê≈ï™îÃîÑäzÇÃçáåv
                  ,SUM(D.TaxExemptionAmount) TaxExemptionAmount                 -- îÒâ€ê≈ï™îÃîÑäzÇÃçáåv
                  ,SUM(D.TotalWithoutTax) TotalWithoutTax                       -- ê≈î≤çáåvÇÃçáåv
                  ,SUM(D.Tax) Tax                                               -- ì‡ê≈ÇÃçáåv
                  ,SUM(D.OutsideTax) OutsideTax                                 -- äOê≈ÇÃçáåv
                  ,SUM(D.ConsumptionTax) ConsumptionTax                         -- è¡îÔê≈åvÇÃçáåv
                  ,SUM(D.TaxIncludedTotal) TaxIncludedTotal                     -- ê≈çûçáåvÇÃçáåv
              FROM (
                    SELECT history.DepositNO                                    -- 
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate    -- ìoò^ì˙
                          ,salesDetails.SalesGaku TaxableAmount                 -- ì‡ê≈ï™îÃîÑäz
                          ,0 ForeignTaxableAmount                               -- äOê≈ï™îÃîÑäz
                          ,0 TaxExemptionAmount                                 -- îÒâ€ê≈ï™îÃîÑäz
                          ,salesDetails.SalesHontaiGaku TotalWithoutTax         -- ê≈î≤çáåv
                          ,salesDetails.SalesTax Tax                            -- ì‡ê≈
                          ,0 OutsideTax                                         -- äOê≈
                          ,salesDetails.SalesTax ConsumptionTax                 -- è¡îÔê≈åv
                          ,salesDetails.SalesGaku TaxIncludedTotal              -- ê≈çûçáåv
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN D_SalesDetails AS salesDetails ON salesDetails.SalesNO = history.Number
                                                                 AND salesDetails.SalesRows = history.[Rows]
                      LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = salesDetails.SalesNO
                     WHERE history.DataKBN = 2
                       AND history.DepositKBN = 1
                       AND history.CancelKBN = 0
                       AND salesDetails.DeleteDateTime IS NULL
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                   ) D
             GROUP BY D.RegistDate
           ) D13;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇSçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory14
      FROM (
            SELECT D.RegistDate                                                                                      -- ìoò^ì˙
                  ,MAX(CASE D.DenominationCD WHEN  1 THEN D.DenominationName  ELSE null END) AS denominationName1    -- ã‡éÌãÊï™ñº1
                  ,MAX(CASE D.DenominationCD WHEN  1 THEN D.Kingaku           ELSE null END) AS Kingaku1             -- ã‡äz1
                  ,MAX(CASE d.DenominationCD WHEN  2 THEN D.DenominationName  ELSE null END) AS denominationName2    -- ã‡éÌãÊï™ñº2
                  ,MAX(CASE d.DenominationCD WHEN  2 THEN D.Kingaku           ELSE null END) AS Kingaku2             -- ã‡äz2
                  ,MAX(CASE d.DenominationCD WHEN  3 THEN D.DenominationName  ELSE null END) AS denominationName3    -- ã‡éÌãÊï™ñº3
                  ,MAX(CASE d.DenominationCD WHEN  3 THEN D.Kingaku           ELSE null END) AS Kingaku3             -- ã‡äz3
                  ,MAX(CASE d.DenominationCD WHEN  4 THEN D.DenominationName  ELSE null END) AS denominationName4    -- ã‡éÌãÊï™ñº4
                  ,MAX(CASE d.DenominationCD WHEN  4 THEN D.Kingaku           ELSE null END) AS Kingaku4             -- ã‡äz4
                  ,MAX(CASE d.DenominationCD WHEN  5 THEN D.DenominationName  ELSE null END) AS denominationName5    -- ã‡éÌãÊï™ñº5
                  ,MAX(CASE d.DenominationCD WHEN  5 THEN D.Kingaku           ELSE null END) AS Kingaku5             -- ã‡äz5
                  ,MAX(CASE d.DenominationCD WHEN  6 THEN D.DenominationName  ELSE null END) AS denominationName6    -- ã‡éÌãÊï™ñº6
                  ,MAX(CASE d.DenominationCD WHEN  6 THEN D.Kingaku           ELSE null END) AS Kingaku6             -- ã‡äz6
                  ,MAX(CASE d.DenominationCD WHEN  7 THEN D.DenominationName  ELSE null END) AS denominationName7    -- ã‡éÌãÊï™ñº7
                  ,MAX(CASE d.DenominationCD WHEN  7 THEN D.Kingaku           ELSE null END) AS Kingaku7             -- ã‡äz7
                  ,MAX(CASE d.DenominationCD WHEN  8 THEN D.DenominationName  ELSE null END) AS denominationName8    -- ã‡éÌãÊï™ñº8
                  ,MAX(CASE d.DenominationCD WHEN  8 THEN D.Kingaku           ELSE null END) AS Kingaku8             -- ã‡äz8
                  ,MAX(CASE d.DenominationCD WHEN  9 THEN D.DenominationName  ELSE null END) AS denominationName9    -- ã‡éÌãÊï™ñº9
                  ,MAX(CASE d.DenominationCD WHEN  9 THEN D.Kingaku           ELSE null END) AS Kingaku9             -- ã‡äz9
                  ,MAX(CASE d.DenominationCD WHEN 10 THEN D.DenominationName  ELSE null END) AS denominationName10   -- ã‡éÌãÊï™ñº10
                  ,MAX(CASE d.DenominationCD WHEN 10 THEN D.Kingaku           ELSE null END) AS Kingaku10            -- ã‡äz10
                  ,MAX(CASE d.DenominationCD WHEN 11 THEN D.DenominationName  ELSE null END) AS denominationName11   -- ã‡éÌãÊï™ñº11
                  ,MAX(CASE d.DenominationCD WHEN 11 THEN D.Kingaku           ELSE null END) AS Kingaku11            -- ã‡äz11
                  ,MAX(CASE d.DenominationCD WHEN 12 THEN D.DenominationName  ELSE null END) AS denominationName12   -- ã‡éÌãÊï™ñº12
                  ,MAX(CASE d.DenominationCD WHEN 12 THEN D.Kingaku           ELSE null END) AS Kingaku12            -- ã‡äz12
                  ,MAX(CASE d.DenominationCD WHEN 13 THEN D.DenominationName  ELSE null END) AS denominationName13   -- ã‡éÌãÊï™ñº13
                  ,MAX(CASE d.DenominationCD WHEN 13 THEN D.Kingaku           ELSE null END) AS Kingaku13            -- ã‡äz13
                  ,MAX(CASE d.DenominationCD WHEN 14 THEN D.DenominationName  ELSE null END) AS denominationName14   -- ã‡éÌãÊï™ñº14
                  ,MAX(CASE d.DenominationCD WHEN 14 THEN D.Kingaku           ELSE null END) AS Kingaku14            -- ã‡äz14
                  ,MAX(CASE d.DenominationCD WHEN 15 THEN D.DenominationName  ELSE null END) AS denominationName15   -- ã‡éÌãÊï™ñº15
                  ,MAX(CASE d.DenominationCD WHEN 15 THEN D.Kingaku           ELSE null END) AS Kingaku15            -- ã‡äz15
                  ,MAX(CASE d.DenominationCD WHEN 16 THEN D.DenominationName  ELSE null END) AS denominationName16   -- ã‡éÌãÊï™ñº16
                  ,MAX(CASE d.DenominationCD WHEN 16 THEN D.Kingaku           ELSE null END) AS Kingaku16            -- ã‡äz16
                  ,MAX(CASE d.DenominationCD WHEN 17 THEN D.DenominationName  ELSE null END) AS denominationName17   -- ã‡éÌãÊï™ñº17
                  ,MAX(CASE d.DenominationCD WHEN 17 THEN D.Kingaku           ELSE null END) AS Kingaku17            -- ã‡äz17
                  ,MAX(CASE d.DenominationCD WHEN 18 THEN D.DenominationName  ELSE null END) AS denominationName18   -- ã‡éÌãÊï™ñº18
                  ,MAX(CASE d.DenominationCD WHEN 18 THEN D.Kingaku           ELSE null END) AS Kingaku18            -- ã‡äz18
                  ,MAX(CASE d.DenominationCD WHEN 19 THEN D.DenominationName  ELSE null END) AS denominationName19   -- ã‡éÌãÊï™ñº19
                  ,MAX(CASE d.DenominationCD WHEN 19 THEN D.Kingaku           ELSE null END) AS Kingaku19            -- ã‡äz19
                  ,MAX(CASE d.DenominationCD WHEN 20 THEN D.DenominationName  ELSE null END) AS denominationName20   -- ã‡éÌãÊï™ñº20
                  ,MAX(CASE d.DenominationCD WHEN 20 THEN D.Kingaku           ELSE null END) AS Kingaku20            -- ã‡äz20
              FROM (
                    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,denominationKbn.DenominationCD 
                          ,MAX(CASE WHEN denominationKbn.SystemKBN = 2 THEN multiPorpose.IDName
                                    ELSE denominationKbn.DenominationName 
                               END) DenominationName
                          ,SUM(history.DepositGaku) Kingaku
                      FROM #Temp_D_DepositHistory0 history
                      LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.number
                      LEFT OUTER JOIN M_DenominationKBN AS denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                      LEFT OUTER JOIN M_MultiPorpose AS multiporpose ON multiporpose.id = 303
                                                                 AND multiporpose.[key] = denominationKbn.CardCompany 
                     WHERE history.DataKBN = 3
                       AND history.DepositKBN = 1
                       AND history.CancelKBN = 0
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                     GROUP BY CONVERT(DATE, history.DepositDateTime)
                             ,denominationKbn.DenominationCD
                             ,denominationKbn.CardCompany
                   ) D
             GROUP BY D.RegistDate
           ) D14;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇTçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory15
      FROM (
            SELECT D.RegistDate
                  ,SUM(DepositTransfer) DepositTransfer      -- ì¸ã‡ êUçû
                  ,SUM(DepositCash) DepositCash              -- ì¸ã‡ åªã‡
                  ,SUM(DepositCheck) DepositCheck            -- ì¸ã‡ è¨êÿéË
                  ,SUM(DepositBill) DepositBill              -- ì¸ã‡ éËå`
                  ,SUM(DepositOffset) DepositOffset          -- ì¸ã‡ ëäéE
                  ,SUM(DepositAdjustment) DepositAdjustment  -- ì¸ã‡ í≤êÆ
                  ,SUM(PaymentTransfer) PaymentTransfer      -- éxï• êUçû
                  ,SUM(PaymentCash) PaymentCash              -- éxï• åªã‡
                  ,SUM(PaymentCheck) PaymentCheck            -- éxï• è¨êÿéË
                  ,SUM(PaymentBill) PaymentBill              -- éxï• éËå`
                  ,SUM(PaymentOffset) PaymentOffset          -- éxï• ëäéE
                  ,SUM(PaymentAdjustment) PaymentAdjustment  -- éxï• í≤êÆ
              FROM (
                    SELECT history.DepositNO
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 5 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositTransfer    -- ì¸ã‡ êUçû
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 1 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositCash        -- ì¸ã‡ åªã‡
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 6 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositCheck       -- ì¸ã‡ è¨êÿéË
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 11 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositBill        -- ì¸ã‡ éËå`
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 7 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositOffset      -- ì¸ã‡ ëäéE
                          ,CASE WHEN history.DepositKBN = 2 AND denominationKbn.SystemKBN = 12 THEN history.DepositGaku
                                ELSE 0
                           END AS DepositAdjustment  -- ì¸ã‡ í≤êÆ
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 5 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentTransfer    -- éxï• êUçû
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 1 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentCash        -- éxï• åªã‡
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 6 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentCheck       -- éxï• è¨êÿéË
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 11 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentBill        -- éxï• éËå`
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 7 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentOffset      -- éxï• ëäéE
                          ,CASE WHEN history.DepositKBN = 3 AND denominationKbn.SystemKBN = 12 THEN history.DepositGaku
                                ELSE 0
                           END AS PaymentAdjustment  -- éxï• í≤êÆ
                      FROM #Temp_D_DepositHistory0 AS history
                      LEFT OUTER JOIN M_DenominationKBN AS denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
                     WHERE history.DataKBN = 3
                       AND history.CancelKBN = 0
                   ) D
             GROUP BY D.RegistDate
           ) D15;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇUçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory16
      FROM (
            SELECT RegistDate                                                              -- ìoò^ì˙
                  ,SUM(OtherAmountReturns) OtherAmountReturns                              -- ëºåªã‡ ï‘ïi
                  ,SUM(OtherAmountDiscount) OtherAmountDiscount                            -- ëºåªã‡ ílà¯
                  ,SUM(OtherAmountCancel) OtherAmountCancel                                -- ëºåªã‡ ílà¯
                  ,SUM(OtherAmountDelivery) OtherAmountDelivery                            -- ëºåªã‡ îzíB
              FROM (
                    SELECT history.DepositNO 
                          ,CONVERT(DATE, history.DepositDateTime) RegistDate               -- ìoò^ì˙
                          ,CASE WHEN history.CancelKBN = 2 THEN history.DepositGaku
                                ELSE 0
                           END AS OtherAmountReturns                                       -- ëºåªã‡ ï‘ïi
                          ,0 OtherAmountDiscount                                           -- ëºåªã‡ ílà¯
                          ,CASE WHEN history.CancelKBN = 1 THEN history.DepositGaku
                                ELSE 0
                           END AS OtherAmountCancel                                        -- ëºåªã‡ ílà¯
                          ,0 OtherAmountDelivery                                           -- ëºåªã‡ îzíB
                      FROM #Temp_D_DepositHistory0 AS history
                      LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number
                     WHERE history.DataKBN = 2
                       AND history.DepositKBN = 1
                       AND history.CancelKBN IN (1, 2)
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                   ) D
             GROUP BY D.RegistDate
           ) D16;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇPÇVçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory17
      FROM (
            SELECT RegistDate                                                              -- ìoò^ì˙
                  ,SUM(ByTimeZoneTaxIncluded_0000_0100) ByTimeZoneTaxIncluded_0000_0100    -- éûä‘ë—ï (ê≈çû) 00:00Å`01:00
                  ,SUM(ByTimeZoneTaxIncluded_0100_0200) ByTimeZoneTaxIncluded_0100_0200    -- éûä‘ë—ï (ê≈çû) 01:00Å`02:00
                  ,SUM(ByTimeZoneTaxIncluded_0200_0300) ByTimeZoneTaxIncluded_0200_0300    -- éûä‘ë—ï (ê≈çû) 02:00Å`03:00
                  ,SUM(ByTimeZoneTaxIncluded_0300_0400) ByTimeZoneTaxIncluded_0300_0400    -- éûä‘ë—ï (ê≈çû) 03:00Å`04:00
                  ,SUM(ByTimeZoneTaxIncluded_0400_0500) ByTimeZoneTaxIncluded_0400_0500    -- éûä‘ë—ï (ê≈çû) 04:00Å`05:00
                  ,SUM(ByTimeZoneTaxIncluded_0500_0600) ByTimeZoneTaxIncluded_0500_0600    -- éûä‘ë—ï (ê≈çû) 05:00Å`06:00
                  ,SUM(ByTimeZoneTaxIncluded_0600_0700) ByTimeZoneTaxIncluded_0600_0700    -- éûä‘ë—ï (ê≈çû) 06:00Å`07:00
                  ,SUM(ByTimeZoneTaxIncluded_0700_0800) ByTimeZoneTaxIncluded_0700_0800    -- éûä‘ë—ï (ê≈çû) 07:00Å`08:00
                  ,SUM(ByTimeZoneTaxIncluded_0800_0900) ByTimeZoneTaxIncluded_0800_0900    -- éûä‘ë—ï (ê≈çû) 08:00Å`09:00
                  ,SUM(ByTimeZoneTaxIncluded_0900_1000) ByTimeZoneTaxIncluded_0900_1000    -- éûä‘ë—ï (ê≈çû) 09:00Å`10:00
                  ,SUM(ByTimeZoneTaxIncluded_1000_1100) ByTimeZoneTaxIncluded_1000_1100    -- éûä‘ë—ï (ê≈çû) 10:00Å`11:00
                  ,SUM(ByTimeZoneTaxIncluded_1100_1200) ByTimeZoneTaxIncluded_1100_1200    -- éûä‘ë—ï (ê≈çû) 11:00Å`12:00
                  ,SUM(ByTimeZoneTaxIncluded_1200_1300) ByTimeZoneTaxIncluded_1200_1300    -- éûä‘ë—ï (ê≈çû) 12:00Å`13:00
                  ,SUM(ByTimeZoneTaxIncluded_1300_1400) ByTimeZoneTaxIncluded_1300_1400    -- éûä‘ë—ï (ê≈çû) 13:00Å`14:00
                  ,SUM(ByTimeZoneTaxIncluded_1400_1500) ByTimeZoneTaxIncluded_1400_1500    -- éûä‘ë—ï (ê≈çû) 14:00Å`15:00
                  ,SUM(ByTimeZoneTaxIncluded_1500_1600) ByTimeZoneTaxIncluded_1500_1600    -- éûä‘ë—ï (ê≈çû) 15:00Å`16:00
                  ,SUM(ByTimeZoneTaxIncluded_1600_1700) ByTimeZoneTaxIncluded_1600_1700    -- éûä‘ë—ï (ê≈çû) 16:00Å`17:00
                  ,SUM(ByTimeZoneTaxIncluded_1700_1800) ByTimeZoneTaxIncluded_1700_1800    -- éûä‘ë—ï (ê≈çû) 17:00Å`18:00
                  ,SUM(ByTimeZoneTaxIncluded_1800_1900) ByTimeZoneTaxIncluded_1800_1900    -- éûä‘ë—ï (ê≈çû) 18:00Å`19:00
                  ,SUM(ByTimeZoneTaxIncluded_1900_2000) ByTimeZoneTaxIncluded_1900_2000    -- éûä‘ë—ï (ê≈çû) 19:00Å`20:00
                  ,SUM(ByTimeZoneTaxIncluded_2000_2100) ByTimeZoneTaxIncluded_2000_2100    -- éûä‘ë—ï (ê≈çû) 20:00Å`21:00
                  ,SUM(ByTimeZoneTaxIncluded_2100_2200) ByTimeZoneTaxIncluded_2100_2200    -- éûä‘ë—ï (ê≈çû) 21:00Å`22:00
                  ,SUM(ByTimeZoneTaxIncluded_2200_2300) ByTimeZoneTaxIncluded_2200_2300    -- éûä‘ë—ï (ê≈çû) 22:00Å`23:00
                  ,SUM(ByTimeZoneTaxIncluded_2300_2400) ByTimeZoneTaxIncluded_2300_2400    -- éûä‘ë—ï (ê≈çû) 23:00Å`24:00
                  ,COUNT(ByTimeZoneSalesNO_0000_0100) ByTimeZoneSalesNO_0000_0100          -- éûä‘ë—ï (îÑè„î‘çÜ) 00:00Å`01:00
                  ,COUNT(ByTimeZoneSalesNO_0100_0200) ByTimeZoneSalesNO_0100_0200          -- éûä‘ë—ï (îÑè„î‘çÜ) 01:00Å`02:00
                  ,COUNT(ByTimeZoneSalesNO_0200_0300) ByTimeZoneSalesNO_0200_0300          -- éûä‘ë—ï (îÑè„î‘çÜ) 02:00Å`03:00
                  ,COUNT(ByTimeZoneSalesNO_0300_0400) ByTimeZoneSalesNO_0300_0400          -- éûä‘ë—ï (îÑè„î‘çÜ) 03:00Å`04:00
                  ,COUNT(ByTimeZoneSalesNO_0400_0500) ByTimeZoneSalesNO_0400_0500          -- éûä‘ë—ï (îÑè„î‘çÜ) 04:00Å`05:00
                  ,COUNT(ByTimeZoneSalesNO_0500_0600) ByTimeZoneSalesNO_0500_0600          -- éûä‘ë—ï (îÑè„î‘çÜ) 05:00Å`06:00
                  ,COUNT(ByTimeZoneSalesNO_0600_0700) ByTimeZoneSalesNO_0600_0700          -- éûä‘ë—ï (îÑè„î‘çÜ) 06:00Å`07:00
                  ,COUNT(ByTimeZoneSalesNO_0700_0800) ByTimeZoneSalesNO_0700_0800          -- éûä‘ë—ï (îÑè„î‘çÜ) 07:00Å`08:00
                  ,COUNT(ByTimeZoneSalesNO_0800_0900) ByTimeZoneSalesNO_0800_0900          -- éûä‘ë—ï (îÑè„î‘çÜ) 08:00Å`09:00
                  ,COUNT(ByTimeZoneSalesNO_0900_1000) ByTimeZoneSalesNO_0900_1000          -- éûä‘ë—ï (îÑè„î‘çÜ) 09:00Å`10:00
                  ,COUNT(ByTimeZoneSalesNO_1000_1100) ByTimeZoneSalesNO_1000_1100          -- éûä‘ë—ï (îÑè„î‘çÜ) 10:00Å`11:00
                  ,COUNT(ByTimeZoneSalesNO_1100_1200) ByTimeZoneSalesNO_1100_1200          -- éûä‘ë—ï (îÑè„î‘çÜ) 11:00Å`12:00
                  ,COUNT(ByTimeZoneSalesNO_1200_1300) ByTimeZoneSalesNO_1200_1300          -- éûä‘ë—ï (îÑè„î‘çÜ) 12:00Å`13:00
                  ,COUNT(ByTimeZoneSalesNO_1300_1400) ByTimeZoneSalesNO_1300_1400          -- éûä‘ë—ï (îÑè„î‘çÜ) 13:00Å`14:00
                  ,COUNT(ByTimeZoneSalesNO_1400_1500) ByTimeZoneSalesNO_1400_1500          -- éûä‘ë—ï (îÑè„î‘çÜ) 14:00Å`15:00
                  ,COUNT(ByTimeZoneSalesNO_1500_1600) ByTimeZoneSalesNO_1500_1600          -- éûä‘ë—ï (îÑè„î‘çÜ) 15:00Å`16:00
                  ,COUNT(ByTimeZoneSalesNO_1600_1700) ByTimeZoneSalesNO_1600_1700          -- éûä‘ë—ï (îÑè„î‘çÜ) 16:00Å`17:00
                  ,COUNT(ByTimeZoneSalesNO_1700_1800) ByTimeZoneSalesNO_1700_1800          -- éûä‘ë—ï (îÑè„î‘çÜ) 17:00Å`18:00
                  ,COUNT(ByTimeZoneSalesNO_1800_1900) ByTimeZoneSalesNO_1800_1900          -- éûä‘ë—ï (îÑè„î‘çÜ) 18:00Å`19:00
                  ,COUNT(ByTimeZoneSalesNO_1900_2000) ByTimeZoneSalesNO_1900_2000          -- éûä‘ë—ï (îÑè„î‘çÜ) 19:00Å`20:00
                  ,COUNT(ByTimeZoneSalesNO_2000_2100) ByTimeZoneSalesNO_2000_2100          -- éûä‘ë—ï (îÑè„î‘çÜ) 20:00Å`21:00
                  ,COUNT(ByTimeZoneSalesNO_2100_2200) ByTimeZoneSalesNO_2100_2200          -- éûä‘ë—ï (îÑè„î‘çÜ) 21:00Å`22:00
                  ,COUNT(ByTimeZoneSalesNO_2200_2300) ByTimeZoneSalesNO_2200_2300          -- éûä‘ë—ï (îÑè„î‘çÜ) 22:00Å`23:00
                  ,COUNT(ByTimeZoneSalesNO_2300_2400) ByTimeZoneSalesNO_2300_2400          -- éûä‘ë—ï (îÑè„î‘çÜ) 23:00Å`24:00
              FROM (
                    SELECT --history.DepositNO 
                          --,
                          CONVERT(DATE, history.DepositDateTime) RegistDate  -- ìoò^ì˙
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '00:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '01:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0000_0100  -- éûä‘ë—ï (ê≈çû) 00:00Å`01:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '01:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '02:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0100_0200  -- éûä‘ë—ï (ê≈çû) 01:00Å`02:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '02:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '03:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0200_0300  -- éûä‘ë—ï (ê≈çû) 02:00Å`03:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '03:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '04:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0300_0400  -- éûä‘ë—ï (ê≈çû) 03:00Å`04:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '04:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '05:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0400_0500  -- éûä‘ë—ï (ê≈çû) 04:00Å`05:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '05:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '06:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0500_0600  -- éûä‘ë—ï (ê≈çû) 05:00Å`06:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '06:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '07:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0600_0700  -- éûä‘ë—ï (ê≈çû) 06:00Å`07:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '07:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '08:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0700_0800  -- éûä‘ë—ï (ê≈çû) 07:00Å`08:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '08:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '09:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0800_0900  -- éûä‘ë—ï (ê≈çû) 08:00Å`09:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '09:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '10:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_0900_1000  -- éûä‘ë—ï (ê≈çû) 09:00Å`10:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '10:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '11:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1000_1100  -- éûä‘ë—ï (ê≈çû) 10:00Å`11:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '11:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '12:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1100_1200  -- éûä‘ë—ï (ê≈çû) 11:00Å`12:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '12:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '13:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1200_1300  -- éûä‘ë—ï (ê≈çû) 12:00Å`13:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '13:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '14:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1300_1400  -- éûä‘ë—ï (ê≈çû) 13:00Å`14:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '14:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '15:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1400_1500  -- éûä‘ë—ï (ê≈çû) 14:00Å`15:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '15:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '16:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1500_1600  -- éûä‘ë—ï (ê≈çû) 15:00Å`16:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '16:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '17:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1600_1700  -- éûä‘ë—ï (ê≈çû) 16:00Å`17:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '17:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '18:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1700_1800  -- éûä‘ë—ï (ê≈çû) 17:00Å`18:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '18:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '19:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1800_1900  -- éûä‘ë—ï (ê≈çû) 18:00Å`19:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '19:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '20:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_1900_2000  -- éûä‘ë—ï (ê≈çû) 19:00Å`20:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '20:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '21:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_2000_2100  -- éûä‘ë—ï (ê≈çû) 20:00Å`21:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '21:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '22:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_2100_2200  -- éûä‘ë—ï (ê≈çû) 21:00Å`22:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '22:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '23:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_2200_2300  -- éûä‘ë—ï (ê≈çû) 22:00Å`23:00
                          ,SUM(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '23:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '24:00' THEN history.TotalGaku
                                ELSE 0
                           END) AS ByTimeZoneTaxIncluded_2300_2400  -- éûä‘ë—ï (ê≈çû) 23:00Å`24:00
                           -- ----------------------------------------------------------------------------------------------------------------------------------------
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '00:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '01:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0000_0100  -- éûä‘ë—ï (îÑè„î‘çÜ) 00:00Å`01:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '01:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '02:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0100_0200  -- éûä‘ë—ï (îÑè„î‘çÜ) 01:00Å`02:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '02:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '03:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0200_0300  -- éûä‘ë—ï (îÑè„î‘çÜ) 02:00Å`03:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '03:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '04:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0300_0400  -- éûä‘ë—ï (îÑè„î‘çÜ) 03:00Å`04:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '04:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '05:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0400_0500  -- éûä‘ë—ï (îÑè„î‘çÜ) 04:00Å`05:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '05:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '06:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0500_0600  -- éûä‘ë—ï (îÑè„î‘çÜ) 05:00Å`06:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '06:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '07:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0600_0700  -- éûä‘ë—ï (îÑè„î‘çÜ) 06:00Å`07:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '07:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '08:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0700_0800  -- éûä‘ë—ï (îÑè„î‘çÜ) 07:00Å`08:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '08:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '09:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0800_0900  -- éûä‘ë—ï (îÑè„î‘çÜ) 08:00Å`09:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '09:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '10:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_0900_1000  -- éûä‘ë—ï (îÑè„î‘çÜ) 09:00Å`10:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '10:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '11:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1000_1100  -- éûä‘ë—ï (îÑè„î‘çÜ) 10:00Å`11:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '11:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '12:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1100_1200  -- éûä‘ë—ï (îÑè„î‘çÜ) 11:00Å`12:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '12:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '13:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1200_1300  -- éûä‘ë—ï (îÑè„î‘çÜ) 12:00Å`13:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '13:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '14:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1300_1400  -- éûä‘ë—ï (îÑè„î‘çÜ) 13:00Å`14:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '14:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '15:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1400_1500  -- éûä‘ë—ï (îÑè„î‘çÜ) 14:00Å`15:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '15:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '16:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1500_1600  -- éûä‘ë—ï (îÑè„î‘çÜ) 15:00Å`16:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '16:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '17:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1600_1700  -- éûä‘ë—ï (îÑè„î‘çÜ) 16:00Å`17:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '17:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '18:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1700_1800  -- éûä‘ë—ï (îÑè„î‘çÜ) 17:00Å`18:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '18:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '19:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1800_1900  -- éûä‘ë—ï (îÑè„î‘çÜ) 18:00Å`19:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '19:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '20:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_1900_2000  -- éûä‘ë—ï (îÑè„î‘çÜ) 19:00Å`20:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '20:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '21:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_2000_2100  -- éûä‘ë—ï (îÑè„î‘çÜ) 20:00Å`21:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '21:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '22:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_2100_2200  -- éûä‘ë—ï (îÑè„î‘çÜ) 21:00Å`22:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '22:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '23:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_2200_2300  -- éûä‘ë—ï (îÑè„î‘çÜ) 22:00Å`23:00
                          ,MAX(CASE WHEN FORMAT(history.DepositDateTime, 'HH:mm') >= '23:00' AND FORMAT(history.DepositDateTime, 'HH:mm') < '24:00' THEN sales.SalesNO
                                ELSE NULL
                           END) AS ByTimeZoneSalesNO_2300_2400  -- éûä‘ë—ï (îÑè„î‘çÜ) 23:00Å`24:00
                      FROM #Temp_D_DepositHistory0 AS history
                      LEFT OUTER JOIN D_Sales AS sales ON sales.SalesNO = history.Number
                     WHERE history.DataKBN = 2
                       AND history.DepositKBN = 1
                       AND history.CancelKBN = 0
                       AND sales.DeleteDateTime IS NULL
                       AND sales.BillingType = 1
                     group by CONVERT(DATE, history.DepositDateTime), sales.SalesNO
                   ) D
             GROUP BY D.RegistDate
           ) D17;

    -- Åyê∏éZèàóùÅzÉèÅ[ÉNÉeÅ[ÉuÉãÇWçÏê¨
    SELECT * 
      INTO #Temp_D_DepositHistory8
      FROM (
            SELECT storeCalculation.CalculationDate RegistDate    -- ìoò^ì˙
                  ,7 DisplayOrder                                 -- ñæç◊ï\é¶èáà 
                  ,storeCalculation.[10000yenNum]                 -- åªã‡écçÇ10,000ñáêî
                  ,storeCalculation.[5000yenNum]                  -- åªã‡écçÇ5,000ñáêî
                  ,storeCalculation.[2000yenNum]                  -- åªã‡écçÇ2,000ñáêî
                  ,storeCalculation.[1000yenNum]                  -- åªã‡écçÇ1,000ñáêî
                  ,storeCalculation.[500yenNum]                   -- åªã‡écçÇ500ñáêî
                  ,storeCalculation.[100yenNum]                   -- åªã‡écçÇ100ñáêî
                  ,storeCalculation.[50yenNum]                    -- åªã‡écçÇ50ñáêî
                  ,storeCalculation.[10yenNum]                    -- åªã‡écçÇ10ñáêî
                  ,storeCalculation.[5yenNum]                     -- åªã‡écçÇ5ñáêî
                  ,storeCalculation.[1yenNum]                     -- åªã‡écçÇ1ñáêî
                  ,storeCalculation.[10000yenGaku]                -- åªã‡écçÇ10,000ã‡äz
                  ,storeCalculation.[5000yenGaku]                 -- åªã‡écçÇ5,000ã‡äz
                  ,storeCalculation.[2000yenGaku]                 -- åªã‡écçÇ2,000ã‡äz
                  ,storeCalculation.[1000yenGaku]                 -- åªã‡écçÇ1,000ã‡äz
                  ,storeCalculation.[500yenGaku]                  -- åªã‡écçÇ500ã‡äz
                  ,storeCalculation.[100yenGaku]                  -- åªã‡écçÇ100ã‡äz
                  ,storeCalculation.[50yenGaku]                   -- åªã‡écçÇ50ã‡äz
                  ,storeCalculation.[10yenGaku]                   -- åªã‡écçÇ10ã‡äz
                  ,storeCalculation.[5yenGaku]                    -- åªã‡écçÇ5ã‡äz
                  ,storeCalculation.[1yenGaku]                    -- åªã‡écçÇ1ã‡äz
                  ,storeCalculation.Etcyen                        -- ÇªÇÃëºã‡äz
                  ,storeCalculation.Change                        -- íﬁëKèÄîıã‡
                  ,tempHistory9.DepositGaku                       -- åªã‡écçÇ åªã‡îÑè„(+)
                  ,tempHistory10.DepositGaku CashDeposit          -- åªã‡écçÇ åªã‡ì¸ã‡(+)
                  ,tempHistory11.DepositGaku CashPayment          -- åªã‡écçÇ åªã‡éxï•(-) 
                  ,storeCalculation.[10000yenGaku]
                    + storeCalculation.[5000yenGaku]
                    + storeCalculation.[2000yenGaku]
                    + storeCalculation.[1000yenGaku]
                    + storeCalculation.[500yenGaku]
                    + storeCalculation.[100yenGaku]
                    + storeCalculation.[50yenGaku]
                    + storeCalculation.[10yenGaku]
                    + storeCalculation.[5yenGaku]
                    + storeCalculation.[1yenGaku]
                    + storeCalculation.Etcyen
                   AS CashBalance                                 -- åªã‡écçÇ åªã‡écçÇ10,000ã‡äzÅ`ÇªÇÃëºã‡äzÇ‹Ç≈ÇÃçáåv
                  ,storeCalculation.Change
                    + tempHistory9.DepositGaku
                    + tempHistory10.DepositGaku
                    - tempHistory11.DepositGaku
                  AS ComputerTotal                               -- ∫›Àﬂ≠∞¿åv íﬁëKèÄîıã‡Å`åªã‡écçÇ åªã‡éxï•(-)Ç‹Ç≈ÇÃçáåv
                  ,(
                    storeCalculation.[10000yenGaku]
                     + storeCalculation.[5000yenGaku]
                     + storeCalculation.[2000yenGaku]
                     + storeCalculation.[1000yenGaku]
                     + storeCalculation.[500yenGaku]
                     + storeCalculation.[100yenGaku]
                     + storeCalculation.[50yenGaku]
                     + storeCalculation.[10yenGaku]
                     + storeCalculation.[5yenGaku]
                     + storeCalculation.[1yenGaku]
                     + storeCalculation.Etcyen
                   ) - (
                    storeCalculation.Change
                     + tempHistory9.DepositGaku
                     + tempHistory10.DepositGaku
                     - tempHistory11.DepositGaku
                  ) AS CashShortage                              -- åªã‡âﬂïsë´ åªã‡écçÇ-∫›Àﬂ≠∞¿åv
                  ,tempHistory12.SalesNOCount                     -- ëçîÑ ì`ï[êî
                  ,tempHistory12.CustomerCDCount                  -- ëçîÑ ãqêî(êl)
                  ,tempHistory12.SalesSUSum                       -- ëçîÑ îÑè„êîó 
                  ,tempHistory12.TotalGakuSum                     -- ëçîÑ îÑè„ã‡äz
                  ,tempHistory13.ForeignTaxableAmount             -- éÊà¯ï  äOê≈ëŒè€äz
                  ,tempHistory13.TaxableAmount                    -- éÊà¯ï  ì‡ê≈ëŒè€äz
                  ,tempHistory13.TaxExemptionAmount               -- éÊà¯ï  îÒâ€ê≈ëŒè€äz
                  ,tempHistory13.TotalWithoutTax                  -- éÊà¯ï  ê≈î≤çáåv
                  ,tempHistory13.Tax                              -- éÊà¯ï  ì‡ê≈
                  ,tempHistory13.OutsideTax                       -- éÊà¯ï  äOê≈
                  ,tempHistory13.ConsumptionTax                   -- éÊà¯ï  è¡îÔê≈åv
                  ,tempHistory13.TaxIncludedTotal                 -- éÊà¯ï  ê≈çûçáåv
                  ,tempHistory14.DenominationName1                -- åàçœï  ã‡éÌãÊï™ñº1
                  ,tempHistory14.Kingaku1                         -- åàçœï  ã‡äz1
                  ,tempHistory14.DenominationName2                -- åàçœï  ã‡éÌãÊï™ñº2
                  ,tempHistory14.Kingaku2                         -- åàçœï  ã‡äz2
                  ,tempHistory14.DenominationName3                -- åàçœï  ã‡éÌãÊï™ñº3
                  ,tempHistory14.Kingaku3                         -- åàçœï  ã‡äz3
                  ,tempHistory14.DenominationName4                -- åàçœï  ã‡éÌãÊï™ñº4
                  ,tempHistory14.Kingaku4                         -- åàçœï  ã‡äz4
                  ,tempHistory14.DenominationName5                -- åàçœï  ã‡éÌãÊï™ñº5
                  ,tempHistory14.Kingaku5                         -- åàçœï  ã‡äz5
                  ,tempHistory14.DenominationName6                -- åàçœï  ã‡éÌãÊï™ñº6
                  ,tempHistory14.Kingaku6                         -- åàçœï  ã‡äz6
                  ,tempHistory14.DenominationName7                -- åàçœï  ã‡éÌãÊï™ñº7
                  ,tempHistory14.Kingaku7                         -- åàçœï  ã‡äz7
                  ,tempHistory14.DenominationName8                -- åàçœï  ã‡éÌãÊï™ñº8
                  ,tempHistory14.Kingaku8                         -- åàçœï  ã‡äz8
                  ,tempHistory14.DenominationName9                -- åàçœï  ã‡éÌãÊï™ñº9
                  ,tempHistory14.Kingaku9                         -- åàçœï  ã‡äz9
                  ,tempHistory14.DenominationName10               -- åàçœï  ã‡éÌãÊï™ñº10
                  ,tempHistory14.Kingaku10                        -- åàçœï  ã‡äz10
                  ,tempHistory14.DenominationName11               -- åàçœï  ã‡éÌãÊï™ñº11
                  ,tempHistory14.Kingaku11                        -- åàçœï  ã‡äz11
                  ,tempHistory14.DenominationName12               -- åàçœï  ã‡éÌãÊï™ñº12
                  ,tempHistory14.Kingaku12                        -- åàçœï  ã‡äz12
                  ,tempHistory14.DenominationName13               -- åàçœï  ã‡éÌãÊï™ñº13
                  ,tempHistory14.Kingaku13                        -- åàçœï  ã‡äz13
                  ,tempHistory14.DenominationName14               -- åàçœï  ã‡éÌãÊï™ñº14
                  ,tempHistory14.Kingaku14                        -- åàçœï  ã‡äz14
                  ,tempHistory14.DenominationName15               -- åàçœï  ã‡éÌãÊï™ñº15
                  ,tempHistory14.Kingaku15                        -- åàçœï  ã‡äz15
                  ,tempHistory14.DenominationName16               -- åàçœï  ã‡éÌãÊï™ñº16
                  ,tempHistory14.Kingaku16                        -- åàçœï  ã‡äz16
                  ,tempHistory14.DenominationName17               -- åàçœï  ã‡éÌãÊï™ñº17
                  ,tempHistory14.Kingaku17                        -- åàçœï  ã‡äz17
                  ,tempHistory14.DenominationName18               -- åàçœï  ã‡éÌãÊï™ñº18
                  ,tempHistory14.Kingaku18                        -- åàçœï  ã‡äz18
                  ,tempHistory14.DenominationName19               -- åàçœï  ã‡éÌãÊï™ñº19
                  ,tempHistory14.Kingaku19                        -- åàçœï  ã‡äz19
                  ,tempHistory14.DenominationName20               -- åàçœï  ã‡éÌãÊï™ñº20
                  ,tempHistory14.Kingaku20                        -- åàçœï  ã‡äz20
                  ,tempHistory15.DepositTransfer                  -- ì¸ã‡éxï•åv ì¸ã‡ êUçû
                  ,tempHistory15.DepositCash                      -- ì¸ã‡éxï•åv ì¸ã‡ åªã‡
                  ,tempHistory15.DepositCheck                     -- ì¸ã‡éxï•åv ì¸ã‡ è¨êÿéË
                  ,tempHistory15.DepositBill                      -- ì¸ã‡éxï•åv ì¸ã‡ éËå`
                  ,tempHistory15.DepositOffset                    -- ì¸ã‡éxï•åv ì¸ã‡ ëäéE
                  ,tempHistory15.DepositAdjustment                -- ì¸ã‡éxï•åv ì¸ã‡ í≤êÆ
                  ,tempHistory15.PaymentTransfer                  -- ì¸ã‡éxï•åv éxï• êUçû
                  ,tempHistory15.PaymentCash                      -- ì¸ã‡éxï•åv éxï• åªã‡
                  ,tempHistory15.PaymentCheck                     -- ì¸ã‡éxï•åv éxï• è¨êÿéË
                  ,tempHistory15.PaymentBill                      -- ì¸ã‡éxï•åv éxï• éËå`
                  ,tempHistory15.PaymentOffset                    -- ì¸ã‡éxï•åv éxï• ëäéE
                  ,tempHistory15.PaymentAdjustment                -- ì¸ã‡éxï•åv éxï• í≤êÆ
                  ,tempHistory16.OtherAmountReturns               -- ëºã‡äz ï‘ïi
                  ,tempHistory16.OtherAmountDiscount              -- ëºã‡äz ílà¯
                  ,tempHistory16.OtherAmountCancel                -- ëºã‡äz éÊè¡
                  ,tempHistory16.OtherAmountDelivery              -- ëºã‡äz îzíB
                  ,tempHistory7.ExchangeCount                     -- óºë÷âÒêî
                  ,tempHistory17.ByTimeZoneTaxIncluded_0000_0100  -- éûä‘ë—ï (ê≈çû) 00:00Å`01:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0100_0200  -- éûä‘ë—ï (ê≈çû) 01:00Å`02:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0200_0300  -- éûä‘ë—ï (ê≈çû) 02:00Å`03:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0300_0400  -- éûä‘ë—ï (ê≈çû) 03:00Å`04:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0400_0500  -- éûä‘ë—ï (ê≈çû) 04:00Å`05:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0500_0600  -- éûä‘ë—ï (ê≈çû) 05:00Å`06:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0600_0700  -- éûä‘ë—ï (ê≈çû) 06:00Å`07:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0700_0800  -- éûä‘ë—ï (ê≈çû) 07:00Å`08:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0800_0900  -- éûä‘ë—ï (ê≈çû) 08:00Å`09:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_0900_1000  -- éûä‘ë—ï (ê≈çû) 09:00Å`10:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1000_1100  -- éûä‘ë—ï (ê≈çû) 10:00Å`11:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1100_1200  -- éûä‘ë—ï (ê≈çû) 11:00Å`12:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1200_1300  -- éûä‘ë—ï (ê≈çû) 12:00Å`13:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1300_1400  -- éûä‘ë—ï (ê≈çû) 13:00Å`14:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1400_1500  -- éûä‘ë—ï (ê≈çû) 14:00Å`15:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1500_1600  -- éûä‘ë—ï (ê≈çû) 15:00Å`16:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1600_1700  -- éûä‘ë—ï (ê≈çû) 16:00Å`17:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1700_1800  -- éûä‘ë—ï (ê≈çû) 17:00Å`18:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1800_1900  -- éûä‘ë—ï (ê≈çû) 18:00Å`19:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_1900_2000  -- éûä‘ë—ï (ê≈çû) 19:00Å`20:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_2000_2100  -- éûä‘ë—ï (ê≈çû) 20:00Å`21:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_2100_2200  -- éûä‘ë—ï (ê≈çû) 21:00Å`22:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_2200_2300  -- éûä‘ë—ï (ê≈çû) 22:00Å`23:00
                  ,tempHistory17.ByTimeZoneTaxIncluded_2300_2400  -- éûä‘ë—ï (ê≈çû) 23:00Å`24:00
                  ,tempHistory17.ByTimeZoneSalesNO_0000_0100      -- éûä‘ë—ï åèêî 00:00Å`01:00
                  ,tempHistory17.ByTimeZoneSalesNO_0100_0200      -- éûä‘ë—ï åèêî 01:00Å`02:00
                  ,tempHistory17.ByTimeZoneSalesNO_0200_0300      -- éûä‘ë—ï åèêî 02:00Å`03:00
                  ,tempHistory17.ByTimeZoneSalesNO_0300_0400      -- éûä‘ë—ï åèêî 03:00Å`04:00
                  ,tempHistory17.ByTimeZoneSalesNO_0400_0500      -- éûä‘ë—ï åèêî 04:00Å`05:00
                  ,tempHistory17.ByTimeZoneSalesNO_0500_0600      -- éûä‘ë—ï åèêî 05:00Å`06:00
                  ,tempHistory17.ByTimeZoneSalesNO_0600_0700      -- éûä‘ë—ï åèêî 06:00Å`07:00
                  ,tempHistory17.ByTimeZoneSalesNO_0700_0800      -- éûä‘ë—ï åèêî 07:00Å`08:00
                  ,tempHistory17.ByTimeZoneSalesNO_0800_0900      -- éûä‘ë—ï åèêî 08:00Å`09:00
                  ,tempHistory17.ByTimeZoneSalesNO_0900_1000      -- éûä‘ë—ï åèêî 09:00Å`10:00
                  ,tempHistory17.ByTimeZoneSalesNO_1000_1100      -- éûä‘ë—ï åèêî 10:00Å`11:00
                  ,tempHistory17.ByTimeZoneSalesNO_1100_1200      -- éûä‘ë—ï åèêî 11:00Å`12:00
                  ,tempHistory17.ByTimeZoneSalesNO_1200_1300      -- éûä‘ë—ï åèêî 12:00Å`13:00
                  ,tempHistory17.ByTimeZoneSalesNO_1300_1400      -- éûä‘ë—ï åèêî 13:00Å`14:00
                  ,tempHistory17.ByTimeZoneSalesNO_1400_1500      -- éûä‘ë—ï åèêî 14:00Å`15:00
                  ,tempHistory17.ByTimeZoneSalesNO_1500_1600      -- éûä‘ë—ï åèêî 15:00Å`16:00
                  ,tempHistory17.ByTimeZoneSalesNO_1600_1700      -- éûä‘ë—ï åèêî 16:00Å`17:00
                  ,tempHistory17.ByTimeZoneSalesNO_1700_1800      -- éûä‘ë—ï åèêî 17:00Å`18:00
                  ,tempHistory17.ByTimeZoneSalesNO_1800_1900      -- éûä‘ë—ï åèêî 18:00Å`19:00
                  ,tempHistory17.ByTimeZoneSalesNO_1900_2000      -- éûä‘ë—ï åèêî 19:00Å`20:00
                  ,tempHistory17.ByTimeZoneSalesNO_2000_2100      -- éûä‘ë—ï åèêî 20:00Å`21:00
                  ,tempHistory17.ByTimeZoneSalesNO_2100_2200      -- éûä‘ë—ï åèêî 21:00Å`22:00
                  ,tempHistory17.ByTimeZoneSalesNO_2200_2300      -- éûä‘ë—ï åèêî 22:00Å`23:00
                  ,tempHistory17.ByTimeZoneSalesNO_2300_2400      -- éûä‘ë—ï åèêî 23:00Å`24:00
                  ,tempHistory12.DiscountGaku                     -- ílà¯äz
              FROM #Temp_D_StoreCalculation1 AS storeCalculation
              LEFT OUTER JOIN #Temp_D_DepositHistory7  AS tempHistory7  ON tempHistory7.RegistDate  = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory9  AS tempHistory9  ON tempHistory9.RegistDate  = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory10 AS tempHistory10 ON tempHistory10.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory11 AS tempHistory11 ON tempHistory11.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory12 AS tempHistory12 ON tempHistory12.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory13 AS tempHistory13 ON tempHistory13.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory14 AS tempHistory14 ON tempHistory14.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory15 AS tempHistory15 ON tempHistory15.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory16 AS tempHistory16 ON tempHistory16.RegistDate = storeCalculation.CalculationDate
              LEFT OUTER JOIN #Temp_D_DepositHistory17 AS tempHistory17 ON tempHistory17.RegistDate = storeCalculation.CalculationDate
           ) D8;

    -- ç≈èI
    SELECT (SELECT Picture FROM M_Image WHERE ID = 2) Logo       -- ÉçÉS
          ,A.CalendarDate                                        -- ì˙ït
          ,A.StoreName                                           -- ìXï‹ñº
          ,A.Address1                                            -- èZèäÇP
          ,A.Address2                                            -- èZèäÇQ
          ,A.TelephoneNO                                         -- ìdòbî‘çÜ
          ,A.IssueDate                                           -- î≠çsì˙éû
          ,A.DepositDate                                         -- î≠çsì˙
          ,ROW_NUMBER() OVER(
               PARTITION BY A.StoreCD 
                   ORDER BY A.IssueDate, A.DepositNO             -- ÉåÉVÅ[ÉgÇ∆èáî‘ÇìØÇ∂Ç…Ç∑ÇÈ
           ) AS DetailOrder                                      -- ñæç◊ï\é¶èá
          ,A.DepositNO
          ,A.JanCD                                               -- JANCD
          ,A.SKUShortName                                        -- è§ïiñº
          ,A.SalesUnitPrice                                      -- íPâø
          ,A.SalesSU                                             -- êîó 
          ,A.kakaku                                              -- âøäi
          ,A.SalesTax                                            -- ê≈äz
          ,A.SalesTaxRate                                        -- ê≈ó¶
          ,A.TotalGaku                                           -- îÃîÑçáåväz
          ,A.SumSalesSU                                          -- è¨åvêîó 
          ,A.Subtotal                                            -- è¨åvã‡äz
          ,A.TargetAmount8                                       -- 8ÅìëŒè€äz
          ,A.ConsumptionTax8                                     -- äOê≈8Åì
          ,A.TargetAmount10                                      -- 10ÅìëŒè€äz
          ,A.ConsumptionTax10                                    -- äOê≈10Åì
          ,A.Total                                               -- çáåv
          --
          ,tempHistory2.PaymentName1                             -- éxï•ï˚ñ@ñº1
          ,tempHistory2.AmountPay1                               -- éxï•ï˚ñ@äz1
          ,tempHistory2.PaymentName2                             -- éxï•ï˚ñ@ñº2
          ,tempHistory2.AmountPay2                               -- éxï•ï˚ñ@äz2
          ,tempHistory2.PaymentName3                             -- éxï•ï˚ñ@ñº3
          ,tempHistory2.AmountPay3                               -- éxï•ï˚ñ@äz3
          ,tempHistory2.PaymentName4                             -- éxï•ï˚ñ@ñº4
          ,tempHistory2.AmountPay4                               -- éxï•ï˚ñ@äz4
          ,tempHistory2.PaymentName5                             -- éxï•ï˚ñ@ñº5
          ,tempHistory2.AmountPay5                               -- éxï•ï˚ñ@äz5
          ,tempHistory2.PaymentName6                             -- éxï•ï˚ñ@ñº6
          ,tempHistory2.AmountPay6                               -- éxï•ï˚ñ@äz6
          ,tempHistory2.PaymentName7                             -- éxï•ï˚ñ@ñº7
          ,tempHistory2.AmountPay7                               -- éxï•ï˚ñ@äz7
          ,tempHistory2.PaymentName8                             -- éxï•ï˚ñ@ñº8
          ,tempHistory2.AmountPay8                               -- éxï•ï˚ñ@äz8
          ,tempHistory2.PaymentName9                             -- éxï•ï˚ñ@ñº9
          ,tempHistory2.AmountPay9                               -- éxï•ï˚ñ@äz9
          ,tempHistory2.PaymentName10                            -- éxï•ï˚ñ@ñº10
          ,tempHistory2.AmountPay10                              -- éxï•ï˚ñ@äz10
          --
          ,tempHistory3.Refund                                   -- íﬁëK
          ,tempHistory3.DiscountGaku                             -- ílà¯äz
          --
          ,A.StaffReceiptPrint                                   -- íSìñCD
          ,A.StoreReceiptPrint                                   -- ìXï‹CD
          ,A.SalesNO                                             -- îÑè„î‘çÜ
          --
          ,tempHistory4.RegistDate ChangePreparationRegistDate   -- ìoò^ì˙
          ,tempHistory4.ChangePreparationDate1                   -- íﬁëKèÄîıì˙1
          ,tempHistory4.ChangePreparationName1                   -- íﬁëKèÄîıñº1
          ,tempHistory4.ChangePreparationAmount1                 -- íﬁëKèÄîıäz1
          ,tempHistory4.ChangePreparationDate2                   -- íﬁëKèÄîıì˙2
          ,tempHistory4.ChangePreparationName2                   -- íﬁëKèÄîıñº2
          ,tempHistory4.ChangePreparationAmount2                 -- íﬁëKèÄîıäz2
          ,tempHistory4.ChangePreparationDate3                   -- íﬁëKèÄîıì˙3
          ,tempHistory4.ChangePreparationName3                   -- íﬁëKèÄîıñº3
          ,tempHistory4.ChangePreparationAmount3                 -- íﬁëKèÄîıäz3
          ,tempHistory4.ChangePreparationDate4                   -- íﬁëKèÄîıì˙4
          ,tempHistory4.ChangePreparationName4                   -- íﬁëKèÄîıñº4
          ,tempHistory4.ChangePreparationAmount4                 -- íﬁëKèÄîıäz4
          ,tempHistory4.ChangePreparationDate5                   -- íﬁëKèÄîıì˙5
          ,tempHistory4.ChangePreparationName5                   -- íﬁëKèÄîıñº5
          ,tempHistory4.ChangePreparationAmount5                 -- íﬁëKèÄîıäz5
          ,tempHistory4.ChangePreparationDate6                   -- íﬁëKèÄîıì˙6
          ,tempHistory4.ChangePreparationName6                   -- íﬁëKèÄîıñº6
          ,tempHistory4.ChangePreparationAmount6                 -- íﬁëKèÄîıäz6
          ,tempHistory4.ChangePreparationDate7                   -- íﬁëKèÄîıì˙7
          ,tempHistory4.ChangePreparationName7                   -- íﬁëKèÄîıñº7
          ,tempHistory4.ChangePreparationAmount7                 -- íﬁëKèÄîıäz7
          ,tempHistory4.ChangePreparationDate8                   -- íﬁëKèÄîıì˙8
          ,tempHistory4.ChangePreparationName8                   -- íﬁëKèÄîıñº8
          ,tempHistory4.ChangePreparationAmount8                 -- íﬁëKèÄîıäz8
          ,tempHistory4.ChangePreparationDate9                   -- íﬁëKèÄîıì˙9
          ,tempHistory4.ChangePreparationName9                   -- íﬁëKèÄîıñº9
          ,tempHistory4.ChangePreparationAmount9                 -- íﬁëKèÄîıäz9
          ,tempHistory4.ChangePreparationDate10                  -- íﬁëKèÄîıì˙10
          ,tempHistory4.ChangePreparationName10                  -- íﬁëKèÄîıñº10
          ,tempHistory4.ChangePreparationAmount10                -- íﬁëKèÄîıäz10
          ,tempHistory4.ChangePreparationRemark                  -- íﬁëKèÄîıîıçl
          --
          ,tempHistory5.RegistDate MiscDepositRegistDate         -- ìoò^ì˙
          ,tempHistory5.MiscDepositDate1                         -- éGì¸ã‡ì˙1
          ,tempHistory5.MiscDepositName1                         -- éGì¸ã‡ñº1
          ,tempHistory5.MiscDepositAmount1                       -- éGì¸ã‡äz1
          --,tempHistory5.MiscDepositDate2                         -- éGì¸ã‡ì˙2
          --,tempHistory5.MiscDepositName2                         -- éGì¸ã‡ñº2
          --,tempHistory5.MiscDepositAmount2                       -- éGì¸ã‡äz2
          --,tempHistory5.MiscDepositDate3                         -- éGì¸ã‡ì˙3
          --,tempHistory5.MiscDepositName3                         -- éGì¸ã‡ñº3
          --,tempHistory5.MiscDepositAmount3                       -- éGì¸ã‡äz3
          --,tempHistory5.MiscDepositDate4                         -- éGì¸ã‡ì˙4
          --,tempHistory5.MiscDepositName4                         -- éGì¸ã‡ñº4
          --,tempHistory5.MiscDepositAmount4                       -- éGì¸ã‡äz4
          --,tempHistory5.MiscDepositDate5                         -- éGì¸ã‡ì˙5
          --,tempHistory5.MiscDepositName5                         -- éGì¸ã‡ñº5
          --,tempHistory5.MiscDepositAmount5                       -- éGì¸ã‡äz5
          --,tempHistory5.MiscDepositDate6                         -- éGì¸ã‡ì˙6
          --,tempHistory5.MiscDepositName6                         -- éGì¸ã‡ñº6
          --,tempHistory5.MiscDepositAmount6                       -- éGì¸ã‡äz6
          --,tempHistory5.MiscDepositDate7                         -- éGì¸ã‡ì˙7
          --,tempHistory5.MiscDepositName7                         -- éGì¸ã‡ñº7
          --,tempHistory5.MiscDepositAmount7                       -- éGì¸ã‡äz7
          --,tempHistory5.MiscDepositDate8                         -- éGì¸ã‡ì˙8
          --,tempHistory5.MiscDepositName8                         -- éGì¸ã‡ñº8
          --,tempHistory5.MiscDepositAmount8                       -- éGì¸ã‡äz8
          --,tempHistory5.MiscDepositDate9                         -- éGì¸ã‡ì˙9
          --,tempHistory5.MiscDepositName9                         -- éGì¸ã‡ñº9
          --,tempHistory5.MiscDepositAmount9                       -- éGì¸ã‡äz9
          --,tempHistory5.MiscDepositDate10                        -- éGì¸ã‡ì˙10
          --,tempHistory5.MiscDepositName10                        -- éGì¸ã‡ñº10
          --,tempHistory5.MiscDepositAmount10                      -- éGì¸ã‡äz10
          ,tempHistory5.MiscDepositRemark                        -- éGì¸ã‡îıçl
          --
          ,tempHistory51.RegistDate DepositRegistDate            -- ìoò^ì˙
          ,tempHistory51.CustomerCD                              -- ì¸ã‡å≥CD
          ,tempHistory51.CustomerName                            -- ì¸ã‡å≥ñº
          ,tempHistory51.DepositDate1                            -- ì¸ã‡ì˙1
          ,tempHistory51.DepositName1                            -- ì¸ã‡ñº1
          ,tempHistory51.DepositAmount1                          -- ì¸ã‡äz1
          --,tempHistory51.DepositDate2                            -- ì¸ã‡ì˙2
          --,tempHistory51.DepositName2                            -- ì¸ã‡ñº2
          --,tempHistory51.DepositAmount2                          -- ì¸ã‡äz2
          --,tempHistory51.DepositDate3                            -- ì¸ã‡ì˙3
          --,tempHistory51.DepositName3                            -- ì¸ã‡ñº3
          --,tempHistory51.DepositAmount3                          -- ì¸ã‡äz3
          --,tempHistory51.DepositDate4                            -- ì¸ã‡ì˙4
          --,tempHistory51.DepositName4                            -- ì¸ã‡ñº4
          --,tempHistory51.DepositAmount4                          -- ì¸ã‡äz4
          --,tempHistory51.DepositDate5                            -- ì¸ã‡ì˙5
          --,tempHistory51.DepositName5                            -- ì¸ã‡ñº5
          --,tempHistory51.DepositAmount5                          -- ì¸ã‡äz5
          --,tempHistory51.DepositDate6                            -- ì¸ã‡ì˙6
          --,tempHistory51.DepositName6                            -- ì¸ã‡ñº6
          --,tempHistory51.DepositAmount6                          -- ì¸ã‡äz6
          --,tempHistory51.DepositDate7                            -- ì¸ã‡ì˙7
          --,tempHistory51.DepositName7                            -- ì¸ã‡ñº7
          --,tempHistory51.DepositAmount7                          -- ì¸ã‡äz7
          --,tempHistory51.DepositDate8                            -- ì¸ã‡ì˙8
          --,tempHistory51.DepositName8                            -- ì¸ã‡ñº8
          --,tempHistory51.DepositAmount8                          -- ì¸ã‡äz8
          --,tempHistory51.DepositDate9                            -- ì¸ã‡ì˙9
          --,tempHistory51.DepositName9                            -- ì¸ã‡ñº9
          --,tempHistory51.DepositAmount9                          -- ì¸ã‡äz9
          --,tempHistory51.DepositDate10                           -- ì¸ã‡ì˙10
          --,tempHistory51.DepositName10                           -- ì¸ã‡ñº10
          --,tempHistory51.DepositAmount10                         -- ì¸ã‡äz10
          ,tempHistory51.DepositRemark                           -- ì¸ã‡îıçl
          --
          ,tempHistory6.RegistDate MiscPaymentRegistDate         -- ìoò^ì˙
          ,tempHistory6.MiscPaymentDate1                         -- éGéxï•ì˙1
          ,tempHistory6.MiscPaymentName1                         -- éGéxï•ñº1
          ,tempHistory6.MiscPaymentAmount1                       -- éGéxï•äz1
          --,tempHistory6.MiscPaymentDate2                         -- éGéxï•ì˙2
          --,tempHistory6.MiscPaymentName2                         -- éGéxï•ñº2
          --,tempHistory6.MiscPaymentAmount2                       -- éGéxï•äz2
          --,tempHistory6.MiscPaymentDate3                         -- éGéxï•ì˙3
          --,tempHistory6.MiscPaymentName3                         -- éGéxï•ñº3
          --,tempHistory6.MiscPaymentAmount3                       -- éGéxï•äz3
          --,tempHistory6.MiscPaymentDate4                         -- éGéxï•ì˙4
          --,tempHistory6.MiscPaymentName4                         -- éGéxï•ñº4
          --,tempHistory6.MiscPaymentAmount4                       -- éGéxï•äz4
          --,tempHistory6.MiscPaymentDate5                         -- éGéxï•ì˙5
          --,tempHistory6.MiscPaymentName5                         -- éGéxï•ñº5
          --,tempHistory6.MiscPaymentAmount5                       -- éGéxï•äz5
          --,tempHistory6.MiscPaymentDate6                         -- éGéxï•ì˙6
          --,tempHistory6.MiscPaymentName6                         -- éGéxï•ñº6
          --,tempHistory6.MiscPaymentAmount6                       -- éGéxï•äz6
          --,tempHistory6.MiscPaymentDate7                         -- éGéxï•ì˙7
          --,tempHistory6.MiscPaymentName7                         -- éGéxï•ñº7
          --,tempHistory6.MiscPaymentAmount7                       -- éGéxï•äz7
          --,tempHistory6.MiscPaymentDate8                         -- éGéxï•ì˙8
          --,tempHistory6.MiscPaymentName8                         -- éGéxï•ñº8
          --,tempHistory6.MiscPaymentAmount8                       -- éGéxï•äz8
          --,tempHistory6.MiscPaymentDate9                         -- éGéxï•ì˙9
          --,tempHistory6.MiscPaymentName9                         -- éGéxï•ñº9
          --,tempHistory6.MiscPaymentAmount9                       -- éGéxï•äz9
          --,tempHistory6.MiscPaymentDate10                        -- éGéxï•ì˙10
          --,tempHistory6.MiscPaymentName10                        -- éGéxï•ñº10
          --,tempHistory6.MiscPaymentAmount10                      -- éGéxï•äz10
          ,tempHistory6.MiscPaymentRemark                        -- éGéxï•îıçl
          --
          ,tempHistory7.RegistDate ExchangeRegistDate            -- ìoò^ì˙
          ,tempHistory7.ExchangeDate1                            -- óºë÷ì˙1
          ,tempHistory7.ExchangeName1                            -- óºë÷ñº1
          ,tempHistory7.ExchangeAmount1                          -- óºë÷äz1
          ,tempHistory7.ExchangeDenomination1                    -- óºë÷éÜïº1
          ,tempHistory7.ExchangeCount1                           -- óºë÷ñáêî1
          --,tempHistory7.ExchangeDate2                            -- óºë÷ì˙2
          --,tempHistory7.ExchangeName2                            -- óºë÷ñº2
          --,tempHistory7.ExchangeAmount2                          -- óºë÷äz2
          --,tempHistory7.ExchangeDenomination2                    -- óºë÷éÜïº2
          --,tempHistory7.ExchangeCount2                           -- óºë÷ñáêî2
          --,tempHistory7.ExchangeDate3                            -- óºë÷ì˙3
          --,tempHistory7.ExchangeName3                            -- óºë÷ñº3
          --,tempHistory7.ExchangeAmount3                          -- óºë÷äz3
          --,tempHistory7.ExchangeDenomination3                    -- óºë÷éÜïº3
          --,tempHistory7.ExchangeCount3                           -- óºë÷ñáêî3
          --,tempHistory7.ExchangeDate4                            -- óºë÷ì˙4
          --,tempHistory7.ExchangeName4                            -- óºë÷ñº4
          --,tempHistory7.ExchangeAmount4                          -- óºë÷äz4
          --,tempHistory7.ExchangeDenomination4                    -- óºë÷éÜïº4
          --,tempHistory7.ExchangeCount4                           -- óºë÷ñáêî4
          --,tempHistory7.ExchangeDate5                            -- óºë÷ì˙5
          --,tempHistory7.ExchangeName5                            -- óºë÷ñº5
          --,tempHistory7.ExchangeAmount5                          -- óºë÷äz5
          --,tempHistory7.ExchangeDenomination5                    -- óºë÷éÜïº5
          --,tempHistory7.ExchangeCount5                           -- óºë÷ñáêî5
          --,tempHistory7.ExchangeDate6                            -- óºë÷ì˙6
          --,tempHistory7.ExchangeName6                            -- óºë÷ñº6
          --,tempHistory7.ExchangeAmount6                          -- óºë÷äz6
          --,tempHistory7.ExchangeDenomination6                    -- óºë÷éÜïº6
          --,tempHistory7.ExchangeCount6                           -- óºë÷ñáêî6
          --,tempHistory7.ExchangeDate7                            -- óºë÷ì˙7
          --,tempHistory7.ExchangeName7                            -- óºë÷ñº7
          --,tempHistory7.ExchangeAmount7                          -- óºë÷äz7
          --,tempHistory7.ExchangeDenomination7                    -- óºë÷éÜïº7
          --,tempHistory7.ExchangeCount7                           -- óºë÷ñáêî7
          --,tempHistory7.ExchangeDate8                            -- óºë÷ì˙8
          --,tempHistory7.ExchangeName8                            -- óºë÷ñº8
          --,tempHistory7.ExchangeAmount8                          -- óºë÷äz8
          --,tempHistory7.ExchangeDenomination8                    -- óºë÷éÜïº8
          --,tempHistory7.ExchangeCount8                           -- óºë÷ñáêî8
          --,tempHistory7.ExchangeDate9                            -- óºë÷ì˙9
          --,tempHistory7.ExchangeName9                            -- óºë÷ñº9
          --,tempHistory7.ExchangeAmount9                          -- óºë÷äz9
          --,tempHistory7.ExchangeDenomination9                    -- óºë÷éÜïº9
          --,tempHistory7.ExchangeCount9                           -- óºë÷ñáêî9
          --,tempHistory7.ExchangeDate10                           -- óºë÷ì˙10
          --,tempHistory7.ExchangeName10                           -- óºë÷ñº10
          --,tempHistory7.ExchangeAmount10                         -- óºë÷äz10
          --,tempHistory7.ExchangeDenomination10                   -- óºë÷éÜïº10
          --,tempHistory7.ExchangeCount10                          -- óºë÷ñáêî10
          ,tempHistory7.ExchangeRemark                           -- óºë÷îıçl
          --
          ,tempHistory8.RegistDate CashBalanceRegistDate         -- ìoò^ì˙
          ,tempHistory8.[10000yenNum]                            --Åyê∏éZèàóùÅzåªã‡écçÇÅ@10,000Å@ñáêî
          ,tempHistory8.[5000yenNum]                             --Åyê∏éZèàóùÅzåªã‡écçÇÅ@5,000Å@ñáêî
          ,tempHistory8.[2000yenNum]                             --Åyê∏éZèàóùÅzåªã‡écçÇÅ@2,000Å@ñáêî
          ,tempHistory8.[1000yenNum]                             --Åyê∏éZèàóùÅzåªã‡écçÇÅ@1,000Å@ñáêî
          ,tempHistory8.[500yenNum]                              --Åyê∏éZèàóùÅzåªã‡écçÇÅ@500Å@ñáêî
          ,tempHistory8.[100yenNum]                              --Åyê∏éZèàóùÅzåªã‡écçÇÅ@100Å@ñáêî
          ,tempHistory8.[50yenNum]                               --Åyê∏éZèàóùÅzåªã‡écçÇÅ@50Å@ñáêî
          ,tempHistory8.[10yenNum]                               --Åyê∏éZèàóùÅzåªã‡écçÇÅ@10Å@ñáêî
          ,tempHistory8.[5yenNum]                                --Åyê∏éZèàóùÅzåªã‡écçÇÅ@5Å@ñáêî
          ,tempHistory8.[1yenNum]                                --Åyê∏éZèàóùÅzåªã‡écçÇÅ@1Å@ñáêî
          ,tempHistory8.[10000yenGaku]                           --Åyê∏éZèàóùÅzåªã‡écçÇÅ@10,000Å@ã‡äz
          ,tempHistory8.[5000yenGaku]                            --Åyê∏éZèàóùÅzåªã‡écçÇÅ@5,000Å@ã‡äz
          ,tempHistory8.[2000yenGaku]                            --Åyê∏éZèàóùÅzåªã‡écçÇÅ@2,000Å@ã‡äz
          ,tempHistory8.[1000yenGaku]                            --Åyê∏éZèàóùÅzåªã‡écçÇÅ@1,000Å@ã‡äz
          ,tempHistory8.[500yenGaku]                             --Åyê∏éZèàóùÅzåªã‡écçÇÅ@500Å@ã‡äz
          ,tempHistory8.[100yenGaku]                             --Åyê∏éZèàóùÅzåªã‡écçÇÅ@100Å@ã‡äz
          ,tempHistory8.[50yenGaku]                              --Åyê∏éZèàóùÅzåªã‡écçÇÅ@50Å@ã‡äz
          ,tempHistory8.[10yenGaku]                              --Åyê∏éZèàóùÅzåªã‡écçÇÅ@10Å@ã‡äz
          ,tempHistory8.[5yenGaku]                               --Åyê∏éZèàóùÅzåªã‡écçÇÅ@5Å@ã‡äz
          ,tempHistory8.[1yenGaku]                               --Åyê∏éZèàóùÅzåªã‡écçÇÅ@1Å@ã‡äz
          ,tempHistory8.Etcyen                                   --Åyê∏éZèàóùÅzÇªÇÃëºã‡äz
          ,tempHistory8.Change                                   --Åyê∏éZèàóùÅzíﬁëKèÄîıã‡
          ,tempHistory8.DepositGaku                              --Åyê∏éZèàóùÅzåªã‡écçÇ åªã‡îÑè„(+)
          ,tempHistory8.CashDeposit                              --Åyê∏éZèàóùÅzåªã‡écçÇ åªã‡ì¸ã‡(+)
          ,tempHistory8.CashPayment                              --Åyê∏éZèàóùÅzåªã‡écçÇ åªã‡éxï•(-)
          ,tempHistory8.CashBalance                              --Åyê∏éZèàóùÅzåªã‡écçÇ ÇªÇÃëºã‡äzÅ`åªã‡écçÇåªã‡éxï•(-)Ç‹Ç≈ÇÃçáåv
          ,tempHistory8.ComputerTotal                            --Åyê∏éZèàóùÅz∫›Àﬂ≠∞¿åv åªã‡écçÇ 10,000Å@ã‡äzÅ`åªã‡écçÇÅ@1Å@ã‡äzÇ‹Ç≈ÇÃçáåv
          ,tempHistory8.CashShortage                             --Åyê∏éZèàóùÅzåªã‡écçÇ åªã‡âﬂïsë´
          ,tempHistory8.SalesNOCount                             --Åyê∏éZèàóùÅzëçîÑÅ@ì`ï[êî
          ,tempHistory8.CustomerCDCount                          --Åyê∏éZèàóùÅzëçîÑÅ@ãqêî(êl)
          ,tempHistory8.SalesSUSum                               --Åyê∏éZèàóùÅzëçîÑÅ@îÑè„êîó 
          ,tempHistory8.TotalGakuSum                             --Åyê∏éZèàóùÅzëçîÑÅ@îÑè„ã‡äz
          ,tempHistory8.ForeignTaxableAmount                     --Åyê∏éZèàóùÅzéÊà¯ï Å@äOê≈ëŒè€äz
          ,tempHistory8.TaxableAmount                            --Åyê∏éZèàóùÅzéÊà¯ï Å@ì‡ê≈ëŒè€äz
          ,tempHistory8.TaxExemptionAmount                       --Åyê∏éZèàóùÅzéÊà¯ï Å@îÒâ€ê≈ëŒè€äz
          ,tempHistory8.TotalWithoutTax                          --Åyê∏éZèàóùÅzéÊà¯ï Å@ê≈î≤çáåv
          ,tempHistory8.Tax                                      --Åyê∏éZèàóùÅzéÊà¯ï Å@ì‡ê≈
          ,tempHistory8.OutsideTax                               --Åyê∏éZèàóùÅzéÊà¯ï Å@äOê≈
          ,tempHistory8.ConsumptionTax                           --Åyê∏éZèàóùÅzéÊà¯ï Å@è¡îÔê≈åv
          ,tempHistory8.TaxIncludedTotal                         --Åyê∏éZèàóùÅzéÊà¯ï Å@ê≈çûçáåv
          ,tempHistory8.DiscountGaku                             --Åyê∏éZèàóùÅzéÊà¯ï Å@ílà¯äz
          ,tempHistory8.DenominationName1                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº1
          ,tempHistory8.Kingaku1                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz1
          ,tempHistory8.DenominationName2                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº2
          ,tempHistory8.Kingaku2                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz2
          ,tempHistory8.DenominationName3                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº3
          ,tempHistory8.Kingaku3                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz3
          ,tempHistory8.DenominationName4                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº4
          ,tempHistory8.Kingaku4                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz4
          ,tempHistory8.DenominationName5                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº5
          ,tempHistory8.Kingaku5                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz5
          ,tempHistory8.DenominationName6                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº6
          ,tempHistory8.Kingaku6                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz6
          ,tempHistory8.DenominationName7                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº7
          ,tempHistory8.Kingaku7                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz7
          ,tempHistory8.DenominationName8                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº8
          ,tempHistory8.Kingaku8                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz8
          ,tempHistory8.DenominationName9                        --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº9
          ,tempHistory8.Kingaku9                                 --Åyê∏éZèàóùÅzåàçœï   ã‡äz9
          ,tempHistory8.DenominationName10                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº10
          ,tempHistory8.Kingaku10                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz10
          ,tempHistory8.DenominationName11                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº11
          ,tempHistory8.Kingaku11                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz11
          ,tempHistory8.DenominationName12                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº12
          ,tempHistory8.Kingaku12                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz12
          ,tempHistory8.DenominationName13                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº13
          ,tempHistory8.Kingaku13                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz13
          ,tempHistory8.DenominationName14                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº14
          ,tempHistory8.Kingaku14                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz14
          ,tempHistory8.DenominationName15                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº15
          ,tempHistory8.Kingaku15                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz15
          ,tempHistory8.DenominationName16                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº16
          ,tempHistory8.Kingaku16                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz16
          ,tempHistory8.DenominationName17                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº17
          ,tempHistory8.Kingaku17                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz17
          ,tempHistory8.DenominationName18                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº18
          ,tempHistory8.Kingaku18                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz18
          ,tempHistory8.DenominationName19                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº19
          ,tempHistory8.Kingaku19                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz19
          ,tempHistory8.DenominationName20                       --Åyê∏éZèàóùÅzåàçœï   ã‡éÌãÊï™ñº20
          ,tempHistory8.Kingaku20                                --Åyê∏éZèàóùÅzåàçœï   ã‡äz20
          ,tempHistory8.DepositTransfer                          --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ êUçû
          ,tempHistory8.DepositCash                              --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ åªã‡
          ,tempHistory8.DepositCheck                             --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ è¨êÿéË
          ,tempHistory8.DepositBill                              --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ éËå`
          ,tempHistory8.DepositOffset                            --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ ëäéE
          ,tempHistory8.DepositAdjustment                        --Åyê∏éZèàóùÅzì¸ã‡éxï•åv ì¸ã‡ í≤êÆ
          ,tempHistory8.PaymentTransfer                          --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• êUçû
          ,tempHistory8.PaymentCash                              --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• åªã‡
          ,tempHistory8.PaymentCheck                             --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• è¨êÿéË
          ,tempHistory8.PaymentBill                              --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• éËå`
          ,tempHistory8.PaymentOffset                            --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• ëäéE
          ,tempHistory8.PaymentAdjustment                        --Åyê∏éZèàóùÅzì¸ã‡éxï•åv éxï• í≤êÆ
          ,tempHistory8.OtherAmountReturns                       --Åyê∏éZèàóùÅzëºã‡äz ï‘ïi
          ,tempHistory8.OtherAmountDiscount                      --Åyê∏éZèàóùÅzëºã‡äz ílà¯
          ,tempHistory8.OtherAmountCancel                        --Åyê∏éZèàóùÅzëºã‡äz éÊè¡
          ,tempHistory8.OtherAmountDelivery                      --Åyê∏éZèàóùÅzëºã‡äz îzíB
          ,tempHistory8.ExchangeCount                            --Åyê∏éZèàóùÅzóºë÷âÒêî
          ,tempHistory8.ByTimeZoneTaxIncluded_0000_0100          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 00:00Å`01:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0100_0200          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 01:00Å`02:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0200_0300          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 02:00Å`03:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0300_0400          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 03:00Å`04:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0400_0500          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 04:00Å`05:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0500_0600          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 05:00Å`06:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0600_0700          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 06:00Å`07:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0700_0800          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 07:00Å`08:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0800_0900          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 08:00Å`09:00
          ,tempHistory8.ByTimeZoneTaxIncluded_0900_1000          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 09:00Å`10:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1000_1100          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 10:00Å`11:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1100_1200          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 11:00Å`12:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1200_1300          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 12:00Å`13:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1300_1400          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 13:00Å`14:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1400_1500          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 14:00Å`15:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1500_1600          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 15:00Å`16:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1600_1700          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 16:00Å`17:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1700_1800          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 17:00Å`18:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1800_1900          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 18:00Å`19:00
          ,tempHistory8.ByTimeZoneTaxIncluded_1900_2000          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 19:00Å`20:00
          ,tempHistory8.ByTimeZoneTaxIncluded_2000_2100          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 20:00Å`21:00
          ,tempHistory8.ByTimeZoneTaxIncluded_2100_2200          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 21:00Å`22:00
          ,tempHistory8.ByTimeZoneTaxIncluded_2200_2300          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 22:00Å`23:00
          ,tempHistory8.ByTimeZoneTaxIncluded_2300_2400          --Åyê∏éZèàóùÅzéûä‘ë—ï (ê≈çû) 23:00Å`24:00
          ,tempHistory8.ByTimeZoneSalesNO_0000_0100              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 00:00Å`01:00
          ,tempHistory8.ByTimeZoneSalesNO_0100_0200              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 01:00Å`02:00
          ,tempHistory8.ByTimeZoneSalesNO_0200_0300              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 02:00Å`03:00
          ,tempHistory8.ByTimeZoneSalesNO_0300_0400              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 03:00Å`04:00
          ,tempHistory8.ByTimeZoneSalesNO_0400_0500              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 04:00Å`05:00
          ,tempHistory8.ByTimeZoneSalesNO_0500_0600              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 05:00Å`06:00
          ,tempHistory8.ByTimeZoneSalesNO_0600_0700              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 06:00Å`07:00
          ,tempHistory8.ByTimeZoneSalesNO_0700_0800              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 07:00Å`08:00
          ,tempHistory8.ByTimeZoneSalesNO_0800_0900              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 08:00Å`09:00
          ,tempHistory8.ByTimeZoneSalesNO_0900_1000              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 09:00Å`10:00
          ,tempHistory8.ByTimeZoneSalesNO_1000_1100              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 10:00Å`11:00
          ,tempHistory8.ByTimeZoneSalesNO_1100_1200              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 11:00Å`12:00
          ,tempHistory8.ByTimeZoneSalesNO_1200_1300              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 12:00Å`13:00
          ,tempHistory8.ByTimeZoneSalesNO_1300_1400              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 13:00Å`14:00
          ,tempHistory8.ByTimeZoneSalesNO_1400_1500              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 14:00Å`15:00
          ,tempHistory8.ByTimeZoneSalesNO_1500_1600              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 15:00Å`16:00
          ,tempHistory8.ByTimeZoneSalesNO_1600_1700              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 16:00Å`17:00
          ,tempHistory8.ByTimeZoneSalesNO_1700_1800              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 17:00Å`18:00
          ,tempHistory8.ByTimeZoneSalesNO_1800_1900              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 18:00Å`19:00
          ,tempHistory8.ByTimeZoneSalesNO_1900_2000              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 19:00Å`20:00
          ,tempHistory8.ByTimeZoneSalesNO_2000_2100              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 20:00Å`21:00
          ,tempHistory8.ByTimeZoneSalesNO_2100_2200              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 21:00Å`22:00
          ,tempHistory8.ByTimeZoneSalesNO_2200_2300              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 22:00Å`23:00
          ,tempHistory8.ByTimeZoneSalesNO_2300_2400              --Åyê∏éZèàóùÅzéûä‘ë—ï åèêî 23:00Å`24:00
      FROM (
            SELECT calendar.CalendarDate                                           -- ì˙ït
                  ,@StoreCD AS StoreCD
                  ,store.StoreName                                                 -- ìXï‹ñº
                  ,store.Address1                                                  -- èZèäÇP
                  ,store.Address2                                                  -- èZèäÇQ
                  ,store.TelephoneNO                                               -- ìdòbî‘çÜ
                  ,CASE 
                       WHEN tempHistory1.RegistDate IS NOT NULL THEN tempHistory1.RegistDate
                       --ELSE CONCAT(calendar.CalendarDate, ' 00:00:00.000')
                       ELSE tempHistory0.DepositDateTime
                   END IssueDate                                                   -- î≠çsì˙éû
                  ,CASE 
                       WHEN tempHistory1.DepositDate IS NOT NULL THEN tempHistory1.DepositDate
                       ELSE calendar.CalendarDate
                   END DepositDate                                                 -- î≠çsì˙
                  ,tempHistory1.SalesNO
                  ,tempHistory1.JanCD                                              -- JANCD
                  ,tempHistory1.SKUShortName                                       -- è§ïiñº
                  ,tempHistory1.SalesUnitPrice                                     -- íPâø
                  ,tempHistory1.SalesSU                                            -- êîó 
                  ,tempHistory1.kakaku                                             -- âøäi
                  ,tempHistory1.SalesTax                                           -- ê≈äz
                  ,tempHistory1.SalesTaxRate                                       -- ê≈ó¶
                  ,tempHistory1.TotalGaku                                          -- îÃîÑçáåväz
                  --
                  ,(SELECT SUM(CASE 
                                   WHEN SalesSU IS NULL THEN 1 
                                   ELSE SalesSU 
                               END)
                      FROM #Temp_D_DepositHistory1 t
                     WHERE t.SalesNO= tempHistory1.SalesNO) SumSalesSU             -- è¨åvêîó 
                  ,(SELECT SUM(kakaku) 
                      FROM #Temp_D_DepositHistory1 t 
                     WHERE t.SalesNO = tempHistory1.SalesNO) Subtotal              -- è¨åvã‡äz
                  ,tempHistory1.TargetAmount8                                      -- 8ÅìëŒè€äz
                  ,tempHistory1.SalesTax8 ConsumptionTax8                          -- äOê≈8Åì
                  ,tempHistory1.TargetAmount10                                     -- 10ÅìëŒè€äz
                  ,tempHistory1.SalesTax10 ConsumptionTax10                        -- äOê≈10Åì
                  ,(SELECT SUM(TotalGaku) 
                      FROM #Temp_D_DepositHistory1 t 
                     WHERE t.SalesNO = tempHistory1.SalesNO) Total                 -- çáåv
                  --
                  ,ISNULL(tempHistory1.StaffReceiptPrint
                         ,(SELECT top 1 staff.ReceiptPrint
                             FROM F_Staff(CONVERT(DATE, GETDATE())) AS staff
                            WHERE staff.StaffCD = tempHistory0.InsertOperator
                              AND staff.DeleteFlg = 0
                            ORDER BY staff.ChangeDate DESC
                            )) AS StaffReceiptPrint                                -- íSìñCD
                  ,store.ReceiptPrint StoreReceiptPrint                            -- ìXï‹CD
                  ,tempHistory0.DepositNO
              FROM M_Calendar AS calendar
             INNER JOIN F_Store(CONVERT(DATE, GETDATE())) AS store
                ON store.StoreCD = @StoreCD
               AND store.DeleteFlg = 0
             INNER JOIN #Temp_D_DepositHistory0 AS tempHistory0 ON tempHistory0.StoreCD = store.StoreCD
                                                               AND tempHistory0.AccountingDate = calendar.CalendarDate
              LEFT OUTER JOIN #Temp_D_DepositHistory1 AS tempHistory1 ON tempHistory1.DepositNO = tempHistory0.DepositNO
             WHERE calendar.CalendarDate >= convert(date, @DateFrom)
               AND calendar.CalendarDate <= convert(date, @DateTo)
            --   AND ((tempHistory0.DataKBN = 2 AND tempHistory0.DepositKBN = 1 AND tempHistory0.CancelKBN = 0 AND EXISTS(SELECT 1 FROM #Temp_D_DepositHistory1 AS T1 WHERE T1.DepositNO = tempHistory0.DepositNO))
            --      OR tempHistory0.DataKBN <> 2
            --      OR tempHistory0.DepositKBN <> 1
            --      OR tempHistory0.CancelKBN <> 0
            --      )
           ) A
      LEFT OUTER JOIN #Temp_D_DepositHistory2 tempHistory2   ON tempHistory2.SalesNO = A.SalesNO
      LEFT OUTER JOIN #Temp_D_DepositHistory3 tempHistory3   ON tempHistory3.SalesNO = A.SalesNO
      
      --LEFT OUTER JOIN #Temp_D_DepositHistory4 tempHistory4   ON tempHistory4.RegistDate = A.CalendarDate
      --LEFT OUTER JOIN #Temp_D_DepositHistory5 tempHistory5   ON tempHistory5.RegistDate = A.CalendarDate
      --LEFT OUTER JOIN #Temp_D_DepositHistory51 tempHistory51 ON tempHistory51.RegistDate = A.CalendarDate
      --LEFT OUTER JOIN #Temp_D_DepositHistory6 tempHistory6   ON tempHistory6.RegistDate = A.CalendarDate
      --LEFT OUTER JOIN #Temp_D_DepositHistory7 tempHistory7   ON tempHistory7.RegistDate = A.CalendarDate
      
      LEFT OUTER JOIN #Temp_D_DepositHistory4 tempHistory4   ON tempHistory4.DepositNO = A.DepositNO
      LEFT OUTER JOIN #Temp_D_DepositHistory5 tempHistory5   ON tempHistory5.DepositNO = A.DepositNO
      LEFT OUTER JOIN #Temp_D_DepositHistory51 tempHistory51 ON tempHistory51.DepositNO = A.DepositNO
      LEFT OUTER JOIN #Temp_D_DepositHistory6 tempHistory6   ON tempHistory6.DepositNO = A.DepositNO
      LEFT OUTER JOIN #Temp_D_DepositHistory7 tempHistory7   ON tempHistory7.DepositNO = A.DepositNO
      
      LEFT OUTER JOIN #Temp_D_DepositHistory8 tempHistory8   ON tempHistory8.RegistDate = A.CalendarDate
      
     ORDER BY DetailOrder ASC
         ;
    
    -- ÉèÅ[ÉNÉeÅ[ÉuÉãÇçÌèú
        DROP TABLE #Temp_D_StoreCalculation1;
        DROP TABLE #Temp_D_DepositHistory0;
        DROP TABLE #Temp_D_DepositHistory1;
        DROP TABLE #Temp_D_DepositHistory2;
        DROP TABLE #Temp_D_DepositHistory3;
        DROP TABLE #Temp_D_DepositHistory4;
        DROP TABLE #Temp_D_DepositHistory5;
        DROP TABLE #Temp_D_DepositHistory51;
        DROP TABLE #Temp_D_DepositHistory6;
        DROP TABLE #Temp_D_DepositHistory7;
        DROP TABLE #Temp_D_DepositHistory8;
        DROP TABLE #Temp_D_DepositHistory9;
        DROP TABLE #Temp_D_DepositHistory10;
        DROP TABLE #Temp_D_DepositHistory11;
        DROP TABLE #Temp_D_DepositHistory12;
        DROP TABLE #Temp_D_DepositHistory13;
        DROP TABLE #Temp_D_DepositHistory14;
        DROP TABLE #Temp_D_DepositHistory15;
        DROP TABLE #Temp_D_DepositHistory16;
        DROP TABLE #Temp_D_DepositHistory17;

END
