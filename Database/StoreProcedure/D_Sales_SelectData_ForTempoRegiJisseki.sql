IF OBJECT_ID ( 'D_Sales_SelectData_ForTempoRegiJisseki', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Sales_SelectData_ForTempoRegiJisseki]
GO

/****** Object:  StoredProcedure [dbo].[D_Sales_SelectData_ForTempoRegiJisseki]    Script Date: 2019/10/20 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--  ======================================================================
--       Program Call    店舗レジ 店舗実績照会
--       Program ID      TempoRegiJisseki
--       Create date:    2019.10.20
--    ======================================================================
CREATE PROCEDURE [D_Sales_SelectData_ForTempoRegiJisseki]
    (    @Date      varchar(10)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

    SET NOCOUNT ON;
    
    --<< 日別の売上額テーブルを作成 >>
    IF OBJECT_ID( N'[dbo]..[#SalesTable]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#SalesTable];
      END
      
    SELECT DP.StoreCD  
          ,DP.SalesNO
          ,DP.SalesNORows
          ,(CASE WHEN DP.SalesDate = @Date THEN CEILING(DP.CardAmount / (1 + DP.TaxRate)) ELSE 0 END) AS CardAmount
          ,(CASE WHEN DP.SalesDate = @Date THEN CEILING(DP.CashAmount / (1 + DP.TaxRate)) ELSE 0 END) AS CashAmount
          ,(CASE WHEN DP.SalesDate = @Date THEN CEILING(DP.CreditAmount / (1 + DP.TaxRate)) ELSE 0 END) AS CreditAmount
          ,(CASE WHEN DP.SalesDate = @Date THEN CEILING(DP.OtherAmount / (1 + DP.TaxRate)) ELSE 0 END) AS OtherAmount
          ,(CASE WHEN DP.SalesDate = @Date THEN (CASE WHEN DP.DiscountAmount = 0 THEN DP.SalesAmount ELSE CEILING(DP.SalesAmount / (1 + DP.TaxRate)) END)
                 ELSE 0 END) AS SalesAmountDay
          ,(CASE WHEN DP.DiscountAmount = 0 THEN DP.SalesAmount ELSE CEILING(DP.SalesAmount / (1 + DP.TaxRate)) END) AS SalesAmountMonth            
    INTO #SalesTable 
    FROM ( SELECT DSP.StoreCD    
                 ,DSP.SalesNO      
                 ,DSP.SalesNORows
                 ,DST.SalesDate
                 ,(CASE WHEN (DSP.PurchaseAmount - DSP.TaxAmount) = 0 OR DSP.TaxAmount = 0 THEN 0
                        WHEN (DSP.TaxAmount / (DSP.PurchaseAmount - DSP.TaxAmount)) >= 0.09 THEN 0.1
                        ELSE 0.08 END) AS TaxRate
                 ,DSP.CardAmount
                 ,DSP.CashAmount
                 ,DSP.CreditAmount
                 ,(DSP.Denomination1Amount + DSP.Denomination2Amount + DSP.PointAmount + DSP.AdvanceAmount) AS OtherAmount
                 ,CASE WHEN DSP.DiscountAmount = 0 THEN DSP.PurchaseAmount - DSP.TaxAmount ELSE DSP.BillingAmount END AS SalesAmount
                 ,DSP.DiscountAmount
            FROM D_SalesTran DST
           INNER JOIN D_StorePayment DSP ON DST.SalesNO = DSP.SalesNO
           WHERE LEFT(CONVERT(NVARCHAR, DST.SalesDate, 112),6) = LEFT(CONVERT(NVARCHAR, convert(date,@Date), 112),6)
             AND DST.SalesEntryKBN = 2
          ) DP
    
    --カーソル定義
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.StoreCD                
              ,tbl.SalesNO
              ,tbl.SalesNORows
              ,tbl.CardAmount
              ,tbl.CashAmount
              ,tbl.CreditAmount
              ,tbl.OtherAmount
              ,(SELECT MAX(Amount) FROM (VALUES (CardAmount),(CashAmount),(CreditAmount),(OtherAmount)) AS LIST(Amount)) AS Max_Amount
              ,(tbl.SalesAmountDay - (tbl.CardAmount + tbl.CashAmount + tbl.CreditAmount + tbl.OtherAmount)) AS Difference
          FROM #SalesTable AS tbl
         WHERE (tbl.SalesAmountDay - (tbl.CardAmount + tbl.CashAmount + tbl.CreditAmount + tbl.OtherAmount)) <> 0
        ;
        
    DECLARE @StoreCD Varchar(6);
    DECLARE @SalesNO Varchar(11);
    DECLARE @SalesNORows int;
    DECLARE @CardAmount money;
    DECLARE @CashAmount money;
    DECLARE @CreditAmount money;
    DECLARE @OtherAmount money;
    DECLARE @Max_Amount money;
    DECLARE @Difference money;
    
    --カーソルオープン
    OPEN CUR_TABLE;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_TABLE
    INTO @StoreCD, @SalesNO, @SalesNORows, @CardAmount, @CashAmount, @CreditAmount, @OtherAmount, @Max_Amount, @Difference;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===       
        
       --【D_Stock】Update   Table転送仕様Ｄ
       IF @CardAmount = @Max_Amount
       BEGIN
           UPDATE #SalesTable SET
                  CardAmount = CardAmount + @Difference
            WHERE StoreCD = @StoreCD
              AND SalesNO = @SalesNO
              AND SalesNORows = @SalesNORows
           ;
       END
       ELSE IF @CashAmount = @Max_Amount
       BEGIN
           UPDATE #SalesTable SET
                  CashAmount = CashAmount + @Difference
            WHERE StoreCD = @StoreCD
              AND SalesNO = @SalesNO
              AND SalesNORows = @SalesNORows
           ;
       END
       ELSE IF @CreditAmount = @Max_Amount
       BEGIN
           UPDATE #SalesTable SET
                  CreditAmount = CreditAmount + @Difference
            WHERE StoreCD = @StoreCD
              AND SalesNO = @SalesNO
              AND SalesNORows = @SalesNORows
           ;
       END
       ELSE 
       BEGIN
           UPDATE #SalesTable SET
                  OtherAmount = OtherAmount + @Difference
            WHERE StoreCD = @StoreCD
              AND SalesNO = @SalesNO
              AND SalesNORows = @SalesNORows
           ;
       END
       
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @StoreCD, @SalesNO, @SalesNORows, @CardAmount, @CashAmount, @CreditAmount, @OtherAmount, @Max_Amount, @Difference;

    END     --LOOPの終わり
    
    --カーソルを閉じる
    CLOSE CUR_TABLE;
    DEALLOCATE CUR_TABLE;
    
    
    --<< データ取得処理 >>
    WITH Juchuu AS ( 
          SELECT DJ.StoreCD
                ,SUM(CASE ISNULL(DJ.ReturnFLG,0) WHEN 0 THEN ISNULL(DJ.HanbaiHontaiGaku,0) WHEN 1 THEN ISNULL(DJ.HanbaiHontaiGaku,0) * -1 ELSE 0 END) AS JuchuuGaku
                ,COUNT(DJ.JuchuuNO) AS CSCount
            FROM D_Juchuu DJ
           WHERE DJ.DeleteDateTime IS NULL
             AND DJ.JuchuuDate = @Date
             AND DJ.JuchuuKBN IN (2,3)
           GROUP BY DJ.StoreCD
    ),
    Sales1 AS (
          SELECT DS.StoreCD  
                ,SUM(DS.CardAmount) AS CardAmount
                ,SUM(DS.CashAmount) AS CashAmount
                ,SUM(DS.CreditAmount) AS CreditAmount
                ,SUM(DS.OtherAmount) AS OtherAmount
                ,SUM(DS.SalesAmountDay) AS SalesAmountDay
                ,SUM(DS.SalesAmountMonth) AS SalesAmountMonth
            FROM #SalesTable DS
            GROUP BY DS.StoreCD
    ),
    Sales2 AS (
          SELECT DST.StoreCD
                ,SUM(CASE WHEN DST.SalesDate = @Date THEN DST.SalesHontaiGaku ELSE 0 END) AS KakeAmountDay
                ,SUM(DST.SalesHontaiGaku) AS KakeAmountMonth
            FROM D_SalesTran DST
           WHERE LEFT(CONVERT(NVARCHAR, DST.SalesDate, 112),6) = LEFT(CONVERT(NVARCHAR, convert(date,@Date), 112),6)
             AND DST.SalesEntryKBN = 3
            GROUP BY DST.StoreCD
    ),
    Budget AS (
          SELECT MS.StoreCD
                ,(CASE WHEN ISNULL(MSB.YearBudget,0) = 0 THEN 0
                       ELSE 
                           CASE WHEN MS.StorePlaceKBN = 1 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY1,0)
                                WHEN MS.StorePlaceKBN = 2 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY2,0)
                                WHEN MS.StorePlaceKBN = 3 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY3,0)
                                WHEN MS.StorePlaceKBN = 4 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY4,0)
                                WHEN MS.StorePlaceKBN = 5 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY5,0)
                                WHEN MS.StorePlaceKBN = 6 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY6,0)
                                WHEN MS.StorePlaceKBN = 7 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY7,0)
                                WHEN MS.StorePlaceKBN = 8 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY8,0)
                                WHEN MS.StorePlaceKBN = 9 THEN ROUND((ISNULL(MSB.YearBudget,0)/12) / CL.DAY9,0)
                                ELSE 0 END 
                        END
                  ) AS BudgetGakuDay
                ,ROUND(ISNULL(MSB.YearBudget,0) / 12,0) AS BudgetGakuMonth
            FROM F_Store(@Date) MS
            LEFT JOIN M_StoreBudget MSB ON MS.StoreCD = MSB.StoreCD 
            LEFT JOIN M_FiscalYear MFY ON MSB.FiscalYear = MFY.FiscalYear
            LEFT JOIN ( SELECT convert(date,@Date) AS SalesDate
                              ,SUM(CASE WHEN DayOff1 = 0 THEN 1 ELSE 0 END) AS DAY1
                              ,SUM(CASE WHEN DayOff2 = 0 THEN 1 ELSE 0 END) AS DAY2
                              ,SUM(CASE WHEN DayOff3 = 0 THEN 1 ELSE 0 END) AS DAY3
                              ,SUM(CASE WHEN DayOff4 = 0 THEN 1 ELSE 0 END) AS DAY4
                              ,SUM(CASE WHEN DayOff5 = 0 THEN 1 ELSE 0 END) AS DAY5
                              ,SUM(CASE WHEN DayOff6 = 0 THEN 1 ELSE 0 END) AS DAY6
                              ,SUM(CASE WHEN DayOff7 = 0 THEN 1 ELSE 0 END) AS DAY7
                              ,SUM(CASE WHEN DayOff8 = 0 THEN 1 ELSE 0 END) AS DAY8
                              ,SUM(CASE WHEN DayOff9 = 0 THEN 1 ELSE 0 END) AS DAY9      
                          FROM M_Calendar
                         WHERE LEFT(CONVERT(NVARCHAR, CalendarDate, 112),6) = LEFT(CONVERT(NVARCHAR, convert(date,@Date), 112),6)
                       ) CL ON CL.SalesDate = convert(date,@Date)   
           WHERE @Date BETWEEN MFY.FiscalStartDate AND MFY.FiscalEndDate
    )     
    SELECT MS.StoreCD
          ,MS.StoreName
          ,ISNULL(Juchuu.JuchuuGaku,0) AS JuchuuGaku    
          ,ISNULL(Sales1.CardAmount,0) AS CardUriageGaku
          ,ISNULL(Sales1.CashAmount,0) AS CashUriageGaku
          ,ISNULL(Sales1.CreditAmount,0) + ISNULL(Sales2.KakeAmountDay,0) AS KakeUriageGaku
          ,ISNULL(Sales1.OtherAmount,0) AS HokaUriageGaku
          ,ISNULL(Sales1.SalesAmountDay,0) + ISNULL(Sales2.KakeAmountDay,0) AS SalesGakuDay
          ,ISNULL(Sales1.SalesAmountMonth,0) + ISNULL(Sales2.KakeAmountMonth,0) AS SalesGakuMonth
          ,ISNULL(Budget.BudgetGakuDay,0) AS BudgetGakuDay
          ,ISNULL(Budget.BudgetGakuMonth,0) AS BudgetGakuMonth
          ,CASE WHEN ISNULL(Budget.BudgetGakuDay,0) = 0 THEN 0 ELSE ROUND((ISNULL(Sales1.SalesAmountDay,0) + ISNULL(Sales2.KakeAmountDay,0)) / ISNULL(Budget.BudgetGakuDay,0) * 100, 2) END AS RituDay
          ,CASE WHEN ISNULL(Budget.BudgetGakuMonth,0) = 0 THEN 0 ELSE ROUND((ISNULL(Sales1.SalesAmountMonth,0) + ISNULL(Sales2.KakeAmountMonth,0)) / ISNULL(Budget.BudgetGakuMonth,0) * 100, 2) END AS RituMonth
          ,ISNULL(Juchuu.CSCount,0) AS CSCount
      FROM F_Store(@Date) MS
      LEFT JOIN Juchuu ON MS.StoreCD = Juchuu.StoreCD
      LEFT JOIN Sales1 ON MS.StoreCD = Sales1.StoreCD
      LEFT JOIN Sales2 ON MS.StoreCD = Sales2.StoreCD
      LEFT JOIN BUDGET ON MS.StoreCD = Budget.StoreCD       
      WHERE MS.DeleteFlg <> 1
        AND MS.StoreKBN = 1           
      ORDER BY MS.StoreCD
      ;
END

GO

