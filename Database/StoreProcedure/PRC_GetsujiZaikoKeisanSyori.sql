USE [CapitalSMS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_GetsujiZaikoKeisanSyori]    Script Date: 2020/11/03 19:54:57 ******/
DROP PROCEDURE [dbo].[PRC_GetsujiZaikoKeisanSyori]
GO

/****** Object:  StoredProcedure [dbo].[PRC_GetsujiZaikoKeisanSyori]    Script Date: 2020/11/03 19:54:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    月次在庫計算処理
--       Program ID      GetsujiZaikoKeisanSyori
--       Create date:    2020.2.9
--    ======================================================================
CREATE PROCEDURE [dbo].[PRC_GetsujiZaikoKeisanSyori]
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
    
    --D_Warehousing
    --D_Warehousing の金額情報Updateする
    --Table転送仕様Ｘ
    UPDATE [D_Warehousing] SET
        [UnitPrice] = ISNULL((CASE DW.ThisMonthCalculationQ WHEN 0 THEN
                             (CASE DW.ThisMonthCost WHEN 0 THEN (SELECT top 1 A.OrderPriceWithoutTax FROM M_SKU AS A 
                                                                 WHERE A.AdminNO = DW.AdminNO
                                                                 AND SUBSTRING(CONVERT(varchar,A.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                                                                 ORDER BY A.ChangeDate desc) 
                             ELSE DW.ThisMonthCost END)
                        ELSE ROUND(DW.ThisMonthCalculationA/DW.ThisMonthCalculationQ,0) END),0)

        ,[Amount] = ISNULL((CASE DW.ThisMonthCalculationQ WHEN 0 THEN
                             (CASE DW.ThisMonthCost WHEN 0 THEN (SELECT top 1 A.OrderPriceWithoutTax FROM M_SKU AS A 
                                                                 WHERE A.AdminNO = DW.AdminNO
                                                                 AND SUBSTRING(CONVERT(varchar,A.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                                                                 ORDER BY A.ChangeDate desc) 
                             ELSE DW.ThisMonthCost END)
                        ELSE ROUND(DW.ThisMonthCalculationA/DW.ThisMonthCalculationQ,0) END),0)
                        * DW.Quantity

        ,[UpdateOperator] = @Operator
        ,[UpdateDateTime] = @SYSDATETIME
    FROM (SELECT DW.WarehousingNO
        ,DM.ThisMonthCalculationQ
        ,DM.ThisMonthCost
        ,DM.AdminNO
        ,DM.ThisMonthCalculationA
        ,DW.Quantity
        FROM D_Warehousing AS DW
        LEFT OUTER JOIN D_MonthlyStock AS DM
        ON DM.AdminNO = DW.AdminNO
        AND DM.SoukoCD = DW.SoukoCD
        AND DM.YYYYMM = CONVERT(int,REPLACE(@W_OldYYYYMM,'/',''))
        LEFT OUTER JOIN (SELECT MS.SoukoCD, MAX(MS.ChangeDate) AS ChangeDate
                         FROM M_Souko AS MS
                         WHERE SUBSTRING(CONVERT(varchar,MS.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                         GROUP BY MS.SoukoCD
        ) AS MSS
        ON MSS.SoukoCD = DW.SoukoCD
        LEFT OUTER JOIN  M_Souko AS MS
        ON MS.SoukoCD = MSS.SoukoCD
        AND MS.ChangeDate = MSS.ChangeDate
        WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DW.WarehousingDate,111),1,7) = @W_FiscalYYYYMM
        AND DW.WarehousingKBN IN (11,12,13,14,31,32,16,26,17,18,41,42,43,44)
    ) AS DW
    WHERE DW.WarehousingNO = [D_Warehousing].WarehousingNO
    ;

    --DataのDelete
    --D_MonthlyStock（月別債権）をDeleteする
    IF ISNULL(@FiscalYYYYMM,'') <> ''
    BEGIN
        IF @Mode = 1
        BEGIN
            --Mode＝1の場合(＝ALL店舗）@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyStock
            WHERE YYYYMM >= @FiscalYYYYMM
            ;
        END
        ELSE IF @Mode = 2
        BEGIN
            --Mode＝2の場合@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyStock
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
        INSERT INTO [D_MonthlyStock]
               ([AdminNO]
              ,[SoukoCD]
              ,[YYYYMM]
              ,[JanCD]
              ,[SKUCD]
              ,[StoreCD]
              ,[LastMonthQuantity]
              ,[LastMonthInventry]
              ,[LastMonthCost]
              ,[LastMonthAmount]
              ,[ThisMonthArrivalQ]
              ,[ThisMonthPurchaseQ]
              ,[ThisMonthPurchaseA]
              ,[ThisMonthReturnsQ]
              ,[ThisMonthReturnsA]
              ,[ThisMonthShippingQ]
              ,[ThisMonthSalesQ]
              ,[ThisMonthSalesA]
              ,[ThisMonthMoveOutQ]
              ,[ThisMonthMoveOutA]
              ,[ThisMonthMoveInQ]
              ,[ThisMonthMoveInA]
              ,[ThisMonthAnyOutQ]
              ,[ThisMonthAnyOutA]
              ,[ThisMonthAnyInQ]
              ,[ThisMonthAnyInA]
              ,[ThisMonthAdjustQ]
              ,[ThisMonthAdjustA]
              ,[ThisMonthMarkDownA]
              ,[ThisMonthQuantity]
              ,[ThisMonthInventry]
              ,[ThisMonthCalculationQ]
              ,[ThisMonthCalculationA]
              ,[ThisMonthCost]
              ,[ThisMonthAmount]
              ,[InsertOperator]
              ,[InsertDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime])
        SELECT DM.AdminNO
              ,DM.SoukoCD
              ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
              ,DM.JanCD
              ,DM.SKUCD
              ,DM.StoreCD
              ,DM.ThisMonthQuantity AS LastMonthQuantity
              ,DM.ThisMonthInventry AS LastMonthInventry
              ,DM.ThisMonthCost AS LastMonthCost
              ,DM.ThisMonthAmount AS LastMonthAmount
              ,0 AS ThisMonthArrivalQ
              ,0 AS ThisMonthPurchaseQ
              ,0 AS ThisMonthPurchaseA
              ,0 AS ThisMonthReturnsQ
              ,0 AS ThisMonthReturnsA
              ,0 AS ThisMonthShippingQ
              ,0 AS ThisMonthSalesQ
              ,0 AS ThisMonthSalesA
              ,0 AS ThisMonthMoveOutQ
              ,0 AS ThisMonthMoveOutA
              ,0 AS ThisMonthMoveInQ
              ,0 AS ThisMonthMoveInA
              ,0 AS ThisMonthAnyOutQ
              ,0 AS ThisMonthAnyOutA
              ,0 AS ThisMonthAnyInQ
              ,0 AS ThisMonthAnyInA
              ,0 AS ThisMonthAdjustQ
              ,0 AS ThisMonthAdjustA
              ,0 AS ThisMonthMarkDownA
              ,DM.ThisMonthQuantity
              ,DM.ThisMonthInventry
              ,0 AS ThisMonthCalculationQ
              ,0 AS ThisMonthCalculationA
              ,DM.ThisMonthCost
              ,DM.ThisMonthAmount
              ,@Operator AS InsertOperator
              ,@SYSDATETIME AS InsertDateTime
              ,@Operator AS UpdateOperator
              ,@SYSDATETIME AS UpdateDateTime
        FROM D_MonthlyStock AS DM
        WHERE DM.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DM.StoreCD END)
        AND DM.YYYYMM = CONVERT(int,REPLACE(@W_OldYYYYMM,'/',''))
        AND (DM.ThisMonthQuantity <> 0 OR DM.ThisMonthAmount <> 0 OR DM.ThisMonthInventry <> 0)
        ;

        --Table転送仕様Ｂ
        --今月(@FiscalYYYYMM)のデータから在庫情報を更新する
        UPDATE [D_MonthlyStock] SET
            [ThisMonthArrivalQ] = DS.ThisMonthArrivalQ
           ,[ThisMonthPurchaseQ] = DS.ThisMonthPurchaseQ
           ,[ThisMonthPurchaseA] = DS.ThisMonthPurchaseA
           ,[ThisMonthReturnsQ] = DS.ThisMonthReturnsQ
           ,[ThisMonthReturnsA] = DS.ThisMonthReturnsA
           ,[ThisMonthShippingQ] = DS.ThisMonthShippingQ
           ,[ThisMonthSalesQ] = DS.ThisMonthSalesQ
           ,[ThisMonthSalesA] = 0
           ,[ThisMonthMoveOutQ] = DS.ThisMonthMoveOutQ
           ,[ThisMonthMoveOutA] = DS.ThisMonthMoveOutA
           ,[ThisMonthMoveInQ] = DS.ThisMonthMoveInQ
           ,[ThisMonthMoveInA] = DS.ThisMonthMoveInA
           ,[ThisMonthAnyOutQ] = DS.ThisMonthAnyOutQ
           ,[ThisMonthAnyOutA] = 0
           ,[ThisMonthAnyInQ] = DS.ThisMonthAnyInQ
           ,[ThisMonthAnyInA] = 0          
           ,[ThisMonthAdjustQ] = DS.ThisMonthAdjustQ
           ,[ThisMonthAdjustA] = DS.ThisMonthAdjustA
           ,[ThisMonthMarkDownA] = DS.ThisMonthMarkDownA

           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.AdminNO, DS.SoukoCD--, DS.WarehousingKBN
                    ,(SUM((CASE DS.WarehousingKBN WHEN 1  THEN DS.Quantity ELSE 0 END)) + SUM((CASE DS.WarehousingKBN WHEN 30 THEN CASE DS.Program WHEN 'ShiireNyuuryoku' THEN DS.Quantity ELSE 0 END ELSE 0 END))) AS ThisMonthArrivalQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 30 THEN DS.Quantity ELSE 0 END)) AS ThisMonthPurchaseQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 30 THEN DS.Amount   ELSE 0 END)) AS ThisMonthPurchaseA
                    ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Quantity ELSE 0 END)) AS ThisMonthReturnsQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Amount   ELSE 0 END)) AS ThisMonthReturnsA
                    ,SUM((CASE DS.WarehousingKBN WHEN 3  THEN DS.Quantity ELSE 0 END)) AS ThisMonthShippingQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 3  THEN DS.Quantity WHEN 23 THEN DS.Quantity 
												 WHEN 24 THEN DS.Quantity 
												 WHEN 25 THEN DS.Quantity ELSE 0 END)) AS ThisMonthSalesQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 11 THEN DS.Quantity WHEN 12 THEN DS.Quantity
												 WHEN 41 THEN DS.Quantity WHEN 42 THEN DS.Quantity
												 WHEN 16 THEN DS.Quantity 
												 ELSE 0 END)) AS ThisMonthMoveOutQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 11 THEN DS.Amount WHEN 12 THEN DS.Amount
												 WHEN 41 THEN DS.Amount WHEN 42 THEN DS.Amount
												 WHEN 16 THEN DS.Amount 
                                                 ELSE 0 END)) AS ThisMonthMoveOutA
                    ,SUM((CASE DS.WarehousingKBN WHEN 13 THEN DS.Quantity WHEN 14 THEN DS.Quantity 
                                                 WHEN 43 THEN DS.Quantity WHEN 44 THEN DS.Quantity
												 WHEN 26 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthMoveInQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 13 THEN DS.Amount WHEN 14 THEN DS.Amount 
                                                 WHEN 43 THEN DS.Amount WHEN 44 THEN DS.Amount
												 WHEN 26 THEN DS.Amount 
												 ELSE 0 END)) AS ThisMonthMoveInA
                    ,SUM((CASE DS.WarehousingKBN WHEN 31 THEN DS.Quantity WHEN 18 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAnyOutQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 32 THEN DS.Quantity WHEN 17 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAnyInQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 19 THEN DS.Quantity WHEN 20 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAdjustQ
                    ,SUM((CASE DS.WarehousingKBN WHEN 19 THEN DS.Amount WHEN 20 THEN DS.Amount 
                                                 ELSE 0 END)) AS ThisMonthAdjustA
                    ,SUM((CASE DS.WarehousingKBN WHEN 40 THEN DS.Amount ELSE 0 END)) AS ThisMonthMarkDownA
            FROM D_Warehousing AS DS
            LEFT OUTER JOIN (SELECT MS.SoukoCD, MAX(MS.ChangeDate) AS ChangeDate
                             FROM M_Souko AS MS
                             WHERE SUBSTRING(CONVERT(varchar,MS.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                             GROUP BY MS.SoukoCD
            ) AS MSS
            ON MSS.SoukoCD = DS.SoukoCD
            LEFT OUTER JOIN  M_Souko AS MS
            ON MS.SoukoCD = MSS.SoukoCD
            AND MS.ChangeDate = MSS.ChangeDate
            WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.WarehousingDate,111),1,7) = @W_FiscalYYYYMM
            GROUP BY DS.AdminNO, DS.SoukoCD--, DS.WarehousingKBN
            ) AS DS
        WHERE D_MonthlyStock.AdminNO = DS.AdminNO
        AND D_MonthlyStock.SoukoCD = DS.SoukoCD
        AND D_MonthlyStock.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;

        
        INSERT INTO [D_MonthlyStock]
           ([AdminNO]
           ,[SoukoCD]
           ,[YYYYMM]
           ,[JanCD]
           ,[SKUCD]
           ,[StoreCD]
           ,[LastMonthQuantity]
           ,[LastMonthInventry]
           ,[LastMonthCost]
           ,[LastMonthAmount]
           ,[ThisMonthArrivalQ]
           ,[ThisMonthPurchaseQ]
           ,[ThisMonthPurchaseA]
           ,[ThisMonthReturnsQ]
           ,[ThisMonthReturnsA]
           ,[ThisMonthShippingQ]
           ,[ThisMonthSalesQ]
           ,[ThisMonthSalesA]
           ,[ThisMonthMoveOutQ]
           ,[ThisMonthMoveOutA]
           ,[ThisMonthMoveInQ]
           ,[ThisMonthMoveInA]
           ,[ThisMonthAnyOutQ]
           ,[ThisMonthAnyOutA]
           ,[ThisMonthAnyInQ]
           ,[ThisMonthAnyInA]
           ,[ThisMonthAdjustQ]
           ,[ThisMonthAdjustA]
           ,[ThisMonthMarkDownA]
           ,[ThisMonthQuantity]
           ,[ThisMonthInventry]
           ,[ThisMonthCalculationQ]
           ,[ThisMonthCalculationA]
           ,[ThisMonthCost]
           ,[ThisMonthAmount]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT DS.AdminNO
            ,DS.SoukoCD
            ,CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
            ,MAX(DS.JanCD) AS JanCD
            ,MAX(DS.SKUCD) AS SKUCD
            ,MAX(MS.StoreCD) AS StoreCD
            ,0 AS LastMonthQuantity
            ,0 AS LastMonthInventry
            ,0 AS LastMonthCost
            ,0 AS LastMonthAmount
            ,(SUM((CASE DS.WarehousingKBN WHEN 1  THEN DS.Quantity ELSE 0 END)) + SUM((CASE DS.WarehousingKBN WHEN 30 THEN CASE DS.Program WHEN 'ShiireNyuuryoku' THEN DS.Quantity ELSE 0 END ELSE 0 END))) AS ThisMonthArrivalQ
            ,SUM((CASE DS.WarehousingKBN WHEN 30 THEN DS.Quantity ELSE 0 END)) AS ThisMonthPurchaseQ
            ,SUM((CASE DS.WarehousingKBN WHEN 30 THEN DS.Amount   ELSE 0 END)) AS ThisMonthPurchaseA
            ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Quantity ELSE 0 END)) AS ThisMonthReturnsQ
            ,SUM((CASE DS.WarehousingKBN WHEN 21 THEN DS.Amount   ELSE 0 END)) AS ThisMonthReturnsA
            ,SUM((CASE DS.WarehousingKBN WHEN 3  THEN DS.Quantity ELSE 0 END)) AS ThisMonthShippingQ
            ,SUM((CASE DS.WarehousingKBN WHEN 3  THEN DS.Quantity WHEN 23 THEN DS.Quantity 
												 WHEN 24 THEN DS.Quantity 
												 WHEN 25 THEN DS.Quantity ELSE 0 END)) AS ThisMonthSalesQ
			,0
            ,SUM((CASE DS.WarehousingKBN WHEN 13 THEN DS.Quantity WHEN 14 THEN DS.Quantity 
                                                 WHEN 43 THEN DS.Quantity WHEN 44 THEN DS.Quantity
												 WHEN 16 THEN DS.Quantity 
												 ELSE 0 END)) AS ThisMonthMoveOutQ
            ,SUM((CASE DS.WarehousingKBN WHEN 13 THEN DS.Amount WHEN 14 THEN DS.Amount 
                                                 WHEN 43 THEN DS.Amount WHEN 44 THEN DS.Amount
												 WHEN 16 THEN DS.Amount 
                                                 ELSE 0 END)) AS ThisMonthMoveOutA
            ,SUM((CASE DS.WarehousingKBN WHEN 11 THEN DS.Quantity WHEN 12 THEN DS.Quantity
												 WHEN 41 THEN DS.Quantity WHEN 42 THEN DS.Quantity
												 WHEN 26 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthMoveInQ
            ,SUM((CASE DS.WarehousingKBN WHEN 11 THEN DS.Amount WHEN 12 THEN DS.Amount
												 WHEN 41 THEN DS.Amount WHEN 42 THEN DS.Amount
												 WHEN 26 THEN DS.Amount 
												 ELSE 0 END)) AS ThisMonthMoveInA
            ,SUM((CASE DS.WarehousingKBN WHEN 31 THEN DS.Quantity WHEN 18 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAnyOutQ
			,0
            ,SUM((CASE DS.WarehousingKBN WHEN 32 THEN DS.Quantity WHEN 17 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAnyInQ
			,0
            ,SUM((CASE DS.WarehousingKBN WHEN 19 THEN DS.Quantity WHEN 20 THEN DS.Quantity 
                                                 ELSE 0 END)) AS ThisMonthAdjustQ
            ,SUM((CASE DS.WarehousingKBN WHEN 19 THEN DS.Amount WHEN 20 THEN DS.Amount 
                                                 ELSE 0 END)) AS ThisMonthAdjustA
            ,SUM((CASE DS.WarehousingKBN WHEN 40 THEN DS.Amount ELSE 0 END)) AS ThisMonthMarkDownA
            ,0 AS ThisMonthQuantity
            ,0 AS ThisMonthInventry
            ,0 AS ThisMonthCalculationQ
            ,0 AS ThisMonthCalculationA
            ,0 AS ThisMonthCost
            ,0 AS ThisMonthAmount
            ,@Operator AS InsertOperator
            ,@SYSDATETIME AS InsertDateTime
            ,@Operator AS UpdateOperator
            ,@SYSDATETIME AS UpdateDateTime
        FROM D_Warehousing AS DS
        LEFT OUTER JOIN (SELECT MS.SoukoCD, MAX(MS.ChangeDate) AS ChangeDate
                         FROM M_Souko AS MS
                         WHERE SUBSTRING(CONVERT(varchar,MS.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                         GROUP BY MS.SoukoCD
        ) AS MSS
        ON MSS.SoukoCD = DS.SoukoCD
        LEFT OUTER JOIN  M_Souko AS MS
        ON MS.SoukoCD = MSS.SoukoCD
        AND MS.ChangeDate = MSS.ChangeDate
        WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.WarehousingDate,111),1,7) = @W_FiscalYYYYMM
            
        AND NOT EXISTS(SELECT DM.AdminNO FROM D_MonthlyStock AS DM
                WHERE DM.AdminNO = DS.AdminNO
                AND DM.SoukoCD = DS.SoukoCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        GROUP BY DS.AdminNO, DS.SoukoCD--,DS.WarehousingKBN
        ;

        --Table転送仕様Ｃ
        --今今月(@FiscalYYYYMM)のデータから在庫情報を更新する
        UPDATE [D_MonthlyStock] SET
            [ThisMonthQuantity] = LastMonthQuantity + ThisMonthPurchaseQ 
                                - ThisMonthReturnsQ - ThisMonthSalesQ + ThisMonthMoveOutQ
                                + ThisMonthMoveInQ
                                + ThisMonthAnyOutQ
                                + ThisMonthAnyInQ + ThisMonthAdjustQ
           ,[ThisMonthInventry] = LastMonthInventry + ThisMonthArrivalQ
                                - ThisMonthReturnsQ - ThisMonthSalesQ + ThisMonthMoveOutQ
                                + ThisMonthMoveInQ
                                + ThisMonthAnyOutQ
                                + ThisMonthAnyInQ + ThisMonthAdjustQ
           ,[ThisMonthCalculationQ] = LastMonthQuantity + ThisMonthPurchaseQ
                                    - ThisMonthReturnsQ + ThisMonthMoveOutQ
                                    + ThisMonthMoveInQ + ThisMonthAdjustQ
           ,[ThisMonthCalculationA] = LastMonthAmount + ThisMonthPurchaseA
                                    - ThisMonthReturnsA + ThisMonthMoveOutA
                                    + ThisMonthMoveInA + ThisMonthAdjustA + ThisMonthMarkDownA
           ,[ThisMonthCost] = (CASE ThisMonthCalculationQ WHEN 0 THEN
                             ISNULL((CASE ThisMonthCost WHEN 0 THEN (SELECT top 1 A.OrderPriceWithoutTax FROM M_SKU AS A 
                                                                     WHERE A.AdminNO = D_MonthlyStock.AdminNO
                                                                     AND SUBSTRING(CONVERT(varchar,A.ChangeDate,111),1,7) <= @W_FiscalYYYYMM
                                                                     ORDER BY A.ChangeDate desc) 
                                                        ELSE ThisMonthCost END),0)
                             ELSE ROUND(ThisMonthCalculationA/ThisMonthCalculationQ,0) END)
                        
           --,[ThisMonthAmount] = ThisMonthQuantity * ThisMonthCost
           ,[ThisMonthAmount] = LastMonthAmount + ThisMonthPurchaseA
                              - ThisMonthReturnsA + ThisMonthMoveOutA
                              + ThisMonthMoveInA + ThisMonthAdjustA + ThisMonthMarkDownA
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME

        WHERE D_MonthlyStock.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE D_MonthlyStock.StoreCD END)
        AND D_MonthlyStock.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        --Table転送仕様Ｄ
        --金額を更新する
        UPDATE [D_MonthlyStock] SET
            [ThisMonthSalesA]  = ThisMonthSalesQ * ThisMonthCost
           ,[ThisMonthAnyOutA] = ThisMonthAnyOutQ * ThisMonthCost
           ,[ThisMonthAnyInA]  = ThisMonthAnyInQ * ThisMonthCost
           ,[UpdateOperator]   = @Operator
           ,[UpdateDateTime]   = @SYSDATETIME
        WHERE D_MonthlyStock.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE D_MonthlyStock.StoreCD END)
        AND D_MonthlyStock.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        --Table転送仕様Ｅ
        --原価金額を更新する
        UPDATE [M_SKULastCost] SET
            [LastCost]       = DS.ThisMonthCost
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM D_MonthlyStock AS DS
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND DS.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))    
        AND DS.AdminNO = M_SKULastCost.AdminNO
        AND DS.SoukoCD = M_SKULastCost.SoukoCD
        ;
        
        INSERT INTO [M_SKULastCost]
           ([AdminNO]
           ,[SoukoCD]
           ,[SKUCD]
           ,[LastCost]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT DS.AdminNO
               ,DS.SoukoCD
               ,DS.SKUCD
               ,DS.ThisMonthCost AS LastCost
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
        FROM D_MonthlyStock AS DS
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND DS.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))    
        AND NOT EXISTS(SELECT DM.AdminNO FROM M_SKULastCost AS DM
                       WHERE DM.AdminNO = DS.AdminNO
                       AND DM.SoukoCD = DS.SoukoCD)
        ;

        --@FiscalYYYYMM を一カ月進める（12月の場合、次の年の１月にする）
        SET @W_OldYYYYMM = @W_FiscalYYYYMM;
        SET @W_FiscalYYYYMM = SUBSTRING(CONVERT(varchar,DATEADD(month, 1, CONVERT(date, @W_FiscalYYYYMM + '/01')),111),1,7);
    END
    
    SET @KeyItem = CONVERT(varchar,@FiscalYYYYMM) + ' ' + (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE 'ALL' END)
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'GetsujiZaikoKeisanSyori',
        @PC,
        '在庫計算',
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


GO


