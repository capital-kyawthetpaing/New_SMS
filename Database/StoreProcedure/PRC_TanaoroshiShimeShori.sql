 BEGIN TRY 
 Drop Procedure dbo.[PRC_TanaoroshiShimeShori]
END try
BEGIN CATCH END CATCH 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [PRC_TanaoroshiShimeShori]    */
CREATE PROCEDURE PRC_TanaoroshiShimeShori(
    -- Add the parameters for the stored procedure here
    @Syori    tinyint,        -- 処理区分（1:請求締,2:請求締キャンセル,3:請求確定）
    @SoukoCD  varchar(6),
    @InventoryDate varchar(10),
    @FromRackNO varchar(13),
    @ToRackNO varchar(13),
    @StoreCD  varchar(4),
    @Operator  varchar(10),
    @PC  varchar(30)
)AS
BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @InventoryNO  varchar(11);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @Program varchar(100); 

    DECLARE @DifferenceQuantity int;
    DECLARE @RackNO varchar(11);
    DECLARE @AdminNO int;
    DECLARE @StockNO varchar(11);
    DECLARE @StockNO_B varchar(11);
    DECLARE @StockSu int;
    DECLARE @AjustSu int;               --★棚卸調整数
    DECLARE @wDifferenceQuantity int;	--確定時に使用
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @Program = 'TanaoroshiShimeShori';	

    --伝票番号採番
    EXEC Fnc_GetNumber
        28,             --in伝票種別 28
        @InventoryDate,    --in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @InventoryNO OUTPUT
        ;
    
    IF ISNULL(@InventoryNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END
    
    --※１件作成    棚卸処理データ(締)
    --D_InventoryProcessing Insert  Table転送仕様Ａ
    INSERT INTO D_InventoryProcessing
    ([InventoryNO]
      ,[SoukoCD]
      ,[FromRackNO]
      ,[ToRackNO]
      ,[InventoryDate]
      ,[InventoryKBN]
      ,[ProcessingDateTime]
      ,[StaffCD]
      ,[InsertOperator]
      ,[InsertDateTime]
      ,[UpdateOperator]
      ,[UpdateDateTime]
      ,[DeleteOperator]
      ,[DeleteDateTime])
  	VALUES (
  	   @InventoryNO
      ,@SoukoCD
      ,@FromRackNO
      ,@ToRackNO
      ,CONVERT(date, @InventoryDate)
      ,@Syori 			--InventoryKBN
      ,@SYSDATETIME		--ProcessingDateTime
      ,@Operator	--StaffCD
      ,@Operator  
      ,@SYSDATETIME
      ,@Operator  
      ,@SYSDATETIME
      ,NULL                  
      ,NULL 
    );
    
    --棚卸締--
    IF @Syori = 1
    BEGIN
        SET @OperateModeNm = '棚卸締';

        --棚卸データ作成
        --D_Inventory Insert    Table転送仕様Ｂ
        INSERT INTO [D_Inventory]
           ([SoukoCD]
           ,[RackNO]
           ,[InventoryDate]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[TheoreticalQuantity]
           ,[ActualQuantity]
           ,[DifferenceQuantity]
           ,[InventoryNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT 
            DS.SoukoCD
           ,DS.RackNO
           ,CONVERT(date, @InventoryDate)
           ,MAX(DS.SKUCD)
           ,DS.AdminNO
           ,MAX(DS.JanCD)
           ,SUM(DS.AllowableSu) AS TheoreticalQuantity
           ,SUM(DS.AllowableSu) AS ActualQuantity
           ,0 AS DifferenceQuantity
           ,@InventoryNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL 
        FROM D_Stock AS DS
        WHERE DS.SoukoCD = @SoukoCD
        AND DS.DeleteDateTime IS NULL
        AND ISNULL(DS.RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(DS.RackNO,'') END)
        AND ISNULL(DS.RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(DS.RackNO,'') END)
        AND DS.RackNO IS Not NULL
        AND DS.ArrivalYetFLG = 0
        GROUP BY DS.SoukoCD, DS.RackNO, DS.AdminNO
        ;
        
        --棚卸制御データ作成
        --D_InventoryControl Insert Table転送仕様Ｃ
        INSERT INTO [D_InventoryControl]
            ([SoukoCD]
            ,[RackNO]
            ,[InventoryDate]
            ,[InventoryKBN]
            ,[InventoryNO]
            ,[AdditionDateTime]
            ,[AdditionStaffCD]
            ,[InsertOperator]
            ,[InsertDateTime]
            ,[UpdateOperator]
            ,[UpdateDateTime]
            ,[DeleteOperator]
            ,[DeleteDateTime])
        SELECT
             ML.SoukoCD
            ,ML.TanaCD AS RackNO
            ,CONVERT(date, @InventoryDate)
            ,1 AS InventoryKBN
            ,@InventoryNO
            ,NULL AS AdditionDateTime
            ,NULL AS AdditionStaffCD
            ,@Operator  
            ,@SYSDATETIME
            ,@Operator  
            ,@SYSDATETIME
            ,NULL                  
            ,NULL 
        FROM F_Location(@InventoryDate) AS ML
        WHERE ML.SoukoCD = @SoukoCD
        AND ML.DeleteFlg = 0
        AND ML.TanaCD >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(ML.TanaCD,'') END)
        AND ML.TanaCD <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(ML.TanaCD,'') END)
        ;
		
    END
    
    --棚卸締ｷｬﾝｾﾙ--
    ELSE IF @Syori = 2
    BEGIN
        SET @OperateModeNm = '棚卸締ｷｬﾝｾﾙ';

        --棚卸データ削除
        --D_Inventory Delete    Table転送仕様Ｂ②
        DELETE FROM [D_Inventory]
        WHERE SoukoCD = @SoukoCD
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        AND InventoryDate = CONVERT(date, @InventoryDate)
        ;
        
        --棚卸制御データ削除
        DELETE FROM [D_InventoryControl]
        WHERE SoukoCD = @SoukoCD
        AND InventoryDate = CONVERT(date, @InventoryDate)
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        ;
        /*
        --D_InventoryControl UPDATE     Table転送仕様Ｃ②
        UPDATE [D_InventoryControl] SET
            InventoryKBN = 3
            ,AdditionDateTime = @SYSDATETIME
            ,AdditionStaffCD = @Operator
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE SoukoCD = @SoukoCD
        AND InventoryDate = @InventoryDate
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        ;
        */
    END
    
    --棚卸確定--
    ELSE IF @Syori = 3
    BEGIN
        --棚卸データの差異数≠０のデータを取得し、在庫データを差異数が０になるまで順にUpdate
        --D_Inventory.DifferenceQuantity≠０&D_Inventory.ADDFlg≠１のデータを取得し、在庫データを差異数が０になるまで順にUpdate

        --カーソル定義
        DECLARE CUR_AAA CURSOR FOR
            SELECT DI.DifferenceQuantity
                  ,DI.RackNO          
                  ,DI.AdminNO     
              FROM D_Inventory AS DI
             WHERE DI.SoukoCD = @SoukoCD
               AND DI.InventoryDate = CONVERT(date, @InventoryDate)
               AND ISNULL(DI.RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND ISNULL(DI.RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND DI.DifferenceQuantity <> 0
               AND DI.ADDFlg <> 1
             ORDER BY DI.RackNO
               ;
                
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @DifferenceQuantity, @RackNO, @AdminNO;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN

    -- ========= ループ内の実際の処理 ここから===
            --※A.DifferenceQuantity＞0の時 差異数がプラスの時
            IF @DifferenceQuantity > 0
            BEGIN
                --★棚卸調整数＝※A.DifferenceQuantity
                --1件目にのみ、更新
                SET @StockNO = (SELECT top 1 DS.StockNO
                                FROM D_Stock AS DS
                                WHERE DS.SoukoCD = @SoukoCD
                                AND DS.RackNO = @RackNO
                                AND DS.AdminNO = @AdminNO
                                AND DS.ArrivalYetFLG = 0
                                AND DS.AllowableSu >= 0
                                ORDER BY DS.AllowableSu desc
                                        ,DS.ArrivalDate
                                        ,DS.StockNO
                                );
                
                --在庫データ更新(1件)                
                --D_Zaiko   Update  Table転送仕様Ｄ 
                UPDATE D_Stock SET
                     StockSu = StockSu + @DifferenceQuantity
                    ,AllowableSu = AllowableSu + @DifferenceQuantity
                    ,AnotherStoreAllowableSu = AnotherStoreAllowableSu + @DifferenceQuantity
                    ,UpdateOperator = @Operator
                    ,UpdateDateTime = @SYSDATETIME
                WHERE StockNO = @StockNO
                ;
                
                --入出庫履歴作成
                --D_Warehousing Insert  Table転送仕様Ｅ
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
                   ,[UnitPrice]
                   ,[Amount]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @InventoryDate --WarehousingDate
                   ,DS.SoukoCD
                   ,DS.RackNO
                   ,@StockNO
                   ,DS.JanCD
                   ,DS.AdminNO
                   ,DS.SKUCD
                   ,17                    --WarehousingKBN   DifferenceQuantity＞0の時 17:棚卸加算、
                   ,0                     --DeleteFlg
                   ,@InventoryNO          --Number
                   ,0                     --NumberRow
                   ,NULL                  --VendorCD
                   ,NULL                  --ToStoreCD
                   ,NULL                  --ToSoukoCD
                   ,NULL                  --ToRackNO
                   ,NULL                  --ToStockNO
                   ,NULL                  --FromStoreCD
                   ,NULL                  --FromSoukoCD]
                   ,NULL                  --FromRackNO
                   ,NULL                  --CustomerCD
                   ,@DifferenceQuantity   --Quantity
                   ,0                     --UnitPrice
                   ,0                     --Amount
                   ,@Program              --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM D_Stock AS DS
                  WHERE StockNO = @StockNO
                  ;
            END
            ELSE    --※A.DifferenceQuantity＜0の時 差異数がマイナスの時
            BEGIN
            	SET @wDifferenceQuantity = @DifferenceQuantity;

                --カーソル定義
                DECLARE CUR_AAA2 CURSOR FOR
                    --在庫数－引当数＞0の在庫データを取得
                    SELECT top 1 DS.StockNO--, DS.StockSu - DS.ReserveSu AS StockSu
                          ,DS.AllowableSu AS StockSu
                    FROM D_Stock AS DS
                    WHERE DS.SoukoCD = @SoukoCD
                    AND DS.RackNO = @RackNO
                    AND DS.AdminNO = @AdminNO
                    AND DS.ArrivalYetFLG = 0
                    AND DS.AllowableSu >= 0
                    --AND DS.StockSu - DS.ReserveSu > 0
                    ORDER BY DS.AllowableSu desc	--(DS.StockSu - DS.ReserveSu) desc
                            ,DS.ArrivalDate
                            ,DS.StockNO
                    ;

                --カーソルオープン
                OPEN CUR_AAA2;

                --最初の1行目を取得して変数へ値をセット
                FETCH NEXT FROM CUR_AAA2
                INTO @StockNO_B, @StockSu;
                
                --データの行数分ループ処理を実行する
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ループ内の実際の処理 ここから===
                    --※A.DifferenceQuantity×(-1)＞※B.StockSu－※B.ReserveSuの時
                    IF -1*@wDifferenceQuantity > @StockSu
                    BEGIN
                        --★棚卸調整数＝※B.StockSu－※B.ReserveSu
                        SET @AjustSu = @StockSu;
                    END
                    ELSE    --※A.DifferenceQuantity×(-1)≦※B.StockSu－※B.ReserveSuの時
                    BEGIN
                        --★棚卸調整数＝(-1)×※A.DifferenceQuantity
                        SET @AjustSu = -1*@wDifferenceQuantity;
                    END
                    
                    --在庫データ更新
                    --D_Stock Update Table転送仕様Ｄ②
                    UPDATE D_Stock SET
                         StockSu = StockSu - @AjustSu        --★棚卸調整数を減算
                        ,AllowableSu = AllowableSu - @AjustSu
                        ,AnotherStoreAllowableSu = AnotherStoreAllowableSu - @AjustSu
                        ,UpdateOperator = @Operator
                        ,UpdateDateTime = @SYSDATETIME
                    WHERE StockNO = @StockNO_B
                    ;
                    
                    --入出庫履歴作成
                    --D_Warehousing	Insert	Table転送仕様Ｅ
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
                       ,[UnitPrice]
                       ,[Amount]
                       ,[Program]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT @InventoryDate --WarehousingDate
                       ,DS.SoukoCD
                       ,DS.RackNO
                       ,@StockNO_B
                       ,DS.JanCD
                       ,DS.AdminNO
                       ,DS.SKUCD
                       ,18            --WarehousingKBN   DifferenceQuantity＜0の時 18:棚卸減算
                       ,0             --DeleteFlg
                       ,@InventoryNO  --Number
                       ,0             --NumberRow
                       ,NULL          --VendorCD
                       ,NULL          --ToStoreCD
                       ,NULL          --ToSoukoCD
                       ,NULL          --ToRackNO
                       ,NULL          --ToStockNO
                       ,NULL          --FromStoreCD
                       ,NULL          --FromSoukoCD]
                       ,NULL          --FromRackNO
                       ,NULL          --CustomerCD
                       ,-1*@AjustSu   --Quantity ★在庫調整数
                                      --DifferenceQuantity＜0の時は、×(-1)
                       ,0             --UnitPrice
                       ,0             --Amount
                       ,@Program      --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL

                      FROM D_Stock AS DS
                      WHERE StockNO = @StockNO_B
                      ;
                      
                    --※A.DifferenceQuantity　＝に★棚卸調整数を加算
                    SET @wDifferenceQuantity = @wDifferenceQuantity + @AjustSu;
                    
                    --※A.DifferenceQuantity　≧0になるまで更新する
                    IF @wDifferenceQuantity >= 0
                    BEGIN
                        BREAK;
                    END
                
                    --次の行のデータを取得して変数へ値をセット
                    FETCH NEXT FROM CUR_AAA2
                    INTO @StockNO_B, @StockSu;
                -- ========= ループ内の実際の処理 ここまで===
                END
                --カーソルを閉じる
                CLOSE CUR_AAA2;
                DEALLOCATE CUR_AAA2;

            END
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @DifferenceQuantity, @RackNO, @AdminNO;
        END
        
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;        

        --D_Inventory.DifferenceQuantity≠０＆ D_Inventory.ADDFlg＝１のデータを取得し、D_StockにInsert

        --カーソル定義
        DECLARE CUR_BBB CURSOR FOR
            SELECT DI.DifferenceQuantity
                  ,DI.RackNO          
                  ,DI.AdminNO     
              FROM D_Inventory AS DI
             WHERE DI.SoukoCD = @SoukoCD
               AND DI.InventoryDate = CONVERT(date, @InventoryDate)
               AND ISNULL(DI.RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND ISNULL(DI.RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND DI.DifferenceQuantity <> 0
               AND DI.ADDFlg = 1
             ORDER BY DI.RackNO
               ;
                
        --カーソルオープン
        OPEN CUR_BBB;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_BBB
        INTO @DifferenceQuantity, @RackNO, @AdminNO;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- ========= ループ内の実際の処理 ここから===

            --Table転送仕様Ｄ 在庫データ更新(1件)
            --伝票番号採番●StockNO
            EXEC Fnc_GetNumber
                21,        --in伝票種別 21
                @InventoryDate, --in基準日
                @StoreCD,  --in店舗CD
                @Operator,
                @StockNO_B OUTPUT
                ;            

            IF ISNULL(@StockNO_B,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_Stock】Insert    
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
                    @StockNO_B
                   ,@SoukoCD
                   ,@RackNO   --RackNO
                   ,NULL    --ArrivalPlanNO
                   ,DI.SKUCD
                   ,DI.AdminNO
                   ,DI.JanCD
                   ,0   --  ArrivalYetFLG(0:入荷済、1:未入荷)
                   ,0   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                   ,NULL    --ArrivalPlanDate
                   ,DI.InventoryDate    --ArrivalDate
                   ,DI.ActualQuantity   --StockSu
                   ,0   --PlanSu
                   ,DI.ActualQuantity   --AllowableSu
                   ,DI.ActualQuantity   --AnotherStoreAllowableSu
                   ,0       --ReserveSu
                   ,0       --InstructionSu
                   ,0       --ShippingSu
                   ,NULL    --OriginalStockNO
                   ,NULL    --ExpectReturnDate
                   ,NULL    --ReturnDate
                   ,0       --ReturnSu
             
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
              FROM D_Inventory AS DI
             WHERE DI.SoukoCD = @SoukoCD
               AND DI.InventoryDate = CONVERT(date, @InventoryDate)
               AND DI.RackNO = @RackNO
               AND DI.AdminNO = @AdminNO
              ;
            
            --Table転送仕様Ｅ 入出庫履歴
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
               ,[UnitPrice]
               ,[Amount]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT @InventoryDate --WarehousingDate
               ,DS.SoukoCD
               ,DS.RackNO
               ,@StockNO_B
               ,DS.JanCD
               ,DS.AdminNO
               ,DS.SKUCD
               ,(CASE WHEN @DifferenceQuantity > 0 THEN 17 ELSE 18 END)            --WarehousingKBN   DifferenceQuantity＜0の時 18:棚卸減算
               ,0             --DeleteFlg
               ,@InventoryNO  --Number
               ,0             --NumberRow
               ,NULL          --VendorCD
               ,NULL          --ToStoreCD
               ,NULL          --ToSoukoCD
               ,NULL          --ToRackNO
               ,NULL          --ToStockNO
               ,NULL          --FromStoreCD
               ,NULL          --FromSoukoCD]
               ,NULL          --FromRackNO
               ,NULL          --CustomerCD
               ,(CASE WHEN @DifferenceQuantity > 0 THEN @DifferenceQuantity ELSE (-1)*@DifferenceQuantity END)   --Quantity ★在庫調整数
                              --DifferenceQuantity＜0の時は、×(-1)
               ,0             --UnitPrice
               ,0             --Amount
               ,@Program      --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM D_Stock AS DS
              WHERE StockNO = @StockNO_B
              ;
              

            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_BBB
            INTO @DifferenceQuantity, @RackNO, @AdminNO;
        END
        
        --カーソルを閉じる
        CLOSE CUR_BBB;
        DEALLOCATE CUR_BBB; 
                
        --棚卸制御データ作成
        --D_InventoryControl UPDATE     Table転送仕様Ｃ②
        UPDATE [D_InventoryControl] SET
            InventoryKBN = 3
            ,AdditionDateTime = @SYSDATETIME
            ,AdditionStaffCD = @Operator
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE SoukoCD = @SoukoCD
        AND InventoryDate = @InventoryDate
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        ;
    END  --棚卸確定
    
    
    --【L_Log】INSERT
    --処理履歴データへ更新
    SET @KeyItem = CONVERT(varchar,@Syori) + ',' + @SoukoCD 
                 + ',' + @InventoryDate + ',' + ISNULL(@FromRackNO,'') + ',' + ISNULL(@ToRackNO,'');
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

END

GO

