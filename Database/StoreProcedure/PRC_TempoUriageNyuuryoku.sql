 BEGIN TRY 
 Drop Procedure dbo.[PRC_TempoUriageNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 売上入力
--       Program ID      TempoUriageNyuuryoku
--       Create date:    2020.3.19
--    ======================================================================
--CREATE TYPE [T_TempoUriage] AS TABLE(
--	[DenNo] [varchar](11),
--    [DenRows] [int] NULL,
--    [SalesSU] [int] NULL,
--    [UpdateFlg] [tinyint] NULL
--)
--GO

CREATE PROCEDURE [PRC_TempoUriageNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @StoreCD   varchar(4),
    @SalesDate  varchar(10),
    @BillingType tinyint,

    @Table  T_TempoUriage READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutSalesNO varchar(11) OUTPUT
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
    DECLARE @FirstCollectPlanDate varchar(10);
    DECLARE @SalesDate_VAR varchar(10);
    DECLARE @BillingCD varchar(13);
    DECLARE @SalesNO varchar(11);
    DECLARE @OldNumber varchar(11);
                
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    --新規Mode時、
    IF @OperateMode = 1
    BEGIN
		SET @OperateModeNm = '新規';

        --Table転送仕様Ａ
        UPDATE D_Reserve
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[ShippingSu]  =  [ShippingSu] + tbl.SalesSU  --出荷数
               ,[ReserveSu]   =  [ReserveSu] - tbl.SalesSU   --引当数
         FROM @Table tbl
         WHERE D_Reserve.[Number] = tbl.DenNO
         AND D_Reserve.NumberRows = tbl.DenRows
         AND D_Reserve.DeleteDateTime IS NULL
         ;
         
		--Table転送仕様Ｂ
        UPDATE [D_Stock]
           SET [StockSu] = [D_Stock].[StockSu] - tbl.SalesSU       --在庫数
              ,[ReserveSu] = [D_Stock].[ReserveSu] - tbl.SalesSU   --引当数
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
         FROM @Table AS tbl
         INNER JOIN D_Reserve AS A
         ON A.[Number] = tbl.DenNO
         AND A.NumberRows = tbl.DenRows
         AND A.DeleteDateTime IS NULL
         WHERE D_Stock.StockNO = A.StockNO
         AND D_Stock.DeleteDateTime IS NULL
         ;
                
        --②TemporaryTableから
        --カーソル定義
        DECLARE CUR_BBB CURSOR FOR
            SELECT tbl.DenNO, tbl.DenRows, A.ReserveNO, tbl.SalesSu
            		,A.ShippingSu
					,(SELECT top 1 M.BillingCD       
                        FROM M_Customer AS M
                        INNER JOIN D_Juchuu AS DJ
                    	ON DJ.JuchuuNO = tbl.DenNO
                        AND M.CustomerCD = DJ.CustomerCD
                        WHERE M.ChangeDate <= @SalesDate 
                        AND M.DeleteFlg = 0
                        ORDER BY M.ChangeDate DESC) AS BillingCD
            	,CONVERT(varchar,@SalesDate,111) As SalesDate
            FROM @Table AS tbl
            INNER JOIN D_Reserve AS A
            ON A.[Number] = tbl.DenNO
	        AND A.NumberRows = tbl.DenRows
	        AND A.DeleteDateTime IS NULL
            ORDER BY tbl.DenNO, tbl.DenRows
            ;
            
        DECLARE @Number varchar(11),    
                @NumberRow int,
                @SalesSu int,
                @ShippingSu int,
                @InstructionNO varchar(11),
                @ReserveNO varchar(11),
                @ShippingNO varchar(11),
                @Rows int,
                @BillingNO varchar(11)
                ;
        
        SET @Rows = 1;
        
        --カーソルオープン
        OPEN CUR_BBB;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_BBB
        INTO @Number, @NumberRow, @ReserveNO, @SalesSu, @ShippingSu, @BillingCD, @SalesDate_VAR
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            IF @Rows = 1
            BEGIN
                --回収日再計算
                EXEC Fnc_PlanDate_SP
                    0,             --0:回収,1:支払
                    @BillingCD,   --in顧客CD
                    @SalesDate_VAR,    --in基準日
                    0,              --帳端区分
                    @FirstCollectPlanDate OUTPUT
                    ;
              
                --処理履歴データへ更新
                SET @KeyItem = @Number;
                
                EXEC L_Log_Insert_SP
                    @SYSDATETIME,
                    @Operator,
                    'TempoUriageNyuuryoku',
                    @PC,
                    @OperateModeNm,
                    @KeyItem;
		        
                --伝票番号採番
                EXEC Fnc_GetNumber
                    14,          --in伝票種別 14：出荷指示
                    @SalesDate, --in基準日
                    @StoreCD,    --in店舗CD
                    @Operator,
                    @InstructionNO OUTPUT
                    ;
                
                IF ISNULL(@InstructionNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END

                --Table転送仕様Ｃ   Insert 出荷指示　D_Instruction      DenNOごとに１Record    
                INSERT INTO [D_Instruction]
                   ([InstructionNO]
                   ,[DeliveryPlanNO]
                   ,[InstructionKBN]
                   ,[FromSoukoCD]
                   --,[ToSoukoCD]
                   ,[PrintDate]
                   ,[PrintStaffCD]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                VALUES
                   (@InstructionNO
                   ,NULL --DeliveryPlanNO
                   ,3    --InstructionKBN
                   ,NULL --FromSoukoCD
                   --,NULL --ToSoukoCD
                   ,NULL --PrintDate
                   ,NULL --PrintStaffCD
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
                   );

                --伝票番号採番
                EXEC Fnc_GetNumber
                    6,          --in伝票種別 6
                    @SalesDate, --in基準日
                    @StoreCD,    --in店舗CD
                    @Operator,
                    @ShippingNO OUTPUT
                    ;

                IF ISNULL(@ShippingNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END

                --Table転送仕様Ｅ   Insert 出荷　D_Shipping                     TemporaryTableごとに１Record
                INSERT INTO [D_Shipping]
                   ([ShippingNO]
                   ,[SoukoCD]
                   ,[ShippingKBN]
                   ,[InstructionNO]
                   ,[CarrierCD]
                   ,[InputDateTime]
                   ,[StaffCD]
                   ,[PrintDate]
                   ,[PrintStaffCD]
                   ,[LinkageDateTime]
                   ,[LinkageStaffCD]
                   ,[InvoiceNO]
                   ,[InvNOLinkDateTime]
                   ,[ReceiveStaffCD]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                 VALUES
                   (@ShippingNO
                   ,NULL --SoukoCD
                   ,3 --ShippingKBN
                   ,@InstructionNO --InstructionNO
                   ,NULL --CarrierCD
                   ,@SYSDATETIME --InputDateTime
                   ,@Operator --StaffCD
                   ,NULL --PrintDate
                   ,NULL --PrintStaffCD
                   ,NULL --LinkageDateTime
                   ,NULL --LinkageStaffCD
                   ,NULL --InvoiceNO
                   ,NULL --InvNOLinkDateTime
                   ,NULL --ReceiveStaffCD
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
                   );

                --伝票番号採番
                EXEC Fnc_GetNumber
                    3,          --in伝票種別 3
                    @SalesDate, --in基準日
                    @StoreCD,    --in店舗CD
                    @Operator,
                    @SalesNO OUTPUT
                    ;
                    
                IF ISNULL(@SalesNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --Table転送仕様Ｇ Insert 売上
                INSERT INTO [D_Sales]
                   ([SalesNO]
                   ,[StoreCD]
                   ,[SalesDate]
                   ,[ShippingNO]
                   ,[CustomerCD]
                   ,[CustomerName]
                   ,[CustomerName2]
                   ,[BillingType]
                   ,[Age]
                   ,[SalesHontaiGaku]
                   ,[SalesHontaiGaku0]
                   ,[SalesHontaiGaku8]
                   ,[SalesHontaiGaku10]
                   ,[SalesTax]
                   ,[SalesTax8]
                   ,[SalesTax10]
                   ,[SalesGaku]
                   ,[LastPoint]
                   ,[WaitingPoint]
                   ,[StaffCD]
                   ,[PrintDate]
                   ,[PrintStaffCD]
                   ,[Discount]
                   ,[Discount8]
                   ,[Discount10]
                   ,[DiscountTax]
                   ,[DiscountTax8]
                   ,[DiscountTax10]                   
                   ,[CostGaku]
                   ,[ProfitGaku]
                   ,[PurchaseNO]
                   ,[SalesEntryKBN]
                   ,[NouhinsyoComment]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                   @SalesNO
                   ,@StoreCD
                   ,CONVERT(date, @SalesDate)
                   ,@ShippingNO
                   ,MAX(DH.CustomerCD)
                   ,MAX(DH.CustomerName)
                   ,MAX(DH.CustomerName2)
                   ,@BillingType
                   ,5 AS Age	--5:その他
                   ,MAX(DH.JuchuuGaku)-SUM(DJM.JuchuuTax)   --SalesHontaiGaku
                   ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN DJM.JuchuuHontaiGaku ELSE 0 END)  --SalesHontaiGaku0
                   ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN DJM.JuchuuHontaiGaku ELSE 0 END)  --SalesHontaiGaku8
                   ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN DJM.JuchuuHontaiGaku ELSE 0 END) --SalesHontaiGaku10
                   ,SUM(DJM.JuchuuTax)  AS SalesTax
                   ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN DJM.JuchuuTax ELSE 0 END)  --SalesTax8
                   ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN DJM.JuchuuTax ELSE 0 END) --SalesTax10
                   ,MAX(DH.JuchuuGaku) AS SalesGaku
                   ,0	--LastPoint, money,>
                   ,0 	--WaitingPoint, money,>
                   ,@Operator   --StaffCD
                   ,NULL    --PrintDate
                   ,NULL    --PrintStaffCD
                   ,0 As Discount
                   ,0 As Discount8
                   ,0 As Discount10
                   ,0 As DiscountTax
                   ,0 As DiscountTax8
                   ,0 As DiscountTax10
                   ,0 AS CostGaku
                   ,0 AS ProfitGaku
                   ,NULL AS PurchaseNO
                   ,0 AS SalesEntryKBN
                   ,NULL AS NouhinsyoComment
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
               FROM D_JuchuuDetails AS DJM
               INNER JOIN D_Juchuu AS DH
               ON DH.JuchuuNO = DJM.JuchuuNO
               INNER JOIN @Table AS tbl ON tbl.DenRows = DJM.JuchuuRows
               WHERE DJM.JuchuuNO = tbl.DenNO
               GROUP BY DJM.JuchuuNO
               ;
               
                SET @BillingNO = NULL;
                --・Form.請求ボタン＝「即請求」の場合のみ
                IF @BillingType = 1
                BEGIN
                    --伝票番号採番
                    EXEC Fnc_GetNumber
                        15,             --in伝票種別 15
                        @SalesDate,    --in基準日
                        @StoreCD,       --in店舗CD
                        @Operator,
                        @BillingNO OUTPUT
                        ;
                            
                    IF ISNULL(@BillingNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --【D_Billing】INSERT Table転送仕様Ｋ
                    INSERT INTO [D_Billing]
                           ([BillingNO]
                           ,[BillingType]   --2019.10.23 add
                           ,[StoreCD]
                           ,[BillingCloseDate]
                           ,[CollectPlanDate]
                           ,[BillingCustomerCD]
                           ,[ProcessingNO]
                           ,[SumBillingHontaiGaku]
                           ,[SumBillingHontaiGaku0]
                           ,[SumBillingHontaiGaku8]
                           ,[SumBillingHontaiGaku10]
                           ,[SumBillingTax]
                           ,[SumBillingTax8]
                           ,[SumBillingTax10]
                           ,[SumBillingGaku]
                           ,[AdjustHontaiGaku8]
                           ,[AdjustHontaiGaku10]
                           ,[AdjustTax8]
                           ,[AdjustTax10]
                           ,[TotalBillingHontaiGaku]
                           ,[TotalBillingHontaiGaku0]
                           ,[TotalBillingHontaiGaku8]
                           ,[TotalBillingHontaiGaku10]
                           ,[TotalBillingTax]
                           ,[TotalBillingTax8]
                           ,[TotalBillingTax10]
                           ,[BillingGaku]
                           ,[PrintDateTime]
                           ,[PrintStaffCD]
                           ,[CollectDate]
                           ,[LastCollectDate]
                           ,[CollectStaffCD]
                           ,[CollectGaku]
                           ,[LastBillingGaku]
                           ,[LastCollectGaku]
                           ,[BillingConfirmFlg]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                     SELECT
                           @BillingNO
                           ,1   --BillingType   2019.10.23 add
                           ,DS.StoreCD
                           ,CONVERT(date, @SalesDate)
                           ,CONVERT(date, @FirstCollectPlanDate)
                           ,(SELECT top 1 M.BillingCD       --BillingCustomerCD
                                FROM M_Customer AS M
                                WHERE M.CustomerCD = DS.CustomerCD
                                AND M.ChangeDate <= CONVERT(date, @SalesDate) 
                                AND M.DeleteFlg = 0
                                ORDER BY M.ChangeDate DESC)
                           ,NULL    --ProcessingNO
                           ,DS.SalesHontaiGaku  --SumBillingHontaiGaku 
                           ,DS.SalesHontaiGaku0 --SumBillingHontaiGaku0 
                           ,DS.SalesHontaiGaku8 --SumBillingHontaiGaku8 
                           ,DS.SalesHontaiGaku10    --SumBillingHontaiGaku10 
                           ,DS.SalesTax --SumBillingTax 
                           ,DS.SalesTax8    --SumBillingTax8 
                           ,DS.SalesTax10   --SumBillingTax10 
                           ,DS.SalesGaku    --SumBillingGaku 
                           ,0   --AdjustHontaiGaku8 
                           ,0   --AdjustHontaiGaku10 
                           ,0   --AdjustTax8 
                           ,0   --AdjustTax10 
                           ,DS.SalesHontaiGaku        
                           ,DS.SalesHontaiGaku0
                           ,DS.SalesHontaiGaku8
                           ,DS.SalesHontaiGaku10
                           ,DS.SalesTax
                           ,DS.SalesTax8
                           ,DS.SalesTax10
                           ,DS.SalesGaku
                           ,NULL    --PrintDateTime
                           ,NULL    --PrintStaffCD
                           ,NULL    --CollectDate
                           ,NULL    --LastCollectDate
                           ,NULL    --CollectStaffCD
                           ,0   --CollectGaku-
                           ,0   --LastBillingGaku
                           ,0   --LastCollectGaku 
                           ,1   -- BillingConfirmFlg
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                       FROM D_Sales AS DS
                       WHERE DS.SalesNO = @SalesNO
                       ;
                END
                
                --Table転送仕様Ｉ Insert 回収予定　D_CollectPlan
                INSERT INTO [D_CollectPlan]
                   (--[CollectPlanNO]
                   [SalesNO]
                   ,[JuchuuNO]
                   ,[JuchuuKBN]
                   ,[StoreCD]
                   ,[CustomerCD]
                   ,[HontaiGaku]
                   ,[HontaiGaku0]
                   ,[HontaiGaku8]
                   ,[HontaiGaku10]
                   ,[Tax]
                   ,[Tax8]
                   ,[Tax10]
                   ,[CollectPlanGaku]
                   ,[AdjustTax8]
                   ,[AdjustTax10]
                   ,[BillingType]
                   ,[BillingDate]
                   ,[BillingNO]
                   ,[MonthlyBillingNO]
                   ,[PaymentMethodCD]
                   ,[CardProgressKBN]
                   ,[PaymentProgressKBN]
                   ,[InvalidFLG]
                   ,[BillingCloseDate]
                   ,[FirstCollectPlanDate]
                   ,[ReminderFLG]
                   ,[NoReminderDate]
                   ,[NextCollectPlanDate]
                   ,[ActionCD]
                   ,[NextActionCD]
                   ,[LastReminderNO]
                   ,[Program]
                   ,[BillingConfirmFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                   --CollectPlanNO
                   @SalesNO
                   ,@Number As JuchuuNO
                   ,(SELECT DJ.JuchuuKBN FROM D_Juchuu AS DJ
                    WHERE DJ.JuchuuNO = @Number
                    )
                   ,DS.StoreCD
                   ,(SELECT top 1 M.BillingCD       
                        FROM M_Customer AS M
                        WHERE M.CustomerCD = DS.CustomerCD
                        AND M.ChangeDate <= DS.SalesDate 
                        AND M.DeleteFlg = 0
                        ORDER BY M.ChangeDate DESC)      --CustomerCD                       
                   ,DS.SalesHontaiGaku      --HontaiGaku
                   ,DS.SalesHontaiGaku0     --HontaiGaku0
                   ,DS.SalesHontaiGaku8     --HontaiGaku8
                   ,DS.SalesHontaiGaku10    --HontaiGaku10
                   ,DS.SalesTax     --Tax
                   ,DS.SalesTax8    --Tax8
                   ,DS.SalesTax10   --Tax10
                   ,DS.SalesGaku    --CollectPlanGaku
                   ,0 AS AdjustTax8
                   ,0 AS AdjustTax10
                   ,(CASE WHEN @BillingType = 1 THEN 1
                        ELSE (SELECT top 1 M.BillingType FROM M_Customer AS M 
                                INNER JOIN D_Juchuu AS DJ
                                ON DJ.JuchuuNO = @Number
                                AND DJ.DeleteDateTime IS NULL
                                AND M.CustomerCD = DJ.CustomerCD
                                AND M.ChangeDate <= DJ.JuchuuDate
                                WHERE M.DeleteFlg = 0
                                ORDER BY M.ChangeDate desc) END)   --BillingType
                   ,(CASE WHEN @BillingType = 1 THEN DS.SalesDate
                        ELSE NULL END) --<BillingDate, date,>
                   ,(CASE WHEN @BillingType = 1 THEN @BillingNO
                   		ELSE NULL END)	--BillingNO
                   ,NULL	--MonthlyBillingNO
                   ,(SELECT DJ.PaymentMethodCD FROM D_Juchuu AS DJ
                    WHERE DJ.JuchuuNO = @Number
                    )
                   ,0   --CardProgressKBN
                   ,0   --PaymentProgressKBN
                   ,0   --InvalidFLG
                   ,NULL    --BillingCloseDate, date,>
                   ,CONVERT(date, @FirstCollectPlanDate)
                   ,0   --ReminderFLG
                   ,NULL    --NoReminderDate
                   ,NULL    --NextCollectPlanDate
                   ,NULL    --ActionCD
                   ,NULL    --NextActionCD
                   ,NULL    --LastReminderNO
                   ,'TempoUriageNyuuryoku'  --Program
                   ,(CASE WHEN @BillingType = 1 THEN 1
                   		ELSE 0 END)   --BillingConfirmFlg
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
               FROM D_Sales AS DS
               WHERE DS.SalesNO = @SalesNO
               ;
            END		--IF @Rows = 1
            
            --Table転送仕様Ｄ   Insert 出荷指示明細　D_InstructionDetails   TemporaryTableRecordごとに１Record
            INSERT INTO [D_InstructionDetails]
               ([InstructionNO]
               ,[InstructionRows]
               ,[InstructionKBN]
               ,[ReserveNO]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[InstructionSu]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            VALUES
               (@InstructionNO
               ,@Rows
               ,3   --InstructionKBN
               ,@ReserveNO
               ,NULL    --CommentOutStore
               ,NULL    --CommentInStore
               ,@ShippingSu --InstructionSu
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
               );
                
            --Table転送仕様Ｆ   Insert 出荷明細　D_ShippingDetails          TemporaryTableRecordごとに１Record
            INSERT INTO [D_ShippingDetails]
               ([ShippingNO]
               ,[ShippingRows]
               ,[ShippingKBN]
               ,[Number]
               ,[NumberRows]
               ,[JanCD]
               ,[AdminNO]
               ,[SKUCD]
               ,[SKUName]
               ,[ColorName]
               ,[SizeName]
               ,[ShippingSu]
               ,[InstructionNO]
               ,[InstructionRows]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @ShippingNO
               ,@Rows
               ,3   --ShippingKBN
               ,@Number
               ,@NumberRow
               ,DM.JanCD
               ,DM.AdminNO
               ,DM.SKUCD
               ,DM.SKUName
               ,DM.ColorName
               ,DM.SizeName
               ,@ShippingSu
               ,@InstructionNO
               ,@Rows
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM D_JuchuuDetails AS DM
            WHERE DM.JuchuuNO = @Number
            AND DM.JuchuuRows = @NumberRow
            ;
            
            --Table転送仕様Ｈ Insert 売上明細
            INSERT INTO [D_SalesDetails]
               ([SalesNO]
               ,[SalesRows]
               ,[JuchuuNO]
               ,[JuchuuRows]
               ,[NotPrintFLG]
               ,[AddSalesRows]
               ,[ShippingNO]
               ,[AdminNO]
               ,[SKUCD]
               ,[JanCD]
               ,[SKUName]
               ,[ColorName]
               ,[SizeName]
               ,[SalesSU]
               ,[SalesUnitPrice]
               ,[TaniCD]
               ,[SalesHontaiGaku]
               ,[SalesTax]
               ,[SalesGaku]
               ,[SalesTaxRitsu]
               ,[ProperGaku]
               ,[DiscountGaku]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[IndividualClientName]
               ,[DeliveryNoteFLG]
               ,[BillingPrintFLG]
               ,[PurchaseNO]
               ,[PurchaseRows]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @SalesNO
               ,@Rows
               ,DM.JuchuuNO
               ,DM.JuchuuRows
               ,DM.NotPrintFLG
               ,0 AS AddSalesRows	--後で更新
               ,@ShippingNO
               ,DM.AdminNO
               ,DM.SKUCD
               ,DM.JanCD
               ,DM.SKUName
               ,DM.ColorName
               ,DM.SizeName
               ,DM.JuchuuSuu		--SalesSU
               ,DM.JuchuuUnitPrice  --SalesUnitPrice
               ,DM.TaniCD
               ,DM.JuchuuHontaiGaku   --SalesHontaiGaku
               ,DM.JuchuuTax 		--vSalesTax
               ,DM.JuchuuGaku		--SalesGaku
               ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
               ,DM.JuchuuGaku AS ProperGaku
               ,0 As DiscountGaku
               ,NULL    --CommentOutStore
               ,NULL    --CommentInStore
               ,DM.IndividualClientName    --IndividualClientName
               ,0   	--DeliveryNoteFLG, tinyint,>
               ,0		--BillingPrintFLG, tinyint,>
               ,NULL AS PurchaseNO
               ,0 AS PurchaseRows
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
           FROM D_JuchuuDetails AS DM
           WHERE DM.JuchuuNO = @Number
           AND DM.JuchuuRows = @NumberRow
           ;
           
           --テーブル転送仕様Ｍ受注明細
           UPDATE D_Juchuu
            SET [UpdateOperator] = @Operator
               ,[UpdateDateTime] = @SYSDATETIME
           WHERE JuchuuNO = @Number
            AND DeleteDateTime IS NULL
            ;
	        
           UPDATE D_JuchuuDetails
            SET [SalesDate] = CONVERT(date, @SalesDate)
               ,[SalesNO] = @SalesNO
               --,[DeliveryOrderSu] =tbl.ShippingSu
			   --,[DeliverySu] = tbl.ShippingSu
               ,[UpdateOperator] = @Operator
               ,[UpdateDateTime] = @SYSDATETIME
           --FROM @Table AS tbl 
           WHERE D_JuchuuDetails.JuchuuNO = @Number
            AND D_JuchuuDetails.JuchuuRows = @NumberRow
           -- AND D_JuchuuDetails.JuchuuRows = tbl.JuchuuRows
            AND D_JuchuuDetails.DeleteDateTime IS NULL
            ;

            --Table転送仕様Ｊ Insert 回収予定明細　D_CollectPlanDetails
            INSERT INTO [D_CollectPlanDetails]
               ([CollectPlanNO]
               ,[CollectPlanRows]
               ,[SalesNO]
               ,[SalesRows]
               ,[JuchuuNO]
               ,[JuchuuRows]
               ,[JuchuuKBN]
               ,[HontaiGaku]
               ,[Tax]
               ,[CollectPlanGaku]
               ,[TaxRitsu]
               ,[FirstCollectPlanDate]
               ,[PaymentProgressKBN]
               ,[BillingPrintFLG]
               ,[AdjustTax]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
               (SELECT top 1 DH.CollectPlanNO 
                FROM D_CollectPlan AS DH
                WHERE DH.SalesNO = @SalesNO
                ORDER BY CollectPlanNO desc)
               ,DSM.SalesRows   --CollectPlanRows
               ,DSM.SalesNO
               ,DSM.SalesRows
               ,DSM.JuchuuNO
               ,DSM.JuchuuRows
               ,DJ.JuchuuKBN
               ,DSM.SalesHontaiGaku --HontaiGaku
               ,DSM.SalesTax    --Tax
               ,DSM.SalesGaku   --CollectPlanGaku
               ,DSM.SalesTaxRitsu   --TaxRitsu
               ,CONVERT(date, @FirstCollectPlanDate)
               ,0   --PaymentProgressKBN
               ,0   --BillingPrintFLG
               ,0	--AdjustTax
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
           FROM D_SalesDetails AS DSM
           LEFT OUTER JOIN D_Juchuu AS DJ
           ON DJ.JuchuuNO = DSM.JuchuuNO
           AND DJ.DeleteDateTime IS NULL
           WHERE DSM.SalesNO = @SalesNO
           AND DSM.DeleteDateTime IS NULL
           AND DSM.JuchuuNO = @Number
           AND DSM.JuchuuRows = @NumberRow
           ;
           
	        --・Form.請求ボタン＝「即請求」の場合のみ
	        IF @BillingType = 1
	        BEGIN
                --【D_BillingDetails】INSERT Table転送仕様Ｌ
                INSERT INTO [D_BillingDetails]
                       ([BillingNO]
                       ,[BillingType]   --2019.10.23 add
                       ,[BillingRows]
                       ,[StoreCD]
                       ,[BillingCloseDate]
                       ,[CustomerCD]
                       ,[SalesNO]
                       ,[SalesRows]
                       ,[CollectPlanNO]
                       ,[CollectPlanRows]
                       ,[BillingHontaiGaku]
                       ,[BillingTax]
                       ,[BillingGaku]
                       ,[TaxRitsu]
                       ,[InvoiceFLG]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                       @BillingNO
                       ,1   --BillingType   2019.10.23 add
                       ,DSM.SalesRows AS BillingRows
                       ,DS.StoreCD
                       ,CONVERT(date, @SalesDate) AS BillingCloseDate
                       ,DS.CustomerCD
                       ,DSM.SalesNO
                       ,DSM.SalesRows 
                       ,DM.CollectPlanNO 
                       ,DM.CollectPlanRows 
                       ,DSM.SalesHontaiGaku 
                       ,DSM.SalesTax 
                       ,DSM.SalesGaku   --CollectPlanGaku 
                       ,DSM.SalesTaxRitsu 
                       ,0   --InvoiceFLG 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                       
                    FROM D_SalesDetails AS DSM
                    
                    LEFT OUTER JOIN D_CollectPlanDetails AS DM
                    ON DM.SalesNO = DSM.SalesNO
                    AND DM.SalesRows = DSM.SalesRows
                    AND DM.DeleteDateTime IS Null
                    
                    LEFT OUTER JOIN D_Sales AS DS
                    ON DS.SalesNO = DSM.SalesNO
                    AND DS.DeleteDateTime IS Null

                    WHERE DSM.SalesNO = @SalesNO    
                    AND DSM.DeleteDateTime IS Null
                    AND DSM.JuchuuNO = @Number
                    AND DSM.JuchuuRows = @NumberRow               
                    ;
	        END

            
            --Table転送仕様Ｎ   insert 入出庫履歴 D_Warehousing             TemporaryTableRecordごとに１Record
            INSERT INTO [D_Warehousing]
               (--[WarehousingNO]
                [WarehousingDate]
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
            SELECT
               --<WarehousingNO
                CONVERT(date, @SalesDate)   --WarehousingDate
               ,DS.SoukoCD
               ,DS.RackNO
               ,DS.StockNO
               ,DSM.JanCD
               ,DSM.AdminNO
               ,DSM.SKUCD
               ,5   --WarehousingKBN 5：出荷売上
               ,0
               ,DSM.ShippingNO
               ,DSM.ShippingRows
               ,NULL
               ,NULL
               ,NULL
               ,NULL
               ,NULL
               ,NULL
               ,NULL
               ,NULL
               ,DJ.CustomerCD
               ,DSM.ShippingSu  --Quantity
               ,'TempoUriageNyuuryoku'
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM D_ShippingDetails AS DSM
            INNER JOIN D_Juchuu AS DJ ON DJ.JuchuuNO = DSM.Number
            INNER JOIN D_JuchuuDetails AS DJM
            ON DJM.JuchuuNO = DSM.Number
            AND DJM.JuchuuRows = DSM.NumberRows
            AND DJM.DeleteDateTime IS NULL
            INNER JOIN D_Reserve AS DR 
            ON DR.Number = DJM.JuchuuNO
            AND DR.NumberRows = DJM.JuchuuRows
            INNER JOIN D_Stock AS DS ON DS.StockNO = DR.StockNO
            WHERE DSM.ShippingNO = @ShippingNO
            AND DSM.Number = @Number
            AND DSM.NumberRows = @NumberRow
            ORDER BY DSM.ShippingNO, DSM.ShippingRows
            ;
            
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_BBB
            INTO @Number, @NumberRow, @ReserveNO, @SalesSu, @ShippingSu, @BillingCD, @SalesDate_VAR;

            SET @Rows = @Rows + 1;
        END
        
        --カーソルを閉じる
        CLOSE CUR_BBB;
        DEALLOCATE CUR_BBB;
        
        /*
        SET @Rows = 1;
        
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @JuchuuRows, @ShippingSu
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
           
           SET @Rows = @Rows + 1;
           
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @JuchuuRows, @ShippingSu;

        END
        
        UPDATE [D_SalesDetails] SET
            AddSalesRows = ISNULL((SELECT DS.SalesRows 
                            FROM D_JuchuuDetails AS DM
                            INNER JOIN D_SalesDetails AS DS
                            ON DS.JuchuuNO = DM.JuchuuNO
                            AND DS.JuchuuRows = DM.AddJuchuuRows
                            AND DS.DeleteDateTime IS NULL
                            WHERE DM.JuchuuNO = D_SalesDetails.JuchuuNO
                            AND DM.JuchuuRows = D_SalesDetails.JuchuuRows
                            AND D_SalesDetails.SalesNO = @SalesNO
                            AND D_SalesDetails.DeleteDateTime IS NULL
                            ),0)
        WHERE SalesNO = @SalesNO
        AND DeleteDateTime IS NULL
        ;
        
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
*/
       --Table転送仕様Ｏ    Insert売上履歴  黒
        INSERT INTO [D_SalesTran]
           ([ProcessKBN]
           ,[RecoredKBN]
           ,[SalesNO]
           ,[StoreCD]
           ,[SalesDate]
           ,[ShippingNO]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[BillingType]
           ,[SalesHontaiGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesTax]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesGaku]
           ,[LastPoint]
           ,[WaitingPoint]
           ,[StaffCD]
           ,[PrintDate]
           ,[PrintStaffCD]
           ,[StoreSalesUpdateFLG]
           ,[StoreSalesUpdatetime]
           ,[Discount]
           ,[Discount8]
           ,[Discount10]
           ,[DiscountTax]
           ,[DiscountTax8]
           ,[DiscountTax10]
           ,[PurchaseAmount]
           ,[TaxAmount]
           ,[DiscountAmount]
           ,[BillingAmount]
           ,[PointAmount]
           ,[CardDenominationCD]
           ,[CardAmount]
           ,[CashAmount]
           ,[DepositAmount]
           ,[RefundAmount]
           ,[CreditAmount]
           ,[Denomination1CD]
           ,[Denomination1Amount]
           ,[Denomination2CD]
           ,[Denomination2Amount]
           ,[AdvanceAmount]
           ,[TotalAmount]
           ,[InsertOperator]
           ,[InsertDateTime])
        SELECT
           1	--ProcessKBN
           ,0 	--RecoredKBN
           ,DS.SalesNO
           ,DS.StoreCD
           ,DS.SalesDate
           ,DS.ShippingNO
           ,DS.CustomerCD
           ,DS.CustomerName
           ,DS.CustomerName2
           ,DS.BillingType
           ,DS.SalesHontaiGaku
           ,DS.SalesHontaiGaku0
           ,DS.SalesHontaiGaku8
           ,DS.SalesHontaiGaku10
           ,DS.SalesTax
           ,DS.SalesTax8
           ,DS.SalesTax10
           ,DS.SalesGaku
           ,DS.LastPoint
           ,DS.WaitingPoint
           ,DS.StaffCD
           ,DS.PrintDate
           ,DS.PrintStaffCD
           ,9	--StoreSalesUpdateFLG
           ,@SYSDATETIME	--StoreSalesUpdatetime
           ,DS.Discount
           ,DS.Discount8
           ,DS.Discount10
           ,DS.DiscountTax
           ,DS.DiscountTax8
           ,DS.DiscountTax10
           ,DS.SalesGaku AS PurchaseAmount
           ,DS.SalesTax AS TaxAmount
           ,DS.Discount AS DiscountAmount
           ,DS.SalesGaku AS BillingAmount
           ,0 AS PointAmount
           ,NULL AS CardDenominationCD
           ,0 AS CardAmount
           ,0 AS CashAmount
           ,0 AS DepositAmount
           ,0 AS RefundAmount
           ,0 AS CreditAmount
           ,NULL AS Denomination1CD
           ,0 AS Denomination1Amount
           ,NULL AS Denomination2CD
           ,0 AS Denomination2Amount
           ,0 AS AdvanceAmount
           ,0 AS TotalAmount
           ,@Operator	--InsertOperator
           ,@SYSDATETIME	--InsertDateTime
           FROM D_Sales AS DS
           WHERE DS.SalesNO = @SalesNO
           ;
           
	   --Table転送仕様Ｐ	Insert売上明細履歴	黒
        INSERT INTO [D_SalesDetailsTran]
           ([DataNo]
           ,[DataRows]
           ,[ProcessKBN]
           ,[RecoredKBN]
           ,[SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[NotPrintFLG]
           ,[AddSalesRows]
           ,[ShippingNO]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[TaniCD]
           ,[SalesHontaiGaku]
           ,[SalesTax]
           ,[SalesGaku]
           ,[SalesTaxRitsu]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[ProperGaku]
           ,[DiscountGaku]
           ,[IndividualClientName]
           ,[DeliveryNoteFLG]
           ,[BillingPrintFLG]
           ,[DeleteOperator]
           ,[DeleteDateTime]
           ,[InsertOperator]
           ,[InsertDateTime])
        SELECT
           (SELECT IDENT_CURRENT('D_SalesTran'))	--(SELECT top 1 D.DataNo FROM D_SalesTran AS D WHERE D.SalesNO = @SalesNO ORDER BY D.DataNo desc)
           ,DS.SalesRows	--DataRows
           ,1	--1:追加 ProcessKBN
           ,0	--RecoredKBN
           ,DS.SalesNO
           ,DS.SalesRows
           ,DS.JuchuuNO
           ,DS.JuchuuRows
           ,DS.NotPrintFLG
           ,DS.AddSalesRows
           ,DS.ShippingNO
           ,DS.SKUCD
           ,DS.AdminNO
           ,DS.JanCD
           ,DS.SKUName
           ,DS.ColorName
           ,DS.SizeName
           ,DS.SalesSU
           ,DS.SalesUnitPrice
           ,DS.TaniCD
           ,DS.SalesHontaiGaku
           ,DS.SalesTax
           ,DS.SalesGaku
           ,DS.SalesTaxRitsu
           ,DS.CommentOutStore
           ,DS.CommentInStore
           ,DS.ProperGaku
           ,DS.DiscountGaku
           ,DS.IndividualClientName
           ,DS.DeliveryNoteFLG
           ,DS.BillingPrintFLG
           ,DS.DeleteOperator
           ,DS.DeleteDateTime
           ,@Operator
           ,@SYSDATETIME
           FROM D_SalesDetails AS DS
           WHERE DS.SalesNO = @SalesNO
           ;
    END
           
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';

        --カーソル定義
        DECLARE CUR_AAA CURSOR FOR
            SELECT tbl.DenNO
            FROM @Table tbl
            ORDER BY tbl.DenNO
            ;
            
        SET @OldNumber = '';
        
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @SalesNO;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
	        IF @OldNumber <> @SalesNO
	        BEGIN
                SET @OldNumber = @SalesNO;

                --処理履歴データへ更新
                SET @KeyItem = @SalesNO;
                
                EXEC L_Log_Insert_SP
                    @SYSDATETIME,
                    @Operator,
                    'TempoUriageNyuuryoku',
                    @PC,
                    @OperateModeNm,
                    @KeyItem;
                    
                --Table転送仕様Ｎ②　insert入出庫履歴 D_Warehousing（出荷取消）
                INSERT INTO [D_Warehousing]
                   (--[WarehousingNO]
                    [WarehousingDate]
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
                SELECT
                   --<WarehousingNO
                    CONVERT(date, @SYSDATETIME)   --WarehousingDate
                   ,DS.SoukoCD
                   ,DS.RackNO
                   ,DS.StockNO
                   ,DSM.JanCD
                   ,DSM.AdminNO
                   ,DSM.SKUCD
                   ,5   --WarehousingKBN 5：出荷売上
                   ,1
                   ,DSM.ShippingNO
                   ,DSM.ShippingRows
                   ,NULL
                   ,NULL
                   ,NULL
                   ,NULL
                   ,NULL
                   ,NULL
                   ,NULL
                   ,NULL
                   ,DH.CustomerCD   --D_Sales
                   ,DSM.ShippingSu*(-1) --Quantity
                   ,'TempoUriageNyuuryoku'
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
                FROM D_Sales AS DH
                INNER JOIN D_ShippingDetails AS DSM 
                ON DSM.ShippingNO = DH.ShippingNO
                AND DSM.DeleteDateTime IS NULL
                INNER JOIN D_Juchuu AS DJ 
                ON DJ.JuchuuNO = DSM.Number
                AND DJ.DeleteDateTime IS NULL
                INNER JOIN D_JuchuuDetails AS DJM
                ON DJM.JuchuuNO = DSM.Number
                AND DJM.JuchuuRows = DSM.NumberRows
                AND DJM.DeleteDateTime IS NULL
                INNER JOIN D_Reserve AS DR 
                ON DR.Number = DJM.JuchuuNO
                AND DR.NumberRows = DJM.JuchuuRows
                AND DR.DeleteDateTime IS NULL
                INNER JOIN D_Stock AS DS 
                ON DS.StockNO = DR.StockNO
                AND DS.DeleteDateTime IS NULL
                WHERE DH.SalesNO = @SalesNO
                AND DH.DeleteDateTime IS NULL
                ORDER BY DSM.ShippingNO, DSM.ShippingRows
                ;

                --在庫 Table転送仕様Ｂ②
                Update [D_Stock]
                   SET [StockSu] = [D_Stock].[StockSu] + DC.InstructionSu       --在庫数
                      ,[ReserveSu] = [D_Stock].[ReserveSu] + DC.InstructionSu   --引当数
                      ,[UpdateOperator] = @Operator
                      ,[UpdateDateTime] = @SYSDATETIME
                 FROM D_Reserve AS DR
                 INNER JOIN D_InstructionDetails AS DC
                 ON DC.ReserveNO = DR.ReserveNO
                 AND DC.DeleteDateTime IS NULL
                 INNER JOIN D_Instruction AS D
                 ON D.InstructionNO = DC.InstructionNO
                 AND D.DeleteDateTime IS NULL
                 INNER JOIN D_Shipping AS DS
                 ON DS.InstructionNO = D.InstructionNO
                 AND DS.DeleteDateTime IS NULL
                 INNER JOIN D_Sales AS DU
                 ON DS.ShippingNO = DU.ShippingNO
                 AND DU.DeleteDateTime IS NULL
                 WHERE [D_Stock].StockNO = DR.StockNO
                 AND [D_Stock].DeleteDateTime IS NULL
                 AND DU.SalesNO = @SalesNO
                 AND DR.DeleteDateTime IS NULL
                 ;  

                --引当 Table転送仕様Ａ②
                UPDATE D_Reserve
                    SET [UpdateOperator]     =  @Operator  
                       ,[UpdateDateTime]     =  @SYSDATETIME
                       ,[ShippingSu]  =  [ShippingSu] - DC.InstructionSu  --出荷数
                       ,[ReserveSu]   =  [ReserveSu] + DC.InstructionSu   --引当数
                 FROM D_InstructionDetails AS DC
                 INNER JOIN D_Instruction AS D
                 ON D.InstructionNO = DC.InstructionNO
                 AND D.DeleteDateTime IS NULL
                 INNER JOIN D_Shipping AS DS
                 ON DS.InstructionNO = D.InstructionNO
                 AND DS.DeleteDateTime IS NULL
                 INNER JOIN D_Sales AS DU
                 ON DS.ShippingNO = DU.ShippingNO
                 AND DU.DeleteDateTime IS NULL
                 WHERE [D_Reserve].ReserveNO = DC.ReserveNO
                 AND [D_Reserve].DeleteDateTime IS NULL
                 AND DU.SalesNO = @SalesNO
                 AND DC.DeleteDateTime IS NULL
                 ;
                 
                 --出荷指示明細
                 DELETE FROM D_InstructionDetails --AS DI
                 WHERE EXISTS(SELECT 1
                    FROM D_Instruction AS D
                     INNER JOIN D_Shipping AS DS
                     ON DS.InstructionNO = D.InstructionNO
                     AND DS.DeleteDateTime IS NULL
                     INNER JOIN D_Sales AS DU
                     ON DS.ShippingNO = DU.ShippingNO
                     AND DU.SalesNO = @SalesNO
                     AND DU.DeleteDateTime IS NULL
                     WHERE D.InstructionNO = D_InstructionDetails.InstructionNO
                     AND D.DeleteDateTime IS NULL             
                 );
                 
                 --出荷指示
                 DELETE FROM D_Instruction --AS D
                 WHERE EXISTS(SELECT 1
                    FROM D_Shipping AS DS
                     INNER JOIN D_Sales AS DU
                     ON DS.ShippingNO = DU.ShippingNO
                     AND DU.SalesNO = @SalesNO
                     AND DU.DeleteDateTime IS NULL
                    WHERE DS.InstructionNO = D_Instruction.InstructionNO
                    AND DS.DeleteDateTime IS NULL
                 );
                 
                 --出荷明細
                 DELETE FROM D_ShippingDetails --AS DSM
                 WHERE EXISTS(SELECT 1
                    FROM D_Shipping AS DS
                     INNER JOIN D_Sales AS DU
                     ON DS.ShippingNO = DU.ShippingNO
                     AND DU.SalesNO = @SalesNO
                     AND DU.DeleteDateTime IS NULL
                     WHERE DS.ShippingNO = D_ShippingDetails.ShippingNO
                     AND DS.DeleteDateTime IS NULL
                 );
                 
                 --出荷
                 DELETE FROM D_Shipping --AS DS
                 WHERE EXISTS(SELECT 1
                    FROM D_Sales AS DU
                     WHERE DU.SalesNO = @SalesNO
                     AND DU.DeleteDateTime IS NULL
                     AND D_Shipping.ShippingNO = DU.ShippingNO
                 );

                 --Table転送仕様Ｏ  Insert売上履歴  赤
                INSERT INTO [D_SalesTran]
                   ([ProcessKBN]
                   ,[RecoredKBN]
                   ,[SalesNO]
                   ,[StoreCD]
                   ,[SalesDate]
                   ,[ShippingNO]
                   ,[CustomerCD]
                   ,[CustomerName]
                   ,[CustomerName2]
                   ,[BillingType]
                   ,[SalesHontaiGaku]
                   ,[SalesHontaiGaku0]
                   ,[SalesHontaiGaku8]
                   ,[SalesHontaiGaku10]
                   ,[SalesTax]
                   ,[SalesTax8]
                   ,[SalesTax10]
                   ,[SalesGaku]
                   ,[LastPoint]
                   ,[WaitingPoint]
                   ,[StaffCD]
                   ,[PrintDate]
                   ,[PrintStaffCD]
                   ,[StoreSalesUpdateFLG]
                   ,[StoreSalesUpdatetime]
                   ,[Discount]
                   ,[Discount8]
                   ,[Discount10]
                   ,[DiscountTax]
                   ,[DiscountTax8]
                   ,[DiscountTax10]
                   ,[PurchaseAmount]
                   ,[TaxAmount]
                   ,[DiscountAmount]
                   ,[BillingAmount]
                   ,[PointAmount]
                   ,[CardDenominationCD]
                   ,[CardAmount]
                   ,[CashAmount]
                   ,[DepositAmount]
                   ,[RefundAmount]
                   ,[CreditAmount]
                   ,[Denomination1CD]
                   ,[Denomination1Amount]
                   ,[Denomination2CD]
                   ,[Denomination2Amount]
                   ,[AdvanceAmount]
                   ,[TotalAmount]
                   ,[InsertOperator]
                   ,[InsertDateTime])
                SELECT
                   3    --ProcessKBN
                   ,1   --RecoredKBN
                   ,DS.SalesNO
                   ,DS.StoreCD
                   ,CONVERT(date, @SalesDate)
                   ,DS.ShippingNO
                   ,DS.CustomerCD
                   ,DS.CustomerName
                   ,DS.CustomerName2
                   ,DS.BillingType
                   ,(-1)*DS.SalesHontaiGaku
                   ,(-1)*DS.SalesHontaiGaku0
                   ,(-1)*DS.SalesHontaiGaku8
                   ,(-1)*DS.SalesHontaiGaku10
                   ,(-1)*DS.SalesTax
                   ,(-1)*DS.SalesTax8
                   ,(-1)*DS.SalesTax10
                   ,(-1)*DS.SalesGaku
                   ,DS.LastPoint
                   ,DS.WaitingPoint
                   ,DS.StaffCD
                   ,DS.PrintDate
                   ,DS.PrintStaffCD
                   ,9   --StoreSalesUpdateFLG
                   ,@SYSDATETIME    --StoreSalesUpdatetime
                   ,(-1)*DS.Discount
                   ,(-1)*DS.Discount8
                   ,(-1)*DS.Discount10
                   ,(-1)*DS.DiscountTax
                   ,(-1)*DS.DiscountTax8
                   ,(-1)*DS.DiscountTax10
                   ,(-1)*DP.PurchaseAmount
                   ,(-1)*DP.TaxAmount
                   ,(-1)*DP.DiscountAmount
                   ,(-1)*DP.BillingAmount
                   ,(-1)*DP.PointAmount
                   ,DP.CardDenominationCD
                   ,(-1)*DP.CardAmount
                   ,(-1)*DP.CashAmount
                   ,(-1)*DP.DepositAmount
                   ,(-1)*DP.RefundAmount
                   ,(-1)*DP.CreditAmount
                   ,DP.Denomination1CD
                   ,DP.Denomination1Amount
                   ,(-1)*DP.Denomination2CD
                   ,(-1)*DP.Denomination2Amount
                   ,(-1)*DP.AdvanceAmount
                   ,(-1)*DP.TotalAmount
                   ,@Operator   --InsertOperator
                   ,@SYSDATETIME    --InsertDateTime
                   FROM D_Sales AS DS
                   LEFT OUTER JOIN D_StorePayment AS DP
                   ON DP.SalesNO = DS.SalesNO
                   WHERE DS.SalesNO = @SalesNO
                   ;

                 --Table転送仕様Ｐ  Insert売上明細履歴  赤
                INSERT INTO [D_SalesDetailsTran]
                   ([DataNo]
                   ,[DataRows]
                   ,[ProcessKBN]
                   ,[RecoredKBN]
                   ,[SalesNO]
                   ,[SalesRows]
                   ,[JuchuuNO]
                   ,[JuchuuRows]
                   ,[ShippingNO]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[SKUName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[SalesSU]
                   ,[SalesUnitPrice]
                   ,[TaniCD]
                   ,[SalesHontaiGaku]
                   ,[SalesTax]
                   ,[SalesGaku]
                   ,[SalesTaxRitsu]
                   ,[ProperGaku]
                   ,[DiscountGaku]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[IndividualClientName]
                   ,[DeliveryNoteFLG]
                   ,[BillingPrintFLG]
                   ,[DeleteOperator]
                   ,[DeleteDateTime]
                   ,[InsertOperator]
                   ,[InsertDateTime])
                SELECT
                   (SELECT IDENT_CURRENT('D_SalesTran'))--(SELECT top 1 D.DataNo FROM D_SalesTran AS D WHERE D.SalesNO = @SalesNO ORDER BY D.DataNo desc)
                   ,DS.SalesRows    --DataRows
                   ,3   --1:追加 ProcessKBN
                   ,1   --RecoredKBN
                   ,DS.SalesNO
                   ,DS.SalesRows
                   ,DS.JuchuuNO
                   ,DS.JuchuuRows
                   ,DS.ShippingNO
                   ,DS.SKUCD
                   ,DS.AdminNO
                   ,DS.JanCD
                   ,DS.SKUName
                   ,DS.ColorName
                   ,DS.SizeName
                   ,(-1)*DS.SalesSU
                   ,DS.SalesUnitPrice
                   ,DS.TaniCD
                   ,(-1)*DS.SalesHontaiGaku
                   ,(-1)*DS.SalesTax
                   ,(-1)*DS.SalesGaku
                   ,DS.SalesTaxRitsu
                   ,(-1)*DS.ProperGaku
                   ,(-1)*DS.DiscountGaku
                   ,DS.CommentOutStore
                   ,DS.CommentInStore
                   ,DS.IndividualClientName
                   ,DS.DeliveryNoteFLG
                   ,DS.BillingPrintFLG
                   ,DS.DeleteOperator
                   ,DS.DeleteDateTime
                   ,@Operator
                   ,@SYSDATETIME
                   FROM D_SalesDetails AS DS
                   WHERE DS.SalesNO = @SalesNO
                   ;

               --テーブル転送仕様Ｍ②受注明細
               UPDATE D_Juchuu
                SET [UpdateOperator] = @Operator
                   ,[UpdateDateTime] = @SYSDATETIME
               FROM D_SalesDetails AS DS
               WHERE D_Juchuu.JuchuuNO = DS.JuchuuNO
                AND D_Juchuu.DeleteDateTime IS NULL
                AND DS.SalesNO = @SalesNO
                ;
               
               UPDATE D_JuchuuDetails
                SET [SalesDate] = NULL
                   ,[SalesNO] = NULL
                   ,[UpdateOperator] = @Operator
                   ,[UpdateDateTime] = @SYSDATETIME
               FROM D_SalesDetails AS DS
               WHERE D_JuchuuDetails.JuchuuNO = DS.JuchuuNO
                AND D_JuchuuDetails.JuchuuRows = DS.JuchuuRows
                AND D_JuchuuDetails.DeleteDateTime IS NULL
                AND DS.SalesNO = @SalesNO
                ;
                
                 --売上明細        
                 DELETE FROM D_SalesDetails
                 WHERE SalesNO = @SalesNO
                 ;
                 
                 --売上
                 DELETE FROM D_Sales
                 WHERE SalesNO = @SalesNO
                 ;
                 
                 --回収予定明細
                 DELETE FROM D_CollectPlanDetails
                 WHERE SalesNO = @SalesNO
                 ;

                 --回収予定
                 DELETE FROM D_CollectPlan
                 WHERE SalesNO = @SalesNO
                 ;
                 
                 --請求
                 DELETE DB
                 FROM D_Billing AS DB
                 INNER JOIN D_BillingDetails AS DBM
                 ON DBM.BillingNO = DB.BillingNO
                 WHERE DBM.SalesNO = @SalesNO
                 ;
                 
                 --請求明細
                 DELETE FROM D_BillingDetails
                 WHERE SalesNO = @SalesNO
                 ;
            END
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @SalesNO;

        END
        
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
    END


    SET @OutSalesNO = @SalesNO;
    
--<<OWARI>>
  return @W_ERR;

END


