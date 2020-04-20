 BEGIN TRY 
 Drop Procedure dbo.[PRC_TempoJuchuuNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE PRC_TempoJuchuuNyuuryoku
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO   varchar(11),
    @StoreCD   varchar(4),
    @JuchuuDate  varchar(10),
    @ReturnFLG tinyint ,
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    @CustomerCD   varchar(13),
    @CustomerName   varchar(80),
    @CustomerName2   varchar(20),
    @AliasKBN   tinyint,
    @ZipCD1   varchar(3),
    @ZipCD2   varchar(4),
    @Address1   varchar(100),
    @Address2   varchar(100),
    @Tel11   varchar(5),
    @Tel12   varchar(4),
    @Tel13   varchar(4),
    @Tel21   varchar(5),
    @Tel22   varchar(4),
    @Tel23   varchar(4),
    
    @DeliveryCD   varchar(13),
    @DeliveryName   varchar(80),
    @DeliveryName2   varchar(20),
    @DeliveryAliasKBN   tinyint,
    @DeliveryZipCD1   varchar(3),
    @DeliveryZipCD2   varchar(4),
    @DeliveryAddress1   varchar(100),
    @DeliveryAddress2   varchar(100),
    @DeliveryTel11   varchar(5),
    @DeliveryTel12   varchar(4),
    @DeliveryTel13   varchar(4),
--    @DeliveryTel21   varchar(5),
--    @DeliveryTel22   varchar(4),
--    @DeliveryTel23   varchar(4),
   
    @JuchuuGaku money ,
    @Discount money ,
    @HanbaiHontaiGaku money ,
    @HanbaiTax8 money ,
    @HanbaiTax10 money ,
    @HanbaiGaku money ,
    @CostGaku money ,
    @ProfitGaku money ,
    @Point money ,
    @InvoiceGaku money ,
    @PaymentMethodCD varchar(3) ,
    @PaymentPlanNO int ,
    @SalesPlanDate date ,
    @FirstPaypentPlanDate date ,
    @LastPaymentPlanDate date ,
    @CommentOutStore varchar(80) ,
    @CommentInStore varchar(80) ,
    @MitsumoriNO varchar(11), 
    @NouhinsyoComment varchar(700),

    @Table  T_Juchuu READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutJuchuuNo varchar(11) OUTPUT
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
    DECLARE @ReserveNO varchar(11);
    DECLARE @Tennic tinyint;
    DECLARE @DeliveryPlanNO  varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    SET @Tennic = (SeLECT M.Tennic FROM M_Control AS M WHERE M.MainKey = 1);
    
    --新規・変更Mode時、
    IF @OperateMode <= 2
    BEGIN
        IF @JuchuuNO <>''
        BEGIN
            --最初に引当情報をクリアし、在庫に戻す 
            --【D_Stock】 
            UPDATE [D_Stock]
               SET [AllowableSu] = [D_Stock].[AllowableSu] + A.ReserveSu
                  ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] + A.ReserveSu
                  ,[ReserveSu] = [D_Stock].[ShippingSu] - A.ReserveSu
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             FROM D_Reserve AS A 
             WHERE A.StockNO = [D_Stock].StockNO
             AND A.DeleteDateTime IS NULL
             AND A.ReserveKBN = 1
             AND A.[Number] = @JuchuuNO
             ;
             
            --ログのSEQ獲得のために更新
            UPDATE D_Juchuu SET
                [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
             --20200225　テーブル転送仕様Ｊ②
            UPDATE D_JuchuuDetails SET
                HikiateSu = D_JuchuuDetails.HikiateSu - A.ReserveSu
                ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                            WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                            WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu = 0 THEN 3
                            ELSE 0 END )
            FROM D_Reserve A
            WHERE A.ReserveKBN = 1
            AND A.[Number] = D_JuchuuDetails.JuchuuNO
            AND A.NumberRows = D_JuchuuDetails.JuchuuRows
            AND D_JuchuuDetails.JuchuuNO = @JuchuuNO
            ;
             
             
             --テーブル転送仕様Ｈ・（削除処理）
             --【D_Reserve】
             DELETE FROM D_Reserve
             WHERE ReserveKBN = 1
             AND [Number] = @JuchuuNO
             ;
        END
        
        --引当可能ロジック
        DECLARE @return_value int,
                @Result tinyint,
                @Error tinyint,
                @LastDay varchar(10),
                @OutKariHikiateNo varchar(11),
                @AdminNO int,
                @DSoukoCD varchar(6),
                @Suryo int,
                @JuchuuRows int,
                @ZaikoKBN tinyint,
                @KariHikiateNo varchar(11);
        
        --カーソル定義
        DECLARE CUR_AAA CURSOR FOR
            SELECT tbl.SKUNO,tbl.SoukoCD,tbl.JuchuuSuu,tbl.JuchuuRows
            	--,tbl.ZaikoKBN
            	,tbl.DirectFlg
            	,tbl.TemporaryNO
            FROM @Table tbl
            ORDER BY tbl.JuchuuRows 
            
        DECLARE @TAB TABLE(
             GNO int NOT NULL
            ,HNO varchar(11)
        );
        
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO  @AdminNO,@DSoukoCD,@Suryo,@JuchuuRows,@ZaikoKBN,@KariHikiateNo;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            --IF @ZaikoKBN = 1
            IF @ZaikoKBN = 0	--中身はDirectFlg
            BEGIN
                EXEC Fnc_Reserve_SP
                    @AdminNO,
                    @JuchuuDate,
                    @StoreCD,
                    @DSoukoCD,
                    @Suryo,
                    1,  --@DenType
                    @JuchuuNO,  --新規時の受注番号を採番するべき？
                    @JuchuuRows,
                    @KariHikiateNo,
                    @Result OUTPUT,
                    @Error OUTPUT,
                    @LastDay OUTPUT,
                    @OutKariHikiateNo OUTPUT
                    ;
            END
            ELSE
            BEGIN
	        	SET @OutKariHikiateNo = null;
            END
            
            INSERT INTO @TAB VALUES(@JuchuuRows, @OutKariHikiateNo);
            
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @AdminNO,@DSoukoCD,@Suryo,@JuchuuRows,@ZaikoKBN,@KariHikiateNo;

        END
        
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
    END
    
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --伝票番号採番
        EXEC Fnc_GetNumber
            1,             --in伝票種別 1
            @JuchuuDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @JuchuuNO OUTPUT
            ;
        
        IF ISNULL(@JuchuuNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
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
           ,[DeliveryCD]
           ,[DeliveryName]
           ,[DeliveryName2]
           ,[DeliveryAliasKBN]
           ,[DeliveryZipCD1]
           ,[DeliveryZipCD2]
           ,[DeliveryAddress1]
           ,[DeliveryAddress2]
           ,[DeliveryTel11]
           ,[DeliveryTel12]
           ,[DeliveryTel13]
           ,[DeliveryTel21]
           ,[DeliveryTel22]
           ,[DeliveryTel23]
           ,[DeliveryKanaName]
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
           ,convert(date,@JuchuuDate)                    
           ,NULL    --JuchuuTime
           ,@ReturnFLG
           ,@SoukoCD
           ,3   --JuchuuKBN
           ,0   --SiteKBN
           ,NULL    --SiteJuchuuDateTime
           ,NULL    --SiteJuchuuNO
           ,0   --InportErrFLG
           ,0   --OnHoldFLG
           ,0   --IdentificationFLG
           ,NULL    --TorikomiDateTime
           ,@StaffCD
           ,@CustomerCD
           ,@CustomerName
           ,@CustomerName2
           ,@AliasKBN
           ,@ZipCD1
           ,@ZipCD2
           ,@Address1
           ,@Address2
           ,@Tel11
           ,@Tel12
           ,@Tel13
           ,NULL    --Tel21
           ,NULL    --Tel22
           ,NULL    --Tel23
           ,NULL    --CustomerKanaName
           ,@DeliveryCD
           ,@DeliveryName
           ,@DeliveryName2
           ,@DeliveryAliasKBN
           ,@DeliveryZipCD1
           ,@DeliveryZipCD2
           ,@DeliveryAddress1
           ,@DeliveryAddress2
           ,@DeliveryTel11
           ,@DeliveryTel12
           ,@DeliveryTel13
           ,NULL    --Tel21
           ,NULL    --Tel22
           ,NULL    --Tel23
           ,NULL   --,[DeliveryKanaName]
           ,NULL    --JuchuuCarrierCD
           ,0   --DecidedCarrierFLG
           ,NULL    --LastCarrierCD
           ,NULL    --NameSortingDateTime
           ,NULL    --NameSortingStaffCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
           ,@JuchuuGaku
           ,@Discount
           ,@HanbaiHontaiGaku
           ,@HanbaiTax8
           ,@HanbaiTax10
           ,@HanbaiGaku
           ,@CostGaku
           ,@ProfitGaku
           ,0   --Coupon
           ,@Point
           ,0   --PayCharge
           ,0   --Adjustments
           ,0   --Postage
           ,0   --GiftWrapCharge
           ,@InvoiceGaku
           ,@PaymentMethodCD
           ,@PaymentPlanNO
           ,0   --CardProgressKBN
           ,NULL    --CardCompany
           ,NULL    --CardNumber
           ,0   --PaymentProgressKBN
           ,0   --PresentFLG
           ,@SalesPlanDate
           ,@FirstPaypentPlanDate
           ,@LastPaymentPlanDate
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
           ,@CommentOutStore
           ,@CommentInStore
           ,NULL    --LastDepositeDate
           ,NULL    --LastOrderDate
           ,NULL    --LastArriveDate
           ,NULL    --LastSalesDate
           ,@MitsumoriNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL                  
           ,NULL
           );               

        --【D_StoreJuchuu】
        INSERT INTO [D_StoreJuchuu]
               ([JuchuuNO]
               ,[NouhinsyoComment])
         VALUES
               (@JuchuuNO
               ,@NouhinsyoComment
               )
               ;
               
        --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
        --M_Control.TennicFLG＝1
        IF @Tennic = 1
        BEGIN
            --【D_DeliveryPlan】配送予定情報　Table転送仕様Ｋ
            --伝票番号採番
            EXEC Fnc_GetNumber
                19,             --in伝票種別 19
                @JuchuuDate,      --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @DeliveryPlanNO OUTPUT
                ;
            
            IF ISNULL(@DeliveryPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END

            INSERT INTO [D_DeliveryPlan]
                   ([DeliveryPlanNO]
                   ,[DeliveryKBN]
                   ,[Number]
                   ,[DeliveryName]
                   ,[DeliverySoukoCD]
                   ,[DeliveryZip1CD]
                   ,[DeliveryZip2CD]
                   ,[DeliveryAddress1]
                   ,[DeliveryAddress2]
                   ,[DeliveryMailAddress]
                   ,[DeliveryTelphoneNO]
                   ,[DeliveryFaxNO]
                   ,[DecidedDeliveryDate]
                   ,[DecidedDeliveryTime]
                   ,[CarrierCD]
                   ,[PaymentMethodCD]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[InvoiceNO]
                   ,[DeliveryPlanDate]
                   ,[HikiateFLG]
                   ,[IncludeFLG]
                   ,[OntheDayFLG]
                   ,[ExpressFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
            SELECT
                    @DeliveryPlanNO
                   ,1 AS DeliveryKBN	--(1:販売、2:倉庫移動)
                   ,@JuchuuNO	AS Number
                   ,@DeliveryName
                   ,NULL	--[DeliverySoukoCD]
                   ,@DeliveryZipCD1
                   ,@DeliveryZipCD2
                   ,@DeliveryAddress1
                   ,@DeliveryAddress2
                   ,NULL	--[DeliveryMailAddress]
                   ,@DeliveryTel11 + '-' + @DeliveryTel12 + '-' + @DeliveryTel13	--[DeliveryTelphoneNO]
                   ,NULL	--[DeliveryFaxNO]
                   ,NULL	--[DecidedDeliveryDate]
                   ,NULL	--[DecidedDeliveryTime]
                   ,NULL	--[CarrierCD]
                   ,@PaymentMethodCD	--[PaymentMethodCD]
                   ,NULL	--[CommentInStore]
                   ,NULL	--[CommentOutStore]
                   ,NULL	--[InvoiceNO]
                   ,NULL	--[DeliveryPlanDate]
                   ,0	--[HikiateFLG]
                   ,0	--[IncludeFLG]
                   ,0	--[OntheDayFLG]
                   ,0	--[ExpressFLG]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
              ;            
        END
    END
        
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
		
        UPDATE D_Juchuu
           SET [StoreCD] = @StoreCD                         
              ,[JuchuuDate] = @JuchuuDate
              ,[ReturnFLG] =  @ReturnFLG
              ,[SoukoCD] =    @SoukoCD
              ,[StaffCD] = @StaffCD                         
              ,[CustomerCD] = @CustomerCD                   
              ,[CustomerName] = @CustomerName               
              ,[CustomerName2] = @CustomerName2             
              ,[AliasKBN] = @AliasKBN
              ,[ZipCD1] = @ZipCD1                           
              ,[ZipCD2] = @ZipCD2                           
              ,[Address1] = @Address1                       
              ,[Address2] = @Address2                       
              ,[Tel11] = @Tel11                             
              ,[Tel12] = @Tel12                             
              ,[Tel13] = @Tel13                             
              ,[Tel21] = @Tel21                             
              ,[Tel22] = @Tel22                             
              ,[Tel23] = @Tel23              
              ,[DeliveryCD]       = @DeliveryCD
              ,[DeliveryName]     = @DeliveryName
              ,[DeliveryName2]    = @DeliveryName2
              ,[DeliveryAliasKBN] = @DeliveryAliasKBN
              ,[DeliveryZipCD1]   = @DeliveryZipCD1
              ,[DeliveryZipCD2]   = @DeliveryZipCD2
              ,[DeliveryAddress1] = @DeliveryAddress1
              ,[DeliveryAddress2] = @DeliveryAddress2
              ,[DeliveryTel11]    = @DeliveryTel11
              ,[DeliveryTel12]    = @DeliveryTel12
              ,[DeliveryTel13]    = @DeliveryTel13
              ,[JuchuuGaku] =       @JuchuuGaku
              ,[Discount] =         @Discount
              ,[HanbaiHontaiGaku] = @HanbaiHontaiGaku
              ,[HanbaiTax8] =       @HanbaiTax8
              ,[HanbaiTax10] =      @HanbaiTax10
              ,[HanbaiGaku] =       @HanbaiGaku
              ,[CostGaku] =         @CostGaku
              ,[ProfitGaku] =       @ProfitGaku                          
              ,[Point] =               @Point       
              ,[InvoiceGaku] =         @InvoiceGaku
              ,[PaymentMethodCD] =       @PaymentMethodCD
              ,[SalesPlanDate] =         @SalesPlanDate
              ,[FirstPaypentPlanDate] =  @FirstPaypentPlanDate
              ,[LastPaymentPlanDate] =   @LastPaymentPlanDate
              ,[CommentOutStore] =       @CommentOutStore
              ,[CommentInStore] =        @CommentInStore       
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ;

        --【D_StoreJuchuu】
        UPDATE  [D_StoreJuchuu]
           SET  [NouhinsyoComment] = @NouhinsyoComment
        WHERE [JuchuuNO] = @JuchuuNO
        ;
        
        UPDATE [D_DeliveryPlan] SET
               [DeliveryName] = @DeliveryName
              ,[DeliveryZip1CD]   = @DeliveryZipCD1
              ,[DeliveryZip2CD]   = @DeliveryZipCD2
              ,[DeliveryAddress1] = @DeliveryAddress1
              ,[DeliveryAddress2] = @DeliveryAddress2
              ,[DeliveryTelphoneNO] = @DeliveryTel11 + '-' + @DeliveryTel12 + '-' + @DeliveryTel13	--[]
              ,[PaymentMethodCD]    = @PaymentMethodCD
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE @JuchuuNO = D_DeliveryPlan.Number
        ;
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        UPDATE [D_Juchuu]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [JuchuuNO] = @JuchuuNO
         ;

        --【D_StoreJuchuu】
        
        --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
        --M_Control.TennicFLG＝1
        IF @Tennic = 1
        BEGIN
            DELETE FROM [D_DeliveryPlan]
            WHERE [Number] = @JuchuuNO;
        END
    END
    
    --【D_JuchuuDetails】
    IF @OperateMode <= 2    --新規・修正時
    BEGIN
        --Table転送仕様Ｂ
        INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
                   ,[NotOrderFLG]
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
                   ,tbl.NotPrintFLG
                   ,tbl.AddJuchuuRows
                   ,tbl.NotOrderFLG
                   ,tbl.SKUNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,tbl.SKUName
                   ,tbl.ColorName
                   ,tbl.SizeName
                   ,tbl.SetKBN
                   ,0 AS SetRows
                   ,tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,tbl.JuchuuGaku
                   ,tbl.JuchuuHontaiGaku
                   ,tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,tbl.CostUnitPrice
                   ,tbl.CostGaku
                   ,tbl.ProfitGaku
                   ,tbl.SoukoCD
                   ,0 --HikiateSu
                   ,0 --DeliveryOrderSu
                   ,0 --DeliverySu
                   ,tbl.DirectFLG
                   ,0 --HikiateFLG
                   ,NULL --JuchuuOrderNO
                   ,tbl.VendorCD
                   ,NULL    --LastOrderNO
                   ,0   --LastOrderRows
                   ,NULL    --LastOrderDateTime
                   ,NULL    --DesiredDeliveryDate
                   ,0   --AnswerFLG
                   ,tbl.ArrivePlanDate
                   ,NULL    --ArrivePlanNO
                   ,NULL    --ArriveDateTime
                   ,NULL    --ArriveNO
                   ,0   --ArribveNORows
                   ,@DeliveryPlanNO
                   ,tbl.CommentOutStore
                   ,tbl.CommentInStore
                   ,tbl.IndividualClientName
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

        --ログのSEQ獲得のために更新
        UPDATE D_Juchuu SET
            [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
         AND NOT EXISTS (SELECT 1 FROM @Table tbl 
                        INNER JOIN D_JuchuuDetails AS DM 
                        ON tbl.JuchuuRows = DM.JuchuuRows
                        AND DM.JuchuuNO = @JuchuuNO
        );

        --行削除されたデータはDELETE処理
        UPDATE D_JuchuuDetails
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [JuchuuNO] = @JuchuuNO
         AND NOT EXISTS (SELECT 1 FROM @Table tbl WHERE tbl.JuchuuRows = D_JuchuuDetails.[JuchuuRows]
         )
         ;

        --ログのSEQ獲得のために更新
        UPDATE D_Juchuu SET
            [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        AND EXISTS(SELECT 1 FROM @Table tbl WHERE tbl.UpdateFlg = 1)
        ;
        
        UPDATE [D_JuchuuDetails]
           SET [DisplayRows] = tbl.DisplayRows                  
               ,[NotPrintFLG] = tbl.NotPrintFLG
               ,[AddJuchuuRows] = tbl.AddJuchuuRows
               ,[NotOrderFLG] = tbl.NotOrderFLG
               ,[AdminNO] = tbl.SKUNO                              
               ,[SKUCD] = tbl.SKUCD                              
               ,[JanCD] = tbl.JanCD                              
               ,[SKUName] = tbl.SKUName                          
               ,[ColorName] = tbl.ColorName                      
               ,[SizeName] = tbl.SizeName                        
               ,[SetKBN] = tbl.SetKBN                            
               ,[JuchuuSuu] = tbl.JuchuuSuu                      
               ,[JuchuuUnitPrice] = tbl.JuchuuUnitPrice          
               ,[TaniCD] = tbl.TaniCD                            
               ,[JuchuuGaku] = tbl.JuchuuGaku                    
               ,[JuchuuHontaiGaku] = tbl.JuchuuHontaiGaku        
               ,[JuchuuTax] = tbl.JuchuuTax                      
               ,[JuchuuTaxRitsu] = tbl.JuchuuTaxRitsu            
               ,[CostUnitPrice] = tbl.CostUnitPrice              
               ,[CostGaku] = tbl.CostGaku                        
               ,[ProfitGaku] = tbl.ProfitGaku                    
               ,[SoukoCD] = tbl.SoukoCD    
               ,[DirectFLG] = tbl.DirectFLG                        
               ,[VendorCD] = tbl.VendorCD                         
               ,[ArrivePlanDate] = tbl.ArrivePlanDate            
               ,[CommentOutStore] = tbl.CommentOutStore          
               ,[CommentInStore] = tbl.CommentInStore            
               ,[IndividualClientName] = tbl.IndividualClientName
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM D_JuchuuDetails
        INNER JOIN @Table tbl
         ON @JuchuuNO = D_JuchuuDetails.JuchuuNO
         AND tbl.JuchuuRows = D_JuchuuDetails.JuchuuRows
         AND tbl.UpdateFlg = 1
         ;
    
        --【D_Mitsumori】
        UPDATE D_Mitsumori
        SET JuchuuFLG = 1
           ,[UpdateOperator] =  @Operator  
           ,[UpdateDateTime] =  @SYSDATETIME
        WHERE MitsumoriNO = @MitsumoriNO
        ;
                
        --【D_Reserve】INSERT処理　受注行数分のレコードを作成(引当在庫レコードの件数分)
        --カーソル定義
        DECLARE CUR_TAB CURSOR FOR
            SELECT T.HNO, T.GNO, A.KeySEQ
            FROM @TAB AS T
            INNER JOIN D_TemporaryReserve AS A
            ON A.TemporaryNO = T.HNO
            ORDER BY T.GNO, A.KeySEQ
            ;
        
        DECLARE @GNO int
            ,@HNO varchar(11)
            ,@KeySEQ int;
            
        --カーソルオープン
        OPEN CUR_TAB;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TAB
        INTO  @HNO, @GNO, @KeySEQ;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
        
            --伝票番号採番
            EXEC Fnc_GetNumber
                12,             --in伝票種別 12
                @JuchuuDate,    --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @ReserveNO OUTPUT
                ;
            
            IF ISNULL(@ReserveNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
	        
	        --Table転送仕様Ｈ
	        --【D_Reserve】
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
            SELECT @ReserveNO
                   ,A.ReserveKBN
                   ,@JuchuuNO
                   ,tbl.JuchuuRows
                   ,A.StockNO
                   ,tbl.SoukoCD
                   ,tbl.JanCD
                   ,tbl.SKUCD
                   ,tbl.SKUNO
                   ,A.ReserveSu
                   ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                           THEN NULL
                           ELSE CONVERT(date,@SYSDATETIME) END)       --[ShippingPossibleDate]
                   ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                           THEN 0
                           ELSE A.ReserveSu END)       --[ShippingPossibleSU]
                   ,NULL    --[ShippingOrderNO]
                   ,0       --[ShippingOrderRows]
                   ,NULL    --[CompletedPickingNO]
                   ,0       --[CompletedPickingRow]
                   ,NULL    --[CompletedPickingDate]
                   ,0   --[ShippingSu]
                   ,0   --[ReturnKBN]
                   ,0   --[OriginalReserveNO]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL    --[DeleteOperator]
                   ,NULL    --[DeleteDateTime]
            
            FROM D_TemporaryReserve A
            INNER JOIN  @Table tbl ON tbl.JuchuuRows = @GNO
            LEFT OUTER JOIN D_Stock B ON B.StockNO = A.StockNO
            WHERE  A.TemporaryNO = @HNO
            AND A.KeySEQ = @KeySEQ
            ;

            --【D_Stock】
            UPDATE [D_Stock]
               SET [AllowableSu] = [D_Stock].[AllowableSu] - A.UpdateSu
                  ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] - A.UpdateSu
                  ,[ReserveSu] = [D_Stock].[ReserveSu] + A.UpdateSu
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             FROM D_TemporaryReserve AS A 
           --     INNER JOIN  @Table tbl ON A.TemporaryNO = (SELECT HNO FROM @TAB WHERE GNO = tbl.JuchuuRows)
             WHERE A.StockNO = [D_Stock].StockNO
             AND A.TemporaryNO = @HNO
             AND A.KeySEQ = @KeySEQ
             ;
                         
            --ログのSEQ獲得のために更新
            UPDATE D_Juchuu SET
                [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --20200225　テーブル転送仕様Ｊ
            UPDATE D_JuchuuDetails SET
                HikiateSu = D_JuchuuDetails.HikiateSu + A.ReserveSu
                ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                            WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                            WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu = 0 THEN 3
                            ELSE 0 END )
            FROM D_TemporaryReserve A
            INNER JOIN @Table tbl ON tbl.JuchuuRows = @GNO
            WHERE A.TemporaryNO = @HNO
            AND A.KeySEQ = @KeySEQ
            AND D_JuchuuDetails.JuchuuNO = @JuchuuNO
            AND D_JuchuuDetails.JuchuuRows = tbl.JuchuuRows
            ;

            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TAB
            INTO  @HNO, @GNO, @KeySEQ;

        END
        
        --カーソルを閉じる
        CLOSE CUR_TAB;
        DEALLOCATE CUR_TAB;

        --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
        --M_Control.TennicFLG＝1
        IF @Tennic = 1
        BEGIN
            --【D_DeliveryPlanDetails】配送予定明細　Table転送仕様Ｌ
            INSERT INTO [D_DeliveryPlanDetails]
               ([DeliveryPlanNO]
               ,[DeliveryPlanRows]
               ,[Number]
               ,[NumberRows]
               ,[CommentInStore]
               ,[CommentOutStore]
               ,[HikiateFLG]
               ,[UpdateCancelKBN]
               ,[DeliveryOrderComIn]
               ,[DeliveryOrderComOut]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
             SELECT  
                @DeliveryPlanNO
               ,tbl.JuchuuRows AS DeliveryPlanRows
               ,@JuchuuNO AS Number
               ,tbl.JuchuuRows  As NumberRows
               ,tbl.CommentInStore
               ,tbl.CommentOutStore
               ,(CASE DM.DirectFLG WHEN 1 THEN 1 ELSE DM.HikiateFLG END)
               ,0	--UpdateCancelKBN]
               ,NULL	--DeliveryOrderComIn]
               ,NULL	--DeliveryOrderComOut]                        
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

              FROM @Table tbl
              INNER JOIN D_JuchuuDetails AS DM
              ON DM.JuchuuRows = tbl.JuchuuRows
              AND DM.JuchuuNO = @JuchuuNO
              WHERE tbl.UpdateFlg = 0
              ;
              
            --行削除されたデータはDELETE処理
            DELETE FROM D_DeliveryPlanDetails
             WHERE [Number] = @JuchuuNO
             AND NOT EXISTS (SELECT 1 FROM @Table tbl WHERE tbl.JuchuuRows = D_DeliveryPlanDetails.[NumberRows]
             )
             ;
	         
            UPDATE [D_DeliveryPlanDetails] SET
                   [CommentInStore]  = tbl.CommentInStore
                  ,[CommentOutStore] = tbl.CommentOutStore
                  ,[HikiateFLG]      = (CASE DM.DirectFLG WHEN 1 THEN 1 ELSE DM.HikiateFLG END)
                  ,[UpdateOperator]  =  @Operator  
                  ,[UpdateDateTime]  =  @SYSDATETIME
                  
             FROM @Table AS tbl
              INNER JOIN D_JuchuuDetails AS DM
              ON DM.JuchuuRows = tbl.JuchuuRows
              AND DM.JuchuuNO = @JuchuuNO
             WHERE @JuchuuNO = D_DeliveryPlanDetails.Number
             AND tbl.JuchuuRows = D_DeliveryPlanDetails.NumberRows
             AND tbl.UpdateFlg = 1
            ;
            
            --出荷予定　D_DeliveryPlan Table転送仕様Ｋ
            UPDATE [D_DeliveryPlan] SET
                HikiateFLG = 1
            WHERE @JuchuuNO = D_DeliveryPlan.Number
            AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS DD
                WHERE DD.DeliveryPlanNO = D_DeliveryPlan.DeliveryPlanNO
                AND DD.HikiateFLG = 0)
            ;
            
        END
        
    END
    ELSE    --削除
    BEGIN
        --【D_Mitsumori】
        UPDATE D_Mitsumori
        SET JuchuuFLG = 0	--受注成約FLG＝０にUPDATE
           ,[UpdateOperator] =  @Operator  
           ,[UpdateDateTime] =  @SYSDATETIME
        WHERE MitsumoriNO = @MitsumoriNO
        ;
        
        --【D_Stock】
        UPDATE [D_Stock]
           SET [AllowableSu] = [D_Stock].[AllowableSu] + A.ReserveSu
              ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] + A.ReserveSu
              ,[ReserveSu] = [D_Stock].[ReserveSu] - A.ReserveSu
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
         FROM D_Reserve AS A 
            
         WHERE A.StockNO = [D_Stock].StockNO
         AND A.Number = @JuchuuNO
         AND A.ReserveKBN = 1
         AND A.DeleteDateTime IS NULL
         AND [D_Stock].DeleteDateTime IS NULL
         ;
        
         --20200225　テーブル転送仕様Ｊ②
        UPDATE D_JuchuuDetails SET
            HikiateSu = D_JuchuuDetails.HikiateSu - A.ReserveSu
            ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                        WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                        WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu = 0 THEN 3
                        ELSE 0 END )
        FROM D_Reserve A
        WHERE A.ReserveKBN = 1
        AND A.[Number] = D_JuchuuDetails.JuchuuNO
        AND A.NumberRows = D_JuchuuDetails.JuchuuRows
        AND D_JuchuuDetails.JuchuuNO = @JuchuuNO
        ;
        
        --【D_Reserve】
        DELETE FROM D_Reserve
        WHERE ReserveKBN = 1
        AND [Number] = @JuchuuNO
        ;

        --ログのSEQ獲得のために更新
        UPDATE D_Juchuu SET
            [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        ;
        
    	--削除
        UPDATE [D_JuchuuDetails]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [JuchuuNO] = @JuchuuNO
         ;
        --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
        --M_Control.TennicFLG＝1
        IF @Tennic = 1
        BEGIN
            DELETE FROM [D_DeliveryPlanDetails]
            WHERE [Number] = @JuchuuNO;
        END
    END
    
    --処理履歴データへ更新
    SET @KeyItem = @JuchuuNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TempoJuchuuNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutJuchuuNo = @JuchuuNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
