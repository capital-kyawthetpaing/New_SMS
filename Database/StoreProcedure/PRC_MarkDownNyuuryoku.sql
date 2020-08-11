IF OBJECT_ID ( 'D_MarkDown_SelectData', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_MarkDown_SelectData]
GO
IF OBJECT_ID ( 'M_SKU_SelectForMarkDown', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_SKU_SelectForMarkDown]
GO
IF OBJECT_ID ( 'PRC_MarkDownNyuuryoku', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_MarkDownNyuuryoku]
GO
IF EXISTS (select * from sys.table_types where name = 'T_MarkDown')
    Drop TYPE dbo.[T_MarkDown]
GO

--  ======================================================================
--       Program Call    É}Å[ÉNÉ_ÉEÉìì¸óÕ
--       Program ID      MarkDownNyuuryoku
--       Create date:    2020.06.20
--    ======================================================================
CREATE PROCEDURE D_MarkDown_SelectData
    (@MarkDownNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 ÉfÅ[É^íäèo                 --
--                                            --
--********************************************--

BEGIN

    SET NOCOUNT ON;

    SELECT DH.MarkDownNO
          ,DH.StoreCD
          ,DH.SoukoCD
          ,CONVERT(varchar,DH.MarkDownDate,111) AS MarkDownDate
          ,DH.ReplicaNO
          ,DH.StaffCD
          ,DH.VendorCD
          ,CONVERT(varchar,DH.CostingDate,111) AS CostingDate
          ,CONVERT(varchar,DH.UnitPriceDate,111) AS UnitPriceDate
          ,CONVERT(varchar,DH.ExpectedPurchaseDate,111) AS ExpectedPurchaseDate
          ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
          ,DH.Comment
          ,DH.MDPurchaseNO
          ,DH.PurchaseNO
          ,(SELECT TOP 1 PayeeCD FROM M_Vendor
            WHERE VendorCD = DH.VendorCD AND ChangeDate <= DH.PurchaseDate
            ORDER BY ChangeDate DESC) AS PayeeCD
          ,DM.MDPurchaseRows
          ,DM.SKUCD
          ,DM.AdminNO
          ,DM.JanCD
          ,DM.TaniCD
          ,DM.PriceOutTax
          ,DM.EvaluationPrice
          ,DM.StockSu
          ,DM.CalculationSu
          ,DM.Rate
          ,DM.MarkDownUnitPrice
          ,DM.MarkDownUnitPrice - DM.EvaluationPrice AS MarkDownSagakuPrice
          ,DM.MarkDownGaku
          ,DM.PurchaseRows
          ,DM.PurchaseSu
          ,DM.PurchaserUnitPrice
          ,DM.PurchaserUnitPrice - DM.EvaluationPrice AS PurchaserSagakuPrice
          ,DM.PurchaseGaku
          ,DM.InsertOperator
          ,Format(DM.InsertDateTime, 'yyyy/MM/dd HH:mm:ss') AS InsertDateTime
      FROM D_MarkDown DH
     INNER JOIN D_MarkDownDetails DM ON DH.MarkDownNO = DM.MarkDownNO
     WHERE DH.MarkDownNO = @MarkDownNO
     ORDER BY DM.MarkDownRows
     ;

END

GO

--********************************************--
--                                            --
--                M_SKUíäèo                   --
--                                            --
--********************************************--
CREATE PROCEDURE M_SKU_SelectForMarkDown(
    @JanCD varchar(13),
    @AdminNO int,
    @ChangeDate varchar(10)
)AS
BEGIN

    SET NOCOUNT ON;

    WITH SKU AS (
                SELECT MS.JanCD , MS.ChangeDate, MS.AdminNO
                  FROM ( SELECT JanCD , ChangeDate, AdminNO
                              , RANK() OVER(PARTITION BY JanCD ORDER BY ChangeDate DESC, AdminNO DESC) RANK
                           FROM M_SKU
                          WHERE DeleteFlg = 0
                            AND SetKBN = 0
                            AND JanCD  = @JanCD
                            AND AdminNO = (CASE WHEN @AdminNO <> 0 THEN @AdminNO ELSE AdminNO END)
                            AND ChangeDate <= @ChangeDate
                       ) MS
                 WHERE MS.RANK = 1      
    )
    SELECT MS.AdminNO          
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.ITemCD
          ,MS.SKUName
          ,MS.ColorName
          ,MS.SizeName
          ,MS.MakerItem
          ,MS.SKUCD
          ,MS.PriceOutTax
          ,MS.NormalCost
          ,(CASE MS.TaxRateFLG WHEN 1 THEN (SELECT TOP 1 A.TaxRate1 FROM M_SalesTax A WHERE A.ChangeDate <= @ChangeDate ORDER BY A.ChangeDate DESC)
                               WHEN 2 THEN (SELECT TOP 1 A.TaxRate2 FROM M_SalesTax A WHERE A.ChangeDate <= @ChangeDate ORDER BY A.ChangeDate DESC)
                               ELSE 0 END ) AS TaxRate
          ,MS.TaniCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.[ID]= 201 AND A.[KEY]= MS.TaniCD) AS TaniName          
    FROM  SKU
    INNER JOIN M_SKU MS ON SKU.AdminNO = MS.AdminNO
                       AND SKU.ChangeDate = MS.ChangeDate
    ;
    
END

GO

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
--                 ÉfÅ[É^çXêV                 --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_MarkDownNyuuryoku
    (@OperateMode    int,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
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
--                 èàóùäJén                   --
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(date, @SYSDATETIME);
    SET @Program = 'MarkDownNyuuryoku';   
    SET @UpdPurchaseNO = NULL;
    SET @IsNew = 0;
    
    --êVãKÅEïœçX--
    IF @OperateMode <> '3'
    BEGIN
        IF @OperateMode = '1'
            SET @OperateModeNm = 'êVãK';
        ELSE IF @OperateMode = 2
            SET @OperateModeNm = 'ïœçX';
    
        -- á@édì¸î‘çÜçÃî‘
        -- êVãK Ç‹ÇΩÇÕ ïœçXÇ≈ó\íËÅ®åãâ  or åãâ Å®ó\íËÇ…ïœçXÇ≥ÇÍÇΩèÍçá
        IF @OperateMode = '1' OR (ISNULL(@PurchaseNO,'') = '' AND @ChkResult = '1') OR (ISNULL(@MDPurchaseNO,'') = '' AND @ChkResult = '0')
        BEGIN
            
            --édì¸î‘çÜçÃî‘
            EXEC Fnc_GetNumber
                4,             --inì`ï[éÌï  4
                @ChangeDate,   --inäÓèÄì˙
                @StoreCD,      --inìXï‹CD
                @Operator,
                @UpdPurchaseNO    OUTPUT
                ;
            
            IF ISNULL(@UpdPurchaseNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            -- édì¸ñæç◊Çí«â¡ÇµÇΩèÍçáÅAtrue
            SET @IsNew = 1;
        END
                
        SET @PaymentPlanDate = NULL;
        SET @PayPlanNO = 0;
        
        -- áAéxï•ó\íËçXêV
        IF @ChkResult = '1'
        BEGIN
            --éxï•ó\íËì˙éÊìæ
            EXEC Fnc_PlanDate_SP
                1,             --inâÒé˚éxï•ãÊï™ 1:éxï•
                @PayeeCD,      --inéÊà¯êÊCD
                @PurchaseDate, --inåvè„ì˙
                0,             --iní†í[ãÊï™
                @PaymentPlanDate    OUTPUT
                ;
        
            IF ISNULL(@PurchaseNO,'') = ''
            BEGIN
                --ÅyD_PayPlanÅzTableì]ëóédólG éxï•ó\íË Insert
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
                
                --íºëOÇ…çÃî‘Ç≥ÇÍÇΩ IDENTITY óÒÇÃílÇéÊìæÇ∑ÇÈ            
                SET @PayPlanNO = @@IDENTITY;
            END
            ELSE
            BEGIN
                UPDATE D_PayPlan
                   SET [RecordedDate] = @PurchaseDate
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
        
        -- áBédì¸ÅEédì¸ñæç◊çXêV
        -- êVãK Ç‹ÇΩÇÕ ïœçXÇ≈ó\íËÅ®åãâ  or åãâ Å®ó\íËÇ…ïœçXÇ≥ÇÍÇΩèÍçá
        IF @IsNew = 1
        BEGIN
            --ÅyD_PurchaseÅzTableì]ëóédólC/E Insert
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
        --  ïœçXÇ≈ó\íË/åãâ Ç™ïœçXÇ»ÇµÇÃèÍçá
        ELSE IF @OperateMode = '2'        
        BEGIN
            IF @ChkResult = '0'
                SET @UpdPurchaseNO = @MDPurchaseNO;
            ELSE
                SET @UpdPurchaseNO = @PurchaseNO;
            
            --ÅyD_PurchaseÅzTableì]ëóédólC Update
            UPDATE [D_Purchase]
               SET [PurchaseDate] = convert(date,@ChangeDate)
                  --,[VendorCD] = @VendorCD
                  --,[CalledVendorCD] = @VendorCD
                  ,[CalculationGaku] = @PurchaseGaku
                  ,[PurchaseGaku] = @PurchaseGaku
                  ,[PurchaseTax] = @PurchaseTax
                  ,[TotalPurchaseGaku] = @TotalPurchaseGaku
                  ,[CommentInStore] = @Comment
                  ,[StaffCD] = @StaffCD
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @UpdPurchaseNO;
             
            --ÅyD_PurchaseDetailsÅzTableì]ëóédólD Delete
            DELETE [D_PurchaseDetails]
            WHERE PurchaseNO = @UpdPurchaseNO;     
            
        END
        
        --ÅyD_PurchaseDetailsÅzTableì]ëóédólD Insert
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
               ,1         --DifferenceFlgÅö
               ,NULL      --DeliveryNo
               ,CASE WHEN @IsNew = 1 THEN @Operator ELSE tbl.InsertOperator END
               ,CASE WHEN @IsNew = 1 THEN @SYSDATETIME ELSE tbl.InsertDateTime END 
               ,@Operator  
               ,@SYSDATETIME
          FROM @Table tbl
          ;
          
        --áCJANî≠íçíPâøÉ}ÉXÉ^
        IF ISNULL(@MarkDownNO,'') <> ''
        BEGIN
            -- ÅyM_JANOrderPriceÅzTableì]ëóédólJ Delete
            DELETE M_JANOrderPrice
            FROM   M_JANOrderPrice MJ
            INNER JOIN D_MarkDown DM ON DM.VendorCD = MJ.VendorCD
                                    AND DM.UnitPriceDate = MJ.ChangeDate
            INNER JOIN D_MarkDownDetails DMD ON DM.MarkDownNO = DMD.MarkDownNO
                                            AND DMD.AdminNO = MJ.AdminNO                                            
            WHERE DM.MarkDownNO = @MarkDownNO
            ;   
        END
        
        -- ÅyM_JANOrderPriceÅzTableì]ëóédólJ Insert
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
        ;
          
        --áDÉ}Å[ÉNÉ_ÉEÉìÅEÉ}Å[ÉNÉ_ÉEÉìñæç◊
        IF @OperateMode = '1'
        BEGIN
            --ì`ï[î‘çÜçÃî‘
            EXEC Fnc_GetNumber
                31,             --inì`ï[éÌï  
                @CostingDate,   --inäÓèÄì˙
                @StoreCD,       --inìXï‹CD
                @Operator,
                @MarkDownNO OUTPUT
                ;
            
            IF ISNULL(@MarkDownNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --ÅyD_MarkDownÅzTableì]ëóédólA Insert
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
            --ÅyD_MarkDownÅzTableì]ëóédólA Update
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
            
            --ÅyD_MarkDownDetailsÅzTableì]ëóédólB Delete
            DELETE [D_MarkDownDetails]
            WHERE MarkDownNO = @MarkDownNO
            ;
        END
        
        --ÅyD_MarkDownDetailsÅzTableì]ëóédólB Insert
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
    
    ELSE IF @OperateMode = 3 --çÌèú--
    BEGIN
        SET @OperateModeNm = 'çÌèú';
        
        -- ÅyM_JANOrderPriceÅzTableì]ëóédólJ Update
        DELETE M_JANOrderPrice
        FROM   M_JANOrderPrice MJ
        INNER JOIN D_MarkDown DM ON DM.VendorCD = MJ.VendorCD
                                AND DM.UnitPriceDate = MJ.ChangeDate
        INNER JOIN D_MarkDownDetails DMD ON DM.MarkDownNO = DMD.MarkDownNO
                                        AND DMD.AdminNO = MJ.AdminNO                                            
        WHERE DM.MarkDownNO = @MarkDownNO
        ;   
        
        --ÅyD_MarkDownÅzTableì]ëóédólA Update
        UPDATE D_MarkDown
           SET [DeleteOperator] = @Operator
              ,[DeleteDateTime] = @SYSDATETIME
        WHERE [MarkDownNO] = @MarkDownNO
        ;
        
        --ÅyD_MarkDownÅzTableì]ëóédólB Update
        UPDATE D_MarkDownDetails
           SET [DeleteOperator] = @Operator
              ,[DeleteDateTime] = @SYSDATETIME
        WHERE [MarkDownNO] = @MarkDownNO
        ;
         
        IF @MDPurchaseNO <> ''
        BEGIN
            --ÅyD_PurchaseÅzTableì]ëóédólC Update
            UPDATE [D_Purchase]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @MDPurchaseNO;
             
            --ÅyD_PurchaseDetailsÅzTableì]ëóédólD Update
            UPDATE [D_PurchaseDetails]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            WHERE PurchaseNO = @MDPurchaseNO;        
        END
            
        IF @PurchaseNO <> ''
        BEGIN
            --ÅyD_PurchaseÅzTableì]ëóédólE Update
            UPDATE [D_Purchase]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE PurchaseNO = @PurchaseNO;
             
            --ÅyD_PurchaseDetailsÅzTableì]ëóédólF Update
            UPDATE [D_PurchaseDetails]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            WHERE PurchaseNO = @PurchaseNO;  
            
            --ÅyD_PayPlanÅzTableì]ëóédólG éxï•ó\íË Update
            UPDATE D_PayPlan
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
             WHERE [Number] = @PurchaseNO
             ;
        
        END
    END
    
    
    --èàóùóöóÉfÅ[É^Ç÷çXêV
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
