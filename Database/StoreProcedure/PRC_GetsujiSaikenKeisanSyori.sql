 BEGIN TRY 
 Drop Procedure dbo.[PRC_GetsujiSaikenKeisanSyori]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    月次債権計算処理
--       Program ID      GetsujiSaikenKeisanSyori
--       Create date:    2020.2.9
--    ======================================================================
CREATE PROCEDURE [dbo].[PRC_GetsujiSaikenKeisanSyori]
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
    --D_MonthlyClaims（月別債権）をDeleteする
    IF ISNULL(@FiscalYYYYMM,'') <> ''
    BEGIN
        IF @Mode = 1
        BEGIN
            --Mode＝1の場合(＝ALL店舗）@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyClaims
            WHERE YYYYMM >= @FiscalYYYYMM
            ;
        END
        ELSE IF @Mode = 2
        BEGIN
            --Mode＝2の場合@FiscalYYYYMM ≠ Nullであれば
            DELETE FROM D_MonthlyClaims
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
		INSERT INTO [D_MonthlyClaims]
           ([YYYYMM]
           ,[StoreCD]
           ,[CustomerCD]
           ,[LastBalanceGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesHontaiGaku]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesTax]
           ,[Claims]
           ,[CollectGaku]
           ,[PaymentConfirmGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[AdvanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DM.StoreCD
               ,DM.CustomerCD
               ,DM.BalanceGaku AS LastBalanceGaku
               ,0 AS SalesHontaiGaku0
               ,0 AS SalesHontaiGaku8
               ,0 AS SalesHontaiGaku10
               ,0 AS SalesHontaiGaku
               ,0 AS SalesTax8
               ,0 AS SalesTax10
               ,0 AS SalesTax
               ,0 AS Claims
               ,0 AS CollectGaku
               ,0 AS PaymentConfirmGaku
               ,0 AS OffsetGaku
               ,DM.BalanceGaku
               ,DM.AdvanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
		FROM D_MonthlyClaims AS DM
		WHERE DM.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DM.StoreCD END)
        AND DM.YYYYMM = CONVERT(int,REPLACE(@W_OldYYYYMM,'/',''))
        AND DM.BalanceGaku <> 0
        ;

        --Table転送仕様Ｂ
        --今月(@FiscalYYYYMM)の売上データから売上額を更新する
        UPDATE [D_MonthlyClaims] SET
            [SalesHontaiGaku0] = DS.SalesHontaiGaku0
           ,[SalesHontaiGaku8] = DS.SalesHontaiGaku8
           ,[SalesHontaiGaku10] = DS.SalesHontaiGaku10
           ,[SalesHontaiGaku] = DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10
           ,[SalesTax8] = DS.SalesTax8
           ,[SalesTax10] = DS.SalesTax10
           ,[SalesTax] = DS.SalesTax8+DS.SalesTax10
           ,[Claims] = DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10+DS.SalesTax8+DS.SalesTax10
           ,[BalanceGaku] = DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10+DS.SalesTax8+DS.SalesTax10
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DS.CustomerCD
            ,SUM(DS.SalesHontaiGaku0) AS SalesHontaiGaku0
            ,SUM(DS.SalesHontaiGaku8) AS SalesHontaiGaku8
            ,SUM(DS.SalesHontaiGaku10) AS SalesHontaiGaku10
            ,SUM(DS.SalesTax8) AS SalesTax8
            ,SUM(DS.SalesTax10) AS SalesTax10
            FROM D_SalesTran AS DS
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.SalesDate,111),1,7) = @W_FiscalYYYYMM
            GROUP BY DS.StoreCD, DS.CustomerCD
            ) AS DS
        WHERE D_MonthlyClaims.StoreCD = DS.StoreCD
        AND D_MonthlyClaims.CustomerCD = DS.CustomerCD
        AND D_MonthlyClaims.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))
        ;

        
        INSERT INTO [D_MonthlyClaims]
           ([YYYYMM]
           ,[StoreCD]
           ,[CustomerCD]
           ,[LastBalanceGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesHontaiGaku]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesTax]
           ,[Claims]
           ,[CollectGaku]
           ,[PaymentConfirmGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[AdvanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DS.CustomerCD
               ,0 AS LastBalanceGaku
               ,SUM(DS.SalesHontaiGaku0) AS SalesHontaiGaku0
               ,SUM(DS.SalesHontaiGaku8) AS SalesHontaiGaku8
               ,SUM(DS.SalesHontaiGaku10) AS SalesHontaiGaku10
               ,SUM(DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10) AS SalesHontaiGaku
               ,SUM(DS.SalesTax8) AS SalesTax8
               ,SUM(DS.SalesTax10) AS SalesTax10
               ,SUM(DS.SalesTax8+DS.SalesTax10) AS SalesTax
               ,SUM(DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10+DS.SalesTax8+DS.SalesTax10) AS Claims
               ,0 AS CollectGaku
               ,0 AS PaymentConfirmGaku
               ,0 AS OffsetGaku
               ,SUM(DS.SalesHontaiGaku0+DS.SalesHontaiGaku8+DS.SalesHontaiGaku10+DS.SalesTax8+DS.SalesTax10) AS BalanceGaku
               ,0 AS AdvanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_SalesTran AS DS
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.SalesDate,111),1,7) = @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyClaims AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.CustomerCD = DS.CustomerCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        GROUP BY DS.StoreCD, DS.CustomerCD
        ;

        --Table転送仕様Ｃ
        --今月(@FiscalYYYYMM)の入金データから入金額を更新する
        UPDATE [D_MonthlyClaims] SET
            [CollectGaku] = DS.ConfirmSource
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DS.CollectCustomerCD
            ,SUM(DS.ConfirmSource) AS ConfirmSource
            FROM D_Collect AS DS
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.CollectDate,111),1,7) = @W_FiscalYYYYMM
            AND DS.DeleteDateTime IS NULL
            AND DS.CollectCustomerCD IS NOT NULL
            GROUP BY DS.StoreCD, DS.CollectCustomerCD
            ) AS DS
        WHERE D_MonthlyClaims.StoreCD = DS.StoreCD
        AND D_MonthlyClaims.CustomerCD = DS.CollectCustomerCD
        AND D_MonthlyClaims.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        
        INSERT INTO [D_MonthlyClaims]
           ([YYYYMM]
           ,[StoreCD]
           ,[CustomerCD]
           ,[LastBalanceGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesHontaiGaku]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesTax]
           ,[Claims]
           ,[CollectGaku]
           ,[PaymentConfirmGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[AdvanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DS.CollectCustomerCD AS CustomerCD
               ,0 AS LastBalanceGaku
               ,0 AS SalesHontaiGaku0
               ,0 AS SalesHontaiGaku8
               ,0 AS SalesHontaiGaku10
               ,0 AS SalesHontaiGaku
               ,0 AS SalesTax8
               ,0 AS SalesTax10
               ,0 AS SalesTax
               ,0 AS Claims
               ,SUM(DS.ConfirmSource) AS CollectGaku
               ,0 AS PaymentConfirmGaku
               ,0 AS OffsetGaku
               ,0 AS BalanceGaku
               ,0 AS AdvanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_Collect AS DS
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.CollectDate,111),1,7) = @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyClaims AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.CustomerCD = DS.CollectCustomerCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        AND DS.DeleteDateTime IS NULL
        AND DS.CollectCustomerCD IS NOT NULL
        GROUP BY DS.StoreCD, DS.CollectCustomerCD
        ;

        --Table転送仕様Ｄ
        --今月(@FiscalYYYYMM)の入金データから入金消込額を更新する
        UPDATE [D_MonthlyClaims] SET
            [PaymentConfirmGaku] = DS.ConfirmAmount
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DS.CollectCustomerCD
            ,SUM(DP.ConfirmAmount) AS ConfirmAmount
            FROM D_PaymentConfirm As DP
            LEFT OUTER JOIN D_Collect AS DS
            ON DS.CollectNO = DP.CollectNO
            AND DS.DeleteDateTime IS NULL
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DP.CollectClearDate,111),1,7) = @W_FiscalYYYYMM
            AND DP.DeleteDateTime IS NULL
            AND DS.CollectCustomerCD IS NOT NULL
            GROUP BY DS.StoreCD, DS.CollectCustomerCD
            ) AS DS
        WHERE D_MonthlyClaims.StoreCD = DS.StoreCD
        AND D_MonthlyClaims.CustomerCD = DS.CollectCustomerCD
        AND D_MonthlyClaims.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        
        INSERT INTO [D_MonthlyClaims]
           ([YYYYMM]
           ,[StoreCD]
           ,[CustomerCD]
           ,[LastBalanceGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesHontaiGaku]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesTax]
           ,[Claims]
           ,[CollectGaku]
           ,[PaymentConfirmGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[AdvanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DS.CollectCustomerCD AS CustomerCD
               ,0 AS LastBalanceGaku
               ,0 AS SalesHontaiGaku0
               ,0 AS SalesHontaiGaku8
               ,0 AS SalesHontaiGaku10
               ,0 AS SalesHontaiGaku
               ,0 AS SalesTax8
               ,0 AS SalesTax10
               ,0 AS SalesTax
               ,0 AS Claims
               ,0 AS CollectGaku
               ,SUM(DP.ConfirmAmount) AS PaymentConfirmGaku
               ,0 AS OffsetGaku
               ,0 AS BalanceGaku
               ,0 AS AdvanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_PaymentConfirm As DP
        LEFT OUTER JOIN D_Collect AS DS
        ON DS.CollectNO = DP.CollectNO
        AND DS.DeleteDateTime IS NULL
        AND DS.CollectCustomerCD IS NOT NULL
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DP.CollectClearDate,111),1,7) = @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyClaims AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.CustomerCD = DS.CollectCustomerCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        AND DP.DeleteDateTime IS NULL
        GROUP BY DS.StoreCD, DS.CollectCustomerCD
        ;

        --Table転送仕様Ｅ
        --今月(@FiscalYYYYMM)の入金データから前受金残を更新する
        UPDATE [D_MonthlyClaims] SET
            [AdvanceGaku] = DS.AdvanceGaku
           ,[UpdateOperator] = @Operator
           ,[UpdateDateTime] = @SYSDATETIME
        FROM (SELECT DS.StoreCD, DS.CollectCustomerCD
            ,SUM(DS.ConfirmSource-ISNULL(DP.ConfirmAmount,0)) AS AdvanceGaku
            FROM D_Collect AS DS
            LEFT OUTER JOIN D_PaymentConfirm As DP
            ON DS.CollectNO = DP.CollectNO
            AND SUBSTRING(CONVERT(varchar,DP.CollectClearDate,111),1,7) <= @W_FiscalYYYYMM
            AND DP.DeleteDateTime IS NULL
            WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
            AND SUBSTRING(CONVERT(varchar,DS.CollectDate,111),1,7) <= @W_FiscalYYYYMM
            AND DS.DeleteDateTime IS NULL
            AND DS.CollectCustomerCD IS NOT NULL
            AND DS.AdvanceFLG = 1
            GROUP BY DS.StoreCD, DS.CollectCustomerCD
            ) AS DS
        WHERE D_MonthlyClaims.StoreCD = DS.StoreCD
        AND D_MonthlyClaims.CustomerCD = DS.CollectCustomerCD
        AND D_MonthlyClaims.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/',''))        
        ;

        
        INSERT INTO [D_MonthlyClaims]
           ([YYYYMM]
           ,[StoreCD]
           ,[CustomerCD]
           ,[LastBalanceGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesHontaiGaku]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesTax]
           ,[Claims]
           ,[CollectGaku]
           ,[PaymentConfirmGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[AdvanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')) AS YYYYMM
               ,DS.StoreCD
               ,DS.CollectCustomerCD AS CustomerCD
               ,0 AS LastBalanceGaku
               ,0 AS SalesHontaiGaku0
               ,0 AS SalesHontaiGaku8
               ,0 AS SalesHontaiGaku10
               ,0 AS SalesHontaiGaku
               ,0 AS SalesTax8
               ,0 AS SalesTax10
               ,0 AS SalesTax
               ,0 AS Claims
               ,0 AS CollectGaku
               ,0 AS PaymentConfirmGaku
               ,0 AS OffsetGaku
               ,0 AS BalanceGaku
               ,SUM(DS.ConfirmSource-ISNULL(DP.ConfirmAmount,0)) AS AdvanceGaku
               ,@Operator AS InsertOperator
               ,@SYSDATETIME AS InsertDateTime
               ,@Operator AS UpdateOperator
               ,@SYSDATETIME AS UpdateDateTime
               ,NULL AS DeleteOperator
               ,NULL AS DeleteDateTime
        FROM D_Collect AS DS
        LEFT OUTER JOIN D_PaymentConfirm As DP
        ON DS.CollectNO = DP.CollectNO
        AND SUBSTRING(CONVERT(varchar,DP.CollectClearDate,111),1,7) <= @W_FiscalYYYYMM
        AND DP.DeleteDateTime IS NULL
        WHERE DS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DS.StoreCD END)
        AND SUBSTRING(CONVERT(varchar,DS.CollectDate,111),1,7) <= @W_FiscalYYYYMM
        AND NOT EXISTS(SELECT DM.StoreCD FROM D_MonthlyClaims AS DM
                WHERE DM.StoreCD = DS.StoreCD
                AND DM.CustomerCD = DS.CollectCustomerCD
                AND DM.YYYYMM = CONVERT(int,REPLACE(@W_FiscalYYYYMM,'/','')))
        AND DS.DeleteDateTime IS NULL
        AND DS.CollectCustomerCD IS NOT NULL
        AND DS.AdvanceFLG = 1
        GROUP BY DS.StoreCD, DS.CollectCustomerCD
        ;

        --Table転送仕様Ｆ
        --残額を更新する
        UPDATE [D_MonthlyClaims] SET
            [BalanceGaku] = LastBalanceGaku + SalesHontaiGaku + SalesTax - PaymentConfirmGaku
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
        'GetsujiSaikenKeisanSyori',
        @PC,
        '債権計算',
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END



