IF OBJECT_ID ( 'PRC_MarkDownNyuuryoku', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_MarkDownNyuuryoku]
GO
IF EXISTS (select * from sys.table_types where name = 'T_MarkDown')
    Drop TYPE dbo.[T_MarkDown]
GO

--  ======================================================================
--       Program Call    マークダウン入力
--       Program ID      MarkDownNyuuryoku
--       Create date:    2020.06.20
--    ======================================================================

CREATE TYPE T_MarkDown AS TABLE
    (
    [MarkDownRows] [int],    
    [SKUCD] [varchar](30) ,
    [AdminNO] [int] ,
    [JanCD] [varchar](13) ,
    [MakerItem] [varchar](50) NULL, 
    [ItemName] [varchar](80) NULL,
    [ColorName] [varchar](20) ,
    [SizeName] [varchar](20) ,
    [PriceOutTax] [money] ,
    [EvaluationPrice] [money] ,
    [StockSu] [int] ,
    [CalculationSu] [int] ,
    [Rate] [decimal](5, 2) ,
    [MarkDownUnitPrice] [money],
    [MarkDownGaku] [money],
    [PurchaseTax] [money],
    [TaxRate] [decimal](3, 1) ,
    [TaniCD] [varchar](2) ,
    [TaniName] [varchar](10),
    [MDPurchaseRows] [int], 
    [PurchaseRows] [int], 
    [InsertOperator] varchar(10),
    [InsertDateTime] datetime
    )
GO

--********************************************--
--                                            --
--                 データ更新                 --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_MarkDownNyuuryoku
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @ChkResult   varchar(1),
    @ChangeDate  varchar(10),
    @MarkDownNO   varchar(11),
    @StoreCD   varchar(4),
    @SoukoCD   varchar(6),
    @ReplicaNO  int,
    @ReplicaDate  date,
    @ReplicaTime  time(7),
    @StaffCD   varchar(10),
    @VendorCD   varchar(13),
    @CostingDate varchar(10),
    @UnitPriceDate varchar(10),
    @ExpectedPurchaseDate varchar(10),
    @PurchaseDate  varchar(10),
    @Comment varchar(300) ,
    @MDPurchaseNO  varchar(11),
    @PurchaseNO  varchar(11),
    @PayeeCD  varchar(11),
    @PurchaseGaku money,
    @PurchaseTax money,
    @TotalPurchaseGaku money,
    @TaxGaku8 money,
    @TaxGaku10 money,
    @HontaiGaku8 money,
    @HontaiGaku10 money,
    
    @Table  T_MarkDown READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutMarkDownNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @SYSDATE date;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @Program varchar(100); 
    DECLARE @UpdPurchaseNO  varchar(11);
    DECLARE @PaymentPlanDate varchar(10);
    DECLARE @PayPlanNO int;
    DECLARE @IsNew bit;
    DECLARE @W_CNT int;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(date, @SYSDATETIME);
    SET @Program = 'MarkDownNyuuryoku';   
    SET @UpdPurchaseNO = NULL;
    SET @IsNew = 0;
    
    --新規・変更--
    IF @OperateMode <> '3'
    BEGIN
        IF @OperateMode = '1'
            SET @OperateModeNm = '新規';
        ELSE IF @OperateMode = 2
            SET @OperateModeNm = '変更';
    
        -- �@仕入番号採番
        -- 新規 または 変更で予定→結果 or 結果→予定に変更された場合
        IF @OperateMode = '1' OR (ISNULL(@PurchaseNO,'') = '' AND @ChkResult = '1') OR (ISNULL(@MDPurchaseNO,'') = '' AND @ChkResult = '0')
        BEGIN
            
            --仕入番号採番
            EXEC Fnc_GetNumber
                4,             --in伝票種別 4
                @ChangeDate,   --in基準日
                @StoreCD,      --in店舗CD
                @Operator,
                @UpdPurchaseNO    OUTPUT
                ;
            
            IF ISNULL(@UpdPurchaseNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            -- 仕入明細を追加した場合、true
            SET @IsNew = 1;
        END
                
        SET @PaymentPlanDate = NULL;
        SET @PayPlanNO = 0;
        
        -- �A支払予定更新
        IF @ChkResult = '1'
        BEGIN
            --支払予定日取得
            EXEC Fnc_PlanDate_SP
                1,             --in回収支払区分 1:支払
                @PayeeCD,      --in取引先CD
                @PurchaseDate, --in計上日
                0,             --in帳端区分
                @PaymentPlanDate    OUTPUT
                ;
        
            IF ISNULL(@PurchaseNO,'') = ''
            BEGIN
                --【D_PayPlan】Table転送仕様G 支払予定 Insert
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
                   ,@UpdPurchaseNO --Number
                   ,@StoreCD
                   ,@PayeeCD
                   ,@PurchaseDate       --RecordedDate
                   ,@PaymentPlanDate    --PayPlanDate
                   ,@HontaiGaku8        --HontaiGaku8
                   ,@HontaiGaku10       --HontaiGaku10
                   ,@TaxGaku8           --TaxGaku8
                   ,@TaxGaku10          --TaxGaku10
                   ,@TotalPurchaseGaku  --PayPlanGaku
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
                SET @PayPlanNO = @@IDENTITY;
            END
            ELSE
            BEGIN
                UPDATE D_PayPlan
                   SET [PayeeCD] = @PayeeCD
                      ,[RecordedDate] = @PurchaseDate
                      ,[PayPlanDate] = @PaymentPlanDate
                      ,[HontaiGaku8] = @HontaiGaku8
                      ,[HontaiGaku10] = @HontaiGaku10
                      ,[TaxGaku8] = @TaxGaku8
                      ,[TaxGaku10] = @TaxGaku10
                      ,[PayPlanGaku] = @TotalPurchaseGaku                         
                      ,[UpdateOperator] = @Operator
                      ,[UpdateDateTime] = @SYSDATETIME
                 WHERE [Number] = @PurchaseNO
                 ;
            END
                
        END
        
        -- �B仕入・仕入明細更新
        -- 新規 または 変更で予定→結果 or 結果→予定に変更された場合
        IF @IsNew = 1
        BEGIN
            --【D_Purchase】Table転送仕様C/E Insert
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
               (@UpdPurchaseNO
               ,@StoreCD
               ,convert(date,@ChangeDate)
               ,0   --CancelFlg
               ,CASE WHEN @ChkResult = '0' THEN 5 ELSE 4 END   --ProcessKBN
               ,0   --ReturnsFlg
               ,@VendorCD     
               ,@VendorCD
               ,@PurchaseGaku
               ,0
               ,@PurchaseGaku
               ,@PurchaseTax
               ,@TotalPurchaseGaku   --TotalPurchaseGaku
               ,NULL --CommentOutStore
               ,@Comment
               ,NULL   --ExpectedDateFrom
               ,NULL   --ExpectedDateTo
               ,@SYSDATETIME    --InputDate
               ,@StaffCD
               ,@PaymentPlanDate
               ,@PayPlanNO
               ,NULL    --OutputDateTime
               ,0       --StockAccountFlg
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
               );     
               
               IF @ChkResult = '0'
                   SET @MDPurchaseNO = @UpdPurchaseNO;
               ELSE
                   SET @PurchaseNO = @UpdPurchaseNO;
        END        
        --  変更で予定/結果が変更なしの場合
        ELSE IF @OperateMode = '2'        
        BEGIN
            IF @ChkResult = '0'
                SET @UpdPurchaseNO = @MDPurchaseNO;
            ELSE
                SET @UpdPurchaseNO = @PurchaseNO;
            
            --【D_Purchase】Table転送仕様C Update
            UPDATE [D_Purchase]
               SET [PurchaseDate] = convert(date,@ChangeDate)
                  ,[VendorCD] = @VendorCD
                  ,[CalledVendorCD] = @VendorCD
                  ,[CalculationGaku] = @PurchaseGaku
                  ,[PurchaseGaku] = @PurchaseGaku
                  ,[PurchaseTax] = @PurchaseTax
                  ,[TotalPurchaseGaku] = @TotalPurchaseGaku
                  ,[CommentInStore] = @Comment
                  ,[StaffCD] = @StaffCD
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @UpdPurchaseNO;
             
            --【D_PurchaseDetails】Table転送仕様D Delete
            DELETE [D_PurchaseDetails]
            WHERE PurchaseNO = @UpdPurchaseNO;     
            
        END
        
        --【D_PurchaseDetails】Table転送仕様D Insert
        INSERT INTO [D_PurchaseDetails]
               ([PurchaseNO]
               ,[PurchaseRows]
               ,[DisplayRows]
               ,[ArrivalNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[MakerItem]
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
               ,[StockNO]
               ,[DifferenceFlg]
               ,[DeliveryNo]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
         SELECT @UpdPurchaseNO                         
               ,tbl.MarkDownRows                       
               ,tbl.MarkDownRows 
               ,NULL    --ArrivalNO
               ,tbl.SKUCD
               ,tbl.AdminNO
               ,tbl.JanCD
               ,tbl.MakerItem
               ,tbl.ItemName
               ,tbl.ColorName
               ,tbl.SizeName
               ,NULL  --Remark
               ,tbl.CalculationSu
               ,tbl.TaniCD
               ,tbl.TaniName
               ,tbl.MarkDownUnitPrice - tbl.EvaluationPrice
               ,tbl.MarkDownGaku
               ,0
               ,tbl.MarkDownGaku
               ,tbl.PurchaseTax 
               ,(tbl.MarkDownGaku + tbl.PurchaseTax)         --TotalPurchaseGaku
               ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)   --CurrencyCD
               ,tbl.TaxRate
               ,NULL      --CommentOutStore
               ,NULL      --CommentInStore
               ,NULL      --ReturnNO
               ,0         --ReturnRows
               ,0         --OrderUnitPrice
               ,NULL      --OrderNO
               ,0         --OrderRows
               ,NULL      --StockNO
               ,1         --DifferenceFlg★
               ,NULL      --DeliveryNo
               ,CASE WHEN tbl.InsertOperator IS NULL THEN @Operator ELSE tbl.InsertOperator END
               ,CASE WHEN tbl.InsertDateTime IS NULL THEN @SYSDATETIME ELSE tbl.InsertDateTime END 
               ,@Operator  
               ,@SYSDATETIME
          FROM @Table tbl
          ;
          
        --�CJAN発注単価マスタ
        IF @ChkResult = '1'
        BEGIN
            IF ISNULL(@MarkDownNO,'') <> ''
            BEGIN
                -- 【M_JANOrderPrice】Table転送仕様J Delete
                DELETE M_JANOrderPrice
                FROM   M_JANOrderPrice MJ
                INNER JOIN D_MarkDown DM ON DM.VendorCD = MJ.VendorCD
                                        AND DM.UnitPriceDate = MJ.ChangeDate
                INNER JOIN D_MarkDownDetails DMD ON DM.MarkDownNO = DMD.MarkDownNO
                                                AND DMD.AdminNO = MJ.AdminNO                                            
                WHERE DM.MarkDownNO = @MarkDownNO
                ;   
            END
            
            -- 【M_JANOrderPrice】Table転送仕様J Insert
            INSERT INTO M_JANOrderPrice
               ([VendorCD]
               ,[StoreCD]
               ,[AdminNO]
               ,[ChangeDate]
               ,[SKUCD]
               ,[Rate]
               ,[PriceWithoutTax]
               ,[Remarks]
               ,[DeleteFlg]
               ,[UsedFlg]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
            SELECT
                @VendorCD
               ,@StoreCD
               ,tbl.AdminNO
               ,@UnitPriceDate
               ,tbl.SKUCD
               ,tbl.Rate
               ,tbl.MarkDownUnitPrice
               ,NULL
               ,0
               ,1
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
            FROM @Table tbl 
            WHERE NOT EXISTS ( SELECT VendorCD FROM M_JANOrderPrice MJ
                                WHERE MJ.VendorCD = @VendorCD
                                  AND MJ.StoreCD = @StoreCD
                                  AND MJ.ChangeDate = @UnitPriceDate
                                  AND tbl.AdminNO = MJ.AdminNO
                              )
            ;
            
            -- 【M_JANOrderPrice】Table転送仕様J Update
            UPDATE M_JANOrderPrice
            SET SKUCD = tbl.SKUCD
               ,Rate = tbl.Rate
               ,PriceWithoutTax = tbl.MarkDownUnitPrice
               ,DeleteFlg = 0
               ,UsedFlg = 1
               ,UpdateOperator = @Operator
               ,UpdateDateTime = @SYSDATETIME
            FROM @Table AS tbl
            INNER JOIN M_JANOrderPrice AS MJ ON tbl.AdminNO = MJ.AdminNO
            WHERE MJ.VendorCD = @VendorCD
              AND MJ.StoreCD = @StoreCD
              AND MJ.ChangeDate = @UnitPriceDate
            ;
        END
        
        --�Dマークダウン・マークダウン明細
        IF @OperateMode = '1'
        BEGIN
            --伝票番号採番
            EXEC Fnc_GetNumber
                31,             --in伝票種別 
                @CostingDate,   --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @MarkDownNO OUTPUT
                ;
            
            IF ISNULL(@MarkDownNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_MarkDown】Table転送仕様A Insert
            INSERT INTO [dbo].[D_MarkDown]
               ([MarkDownNO]
               ,[StoreCD]
               ,[SoukoCD]
               ,[MarkDownDate]
               ,[ReplicaNO]
               ,[ReplicaDate]
               ,[ReplicaTime]
               ,[StaffCD]
               ,[VendorCD]
               ,[CostingDate]
               ,[UnitPriceDate]
               ,[ExpectedPurchaseDate]
               ,[PurchaseDate]
               ,[Comment]
               ,[MDPurchaseNO]
               ,[PurchaseNO]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            VALUES
               (@MarkDownNO
               ,@StoreCD
               ,@SoukoCD
               ,@SYSDATE
               ,@ReplicaNO
               ,@ReplicaDate
               ,@ReplicaTime
               ,@StaffCD
               ,@VendorCD
               ,@CostingDate
               ,@UnitPriceDate
               ,@ExpectedPurchaseDate
               ,@PurchaseDate
               ,@Comment
               ,@MDPurchaseNO
               ,@PurchaseNO
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
               );
        END
        ELSE IF @OperateMode = '2'
        BEGIN
            --【D_MarkDown】Table転送仕様A Update
            UPDATE D_MarkDown
               SET [ReplicaNO] = @ReplicaNO
                  ,[ReplicaDate] = @ReplicaDate
                  ,[ReplicaTime] = @ReplicaTime
                  ,[StaffCD] = @StaffCD
                  ,[VendorCD] = @VendorCD
                  ,[CostingDate] = @CostingDate
                  ,[UnitPriceDate] = @UnitPriceDate
                  ,[ExpectedPurchaseDate] = @ExpectedPurchaseDate
                  ,[PurchaseDate] = @PurchaseDate
                  ,[Comment] = @Comment
                  ,[MDPurchaseNO] = @MDPurchaseNO
                  ,[PurchaseNO] = @PurchaseNO
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            WHERE [MarkDownNO] = @MarkDownNO
            ;
            
            --【D_MarkDownDetails】Table転送仕様B Delete
            DELETE [D_MarkDownDetails]
            WHERE MarkDownNO = @MarkDownNO
            ;
        END
        
        --【D_MarkDownDetails】Table転送仕様B Insert
        INSERT INTO D_MarkDownDetails
           ([MarkDownNO]
           ,[MarkDownRows]
           ,[MDPurchaseNO]
           ,[MDPurchaseRows]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[TaniCD]
           ,[PriceOutTax]
           ,[EvaluationPrice]
           ,[StockSu]
           ,[CalculationSu]
           ,[Rate]
           ,[MarkDownUnitPrice]
           ,[MarkDownGaku]
           ,[Remark]
           ,[PurchaseNO]
           ,[PurchaseRows]
           ,[PurchaseSu]
           ,[PurchaserUnitPrice]
           ,[PurchaseGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime]
           )
        SELECT
            @MarkDownNO
           ,tbl.MarkDownRows
           ,@MDPurchaseNO
           ,tbl.MDPurchaseRows
           ,tbl.SKUCD
           ,tbl.AdminNO
           ,tbl.JanCD
           ,tbl.TaniCD
           ,tbl.PriceOutTax
           ,tbl.EvaluationPrice
           ,tbl.StockSu
           ,CASE WHEN @ChkResult = '0' THEN tbl.CalculationSu ELSE 0 END
           ,tbl.Rate
           ,CASE WHEN @ChkResult = '0' THEN tbl.MarkDownUnitPrice ELSE 0 END
           ,CASE WHEN @ChkResult = '0' THEN tbl.MarkDownGaku ELSE 0 END
           ,NULL              --Remark
           ,@PurchaseNO       --PurchaseNO
           ,tbl.PurchaseRows  --PurchaseRows
           ,CASE WHEN @ChkResult = '1' THEN tbl.CalculationSu ELSE 0 END
           ,CASE WHEN @ChkResult = '1' THEN tbl.MarkDownUnitPrice ELSE 0 END
           ,CASE WHEN @ChkResult = '1' THEN tbl.MarkDownGaku ELSE 0 END
           ,ISNULL(tbl.InsertOperator,@Operator)
           ,ISNULL(tbl.InsertDateTime,@SYSDATETIME)
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
        FROM @Table tbl    
        
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        -- 【M_JANOrderPrice】Table転送仕様J Update
        DELETE M_JANOrderPrice
        FROM   M_JANOrderPrice MJ
        INNER JOIN D_MarkDown DM ON DM.VendorCD = MJ.VendorCD
                                AND DM.UnitPriceDate = MJ.ChangeDate
        INNER JOIN D_MarkDownDetails DMD ON DM.MarkDownNO = DMD.MarkDownNO
                                        AND DMD.AdminNO = MJ.AdminNO                                            
        WHERE DM.MarkDownNO = @MarkDownNO
        ;   
        
        --【D_MarkDown】Table転送仕様A Update
        UPDATE D_MarkDown
           SET [DeleteOperator] = @Operator
              ,[DeleteDateTime] = @SYSDATETIME
        WHERE [MarkDownNO] = @MarkDownNO
        ;
        
        --【D_MarkDown】Table転送仕様B Update
        UPDATE D_MarkDownDetails
           SET [DeleteOperator] = @Operator
              ,[DeleteDateTime] = @SYSDATETIME
        WHERE [MarkDownNO] = @MarkDownNO
        ;
         
        IF @MDPurchaseNO <> ''
        BEGIN
            --【D_Purchase】Table転送仕様C Update
            UPDATE [D_Purchase]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @MDPurchaseNO;
             
            --【D_PurchaseDetails】Table転送仕様D Update
            UPDATE [D_PurchaseDetails]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            WHERE PurchaseNO = @MDPurchaseNO;        
        END
            
        IF @PurchaseNO <> ''
        BEGIN
            --【D_Purchase】Table転送仕様E Update
            UPDATE [D_Purchase]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @PurchaseNO;
             
            --【D_PurchaseDetails】Table転送仕様F Update
            UPDATE [D_PurchaseDetails]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            WHERE PurchaseNO = @PurchaseNO;  
            
            --【D_PayPlan】Table転送仕様G 支払予定 Update
            UPDATE D_PayPlan
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE [Number] = @PurchaseNO
             ;
        
        END
    END
    
    
    --処理履歴データへ更新
    SET @KeyItem = @VendorCD + ' ' + @UnitPriceDate;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutMarkDownNO = @MarkDownNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
