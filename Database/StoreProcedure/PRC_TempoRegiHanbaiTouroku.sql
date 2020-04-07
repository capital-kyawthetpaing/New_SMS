 BEGIN TRY 
 Drop Procedure dbo.[PRC_TempoRegiHanbaiTouroku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PRC_TempoRegiHanbaiTouroku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @SalesNO  varchar(11),
    @JuchuuNO  varchar(11),
    @StoreCD   varchar(4),
    @CustomerCD   varchar(13),
    @Age  tinyint,	--2019.12.16 add
    
    @SalesRate int,
    @AdvanceAmount money,
    @PointAmount money,
    @CashAmount money,
    @RefundAmount money,
    @DenominationCD varchar(3),
    @CardAmount money,
    @Discount money,
    @CreditAmount money,
    @Denomination1Amount money,
    @DenominationCD1 varchar(3),
    @Denomination2Amount money,
    @DenominationCD2 varchar(3),
    @DepositAmount money,
    @Keijobi  varchar(10),
    
    --第一画面
    @SalesGaku money,   --お買上金額計
    @SalesTax money,    --うち税額
    
    @SalesHontaiGaku0 money,
    @SalesHontaiGaku8 money,
    @SalesHontaiGaku10 money,
    @HanbaiTax10 money,
    @HanbaiTax8 money,
    @InvoiceGaku money,
    @Discount8 money,
    @Discount10 money,
    @DiscountTax money,
    @DiscountTax8 money,
    @DiscountTax10 money,

    --第二画面
    @TotalAmount money,		--支払計
    
    @Table  T_TempoHanbai READONLY,
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
    DECLARE @ChangeDate varchar(10);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @ChangeDate = convert(VARCHAR,@SYSDATETIME,111);
    
    DECLARE @Program varchar(50);
    
    DECLARE @OldAdvanceAmount money;
    DECLARE @OldPointAmount money;
    DECLARE @OldCashAmount money;
    DECLARE @OldRefundAmount money;
    DECLARE @OldDenominationCD varchar(3);
    DECLARE @OldCardAmount money;
    DECLARE @OldDiscount money;
    DECLARE @OldCreditAmount money;
    DECLARE @OldDenomination1Amount money;
    DECLARE @OldDenominationCD1 varchar(3);
    DECLARE @OldDenomination2Amount money;
    DECLARE @OldDenominationCD2 varchar(3);
    
    SET @Program = 'TempoRegiHanbaiTouroku';
	
    
    --更新処理（通常販売）-----------------------------------------------------------------
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = NULL;

        --トリガーがあるのでヘッダ部からInsert
        --伝票番号採番
        EXEC Fnc_GetNumber
            1,             --in伝票種別 1
            @ChangeDate    , --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @JuchuuNO OUTPUT
            ;
        
        IF ISNULL(@JuchuuNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --テーブル転送仕様Ｆ(通常）     Insert受注 D_Juchuu
        --【D_Juchuu】
        INSERT INTO [D_Juchuu]
           ([JuchuuNO]
           ,[StoreCD]
           ,[JuchuuDate]
           ,[JuchuuTime]
           ,[ReturnFLG]
           ,[SoukoCD]
           ,[JuchuuKBN]
           ,[SiteKBN]
           ,[SiteJuchuuDateTime]
           ,[SiteJuchuuNO]
           ,[InportErrFLG]
           ,[OnHoldFLG]
           ,[IdentificationFLG]
           ,[TorikomiDateTime]
           ,[StaffCD]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[AliasKBN]
           ,[ZipCD1]
           ,[ZipCD2]
           ,[Address1]
           ,[Address2]
           ,[Tel11]
           ,[Tel12]
           ,[Tel13]
           ,[Tel21]
           ,[Tel22]
           ,[Tel23]
           ,[CustomerKanaName]
           ,[JuchuuCarrierCD]
           ,[DecidedCarrierFLG]
           ,[LastCarrierCD]
           ,[NameSortingDateTime]
           ,[NameSortingStaffCD]
           ,[CurrencyCD]
           ,[JuchuuGaku]
           ,[Discount]
           ,[HanbaiHontaiGaku]
           ,[HanbaiTax8]
           ,[HanbaiTax10]
           ,[HanbaiGaku]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[Coupon]
           ,[Point]
           ,[PayCharge]
           ,[Adjustments]
           ,[Postage]
           ,[GiftWrapCharge]
           ,[InvoiceGaku]
           ,[PaymentMethodCD]
           ,[PaymentPlanNO]
           ,[CardProgressKBN]
           ,[CardCompany]
           ,[CardNumber]
           ,[PaymentProgressKBN]
           ,[PresentFLG]
           ,[SalesPlanDate]
           ,[FirstPaypentPlanDate]
           ,[LastPaymentPlanDate]
           ,[DemandProgressKBN]
           ,[CommentDemand]
           ,[CancelDate]
           ,[CancelReasonKBN]
           ,[CancelRemarks]
           ,[NoMailFLG]
           ,[IndividualContactKBN]
           ,[TelephoneContactKBN]
           ,[LastMailKBN]
           ,[LastMailPatternCD]
           ,[LastMailDatetime]
           ,[LastMailName]
           ,[NextMailKBN]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[LastDepositeDate]
           ,[LastOrderDate]
           ,[LastArriveDate]
           ,[LastSalesDate]
           ,[MitsumoriNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[JuchuuDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@JuchuuNO                      
           ,@StoreCD                          
           ,convert(date,@SYSDATETIME)                    
           ,convert(Time,@SYSDATETIME)    --JuchuuTime
           ,0   --ReturnFLG
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            AND M.SoukoType= 3
            ORDER BY M.ChangeDate desc)

           ,2   --JuchuuKBN 2   店頭
           ,0   --SiteKBN
           ,NULL    --SiteJuchuuDateTime
           ,NULL    --SiteJuchuuNO
           ,0   --InportErrFLG
           ,0   --OnHoldFLG
           ,0   --IdentificationFLG
           ,NULL    --TorikomiDateTime
           ,@Operator
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
            */
           ,(SELECT top 1 M.AliasKBN FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel11 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel12 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel13 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel21 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel21
           ,(SELECT top 1 M.Tel22 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel22
           ,(SELECT top 1 M.Tel23 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel23
           ,(SELECT top 1 M.KanaName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --CustomerKanaName
           ,NULL    --JuchuuCarrierCD
           ,0   --DecidedCarrierFLG
           ,NULL    --LastCarrierCD
           ,NULL    --NameSortingDateTime
           ,NULL    --NameSortingStaffCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
           ,@SalesGaku + @Discount
           ,@Discount
           ,@SalesGaku - @SalesTax
           ,@HanbaiTax8
           ,@HanbaiTax10
           ,@SalesGaku
           ,0   --CostGaku
           ,@SalesGaku - @SalesTax
           ,0   --Coupon
           ,0   --Point
           ,0   --PayCharge
           ,0   --Adjustments
           ,0   --Postage
           ,0   --GiftWrapCharge
           ,@InvoiceGaku
           ,NULL	--PaymentMethodCD
           ,0	--PaymentPlanNO
           ,0   --CardProgressKBN
           ,NULL    --CardCompany
           ,NULL    --CardNumber
           ,0   --PaymentProgressKBN
           ,0   --PresentFLG
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,0   --DemandProgressKBN
           ,NULL    --CommentDemand
           ,NULL    --CancelDate
           ,NULL    --CancelReasonKBN
           ,NULL    --CancelRemarks
           ,0   --NoMailFLG
           ,0   --IndividualContactKBN
           ,0   --TelephoneContactKBN
           ,NULL    --LastMailKBN
           ,NULL    --LastMailPatternCD
           ,NULL    --LastMailDatetime
           ,NULL    --LastMailName
           ,NULL    --NextMailKBN
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,convert(date,@SYSDATETIME)    --LastDepositeDate
           ,NULL    --LastOrderDate
           ,NULL    --LastArriveDate
           ,convert(date,@SYSDATETIME)    --LastSalesDate
           ,NULL    --MitsumoriNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL                  
           ,NULL
           );               

        --テーブル転送仕様Ｇ(通常）     Insert受注明細 D_JuchuuDetails    
       INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[JanCD]
                   ,[SKUName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[SetKBN]
                   ,[SetRows]
                   ,[JuchuuSuu]
                   ,[JuchuuUnitPrice]
                   ,[TaniCD]
                   ,[JuchuuGaku]
                   ,[JuchuuHontaiGaku]
                   ,[JuchuuTax]
                   ,[JuchuuTaxRitsu]
                   ,[CostUnitPrice]
                   ,[CostGaku]
                   ,[ProfitGaku]
                   ,[SoukoCD]
                   ,[HikiateSu]
                   ,[DeliveryOrderSu]
                   ,[DeliverySu]
                   ,[DirectFLG]
                   ,[HikiateFLG]
                   ,[JuchuuOrderNO]
                   ,[VendorCD]
                   ,[LastOrderNO]
                   ,[LastOrderRows]
                   ,[LastOrderDateTime]
                   ,[DesiredDeliveryDate]
                   ,[AnswerFLG]
                   ,[ArrivePlanDate]
                   ,[ArrivePlanNO]
                   ,[ArriveDateTime]
                   ,[ArriveNO]
                   ,[ArribveNORows]
                   ,[DeliveryPlanNO]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[IndividualClientName]
                   ,[ShippingPlanDate]
                   ,[SalesDate]
                   ,[SalesNO]
                   ,[DepositeDetailNO]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @JuchuuNO                         
                   ,tbl.JuchuuRows                       
                   ,tbl.DisplayRows                      
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,tbl.JuchuuGaku
                   ,tbl.JuchuuHontaiGaku
                   ,tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,0	--CostUnitPrice
                   ,0	--CostGaku
                   ,tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,tbl.JuchuuSuu --HikiateSu
                   ,tbl.JuchuuSuu --DeliveryOrderSu
                   ,tbl.JuchuuSuu --DeliverySu
                   ,0	--DirectFLG
                   ,0 --HikiateFLG
                   ,NULL --JuchuuOrderNO
                   ,NULL	--VendorCD
                   ,NULL    --LastOrderNO
                   ,0   --LastOrderRows
                   ,NULL    --LastOrderDateTime
                   ,NULL    --DesiredDeliveryDate
                   ,0   --AnswerFLG
                   ,NULL	--ArrivePlanDate
                   ,NULL    --ArrivePlanNO
                   ,NULL    --ArriveDateTime
                   ,NULL    --ArriveNO
                   ,0   --ArribveNORows
                   ,NULL    --DeliveryPlanNO
                   ,NULL	--CommentOutStore
                   ,NULL	--CommentInStore
                   ,NULL	--IndividualClientName
                   ,convert(date,@SYSDATETIME)  --ShippingPlanDate      2020.01.30 add
                   ,convert(date,@SYSDATETIME)  --SalesDate     2020.01.30 add
                   ,NULL    --SalesNO
                   ,NULL    --DepositeDetailNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
                                                                       
        --テーブル転送仕様Ｆ履歴        Insert受注履歴 
        --テーブル転送仕様Ｇ履歴        Insert受注明細履歴
        --Trigger
         
        --テーブル転送仕様Ａ(通常）     Insert売上 D_Sales
        --伝票番号採番
        EXEC Fnc_GetNumber
            3,          --in伝票種別 3
            @ChangeDate    , --in基準日
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
           ,[Age]	--2019.12.16 add
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
           ,CONVERT(date, @SYSDATETIME)
           ,NULL	--ShippingNO
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
                WHERE M.CustomerCD = @CustomerCD
                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
                WHERE M.CustomerCD = @CustomerCD
                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc)
            */
           ,1	--BillingType
           ,@Age	--2019.12.16 add
           ,@SalesGaku-@SalesTax    --SalesHontaiGaku
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku0
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku8
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END) --SalesHontaiGaku10
           ,SUM(tbl.JuchuuTax) 
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuTax ELSE 0 END)  --SalesTax8
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuTax ELSE 0 END) --SalesTax10
           ,@SalesGaku
           ,0	--<LastPoint, money,>
           ,0	--<WaitingPoint, money,>
           ,@Operator   --StaffCD
           ,NULL    --PrintDate
           ,NULL    --PrintStaffCD
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DJM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DJM.JuchuuRows
       WHERE DJM.JuchuuNO = @JuchuuNO
       GROUP BY DJM.JuchuuNO
       ;
                                          
        --テーブル転送仕様Ｂ(通常)      Insert売上明細   D_SalesDetails  
       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
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
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
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
           ,DM.JuchuuRows
           ,DM.JuchuuNO
           ,DM.JuchuuRows
           ,NULL	--ShippingNO
           ,DM.AdminNO
           ,DM.SKUCD
           ,DM.JanCD
           ,DM.SKUName
           ,DM.ColorName
           ,DM.SizeName
           ,DM.JuchuuSuu	--SalesSU
           ,DM.JuchuuUnitPrice  --SalesUnitPrice
           ,DM.TaniCD
           ,DM.JuchuuHontaiGaku    --SalesHontaiGaku
           ,DM.JuchuuTax
           ,DM.JuchuuGaku	--SalesGaku
           ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
           ,tbl.ProperGaku	--2019.12.12 add
		   ,tbl.ProperGaku - DM.JuchuuGaku AS DiscountGaku	--2019.12.12 add
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,NULL  --IndividualClientName
           ,1	--<DeliveryNoteFLG, tinyint,>
           ,1	--<BillingPrintFLG, tinyint,>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DM.JuchuuRows
       WHERE DM.JuchuuNO = @JuchuuNO
       ;
            
        --テーブル転送仕様Ｈ         Insert店舗入金
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
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
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             VALUES
                   (@SalesNO
                   ,IDENT_CURRENT('D_SalesTran')
                   ,@StoreCD
                   ,1	--1:通常、2:返品、3;訂正、4:取消
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME)
                   ;
                                       
        --テーブル転送仕様Ｉ	Insert売上推移・黒
        --テーブル転送仕様Ｊ	Insert売上明細推移・黒                   
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,1  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1  --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
            
        --テーブル転送仕様Ｃ(通常)      Insert店舗入出金履歴　D_DepositHistory  売上のヘッダ情報    
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,0	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,@SalesGaku - @SalesTax	--SalesGaku, money,>
           ,@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>★
           ,@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
			;

        --テーブル転送仕様Ｄ(通常) Insert店舗入出金履歴　D_DepositHistory  売上の明細情報  
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,2	--DepositKBN
           ,0	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,tbl.JuchuuRows	--Rows, tinyint,>★
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku  --TotalGaku, money,>
           ,0   --Refund, money,>
           ,tbl.ProperGaku                          --ProperGaku 2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku         --DiscountGaku 2019.12.12 add
           ,0   --IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
	      FROM @Table tbl
	      WHERE tbl.UpdateFlg = 0
       ;

        --入金：ポイントの場合
        IF @PointAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ①(通常) insert店舗入出金履歴　D_DepositHistory  ポイントありの時
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        END
        
        --現金ありの時    
        IF @CashAmount > 0
        BEGIN
        	----テーブル転送仕様Ｅ閼'(通常) insert店舗入出金履歴　D_DepositHistory  現金ありの時    
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;

        END
        
        --カードありの時  
        IF @CardAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ③(通常) Insert店舗入出金履歴　D_DepositHistory  カードありの時  
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END
        
        --値引きありの時  
        IF @Discount <> 0
        BEGIN
        	--テーブル転送仕様Ｅ④(通常) Insert店舗入出金履歴　D_DepositHistory  値引きありの時  
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;
        END
        
        --その他①、②ありの時    その他の場合 MAX２レコード
        IF @Denomination1Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        IF @Denomination2Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END
        
        --入金：前受金の場合
        IF @AdvanceAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ⑦(通常) insert店舗入出金履歴　D_DepositHistory  ポイントありの時
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;
        END
       
    END
    
    --更新処理（返品）-------------------------------------------------------------
    ELSE IF @OperateMode = 4 --返品
    BEGIN
        SET @OperateModeNm = '返品';
        
        --【D_Juchuu】
        --伝票番号採番
        EXEC Fnc_GetNumber
            1,             --in伝票種別 1
            @ChangeDate    , --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @JuchuuNO OUTPUT
            ;
        
        IF ISNULL(@JuchuuNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END

        --テーブル転送仕様Ｆ(返品） Insert受注 D_Juchuu   
        --【D_Juchuu】
        INSERT INTO [D_Juchuu]
           ([JuchuuNO]
           ,[StoreCD]
           ,[JuchuuDate]
           ,[JuchuuTime]
           ,[ReturnFLG]
           ,[SoukoCD]
           ,[JuchuuKBN]
           ,[SiteKBN]
           ,[SiteJuchuuDateTime]
           ,[SiteJuchuuNO]
           ,[InportErrFLG]
           ,[OnHoldFLG]
           ,[IdentificationFLG]
           ,[TorikomiDateTime]
           ,[StaffCD]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[AliasKBN]
           ,[ZipCD1]
           ,[ZipCD2]
           ,[Address1]
           ,[Address2]
           ,[Tel11]
           ,[Tel12]
           ,[Tel13]
           ,[Tel21]
           ,[Tel22]
           ,[Tel23]
           ,[CustomerKanaName]
           ,[JuchuuCarrierCD]
           ,[DecidedCarrierFLG]
           ,[LastCarrierCD]
           ,[NameSortingDateTime]
           ,[NameSortingStaffCD]
           ,[CurrencyCD]
           ,[JuchuuGaku]
           ,[Discount]
           ,[HanbaiHontaiGaku]
           ,[HanbaiTax8]
           ,[HanbaiTax10]
           ,[HanbaiGaku]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[Coupon]
           ,[Point]
           ,[PayCharge]
           ,[Adjustments]
           ,[Postage]
           ,[GiftWrapCharge]
           ,[InvoiceGaku]
           ,[PaymentMethodCD]
           ,[PaymentPlanNO]
           ,[CardProgressKBN]
           ,[CardCompany]
           ,[CardNumber]
           ,[PaymentProgressKBN]
           ,[PresentFLG]
           ,[SalesPlanDate]
           ,[FirstPaypentPlanDate]
           ,[LastPaymentPlanDate]
           ,[DemandProgressKBN]
           ,[CommentDemand]
           ,[CancelDate]
           ,[CancelReasonKBN]
           ,[CancelRemarks]
           ,[NoMailFLG]
           ,[IndividualContactKBN]
           ,[TelephoneContactKBN]
           ,[LastMailKBN]
           ,[LastMailPatternCD]
           ,[LastMailDatetime]
           ,[LastMailName]
           ,[NextMailKBN]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[LastDepositeDate]
           ,[LastOrderDate]
           ,[LastArriveDate]
           ,[LastSalesDate]
           ,[MitsumoriNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[JuchuuDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@JuchuuNO                      
           ,@StoreCD                          
           ,convert(date,@SYSDATETIME)                    
           ,convert(Time,@SYSDATETIME)    --JuchuuTime
           ,1   --ReturnFLG★
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            AND M.SoukoType= 3
            ORDER BY M.ChangeDate desc)

           ,2   --JuchuuKBN 2   店頭
           ,0   --SiteKBN
           ,NULL    --SiteJuchuuDateTime
           ,NULL    --SiteJuchuuNO
           ,0   --InportErrFLG
           ,0   --OnHoldFLG
           ,0   --IdentificationFLG
           ,NULL    --TorikomiDateTime
           ,@Operator
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL	--CustomerName2
           ,(SELECT top 1 M.AliasKBN FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel11 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel12 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel13 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel21 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel21
           ,(SELECT top 1 M.Tel22 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel22
           ,(SELECT top 1 M.Tel23 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel23
           ,(SELECT top 1 M.KanaName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --CustomerKanaName
           ,NULL    --JuchuuCarrierCD
           ,0   --DecidedCarrierFLG
           ,NULL    --LastCarrierCD
           ,NULL    --NameSortingDateTime
           ,NULL    --NameSortingStaffCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
           ,-1* (@SalesGaku + @Discount)
           ,-1* @Discount
           ,-1* (@SalesGaku - @SalesTax)
           ,-1* @HanbaiTax8
           ,-1* @HanbaiTax10
           ,-1* @SalesGaku
           ,0   --CostGaku
           ,-1* (@SalesGaku - @SalesTax)
           ,0   --Coupon
           ,0   --Point
           ,0   --PayCharge
           ,0   --Adjustments
           ,0   --Postage
           ,0   --GiftWrapCharge
           ,-1* @InvoiceGaku
           ,NULL	--PaymentMethodCD
           ,0	--PaymentPlanNO
           ,0   --CardProgressKBN
           ,NULL    --CardCompany
           ,NULL    --CardNumber
           ,0   --PaymentProgressKBN
           ,0   --PresentFLG
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,0   --DemandProgressKBN
           ,NULL    --CommentDemand
           ,NULL    --CancelDate
           ,NULL    --CancelReasonKBN
           ,NULL    --CancelRemarks
           ,0   --NoMailFLG
           ,0   --IndividualContactKBN
           ,0   --TelephoneContactKBN
           ,NULL    --LastMailKBN
           ,NULL    --LastMailPatternCD
           ,NULL    --LastMailDatetime
           ,NULL    --LastMailName
           ,NULL    --NextMailKBN
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,convert(date,@SYSDATETIME)    --LastDepositeDate
           ,NULL    --LastOrderDate
           ,NULL    --LastArriveDate
           ,convert(date,@SYSDATETIME)    --LastSalesDate
           ,NULL    --MitsumoriNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL                  
           ,NULL
           );                     
                         
       --テーブル転送仕様Ｇ(返品） Insert受注明細  D_JuchuuDetails
       INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[JanCD]
                   ,[SKUName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[SetKBN]
                   ,[SetRows]
                   ,[JuchuuSuu]
                   ,[JuchuuUnitPrice]
                   ,[TaniCD]
                   ,[JuchuuGaku]
                   ,[JuchuuHontaiGaku]
                   ,[JuchuuTax]
                   ,[JuchuuTaxRitsu]
                   ,[CostUnitPrice]
                   ,[CostGaku]
                   ,[ProfitGaku]
                   ,[SoukoCD]
                   ,[HikiateSu]
                   ,[DeliveryOrderSu]
                   ,[DeliverySu]
                   ,[DirectFLG]
                   ,[HikiateFLG]
                   ,[JuchuuOrderNO]
                   ,[VendorCD]
                   ,[LastOrderNO]
                   ,[LastOrderRows]
                   ,[LastOrderDateTime]
                   ,[DesiredDeliveryDate]
                   ,[AnswerFLG]
                   ,[ArrivePlanDate]
                   ,[ArrivePlanNO]
                   ,[ArriveDateTime]
                   ,[ArriveNO]
                   ,[ArribveNORows]
                   ,[DeliveryPlanNO]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[IndividualClientName]
                   ,[ShippingPlanDate]
                   ,[SalesDate]
                   ,[SalesNO]
                   ,[DepositeDetailNO]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @JuchuuNO                         
                   ,tbl.JuchuuRows                       
                   ,tbl.DisplayRows                      
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,-1* tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,-1* tbl.JuchuuGaku
                   ,-1* tbl.JuchuuHontaiGaku
                   ,-1* tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,0	--CostUnitPrice
                   ,0	--CostGaku
                   ,-1* tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,-1* tbl.JuchuuSuu --HikiateSu
                   ,-1* tbl.JuchuuSuu --DeliveryOrderSu
                   ,-1* tbl.JuchuuSuu --DeliverySu
                   ,0	--DirectFLG
                   ,0 --HikiateFLG
                   ,NULL --JuchuuOrderNO
                   ,NULL	--VendorCD
                   ,NULL    --LastOrderNO
                   ,0   --LastOrderRows
                   ,NULL    --LastOrderDateTime
                   ,NULL    --DesiredDeliveryDate
                   ,0   --AnswerFLG
                   ,NULL	--ArrivePlanDate
                   ,NULL    --ArrivePlanNO
                   ,NULL    --ArriveDateTime
                   ,NULL    --ArriveNO
                   ,0   --ArribveNORows
                   ,NULL    --DeliveryPlanNO
                   ,NULL	--CommentOutStore
                   ,NULL	--CommentInStore
                   ,NULL	--IndividualClientName
                   ,NULL    --ShippingPlanDate      2020.01.30 add
                   ,NULL    --SalesDate
                   ,NULL    --SalesNO
                   ,NULL    --DepositeDetailNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
                                                                                
        --テーブル転送仕様Ｆ履歴    Insert受注履歴  
        --テーブル転送仕様Ｇ履歴    Insert受注明細履歴                                                                                      
        --テーブル転送仕様Ａ(返品） Insert売上 D_Sales
        --伝票番号採番
        EXEC Fnc_GetNumber
            3,          --in伝票種別 3
            @ChangeDate    , --in基準日
            @StoreCD,    --in店舗CD
            @Operator,
            @SalesNO OUTPUT
            ;

        IF ISNULL(@SalesNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
                                                                               
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
           ,CONVERT(date, @SYSDATETIME)
           ,NULL	--ShippingNO
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)*/
           ,1	--BillingType
           ,(-1)*(@SalesGaku-@SalesTax)    --SalesHontaiGaku
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku0
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku8
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END) --SalesHontaiGaku10
           ,(-1)*SUM(tbl.JuchuuTax) 
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuTax ELSE 0 END)  --SalesTax8
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuTax ELSE 0 END) --SalesTax10
           ,(-1)*@SalesGaku
           ,0	--<LastPoint, money,>
           ,0	--<WaitingPoint, money,>
           ,@Operator   --StaffCD
           ,NULL    --PrintDate
           ,NULL    --PrintStaffCD
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DJM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DJM.JuchuuRows
       WHERE DJM.JuchuuNO = @JuchuuNO
       GROUP BY DJM.JuchuuNO
       ;
   
        --テーブル転送仕様Ｂ(返品)  Insert売上明細  D_SalesDetails                                                              
       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
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
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
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
           ,DM.JuchuuRows
           ,DM.JuchuuNO
           ,DM.JuchuuRows
           ,NULL	--ShippingNO
           ,DM.AdminNO
           ,DM.SKUCD
           ,DM.JanCD
           ,DM.SKUName
           ,DM.ColorName
           ,DM.SizeName
           ,DM.JuchuuSuu	--SalesSU
           ,DM.JuchuuUnitPrice  --SalesUnitPrice
           ,DM.TaniCD
           ,DM.JuchuuHontaiGaku    --SalesHontaiGaku
           ,DM.JuchuuTax
           ,DM.JuchuuGaku	--SalesGaku
           ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
           ,-1* tbl.ProperGaku	--2019.12.12 add
		   ,-1* (tbl.ProperGaku - DM.JuchuuGaku) AS DiscountGaku	--2019.12.12 add
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,NULL  --IndividualClientName
           ,1	--<DeliveryNoteFLG, tinyint,>
           ,1	--<BillingPrintFLG, tinyint,>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DM.JuchuuRows
       WHERE DM.JuchuuNO = @JuchuuNO
       ;
 
        --テーブル転送仕様Ａ履歴    Insert売上履歴
        --テーブル転送仕様Ｂ履歴    Insert売上明細履歴

        --テーブル転送仕様Ｈ         Insert店舗入金
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
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
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT
                    @SalesNO
                   ,IDENT_CURRENT('D_SalesTran')
                   ,@StoreCD
                   ,2	--1:通常、2:返品、3;訂正、4:取消
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ;

        --テーブル転送仕様Ｉ        Insert売上推移・黒                                  
        --テーブル転送仕様Ｊ        Insert売上明細推移・黒
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,4  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1 --SIGN int,		D_Salesと同じ符号のため
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
                                                                                                                                                
        --テーブル転送仕様Ｃ(返品)  Insert店舗入出金履歴　D_DepositHistory売上のヘッダ情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,2	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(-1)*(@SalesGaku - @SalesTax)	--SalesGaku, money,>
           ,(-1)*@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>★
           ,(-1)*@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
			;
        
        --テーブル転送仕様Ｄ(返品)  Insert店舗入出金履歴　D_DepositHistory売上の明細情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
           ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
           ,2   --CancelKBN 1:取消、2:返品、3:訂正
           ,0   --RecoredKBN 0:黒、1:赤
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@SYSDATETIME)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,tbl.JuchuuRows   --Rows, tinyint,>★
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,tbl.ProperGaku                                  --2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku --2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
          FROM @Table tbl
          WHERE tbl.UpdateFlg = 0
          ;

        --入金：ポイントの場合
        IF @PointAmount > 0
        BEGIN
            --テーブル転送仕様Ｅ①(返品)insert店舗入出金履歴　D_DepositHistoryポイントありの時
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,-1 --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        END

        --現金ありの時    
        IF @CashAmount > 0
        BEGIN
        --テーブル転送仕様Ｅ閼(返品)insert店舗入出金履歴　D_DepositHistory現金ありの時
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;
        END
                
        --カードありの時  
        IF @CardAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ③(返品)Insert店舗入出金履歴　D_DepositHistoryカードありの時
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END

        --値引きありの時  
        IF @Discount <> 0
        BEGIN
        	--テーブル転送仕様Ｅ④(返品)Insert店舗入出金履歴　D_DepositHistory値引きありの時
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;
        END
                                      
        --テーブル転送仕様Ｅ⑤(返品)Insert店舗入出金履歴　D_DepositHistoryその他①、②ありの時
        --その他①、②ありの時    その他の場合 MAX２レコード
        IF @Denomination1Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        
        IF @Denomination2Amount > 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END
        
        --入金：前受金の場合
        IF @AdvanceAmount > 0
        BEGIN
            --テーブル転送仕様Ｅ⑦(返品)insert店舗入出金履歴　D_DepositHistoryポイントありの時
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,-1 --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;
        END
    END

    --更新処理（訂正）-------------------------------------------------------------
    ELSE IF @OperateMode = 2 --訂正--
    BEGIN
        SET @OperateModeNm = '訂正';
        --テーブル転送仕様Ｄ(訂正)	Insert 店舗入出金履歴　D_DepositHistory	売上の明細情報	  訂正前
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
           ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
           ,3   --CancelKBN 1:取消、2:返品、3:訂正
           ,1   --RecoredKBN 0:黒、1:赤
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,DM.SalesRows   --Rows, tinyint,>
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,DM.AdminNO	--AdminNO, int,>
           ,DM.SKUCD	--SKUCD, varchar(30),>
           ,DM.JanCD	--JanCD, varchar(13),>
           ,DM.SalesSU	--SalesSU, money,>
           ,DM.SalesUnitPrice	--SalesUnitPrice, money,>
           ,DM.SalesGaku - DM.SalesTax	--SalesGaku, money,>
           ,DM.SalesTax	--SalesTax, money,>
           ,DM.SalesTaxRitsu 	--SalesTaxRate, int,>
           ,DM.SalesGaku 	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,DM.ProperGaku	--2019.12.12 add
		   ,DM.DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_SalesDetails AS DM	
       WHERE DM.SalesNO = @SalesNO
       ;

        SELECT @OldDiscount = DS.DiscountAmount
        --  ,DS.BillingAmount
            ,@OldAdvanceAmount = DS.AdvanceAmount
            ,@OldPointAmount = DS.PointAmount
            ,@OldDenominationCD = DS.CardDenominationCD
            ,@OldCardAmount = DS.CardAmount
            ,@OldCashAmount = DS.CashAmount
        --  ,DS.DepositAmount
            ,@OldRefundAmount = DS.RefundAmount
            ,@OldCreditAmount = DS.CreditAmount
            ,@OldDenominationCD1 = DS.Denomination1CD
            ,@OldDenomination1Amount = DS.Denomination1Amount
            ,@OldDenominationCD2 = DS.Denomination2CD
            ,@OldDenomination2Amount = DS.Denomination2Amount
        FROM D_StorePayment As DS
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
        
        IF @OldPointAmount > 0
        BEGIN
	        --テーブル転送仕様Ｅ①(訂正)insert 店舗入出金履歴　D_DepositHistory ポイントありの時  訂正前
	        EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint, 1:取消、2:返品、3:訂正
                ,1	--@RecoredKBN tinyint,0:黒、1:赤
                ,@OldPointAmount
                ;
        END        
        
		--現金ありの時    
		IF @OldCashAmount > 0
		BEGIN
        	--テーブル転送仕様Ｅ閼(訂正)insert 店舗入出金履歴　D_DepositHistory 現金ありの時      訂正前
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCashAmount
                ,@OldRefundAmount
                ;
        END
        
        --カードありの時  
        IF @OldCardAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ③(訂正)Insert 店舗入出金履歴　D_DepositHistory カードありの時    訂正前
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD
                ,@OldCardAmount
                ;
        
        END
        
        --値引きありの時  
        IF @OldDiscount <> 0
        BEGIN
        	--テーブル転送仕様Ｅ④(訂正)Insert 店舗入出金履歴　D_DepositHistory 値引きありの時    訂正前
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDiscount
                ;
        END
        
        --テーブル転送仕様Ｅ⑤(訂正)Insert 店舗入出金履歴　D_DepositHistory その他①、②ありの時 訂正前
        IF @OldDenomination1Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD1
                ,@OldDenomination1Amount
                ;
        END
        
        IF @OldDenomination2Amount > 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD2
                ,@OldDenomination2Amount
                ;
        END
        
        --テーブル転送仕様Ｅ⑥(訂正)Insert 店舗入出金履歴　D_DepositHistory 掛ありの時        訂正前
        IF @OldCreditAmount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCreditAmount
                ;
        END
        
		--前受金ありの時    
		IF @OldAdvanceAmount > 0
		BEGIN
        	--テーブル転送仕様Ｅ閼(訂正)insert 店舗入出金履歴　D_DepositHistory 現金ありの時      訂正前
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@CustomerCD
                ,@OldAdvanceAmount
                ;
        END
        
        --テーブル転送仕様Ｆ(訂正）Update 受注　      D_Juchuu
        UPDATE D_Juchuu SET
               [JuchuuDate] = CONVERT(date, @Keijobi)
              ,[JuchuuGaku] =       @SalesGaku + @Discount
              ,[Discount] =         @Discount
              ,[HanbaiHontaiGaku] = @SalesGaku - @SalesTax
              ,[HanbaiTax8] =       @HanbaiTax8
              ,[HanbaiTax10] =      @HanbaiTax10
              ,[HanbaiGaku] =       @SalesGaku
              ,[CostGaku] = 0
              ,[ProfitGaku] =       @SalesGaku - @SalesTax                             
              ,[InvoiceGaku] =      @InvoiceGaku
              ,[SalesPlanDate] = CONVERT(date, @Keijobi)
              ,[FirstPaypentPlanDate] = CONVERT(date, @Keijobi)
              ,[LastPaymentPlanDate] = CONVERT(date, @Keijobi) 
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ; 
        
        --テーブル転送仕様Ｇ(訂正）Delete 受注明細    D_JuchuuDetails
                            --& Insert
	    IF OBJECT_ID( N'[dbo]..[#Table_D_JuchuuDetails_T]', N'U' ) IS NOT NULL
	    BEGIN
	        DROP TABLE [#Table_D_JuchuuDetails_T];
	    END
      
        SELECT * 
        INTO #Table_D_JuchuuDetails_T
        FROM [D_JuchuuDetails]
        WHERE JuchuuNO = @JuchuuNO
        ;
        
        DELETE FROM [D_JuchuuDetails]
        WHERE JuchuuNO = @JuchuuNO
        ;
        
		INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[JanCD]
                   ,[SKUName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[SetKBN]
                   ,[SetRows]
                   ,[JuchuuSuu]
                   ,[JuchuuUnitPrice]
                   ,[TaniCD]
                   ,[JuchuuGaku]
                   ,[JuchuuHontaiGaku]
                   ,[JuchuuTax]
                   ,[JuchuuTaxRitsu]
                   ,[CostUnitPrice]
                   ,[CostGaku]
                   ,[ProfitGaku]
                   ,[SoukoCD]
                   ,[HikiateSu]
                   ,[DeliveryOrderSu]
                   ,[DeliverySu]
                   ,[DirectFLG]
                   ,[HikiateFLG]
                   ,[JuchuuOrderNO]
                   ,[VendorCD]
                   ,[LastOrderNO]
                   ,[LastOrderRows]
                   ,[LastOrderDateTime]
                   ,[DesiredDeliveryDate]
                   ,[AnswerFLG]
                   ,[ArrivePlanDate]
                   ,[ArrivePlanNO]
                   ,[ArriveDateTime]
                   ,[ArriveNO]
                   ,[ArribveNORows]
                   ,[DeliveryPlanNO]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[IndividualClientName]
                   ,[ShippingPlanDate]
                   ,[SalesDate]
                   ,[SalesNO]
                   ,[DepositeDetailNO]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @JuchuuNO
                   ,tbl.JuchuuRows
                   ,tbl.DisplayRows
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,tbl.JuchuuGaku
                   ,tbl.JuchuuHontaiGaku
                   ,tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,ISNULL(DM.CostUnitPrice,0)	--CostUnitPrice
                   ,ISNULL(DM.CostGaku,0)	--CostGaku
                   ,tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@Keijobi)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,tbl.JuchuuSuu --HikiateSu
                   ,tbl.JuchuuSuu --DeliveryOrderSu
                   ,tbl.JuchuuSuu --DeliverySu
                   ,DM.DirectFLG
                   ,ISNULL(DM.HikiateFLG,0)
                   ,DM.JuchuuOrderNO
                   ,DM.VendorCD
                   ,DM.LastOrderNO
                   ,ISNULL(DM.LastOrderRows,0)
                   ,DM.LastOrderDateTime
                   ,DM.DesiredDeliveryDate
                   ,ISNULL(DM.AnswerFLG,0)
                   ,DM.ArrivePlanDate
                   ,ISNULL(DM.ArrivePlanNO,0)	--intでいい？★
                   ,DM.ArriveDateTime
                   ,DM.ArriveNO
                   ,ISNULL(DM.ArribveNORows,0)
                   ,ISNULL(DM.DeliveryPlanNO,0)
                   ,DM.CommentOutStore
                   ,DM.CommentInStore
                   ,DM.IndividualClientName
                   ,DM.ShippingPlanDate      --2020.01.30 add
                   ,DM.SalesDate
                   ,DM.SalesNO
                   ,DM.DepositeDetailNO
                   ,DM.InsertOperator
                   ,DM.InsertDateTime
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table AS tbl
              LEFT OUTER JOIN #Table_D_JuchuuDetails_T AS DM
              ON tbl.JuchuuRows = DM.JuchuuRows
              WHERE tbl.UpdateFlg = 0
              ;
                            
        --テーブル転送仕様Ｆ履歴    Insert 受注履歴
        --テーブル転送仕様Ｇ履歴    Insert 受注明細履歴             
        --テーブル転送仕様Ｉ        Insert 売上推移・赤
        --テーブル転送仕様Ｊ        Insert 売上明細推移・赤
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,2  --ProcessKBN tinyint,
            ,1	--RecoredKBN
            ,-1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
        
        --テーブル転送仕様Ａ(訂正） Update 売上     D_Sales
        UPDATE D_Sales SET
               [SalesDate] = CONVERT(date, @Keijobi)
              ,[Age] = @Age	--2019.12.16 add
              ,[SalesHontaiGaku] = @SalesGaku - @SalesTax
              ,[SalesHontaiGaku0] = @SalesHontaiGaku0
              ,[SalesHontaiGaku8] = @SalesHontaiGaku8
              ,[SalesHontaiGaku10] = @SalesHontaiGaku10
              ,[SalesTax] =   @SalesTax
              ,[SalesTax8] =  @HanbaiTax8
              ,[SalesTax10] = @HanbaiTax10
              ,[SalesGaku] =  @SalesGaku
              ,[StaffCD] = @Operator
              ,[Discount] = @Discount
              ,[Discount8] = @Discount8
              ,[Discount10] = @Discount10
              ,[DiscountTax] = @DiscountTax
              ,[DiscountTax8] = @DiscountTax8
              ,[DiscountTax10] = @DiscountTax10
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
        
        --テーブル転送仕様Ｂ(訂正)  Delete 売上明細     D_SalesDetails      
        --          &   Insert      
        SELECT * 
        INTO #Table_D_SalesDetails
        FROM [D_SalesDetails]
        WHERE SalesNO = @SalesNO
        ;
        
        DELETE FROM [D_SalesDetails]
        WHERE SalesNO = @SalesNO
        ;

       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
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
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
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
           ,tbl.JuchuuRows
           ,@JuchuuNO
           ,tbl.JuchuuRows
           ,DM.ShippingNO
           ,tbl.AdminNO
           ,tbl.SKUCD
           ,tbl.JanCD
           ,(SELECT top 1 M.SKUName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS SKUName
            ,(SELECT top 1 M.ColorName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS ColorName
           ,(SELECT top 1 M.SizeName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS SizeName
           ,tbl.JuchuuSuu	--SalesSU
           ,tbl.JuchuuUnitPrice  --SalesUnitPrice
           ,tbl.TaniCD
           ,tbl.JuchuuHontaiGaku    --SalesHontaiGaku
           ,tbl.JuchuuTax
           ,tbl.JuchuuGaku	--SalesGaku
           ,tbl.JuchuuTaxRitsu   --SalesTaxRitsu
           ,tbl.ProperGaku	--2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku	--2019.12.12 add
           ,DM.CommentOutStore
           ,DM.CommentInStore
           ,DM.IndividualClientName
           ,ISNULL(DM.DeliveryNoteFLG,1)
           ,ISNULL(DM.BillingPrintFLG,1)
           ,ISNULL(DM.InsertOperator,@Operator)
           ,ISNULL(DM.InsertDateTime,@SYSDATETIME)
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM @Table tbl
       LEFT OUTER JOIN #Table_D_SalesDetails AS DM
       ON tbl.JuchuuRows = DM.SalesRows
       WHERE tbl.UpdateFlg = 0
       ; 	--行追加時の訂正について要確認
        
        --テーブル転送仕様Ｈ        Delete 店舗入金　
        DELETE FROM D_StorePayment
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;

        --テーブル転送仕様Ｈ         Insert店舗入金
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
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
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             VALUES
                   (@SalesNO
                   ,IDENT_CURRENT('D_SalesTran')
                   ,@StoreCD
                   ,3	--1:通常、2:返品、3;訂正、4:取消
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME)
                   ;
                   
        --テーブル転送仕様Ｉ        Insert 売上推移・黒
        --テーブル転送仕様Ｊ        Insert 売上明細推移・黒
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,2  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;

        --テーブル転送仕様Ｃ(訂正)  Insert  店舗入出金履歴　D_DepositHistory        売上のヘッダ情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,2	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@Keijobi)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(@SalesGaku - @SalesTax)	--SalesGaku, money,>
           ,@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>★
           ,@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
            ;
        
        --テーブル転送仕様Ｄ(訂正)  Insert  店舗入出金履歴　D_DepositHistory        売上の明細情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]    --2019.12.12 add
           ,[DiscountGaku]  --2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
           ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
           ,3   --CancelKBN 1:取消、2:返品、3:訂正
           ,0   --RecoredKBN 0:黒、1:赤
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,tbl.JuchuuRows   --Rows, tinyint,>★
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,tbl.ProperGaku  --2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM @Table tbl
          WHERE tbl.UpdateFlg = 0
       ;
        
        
        IF @PointAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ①(訂正)insert  店舗入出金履歴　D_DepositHistory        ポイントありの時
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        
        END
        
        --現金ありの時    
        IF @CashAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ閼'(訂正)insert  店舗入出金履歴　D_DepositHistory        現金ありの時
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;
        END
        
        
        --カードありの時  
        IF @CardAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ③(訂正)Insert  店舗入出金履歴　D_DepositHistory        カードありの時
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END

        
        --値引きありの時  
        IF @Discount <> 0
        BEGIN
        	--テーブル転送仕様Ｅ④(訂正)Insert  店舗入出金履歴　D_DepositHistory        値引きありの時
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;        
        END
        
        --テーブル転送仕様Ｅ⑤(訂正)Insert  店舗入出金履歴　D_DepositHistory        その他①、②ありの時
        IF @Denomination1Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        
        IF @Denomination2Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END
        
        --テーブル転送仕様Ｅ⑥(訂正)Insert  店舗入出金履歴　D_DepositHistory        掛ありの時
        IF @CreditAmount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@CreditAmount
                ;
        END
        
        IF @AdvanceAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ⑦(訂正)insert  店舗入出金履歴　D_DepositHistory        ポイントありの時
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;
        
        END
    END
    
    ELSE IF @OperateMode = 3 --取消---------------------------------------------------------------
    BEGIN
        SET @OperateModeNm = '取消';
        
        --テーブル転送仕様Ｆ(取消） Update  受注　D_Juchuu
        --【D_Juchuu】
        UPDATE D_Juchuu
           SET [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ;   
        
        --テーブル転送仕様Ｇ(取消） Update  受注明細D_JuchuuDetails
        --【D_JuchuuDetails】
        UPDATE D_JuchuuDetails
           SET [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ;   
        
        --テーブル転送仕様Ｆ履歴    Insert  受注履歴
        --テーブル転送仕様Ｇ履歴    Insert  受注明細履歴
        --テーブル転送仕様Ｉ        Insert      売上推移・赤
        --テーブル転送仕様Ｊ        Insert      売上明細推移・赤
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,3  --ProcessKBN tinyint,
            ,1	--RecoredKBN
            ,-1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
            
        --テーブル転送仕様Ａ(取消） Update  売上D_Sales 
        UPDATE D_Sales SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
                    
        --テーブル転送仕様Ｂ(取消)  Update  売上明細D_SalesDetails
        UPDATE D_SalesDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
                      
        --テーブル転送仕様Ｃ(取消)  Insert  店舗入出金履歴　D_DepositHistory売上のヘッダ情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 SELECT
            @StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,1	--CancelKBN
           ,1	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@Keijobi)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(DH.SalesGaku - DH.SalesTax)	--SalesGaku, money,>
           ,DH.SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>
           ,DH.SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_Sales AS DH
       WHERE DH.SalesNO = @SalesNO
			;
         
        --テーブル転送仕様Ｄ(取消)  Insert  店舗入出金履歴　D_DepositHistory売上の明細情報
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
           ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
           ,1   --CancelKBN 1:取消、2:返品、3:訂正
           ,1   --RecoredKBN 0:黒、1:赤
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,DM.SalesRows   --Rows, tinyint,>★
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,DM.AdminNO	--AdminNO, int,>
           ,DM.SKUCD	--SKUCD, varchar(30),>
           ,DM.JanCD	--JanCD, varchar(13),>
           ,DM.SalesSU	--SalesSU, money,>
           ,DM.SalesUnitPrice	--SalesUnitPrice, money,>
           ,DM.SalesGaku - DM.SalesTax	--SalesGaku, money,>
           ,DM.SalesTax	--SalesTax, money,>
           ,DM.SalesTaxRitsu 	--SalesTaxRate, int,>
           ,DM.SalesGaku  	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,DM.ProperGaku	--2019.12.12 add
		   ,DM.ProperGaku - DM.SalesGaku AS DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_SalesDetails AS DM	--★SalesDetailsに変更必要かも
          INNER JOIN @Table tbl
          ON tbl.JuchuuRows = DM.SalesRows
          WHERE tbl.UpdateFlg = 0
          AND DM.SalesNO = @SalesNO
       ;

        SELECT @OldDiscount = DS.DiscountAmount
        --  ,DS.BillingAmount
            ,@OldAdvanceAmount = DS.AdvanceAmount
            ,@OldPointAmount = DS.PointAmount
            ,@OldDenominationCD = DS.CardDenominationCD
            ,@OldCardAmount = DS.CardAmount
            ,@OldCashAmount = DS.CashAmount
        --  ,DS.DepositAmount
            ,@OldRefundAmount = DS.RefundAmount
            ,@OldCreditAmount = DS.CreditAmount
            ,@OldDenominationCD1 = DS.Denomination1CD
            ,@OldDenomination1Amount = DS.Denomination1Amount
            ,@OldDenominationCD2 = DS.Denomination2CD
            ,@OldDenomination2Amount = DS.Denomination2Amount
        FROM D_StorePayment As DS
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
        
        IF @OldPointAmount > 0
        BEGIN
            --テーブル転送仕様Ｅ①(取消)insert  店舗入出金履歴　D_DepositHistoryポイントありの時
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint, 1:取消、2:返品、3:訂正
                ,1  --@RecoredKBN tinyint,0:黒、1:赤
                ,@OldPointAmount
                ;
        END 
         
        
        --現金ありの時    
        IF @OldCashAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ閼'(取消)insert  店舗入出金履歴　D_DepositHistory現金ありの時
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCashAmount
                ,@OldRefundAmount
                ;
		END
		                
        --カードありの時  
        IF @OldCardAmount > 0
        BEGIN
        	--テーブル転送仕様Ｅ③(取消)Insert  店舗入出金履歴　D_DepositHistoryカードありの時
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD
                ,@OldCardAmount
                ;
        END
        
        --値引きありの時  
        IF @OldDiscount <> 0
        BEGIN
        	--テーブル転送仕様Ｅ④(取消)Insert  店舗入出金履歴　D_DepositHistory値引きありの時
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDiscount
                ;
        END
        
        --テーブル転送仕様Ｅ⑤(取消)Insert  店舗入出金履歴　D_DepositHistoryその他①、②ありの時
        IF @OldDenomination1Amount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD1
                ,@OldDenomination1Amount
                ;
        END
        
        IF @OldDenomination2Amount > 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD2
                ,@OldDenomination2Amount
                ;
        END
        
        --テーブル転送仕様Ｅ⑥(取消)Insert  店舗入出金履歴　D_DepositHistory掛ありの時
        IF @OldCreditAmount > 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCreditAmount
                ;
        END

        IF @OldAdvanceAmount > 0
        BEGIN
            --テーブル転送仕様Ｅ⑦(取消)insert  店舗入出金履歴　D_DepositHistoryポイントありの時
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint, 1:取消、2:返品、3:訂正
                ,1  --@RecoredKBN tinyint,0:黒、1:赤
                ,@CustomerCD
                ,@OldAdvanceAmount
                ;
        END
        
        --テーブル転送仕様Ｈ        Delete 店舗入金　
        DELETE FROM D_StorePayment
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
    END
    
    --処理履歴データへ更新
    SET @KeyItem = @SalesNO;
        
    --Table転送仕様Ｚ InsertL_Log 
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutSalesNO = @SalesNO;
    
--<<OWARI>>
  return @W_ERR;

END



