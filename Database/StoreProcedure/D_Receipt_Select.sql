SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 領収書印刷 　レシート印刷出力
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2019.11.19
--       Update date:    2020.05.23  TelphoneNO → TelephoneNO
--                       2020.07.19  仕様書変更、不具合修正
--  ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_Receipt_Select')
  DROP PROCEDURE [dbo].[D_Receipt_Select]
GO

Create PROCEDURE [dbo].[D_Receipt_Select]
(
@StoreCD varchar(10),
    @SalesNO  varchar(11),
    @IsIssued tinyint
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    -- ワークテーブル１作成
    SELECT * 
      INTO #Temp_D_DepositHistory1
      FROM (
            SELECT DISTINCT
                   history.Number SalesNO
                  ,history.DepositNO
                  ,history.JanCD
                  ,(SELECT top 1 sku.SKUShortName
                    FROM M_SKU AS sku
                    WHERE sku.DeleteFlg = 0
                    AND sku.AdminNO = history.AdminNO
                    AND sku.ChangeDate <= history.AccountingDate
                    ORDER BY sku.ChangeDate desc) AS SKUShortName
                  ,history.DepositDateTime
                  ,CASE
                     WHEN history.SalesSu = 1 THEN NULL
                     ELSE history.SalesUnitPrice
                   END AS SalesUnitPrice
                  ,CASE
                     WHEN history.SalesSu = 1 THEN NULL
                     ELSE history.SalesSu
                   END AS SalesSu
                  ,history.TotalGaku Kakaku
                  ,history.SalesTax
                  ,history.SalesTaxRate
                  ,history.TotalGaku
                  ,sales.SalesHontaiGaku8 + sales.SalesTax8 TargetAmount8
                  ,sales.SalesHontaiGaku10 + sales.SalesTax10 TargetAmount10
                  ,sales.SalesTax8
                  ,sales.SalesTax10
                  ,(SELECT top 1 staff.ReceiptPrint
                    FROM M_Staff AS staff
                    WHERE staff.DeleteFlg = 0
                    AND staff.StaffCD = sales.StaffCD
                    AND staff.ChangeDate <= sales.SalesDate
                    ORDER BY staff.ChangeDate desc)AS StaffReceiptPrint
                  ,(SELECT top 1 store.ReceiptPrint
                    FROM M_Store AS store
                    WHERE store.DeleteFlg = 0
                    AND store.StoreCD = history.StoreCD
                    AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
                    ORDER BY store.ChangeDate desc)AS StoreReceiptPrint
                  ,(SELECT top 1 store.StoreName
                    FROM M_Store AS store
                    WHERE store.DeleteFlg = 0
                    AND store.StoreCD = history.StoreCD
                    AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
                    ORDER BY store.ChangeDate desc)AS StoreName
                  ,(SELECT top 1 store.Address1
                  FROM M_Store AS store
                    WHERE store.DeleteFlg = 0
                    AND store.StoreCD = history.StoreCD
                    AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
                    ORDER BY store.ChangeDate desc)AS Address1
                  ,(SELECT top 1 store.Address2
                  FROM M_Store AS store
                    WHERE store.DeleteFlg = 0
                    AND store.StoreCD = history.StoreCD
                    AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
                    ORDER BY store.ChangeDate desc)AS Address2
                  ,(SELECT top 1 store.TelephoneNO
                    FROM M_Store AS store
                    WHERE store.DeleteFlg = 0
                    AND store.StoreCD = history.StoreCD
                    AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
                    ORDER BY store.ChangeDate desc)AS TelephoneNO
              FROM D_DepositHistory history
              LEFT OUTER JOIN (
                               SELECT SalesNO
                                     ,StaffCD
                                     ,SalesDate
                                     ,SalesHontaiGaku8
                                     ,SalesTax8
                                     ,SalesHontaiGaku10
                                     ,SalesTax10
                                 FROM D_Sales
                                WHERE DeleteDateTime IS NULL
                              ) sales ON sales.SalesNO = history.Number

             WHERE history.Number     = @SalesNO 
               AND history.DataKBN    = 2
               AND history.DepositKBN = 1
               AND history.IsIssued   = @IsIssued
               AND history.CancelKBN  = 0
           ) D1;

    -- ワークテーブル２作成
    SELECT *
      INTO #Temp_D_DepositHistory2
      FROM (
            SELECT D.SalesNO
                  ,MAX(CASE D.RANK WHEN  1 THEN D.DenominationName ELSE NULL END) AS PaymentName1
                  ,MAX(CASE D.RANK WHEN  1 THEN D.DepositGaku      ELSE NULL END) AS AmountPay1
                  ,MAX(CASE D.RANK WHEN  2 THEN D.DenominationName ELSE NULL END) AS PaymentName2
                  ,MAX(CASE D.RANK WHEN  2 THEN D.DepositGaku      ELSE NULL END) AS AmountPay2
                  ,MAX(CASE D.RANK WHEN  3 THEN D.DenominationName ELSE NULL END) AS PaymentName3
                  ,MAX(CASE D.RANK WHEN  3 THEN D.DepositGaku      ELSE NULL END) AS AmountPay3
                  ,MAX(CASE D.RANK WHEN  4 THEN D.DenominationName ELSE NULL END) AS PaymentName4
                  ,MAX(CASE D.RANK WHEN  4 THEN D.DepositGaku      ELSE NULL END) AS AmountPay4
                  ,MAX(CASE D.RANK WHEN  5 THEN D.DenominationName ELSE NULL END) AS PaymentName5
                  ,MAX(CASE D.RANK WHEN  5 THEN D.DepositGaku      ELSE NULL END) AS AmountPay5
                  ,MAX(CASE D.RANK WHEN  6 THEN D.DenominationName ELSE NULL END) AS PaymentName6
                  ,MAX(CASE D.RANK WHEN  6 THEN D.DepositGaku      ELSE NULL END) AS AmountPay6
                  ,MAX(CASE D.RANK WHEN  7 THEN D.DenominationName ELSE NULL END) AS PaymentName7
                  ,MAX(CASE D.RANK WHEN  7 THEN D.DepositGaku      ELSE NULL END) AS AmountPay7
                  ,MAX(CASE D.RANK WHEN  8 THEN D.DenominationName ELSE NULL END) AS PaymentName8
                  ,MAX(CASE D.RANK WHEN  8 THEN D.DepositGaku      ELSE NULL END) AS AmountPay8
                  ,MAX(CASE D.RANK WHEN  9 THEN D.DenominationName ELSE NULL END) AS PaymentName9
                  ,MAX(CASE D.RANK WHEN  9 THEN D.DepositGaku      ELSE NULL END) AS AmountPay9
                  ,MAX(CASE D.RANK WHEN 10 THEN D.DenominationName ELSE NULL END) AS PaymentName10
                  ,MAX(CASE D.RANK WHEN 10 THEN D.DepositGaku      ELSE NULL END) AS AmountPay10
              FROM (
                    SELECT @SalesNO SalesNO
                          ,history.DenominationCD
                          ,denomination.DenominationName
                          ,history.DepositGaku + history.Refund DepositGaku
                          ,history.DepositDateTime
                          ,ROW_NUMBER() OVER(PARTITION BY history.Number ORDER BY history.DepositDateTime ASC) as RANK
                      FROM D_DepositHistory history
                      LEFT OUTER JOIN M_DenominationKBN denomination ON denomination.DenominationCD = history.DenominationCD
                     WHERE history.Number     =@SalesNO 
                       AND history.DataKBN    = 3
                       AND history.DepositKBN = 1
                       AND history.IsIssued   = @IsIssued
                       AND history.CancelKBN  = 0
                   ) D
             GROUP BY D.SalesNO
           ) D2;

    -- ワークテーブル３作成
    SELECT * 
      INTO #Temp_D_DepositHistory3
      FROM (
            SELECT @SalesNO SalesNO
                  ,SUM(history.Refund) Refund
                  ,MAX(sales.LastPoint) SalesLastPoint
                  ,(SELECT top 1 customer.LastPoint
                    FROM M_Customer AS customer
                    WHERE customer.CustomerCD = MAX(sales.CustomerCD)
                    AND customer.ChangeDate <= MAX(sales.SalesDate)
                    AND customer.DeleteFlg   = 0
                    --AND customer.CustomerKBN = 1 
                    ORDER BY ChangeDate DESC
                   ) AS CustomerLastPoint
                  ,(SELECT top 1 customer.CustomerKBN
                    FROM M_Customer AS customer
                    WHERE customer.CustomerCD = MAX(sales.CustomerCD)
                    AND customer.ChangeDate <= MAX(sales.SalesDate)
                    AND customer.DeleteFlg   = 0
                    ORDER BY ChangeDate DESC
                   ) AS CustomerKBN
              FROM D_DepositHistory history
              LEFT OUTER JOIN (
                               SELECT SalesNO
                                     ,LastPoint
                                     ,CustomerCD
                                     ,SalesDate
                                     ,DeleteDateTime
                                 FROM D_Sales
                              ) sales ON sales.SalesNO = history.Number
             WHERE history.Number     = @SalesNO
               AND history.DataKBN    = 3
               AND history.DepositKBN = 1
               AND history.IsIssued   = @IsIssued
               AND history.CancelKBN  = 0
               AND sales.DeleteDateTime IS NULL
             GROUP BY SalesNO
           ) D3;

    -- レシートデータ出力
    SELECT (SELECT Picture FROM M_StoreImage WHERE StoreCD = @StoreCD) Logo
          ,(SELECT CompanyName FROM M_StoreImage WHERE StoreCD = @StoreCD) CompanyName
          ,(  SELECT StoreName FROM M_StoreImage WHERE StoreCD = @StoreCD)StoreName
          ,t0.Address1
          ,t0.Address2
          ,t0.TelephoneNO
          ,(SELECT Char3 FROM M_MultiPorpose WHERE ID = 305 AND [Key] = '1') Char3
          ,(SELECT Char4 FROM M_MultiPorpose WHERE ID = 305 AND [Key] = '1') Char4
          ,t0.IssueDateTime
          ,t0.ReIssueDatetime
          ,t0.JanCD
          ,t0.SKUShortName
          ,t0.SalesUnitPrice
          ,t0.SalesSu
          ,t0.Kakaku
          ,t0.SalesTax
          ,t0.SalesTaxRate
          ,t0.TotalGaku
          ,t0.SumSalesSu
          ,t0.Subtotal 
          ,t0.TargetAmount8
          ,t0.ConsumptionTax8
          ,t0.TargetAmount10
          ,t0.ConsumptionTax10
          ,t0.Total
          ,t0.PaymentName1
          ,t0.AmountPay1
          ,t0.PaymentName2
          ,t0.AmountPay2
          ,t0.PaymentName3
          ,t0.AmountPay3
          ,t0.PaymentName4
          ,t0.AmountPay4
          ,t0.PaymentName5
          ,t0.AmountPay5
          ,t0.PaymentName6
          ,t0.AmountPay6
          ,t0.PaymentName7
          ,t0.AmountPay7
          ,t0.PaymentName8
          ,t0.AmountPay8
          ,t0.PaymentName9
          ,t0.AmountPay9
          ,t0.PaymentName10
          ,t0.AmountPay10
          ,t0.Refund
          ,t0.SalesLastPoint
          ,t0.CustomerLastPoint
          ,t0.CustomerKBN
          ,t0.StaffReceiptPrint
          ,t0.StoreReceiptPrint
          ,t0.SalesNO
      FROM (
            SELECT t1.StoreName
                  ,t1.Address1
                  ,t1.Address2
                  ,t1.TelephoneNO
                  ,t1.DepositDateTime IssueDateTime
                  ,CASE
                     WHEN @IsIssued = 1 THEN FORMAT(GETDATE(),'yyyy/MM/dd HH:mm:ss')
                     ELSE ''
                   END AS ReIssueDatetime
                  ,t1.JanCD
                  ,t1.SKUShortName
                  ,t1.SalesUnitPrice
                  ,t1.SalesSu
                  ,t1.Kakaku
                  ,t1.SalesTax
                  ,t1.SalesTaxRate
                  ,t1.TotalGaku
                  ,(SELECT SUM(CASE WHEN SalesSU IS NULL THEN 1 ELSE SalesSU END) FROM #Temp_D_DepositHistory1 t WHERE t.SalesNO= t1.SalesNO) SumSalesSU
                  ,(SELECT SUM(kakaku) FROM #Temp_D_DepositHistory1 t WHERE t.SalesNO = t1.SalesNO) Subtotal
                  ,t1.TargetAmount8
                  ,t1.SalesTax8 ConsumptionTax8
                  ,t1.TargetAmount10
                  ,t1.SalesTax10 ConsumptionTax10
                  ,(SELECT SUM(TotalGaku) FROM #Temp_D_DepositHistory1 t WHERE t.SalesNO = t1.SalesNO) Total
                  ,t2.PaymentName1
                  ,t2.AmountPay1
                  ,t2.PaymentName2
                  ,t2.AmountPay2
                  ,t2.PaymentName3
                  ,t2.AmountPay3
                  ,t2.PaymentName4
                  ,t2.AmountPay4
                  ,t2.PaymentName5
                  ,t2.AmountPay5
                  ,t2.PaymentName6
                  ,t2.AmountPay6
                  ,t2.PaymentName7
                  ,t2.AmountPay7
                  ,t2.PaymentName8
                  ,t2.AmountPay8
                  ,t2.PaymentName9
                  ,t2.AmountPay9
                  ,t2.PaymentName10
                  ,t2.AmountPay10
                  ,t3.Refund
                  ,t3.SalesLastPoint
                  ,t3.CustomerLastPoint
                  ,t3.CustomerKBN
                  ,t1.StaffReceiptPrint
                  ,t1.StoreReceiptPrint
                  ,t1.SalesNO
                  ,t1.DepositNO
              FROM #Temp_D_DepositHistory1 t1
              LEFT OUTER JOIN #Temp_D_DepositHistory2 t2 ON t2.SalesNO = t1.SalesNO
              LEFT OUTER JOIN #Temp_D_DepositHistory3 t3 ON t3.SalesNO = t1.SalesNO
             GROUP BY t1.StoreName
                     ,t1.Address1
                     ,t1.Address2
                     ,t1.TelephoneNO
                     ,t1.DepositDateTime
                     ,t1.JanCD
                     ,t1.SKUShortName
                     ,t1.SalesUnitPrice
                     ,t1.SalesSu
                     ,t1.kakaku
                     ,t1.SalesTax
                     ,t1.SalesTaxRate
                     ,t1.TotalGaku
                     ,t1.TargetAmount8 
                     ,t1.SalesTax8
                     ,t1.TargetAmount10
                     ,t1.SalesTax10
                     ,t2.PaymentName1
                     ,t2.AmountPay1
                     ,t2.PaymentName2
                     ,t2.AmountPay2
                     ,t2.PaymentName3
                     ,t2.AmountPay3
                     ,t2.PaymentName4
                     ,t2.AmountPay4
                     ,t2.PaymentName5
                     ,t2.AmountPay5
                     ,t2.PaymentName6
                     ,t2.AmountPay6
                     ,t2.PaymentName7
                     ,t2.AmountPay7
                     ,t2.PaymentName8
                     ,t2.AmountPay8
                     ,t2.PaymentName9
                     ,t2.AmountPay9
                     ,t2.PaymentName10
                     ,t2.AmountPay10
                     ,t3.Refund
                     ,t3.SalesLastPoint
                     ,t3.CustomerLastPoint
                     ,t3.CustomerKBN
                     ,t1.StaffReceiptPrint
                     ,t1.StoreReceiptPrint
                     ,t1.SalesNO
                     ,t1.DepositNO) t0
     ORDER BY t0.DepositNO

    -- ワークテーブルを削除
    DROP TABLE #Temp_D_DepositHistory1;
    DROP TABLE #Temp_D_DepositHistory2;
    DROP TABLE #Temp_D_DepositHistory3;

END
