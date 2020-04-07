 BEGIN TRY 
 Drop Procedure dbo.[D_PaymentConfirm_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_PaymentConfirm_SelectForPrint]    */
CREATE PROCEDURE [dbo].[D_PaymentConfirm_SelectForPrint](
    -- Add the parameters for the stored procedure here
    @DateFrom  varchar(7),		--年月
    @DateTo  varchar(7),		--年月
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @DFrom date;
    DECLARE @DTo date;
    
    IF ISNULL(@DateFrom,'') <> ''
    BEGIN
        SET @DFrom = CONVERT(date, @DateFrom + '/01');
    END
    IF ISNULL(@DateTo,'') <> ''
    BEGIN
        SET @DTo = DATEADD(day, -1, DATEADD(month,1, CONVERT(date, @DateTo + '/01')));
    END
    
    IF OBJECT_ID( N'[dbo]..[#temp_Seikyu]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#temp_Seikyu];
      END

    IF OBJECT_ID( N'[dbo]..[#temporary1]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#temporary1];
      END	

    IF OBJECT_ID( N'[dbo]..[#temp_Shiharai]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#temp_Shihaarai];
      END

    IF OBJECT_ID( N'[dbo]..[#temporary2]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#temporary2];
      END	
      
    --一時ワークテーブル「temp_Seikyu」①（入金データ）
    SELECT DC.StoreCD
        ,DC.JuchuuNO
        ,DC.SalesNO
        ,4 AS Status
        ,DP.CollectClearDate AS Hiduke
        ,DB.CollectAmount AS KinGaku
        ,DS.CustomerName
        ,(SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 207 AND M.[Key] = D.PaymentMethodCD) AS Kinsyu
    INTO #temp_Seikyu
    FROM D_PaymentConfirm AS DP
    INNER JOIN D_CollectBilling AS DB
    ON DB.ConfirmNO = DP.ConfirmNO
    AND DB.DeleteDateTime IS NULL
    INNER JOIN D_CollectPlan AS DC
    ON DC.CollectPlanNO = DB.CollectPlanNO
    AND DC.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DC.StoreCD END)
    AND DC.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Sales AS DS
    ON DS.SalesNO = DC.SalesNO
    AND DS.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Collect AS D
    ON D.CollectNO = DP.CollectNO
    AND D.DeleteDateTime IS NULL
    WHERE DP.DeleteDateTime IS NULL
    AND DP.CollectClearDate >= (CASE WHEN @DateFrom <> '' THEN @DFrom ELSE DP.CollectClearDate END)
    AND DP.CollectClearDate <= (CASE WHEN @DateTo <> '' THEN @DTo ELSE DP.CollectClearDate END)
	;

	INSERT INTO #temp_Seikyu
	--一時ワークテーブル「temp_Seikyu」②請求・売上データ
    SELECT DC.StoreCD
        ,DC.JuchuuNO
        ,DC.SalesNO
        ,3 AS Status
        ,ISNULL(DC.NextCollectPlanDate, DC.FirstCollectPlanDate) AS Hiduke
        ,DC.CollectPlanGaku AS KinGaku
        ,DS.CustomerName
        ,(SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 207 AND M.[Key] = DC.PaymentMethodCD) AS Kinsyu
    FROM D_CollectPlan AS DC
    LEFT OUTER JOIN D_Sales AS DS
    ON DS.SalesNO = DC.SalesNO
    AND DS.DeleteDateTime IS NULL
    WHERE DC.DeleteDateTime IS NULL
    AND DC.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DC.StoreCD END)
    AND ISNULL(DC.NextCollectPlanDate, DC.FirstCollectPlanDate) >= (CASE WHEN @DateFrom <> '' THEN @DFrom ELSE ISNULL(DC.NextCollectPlanDate, DC.FirstCollectPlanDate) END)
    AND ISNULL(DC.NextCollectPlanDate, DC.FirstCollectPlanDate) <= (CASE WHEN @DateTo <> '' THEN @DTo ELSE ISNULL(DC.NextCollectPlanDate, DC.FirstCollectPlanDate) END)
    ;
    
	-- インデックスindex1作成
	CREATE CLUSTERED INDEX index_Seikyu on [#temp_Seikyu] (StoreCD,Hiduke);
    
    --一時ワークテーブル「temporary1」①受注データ
    SELECT DJ.StoreCD
        ,DJ.JuchuuNO
--        ,'' AS SalesNO
        ,1 AS Status         
        ,ISNULL(DJ.FirstPaypentPlanDate, DATEADD(DAY,-1,DATEADD(MONTH,2,DATEADD(DAY,1-DATEPART(DAY,DJ.JuchuuDate),DJ.JuchuuDate)))) AS Hiduke	--JuchuuDateの翌月末日
        ,DJ.InvoiceGaku AS KinGaku
        ,DJ.CustomerName
        ,(SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 207 AND M.[Key] = DJ.PaymentMethodCD) AS Kinsyu
        ,ROW_NUMBER() OVER(ORDER BY DJ.StoreCD, ISNULL(DJ.FirstPaypentPlanDate,DJ.JuchuuDate), DJ.JuchuuNO) AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
    INTO #temporary1
    FROM D_Juchuu AS DJ
    WHERE DJ.DeleteDateTime IS NULL
    AND DJ.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DJ.StoreCD END)
    AND DJ.CancelDate IS NULL
    AND NOT EXISTS(SELECT 1 FROM #temp_Seikyu AS tS
                WHERE tS.JuchuuNO = DJ.JuchuuNO
                )
    AND ISNULL(DJ.FirstPaypentPlanDate, DATEADD(DAY,-1,DATEADD(MONTH,2,DATEADD(DAY,1-DATEPART(DAY,DJ.JuchuuDate),DJ.JuchuuDate)))) >= @DFrom
    AND ISNULL(DJ.FirstPaypentPlanDate, DATEADD(DAY,-1,DATEADD(MONTH,2,DATEADD(DAY,1-DATEPART(DAY,DJ.JuchuuDate),DJ.JuchuuDate)))) <= @DTo 
    ;
	
	INSERT INTO #temporary1
	--一時ワークテーブル「temporary1」②temp_Seikyu(入金データ)
    SELECT tS.StoreCD
--        ,'' AS JuchuuNO
        ,tS.SalesNO
        ,4 AS Status
        ,tS.Hiduke
        ,SUM(tS.KinGaku) AS KinGaku
        ,tS.CustomerName
        ,tS.Kinsyu
        ,1 AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
    FROM #temp_Seikyu AS tS
   WHERE tS.Status = 4
	GROUP BY tS.StoreCD, tS.SalesNO, tS.Hiduke, tS.CustomerName, tS.Kinsyu
	;
	
	INSERT INTO #temporary1
	--一時ワークテーブル「temporary1」③temp_Seikyu(請求・売上データ)
    SELECT tS.StoreCD
--        ,'' AS JuchuuNO
    	,tS.SalesNO
        ,3 AS Status
        ,MAX(tS.Hiduke) AS Hiduke
        ,SUM(CASE ts.Status WHEN 3 THEN tS.KinGaku
        	ELSE (-1)*tS.KinGaku END) AS KinGaku
        ,MAX(tS.CustomerName) AS CustomerName
        ,MAX(tS.Kinsyu) AS Kinsyu
        ,ROW_NUMBER() OVER(ORDER BY tS.StoreCD, tS.SalesNO) AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
    FROM #temp_Seikyu AS tS
   WHERE tS.Status IN (3,4)
    GROUP BY tS.StoreCD, tS.SalesNO
    HAVING SUM(CASE ts.Status WHEN 3 THEN tS.KinGaku
            ELSE (-1)*tS.KinGaku END) > 0
    ;
    
    CREATE CLUSTERED INDEX index_temporary1 on [#temporary1] (StoreCD,Hiduke,RowNO);
    
    --一時ワークテーブル「temporary1」④temporary1  Update  (店舗、日付毎に行番号を採番)
    DECLARE CUR1 CURSOR FOR
        SELECT T1.StoreCD, T1.Hiduke, T1.Status, T1.RowNO
            ,ROW_NUMBER() OVER(PARTITION BY T1.StoreCD, T1.Hiduke ORDER BY T1.JuchuuNO) AS DetailNO
        FROM #temporary1 AS T1
        ;
    
    DECLARE @wStoreCD varchar(4);
    DECLARE @Hiduke date;
    DECLARE @Status tinyint;
    DECLARE @RowNO int;
    DECLARE @DetailNO int;
    DECLARE @Kingaku money;
    DECLARE @W_SUM money;
    
    --カーソルオープン
    OPEN CUR1;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR1
    INTO @wStoreCD,@Hiduke,@Status,@RowNO,@DetailNO;

    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        -- ========= ループ内の実際の処理 ここから===
        UPDATE #temporary1 SET
          GyoNO = @DetailNO
        WHERE StoreCD = @wStoreCD
        AND Hiduke = @Hiduke
        AND RowNO = @RowNO
        ;
        
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR1
    INTO @wStoreCD,@Hiduke,@Status,@RowNO,@DetailNO;

    END

    --カーソルを閉じる
    CLOSE CUR1;
    DEALLOCATE CUR1;
    /*
    UPDATE #temporary1 SET
          GyoNO = (SELECT ROW_NUMBER() OVER(PARTITION BY T1.StoreCD, T1.Hiduke ORDER BY T1.JuchuuNO)
                    FROM #temporary1 AS T1 
                    WHERE #temporary1.StoreCD = T1.StoreCD
                    AND #temporary1.Hiduke = T1.Hiduke
                  --  AND #temporary1.JuchuuNO = T1.JuchuuNO
                  --  AND #temporary1.Status = T1.Status
                    )
    ;
    */
    --一時ワークテーブル「temp_Shiharai①支払データ
    SELECT PP.StoreCD
        ,'' AS OrderNO
        ,PP.Number AS PurchaseNO
        ,4 AS Status
        ,DP.PayDate AS Hiduke
        ,DPM.PayGaku AS KinGaku
        ,(SELECT top 1 M.VendorName FROM M_Vendor AS M 
        	WHERE M.VendorCD = PP.PayeeCD 
        	AND M.DeleteFlg = 0
        	AND M.ChangeDate <= DP.PayDate
        	ORDER BY M.ChangeDate desc) AS VendorName
        ,NULL AS Kinsyu
    INTO #temp_Shiharai
    FROM D_Pay AS DP
    INNER JOIN D_PayDetails AS DPM
    ON DPM.PayNO = DP.PayNO
    AND DPM.DeleteDateTime IS NULL
    INNER JOIN D_PayPlan AS PP
    ON PP.PayPlanNO = DPM.PayPlanNO
    AND PP.DeleteDateTime IS NULL
    AND PP.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE PP.StoreCD END)
    WHERE DP.DeleteDateTime IS NULL
    AND DP.PayDate >= (CASE WHEN @DateFrom <> '' THEN @DFrom ELSE DP.PayDate END)
    AND DP.PayDate <= (CASE WHEN @DateTo <> '' THEN @DTo ELSE DP.PayDate END)
	;
	
	--一時ワークテーブル「temp_Shiharai②仕入・支払予定データ
    INSERT INTO #temp_Shiharai
    SELECT PP.StoreCD
        ,'' AS OrderNO
        ,PP.Number AS PurchaseNO
        ,3 AS Status
        ,PP.PayPlanDate AS Hiduke
        ,PP.PayPlanGaku AS KinGaku
        ,(SELECT top 1 M.VendorName FROM M_Vendor AS M 
            WHERE M.VendorCD = PP.PayeeCD 
            AND M.DeleteFlg = 0
            AND M.ChangeDate <= PP.PayPlanDate
            ORDER BY M.ChangeDate desc) AS VendorName
        ,NULL AS Kinsyu
    FROM D_PayPlan AS PP
    WHERE PP.DeleteDateTime IS NULL
    AND PP.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE PP.StoreCD END)
    AND PP.PayPlanDate >= (CASE WHEN @DateFrom <> '' THEN @DFrom ELSE PP.PayPlanDate END)
    AND PP.PayPlanDate <= (CASE WHEN @DateTo <> '' THEN @DTo ELSE PP.PayPlanDate END)
    ;
    
    --一時ワークテーブル「temporary2」①発注データ
    SELECT W.StoreCD
        ,W.OrderNO
--        ,'' AS PurchaseNO
        ,1 AS Status
        ,W.Hiduke	--DM.ArrivePlanDateの翌月末 
        ,W.KinGaku
        ,(SELECT top 1 M.VendorName FROM M_Vendor AS M 
            WHERE M.VendorCD = W.OrderCD 
            AND M.DeleteFlg = 0
            AND M.ChangeDate <= W.Hiduke
            ORDER BY M.ChangeDate desc) AS VendorName
        ,NULL AS Kinsyu
        ,ROW_NUMBER() OVER(PARTITION BY W.StoreCD ORDER BY W.StoreCD, W.Hiduke, W.OrderNO) AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
        INTO #temporary2
        FROM (
        SELECT DO.StoreCD
            ,DO.OrderNO
            ,DO.OrderCD
            ,DATEADD(DAY,-1,DATEADD(MONTH,2,DATEADD(DAY,1-DATEPART(DAY,DM.ArrivePlanDate),DM.ArrivePlanDate))) AS Hiduke    --DM.ArrivePlanDateの翌月末 
            ,SUM(DM.OrderGaku) AS KinGaku
        FROM D_Order AS DO
        INNER JOIN D_OrderDetails AS DM
        ON DM.OrderNO = DO.OrderNO
        AND DM.DeleteDateTime IS NULL
        WHERE DO.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DO.StoreCD END)
        AND DO.DeleteDateTime IS NULL
        AND DATEADD(DAY,-1,DATEADD(MONTH,2,DATEADD(DAY,1-DATEPART(DAY,DM.ArrivePlanDate),DM.ArrivePlanDate))) >= @DFrom
        AND NOT EXISTS(SELECT 1 FROM #temp_Shiharai AS tS
                    INNER JOIN D_PurchaseDetails AS DP
                    ON DP.PurchaseNO = tS.PurchaseNO
                    WHERE DM.OrderNO = DP.OrderNO
                    AND DP.DeleteDateTime IS NULL)
        GROUP BY DO.StoreCD, DM.ArrivePlanDate, DO.OrderNO, DO.OrderCD
        )AS W
    ;
	
	--一時ワークテーブル「temporary2」②temp_Shiharai(支払データ)
    INSERT INTO #temporary2
    SELECT tS.StoreCD
--        ,'' AS OrderNO
        ,tS.PurchaseNO
        ,4 AS Status
        ,tS.Hiduke
        ,SUM(tS.KinGaku) AS KinGaku
        ,tS.VendorName
        ,tS.Kinsyu
        ,ROW_NUMBER() OVER(ORDER BY tS.StoreCD, tS.Hiduke, tS.PurchaseNO) AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
    FROM #temp_Shiharai AS tS
    WHERE tS.Status = 4
    GROUP BY tS.StoreCD, tS.PurchaseNO, tS.Hiduke, tS.VendorName, tS.Kinsyu
    ;
	
	--一時ワークテーブル「temporary2」③temp_Shiharai(仕入・支払予定データ)
    INSERT INTO #temporary2
    SELECT tS.StoreCD
        --,'' AS OrderNO
        ,ISNULL(tS.PurchaseNO,'') AS PurchaseNO
        ,3 AS Status
        ,MAX(tS.Hiduke) AS Hiduke
        ,SUM(CASE ts.Status WHEN 3 THEN tS.KinGaku
        	ELSE (-1)*tS.KinGaku END) AS KinGaku
        ,MAX(tS.VendorName) AS VendorName
        ,MAX(tS.Kinsyu) AS Kinsyu
        ,ROW_NUMBER() OVER(ORDER BY tS.StoreCD, ISNULL(tS.PurchaseNO,'')) AS RowNO
        ,0 AS GyoNO
        ,0 AS Kei
    FROM #temp_Shiharai AS tS
    WHERE tS.Status IN (3,4)
    GROUP BY tS.StoreCD, ISNULL(tS.PurchaseNO,'') 
	HAVING SUM(CASE ts.Status WHEN 3 THEN tS.KinGaku
        	ELSE (-1)*tS.KinGaku END) > 0
    ;

    CREATE CLUSTERED INDEX index_temporary2 on [#temporary2] (StoreCD,Hiduke,RowNO);

    --一時ワークテーブル「temporary2」④Update  (店舗、日付毎に行番号を採番)
    DECLARE CUR2 CURSOR FOR
        SELECT T2.StoreCD, T2.Hiduke, T2.Status, T2.RowNO
            ,ROW_NUMBER() OVER(PARTITION BY T2.StoreCD, T2.Hiduke ORDER BY T2.OrderNO) AS DetailNO
        FROM #temporary2 AS T2
        ;
    
    --カーソルオープン
    OPEN CUR2;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR2
    INTO @wStoreCD,@Hiduke,@Status,@RowNO,@DetailNO;

    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        -- ========= ループ内の実際の処理 ここから===
        UPDATE #temporary2 SET
          GyoNO = @DetailNO
        WHERE StoreCD = @wStoreCD
        AND Hiduke = @Hiduke
        AND RowNO = @RowNO
        ;
        
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR2
    INTO @wStoreCD,@Hiduke,@Status,@RowNO,@DetailNO;

    END

    --カーソルを閉じる
    CLOSE CUR2;
    DEALLOCATE CUR2;
    
/*    UPDATE #temporary2 SET
          GyoNO = (SELECT ROW_NUMBER() OVER(PARTITION BY T2.StoreCD, T2.Hiduke ORDER BY T2.OrderNO)
                    FROM #temporary2 AS T2 
                    WHERE #temporary2.StoreCD = T2.StoreCD
                    AND #temporary2.Hiduke = T2.Hiduke
                    AND #temporary2.OrderNO = T2.OrderNO
                    AND #temporary2.Status = T2.Status
                    AND #temporary2.GyoNO = T2.GyoNO
                    )
    ;
*/
    SELECT A.StoreCD
        ,(SELECT top 1 M.StoreName FROM M_Store AS M WHERE M.StoreCD = A.StoreCD
            AND M.DeleteFlg = 0 AND M.ChangeDate <= @DTo
            ORDER BY M.ChangeDate DESC) As StoreName
        ,@DateFrom + '～' + @DateTo AS Ym	--9999/99～9999/99
        ,A.Hiduke
        ,A.GyoNO
        ,T1.Kinsyu
        ,T1.JuchuuNO
        ,T1.CustomerName
        ,(CASE T1.Status WHEN 4 THEN '*' ELSE '' END) AS Status1
        ,T1.KinGaku AS KinGaku1
        
        ,T2.OrderNO
        ,T2.VendorName
        ,(CASE T2.Status WHEN 4 THEN '*' ELSE '' END) AS Status2
        ,T2.KinGaku AS KinGaku2
        ,0 AS Kei
        ,ROW_NUMBER() OVER(PARTITION BY A.StoreCD ORDER BY A.Hiduke, A.GyoNO) AS RowNO
    INTO #TMain
    FROM(
        SELECT T1.StoreCD
                ,T1.Hiduke
                ,T1.GyoNO
        FROM #temporary1 AS T1
        UNION 
        SELECT T2.StoreCD
                ,T2.Hiduke
                ,T2.GyoNO
        FROM #temporary2 AS T2
    ) AS A
    LEFT OUTER JOIN #temporary1 AS T1
    ON T1.StoreCD = A.StoreCD
    AND T1.Hiduke = A.Hiduke
    AND T1.GyoNO = A.GyoNO
    LEFT OUTER JOIN #temporary2 AS T2
    ON T2.StoreCD = A.StoreCD
    AND T2.Hiduke = A.Hiduke
    AND T2.GyoNO = A.GyoNO
    
    ORDER BY A.StoreCD, A.Hiduke, A.GyoNO
	;

    CREATE CLUSTERED INDEX index_main on [#TMain] (StoreCD,Hiduke);

    DECLARE CUR_MAIN CURSOR FOR
        SELECT TM.StoreCD, TM.RowNO, ISNULL(TM.KinGaku1,0)-ISNULL(TM.KinGaku2,0) AS KinGaku2
        FROM #TMain AS TM
        ORDER BY TM.StoreCD, TM.Hiduke, TM.GyoNO
        ;
            
    --カーソルオープン
    OPEN CUR_MAIN;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_MAIN
    INTO @wStoreCD,@RowNO,@Kingaku;
    
    DECLARE @OldStoreCD  varchar(4);
    SET @OldStoreCD = '';
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @wStoreCD <> @OldStoreCD
        BEGIN
            SET @W_SUM = 0;
            SET @OldStoreCD = @wStoreCD;
        END
        
        SET @W_SUM = @W_SUM + @Kingaku;
        
        -- ========= ループ内の実際の処理 ここから===
        UPDATE #TMain SET
          Kei =  @W_SUM
        WHERE StoreCD = @wStoreCD
        AND RowNO = @RowNO
        ;
        
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_MAIN
    	INTO @wStoreCD,@RowNO,@Kingaku;

    END

    --カーソルを閉じる
    CLOSE CUR_MAIN;
    DEALLOCATE CUR_MAIN;
    
    SELECT * FROM #TMain AS A
    ORDER BY A.StoreCD, A.Hiduke, A.GyoNO
    ;
END


