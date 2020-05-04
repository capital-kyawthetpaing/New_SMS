IF OBJECT_ID ( 'D_Sales_SelectData_ForTempoRegiJisseki', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Sales_SelectData_ForTempoRegiJisseki]
GO

/****** Object:  StoredProcedure [dbo].[D_Sales_SelectData_ForTempoRegiJisseki]    Script Date: 2019/10/20 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--  ======================================================================
--       Program Call    ìXï‹ÉåÉW ìXï‹é¿ê—è∆âÔ
--       Program ID      TempoRegiJisseki
--       Create date:    2019.10.20
--    ======================================================================
CREATE PROCEDURE [D_Sales_SelectData_ForTempoRegiJisseki]
    (    @Date      varchar(10)
    )AS
    
--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    WITH Sales AS (
          SELECT DS.StoreCD
                ,MS.StoreName
                ,DS.SalesDate
                ,DS.SalesGaku
                ,DJ.JuchuuNO
                ,(CASE ISNULL(DJ.ReturnFLG,0) WHEN 0 THEN ISNULL(DJ.HanbaiHontaiGaku,0) WHEN 1 THEN ISNULL(DJ.HanbaiHontaiGaku,0) * -1 ELSE 0 END) AS JuchuuGaku 
                ,(CASE WHEN DS.BillingType = 1 AND ISNULL(MD.SystemKBN,0) = 2 THEN ISNULL(DC.HontaiGaku,0) ELSE 0 END) AS CardUriageGaku
                ,(CASE WHEN DS.BillingType = 1 AND ISNULL(MD.SystemKBN,0) = 1 THEN ISNULL(DC.HontaiGaku,0) ELSE 0 END) AS CashUriageGaku
                ,(CASE WHEN DS.BillingType = 2 THEN ISNULL(DC.HontaiGaku,0) ELSE 0 END) AS KakeUriageGaku
                ,(CASE WHEN DS.BillingType = 2 AND DC.PaymentMethodCD NOT IN (1,2) THEN ISNULL(DC.HontaiGaku,0) ELSE 0 END) AS HokaUriageGaku
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
                 ) BudgetGakuDay
                ,ROUND(ISNULL(MSB.YearBudget,0) / 12,0) AS BudgetGakuMonth
            FROM D_Sales DS
            LEFT JOIN D_CollectPlan DC ON DS.SalesNO = DC.SalesNO
            LEFT JOIN D_Juchuu DJ ON DC.JuchuuNO = DJ.JuchuuNO
            LEFT JOIN M_DenominationKBN MD ON DC.PaymentMethodCD = MD.DenominationCD
            INNER JOIN ( SELECT A.StoreCD, A.StoreName , A.StorePlaceKBN
                    FROM (
                        SELECT StoreCD, StoreName , StorePlaceKBN,
                               ROW_NUMBER() OVER (PARTITION BY StoreCD ORDER BY ChangeDate DESC) AS RowNum
                        FROM M_Store 
                        WHERE DeleteFlg = 0
                          AND StoreKBN = 1
                          AND ChangeDate <= convert(date,@Date)
                         ) A
                    WHERE A.RowNum = 1
                ) MS ON DS.StoreCD = MS.StoreCD
            LEFT JOIN M_StoreBudget MSB ON MS.StoreCD = MSB.StoreCD 
                                AND MSB.FiscalYear = ( SELECT FiscalYear FROM M_Control WHERE MainKey = 1)
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
            WHERE DS.DeleteDateTime IS NULL
              AND DC.DeleteDateTime IS NULL
              AND DC.InvalidFLG = 0
              AND DJ.JuchuuKBN IN (2,3)
        )
    SELECT DS_D.StoreCD
          ,MAX(DS_D.StoreName) AS StoreName
          ,SUM(DS_D.JuchuuGaku) AS JuchuuGaku                   
          ,SUM(DS_D.CardUriageGaku) AS CardUriageGaku
          ,SUM(DS_D.CashUriageGaku) AS CashUriageGaku
          ,SUM(DS_D.KakeUriageGaku) AS KakeUriageGaku
          ,SUM(DS_D.HokaUriageGaku) AS HokaUriageGaku
          ,SUM(DS_D.SalesGaku) AS SalesGakuDay
          ,MAX(DS_M.SalesGaku) AS SalesGakuMonth
          ,MAX(DS_D.BudgetGakuDay) BudgetGakuDay
          ,MAX(DS_D.BudgetGakuMonth) AS BudgetGakuMonth
          ,CASE WHEN MAX(DS_D.BudgetGakuDay) = 0 THEN 0 ELSE ROUND(SUM(DS_D.SalesGaku) / MAX(DS_D.BudgetGakuDay) * 100, 2) END AS RituDay
          ,CASE WHEN MAX(DS_D.BudgetGakuMonth) = 0 THEN 0 ELSE ROUND(MAX(DS_M.SalesGaku) / MAX(DS_D.BudgetGakuMonth) * 100, 2) END AS RituMonth
          ,COUNT(DS_D.JuchuuNO) CSCount
      FROM Sales DS_D
      -- åéï îÑè„ã‡äz
      LEFT JOIN (SELECT DS.StoreCD
                       ,SUM(DS.SalesGaku) SalesGaku
                 FROM Sales DS
                 WHERE LEFT(CONVERT(NVARCHAR, DS.SalesDate, 112),6) = LEFT(CONVERT(NVARCHAR, convert(date,@Date), 112),6)   
                 GROUP BY DS.StoreCD
                ) DS_M ON DS_D.StoreCD = DS_M.StoreCD            
      WHERE DS_D.SalesDate = convert(date,@Date)              
      GROUP BY DS_D.StoreCD                 
      ORDER BY DS_D.StoreCD
      ;
END

GO

