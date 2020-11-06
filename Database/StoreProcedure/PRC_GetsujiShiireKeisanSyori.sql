 BEGIN TRY 
 Drop Procedure dbo.[PRC_GetsujiShiireKeisanSyori]
END try
BEGIN CATCH END CATCH 

/****** Object:  StoredProcedure [dbo].[PRC_GetsujiShiireKeisanSyori]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--  ======================================================================
--       Program Call    月次仕入計算処理
--       Program ID      GetsujiShiireKeisanSyori
--       Create date:    2020.2.9
--    ======================================================================
CREATE PROCEDURE [PRC_GetsujiShiireKeisanSyori]
    (    
    @FiscalYYYYMM int,
    @StoreCD  varchar(4),
    @Mode tinyint,	--1:全社
    @Operator  varchar(10),
    @PC  varchar(30)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @SYSDATE_YYYYMM varchar(7);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);

    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE_YYYYMM = SUBSTRING(CONVERT(varchar, @SYSDATETIME,111),1,7);

    DECLARE @W_FiscalYYYYMM varchar(7);
    SET @W_FiscalYYYYMM = SUBSTRING(CONVERT(varchar,@FiscalYYYYMM),1,4) + '/' + SUBSTRING(CONVERT(varchar,@FiscalYYYYMM),5,2);
    DECLARE @W_OldYYYYMM varchar(7);
    SET @W_OldYYYYMM = SUBSTRING(CONVERT(varchar,DATEADD(month, -1, CONVERT(date, @W_FiscalYYYYMM + '/01')),111),1,7);
    DECLARE @W_FiscalYYYYMMDD date;		--当月末
    SET @W_FiscalYYYYMMDD = DATEADD(day, -1 ,DATEADD(month, 1, CONVERT(date, @W_FiscalYYYYMM + '/01')));
    
    --DataのDelete
    --D_MonthlyPurchase（月別仕入状況）をDeleteする
    IF ISNULL(@FiscalYYYYMM,'') <> ''
    BEGIN
        IF @Mode = 1
        BEGIN
            --Mode＝1の場合(＝ALL店舗）@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyPurchase
            WHERE YYYYMM >= @FiscalYYYYMM
            ;
        END
        ELSE IF @Mode = 2
        BEGIN
            --Mode＝2の場合@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyPurchase
            WHERE YYYYMM >= @FiscalYYYYMM
            AND StoreCD = @StoreCD
            ;
        END
    END

    --DataのInsert
    --以下の順番に計算処理を行う
    --★Today のYYYYMM＜@FiscalYYYYMMになるまでの間 以下の処理を行う
    --データの行数分ループ処理を実行する
    WHILE @W_FiscalYYYYMM <= @SYSDATE_YYYYMM
    BEGIN
    --（Today のYYYYMM＜@FiscalYYYYMMになれば、処理を終える　☆へ）
        --Table転送仕様Ａ 
        --@FiscalYYYYMMの前月のデータから、今月のデータを作成する
        INSERT INTO [D_MonthlyPurchase]
               ( [VendorCD]
              ,[StoreCD]
              ,[YYYYMM]
              ,[SKUCD]
              ,[AdminNO]
              ,[JanCD]
              ,[LastMonthQuantity]
              ,[LastMonthAmount]
              ,[LastMonthCost]
              ,[ThisMonthPurchaseQ]
              ,[ThisMonthPurchaseA]
              ,[ThisMonthCustPurchaseQ]
              ,[ThisMonthCustPurchaseA]
              ,[ThisMonthPurchasePlanQ]
              ,[ThisMonthPurchasePlanA]
              ,[ThisMonthSalesQ]
              ,[ThisMonthSalesA]
              ,[ThisMonthCustSalesQ]
              ,[ThisMonthCustSalesA]
              ,[ThisMonthSalesPlanQ]
              ,[ThisMonthSalesPlanA]
              ,[ThisMonthReturnsQ]
              ,[ThisMonthReturnsA]
              ,[ThisMonthReturnsPlanQ]
              ,[ThisMonthReturnsPlanA]
              ,[ThisMonthPlanAmount]
              ,[ThisMonthPlanQuantity]
              ,[ThisMonthAmount]
              ,[ThisMonthQuantity]
              ,[ThisMonthCost]
              ,[InsertOperator]
              ,[InsertDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime])
        SELECT DM.VendorCD
              ,DM.StoreCD
              ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
              ,DM.SKUCD
              ,DM.AdminNO
              ,DM.JanCD
              ,DM.ThisMonthPurchaseQ AS LastMonthQuantity
              ,DM.ThisMonthAmount AS LastMonthAmount
              ,DM.ThisMonthCost AS LastMonthCost
              ,0 AS ThisMonthPurchaseQ
              ,0 AS ThisMonthPurchaseA
              ,0 AS ThisMonthCustPurchaseQ
              ,0 AS ThisMonthCustPurchaseA
              ,0 AS ThisMonthPurchasePlanQ
              ,0 AS ThisMonthPurchasePlanA
              ,0 AS ThisMonthSalesQ
              ,0 AS ThisMonthSalesA
              ,0 AS ThisMonthCustSalesQ
              ,0 AS ThisMonthCustSalesA
              ,0 AS ThisMonthSalesPlanQ
              ,0 AS ThisMonthSalesPlanA
              ,0 AS ThisMonthReturnsQ
              ,0 AS ThisMonthReturnsA
              ,0 AS ThisMonthReturnsPlanQ
              ,0 AS ThisMonthReturnsPlanA
              ,0 AS ThisMonthPlanAmount
              ,0 AS ThisMonthPlanQuantity
              ,0 AS ThisMonthAmount
              ,0 AS ThisMonthQuantity
              ,0 AS ThisMonthCost
              ,@Operator AS InsertOperator
              ,@SYSDATETIME AS InsertDateTime
              ,@Operator AS UpdateOperator
              ,@SYSDATETIME AS UpdateDateTime
        FROM D_MonthlyPurchase AS DM
        WHERE DM.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DM.StoreCD END)
        AND DM.YYYYMM = CONVERT(int,REPLACE(@W_OldYYYYMM,'/',''))
        AND (DM.ThisMonthQuantity <> 0 OR DM.ThisMonthAmount <> 0)
        ;

        --テーブル転送仕様Ｂ　履歴から実績
        UPDATE [D_MonthlyPurchase] SET
            [ThisMonthPurchaseQ] = DS.ThisMonthPurchaseQ
           ,[ThisMonthPurchaseA] = DS.ThisMonthPurchaseA
           ,[ThisMonthCustPurchaseQ] = DS.ThisMonthCustPurchaseQ
           ,[ThisMonthCustPurchaseA] = DS.ThisMonthCustPurchaseA
           ,[ThisMonthSalesQ] = DS.ThisMonthSalesQ
           ,[ThisMonthSalesA] = DS.ThisMonthSalesA
           ,[ThisMonthCustSalesQ] = DS.ThisMonthCustSalesQ
           ,[ThisMonthCustSalesA] = DS.ThisMonthCustSalesA           
           ,[ThisMonthReturnsQ] = DS.ThisMonthReturnsQ
           ,[ThisMonthReturnsA] = DS.ThisMonthReturnsA
           --,[ThisMonthMarkDownA] = DS.ThisMonthMarkDownA           

           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.AdminNO, DS.SoukoCD, DS.SKUCD, DS.JanCD
            ,ISNULL(DS.VendorCD,SK.MainVendorCD) AS VendorCD --, DS.WarehousingKBN
            ,MAX(MS.StoreCD) AS StoreCD
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN DS.Quantity ELSE 0 END)) AS ThisMonthPurchaseQ
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN DS.Amount ELSE 0 END)) AS ThisMonthPurchaseA
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN (CASE WHEN ISNULL(DO.JuchuuNO,'') <> '' THEN DS.Quantity ELSE 0 END)
                       ELSE 0 END)) AS ThisMonthCustPurchaseQ
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN (CASE WHEN ISNULL(DO.JuchuuNO,'') <> '' THEN DS.Amount ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustPurchaseA
            ,SUM((CASE WHEN DS.WarehousingKBN IN (23,24,25) THEN DS.Quantity ELSE 0 END)) AS ThisMonthSalesQ
            ,SUM((CASE WHEN DS.WarehousingKBN IN (23,24,25) THEN DS.Amount ELSE 0 END)) AS ThisMonthSalesA
            ,SUM((CASE WHEN DS.WarehousingKBN IN (5,23,24,25) THEN (CASE WHEN ISNULL(DO2.JuchuuNO,'') <> '' THEN DS.Quantity ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustSalesQ
            ,SUM((CASE WHEN DS.WarehousingKBN IN (5,23,24,25) THEN (CASE WHEN ISNULL(DO2.JuchuuNO,'') <> '' THEN DS.Amount ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustSalesA
            ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Quantity ELSE 0 END)) AS ThisMonthReturnsQ
            ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Amount ELSE 0 END)) AS ThisMonthReturnsA
            --,SUM((CASE DS.WarehousingKBN WHEN 40 THEN DS.Amount ELSE 0 END)) AS ThisMonthMarkDownA
    
            FROM D_Warehousing AS DS
            LEFT OUTER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
            ON MS.SoukoCD = DS.SoukoCD
            LEFT OUTER JOIN F_SKU(@W_FiscalYYYYMMDD) AS SK
            ON SK.AdminNO = DS.AdminNO
            AND SK.DeleteFlg = 0

            --客注仕入の判断(仕入明細から発注明細をJoinし、その発注明細に受注番号がセットされていれば客注分の仕入とみなす）
            LEFT OUTER JOIN D_PurchaseDetails AS DM
            ON DM.PurchaseNO = DS.Number
            AND DM.PurchaseRows = DS.NumberRow
            AND DM.DeleteDateTime IS NULL
            --AND 30 = DS.WarehousingKBN
            LEFT OUTER JOIN D_OrderDetails AS DO
            ON DO.OrderNO = DM.OrderNO
            AND DO.OrderRows = DM.OrderRows
            AND DO.DeleteDateTime IS NULL
            --客注売上の判断（出荷明細の受注番号が発注データ内にあれば、客注分の出荷売上とみなす）
            LEFT OUTER JOIN D_ShippingDetails AS DP
            ON DP.ShippingNO = DS.Number
            AND DP.ShippingRows = DS.NumberRow
            AND DP.DeleteDateTime IS NULL
            --AND 5,23,24,25 = DS.WarehousingKBN
            LEFT OUTER JOIN D_OrderDetails AS DO2
            ON DO2.JuchuuNO = DP.Number
            AND DO2.JuchuuRows = DP.NumberRows
            AND DO2.DeleteDateTime IS NULL

            WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.WarehousingDate,111),1,7) = @W_FiscalYYYYMM
            GROUP BY DS.AdminNO, DS.SoukoCD, DS.SKUCD, DS.JanCD, ISNULL(DS.VendorCD,SK.MainVendorCD)	-- DS.WarehousingKBN
        ) AS DS
        WHERE D_MonthlyPurchase.AdminNO = DS.AdminNO
        AND D_MonthlyPurchase.StoreCD = DS.StoreCD
        AND D_MonthlyPurchase.VendorCD = DS.VendorCD
        AND D_MonthlyPurchase.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;
        
        INSERT INTO [D_MonthlyPurchase]
           ([VendorCD]
           ,[StoreCD]
           ,[YYYYMM]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[LastMonthQuantity]
           ,[LastMonthAmount]
           ,[LastMonthCost]
           ,[ThisMonthPurchaseQ]
           ,[ThisMonthPurchaseA]
           ,[ThisMonthCustPurchaseQ]
           ,[ThisMonthCustPurchaseA]
           ,[ThisMonthPurchasePlanQ]
           ,[ThisMonthPurchasePlanA]
           ,[ThisMonthSalesQ]
           ,[ThisMonthSalesA]
           ,[ThisMonthCustSalesQ]
           ,[ThisMonthCustSalesA]
           ,[ThisMonthSalesPlanQ]
           ,[ThisMonthSalesPlanA]
           ,[ThisMonthReturnsQ]
           ,[ThisMonthReturnsA]
           ,[ThisMonthReturnsPlanQ]
           ,[ThisMonthReturnsPlanA]
           ,[ThisMonthPlanAmount]
           ,[ThisMonthPlanQuantity]
           ,[ThisMonthAmount]
           ,[ThisMonthQuantity]
           ,[ThisMonthCost]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT ISNULL(DS.VendorCD,SK.MainVendorCD) AS VendorCD
            ,MS.StoreCD
            ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
            ,MAX(DS.SKUCD) AS SKUCD
            ,DS.AdminNO
            ,MAX(DS.JanCD) AS JanCD
            ,0 AS LastMonthQuantity
            ,0 AS LastMonthAmount
            ,0 AS LastMonthCost
            
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN DS.Quantity ELSE 0 END)) AS ThisMonthPurchaseQ
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN DS.Amount ELSE 0 END)) AS ThisMonthPurchaseA
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN (CASE WHEN ISNULL(DO.JuchuuNO,'') <> '' THEN DS.Quantity ELSE 0 END)
                       ELSE 0 END)) AS ThisMonthCustPurchaseQ
            ,SUM((CASE WHEN DS.WarehousingKBN = 30 THEN (CASE WHEN ISNULL(DO.JuchuuNO,'') <> '' THEN DS.Amount ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustPurchaseA
            ,0 AS ThisMonthPurchasePlanQ
            ,0 AS ThisMonthPurchasePlanA
            ,SUM((CASE WHEN DS.WarehousingKBN IN (23,24,25) THEN DS.Quantity ELSE 0 END)) AS ThisMonthSalesQ
            ,SUM((CASE WHEN DS.WarehousingKBN IN (23,24,25) THEN DS.Amount ELSE 0 END)) AS ThisMonthSalesA
            ,SUM((CASE WHEN DS.WarehousingKBN IN (5,23,24,25) THEN (CASE WHEN ISNULL(DO2.JuchuuNO,'') <> '' THEN DS.Quantity ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustSalesQ
            ,SUM((CASE WHEN DS.WarehousingKBN IN (5,23,24,25) THEN (CASE WHEN ISNULL(DO2.JuchuuNO,'') <> '' THEN DS.Amount ELSE 0 END) 
                       ELSE 0 END)) AS ThisMonthCustSalesA
            ,0 AS ThisMonthSalesPlanQ
            ,0 AS ThisMonthSalesPlanA
            ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Quantity ELSE 0 END)) AS ThisMonthReturnsQ
             ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Amount ELSE 0 END)) AS ThisMonthReturnsA
            ,0 AS ThisMonthReturnsPlanQ
            ,0 AS ThisMonthReturnsPlanA
            --,SUM((CASE DS.WarehousingKBN WHEN 40 THEN DS.Amount ELSE 0 END)) AS ThisMonthMarkDownA
            
            ,0 AS ThisMonthPlanAmount
            ,0 AS ThisMonthPlanQuantity
            ,0 AS ThisMonthAmount
            ,0 AS ThisMonthQuantity
            ,0 AS ThisMonthCost
            ,@Operator AS InsertOperator
            ,@SYSDATETIME AS InsertDateTime
            ,@Operator AS UpdateOperator
            ,@SYSDATETIME AS UpdateDateTime
        FROM D_Warehousing AS DS
        LEFT OUTER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
        ON MS.SoukoCD = DS.SoukoCD
        LEFT OUTER JOIN F_SKU(@W_FiscalYYYYMMDD) AS SK
        ON SK.AdminNO = DS.AdminNO
        AND SK.DeleteFlg = 0
        --客注仕入の判断(仕入明細から発注明細をJoinし、その発注明細に受注番号がセットされていれば客注分の仕入とみなす）
        LEFT OUTER JOIN D_PurchaseDetails AS DM
        ON DM.PurchaseNO = DS.Number
        AND DM.PurchaseRows = DS.NumberRow
        AND DM.DeleteDateTime IS NULL
        --AND 30 = DS.WarehousingKBN
        LEFT OUTER JOIN D_OrderDetails AS DO
        ON DO.OrderNO = DM.OrderNO
        AND DO.OrderRows = DM.OrderRows
        AND DO.DeleteDateTime IS NULL
        --客注売上の判断（出荷明細の受注番号が発注データ内にあれば、客注分の出荷売上とみなす）
        LEFT OUTER JOIN D_ShippingDetails AS DP
        ON DP.ShippingNO = DS.Number
        AND DP.ShippingRows = DS.NumberRow
        AND DP.DeleteDateTime IS NULL
        --AND 5,23,24,25 = DS.WarehousingKBN
        LEFT OUTER JOIN D_OrderDetails AS DO2
        ON DO2.JuchuuNO = DP.Number
        AND DO2.JuchuuRows = DP.NumberRows
        AND DO2.DeleteDateTime IS NULL
        
        WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.WarehousingDate,111),1,7) = @W_FiscalYYYYMM
        AND ISNULL(DS.VendorCD,ISNULL(SK.MainVendorCD,'')) <> ''
        
        AND NOT EXISTS(SELECT A.AdminNO FROM D_MonthlyPurchase AS A
                       WHERE A.AdminNO = DS.AdminNO
                       AND A.StoreCD = MS.StoreCD
                       AND A.VendorCD = ISNULL(DS.VendorCD,SK.MainVendorCD)
                       AND A.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        GROUP BY DS.AdminNO, MS.StoreCD, ISNULL(DS.VendorCD,SK.MainVendorCD)--, DS.WarehousingKBN
        ;

        --Table転送仕様Ｃ
        --テーブル転送仕様Ｃ　仕入予定
        UPDATE [D_MonthlyPurchase] SET
            [ThisMonthPurchasePlanQ] = DS.ThisMonthPurchasePlanQ
            ,[ThisMonthPurchasePlanA] = DS.ThisMonthPurchasePlanA
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.AdminNO, DS.SoukoCD, DS.SKUCD, DS.JanCD, DS.OrderCD
            ,MAX(MS.StoreCD) AS StoreCD
            ,SUM((CASE WHEN ISNULL(DM.OrderNO,'') = '' THEN DS.ArrivalPlanSu 
                WHEN DP.PurchaseDate > @W_FiscalYYYYMMDD THEN DS.ArrivalPlanSu ELSE 0 END)) AS ThisMonthPurchasePlanQ
            ,SUM((CASE WHEN ISNULL(DM.OrderNO,'') = '' THEN DS.ArrivalPlanSu 
                WHEN DP.PurchaseDate > @W_FiscalYYYYMMDD THEN DS.ArrivalPlanSu ELSE 0 END)*DO.OrderUnitPrice) AS ThisMonthPurchasePlanA

            --入荷予定データから（仕入のない or  仕入が来月以降）未仕入れの情報を集計する
            FROM D_ArrivalPlan AS DS
            LEFT OUTER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
            ON MS.SoukoCD = DS.SoukoCD

            --入荷予定が仕入されているか？
            LEFT OUTER JOIN D_PurchaseDetails AS DM
            ON DM.OrderNO = DS.Number
            AND DM.OrderRows = DS.NumberRows
            AND DM.DeleteDateTime IS NULL
            --仕入日を仕入データから
            LEFT OUTER JOIN D_Purchase AS DP
            ON DP.PurchaseNO = DM.PurchaseNO
            AND DP.DeleteDateTime IS NULL
            --発注単価を発注明細から
            LEFT OUTER JOIN D_OrderDetails AS DO
            ON DO.OrderNO = DS.Number
            AND DO.OrderRows = DS.NumberRows
            AND DO.DeleteDateTime IS NULL

            WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.CalcuArrivalPlanDate,111),1,7) = @W_FiscalYYYYMM
            AND DS.ArrivalPlanKBN = 1
            GROUP BY DS.AdminNO, DS.SoukoCD, DS.SKUCD, DS.JanCD, DS.OrderCD	-- DS.WarehousingKBN
            ) AS DS
        WHERE D_MonthlyPurchase.AdminNO = DS.AdminNO
        AND D_MonthlyPurchase.StoreCD = DS.StoreCD
        AND D_MonthlyPurchase.VendorCD = DS.OrderCD
        AND D_MonthlyPurchase.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;
        
        INSERT INTO [D_MonthlyPurchase]
           ([VendorCD]
           ,[StoreCD]
           ,[YYYYMM]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[LastMonthQuantity]
           ,[LastMonthAmount]
           ,[LastMonthCost]
           ,[ThisMonthPurchaseQ]
           ,[ThisMonthPurchaseA]
           ,[ThisMonthCustPurchaseQ]
           ,[ThisMonthCustPurchaseA]
           ,[ThisMonthPurchasePlanQ]
           ,[ThisMonthPurchasePlanA]
           ,[ThisMonthSalesQ]
           ,[ThisMonthSalesA]
           ,[ThisMonthCustSalesQ]
           ,[ThisMonthCustSalesA]
           ,[ThisMonthSalesPlanQ]
           ,[ThisMonthSalesPlanA]
           ,[ThisMonthReturnsQ]
           ,[ThisMonthReturnsA]
           ,[ThisMonthReturnsPlanQ]
           ,[ThisMonthReturnsPlanA]
           ,[ThisMonthPlanAmount]
           ,[ThisMonthPlanQuantity]
           ,[ThisMonthAmount]
           ,[ThisMonthQuantity]
           ,[ThisMonthCost]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT DS.OrderCD
            ,MAX(MS.StoreCD) AS StoreCD
            ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
            ,MAX(DS.SKUCD) AS SKUCD
            ,DS.AdminNO
            ,MAX(DS.JanCD) AS JanCD
            ,0 AS LastMonthQuantity
            ,0 AS LastMonthAmount
            ,0 AS LastMonthCost
            
            ,0 AS ThisMonthPurchaseQ
            ,0 AS ThisMonthPurchaseA
            ,0 AS ThisMonthCustPurchaseQ
            ,0 AS ThisMonthCustPurchaseA
            ,SUM((CASE WHEN ISNULL(DM.OrderNO,'') = '' THEN DS.ArrivalPlanSu 
                       WHEN DP.PurchaseDate > @W_FiscalYYYYMMDD THEN DS.ArrivalPlanSu ELSE 0 END)) AS ThisMonthPurchasePlanQ
            ,SUM((CASE WHEN ISNULL(DM.OrderNO,'') = '' THEN DS.ArrivalPlanSu 
                       WHEN DP.PurchaseDate > @W_FiscalYYYYMMDD THEN DS.ArrivalPlanSu ELSE 0 END)*DO.OrderUnitPrice) AS ThisMonthPurchasePlanA
            ,0 AS ThisMonthSalesQ
            ,0 AS ThisMonthSalesA
            ,0 AS ThisMonthCustSalesQ
            ,0 AS ThisMonthCustSalesA
            ,0 AS [ThisMonthSalesPlanQ]
            ,0 AS [ThisMonthSalesPlanA]
            ,0 AS ThisMonthReturnsQ
            ,0 AS ThisMonthReturnsA
            ,0 AS [ThisMonthReturnsPlanQ]
            ,0 AS [ThisMonthReturnsPlanA]
            --,SUM((CASE DS.WarehousingKBN WHEN 40 THEN DS.Amount ELSE 0 END)) AS ThisMonthMarkDownA
            
            ,0 AS ThisMonthPlanAmount
            ,0 AS ThisMonthPlanQuantity
            ,0 AS ThisMonthAmount
            ,0 AS ThisMonthQuantity
            ,0 AS ThisMonthCost
            ,@Operator AS InsertOperator
            ,@SYSDATETIME AS InsertDateTime
            ,@Operator AS UpdateOperator
            ,@SYSDATETIME AS UpdateDateTime
        --入荷予定データから（仕入のない or  仕入が来月以降）未仕入れの情報を集計する
        FROM D_ArrivalPlan AS DS
        LEFT OUTER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
        ON MS.SoukoCD = DS.SoukoCD

        --入荷予定が仕入されているか？
        LEFT OUTER JOIN D_PurchaseDetails AS DM
        ON DM.OrderNO = DS.Number
        AND DM.OrderRows = DS.NumberRows
        AND DM.DeleteDateTime IS NULL
        --仕入日を仕入データから
        LEFT OUTER JOIN D_Purchase AS DP
        ON DP.PurchaseNO = DM.PurchaseNO
        AND DP.DeleteDateTime IS NULL
        --発注単価を発注明細から
        LEFT OUTER JOIN D_OrderDetails AS DO
        ON DO.OrderNO = DS.Number
        AND DO.OrderRows = DS.NumberRows
        AND DO.DeleteDateTime IS NULL

        WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.CalcuArrivalPlanDate,111),1,7) = @W_FiscalYYYYMM
        AND DS.ArrivalPlanKBN = 1
        AND DS.OrderCD IS NOT NULL
        
        AND NOT EXISTS(SELECT A.AdminNO FROM D_MonthlyPurchase AS A
                       WHERE A.AdminNO = DS.AdminNO
                       AND A.StoreCD = MS.StoreCD
                       AND A.VendorCD = DS.OrderCD
                       AND A.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        GROUP BY DS.AdminNO, DS.SoukoCD, DS.SKUCD, DS.JanCD, DS.OrderCD
        ;
        
        --Table転送仕様Ｄ
        --テーブル転送仕様Ｄ　売上予定
        UPDATE [D_MonthlyPurchase] SET
            [ThisMonthSalesPlanQ] = DS.ThisMonthSalesPlanQ
            ,[ThisMonthSalesPlanA] = DS.ThisMonthSalesPlanA
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DM.AdminNO, DM.SKUCD, DM.JanCD
            ,ISNULL(DM.VendorCD,SK.MainVendorCD) AS VendorCD
            ,DJ.StoreCD
            ,SUM((CASE WHEN ISNULL(DS.JuchuuNO,'') = '' THEN DM.JuchuuSuu
                       WHEN DH.SalesDate > @W_FiscalYYYYMMDD THEN DM.JuchuuSuu ELSE 0 END)) AS ThisMonthSalesPlanQ
            ,SUM((CASE WHEN ISNULL(DS.JuchuuNO,'') = '' THEN DM.CostGaku 
                       WHEN DH.SalesDate > @W_FiscalYYYYMMDD THEN DM.CostGaku ELSE 0 END)) AS ThisMonthSalesPlanA

            FROM D_Juchuu AS DJ
            LEFT OUTER JOIN D_JuchuuDetails AS DM
            ON DM.JuchuuNO = DJ.JuchuuNO
            AND DM.DeleteDateTime IS NULL
            
            LEFT OUTER JOIN D_SalesDetails AS DS
            ON DS.JuchuuNO = DM.JuchuuNO
            AND DS.JuchuuRows = DM.JuchuuRows
            AND DS.DeleteDateTime IS NULL

            LEFT OUTER JOIN D_Sales AS DH
            ON DH.SalesNO = DS.SalesNO
            AND DH.DeleteDateTime IS NULL
            
            LEFT OUTER JOIN F_SKU(@W_FiscalYYYYMMDD) AS SK
            ON SK.AdminNO = DM.AdminNO
            AND SK.DeleteFlg = 0

            WHERE DJ.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DJ.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DJ.SalesPlanDate,111),1,7) = @W_FiscalYYYYMM
            AND DJ.DeleteDateTime IS NULL
            GROUP BY DJ.StoreCD, DM.AdminNO, DM.SKUCD, DM.JanCD, ISNULL(DM.VendorCD,SK.MainVendorCD)
            ) AS DS
        WHERE D_MonthlyPurchase.AdminNO = DS.AdminNO
        AND D_MonthlyPurchase.StoreCD = DS.StoreCD
        AND D_MonthlyPurchase.VendorCD = DS.VendorCD
        AND D_MonthlyPurchase.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;
        
        INSERT INTO [D_MonthlyPurchase]
           ([VendorCD]
           ,[StoreCD]
           ,[YYYYMM]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[LastMonthQuantity]
           ,[LastMonthAmount]
           ,[LastMonthCost]
           ,[ThisMonthPurchaseQ]
           ,[ThisMonthPurchaseA]
           ,[ThisMonthCustPurchaseQ]
           ,[ThisMonthCustPurchaseA]
           ,[ThisMonthPurchasePlanQ]
           ,[ThisMonthPurchasePlanA]
           ,[ThisMonthSalesQ]
           ,[ThisMonthSalesA]
           ,[ThisMonthCustSalesQ]
           ,[ThisMonthCustSalesA]
           ,[ThisMonthSalesPlanQ]
           ,[ThisMonthSalesPlanA]
           ,[ThisMonthReturnsQ]
           ,[ThisMonthReturnsA]
           ,[ThisMonthReturnsPlanQ]
           ,[ThisMonthReturnsPlanA]
           ,[ThisMonthPlanAmount]
           ,[ThisMonthPlanQuantity]
           ,[ThisMonthAmount]
           ,[ThisMonthQuantity]
           ,[ThisMonthCost]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT ISNULL(DM.VendorCD,SK.MainVendorCD) AS VendorCD
            ,DJ.StoreCD
    		,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
            ,DM.SKUCD
        	,DM.AdminNO
            ,DM.JanCD
            ,0 AS LastMonthQuantity
            ,0 AS LastMonthAmount
            ,0 AS LastMonthCost
            
            ,0 AS ThisMonthPurchaseQ
            ,0 AS ThisMonthPurchaseA
            ,0 AS ThisMonthCustPurchaseQ
            ,0 AS ThisMonthCustPurchaseA
            ,0 AS ThisMonthPurchasePlanQ
            ,0 AS ThisMonthPurchasePlanA
            ,0 AS ThisMonthSalesQ
            ,0 AS ThisMonthSalesA
            ,0 AS ThisMonthCustSalesQ
            ,0 AS ThisMonthCustSalesA
            ,SUM((CASE WHEN ISNULL(DS.JuchuuNO,'') = '' THEN DM.JuchuuSuu
            	WHEN DH.SalesDate > @W_FiscalYYYYMMDD THEN DM.JuchuuSuu ELSE 0 END)) AS [ThisMonthSalesPlanQ]
            ,SUM((CASE WHEN ISNULL(DS.JuchuuNO,'') = '' THEN DM.CostGaku 
            	WHEN DH.SalesDate > @W_FiscalYYYYMMDD THEN DM.CostGaku ELSE 0 END)) AS [ThisMonthSalesPlanA]
            ,0 AS ThisMonthReturnsQ
            ,0 AS ThisMonthReturnsA
            ,0 AS ThisMonthReturnsPlanQ
            ,0 AS ThisMonthReturnsPlanA
            ,0 AS ThisMonthPlanAmount
            ,0 AS ThisMonthPlanQuantity
            ,0 AS ThisMonthAmount
            ,0 AS ThisMonthQuantity
            ,0 AS ThisMonthCost
            ,@Operator AS InsertOperator
            ,@SYSDATETIME AS InsertDateTime
            ,@Operator AS UpdateOperator
            ,@SYSDATETIME AS UpdateDateTime
        FROM D_Juchuu AS DJ
        LEFT OUTER JOIN D_JuchuuDetails AS DM
        ON DM.JuchuuNO = DJ.JuchuuNO
        AND DM.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_SalesDetails AS DS
        ON DS.JuchuuNO = DM.JuchuuNO
        AND DS.JuchuuRows = DM.JuchuuRows
        AND DS.DeleteDateTime IS NULL

        LEFT OUTER JOIN D_Sales AS DH
        ON DH.SalesNO = DS.SalesNO
        AND DH.DeleteDateTime IS NULL
            
        LEFT OUTER JOIN F_SKU(@W_FiscalYYYYMMDD) AS SK
        ON SK.AdminNO = DM.AdminNO
        AND SK.DeleteFlg = 0

        WHERE DJ.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DJ.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DJ.SalesPlanDate,111),1,7) = @W_FiscalYYYYMM
        AND DJ.DeleteDateTime IS NULL
        AND ISNULL(DM.VendorCD,ISNULL(SK.MainVendorCD,'')) <> ''
        
        AND NOT EXISTS(SELECT DM.AdminNO FROM D_MonthlyPurchase AS A
                       WHERE A.AdminNO = DM.AdminNO
                       AND A.StoreCD = DJ.StoreCD
                       AND A.VendorCD = ISNULL(DM.VendorCD,SK.MainVendorCD)
                       AND A.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))

        GROUP BY DJ.StoreCD, DM.AdminNO, DM.SKUCD, DM.JanCD, ISNULL(DM.VendorCD,SK.MainVendorCD)
        ;
        
        --Table転送仕様Ｅ
        --テーブル転送仕様Ｅ　返品予定
        UPDATE [D_MonthlyPurchase] SET
            [ThisMonthReturnsPlanQ] = DS.ThisMonthReturnsPlanQ
           ,[ThisMonthReturnsPlanA] = DS.ThisMonthReturnsPlanA
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.AdminNO, DS.SKUCD, DS.JanCD, DS.VendorCD
            ,MS.StoreCD AS StoreCD
            ,SUM(ISNULL(DS.ReturnPlanSu,0)-ISNULL(DS.ReturnSu,0)) AS ThisMonthReturnsPlanQ
            ,SUM((ISNULL(DS.ReturnPlanSu,0)-ISNULL(DS.ReturnSu,0))*ISNULL(ML.LastCost,0)) AS ThisMonthReturnsPlanA

            FROM D_Stock AS DS
            INNER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
            ON MS.SoukoCD = DS.SoukoCD
            AND MS.SoukoType = 8	--返品倉庫
            
            LEFT OUTER JOIN M_SKULastCost AS ML
            ON ML.AdminNO = DS.AdminNO
            AND ML.SoukoCD = DS.SoukoCD            

            WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.ExpectReturnDate,111),1,7) = @W_FiscalYYYYMM
            AND DS.DeleteDateTime IS NULL
            AND DS.ReturnPlanSu > DS.ReturnSu
            GROUP BY MS.StoreCD, DS.AdminNO, DS.SKUCD, DS.JanCD, DS.VendorCD
            ) AS DS
        WHERE D_MonthlyPurchase.AdminNO = DS.AdminNO
        AND D_MonthlyPurchase.StoreCD = DS.StoreCD
        AND D_MonthlyPurchase.VendorCD = DS.VendorCD
        AND D_MonthlyPurchase.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;
        
        INSERT INTO [D_MonthlyPurchase]
           ([VendorCD]
           ,[StoreCD]
           ,[YYYYMM]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[LastMonthQuantity]
           ,[LastMonthAmount]
           ,[LastMonthCost]
           ,[ThisMonthPurchaseQ]
           ,[ThisMonthPurchaseA]
           ,[ThisMonthCustPurchaseQ]
           ,[ThisMonthCustPurchaseA]
           ,[ThisMonthPurchasePlanQ]
           ,[ThisMonthPurchasePlanA]
           ,[ThisMonthSalesQ]
           ,[ThisMonthSalesA]
           ,[ThisMonthCustSalesQ]
           ,[ThisMonthCustSalesA]
           ,[ThisMonthSalesPlanQ]
           ,[ThisMonthSalesPlanA]
           ,[ThisMonthReturnsQ]
           ,[ThisMonthReturnsA]
           ,[ThisMonthReturnsPlanQ]
           ,[ThisMonthReturnsPlanA]
           ,[ThisMonthPlanAmount]
           ,[ThisMonthPlanQuantity]
           ,[ThisMonthAmount]
           ,[ThisMonthQuantity]
           ,[ThisMonthCost]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT DS.VendorCD
            ,MS.StoreCD
            ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
            ,DS.SKUCD
            ,DS.AdminNO
            ,DS.JanCD
            ,0 AS LastMonthQuantity
            ,0 AS LastMonthAmount
            ,0 AS LastMonthCost
            
            ,0 AS ThisMonthPurchaseQ
            ,0 AS ThisMonthPurchaseA
            ,0 AS ThisMonthCustPurchaseQ
            ,0 AS ThisMonthCustPurchaseA
            ,0 AS ThisMonthPurchasePlanQ
            ,0 AS ThisMonthPurchasePlanA
            ,0 AS ThisMonthSalesQ
            ,0 AS ThisMonthSalesA
            ,0 AS ThisMonthCustSalesQ
            ,0 AS ThisMonthCustSalesA
            ,0 AS ThisMonthSalesPlanQ
            ,0 AS ThisMonthSalesPlanA
            ,0 AS ThisMonthReturnsQ
            ,0 AS ThisMonthReturnsA
            ,SUM(ISNULL(DS.ReturnPlanSu,0)-ISNULL(DS.ReturnSu,0)) AS ThisMonthReturnsPlanQ
            ,SUM((ISNULL(DS.ReturnPlanSu,0)-ISNULL(DS.ReturnSu,0))*ISNULL(ML.LastCost,0)) AS ThisMonthReturnsPlanA
            ,0 AS ThisMonthPlanAmount
            ,0 AS ThisMonthPlanQuantity
            ,0 AS ThisMonthAmount
            ,0 AS ThisMonthQuantity
            ,0 AS ThisMonthCost
            ,@Operator AS InsertOperator
            ,@SYSDATETIME AS InsertDateTime
            ,@Operator AS UpdateOperator
            ,@SYSDATETIME AS UpdateDateTime
        FROM D_Stock AS DS

        INNER JOIN F_Souko(@W_FiscalYYYYMMDD) AS MS
        ON MS.SoukoCD = DS.SoukoCD
        AND MS.SoukoType = 8	--返品倉庫
        
        LEFT OUTER JOIN M_SKULastCost AS ML
        ON ML.AdminNO = DS.AdminNO
        AND ML.SoukoCD = DS.SoukoCD            

        WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.ExpectReturnDate,111),1,7) = @W_FiscalYYYYMM
        AND DS.DeleteDateTime IS NULL
        AND DS.ReturnPlanSu > DS.ReturnSu
        AND DS.VendorCD IS NOT NULL
        
        AND NOT EXISTS(SELECT DM.AdminNO FROM D_MonthlyPurchase AS DM
                       WHERE DM.AdminNO = DS.AdminNO
                       AND DM.StoreCD = MS.StoreCD
                       AND DM.VendorCD = DS.VendorCD
                       AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))

        GROUP BY MS.StoreCD, DS.AdminNO, DS.SKUCD, DS.JanCD, DS.VendorCD
        ;

        --テーブル転送仕様Ｆ　予定数計算
        UPDATE [D_MonthlyPurchase] SET
            [ThisMonthPlanQuantity] = LastMonthQuantity + ThisMonthPurchaseQ + ThisMonthPurchasePlanQ
                                    - ThisMonthSalesQ - ThisMonthSalesPlanQ - ThisMonthReturnsQ - ThisMonthReturnsPlanQ
           ,[ThisMonthPlanAmount]   = LastMonthAmount + ThisMonthPurchaseA + ThisMonthPurchasePlanA
                                    - ThisMonthSalesA - ThisMonthSalesPlanA - ThisMonthReturnsA - ThisMonthReturnsPlanA --- ThisMonthMarkDownA

           ,[ThisMonthQuantity] = ISNULL((SELECT SUM(DS.ThisMonthQuantity) 
                                          FROM D_MonthlyStock AS DS
                                          INNER JOIN (SELECT top 1  FS.SoukoCD, FS.StoreCD
                                                      FROM F_Souko(@W_FiscalYYYYMMDD) AS FS
                                                      WHERE FS.SoukoType IN (1,3)
                                                      ORDER BY FS.ChangeDate desc ) AS MS
                                          ON MS.SoukoCD = DS.SoukoCD
                                          AND MS.StoreCD = DS.StoreCD
                                          WHERE DS.AdminNO = D_MonthlyPurchase.AdminNO
                                          AND MS.StoreCD = D_MonthlyPurchase.StoreCD
                                          AND DS.YYYYMM = D_MonthlyPurchase.YYYYMM ),0)
           ,[ThisMonthAmount] = ISNULL((SELECT SUM(DS.ThisMonthAmount) 
                                        FROM D_MonthlyStock AS DS
                                        INNER JOIN (SELECT top 1  FS.SoukoCD, FS.StoreCD
                                                    FROM F_Souko(@W_FiscalYYYYMMDD) AS FS
                                                    WHERE FS.SoukoType IN (1,3)
                                                    ORDER BY FS.ChangeDate desc ) AS MS
                                        ON MS.SoukoCD = DS.SoukoCD
                                        AND MS.StoreCD = DS.StoreCD
                                        WHERE DS.AdminNO = D_MonthlyPurchase.AdminNO
                                        AND MS.StoreCD = D_MonthlyPurchase.StoreCD
                                        AND DS.YYYYMM = D_MonthlyPurchase.YYYYMM ),0)
           ,[ThisMonthCost] = ISNULL((SELECT SUM(DS.ThisMonthCost) 
                                      FROM D_MonthlyStock AS DS
                                      INNER JOIN (SELECT top 1  FS.SoukoCD, FS.StoreCD
                                                  FROM F_Souko(@W_FiscalYYYYMMDD) AS FS
                                                  WHERE FS.SoukoType IN (1,3)
                                                  ORDER BY FS.ChangeDate desc ) AS MS
                                      ON MS.SoukoCD = DS.SoukoCD
                                      AND MS.StoreCD = DS.StoreCD
                                      WHERE DS.AdminNO = D_MonthlyPurchase.AdminNO
                                      AND MS.StoreCD = D_MonthlyPurchase.StoreCD
                                      AND DS.YYYYMM = D_MonthlyPurchase.YYYYMM ),0)
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        
        WHERE D_MonthlyPurchase.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE D_MonthlyPurchase.StoreCD END)
        AND D_MonthlyPurchase.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        --@FiscalYYYYMM を一カ月進める（12月の場合、次の年の１月にする）
        SET @W_OldYYYYMM = @W_FiscalYYYYMM;
        SET @W_FiscalYYYYMM = SUBSTRING(CONVERT(varchar,DATEADD(month, 1, CONVERT(date, @W_FiscalYYYYMM + '/01')),111),1,7);
        SET @W_FiscalYYYYMMDD = DATEADD(day, -1 ,DATEADD(month, 1, CONVERT(date, @W_FiscalYYYYMM + '/01')));
	END
	
	SET @KeyItem = CONVERT(varchar,@FiscalYYYYMM) + ' ' + (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE 'ALL' END)
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'GetsujiShiireKeisanSyori',
        @PC,
        '仕入計算',
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


GO

