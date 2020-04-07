 BEGIN TRY 
 Drop Procedure dbo.[PRC_GetsujiSaimuKeisanSyori]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    月次債務計算処理
--       Program ID      GetsujiSaimuKeisanSyori
--       Create date:    2020.2.9
--    ======================================================================
CREATE PROCEDURE [dbo].[PRC_GetsujiSaimuKeisanSyori]
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

    --DataのDelete
    --D_MonthlyDebt（月別債務）をDeleteする
    IF ISNULL(@FiscalYYYYMM,'') <> ''
    BEGIN
        IF @Mode = 1
        BEGIN
            --Mode＝1の場合(＝ALL店舗）@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyDebt
            WHERE YYYYMM >= @FiscalYYYYMM
            ;
        END
        ELSE IF @Mode = 2
        BEGIN
            --Mode＝2の場合@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyDebt
            WHERE YYYYMM >= @FiscalYYYYMM
            AND StoreCD = @StoreCD
            ;
        END
    END

    DECLARE @W_FiscalYYYYMM varchar(7);
    SET @W_FiscalYYYYMM = SUBSTRING(CONVERT(varchar,@FiscalYYYYMM),1,4) + '/' + SUBSTRING(CONVERT(varchar,@FiscalYYYYMM),5,2);
    DECLARE @W_OldYYYYMM varchar(7);
    SET @W_OldYYYYMM = SUBSTRING(CONVERT(varchar,DATEADD(month, -1, CONVERT(date, @W_FiscalYYYYMM + '/01')),111),1,7);
    
    --DataのInsert
    --以下の順番に計算処理を行う
    --★Today のYYYYMM＜@FiscalYYYYMMになるまでの間 以下の処理を行う
    --データの行数分ループ処理を実行する
    WHILE @W_FiscalYYYYMM <= @SYSDATE_YYYYMM
    BEGIN
    --（Today のYYYYMM＜@FiscalYYYYMMになれば、処理を終える　☆へ）
        --Table転送仕様Ａ 
        --@FiscalYYYYMMの前月のデータから、今月のデータを作成する
		INSERT INTO [D_MonthlyDebt]
           ([YYYYMM]
           ,[StoreCD]
           ,[PayeeCD]
           ,[LastBalanceGaku]
           ,[HontaiGaku0]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[HontaiGaku]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[TaxGaku]
           ,[DebtGaku]
           ,[PayGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DM.StoreCD
               ,DM.PayeeCD
               ,DM.BalanceGaku AS LastBalanceGaku
               ,0 AS HontaiGaku0
               ,0 AS HontaiGaku8
               ,0 AS HontaiGaku10
               ,0 AS HontaiGaku
               ,0 AS Tax8
               ,0 AS Tax10
               ,0 AS Tax
               ,0 AS DebtGaku
               ,0 AS PayGaku
               ,0 AS OffsetGaku
               ,DM.BalanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_MonthlyDebt AS DM
        WHERE DM.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DM.StoreCD END)
        AND DM.YYYYMM = CONVERT(int,REPLACE(@W_OldYYYYMM,'/',''))
        AND DM.BalanceGaku <> 0
        ;

        --Table転送仕様Ｂ
        --今月(@FiscalYYYYMM)の仕入データから仕入額を更新する
        UPDATE [D_MonthlyDebt] SET
            [HontaiGaku0] = DS.HontaiGaku0
           ,[HontaiGaku8] = DS.HontaiGaku8
           ,[HontaiGaku10] = DS.HontaiGaku10
           ,[HontaiGaku] = DS.HontaiGaku0+DS.HontaiGaku8+DS.HontaiGaku10
           ,[TaxGaku8] = DS.Tax8
           ,[TaxGaku10] = DS.Tax10
           ,[TaxGaku] = DS.Tax8+DS.Tax10
           ,[DebtGaku] = DS.HontaiGaku0+DS.HontaiGaku8+DS.HontaiGaku10+DS.Tax8+DS.Tax10
           ,[BalanceGaku] = DS.HontaiGaku0+DS.HontaiGaku8+DS.HontaiGaku10+DS.Tax8+DS.Tax10
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DS.VendorCD
            ,SUM((CASE DM.TaxRitsu WHEN 0 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku0
            ,SUM((CASE DM.TaxRitsu WHEN 8 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku8
            ,SUM((CASE DM.TaxRitsu WHEN 10 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku10
            ,SUM((CASE DM.TaxRitsu WHEN 8 THEN DM.PurchaseTax ELSE 0 END)) AS Tax8
            ,SUM((CASE DM.TaxRitsu WHEN 10 THEN DM.PurchaseTax ELSE 0 END)) AS Tax10
            FROM D_PurchaseHistory AS DS
            INNER JOIN D_PurchaseDetailsHistory AS DM
            ON DM.HistoryNO = DS.HistoryNO
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.PurchaseDate,111),1,7) = @W_FiscalYYYYMM
            GROUP BY DS.StoreCD, DS.VendorCD
            ) AS DS
        WHERE D_MonthlyDebt.StoreCD = DS.StoreCD
        AND D_MonthlyDebt.PayeeCD = DS.VendorCD
        AND D_MonthlyDebt.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;
        
        INSERT INTO [D_MonthlyDebt]
           ([YYYYMM]
           ,[StoreCD]
           ,[PayeeCD]
           ,[LastBalanceGaku]
           ,[HontaiGaku0]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[HontaiGaku]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[TaxGaku]
           ,[DebtGaku]
           ,[PayGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DS.VendorCD AS PayeeCD
               ,0 AS LastBalanceGaku
               ,SUM((CASE DM.TaxRitsu WHEN 0 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku0
               ,SUM((CASE DM.TaxRitsu WHEN 8 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku8
               ,SUM((CASE DM.TaxRitsu WHEN 10 THEN DM.PurchaseGaku ELSE 0 END)) AS HontaiGaku10
               ,SUM(DM.PurchaseGaku) AS HontaiGaku
               ,SUM((CASE DM.TaxRitsu WHEN 8 THEN DM.PurchaseTax ELSE 0 END)) AS Tax8
               ,SUM((CASE DM.TaxRitsu WHEN 10 THEN DM.PurchaseTax ELSE 0 END)) AS Tax10
               ,SUM(DM.PurchaseTax) AS Tax
               ,SUM(DM.PurchaseGaku+DM.PurchaseTax) AS DebtGaku
               ,0 AS PayGaku
               ,0 AS OffsetGaku
               ,SUM(DM.PurchaseGaku+DM.PurchaseTax) AS BalanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_PurchaseHistory AS DS
        INNER JOIN D_PurchaseDetailsHistory AS DM
        ON DM.HistoryNO = DS.HistoryNO
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.PurchaseDate,111),1,7) = @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyDebt AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.PayeeCD = DS.VendorCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        GROUP BY DS.StoreCD, DS.VendorCD
        ;

        --Table転送仕様Ｃ
        --今月(@FiscalYYYYMM)の支払データから入金額を更新する
        UPDATE [D_MonthlyDebt] SET
            [PayGaku] = DS.PayGaku
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DP.PayeeCD
            ,SUM(DM.PayGaku) AS PayGaku
            FROM D_Pay AS DP
            INNER JOIN D_PayDetails AS DM
            ON DP.PayNO = DM.PayNO
            AND DM.DeleteDateTime IS NULL
            INNER JOIN D_PayPlan AS DS
            ON DS.PayPlanNO = DM.PayPlanNO
            AND DS.DeleteDateTime IS NULL
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DP.PayDate,111),1,7) = @W_FiscalYYYYMM
			AND DP.DeleteDateTime IS NULL
            --AND DS.CollectPayeeCD IS NOT NULL
            GROUP BY DS.StoreCD, DP.PayeeCD
            ) AS DS
        WHERE D_MonthlyDebt.StoreCD = DS.StoreCD
        AND D_MonthlyDebt.PayeeCD = DS.PayeeCD
        AND D_MonthlyDebt.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        
        INSERT INTO [D_MonthlyDebt]
           ([YYYYMM]
           ,[StoreCD]
           ,[PayeeCD]
           ,[LastBalanceGaku]
           ,[HontaiGaku0]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[HontaiGaku]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[TaxGaku]
           ,[DebtGaku]
           ,[PayGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DP.PayeeCD
               ,0 AS LastBalanceGaku
               ,0 AS HontaiGaku0
               ,0 AS HontaiGaku8
               ,0 AS HontaiGaku10
               ,0 AS HontaiGaku
               ,0 AS Tax8
               ,0 AS Tax10
               ,0 AS Tax
               ,0 AS DebtGaku
               ,SUM(DM.PayGaku) AS PayGaku
               ,0 AS OffsetGaku
               ,0 AS BalanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_Pay AS DP
        INNER JOIN D_PayDetails AS DM
        ON DP.PayNO = DM.PayNO
        AND DM.DeleteDateTime IS NULL
        INNER JOIN D_PayPlan AS DS
        ON DS.PayPlanNO = DM.PayPlanNO
        AND DS.DeleteDateTime IS NULL
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DP.PayDate,111),1,7) = @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyDebt AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.PayeeCD = DP.PayeeCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        AND DP.DeleteDateTime IS NULL
        --AND DS.CollectPayeeCD IS NOT NULL
        GROUP BY DS.StoreCD, DP.PayeeCD
        ;

        --Table転送仕様Ｄ
        --残額を更新する
        UPDATE [D_MonthlyDebt] SET
            [BalanceGaku] = LastBalanceGaku + HontaiGaku + TaxGaku - PayGaku
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
		WHERE StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE StoreCD END)
        AND YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
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
        'GetsujiSaimuKeisanSyori',
        @PC,
        '債務計算',
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END



