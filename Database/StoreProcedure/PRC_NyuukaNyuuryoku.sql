 BEGIN TRY 
 Drop Procedure dbo.[PRC_NyuukaNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--CREATE TYPE T_Nyuuka AS TABLE
--    (
--    [DataKbn][tinyint],		--1:【引当】,2:【発注】,3:【移動】
--    [ArrivalRows] [int],
--    [ArrivalKBN] [tinyint] ,
--    [OrderNO] [varchar](11) ,
--    [OrderRows] [int],
----    [MoveNO] [varchar](11) ,
----    [MoveRows] [int],
--    [ArrivalPlanNO] [varchar](11) ,
--    [StockNO] [varchar](11) ,
--    [ReserveNO] [varchar](11) ,
--    [CustomerCD] [varchar](13) ,
--    [ArrivalSu] [int] ,
--    [ArrivalPlanKBN][tinyint],
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_NyuukaNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @ArrivalNO   varchar(11),
    @VendorDeliveryNo  varchar(15),
    @StoreCD   varchar(4),
    @VendorCD   varchar(13),
    @ArrivalDate  varchar(10),
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    @JANCD   varchar(13),
    @AdminNO int ,
    @SKUCD   varchar(30),
    @ArrivalSu   int,

    @Table  T_Nyuuka READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutArrivalNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO  varchar(11);
    DECLARE @ReserveNO varchar(11);
    DECLARE @SYSDATE date;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @SYSDATE = CONVERT(date, @SYSDATETIME);
	
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --伝票番号採番
        EXEC Fnc_GetNumber
            5,             --in伝票種別 5
            @ArrivalDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @ArrivalNO OUTPUT
            ;
        
        IF ISNULL(@ArrivalNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --【D_Arrival】Table転送仕様Ａ
        INSERT INTO [D_Arrival]
           ([ArrivalNO]
           ,[VendorDeliveryNo]
           ,[StoreCD]
           ,[VendorCD]
           ,[ArrivalDate]
           ,[InputDate]
           ,[ArrivalKBN]
           ,[StaffCD]
           ,[SoukoCD]
           ,[RackNO]
           ,[JanCD]
           ,[AdminNO]
           ,[SKUCD]
           ,[ArrivalSu]
           ,[PurchaseSu]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@ArrivalNO     
           ,@VendorDeliveryNo        
           ,(SELECT top 1 M.StoreCD
                FROM M_Souko AS M
                WHERE M.SoukoCD = @SoukoCD
                AND M.ChangeDate <= @ArrivalDate
                ORDER BY M.ChangeDate desc)
           ,@VendorCD                                             
           ,convert(date,@ArrivalDate)  
           ,@SYSDATE    --InputDate
           ,(SELECT A.ArrivalPlanKBN
            FROM D_ArrivalPlan AS A
            WHERE A.ArrivalPlanNO = (SELECT top 1 tbl.ArrivalPlanNO FROM @Table tbl))	--ArrivalKBN
           ,@StaffCD
           ,@SoukoCD
           ,NULL    --RackNO
           ,@JANCD
           ,@AdminNO
           ,@SKUCD
           ,@ArrivalSu  --ArrivalSu
           ,0 --PurchaseSu
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

    END
        
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
        
        UPDATE D_Arrival SET
               [VendorDeliveryNo] = @VendorDeliveryNo
              ,[StoreCD] = (SELECT top 1 M.StoreCD
                            FROM M_Souko AS M
                            WHERE M.SoukoCD = @SoukoCD
                            AND M.ChangeDate <= @ArrivalDate
                            ORDER BY M.ChangeDate desc)  
              ,[VendorCD] = @VendorCD
              ,[ArrivalDate] = @ArrivalDate
              ,[InputDate] = @SYSDATE
              ,[SoukoCD] = @SoukoCD
              ,[JanCD] = @JanCD
              ,[AdminNO] = @AdminNO
              ,[SKUCD] = @SKUCD
              ,[ArrivalSu] = @ArrivalSu      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE ArrivalNO = @ArrivalNO
           ;
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        DELETE FROM [D_Arrival]
         WHERE [ArrivalNO] = @ArrivalNO
         ;

    END
    
    --【D_ArrivalDetails】Table転送仕様Ｂ
    --行削除されたデータはDELETE処理
    DELETE FROM [D_ArrivalDetails]
     WHERE [ArrivalNO] = @ArrivalNO
     ;
         
    IF @OperateMode <= 2    --新規・修正時
    BEGIN
        INSERT INTO [D_ArrivalDetails]
                   ([ArrivalNO]
                   ,[ArrivalRows]
                   ,[ArrivalKBN]
                   ,[Number]
                   ,[ArrivalPlanNO]
                   ,[ArrivalSu]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @ArrivalNO                         
                   ,tbl.ArrivalRows                       
                   ,tbl.ArrivalKBN
                   ,tbl.OrderNO	--【引当】なら受注番号、【在庫】なら発注・移動番号
                   ,tbl.ArrivalPlanNO  --ArrivalPlanNO
                   ,tbl.ArrivalSu

                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;

        --【D_ArrivalPlan】Update   Table転送仕様Ｃ	★★
        UPDATE [D_ArrivalPlan] SET
           [ArrivalPlanSu] = tbl.ArrivalSu
          ,[ArrivalSu] = tbl.ArrivalSu
          ,[UpdateOperator]     =  @Operator  
          ,[UpdateDateTime]     =  @SYSDATETIME
        
         FROM D_ArrivalPlan AS DP
         INNER JOIN @Table AS tbl
         ON tbl.ArrivalPlanNO = DP.ArrivalPlanNO
         AND tbl.UpdateFlg = 0
        ;
        
        
        --カーソル定義
        DECLARE CUR_TABLE CURSOR FOR
            SELECT tbl.ArrivalPlanNO, tbl.StockNO, tbl.ReserveNO, tbl.ArrivalSu
            FROM @Table AS tbl
            ORDER BY tbl.ArrivalPlanNO
            ;
        
        DECLARE @tblArrivalPlanNO varchar(11);
        DECLARE @tblStockNO varchar(11);
        DECLARE @tblReserveNO varchar(11);
    	DECLARE @tblArrivalSu int;

        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblArrivalPlanNO, @tblStockNO, @tblReserveNO, @tblArrivalSu;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
                
            --伝票番号採番
            EXEC Fnc_GetNumber
                22,             --in伝票種別 5
                @ArrivalDate, --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @ArrivalPlanNO OUTPUT
                ;
            
            IF ISNULL(@ArrivalPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
        
            --【D_ArrivalPlan】分割分更新（Insert） Table転送仕様Ｃ     TableのデータをLoopする必要あり?
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
                   ,DP.ArrivalPlanKBN
                   ,DP.Number
                   ,DP.NumberRows
                   ,DP.NumberSEQ
                   ,DP.ArrivalPlanDate
                   ,DP.ArrivalPlanMonth
                   ,DP.ArrivalPlanCD
                   ,DP.CalcuArrivalPlanDate
                   ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                   ,@StaffCD
                   ,DP.LastestFLG
                   ,DP.EDIImportNO
                   ,DP.SoukoCD
                   ,DP.SKUCD
                   ,DP.AdminNO
                   ,DP.JanCD
                   ,DP.ArrivalPlanSu-@tblArrivalSu  --元のレコードのArrivalPlanSu - 明細入荷数
                   ,0   --ArrivalSu
                   ,DP.ArrivalPlanNO	--元のレコードのArrivalPlanNO
                   ,DP.OrderCD
                   ,DP.FromSoukoCD
                   ,DP.ToStoreCD
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
             FROM D_ArrivalPlan AS DP
             WHERE DP.ArrivalPlanNO = @tblArrivalPlanNO
             AND DP.DeleteDateTime IS NULL
             ;

            
            IF @OperateMode = 2     --修正時
            BEGIN  
                --伝票番号採番
                EXEC Fnc_GetNumber
                    21,             --in伝票種別 21
                    @ArrivalDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @StockNO OUTPUT
                    ;
                
                IF ISNULL(@StockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Stock】分割分更新（Insert）
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
                       ,DS.SoukoCD
                       ,NULL    --RackNO
                       ,@ArrivalPlanNO
                       ,DS.SKUCD
                       ,DS.AdminNO
                       ,DS.JanCD
                       ,1   --  ArrivalYetFLG
                       ,DS.ArrivalPlanKBN
                       ,DS.ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,0   --StockSu
                       ,DS.StockSu - @tblArrivalSu  --PlanSu
                       ,0   --AllowableSu
                       ,0   --AnotherStoreAllowableSu
                       ,DS.ReserveSu - @tblArrivalSu    --ReserveSu
                       ,DS.InstructionSu
                       ,DS.ShippingSu
                       ,DS.StockNO  --OriginalStockNO
                       ,DS.ExpectReturnDate
                       ,DS.ReturnDate
                       ,DS.ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                 FROM D_Stock AS DS
                 WHERE DS.StockNO = @tblStockNO
                ;
                
                --伝票番号採番
                EXEC Fnc_GetNumber
                    12,             --in伝票種別 12
                    @ArrivalDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @ReserveNO OUTPUT
                    ;
                
                IF ISNULL(@ReserveNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Reserve】分割分更新（Insert）
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
                       ,DR.ReserveKBN
                       ,DR.Number
                       ,DR.NumberRows
                       ,@StockNO
                       ,DR.SoukoCD
                       ,DR.JanCD
                       ,DR.SKUCD
                       ,DR.AdminNO
                       ,DR.ReserveSu - @tblArrivalSu    --元のレコードのReserveSu - 明細入荷数
                       ,NULL    --ShippingPossibleDate
                       ,0       --ShippingPossibleSU
                       ,NULL    --ShippingOrderNO
                       ,0       --ShippingOrderRows
                       ,NULL    --CompletedPickingNO
                       ,0       --CompletedPickingRow
                       ,NULL    --CompletedPickingDate
                       ,0       --ShippingSu
                       ,0       --ReturnKBN
                       ,DR.OriginalReserveNO
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                 FROM D_Reserve AS DR
                 WHERE @tblReserveNO = DR.ReserveNO
                ;
            END		--修正時のみINSERT？
            
            --【D_Stock】           Update  Table転送仕様Ｄ
            UPDATE [D_Stock] SET
                   [ArrivalYetFLG] = 0
                  ,[ArrivalDate] = @SYSDATE
                  ,[StockSu] = @tblArrivalSu
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM D_Stock AS DS
             WHERE DS.StockNO = @tblStockNO
             AND DS.DeleteDateTime IS NULL
            ;
            
            --【D_Reserve】         Update  Table転送仕様Ｅ
            UPDATE [D_Reserve] SET
                   [ShippingPossibleDate] = @SYSDATE
                  ,[ShippingPossibleSU] = [ShippingPossibleSU] + @tblArrivalSu
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM D_Reserve AS DR
             WHERE DR.ReserveNO  = @tblReserveNO
             AND DR.DeleteDateTime IS NULL
            ;
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
            INTO @tblArrivalPlanNO, @tblStockNO, @tblReserveNO, @tblArrivalSu;

        END
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
        

    END
	ELSE	--削除時
	BEGIN
        --【D_ArrivalPlan】     Update/Delete   Table転送仕様Ｃ②
        UPDATE [D_ArrivalPlan] SET
           [ArrivalPlanSu] = [ArrivalPlanSu] + tbl.ArrivalSu
          ,[ArrivalSu] = D_ArrivalPlan.[ArrivalSu] - tbl.ArrivalSu
          ,[UpdateOperator]     =  @Operator  
          ,[UpdateDateTime]     =  @SYSDATETIME
        
         FROM @Table AS tbl
         WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
         AND tbl.UpdateFlg = 0
        ;
        
        --【D_ArrivalPlan】  分割分削除（Delete）
        DELETE FROM D_ArrivalPlan
        WHERE EXISTS (SELECT 1 FROM D_ArrivalPlan AS DP 
                            INNER JOIN @Table AS tbl
                             ON tbl.ArrivalPlanNO = DP.ArrivalPlanNO
                             AND tbl.UpdateFlg = 0
                    WHERE DP.OriginalArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO)
                    ;

        --【D_Stock】           Update/Delete   Table転送仕様Ｄ②
        UPDATE [D_Stock] SET
	           [ArrivalYetFLG] = 1
	          ,[ArrivalDate] = NULL
	          ,[StockSu] = [StockSu] - tbl.ArrivalSu
	          ,[PlanSu] = [PlanSu] + tbl.ArrivalSu
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              
         FROM @Table AS tbl
         WHERE tbl.StockNO = D_Stock.StockNO
         AND tbl.UpdateFlg = 0
        ;
        
        --【D_Stock】  分割分削除（Delete）
        DELETE FROM D_Stock
        WHERE EXISTS (SELECT 1 FROM D_Stock AS DS
                            INNER JOIN @Table AS tbl
                             ON tbl.StockNO = DS.StockNO
                             AND tbl.UpdateFlg = 0
                    WHERE DS.OriginalStockNO = D_Stock.StockNO)
                    ;
        
        --【D_Reserve】         Update/Delete   Table転送仕様Ｅ②
        UPDATE [D_Reserve] SET
	           [ShippingPossibleDate] = NULL
	          ,[ShippingPossibleSU] = [ShippingPossibleSU] - tbl.ArrivalSu
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              
         FROM @Table AS tbl
         WHERE tbl.ReserveNO = D_Reserve.ReserveNO
         AND tbl.UpdateFlg = 0
        ;
        
        --【D_Reserve】  分割分削除（Delete）
        DELETE FROM D_Reserve
        WHERE EXISTS (SELECT 1 FROM D_Reserve AS DR
                            INNER JOIN @Table AS tbl
                             ON tbl.ReserveNO = DR.ReserveNO
                             AND tbl.UpdateFlg = 0
                    WHERE DR.OriginalReserveNO = D_Reserve.ReserveNO)
                    ;

	END

    --【D_Warehousing】追加更新（Insert)  Table転送仕様Ｆ 発注
	INSERT INTO [D_Warehousing]
       ([WarehousingDate]
       ,[SoukoCD]
       ,[RackNO]
       ,[StockNO]
       ,[JanCD]
       ,[AdminNO]
       ,[SKUCD]
       ,[WarehousingKBN]
       ,[DeleteFlg]
       ,[Number]
       ,[NumberRow]
       ,[VendorCD]
       ,[ToStoreCD]
       ,[ToSoukoCD]
       ,[ToRackNO]
       ,[ToStockNO]
       ,[FromStoreCD]
       ,[FromSoukoCD]
       ,[FromRackNO]
       ,[CustomerCD]
       ,[Quantity]
       ,[Program]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
    SELECT (CASE WHEN @OperateMode = 3 THEN (CASE WHEN @ArrivalDate > @SYSDATE THEN @ArrivalDate ELSE @SYSDATE END)
    			 ELSE @ArrivalDate END)	--WarehousingDate
       ,@SoukoCD
       ,NULL	--RackNO
       ,@StockNO
       ,@JanCD
       ,@AdminNO
       ,@SKUCD
       ,1	--WarehousingKBN
       ,(CASE WHEN @OperateMode = 3 THEN 1 ELSE 0 END)	--DeleteFlg
       ,@ArrivalNO	--Number
       ,tbl.ArrivalRows	--NumberRow
       ,@VendorCD
       ,NULL	--ToStoreCD
       ,NULL	--ToSoukoCD
       ,NULL	--ToRackNO
       ,NULL	--ToStockNO
       ,NULL    --FromStoreCD
       ,NULL	--FromSoukoCD]
       ,NULL	--FromRackNO
       ,tbl.CustomerCD
       ,(CASE @OperateMode WHEN 3 THEN -1 ELSE 1 END) * tbl.ArrivalSu	--Quantity
       ,'NyuukaNyuuryoku'	--Program
       
       ,@Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
       ,NULL
       ,NULL

      FROM @Table tbl
      WHERE tbl.UpdateFlg = 0
      AND tbl.ArrivalPlanKBN = 1	--発注
      ;

    --【D_Warehousing】追加更新（Insert)  Table転送仕様Ｆ 移動
	INSERT INTO [D_Warehousing]
       ([WarehousingDate]
       ,[SoukoCD]
       ,[RackNO]
       ,[StockNO]
       ,[JanCD]
       ,[AdminNO]
       ,[SKUCD]
       ,[WarehousingKBN]
       ,[DeleteFlg]
       ,[Number]
       ,[NumberRow]
       ,[VendorCD]
       ,[ToStoreCD]
       ,[ToSoukoCD]
       ,[ToRackNO]
       ,[ToStockNO]
       ,[FromStoreCD]
       ,[FromSoukoCD]
       ,[FromRackNO]
       ,[CustomerCD]
       ,[Quantity]
       ,[Program]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
    SELECT @ArrivalDate	--WarehousingDate
       ,@SoukoCD
       ,NULL	--RackNO
       ,@StockNO
       ,@JanCD
       ,@AdminNO
       ,@SKUCD
       ,14	--WarehousingKBN
       ,(CASE WHEN @OperateMode = 3 THEN 1 ELSE 0 END)	--DeleteFlg
       ,@ArrivalNO	--Number
       ,tbl.ArrivalRows	--NumberRow
       ,NULL	--VendorCD
       ,NULL	--ToStoreCD
       ,NULL	--ToSoukoCD
       ,NULL	--ToRackNO
       ,NULL	--ToStockNO
       ,(SELECT top 1 M.StoreCD
                        FROM M_Souko AS M
                        WHERE M.SoukoCD = @SoukoCD
                        AND M.ChangeDate <= @ArrivalDate
                        ORDER BY M.ChangeDate desc)  --FromStoreCD
       ,@SoukoCD	--FromSoukoCD]
       ,NULL	--FromRackNO
       ,tbl.CustomerCD
       ,(CASE @OperateMode WHEN 3 THEN -1 ELSE 1 END) * tbl.ArrivalSu	--Quantity
       ,'NyuukaNyuuryoku'	--Program
       
       ,@Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
       ,NULL
       ,NULL

      FROM @Table tbl
      WHERE tbl.UpdateFlg = 0
      AND tbl.ArrivalPlanKBN = 2	--移動
      ;
    
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.OrderNO, tbl.OrderRows, tbl.ArrivalSu
        FROM @Table AS tbl
        WHERE tbl.DataKbn > 1
        UNION ALL
        SELECT DO.OrderNO, DO.OrderRows, tbl.ArrivalSu
        FROM @Table AS tbl
        INNER JOIN D_OrderDetails AS DO
        ON DO.JuchuuNO = tbl.OrderNO
        AND DO.JuchuuRows = tbl.OrderRows
        WHERE tbl.DataKbn = 1
        ORDER BY OrderNO, OrderRows
        ;
    
    DECLARE @OrderNO varchar(11);
    DECLARE @OrderRows int;
	
    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO @OrderNO, @OrderRows, @tblArrivalSu;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        --履歴ファイルのSEQが重複してしまうためヘッダも更新
        UPDATE D_Order SET
            [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE OrderNO = @OrderNO
         ;
         
        --【D_OrderDetails】    Update  Table転送仕様Ｇ     TableのデータをLoopする必要あり
        UPDATE D_OrderDetails
            SET [TotalArrivalSu] = [TotalArrivalSu] + (CASE WHEN @OperateMode = 3 THEN -1 * @tblArrivalSu ELSE @tblArrivalSu END)
               ,[UpdateOperator] = @Operator  
               ,[UpdateDateTime] = @SYSDATETIME
         WHERE OrderNO = @OrderNO
         AND OrderRows = @OrderRows
         ;
            
        --【D_MoveDetails】     Update  Table転送仕様Ｈ
        UPDATE D_MoveDetails
            SET [TotalArrivalSu] =  [TotalArrivalSu] + (CASE WHEN @OperateMode = 3 THEN -1 * @tblArrivalSu ELSE @tblArrivalSu END)
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
         WHERE MoveNO = @OrderNO
         AND MoveRows = @OrderRows
         ;
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @OrderNO, @OrderRows, @tblArrivalSu;

    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
    
    --処理履歴データへ更新
    SET @KeyItem = @ArrivalNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NyuukaNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutArrivalNO = @ArrivalNO;
    
--<<OWARI>>
  return @W_ERR;

END


