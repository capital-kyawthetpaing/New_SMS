 BEGIN TRY 
 Drop Procedure dbo.[PRC_TempoJuchuuNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--CREATE TYPE T_Juchuu AS TABLE
--    (
--    [JuchuuRows] [int],
--    [DisplayRows] [int],
--    [SiteJuchuuRows] [int] ,
--    [NotPrintFLG] [tinyint] ,
--    [AddJuchuuRows] [int],
--    [NotOrderFLG] [tinyint] ,
--    [DirectFLG] [tinyint] ,
--    [SKUNO] [int] ,
--    [SKUCD] [varchar](30) ,
--    [JanCD] [varchar](13) ,
--    [SKUName] [varchar](80) ,
--    [ColorName] [varchar](20) ,
--    [SizeName] [varchar](20) ,
--    [SetKBN] [tinyint] ,
----  [SetRows] [tinyint] ,
--    [JuchuuSuu] [int] ,
--    [JuchuuUnitPrice] [money] ,
--    [TaniCD] [varchar](2) ,
--    [JuchuuGaku] [money] ,
--    [JuchuuHontaiGaku] [money] ,
--    [JuchuuTax] [money] ,
--    [JuchuuTaxRitsu] [int] ,
--    [CostUnitPrice] [money] ,
--    [CostGaku] [money] ,
--    [ProfitGaku] [money] ,
--    [SoukoCD] [varchar](6) ,
--    [VendorCD] [varchar](10) ,
--    [ArrivePlanDate] [date] ,
--    [CommentOutStore] [varchar](80) ,
--    [CommentInStore] [varchar](80) ,
--    [IndividualClientName] [varchar](80) ,
--    [ZaikoKBN] [tinyint] ,
--    [TemporaryNO] [varchar](11) ,	--仮引当番号
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_TempoJuchuuNyuuryoku]
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
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
                   ,NULL    --DeliveryPlanNO
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


