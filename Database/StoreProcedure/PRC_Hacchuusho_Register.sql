IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_Register')
begin
    DROP PROCEDURE PRC_Hacchuusho_Register
end
GO

CREATE PROCEDURE PRC_Hacchuusho_Register(
     @p_Operator        varchar(10)
    ,@p_StoreCD         varchar(4)
    ,@p_OrderCD         varchar(13)
)AS
BEGIN

    DECLARE @SYSDATETIME datetime
    DECLARE @SYSDATE date
    SET @SYSDATETIME = SYSDATETIME()
    SET @SYSDATE = CONVERT(datetime, @SYSDATETIME)

    DECLARE @OrderCD varchar(13)
    DECLARE @CancelOrderNO varchar(11)
    DECLARE @CancelOrderProcessNO varchar(11)

	--対象データ用一時テーブル
	CREATE TABLE #tmpTargetKey
    (
	     JuchuuNO	varchar(11) COLLATE database_default NOT NULL
	    ,JuchuuRows	int NOT NULL
	    ,OrderSEQ	int NOT NULL
	    ,OrderNO	varchar(11) COLLATE database_default NULL
	    ,OrderRows	int
	    ,OrderCD	varchar(13) COLLATE database_default NULL
	)
    ALTER TABLE #tmpTargetKey ADD PRIMARY KEY CLUSTERED 	
    (	
	 JuchuuNO
	,JuchuuRows
	,OrderSEQ
    )	

	--対象データを一時テーブルに格納する
	INSERT INTO #tmpTargetKey
	SELECT    
	     DLOR.JuchuuNO
	    ,DLOR.JuchuuRows
	    ,DLOR.OrderSEQ
	    ,DLOR.OrderNO
	    ,DLOR.OrderRows
	    ,DLOR.OrderCD
    FROM D_LastOrder DLOR
	INNER JOIN D_Order DODH
    ON DODH.OrderNO = DLOR.OrderNO
    WHERE DODH.StoreCD = @p_StoreCD
	AND DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)

    --カーソル定義（仕入先ごと）
    DECLARE curOrderCD CURSOR FOR
	SELECT OrderCD
    FROM #tmpTargetKey
	GROUP BY OrderCD

    --カーソルオープン
    OPEN curOrderCD;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM curOrderCD
    INTO @OrderCD
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN

	    --伝票番号採番（発注処理番号）        
		--EXEC    Fnc_GetNumber    
		-- 20,              --in伝票種別 11    
		-- @SYSDATE,        --in基準日    
        -- @p_StoreCD,      --in店舗CD    
        -- @p_Operator,    
        -- @CancelOrderProcessNO OUTPUT        

        --伝票番号採番（発注番号）            
        EXEC Fnc_GetNumber    
            11,             --in伝票種別 11    
            @SYSDATE,       --in基準日    
            @p_StoreCD,     --in店舗CD    
            @p_Operator,
            @CancelOrderNO OUTPUT

		--発注明細更新    
        INSERT INTO D_OrderDetails(
             OrderNO
            ,OrderRows
            ,DisplayRows
            ,JuchuuNO
            ,JuchuuRows
            ,SKUCD
            ,AdminNO
            ,JanCD
            --,MakerItem
            ,ItemName
            ,ColorName
            ,SizeName
            ,Remarks
            ,OrderSu
            ,TaniCD
            ,PriceOutTax
            ,Rate
            ,OrderUnitPrice
            ,OrderHontaiGaku
            ,OrderTax
            ,OrderTaxRitsu
            ,OrderGaku
            ,SoukoCD
            ,DirectFLG
            ,NotNetFLG
            ,EDIFLG
            ,DesiredDeliveryDate
            ,ArrivePlanDate
            ,TotalArrivalSu
            ,CommentOutStore
            ,CommentInStore
            ,FirstOrderNO
            ,FirstOrderRows
            ,CancelOrderNO
            ,AnswerFLG
            ,EDIOutputDatetime
            ,InsertOperator
            ,InsertDateTime
            ,UpdateOperator
            ,UpdateDateTime
            ,DeleteOperator
            ,DeleteDateTime
				)        
		SELECT  
             OrderNO                = @CancelOrderNO
            ,OrderRows              = ROW_NUMBER() OVER(PARTITION BY DLOR.OrderCD ORDER BY DLOR.OrderNO, DLOR.OrderRows)        
            ,DisplayRows            = ROW_NUMBER() OVER(PARTITION BY DLOR.OrderCD ORDER BY DLOR.OrderNO, DLOR.OrderRows)
            ,JuchuuNO               = DLOR.JuchuuNO
            ,JuchuuRows             = DLOR.JuchuuRows
            ,SKUCD                  = DODD.SKUCD
            ,AdminNO                = DODD.AdminNO
            ,JanCD                  = DODD.JanCD
            --,MakerItem
            ,ItemName               = DODD.ItemName
            ,ColorName              = DODD.ColorName
            ,SizeName               = DODD.SizeName
            ,Remarks                = DODD.Remarks
            ,OrderSu                = DODD.OrderSu
            ,TaniCD                 = DODD.TaniCD
            ,PriceOutTax            = DODD.PriceOutTax
            ,Rate                   = DODD.Rate
            ,OrderUnitPrice         = DODD.OrderUnitPrice
            ,OrderHontaiGaku        = DODD.OrderHontaiGaku
            ,OrderTax               = DODD.OrderTax
            ,OrderTaxRitsu          = DODD.OrderTaxRitsu
            ,OrderGaku              = DODD.OrderGaku
            ,SoukoCD                = DODD.SoukoCD
            ,DirectFLG              = 0
            ,NotNetFLG              = 0
            ,EDIFLG                 = 0
            ,DesiredDeliveryDate    = DODD.DesiredDeliveryDate        
            ,ArrivePlanDate         = null
            ,TotalArrivalSu         = 0
            ,CommentOutStore        = null
            ,CommentInStore         = null
            ,FirstOrderNO           = DLOR.OrderNO
            ,FirstOrderRows         = DLOR.OrderRows
            ,CancelOrderNO          = null
            ,AnswerFLG              = 0
            ,EDIOutputDatetime      = null
            ,InsertOperator         = @p_Operator
            ,InsertDateTime         = @SYSDATETIME
            ,UpdateOperator         = @p_Operator
            ,UpdateDateTime         = @SYSDATETIME
            ,DeleteOperator         = null
            ,DeleteDateTime         = null
		FROM D_LastOrder DLOR
		
		INNER JOIN #tmpTargetKey tmp
		ON  tmp.JuchuuNO    = DLOR.JuchuuNO
		AND tmp.JuchuuRows  = DLOR.JuchuuRows
		AND tmp.OrderSEQ    = DLOR.OrderSEQ

		INNER JOIN D_OrderDetails DODD
		ON  DODD.OrderNO    = DLOR.OrderNO
		AND DODD.OrderRows  = DLOR.OrderRows

		WHERE tmp.OrderCD   = @OrderCD

		--発注ヘッダ更新        
		INSERT INTO D_Order(        
             OrderNO
            ,OrderProcessNO
            ,StoreCD
            ,OrderDate
            ,ReturnFLG
            ,OrderDataKBN
            ,OrderWayKBN
            ,OrderCD
            ,OrderPerson
            ,AliasKBN
            ,DestinationKBN
            ,DestinationName
            ,DestinationZip1CD
            ,DestinationZip2CD
            ,DestinationAddress1
            ,DestinationAddress2
            ,DestinationTelphoneNO
            ,DestinationFaxNO
            ,DestinationSoukoCD
            ,CurrencyCD
            ,OrderHontaiGaku
            ,OrderTax8
            ,OrderTax10
            ,OrderGaku
            ,CommentOutStore
            ,CommentInStore
            ,StaffCD
            ,FirstArriveDate
            ,LastArriveDate
            ,ApprovalDate
            ,LastApprovalDate
            ,LastApprovalStaffCD
            ,ApprovalStageFLG
            ,FirstPrintDate
            ,LastPrintDate
            ,InsertOperator
            ,InsertDateTime
            ,UpdateOperator
            ,UpdateDateTime
            ,DeleteOperator
            ,DeleteDateTime             
				)        
		SELECT
             OrderNO                = @CancelOrderNO        
            ,OrderProcessNO         = null --@CancelOrderProcessNO        
            ,StoreCD                = @p_StoreCD        
            ,OrderDate              = @SYSDATE    
            ,ReturnFLG              = 1
            ,OrderDataKBN           = 3
            ,OrderWayKBN            = 3
            ,OrderCD                = @OrderCD    
            ,OrderPerson            = NULL        
            ,AliasKBN               = 0
            ,DestinationKBN         = 1
            ,DestinationName        = null
            ,DestinationZip1CD      = null
            ,DestinationZip2CD      = null
            ,DestinationAddress1    = null
            ,DestinationAddress2    = null
            ,DestinationTelphoneNO  = null        
            ,DestinationFaxNO       = null
            ,DestinationSoukoCD     = null
            ,CurrencyCD             = MCON.CurrencyCD        
            ,OrderHontaiGaku        = SUB_DODD.SumOrderHontaiGaku
            ,OrderTax8              = SUB_DODD.SumOrderTax8        
            ,OrderTax10             = SUB_DODD.SumOrderTax10        
            ,OrderGaku              = SUB_DODD.SumOrderGaku        
            ,CommentOutStore        = null
            ,CommentInStore         = null
            ,StaffCD                = null
            ,FirstArriveDate        = null
            ,LastArriveDate         = null
            ,ApprovalDate           = @SYSDATE
            ,LastApprovalDate       = @SYSDATE
            ,LastApprovalStaffCD    = @p_Operator        
            ,ApprovalStageFLG       = 10
            ,FirstPrintDate         = null
            ,LastPrintDate          = null
            ,InsertOperator         = @p_Operator
            ,InsertDateTime         = @SYSDATETIME    
            ,UpdateOperator         = @p_Operator
            ,UpdateDateTime         = @SYSDATETIME    
            ,DeleteOperator         = null
            ,DeleteDateTime         = null
		FROM (SELECT SUM(OrderHontaiGaku) SumOrderHontaiGaku
					,SUM(OrderGaku) SumOrderGaku
					,SUM(CASE WHEN DODD.OrderTaxRitsu = 2 THEN OrderTax ELSE 0 END) SumOrderTax8
					,SUM(CASE WHEN DODD.OrderTaxRitsu = 1 THEN OrderTax ELSE 0 END) SumOrderTax10
				FROM D_OrderDetails DODD
				INNER JOIN #tmpTargetKey tmp
				ON  tmp.OrderNO = DODD.OrderNO
				AND tmp.OrderRows = DODD.OrderRows
				AND tmp.OrderCD = @OrderCD) SUB_DODD

		INNER JOIN M_Control MCON
		ON MCON.MainKey = 1

		--元の発注明細にキャンセル発注番号を更新
		UPDATE DODD
		SET CancelOrderNO   = @CancelOrderNO
            ,UpdateOperator = @p_Operator
            ,UpdateDateTime = @SYSDATETIME
		FROM D_OrderDetails DODD		
		CROSS APPLY(
            SELECT TOP 1 *    
            FROM #tmpTargetKey tmp
	        WHERE tmp.OrderNO = DODD.OrderNO
            AND tmp.OrderRows = DODD.OrderRows
            AND tmp.OrderCD = @OrderCD) SUB_tmp

		--キャンセルデータ作成日を更新
		UPDATE DLOR
		SET CancelOrderDate = @SYSDATE
			,UpdateOperator = @p_Operator
			,UpdateDateTime = @SYSDATETIME
		FROM D_LastOrder DLOR
		INNER JOIN #tmpTargetKey tmp
		ON tmp.JuchuuNO = DLOR.JuchuuNO
		AND tmp.JuchuuRows = DLOR.JuchuuRows
		AND tmp.OrderSEQ = DLOR.OrderSEQ

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM curOrderCD
    	INTO @OrderCD

    END
    
    --カーソルを閉じる
    CLOSE curOrderCD;
	DEALLOCATE curOrderCD;

	--一時テーブル削除
	DROP TABLE #tmpTargetKey

END
GO

