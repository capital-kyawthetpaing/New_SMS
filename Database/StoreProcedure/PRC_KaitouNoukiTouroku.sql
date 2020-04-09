 BEGIN TRY 
 Drop Procedure dbo.[PRC_KaitouNoukiTouroku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    回答納期登録
--       Program ID      KaitouNoukiTouroku
--       Create date:    2019.9.30
--    ======================================================================

--CREATE TYPE T_KaitouNouki AS TABLE
--    (
--    [Seq] [int],
--    [OrderNO] [varchar](11) ,
--    [OrderRows] [int],
--    [GyoNO] [int],
--    [JuchuuNO] [varchar](11) ,
--    [JuchuuRows] [int],
--    [StockNO] [varchar](11),
--    [ReserveNO] [varchar](11),
--    [ArrivalPlanNO] [varchar](11),
--    [ArrivalPlanDate] [date] ,
--    [ArrivalPlanMonth] [int],
--    [ArrivalPlanCD] [varchar](4) ,
--    [CalcuArrivalPlanDate] [date],
--	[ArrivalPlanSu] [int],
--    [SoukoCD] [varchar](6) ,
--    [AdminNO] [int] ,
--    [SKUCD] [varchar](30) ,
--    [JanCD] [varchar](13) ,
--    [SKUName] [varchar](80) ,
--    [num2] [int],
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_KaitouNoukiTouroku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @StoreCD varchar(4),
    @OrderCD varchar(13),

    @Table  T_KaitouNouki READONLY,
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
    DECLARE @SYSDATE varchar(10);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO varchar(11);
    DECLARE @ReserveNO varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @SYSDATE = convert(varchar, @SYSDATETIME, 111);
	
	--【Form.Details.Sub.入荷予定日～予定状況に入力がある場合】

	--OrderNO毎にUPDATEしないと履歴ファイルのSEQが重複エラーになるため
    --カーソル定義
    DECLARE CUR_ORDER CURSOR FOR
        SELECT tbl.OrderNO, MIN(tbl.Seq) AS SEQ
        FROM @Table AS tbl
        WHERE tbl.UpdateFlg = 1
        GROUP BY tbl.OrderNO
        ORDER BY SEQ;
	
	DECLARE @OrderNO varchar(11);
	DECLARE @SEQ int;
	
    --カーソルオープン
    OPEN CUR_ORDER;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_ORDER
    INTO  @OrderNO, @SEQ;
     
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        UPDATE [D_Order]
          SET [UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_Order
        INNER JOIN @Table tbl
         ON tbl.OrderNO = D_Order.OrderNO
         AND tbl.ArrivalPlanDate IS NOT NULL
         AND tbl.UpdateFlg = 1
        WHERE D_Order.OrderNO = @OrderNO
         ;

        --D_OrderDetails    Update  Table転送仕様
        UPDATE [D_OrderDetails]
           SET [ArrivePlanDate] = tbl.ArrivalPlanDate                  
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_OrderDetails
        INNER JOIN @Table tbl
         ON tbl.OrderNO = D_OrderDetails.OrderNO
         AND tbl.OrderRows = D_OrderDetails.OrderRows
         AND tbl.ArrivalPlanDate IS NOT NULL
         AND tbl.UpdateFlg = 1
        WHERE D_OrderDetails.OrderNO = @OrderNO
         ;
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_ORDER
    	INTO  @OrderNO, @SEQ;

    END
    
    --カーソルを閉じる
    CLOSE CUR_ORDER;
    DEALLOCATE CUR_ORDER;
    
    --D_ArrivalPlan Update  Table転送仕様Ｂ
    UPDATE [D_ArrivalPlan]
       SET [LastestFLG] = 9     --9（非最新）
           ,[UpdateOperator] =  @Operator  
           ,[UpdateDateTime] =  @SYSDATETIME
    FROM D_ArrivalPlan
    INNER JOIN D_OrderDetails AS DO
     ON DO.OrderNO = D_ArrivalPlan.Number
     AND DO.OrderRows = D_ArrivalPlan.NumberRows
    INNER JOIN @Table tbl
	    ON tbl.JuchuuNO = DO.JuchuuNO
	    AND tbl.JuchuuRows = DO.JuchuuRows
--     AND tbl.UpdateFlg = 1
    WHERE D_ArrivalPlan.ArrivalPlanKBN = 1
	AND NOT EXISTS(SELECT tbl.ArrivalPlanNO
		FROM @Table tbl
	    WHERE tbl.JuchuuNO = DO.JuchuuNO
	     AND tbl.JuchuuRows = DO.JuchuuRows
	     AND tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO)
    ;
    

--小画面明細件数分　INSERT　▼START
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.Seq, ISNULL(tbl.ArrivalPlanNO,'') AS ArrivalPlanNO
        FROM @Table AS tbl
        WHERE tbl.UpdateFlg = 1		--要確認
        ORDER BY tbl.Seq;
	
	
    --カーソルオープン
    OPEN CUR_AAA;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO  @SEQ, @ArrivalPlanNO;
     
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        IF @ArrivalPlanNO = ''
        BEGIN
            --D_ArrivalPlan Insert／Update  Table転送仕様Ｃ
            --伝票番号採番
            EXEC Fnc_GetNumber
                22,         --in伝票種別 22
                @SysDate, --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @ArrivalPlanNO OUTPUT
                ;
                
            IF ISNULL(@ArrivalPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END

            INSERT INTO [D_ArrivalPlan]
                   ([ArrivalPlanNO]
                   ,[ArrivalPlanKBN]
                   ,[Number]
                   ,[NumberRows]
                   ,[NumberSEQ]
                   ,[ArrivalPlanDate]
                   ,[ArrivalPlanMonth]
                   ,[ArrivalPlanCD]
                   ,[CalcuArrivalPlanDate]
                   ,[ArrivalPlanUpdateDateTime]
                   ,[StaffCD]
                   ,[LastestFLG]
                   ,[EDIImportNO]
                   ,[SoukoCD]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[ArrivalPlanSu]
                   ,[ArrivalSu]
                   ,[OriginalArrivalPlanNO]
                   ,[OrderCD]
                   ,[FromSoukoCD]
                   ,[ToStoreCD]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             SELECT
                   @ArrivalPlanNO
                   ,1   --ArrivalPlanKBN
                   ,tbl.OrderNo     --Number
                   ,tbl.OrderRows   --NumberRows
                   ,tbl.GyoNO
                   ,tbl.ArrivalPlanDate
                   ,tbl.ArrivalPlanMonth
                   ,tbl.ArrivalPlanCD
                   ,tbl.CalcuArrivalPlanDate
                   ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                   ,@Operator   --StaffCD
                   ,1   --LastestFLG
                   ,NULL    --EDIImportNO
                   ,tbl.SoukoCD    --SoukoCD	2020.03.30 chg
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.ArrivalPlanSu
                   ,0   --ArrivalSu
                   ,NULL    --OriginalArrivalPlanNO
                   ,@OrderCD
                   ,NULL    --FromSoukoCD
                   ,NULL    --ToStoreCD
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL    --DeleteOperator
                   ,NULL    --DeleteDateTime
               FROM  @Table tbl
               WHERE ISNULL(tbl.ArrivalPlanNO,'') = ''
               AND tbl.UpdateFlg = 1
               AND tbl.SEQ = @SEQ
               ;
        END
        
        UPDATE [D_ArrivalPlan]
        SET [ArrivalPlanDate] = tbl.ArrivalPlanDate
          ,[ArrivalPlanMonth] = tbl.ArrivalPlanMonth
          ,[ArrivalPlanCD] = tbl.ArrivalPlanCD
          ,[CalcuArrivalPlanDate] = tbl.CalcuArrivalPlanDate
          ,[ArrivalPlanUpdateDateTime] = @SYSDATETIME
          ,[ArrivalPlanSu] = tbl.ArrivalPlanSu
          ,[UpdateOperator] =  @Operator  
          ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_ArrivalPlan
        INNER JOIN @Table tbl
           ON tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
           AND tbl.UpdateFlg = 1
	       AND tbl.SEQ = @SEQ
        ;

        --D_Stock   Insert／Update  Table転送仕様Ｅ
        --伝票番号採番
        EXEC Fnc_GetNumber
            21,         --in伝票種別 21:在庫
            @SysDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @StockNO OUTPUT
            ;

        IF ISNULL(@StockNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        INSERT INTO [D_Stock]
               ([StockNO]
               ,[SoukoCD]
               ,[RackNO]
               ,[ArrivalPlanNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[ArrivalYetFLG]
               ,[ArrivalPlanKBN]
               ,[ArrivalPlanDate]
               ,[ArrivalDate]
               ,[StockSu]
               ,[PlanSu]
               ,[AllowableSu]
               ,[AnotherStoreAllowableSu]
               ,[ReserveSu]
               ,[InstructionSu]
               ,[ShippingSu]
               ,[OriginalStockNO]
               ,[ExpectReturnDate]
               ,[ReturnDate]
               ,[ReturnSu]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
         SELECT
               @StockNO
               ,tbl.SoukoCD
               ,NULL    --RackNO
               ,@ArrivalPlanNO
               ,tbl.SKUCD
               ,tbl.AdminNO
               ,tbl.JanCD
               ,1   --ArrivalYetFLG
               ,(CASE WHEN ISNULL(tbl.JuchuuNO,'') <> '' THEN 1
                    ELSE 2 END)   --ArrivalPlanKBN
               ,tbl.CalcuArrivalPlanDate
               ,NULL    --ArrivalDate
               ,0   --StockSu
               ,tbl.ArrivalPlanSu
               ,(CASE WHEN ISNULL(tbl.JuchuuNO,'') <> '' THEN 0
                    ELSE tbl.ArrivalPlanSu END)   --AllowableSu
               ,(CASE WHEN ISNULL(tbl.JuchuuNO,'') <> '' THEN 0
                    ELSE tbl.ArrivalPlanSu END)   --AnotherStoreAllowableSu
               ,(CASE WHEN ISNULL(tbl.JuchuuNO,'') <> '' THEN tbl.ArrivalPlanSu
                    ELSE 0 END)   --ReserveSu
               ,0   --InstructionSu
               ,0   --ShippingSu
               ,NULL    --OriginalStockNO
               ,NULL    --ExpectReturnDate
               ,NULL    --ReturnDate
               ,0   --ReturnSu
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           WHERE (tbl.CalcuArrivalPlanDate IS NOT NULL
                OR tbl.num2 IN (1,2))   --M_MultiPorpose.Num2＝1 or 2 の場合
           AND ISNULL(tbl.StockNO,'') = ''
           AND tbl.UpdateFlg = 1
           AND tbl.SEQ = @SEQ
           ;
       
        UPDATE [D_Stock]
           SET [ArrivalPlanDate] = tbl.CalcuArrivalPlanDate
              ,[PlanSu] = tbl.ArrivalPlanSu
              ,[ReserveSu] = tbl.ArrivalPlanSu  --引当数
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
        FROM D_Stock
        INNER JOIN @Table tbl
         ON tbl.StockNO = D_Stock.StockNO
         AND tbl.UpdateFlg = 1
	       AND tbl.SEQ = @SEQ
		;
                     
        --D_Reserve Insert／Update  Table転送仕様Ｇ
        --伝票番号採番
        EXEC Fnc_GetNumber
            12,         --in伝票種別 12:引当
            @SysDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @ReserveNO OUTPUT
            ;

        IF ISNULL(@ReserveNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        INSERT INTO [D_Reserve]
               ([ReserveNO]
               ,[ReserveKBN]
               ,[Number]
               ,[NumberRows]
               ,[StockNO]
               ,[SoukoCD]
               ,[JanCD]
               ,[SKUCD]
               ,[AdminNO]
               ,[ReserveSu]
               ,[ShippingPossibleDate]
               ,[ShippingPossibleSU]
               ,[ShippingOrderNO]
               ,[ShippingOrderRows]
               ,[CompletedPickingNO]
               ,[CompletedPickingRow]
               ,[CompletedPickingDate]
               ,[ShippingSu]
               ,[ReturnKBN]
               ,[OriginalReserveNO]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
         SELECT
                @ReserveNO
               ,1   --ReserveKBN
               ,tbl.JuchuuNO
               ,tbl.JuchuuRows
               ,@StockNO
               ,tbl.SoukoCD
               ,tbl.JanCD
               ,tbl.SKUCD
               ,tbl.AdminNO
               ,tbl.ArrivalPlanSu   --ReserveSu
               ,NULL    --ShippingPossibleDate
               ,0   --ShippingPossibleSU
               ,NULL    --ShippingOrderNO
               ,0    --ShippingOrderRows
               ,0   --CompletedPickingNO
               ,0   --CompletedPickingRow
               ,NULL    --CompletedPickingDate
               ,0   --ShippingSu
               ,0   --ReturnKBN
               ,NULL    --OriginalReserveNO
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           WHERE (tbl.CalcuArrivalPlanDate IS NOT NULL
                OR tbl.num2 IN (1,2))   --M_MultiPorpose.Num2＝1 or 2 の場合
           AND ISNULL(tbl.JuchuuNO,'') <> ''	--修正　2020.3.28
           AND ISNULL(tbl.ReserveNO,'') = ''
           AND tbl.UpdateFlg = 1
	       AND tbl.SEQ = @SEQ
           ;

        UPDATE D_Reserve
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[ReserveSu]   =  tbl.ArrivalPlanSu   --引当数
        FROM D_Reserve
        INNER JOIN @Table tbl
         ON tbl.ReserveNO = D_Reserve.ReserveNO
         AND tbl.UpdateFlg = 1
	     AND tbl.SEQ = @SEQ
         ;

        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @SEQ, @ArrivalPlanNO;

    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
--小画面明細件数分　INSERT　▲END

	--【Form.Details.Sub.入荷予定日～予定状況に入力ない場合（入力あったのに、なくなった）】
	--OrderNO毎にUPDATEしないと履歴ファイルのSEQが重複エラーになるため
    --カーソル定義
    DECLARE CUR_ORDER2 CURSOR FOR
        SELECT tbl.OrderNO, MIN(tbl.Seq) AS SEQ
        FROM @Table AS tbl
        WHERE tbl.UpdateFlg = 2
        GROUP BY tbl.OrderNO
        ORDER BY SEQ;
	
    --カーソルオープン
    OPEN CUR_ORDER2;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_ORDER2
    INTO  @OrderNO, @SEQ;
     
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        UPDATE [D_Order]
          SET [UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_Order
        INNER JOIN @Table tbl
         ON tbl.OrderNO = D_Order.OrderNO
         AND tbl.UpdateFlg = 2
        WHERE D_Order.OrderNO = @OrderNO
         ;

        --D_OrderDetails　Update    Table転送仕様Ａ②
        UPDATE [D_OrderDetails]
           SET [ArrivePlanDate] = tbl.ArrivalPlanDate                  
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_OrderDetails
        INNER JOIN @Table tbl
         ON tbl.OrderNO = D_OrderDetails.OrderNO
         AND tbl.OrderRows = D_OrderDetails.OrderRows
         AND tbl.UpdateFlg = 2
        WHERE D_OrderDetails.OrderNO = @OrderNO
         ;
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_ORDER2
    	INTO  @OrderNO, @SEQ;

    END
    
    --カーソルを閉じる
    CLOSE CUR_ORDER2;
    DEALLOCATE CUR_ORDER2;
	
	--D_ArrivalPlan　Update	Table転送仕様Ｂ②
    UPDATE [D_ArrivalPlan]
    SET [LastestFLG] = 1
      ,[UpdateOperator] =  @Operator  
      ,[UpdateDateTime] =  @SYSDATETIME
    FROM D_ArrivalPlan
    INNER JOIN D_OrderDetails AS DO
     ON DO.OrderNO = D_ArrivalPlan.Number
     AND DO.OrderRows = D_ArrivalPlan.NumberRows
    INNER JOIN @Table tbl
     ON tbl.JuchuuNO = DO.JuchuuNO
     AND tbl.JuchuuRows = DO.JuchuuRows
     AND tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
     AND tbl.UpdateFlg = 2
     WHERE D_ArrivalPlan.ArrivalPlanKBN = 1
	;

	--D_ArrivalPlan　Update	Table転送仕様Ｃ②
    UPDATE [D_ArrivalPlan]
    SET [ArrivalPlanDate] = NULL
      ,[ArrivalPlanMonth] = NULL
      ,[ArrivalPlanCD] = NULL
      ,[CalcuArrivalPlanDate] = NULL
      ,[ArrivalPlanUpdateDateTime] = @SYSDATETIME
      ,[ArrivalPlanSu] = 0
      ,[DeleteOperator] =  @Operator  
      ,[DeleteDateTime] =  @SYSDATETIME
    FROM D_ArrivalPlan
    INNER JOIN @Table tbl
     ON tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
     AND tbl.UpdateFlg = 2
     
	--D_Stock　Update	Table転送仕様Ｅ②
    UPDATE [D_Stock]
       SET [ArrivalPlanDate] = NULL
          ,[PlanSu] = 0
          ,[AllowableSu] = 0
          ,[AnotherStoreAllowableSu] = 0
          ,[ReserveSu] = 0	--引当数
	      ,[DeleteOperator] =  @Operator  
	      ,[DeleteDateTime] =  @SYSDATETIME
    FROM D_Stock
    INNER JOIN @Table tbl
     ON tbl.StockNO = D_Stock.StockNO
--	 AND (tbl.CalcuArrivalPlanDate IS NOT NULL
--       		OR tbl.num2 IN (1,2))	--M_MultiPorpose.Num2＝1 or 2 の場合
     AND tbl.UpdateFlg = 2
	;

	--D_Reserve　Update	Table転送仕様Ｇ②
    UPDATE D_Reserve
        SET [DeleteOperator] =  @Operator  
	      ,[DeleteDateTime] =  @SYSDATETIME
           ,[ReserveSu]   =  0   --引当数
    FROM D_Reserve
    INNER JOIN @Table tbl
     ON tbl.ReserveNO = D_Reserve.ReserveNO
     AND tbl.UpdateFlg = 2
     ;
	
	--L_LogInsert	Table転送仕様Ｚ
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'KaitouNoukiTouroku',
        @PC,
        @OperateModeNm,
        NULL;	--@KeyItem;

--    SET @OutJuchuuNo = @JuchuuNO;
    
--<<OWARI>>
  return @W_ERR;

END


