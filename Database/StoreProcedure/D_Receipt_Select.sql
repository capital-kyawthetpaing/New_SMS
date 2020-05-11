USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 領収書印刷　レシート印刷出力
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2019.11.19
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Receipt_Select]
(
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

    -- ワークテーブルが残っている場合は削除
    IF OBJECT_ID( N'#Temp_D_DepositHistory1', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#Temp_D_DepositHistory1];
    END

    IF OBJECT_ID( N'#Temp_D_DepositHistory2', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#Temp_D_DepositHistory2];
    END

    IF OBJECT_ID( N'#Temp_D_DepositHistory3', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#Temp_D_DepositHistory3];
    END

    -- ワークテーブル１作成
    SELECT * 
      INTO #Temp_D_DepositHistory1
      FROM (SELECT history.Number SalesNO
                  ,history.DepositNO
                  ,history.JanCD
                  ,sku.SKUShortName
                  ,history.DepositDateTime
                  ,CASE
                     WHEN history.SalesSu = 1 THEN NULL
                     ELSE history.SalesUnitPrice
                   END AS SalesUnitPrice
                  ,CASE
                     WHEN history.SalesSu = 1 THEN NULL
                     ELSE history.SalesSu
                   END AS SalesSu
                  ,history.SalesGaku
                  ,history.SalesTax
                  ,sales.SalesHontaiGaku8
                  ,sales.SalesHontaiGaku10
                  ,sales.SalesTax8
                  ,sales.SalesTax10
                  ,staff.ReceiptPrint StaffReceiptPrint
                  ,store.ReceiptPrint StoreReceiptPrint
                  ,store.StoreName
                  ,store.Address1
                  ,store.Address2
                  ,store.TelphoneNO
              FROM D_DepositHistory history
              LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
              LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY JanCD, SKUCD ORDER BY ChangeDate DESC) as RANK
                                     ,JanCD
                                     ,SKUCD
                                     ,ChangeDate
                                     ,SKUShortName
                                     ,DeleteFlg
                                 FROM M_SKU
                              ) sku ON sku.RANK = 1
                                   AND sku.JanCD = history.JanCD
                                   AND sku.SKUCD = history.SKUCD
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
                                     AND store.StoreCD = history.StoreCD
                                     AND store.ChangeDate <= CONVERT(date, history.DepositDateTime)
             WHERE history.Number     = @SalesNO 
               AND history.DataKBN    = 2
               AND history.DepositKBN = 1
               AND history.IsIssued   = @IsIssued
               AND history.CancelKBN  = 0
               AND sku.DeleteFlg      = 0
               AND staff.DeleteFlg    = 0
               AND store.DeleteFlg    = 0
               AND sales.DeleteDateTime IS NULL) D1;

    -- ワークテーブル２作成
    SELECT *
      INTO #Temp_D_DepositHistory2
      FROM (SELECT D.SalesNO
                  ,MAX(CASE D.seq WHEN  1 THEN D.DenominationName ELSE NULL END) AS denominationName1
                  ,MAX(CASE D.seq WHEN  1 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku1
                  ,MAX(CASE D.seq WHEN  2 THEN D.DenominationName ELSE NULL END) AS denominationName2
                  ,MAX(CASE D.seq WHEN  2 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku2
                  ,MAX(CASE D.seq WHEN  3 THEN D.DenominationName ELSE NULL END) AS denominationName3
                  ,MAX(CASE D.seq WHEN  3 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku3
                  ,MAX(CASE D.seq WHEN  4 THEN D.DenominationName ELSE NULL END) AS denominationName4
                  ,MAX(CASE D.seq WHEN  4 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku4
                  ,MAX(CASE D.seq WHEN  5 THEN D.DenominationName ELSE NULL END) AS denominationName5
                  ,MAX(CASE D.seq WHEN  5 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku5
                  ,MAX(CASE D.seq WHEN  6 THEN D.DenominationName ELSE NULL END) AS denominationName6
                  ,MAX(CASE D.seq WHEN  6 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku6
                  ,MAX(CASE D.seq WHEN  7 THEN D.DenominationName ELSE NULL END) AS denominationName7
                  ,MAX(CASE D.seq WHEN  7 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku7
                  ,MAX(CASE D.seq WHEN  8 THEN D.DenominationName ELSE NULL END) AS denominationName8
                  ,MAX(CASE D.seq WHEN  8 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku8
                  ,MAX(CASE D.seq WHEN  9 THEN D.DenominationName ELSE NULL END) AS denominationName9
                  ,MAX(CASE D.seq WHEN  9 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku9
                  ,MAX(CASE D.seq WHEN 10 THEN D.DenominationName ELSE NULL END) AS denominationName10
                  ,MAX(CASE D.seq WHEN 10 THEN D.DepositGaku      ELSE NULL END) AS DepositGaku10
              FROM (SELECT @SalesNO SalesNO
                          ,history.DepositNO
                          ,denomination.DenominationName
                          ,history.DepositGaku
                          ,ROW_NUMBER() OVER (PARTITION BY history.Number ORDER BY history.DepositNO) AS SEQ
                      FROM D_DepositHistory history
                      LEFT OUTER JOIN M_DenominationKBN denomination
                                   ON denomination.DenominationCD = history.DenominationCD
                     WHERE history.Number     = @SalesNO
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
      FROM (SELECT @SalesNO SalesNO
                  ,history.Refund
                  ,sales.LastPoint SalesLastPoint
                  ,customer.LastPoint CustomerLasPoint
              FROM D_DepositHistory history
              LEFT OUTER JOIN D_Sales sales
                           ON sales.SalesNO = history.Number
              LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) as RANK
                                     ,CustomerCD
                                     ,ChangeDate
                                     ,CustomerKBN
                                     ,LastPoint
                                     ,DeleteFlg
                                 FROM M_Customer
                              ) customer ON customer.RANK = 1
                                        AND customer.CustomerCD = sales.CustomerCD
                                        AND customer.ChangeDate <= sales.SalesDate
             WHERE history.Number       = @SalesNO
               AND history.DataKBN      = 1
               AND history.DepositKBN   = 1
               AND history.IsIssued     = @IsIssued
               AND history.CancelKBN    = 0
               AND customer.DeleteFlg   = 0
       AND customer.CustomerKBN = 1
       AND sales.DeleteDateTime IS NULL
           ) D3;

    -- レシートデータ出力
    SELECT image.Picture Logo
          ,control.CompanyName
          ,t0.StoreName
          ,t0.Address1
          ,t0.Address2
          ,t0.TelphoneNO
          ,multiPorpose.Char3
          ,multiPorpose.Char4
          ,t0.DepositDateTime
          ,t0.IssuedDatetime
          ,t0.JanCD
          ,t0.SKUShortName
          ,t0.SalesUnitPrice
          ,t0.SalesSu
          ,t0.SalesGaku
          ,t0.SumSalesSu
          ,t0.SumSalesGaku
          ,t0.SalesHontaiGaku8 
          ,t0.SalesTax8
          ,t0.SalesHontaiGaku10
          ,t0.SalesTax10
          ,t0.TotalSalesGaku
          ,t0.DenominationName1
          ,t0.DepositGaku1
          ,t0.DenominationName2
          ,t0.DepositGaku2
          ,t0.DenominationName3
          ,t0.DepositGaku3
          ,t0.DenominationName4
          ,t0.DepositGaku4
          ,t0.DenominationName5
          ,t0.DepositGaku5
          ,t0.DenominationName6
          ,t0.DepositGaku6
          ,t0.DenominationName7
          ,t0.DepositGaku7
          ,t0.DenominationName8
          ,t0.DepositGaku8
          ,t0.DenominationName9
          ,t0.DepositGaku9
          ,t0.DenominationName10
          ,t0.DepositGaku10
          ,t0.Refund
          ,t0.SalesLastPoint
          ,t0.CustomerLasPoint
          ,t0.StaffReceiptPrint
          ,t0.StoreReceiptPrint
          ,t0.SalesNO
      FROM (SELECT t1.StoreName
                  ,t1.Address1
                  ,t1.Address2
                  ,t1.TelphoneNO
                  ,t1.DepositDateTime
                  ,CASE
                     WHEN @IsIssued = 1 THEN FORMAT(GETDATE(),'yyyy/MM/dd HH:mm:ss')
                     ELSE ''
                   END AS IssuedDatetime
                  ,t1.JanCD
                  ,t1.SKUShortName
                  ,t1.SalesUnitPrice
                  ,t1.SalesSu
                  ,t1.SalesGaku
                  ,SUM(t1.SalesSu) SumSalesSu
                  ,SUM(t1.SalesGaku) SumSalesGaku
                  ,t1.SalesHontaiGaku8 
                  ,t1.SalesTax8
                  ,t1.SalesHontaiGaku10
                  ,t1.SalesTax10
                  ,t1.SalesGaku + t1.SalesHontaiGaku8 + t1.SalesHontaiGaku10 TotalSalesGaku
                  ,t2.DenominationName1
                  ,t2.DepositGaku1
                  ,t2.DenominationName2
                  ,t2.DepositGaku2
                  ,t2.DenominationName3
                  ,t2.DepositGaku3
                  ,t2.DenominationName4
                  ,t2.DepositGaku4
                  ,t2.DenominationName5
                  ,t2.DepositGaku5
                  ,t2.DenominationName6
                  ,t2.DepositGaku6
                  ,t2.DenominationName7
                  ,t2.DepositGaku7
                  ,t2.DenominationName8
                  ,t2.DepositGaku8
                  ,t2.DenominationName9
                  ,t2.DepositGaku9
                  ,t2.DenominationName10
                  ,t2.DepositGaku10
                  ,t3.Refund
                  ,CASE
                     WHEN @IsIssued = 1 Then t3.SalesLastPoint
                     ELSE ''
                   END SalesLastPoint
                  ,t3.CustomerLasPoint
                  ,t1.StaffReceiptPrint
                  ,t1.StoreReceiptPrint
                  ,t1.SalesNO
                  ,t1.DepositNO
              FROM #Temp_D_DepositHistory1 t1
              LEFT OUTER JOIN #Temp_D_DepositHistory2 t2
                           ON t2.SalesNO = t1.SalesNO
              LEFT OUTER JOIN #Temp_D_DepositHistory3 t3
                           ON t3.SalesNO = t1.SalesNO
             GROUP BY t1.StoreName
                     ,t1.Address1
                     ,t1.Address2
                     ,t1.TelphoneNO
                     ,t1.DepositDateTime
                     ,t1.JanCD
                     ,t1.SKUShortName
                     ,t1.SalesUnitPrice
                     ,t1.SalesSu
                     ,t1.SalesGaku
                     ,t1.SalesHontaiGaku8 
                     ,t1.SalesTax8
                     ,t1.SalesHontaiGaku10
                     ,t1.SalesTax10
                     ,t2.DenominationName1
                     ,t2.DepositGaku1
                     ,t2.DenominationName2
                     ,t2.DepositGaku2
                     ,t2.DenominationName3
                     ,t2.DepositGaku3
                     ,t2.DenominationName4
                     ,t2.DepositGaku4
                     ,t2.DenominationName5
                     ,t2.DepositGaku5
                     ,t2.DenominationName6
                     ,t2.DepositGaku6
                     ,t2.DenominationName7
                     ,t2.DepositGaku7
                     ,t2.DenominationName8
                     ,t2.DepositGaku8
                     ,t2.DenominationName9
                     ,t2.DepositGaku9
                     ,t2.DenominationName10
                     ,t2.DepositGaku10
                     ,t3.Refund
                     ,t3.SalesLastPoint
                     ,t3.CustomerLasPoint
                     ,t1.StaffReceiptPrint
                     ,t1.StoreReceiptPrint
                     ,t1.SalesNO
                     ,t1.DepositNO) t0
      LEFT OUTER JOIN M_Control control 
                   ON control.MainKey = 1
      LEFT OUTER JOIN M_Image image
                   ON image.ID = 2
      LEFT OUTER JOIN M_MultiPorpose multiPorpose
                   ON multiPorpose.ID = 305
                  AND multiPorpose.[Key] = '1'
     ORDER BY t0.DepositNO
     ;

END
GO


