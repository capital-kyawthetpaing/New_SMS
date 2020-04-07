 BEGIN TRY 
 Drop Procedure dbo.[PRC_ShukkaUriageUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    出荷売上データ更新処理
--       Program ID      ShukkaUriageUpdate
--       Create date:    2020.2.5
--    ======================================================================
CREATE PROCEDURE [dbo].[PRC_ShukkaUriageUpdate]
    (@Operator  varchar(10),
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
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

	DECLARE @ShippingNO varchar(11);
	DECLARE @OldShippingNO varchar(11);
    DECLARE @ShippingRows int;
	DECLARE @JuchuuNO varchar(11);
    DECLARE @JuchuuRows int;
    DECLARE @ShippingDate varchar(10);
    DECLARE @StoreCD varchar(4);
    DECLARE @JuchuuKBN tinyint;
    DECLARE @PaymentMethodCD varchar(3);
    DECLARE @StaffCD varchar(10);
    
    DECLARE @SalesNO varchar(11);
    DECLARE @Rows int;
    DECLARE @BillingNO varchar(11);
                
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT DH.ShippingNO, DM.ShippingRows
            ,DM.Number, DM.NumberRows
            ,CONVERT(varchar,DH.ShippingDate,111) AS ShippingDate
            ,DJ.StoreCD
            ,DJ.JuchuuKBN
            ,DJ.PaymentMethodCD
            ,DH.StaffCD
        
        FROM D_Shipping AS DH
        LEFT OUTER JOIN D_ShippingDetails AS DM
        ON DM.ShippingNO = DH.ShippingNO
        AND DM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DM.Number
        AND DJM.JuchuuRows = DM.NumberRows
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Juchuu AS DJ
        ON DJ.JuchuuNO = DJ.JuchuuNO
        AND DJ.DeleteDateTime IS NULL
        WHERE DH.DeleteDateTime IS NULL
        AND DH.ShippingKBN = 1	--1:販売
        AND DH.SalesDateTime IS NULL
        ORDER BY DH.ShippingNO, DM.ShippingRows
        ;
    	
	SET @OldShippingNO = '';
	
    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO @ShippingNO, @ShippingRows, @JuchuuNO, @JuchuuRows, @ShippingDate, @StoreCD, @JuchuuKBN, @PaymentMethodCD, @StaffCD;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        IF @OldShippingNO <> @ShippingNO
        BEGIN
            SET @Rows = 1;
            
            --売上  テーブル転送仕様Ａ
            --伝票番号採番
            EXEC Fnc_GetNumber
                3,          --in伝票種別 3
                @ShippingDate, --in基準日
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
               
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
               @SalesNO
               ,@StoreCD
               ,CONVERT(date, @ShippingDate)
               ,@ShippingNO
               ,MAX(DJ.CustomerCD)
               ,MAX(DJ.CustomerName)
               ,MAX(DJ.CustomerName2)
               ,1	--1:都度 BillingType
               ,SUM(DJM.JuchuuHontaiGaku)    --売上本体額0 +売上本体額8 + 売上本体額10 SalesHontaiGaku
               ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN DJM.JuchuuHontaiGaku ELSE 0 END)  --SalesHontaiGaku0
               ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN DJM.JuchuuHontaiGaku ELSE 0 END)  --SalesHontaiGaku8
               ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN DJM.JuchuuHontaiGaku ELSE 0 END) --SalesHontaiGaku10
               ,SUM(DJM.JuchuuTax) 
               ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN DJM.JuchuuTax ELSE 0 END)  --SalesTax8
               ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN DJM.JuchuuTax ELSE 0 END) --SalesTax10
               ,SUM(DJM.JuchuuGaku)
               ,0	--LastPoint, money,>
               ,0	--WaitingPoint, money,>
               ,@StaffCD   --StaffCD
               ,NULL    --PrintDate
               ,NULL    --PrintStaffCD
               ,'Administrator'	--Operator
               ,@SYSDATETIME
               ,'Administrator' --Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
           FROM D_JuchuuDetails AS DJM
           INNER JOIN D_Juchuu AS DJ
            ON DJ.JuchuuNO = DJ.JuchuuNO
            AND DJ.DeleteDateTime IS NULL
           WHERE DJM.JuchuuNO = @JuchuuNO
           AND DJM.DeleteDateTime IS NULL
           GROUP BY DJM.JuchuuNO
           ;
            
            --売上履歴	テーブル転送仕様Ｃ
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
               ,[TotalAmount]
               ,[InsertOperator]
               ,[InsertDateTime])
            SELECT
               1    --ProcessKBN
               ,0   --RecoredKBN
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
               ,9   --StoreSalesUpdateFLG
               ,@SYSDATETIME    --StoreSalesUpdatetime
               ,DS.Discount
               ,DS.Discount8
               ,DS.Discount10
               ,DS.DiscountTax
               ,DS.DiscountTax8
               ,DS.DiscountTax10
               ,DS.SalesHontaiGaku	--PurchaseAmount
               ,DS.SalesTax			--TaxAmount
               ,DS.Discount
               ,DS.SalesGaku	--BillingAmount
               ,0	--PointAmount
               ,NULL	--CardDenominationCD
               ,0 	--CardAmount
               ,0 	--CashAmount
               ,0 	--DepositAmount
               ,0 	--RefundAmount
               ,0 	--CreditAmount
               ,NULL 	--Denomination1CD
               ,0 	--Denomination1Amount
               ,NULL 	--Denomination2CD
               ,0 	--Denomination2Amount
               ,DS.SalesGaku 	--TotalAmount
               ,'Administrator'   --InsertOperator
               ,@SYSDATETIME    --InsertDateTime
               FROM D_Sales AS DS
               LEFT OUTER JOIN D_StorePayment AS DP
               ON DP.SalesNO = DS.SalesNO
               WHERE DS.SalesNO = @SalesNO
               ;

            --テーブル転送仕様I受注明細
            UPDATE D_Juchuu
            SET [UpdateOperator] = @Operator
               ,[UpdateDateTime] = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            AND DeleteDateTime IS NULL
            ;

			--請求	テーブル転送仕様Ｇ	
            --伝票番号採番
            EXEC Fnc_GetNumber
                15,             --in伝票種別 15
                @ShippingDate,    --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @BillingNO OUTPUT
                ;
                    
            IF ISNULL(@BillingNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_Billing】INSERT
            INSERT INTO [D_Billing]
                   ([BillingNO]
                   ,[BillingType]  
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
                   ,1   --1:都度 BillingType   2019.10.23 add
                   ,@StoreCD
                   ,CONVERT(date, @ShippingDate)
                   ,CONVERT(date, @ShippingDate)
                   ,DS.CustomerCD      --BillingCustomerCD
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
                   ,0   -- BillingConfirmFlg
                   ,'Administrator'  
                   ,@SYSDATETIME
                   ,'Administrator'
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
               FROM D_Sales AS DS
               WHERE DS.SalesNO = @SalesNO
               ;

            --回収予定 テーブル転送仕様Ｅ
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
               ,[Datatype]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
               --CollectPlanNO
               @SalesNO
               ,@JuchuuNO
               ,@JuchuuKBN
               ,@StoreCD
               ,DS.CustomerCD       --BillingCustomerCD
               ,DS.SalesHontaiGaku      --HontaiGaku
               ,DS.SalesHontaiGaku0     --HontaiGaku0
               ,DS.SalesHontaiGaku8     --HontaiGaku8
               ,DS.SalesHontaiGaku10    --HontaiGaku10
               ,DS.SalesTax     --Tax
               ,DS.SalesTax8    --Tax8
               ,DS.SalesTax10   --Tax10
               ,DS.SalesGaku    --CollectPlanGaku
               ,DS.BillingType  --BillingType
               ,DS.SalesDate    --BillingDate, date,>
               ,@BillingNO
               ,NULL	--MonthlyBillingNO
               ,@PaymentMethodCD
               ,0   --CardProgressKBN
               ,0   --PaymentProgressKBN
               ,0   --InvalidFLG
               ,DS.SalesDate    --BillingCloseDate, date,>
               ,DS.SalesDate	--FirstCollectPlanDate
               ,0   --ReminderFLG
               ,NULL    --NoReminderDate
               ,DS.SalesDate    --NextCollectPlanDate
               ,NULL    --ActionCD
               ,NULL    --NextActionCD
               ,NULL    --LastReminderNO
               ,'ShukkaUriageUpdate'  --Program
               ,1   --BillingConfirmFlg
               ,0	--Datatype
               ,'Administrator'
               ,@SYSDATETIME
               ,'Administrator'
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
           FROM D_Sales AS DS
           WHERE DS.SalesNO = @SalesNO
           ;

		END

		--売上明細	テーブル転送仕様Ｂ
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
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[DeliveryNoteFLG]
           ,[BillingPrintFLG]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
            @SalesNO
           ,@Rows
           ,@JuchuuNO
           ,@JuchuuRows
           ,0 AS NotPrintFLG
           ,0 AS AddSalesRows
           ,@ShippingNO
           ,DM.AdminNO
           ,DM.SKUCD
           ,DM.JanCD
           ,DM.SKUName
           ,DM.ColorName
           ,DM.SizeName
           ,DM.JuchuuSuu
           ,DM.JuchuuUnitPrice  --SalesUnitPrice
           ,DM.TaniCD
           ,DM.JuchuuHontaiGaku    --SalesHontaiGaku
           ,DM.JuchuuTax
           ,DM.JuchuuGaku
           ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
           ,DM.JuchuuGaku	--ProperGaku
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,NULL  --IndividualClientName
           ,0	--DeliveryNoteFLG, tinyint,>
           ,0	--BillingPrintFLG, tinyint,>
           ,'Administrator'
           ,@SYSDATETIME
           ,'Administrator'
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DM
       WHERE DM.JuchuuNO = @JuchuuNO
       AND DM.JuchuuRows = @JuchuuRows
       ;
        
        --売上明細履歴  テーブル転送仕様Ｄ
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
           (SELECT IDENT_CURRENT('D_SalesTran'))    --(SELECT top 1 D.DataNo FROM D_SalesTran AS D WHERE D.SalesNO = @SalesNO ORDER BY D.DataNo desc)
           ,DS.SalesRows    --DataRows
           ,1   --1:追加 ProcessKBN
           ,0   --RecoredKBN
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
           ,DS.SalesSU
           ,DS.SalesUnitPrice
           ,DS.TaniCD
           ,DS.SalesHontaiGaku
           ,DS.SalesTax
           ,DS.SalesGaku
           ,DS.SalesTaxRitsu
           ,DS.CommentOutStore
           ,DS.CommentInStore
           ,DS.IndividualClientName
           ,DS.DeliveryNoteFLG
           ,DS.BillingPrintFLG
           ,DS.DeleteOperator
           ,DS.DeleteDateTime
           ,'Administrator'
           ,@SYSDATETIME
           FROM D_SalesDetails AS DS
           WHERE DS.SalesNO = @SalesNO
           AND DS.SalesRows = @Rows
           ;

		--請求明細	テーブル転送仕様Ｈ
        --【D_BillingDetails】INSERT 
        INSERT INTO [D_BillingDetails]
               ([BillingNO]
               ,[BillingType]	--2019.10.23 add
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
               ,1	--1:都度 BillingType
               ,DSM.SalesRows AS BillingRows
               ,@StoreCD
               ,DS.SalesDate AS BillingCloseDate
               ,DS.CustomerCD
               ,DSM.SalesNO
               ,DSM.SalesRows 
               ,(SELECT top 1 DH.CollectPlanNO 
	            FROM D_CollectPlan AS DH
	            WHERE DH.SalesNO = @SalesNO
	            ORDER BY CollectPlanNO desc) AS CollectPlanNO 
               ,DSM.SalesRows AS CollectPlanRows 
               ,DSM.SalesHontaiGaku 
               ,DSM.SalesTax 
               ,DSM.SalesGaku   --CollectPlanGaku 
               ,DSM.SalesTaxRitsu 
               ,0	--InvoiceFLG 
               ,'Administrator' 
               ,@SYSDATETIME
               ,'Administrator' 
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
               
        FROM D_SalesDetails AS DSM
        INNER JOIN D_Sales AS DS
        ON DS.SalesNO = DSM.SalesNO
        WHERE DSM.SalesNO = @SalesNO
        AND DSM.SalesRows = @Rows
        ;
            
        --回収予定明細	テーブル転送仕様Ｆ
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
           ,@JuchuuKBN   --JuchuuKBN
           ,DSM.SalesHontaiGaku --HontaiGaku
           ,DSM.SalesTax    --Tax
           ,DSM.SalesGaku   --CollectPlanGaku
           ,DSM.SalesTaxRitsu   --TaxRitsu
           ,DS.SalesDate
           ,0   --PaymentProgressKBN
           ,0   --BillingPrintFLG
           ,'Administrator'
           ,@SYSDATETIME
           ,'Administrator'
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_SalesDetails AS DSM
       INNER JOIN D_Sales AS DS
       ON DS.SalesNO = DSM.SalesNO
       WHERE DSM.SalesNO = @SalesNO
       AND DSM.SalesRows = @Rows
       ;

        SET @Rows = @Rows + 1;

      	--受注明細	テーブル転送仕様Ｉ
       	UPDATE D_JuchuuDetails
        SET [SalesDate] = CONVERT(date, @ShippingDate)
           ,[SalesNO] = @SalesNO
           --,[UpdateOperator] = @Operator
           --,[UpdateDateTime] = @SYSDATETIME
       	WHERE JuchuuNO = @JuchuuNO
        AND JuchuuRows = @JuchuuRows
        AND DeleteDateTime IS NULL
        ;

        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
    	INTO @ShippingNO, @ShippingRows, @JuchuuNO, @JuchuuRows, @ShippingDate, @StoreCD, @JuchuuKBN, @PaymentMethodCD, @StaffCD;

    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
    
    --出荷	テーブル転送仕様Ｊ
    UPDATE D_Shipping SET
    	[SalesDateTime] = @SYSDATETIME
       ,[UpdateOperator] = @Operator
       ,[UpdateDateTime] = @SYSDATETIME
    WHERE DeleteDateTime IS NULL
    AND ShippingKBN = 1	--1:販売
    AND SalesDateTime IS NULL
    ;
        
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'ShukkaUriageUpdate',
        @PC,
        NULL,
        NULL;
    
--<<OWARI>>
  return @W_ERR;

END



