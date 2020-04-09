 BEGIN TRY 
 Drop Procedure dbo.[PRC_ShiireNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[PRC_ShiireNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @PurchaseNO   varchar(11),
    @StoreCD   varchar(4),
    @PurchaseDate  varchar(10),
    @PaymentPlanDate varchar(10),
    @StockAccountFlg tinyint,
    @StaffCD   varchar(10),
    @VendorCD   varchar(13),
    @PayeeCD  varchar(13),
    @CalculationGaku money,
    @AdjustmentGaku money,
    @PurchaseGaku money,
    @PurchaseTax money,
    @CommentInStore varchar(80) ,

    @Table  T_Shiire READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutPurchaseNO varchar(11) OUTPUT
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
    DECLARE @Program varchar(100); 
    DECLARE @StockNO varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @Program = 'ShiireNyuuryoku';	

    --カーソル定義
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.PurchaseRows, tbl.UpdateFlg
        FROM @Table AS tbl
        ORDER BY tbl.PurchaseRows
        ;
        
	DECLARE @tblPurchaseRows int;
	DECLARE @tblUpdateFlg int;
	
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';

		--【D_PayPlan】Insert　Table転送仕様Ｅ 支払予定
		INSERT INTO [D_PayPlan]
           ([PayPlanKBN]
           ,[Number]
           ,[StoreCD]
           ,[PayeeCD]
           ,[RecordedDate]
           ,[PayPlanDate]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[PayPlanGaku]
           ,[PayConfirmGaku]
           ,[PayConfirmFinishedKBN]
           ,[PayCloseDate]
           ,[PayCloseNO]
           ,[Program]
           ,[PayConfirmFinishedDate]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        VALUES(        
            1   --PayPlanKBN
           ,@PurchaseNO --Number
           ,@StoreCD
           ,@PayeeCD
           ,@PurchaseDate   --RecordedDate
           ,@PaymentPlanDate    --PayPlanDate
           ,0   --HontaiGaku8
           ,0   --HontaiGaku10
           ,0   --TaxGaku8
           ,0   --TaxGaku10
           ,@PurchaseGaku + @PurchaseTax    --PayPlanGaku
           ,0   --PayConfirmGaku
           ,0   --PayConfirmFinishedKBN
           ,NULL    --PayCloseDate
           ,NULL    --PayCloseNO
           ,@Program
           ,NULL    --PayConfirmFinishedDate
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
        );
        
        --直前に採番された IDENTITY 列の値を取得する
        DECLARE @PayPlanNO int;
        SET @PayPlanNO = @@IDENTITY;
        
        --伝票番号採番
        EXEC Fnc_GetNumber
            4,             --in伝票種別 4
            @PurchaseDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @PurchaseNO OUTPUT
            ;
        
        IF ISNULL(@PurchaseNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --【D_Purchase】Table転送仕様Ａ
        INSERT INTO [D_Purchase]
           ([PurchaseNO]
           ,[StoreCD]
           ,[PurchaseDate]
           ,[CancelFlg]
           ,[ProcessKBN]
           ,[ReturnsFlg]
           ,[VendorCD]
           ,[CalledVendorCD]
           ,[CalculationGaku]
           ,[AdjustmentGaku]
           ,[PurchaseGaku]
           ,[PurchaseTax]
           ,[TotalPurchaseGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[ExpectedDateFrom]
           ,[ExpectedDateTo]
           ,[InputDate]
           ,[StaffCD]
           ,[PaymentPlanDate]
           ,[PayPlanNO]
           ,[OutputDateTime]
           ,[StockAccountFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     	VALUES
           (@PurchaseNO
           ,@StoreCD
           ,convert(date,@PurchaseDate)
           ,0	--CancelFlg
           ,2	--ProcessKBN
           ,0	--ReturnsFlg
           ,@VendorCD
           ,@VendorCD
           ,@CalculationGaku
           ,@AdjustmentGaku
           ,@PurchaseGaku
           ,@PurchaseTax
           ,@PurchaseGaku + @PurchaseTax	--TotalPurchaseGaku
           ,NULL	--CommentOutStore
           ,@CommentInStore
           ,NULL	--ExpectedDateFrom
           ,NULL	--ExpectedDateTo
           ,@SYSDATETIME	--InputDate
           ,@StaffCD
           ,@PaymentPlanDate
           ,@PayPlanNO
           ,NULL	--OutputDateTime
           ,@StockAccountFlg

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

		--テーブル転送仕様Ｂ
        INSERT INTO [D_PurchaseDetails]
                   ([PurchaseNO]
                   ,[PurchaseRows]
                   ,[DisplayRows]
                   ,[ArrivalNO]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[ItemName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[Remark]
                   ,[PurchaseSu]
                   ,[TaniCD]
                   ,[TaniName]
                   ,[PurchaserUnitPrice]
                   ,[CalculationGaku]
                   ,[AdjustmentGaku]
                   ,[PurchaseGaku]
                   ,[PurchaseTax]
                   ,[TotalPurchaseGaku]
                   ,[CurrencyCD]
                   ,[TaxRitsu]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[ReturnNO]
                   ,[ReturnRows]
                   ,[OrderUnitPrice]
                   ,[OrderNO]
                   ,[OrderRows]
                   ,[DifferenceFlg]
                   ,[DeliveryNo]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @PurchaseNO                         
                   ,tbl.PurchaseRows                       
                   ,tbl.DisplayRows 
                   ,NULL	--ArrivalNO   
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.ItemName
                   ,tbl.ColorName
                   ,tbl.SizeName
                   ,NULL	--Remark
                   ,tbl.PurchaseSu
                   ,tbl.TaniCD
                   ,tbl.TaniName
                   ,tbl.PurchaserUnitPrice
                   ,tbl.CalculationGaku
                   ,tbl.AdjustmentGaku
                   ,tbl.PurchaseGaku
                   ,tbl.PurchaseTax
                   ,tbl.PurchaseGaku + tbl.PurchaseTax --TotalPurchaseGaku
                   ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)	--CurrencyCD
                   ,tbl.TaxRitsu
                   ,tbl.CommentOutStore
                   ,tbl.CommentInStore
                   ,NULL	--ReturnNO
                   ,NULL	--ReturnRows
                   ,0	--OrderUnitPrice
                   ,NULL	--OrderNO
                   ,0	--OrderRows
                   ,1	--DifferenceFlg
                   ,NULL	--DeliveryNo
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;

        --明細数分Insert★
        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            --伝票番号採番
            EXEC Fnc_GetNumber
                21,             --in伝票種別 21
                @PurchaseDate, --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @StockNO OUTPUT
                ;
            
            IF ISNULL(@StockNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_Stock】Insert Table転送仕様Ｆ     
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
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                    WHERE M.StoreCD = @StoreCD
                    AND M.SoukoType = 3     --3:店舗Main倉庫
                    AND M.ChangeDate <= convert(date,@PurchaseDate)
                    ORDER BY M.ChangeDate desc)
                   ,NULL    --RackNO
                   ,NULL    --ArrivalPlanNO
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,0   --  ArrivalYetFLG
                   ,4   --ArrivalPlanKBN
                   ,NULL    --ArrivalPlanDate
                   ,NULL    --ArrivalDate
                   ,tbl.PurchaseSu   --StockSu
                   ,0  --PlanSu
                   ,tbl.PurchaseSu   --AllowableSu
                   ,tbl.PurchaseSu   --AnotherStoreAllowableSu
                   ,0    --ReserveSu
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
                   ,NULL                  
                   ,NULL
              FROM @Table tbl
              WHERE tbl.PurchaseRows = @tblPurchaseRows
              ;
          
            --【D_Warehousing】追加更新（Insert)  Table転送仕様Ｇ
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
            SELECT @PurchaseDate --WarehousingDate
               ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                WHERE M.StoreCD = @StoreCD
                AND M.SoukoType = 3     --3:店舗Main倉庫
                AND M.ChangeDate <= convert(date,@PurchaseDate)
                ORDER BY M.ChangeDate desc) AS SoukoCD
               ,NULL    --RackNO
               ,@StockNO
               ,tbl.JanCD
               ,tbl.AdminNO
               ,tbl.SKUCD
               ,30   --WarehousingKBN	2020/01/27 chg
               ,0  --DeleteFlg
               ,@PurchaseNO  --Number
               ,tbl.PurchaseRows --NumberRow
               ,@VendorCD
               ,NULL    --ToStoreCD
               ,NULL    --ToSoukoCD
               ,NULL    --ToRackNO
               ,NULL    --ToStockNO
               ,NULL    --FromStoreCD
               ,NULL    --FromSoukoCD]
               ,NULL    --FromRackNO
               ,NULL    --CustomerCD
               ,tbl.PurchaseSu   --Quantity
               ,tbl.PurchaserUnitPrice		--UnitPrice		2020/01/27 add
               ,tbl.PurchaseGaku 			--Amount		2020/01/27 add
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              WHERE tbl.PurchaseRows = @tblPurchaseRows
              ;
              

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
            INTO @tblPurchaseRows, @tblUpdateFlg;
        END            --LOOPの終わり
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
    END
        
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
        
        --【D_PurchaseHistory】Insert　Table転送仕様Ｃ　赤
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;
        
        UPDATE D_Purchase
           SET [StoreCD] = @StoreCD                         
              ,[PurchaseDate] = convert(date,@PurchaseDate)
              ,[VendorCD] = @VendorCD
              ,[CalledVendorCD] = @VendorCD
              ,[CalculationGaku] = @CalculationGaku
              ,[AdjustmentGaku] = @AdjustmentGaku
              ,[PurchaseGaku] = @PurchaseGaku
              ,[PurchaseTax] = @PurchaseTax
              ,[TotalPurchaseGaku] = @PurchaseGaku + @PurchaseTax
              ,[CommentInStore]  = @CommentInStore
              ,[InputDate]  = @SYSDATETIME
              ,[StaffCD]         = @StaffCD
              ,[PaymentPlanDate] = @PaymentPlanDate      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE PurchaseNO = @PurchaseNO
           ;
           
        UPDATE [D_PurchaseDetails]
           SET [DisplayRows] = tbl.DisplayRows        
               ,[SKUCD]              = tbl.SKUCD
               ,[AdminNO]            = tbl.AdminNO
               ,[JanCD]              = tbl.JanCD
               ,[ItemName]           = tbl.ItemName
               ,[ColorName]          = tbl.ColorName
               ,[SizeName]           = tbl.SizeName
               ,[PurchaseSu]         = tbl.PurchaseSu
               ,[TaniCD]             = tbl.TaniCD
               ,[TaniName]           = tbl.TaniName
               ,[PurchaserUnitPrice] = tbl.PurchaserUnitPrice
               ,[CalculationGaku]    = tbl.CalculationGaku
               ,[AdjustmentGaku]     = tbl.AdjustmentGaku
               ,[PurchaseGaku]       = tbl.PurchaseGaku
               ,[PurchaseTax]        = tbl.PurchaseTax
               ,[TotalPurchaseGaku]  = tbl.PurchaseGaku + tbl.PurchaseTax --TotalPurchaseGaku
               ,[CurrencyCD]         = (SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)	--CurrencyCD
               ,[TaxRitsu]           = tbl.TaxRitsu
               ,[CommentOutStore]    = tbl.CommentOutStore
               ,[CommentInStore]     = tbl.CommentInStore         
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM D_PurchaseDetails
        INNER JOIN @Table tbl
         ON @PurchaseNO = D_PurchaseDetails.PurchaseNO
         AND tbl.PurchaseRows = D_PurchaseDetails.PurchaseRows
         AND tbl.UpdateFlg = 1
         ;
        
        --追加行
        INSERT INTO [D_PurchaseDetails]
                   ([PurchaseNO]
                   ,[PurchaseRows]
                   ,[DisplayRows]
                   ,[ArrivalNO]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[ItemName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[Remark]
                   ,[PurchaseSu]
                   ,[TaniCD]
                   ,[TaniName]
                   ,[PurchaserUnitPrice]
                   ,[CalculationGaku]
                   ,[AdjustmentGaku]
                   ,[PurchaseGaku]
                   ,[PurchaseTax]
                   ,[TotalPurchaseGaku]
                   ,[CurrencyCD]
                   ,[TaxRitsu]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[ReturnNO]
                   ,[ReturnRows]
                   ,[OrderUnitPrice]
                   ,[OrderNO]
                   ,[OrderRows]
                   ,[DifferenceFlg]
                   ,[DeliveryNo]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @PurchaseNO                         
                   ,tbl.PurchaseRows                       
                   ,tbl.DisplayRows 
                   ,NULL    --ArrivalNO   
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.ItemName
                   ,tbl.ColorName
                   ,tbl.SizeName
                   ,NULL    --Remark
                   ,tbl.PurchaseSu
                   ,tbl.TaniCD
                   ,tbl.TaniName
                   ,tbl.PurchaserUnitPrice
                   ,tbl.CalculationGaku
                   ,tbl.AdjustmentGaku
                   ,tbl.PurchaseGaku
                   ,tbl.PurchaseTax
                   ,tbl.PurchaseGaku + tbl.PurchaseTax --TotalPurchaseGaku
                   ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)   --CurrencyCD
                   ,tbl.TaxRitsu
                   ,tbl.CommentOutStore
                   ,tbl.CommentInStore
                   ,NULL    --ReturnNO
                   ,NULL    --ReturnRows
                   ,0   --OrderUnitPrice
                   ,NULL    --OrderNO
                   ,0   --OrderRows
                   ,1   --DifferenceFlg
                   ,NULL    --DeliveryNo
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
        
        --【D_PayPlan】Update　Table転送仕様Ｅ 支払予定
        UPDATE [D_PayPlan]
           SET 
            [PayeeCD]                = @PayeeCD
           ,[RecordedDate]           = @PurchaseDate   --RecordedDate
           ,[PayPlanDate]            = @PaymentPlanDate    --PayPlanDate
           ,[PayPlanGaku]            = @PurchaseGaku + @PurchaseTax    --PayPlanGaku
           ,[UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
        WHERE [Number] = @PurchaseNO
        AND [DeleteDateTime] IS NULL           
           ;

        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            IF @tblUpdateFlg >= 1
            BEGIN
                --【D_Stock】           Update  Table転送仕様Ｆ②赤
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] - tbl.OldPurchaseSu
                      ,[AllowableSu] = [AllowableSu] - tbl.OldPurchaseSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - tbl.OldPurchaseSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Stock AS DS
                 INNER JOIN @Table tbl
                 ON tbl.StockNO = DS.StockNO
                 AND tbl.PurchaseRows = @tblPurchaseRows
                 WHERE DS.DeleteDateTime IS NULL
                ;

                --【D_Warehousing】Table転送仕様Ｇ②赤
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
               SELECT (CASE WHEN @PurchaseDate >= CONVERT(date,@SYSDATETIME)
               			THEN @PurchaseDate ELSE CONVERT(date,@SYSDATETIME) END) --WarehousingDate
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                    WHERE M.StoreCD = @StoreCD
                    AND M.SoukoType = 3     --3:店舗Main倉庫
                    AND M.ChangeDate <= convert(date,@PurchaseDate)
                    ORDER BY M.ChangeDate desc) AS SoukoCD
                   ,NULL    --RackNO
                   ,DW.StockNO
                   ,DW.JanCD
                   ,DW.AdminNO
                   ,DW.SKUCD
                   ,30   --WarehousingKBN	2020/01/27 chg
                   ,1  --DeleteFlg★
                   ,DW.Number  --Number
                   ,DW.NumberRow --NumberRow
                   ,DW.VendorCD
                   ,NULL    --ToStoreCD
                   ,NULL    --ToSoukoCD
                   ,NULL    --ToRackNO
                   ,NULL    --ToStockNO
                   ,NULL    --FromStoreCD
                   ,NULL    --FromSoukoCD]
                   ,NULL    --FromRackNO
                   ,NULL    --CustomerCD
                   ,DW.Quantity * (-1)   --Quantity
                   ,DW.UnitPrice		--UnitPrice		2020/01/27 add
                   ,DW.Amount * (-1)	--Amount		2020/01/27 add
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL
                 FROM D_Warehousing AS DW
                 INNER JOIN @Table tbl
                 ON tbl.WarehousingNO = DW.WarehousingNO
                 AND tbl.PurchaseRows = @tblPurchaseRows
                 WHERE DW.DeleteDateTime IS NULL
                  ;
                
                IF @tblUpdateFlg = 1
                BEGIN
                    --【D_Stock】           Update  Table転送仕様Ｆ　黒
                    UPDATE [D_Stock] SET
                           [SoukoCD] = (SELECT top 1 M.SoukoCD FROM M_Souko AS M
                                WHERE M.StoreCD = @StoreCD
                                AND M.SoukoType = 3     --3:店舗Main倉庫
                                AND M.ChangeDate <= convert(date,@PurchaseDate)
                                ORDER BY M.ChangeDate desc)
                          ,[SKUCD] = tbl.SKUCD
                          ,[AdminNo] = tbl.AdminNo
                          ,[JANCD] = tbl.JANCD
                          ,[StockSu] = [StockSu] + tbl.PurchaseSu
                          ,[AllowableSu] = [AllowableSu] + tbl.PurchaseSu
                          ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + tbl.PurchaseSu
                          ,[UpdateOperator]     =  @Operator  
                          ,[UpdateDateTime]     =  @SYSDATETIME
                          
                     FROM D_Stock AS DS
                     INNER JOIN @Table tbl
                     ON tbl.StockNO = DS.StockNO
                     AND tbl.PurchaseRows = @tblPurchaseRows
                     WHERE DS.DeleteDateTime IS NULL
                    ;
                
                    --【D_Warehousing】追加更新（Insert)  Table転送仕様Ｇ 黒
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
                    SELECT @PurchaseDate --WarehousingDate
                       ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                        WHERE M.StoreCD = @StoreCD
                        AND M.SoukoType = 3     --3:店舗Main倉庫
                        AND M.ChangeDate <= convert(date,@PurchaseDate)
                        ORDER BY M.ChangeDate desc) AS SoukoCD
                       ,NULL    --RackNO
                       ,tbl.StockNO
                       ,tbl.JanCD
                       ,tbl.AdminNO
                       ,tbl.SKUCD
                       ,30   --WarehousingKBN	2020/01/27 chg
                       ,0  --DeleteFlg
                       ,@PurchaseNO  --Number
                       ,tbl.PurchaseRows --NumberRow
                       ,@VendorCD
                       ,NULL    --ToStoreCD
                       ,NULL    --ToSoukoCD
                       ,NULL    --ToRackNO
                       ,NULL    --ToStockNO
                       ,NULL    --FromStoreCD
                       ,NULL    --FromSoukoCD]
                       ,NULL    --FromRackNO
                       ,NULL    --CustomerCD
                       ,tbl.PurchaseSu   --Quantity               
                       ,tbl.PurchaserUnitPrice		--UnitPrice		2020/01/27 add
                       ,tbl.PurchaseGaku 			--Amount		2020/01/27 add
                       ,@Program  --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL

                      FROM @Table tbl
                      WHERE tbl.PurchaseRows = @tblPurchaseRows
                      ;
                END

                --削除行
                ELSE
                BEGIN
                    UPDATE [D_PurchaseDetails]
                        SET [DeleteOperator]     =  @Operator  
                           ,[DeleteDateTime]     =  @SYSDATETIME
                     WHERE [PurchaseNO] = @PurchaseNO
                     AND [PurchaseRows] = @tblPurchaseRows
                     AND [DeleteDateTime] IS NULL
                     ;               
                     
                    --【D_PickingDetails】Table転送仕様Ｉ
                    UPDATE [D_PickingDetails] SET
                           [DeleteOperator]     =  @Operator  
                          ,[DeleteDateTime]     =  @SYSDATETIME
                     FROM @Table AS tbl
                     WHERE tbl.ReserveNO = D_PickingDetails.ReserveNO
                     AND tbl.PurchaseRows = @tblPurchaseRows
                     ;
                     
                    --【D_Picking】Table転送仕様Ｊ
                    UPDATE [D_Picking] SET
                           [DeleteOperator]     =  @Operator  
                          ,[DeleteDateTime]     =  @SYSDATETIME
                     FROM @Table AS tbl
                     INNER JOIN D_PickingDetails AS DP
                     ON DP.ReserveNO = tbl.ReserveNO
                     WHERE DP.PickingNO = D_Picking.PickingNO
                     AND tbl.PurchaseRows = @tblPurchaseRows
                     AND NOT EXISTS(SELECT A.PickingNO FROM D_PickingDetails AS A
                                WHERE A.PickingNO = D_Picking.PickingNO
                                AND A.DeleteDateTime IS NULL)
                     ;
                     
                    --【D_InstructionDetails】Table転送仕様Ｋ
                    UPDATE [D_InstructionDetails] SET
                           [DeleteOperator]     =  @Operator  
                          ,[DeleteDateTime]     =  @SYSDATETIME
                     FROM @Table AS tbl
                     WHERE tbl.ReserveNO = D_InstructionDetails.ReserveNO
                     AND tbl.PurchaseRows = @tblPurchaseRows
                     ;
                     
                    --【D_Instruction】Table転送仕様Ｌ
                    UPDATE [D_Instruction] SET
                           [DeleteOperator]     =  @Operator  
                          ,[DeleteDateTime]     =  @SYSDATETIME
                     FROM @Table AS tbl
                     INNER JOIN D_InstructionDetails AS DI
                     ON DI.ReserveNO = tbl.ReserveNO
                     WHERE DI.InstructionNO = D_Instruction.InstructionNO
                     AND tbl.PurchaseRows = @tblPurchaseRows
                     AND NOT EXISTS(SELECT A.InstructionNO FROM D_InstructionDetails AS A
                                WHERE A.InstructionNO = D_Instruction.InstructionNO
                                AND A.DeleteDateTime IS NULL)
                     ;
                END
            END
	        
            --追加行
            ELSE IF @tblUpdateFlg = 0
            BEGIN
                --伝票番号採番
                EXEC Fnc_GetNumber
                    21,             --in伝票種別 21
                    @PurchaseDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @StockNO OUTPUT
                    ;
                
                IF ISNULL(@StockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Stock】Insert Table転送仕様Ｆ     
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
                       ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                        WHERE M.StoreCD = @StoreCD
                        AND M.SoukoType = 3     --3:店舗Main倉庫
                        AND M.ChangeDate <= convert(date,@PurchaseDate)
                        ORDER BY M.ChangeDate desc)
                       ,NULL    --RackNO
                       ,NULL    --ArrivalPlanNO
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,0   --  ArrivalYetFLG
                       ,4   --ArrivalPlanKBN
                       ,NULL    --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,tbl.PurchaseSu   --StockSu
                       ,0  --PlanSu
                       ,tbl.PurchaseSu   --AllowableSu
                       ,tbl.PurchaseSu   --AnotherStoreAllowableSu
                       ,0    --ReserveSu
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
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.PurchaseRows = @tblPurchaseRows
                  ;
              
                --【D_Warehousing】追加更新（Insert)  Table転送仕様Ｇ
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
                SELECT @PurchaseDate --WarehousingDate
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                    WHERE M.StoreCD = @StoreCD
                    AND M.SoukoType = 3     --3:店舗Main倉庫
                    AND M.ChangeDate <= convert(date,@PurchaseDate)
                    ORDER BY M.ChangeDate desc) AS SoukoCD
                   ,NULL    --RackNO
                   ,@StockNO
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,30   --WarehousingKBN	2020/01/27 chg
                   ,0  --DeleteFlg
                   ,@PurchaseNO  --Number
                   ,tbl.PurchaseRows --NumberRow
                   ,@VendorCD
                   ,NULL    --ToStoreCD
                   ,NULL    --ToSoukoCD
                   ,NULL    --ToRackNO
                   ,NULL    --ToStockNO
                   ,NULL    --FromStoreCD
                   ,NULL    --FromSoukoCD]
                   ,NULL    --FromRackNO
                   ,NULL    --CustomerCD
                   ,tbl.PurchaseSu   --Quantity
                   ,tbl.PurchaserUnitPrice		--UnitPrice		2020/01/27 add
                   ,tbl.PurchaseGaku 			--Amount		2020/01/27 add
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  WHERE tbl.PurchaseRows = @tblPurchaseRows
                  ;
            
            END


        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;

        END		--LOOPの終わり
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
    	
    	--【D_PurchaseHistory】Insert　Table転送仕様Ｃ　赤
    	EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;
         
        UPDATE [D_Purchase]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [PurchaseNO] = @PurchaseNO
         ;

    END
    
    IF @OperateMode <= 2    --新規・修正時
    BEGIN
    	--【D_PurchaseHistory】Insert　Table転送仕様Ｃ　黒
    	EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,1  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;

    END
    ELSE    --削除
    BEGIN
        UPDATE [D_PurchaseDetails]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [PurchaseNO] = @PurchaseNO
         AND [DeleteDateTime] IS NULL
         ;

        --【D_PayPlan】Delete　Table転送仕様Ｅ② 支払予定
        UPDATE [D_PayPlan]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [Number] = @PurchaseNO
         AND [DeleteDateTime] IS NULL
         ;
         
        --【D_Stock】           Update  Table転送仕様Ｆ②
        UPDATE D_Stock 
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         FROM D_Stock AS DS
         INNER JOIN @Table tbl
         ON tbl.StockNO = DS.StockNO
         WHERE DS.DeleteDateTime IS NULL
        ;
        
        --【D_Warehousing】Table転送仕様Ｇ②赤
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
       SELECT (CASE WHEN @PurchaseDate >= CONVERT(date,@SYSDATETIME)
                THEN @PurchaseDate ELSE CONVERT(date,@SYSDATETIME) END) --WarehousingDate
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.SoukoType = 3     --3:店舗Main倉庫
            AND M.ChangeDate <= convert(date,@PurchaseDate)
            ORDER BY M.ChangeDate desc) AS SoukoCD
           ,NULL    --RackNO
           ,@StockNO
           ,DW.JanCD
           ,DW.AdminNO
           ,DW.SKUCD
           ,30   --WarehousingKBN
           ,1  --DeleteFlg★
           ,DW.Number  --Number
           ,DW.NumberRow --NumberRow
           ,DW.VendorCD
           ,NULL    --ToStoreCD
           ,NULL    --ToSoukoCD
           ,NULL    --ToRackNO
           ,NULL    --ToStockNO
           ,NULL    --FromStoreCD
           ,NULL    --FromSoukoCD]
           ,NULL    --FromRackNO
           ,NULL    --CustomerCD
           ,DW.Quantity * (-1)   --Quantity
           ,DW.UnitPrice		--UnitPrice		2020/01/27 add
           ,DW.Amount * (-1)	--Amount		2020/01/27 add
           ,@Program  --Program
           
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL
         FROM D_Warehousing AS DW
         INNER JOIN @Table tbl
         ON tbl.WarehousingNO = DW.WarehousingNO
         AND tbl.PurchaseRows = @tblPurchaseRows
         WHERE DW.DeleteDateTime IS NULL
          ;

		--各明細行について
        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN            
            --【D_PickingDetails】Table転送仕様Ｉ
            UPDATE [D_PickingDetails] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
             FROM @Table AS tbl
             WHERE tbl.ReserveNO = D_PickingDetails.ReserveNO
             AND tbl.PurchaseRows = @tblPurchaseRows
             ;
             
            --【D_Picking】Table転送仕様Ｊ
            UPDATE [D_Picking] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
             FROM @Table AS tbl
             INNER JOIN D_PickingDetails AS DP
             ON DP.ReserveNO = tbl.ReserveNO
             WHERE DP.PickingNO = D_Picking.PickingNO
             AND tbl.PurchaseRows = @tblPurchaseRows
             AND NOT EXISTS(SELECT A.PickingNO FROM D_PickingDetails AS A
                        WHERE A.PickingNO = D_Picking.PickingNO
                        AND A.DeleteDateTime IS NULL)
             ;
             
            --【D_InstructionDetails】Table転送仕様Ｋ
            UPDATE [D_InstructionDetails] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
             FROM @Table AS tbl
             WHERE tbl.ReserveNO = D_InstructionDetails.ReserveNO
             AND tbl.PurchaseRows = @tblPurchaseRows
             ;
             
            --【D_Instruction】Table転送仕様Ｌ
            UPDATE [D_Instruction] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
             FROM @Table AS tbl
             INNER JOIN D_InstructionDetails AS DI
             ON DI.ReserveNO = tbl.ReserveNO
             WHERE DI.InstructionNO = D_Instruction.InstructionNO
             AND tbl.PurchaseRows = @tblPurchaseRows
             AND NOT EXISTS(SELECT A.InstructionNO FROM D_InstructionDetails AS A
             			WHERE A.InstructionNO = D_Instruction.InstructionNO
             			AND A.DeleteDateTime IS NULL)
             ;
             
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;

        END		--LOOPの終わり
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
    END
	
    --処理履歴データへ更新
    SET @KeyItem = @PurchaseNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutPurchaseNO = @PurchaseNO;
    
--<<OWARI>>
  return @W_ERR;

END


