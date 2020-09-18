 BEGIN TRY 
 Drop Procedure dbo.[PRC_ShiireNyuuryokuF]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--CREATE TYPE T_ShiireF AS TABLE
--    (
--    [PurchaseRows] [int],
--    [DisplayRows] [int],
    
--    [ArrivalNO] [varchar](11),
--    [SKUCD] [varchar](30) ,
--    [AdminNO] [int] ,
--    [JanCD] [varchar](13) ,
--    [MakerItem] [varchar](50) ,
--    [ItemName] [varchar](80) NULL,
--    [ColorName] [varchar](20) ,
--    [SizeName] [varchar](20) ,
    
--    [PurchaseSu] [int] ,
--    [OldPurchaseSu] [int] ,
--    [TaniCD] [varchar](2) ,
--    [TaniName] [varchar](10) ,
--    [PurchaserUnitPrice] [money] ,
--    [CalculationGaku] [money] ,
--    [AdjustmentGaku] [money] ,
--    [PurchaseGaku] [money] ,
--    [PurchaseTax] [money] ,
--    [TaxRitsu] [int],
--    [CommentOutStore] [varchar](80) ,
--    [CommentInStore] [varchar](80) ,
--    [OrderUnitPrice] [money] ,
--    [OrderNO] [varchar](11) ,
--    [OrderRows] [int] ,
--    [DifferenceFlg] [tinyint],
--    [DeliveryNo] [int],
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE PRC_ShiireNyuuryokuF
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @PurchaseNO   varchar(11),
    @StoreCD   varchar(4),
    @PurchaseDate  varchar(10),
    @PaymentPlanDate varchar(10),
    @StaffCD   varchar(10),
    @VendorCD   varchar(13),
    @CalledVendorCD   varchar(13),
    @PayeeCD  varchar(13),
    @CalculationGaku money,
    @AdjustmentGaku money,
    @PurchaseGaku money,
    @PurchaseTax money,
    @CommentInStore varchar(180) ,

    @Table  T_ShiireF READONLY,
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @Program = 'ShiireNyuuryokuFromNyuuka'; 


    DECLARE CUR_PurArr CURSOR FOR
        SELECT DA.ArrivalSu - DA.PurchaseSu AS Mishiire
              ,DP.PurchaseSu
              ,DP.PurchaseRows
              ,DA.ArrivalNO
              ,DA.ArrivalRows
        FROM D_PurchaseDetails AS DP
        INNER JOIN D_ArrivalDetails AS DA
        ON DA.ArrivalNO = DP.ArrivalNO
        AND DA.DeleteDateTime IS NULL
        WHERE DP.PurchaseNO = @PurchaseNO
        ORDER BY DP.PurchaseRows, DA.ArrivalNO, DA.ArrivalRows
        ;

    DECLARE @Mishiire int;
    DECLARE @PurchaseSu int;
    DECLARE @PurchaseRows int;
    DECLARE @OldPurchaseRows int;
    DECLARE @ArrivalNO varchar(11);
    DECLARE @ArrivalRows int;
    DECLARE @UpdSu int;
    DECLARE @InputSu int;
    
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
           ,1	--ProcessKBN
           ,0	--ReturnsFlg
           ,@VendorCD
           ,@CalledVendorCD
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
           ,0	--StockAccountFlg

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

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
                   ,[DifferenceFlg]
                   ,[DeliveryNo]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @PurchaseNO                         
                   ,tbl.PurchaseRows                       
                   ,tbl.DisplayRows 

                   ,tbl.ArrivalNO
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.MakerItem
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
                   ,tbl.OrderUnitPrice
                   ,tbl.OrderNO
                   ,tbl.OrderRows
                   ,tbl.DifferenceFlg
                   ,tbl.DeliveryNo
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;

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
        
        --【D_Delivery】Update	Table転送仕様Ｆ	赤	納品書
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;

        --D_ArrivalDetails  Update Table転送仕様Ｉ  赤  入荷明細
        UPDATE D_ArrivalDetails SET
           [PurchaseSu] = D_ArrivalDetails.[PurchaseSu] - DP.PurchaseSu
          ,[UpdateOperator] =  @Operator  
          ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_PurchaseArrival AS DP     
        WHERE DP.ArrivalNO = D_ArrivalDetails.ArrivalNO
        AND DP.ArrivalRows = D_ArrivalDetails.ArrivalRows
        AND DP.PurchaseNO = @PurchaseNO
        ;

        --D_PurchaseArrival     Delete 仕入入荷明細
        Delete From D_PurchaseArrival
        WHERE PurchaseNO = @PurchaseNO
        ;
        
        --【D_Warehousing】Insert	Table転送仕様Ｇ②	赤	2020/01/27 add
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
       SELECT DH.PurchaseDate --WarehousingDate
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.SoukoType = 3     --3:店舗Main倉庫
            AND M.ChangeDate <= DH.PurchaseDate
            ORDER BY M.ChangeDate desc) AS SoukoCD
           ,NULL    --RackNO
           ,NULL	--StockNO
           ,DM.JanCD
           ,DM.AdminNO
           ,DM.SKUCD
           ,30   --WarehousingKBN	2020/01/27 chg
           ,1  --DeleteFlg★
           ,DM.PurchaseNO  --Number
           ,DM.PurchaseRows --NumberRow
           ,DH.VendorCD
           ,NULL    --ToStoreCD
           ,NULL    --ToSoukoCD
           ,NULL    --ToRackNO
           ,NULL    --ToStockNO
           ,NULL    --FromStoreCD
           ,NULL    --FromSoukoCD]
           ,NULL    --FromRackNO
           ,NULL    --CustomerCD
           ,DM.PurchaseSu * (-1)   --Quantity
           ,DM.PurchaserUnitPrice		--UnitPrice		2020/01/27 add
           ,DM.CalculationGaku * (-1)	--Amount		2020/01/27 add
           ,@Program  --Program
           
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL
        FROM D_PurchaseDetails AS DM
        INNER JOIN D_Purchase AS DH
        ON DH.PurchaseNO = DM.PurchaseNO
        WHERE DM.PurchaseNO = @PurchaseNO
          AND DM.DeleteDateTime IS NULL
          ;
   
        --【D_Arrival】Update	Table転送仕様Ｈ	赤	2020/01/27 add
        UPDATE [D_Arrival]
           SET [PurchaseSu] = D_Arrival.[PurchaseSu] - tbl.OldPurchaseSu
              ,[UpdateOperator] =  @Operator  
              ,[UpdateDateTime] =  @SYSDATETIME 
          FROM @Table AS tbl
         WHERE tbl.ArrivalNO = D_Arrival.ArrivalNO
         AND D_Arrival.DeleteDateTime IS NULL
         AND tbl.UpdateFlg = 1
         ;
         
        UPDATE D_Purchase
           SET [StoreCD] = @StoreCD                         
              ,[PurchaseDate] = convert(date,@PurchaseDate)
              ,[VendorCD] = @VendorCD
              ,[CalledVendorCD] = @CalledVendorCD
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
               ,[ArrivalNO]          = tbl.ArrivalNO
               ,[SKUCD]              = tbl.SKUCD
               ,[AdminNO]            = tbl.AdminNO
               ,[JanCD]              = tbl.JanCD
               ,[MakerItem]          = tbl.MakerItem
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
               ,[OrderUnitPrice]     = tbl.OrderUnitPrice
               ,[OrderNO]            = tbl.OrderNO
               ,[OrderRows]          = tbl.OrderRows
               ,[DifferenceFlg]      = tbl.DifferenceFlg
               ,[DeliveryNo]         = tbl.DeliveryNo          
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM D_PurchaseDetails
        INNER JOIN @Table tbl
         ON @PurchaseNO = D_PurchaseDetails.PurchaseNO
         AND tbl.PurchaseRows = D_PurchaseDetails.PurchaseRows
         AND tbl.UpdateFlg = 1
         ;
        
        --【D_PayPlan】Update　Table転送仕様Ｅ 支払予定
        UPDATE [D_PayPlan]
           SET 
            [PayeeCD]         = @PayeeCD
           ,[RecordedDate]    = @PurchaseDate   --RecordedDate
           ,[PayPlanDate]     = @PaymentPlanDate    --PayPlanDate
           ,[PayPlanGaku]     = @PurchaseGaku + @PurchaseTax    --PayPlanGaku
           ,[UpdateOperator]  = @Operator  
           ,[UpdateDateTime]  = @SYSDATETIME
        WHERE [Number] = @PurchaseNO
        AND [DeleteDateTime] IS NULL           
           ;
           
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

        --【D_Delivery】Update	Table転送仕様Ｆ	赤	納品書
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] - tbl.PurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;
         
        --D_ArrivalDetails  Update Table転送仕様Ｉ  赤  入荷明細
        UPDATE D_ArrivalDetails SET
           [PurchaseSu] = D_ArrivalDetails.[PurchaseSu] - DP.PurchaseSu
          ,[UpdateOperator] =  @Operator  
          ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_PurchaseArrival AS DP     
        WHERE DP.ArrivalNO = D_ArrivalDetails.ArrivalNO
        AND DP.ArrivalRows = D_ArrivalDetails.ArrivalRows
        AND DP.PurchaseNO = @PurchaseNO
        ;

        --D_PurchaseArrival     Delete 仕入入荷明細
        Delete From D_PurchaseArrival
        WHERE PurchaseNO = @PurchaseNO
        ;
        
        --【D_Warehousing】Insert	Table転送仕様Ｇ②	赤	2020/01/27 add
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
       SELECT DH.PurchaseDate --WarehousingDate
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.SoukoType = 3     --3:店舗Main倉庫
            AND M.ChangeDate <= DH.PurchaseDate
            ORDER BY M.ChangeDate desc) AS SoukoCD
           ,NULL    --RackNO
           ,NULL	--StockNO
           ,DM.JanCD
           ,DM.AdminNO
           ,DM.SKUCD
           ,30   --WarehousingKBN	2020/01/27 chg
           ,1  --DeleteFlg★
           ,DM.PurchaseNO  --Number
           ,DM.PurchaseRows --NumberRow
           ,DH.VendorCD
           ,NULL    --ToStoreCD
           ,NULL    --ToSoukoCD
           ,NULL    --ToRackNO
           ,NULL    --ToStockNO
           ,NULL    --FromStoreCD
           ,NULL    --FromSoukoCD]
           ,NULL    --FromRackNO
           ,NULL    --CustomerCD
           ,DM.PurchaseSu * (-1)   --Quantity
           ,DM.PurchaserUnitPrice		--UnitPrice		2020/01/27 add
           ,DM.CalculationGaku * (-1)	--Amount		2020/01/27 add
           ,@Program  --Program
           
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL
        FROM D_PurchaseDetails AS DM
        INNER JOIN D_Purchase AS DH
        ON DH.PurchaseNO = DM.PurchaseNO
        WHERE DM.PurchaseNO = @PurchaseNO
          AND DM.DeleteDateTime IS NULL
          ;
   
        --【D_Arrival】Update	Table転送仕様Ｈ	赤	2020/01/27 add
        UPDATE [D_Arrival]
           SET [PurchaseSu] = D_Arrival.[PurchaseSu] - tbl.OldPurchaseSu
              ,[UpdateOperator] =  @Operator  
              ,[UpdateDateTime] =  @SYSDATETIME 
          FROM @Table AS tbl
         WHERE tbl.ArrivalNO = D_Arrival.ArrivalNO
         AND D_Arrival.DeleteDateTime IS NULL
         AND tbl.UpdateFlg = 1
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
        
        --【D_Delivery】Update	Table転送仕様Ｆ	黒	納品書
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;
        
        --【D_Warehousing】Insert	Table転送仕様Ｇ	黒	2020/01/27 add
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
           ,NULL	--StockNO
           ,tbl.JanCD
           ,tbl.AdminNO
           ,tbl.SKUCD
           ,30   --WarehousingKBN	2020/01/27 chg
           ,0  --DeleteFlg
           ,@PurchaseNO  --Number
           ,tbl.PurchaseRows --NumberRow
           ,@VendorCD	--計上仕入先CD
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
           ,tbl.CalculationGaku			--Amount		2020/01/27 add
           ,@Program  --Program
           
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL

          FROM @Table tbl
          --WHERE tbl.PurchaseRows = @tblPurchaseRows
          ;

        SET @OldPurchaseRows = 0;
        
        --カーソルオープン
        OPEN CUR_PurArr;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_PurArr
        INTO @Mishiire, @PurchaseRows, @ArrivalNO, @ArrivalRows;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            IF @OldPurchaseRows <> @PurchaseRows
            BEGIN
            	--画面明細で入力された仕入数
                SET @InputSu = @PurchaseSu;
                
                SET @OldPurchaseRows = @PurchaseRows;
            END
            
            --D_ArrivalDetails.ArrivalSu－PurchaseSu≧仕入数
            IF @Mishiire > @InputSu
            BEGIN
                SET @UpdSu = @InputSu;
                SET @InputSu = 0;
            END
            ELSE
            BEGIN
                SET @UpdSu = @Mishiire;
                SET @InputSu = @InputSu - @Mishiire;
            END
            
            IF @UpdSu > 0
            BEGIN
                --【D_PurchaseArrival】Table転送仕様Ｊ Insert
                INSERT INTO [D_PurchaseArrival]
                   ([PurchaseNO]
                   ,[PurchaseRows]
                   ,[ArrivalNO]
                   ,[ArrivalRows]
                   ,[PurchaseSu]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                    @PurchaseNO
                   ,DP.PurchaseRows
                   ,DA.ArrivalNO
                   ,DA.ArrivalRows
                   ,@UpdSu AS PurchaseSu
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL
                  FROM D_PurchaseDetails AS DP
                  INNER JOIN D_ArrivalDetails AS DA
                  ON DA.ArrivalNO = DP.ArrivalNO
                  AND DA.DeleteDateTime IS NULL
                  WHERE DP.PurchaseNO = @PurchaseNO
                  AND DP.PurchaseRows = @PurchaseRows
                  AND DA.ArrivalNO = @ArrivalNO
                  AND DA.ArrivalRows = @ArrivalRows
                  ;
            
                --入荷明細　D_ArrivalDetails
                UPDATE D_ArrivalDetails SET
                   [PurchaseSu] = [PurchaseSu] + @UpdSu
                  ,[UpdateOperator] =  @Operator  
                  ,[UpdateDateTime] =  @SYSDATETIME     
                WHERE ArrivalNO = @ArrivalNO
                AND ArrivalRows = @ArrivalRows
                ;
            END
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_PurArr
            INTO @Mishiire, @PurchaseRows, @ArrivalNO, @ArrivalRows;

        END
        
        --カーソルを閉じる
        CLOSE CUR_PurArr;
        DEALLOCATE CUR_PurArr;
        
        --【D_Arrival】Update	Table転送仕様Ｈ	黒	2020/01/27 add
        UPDATE [D_Arrival]
           SET [PurchaseSu] = D_Arrival.[PurchaseSu] + tbl.PurchaseSu
              ,[UpdateOperator] =  @Operator  
              ,[UpdateDateTime] =  @SYSDATETIME 
          FROM @Table AS tbl
         WHERE tbl.ArrivalNO = D_Arrival.ArrivalNO
         AND D_Arrival.DeleteDateTime IS NULL
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

        --【D_PayPlan】Delete　Table転送仕様Ｅ 支払予定
        UPDATE [D_PayPlan]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [Number] = @PurchaseNO
         AND [DeleteDateTime] IS NULL
         ;
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

