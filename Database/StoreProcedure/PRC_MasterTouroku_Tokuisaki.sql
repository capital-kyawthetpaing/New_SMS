 BEGIN TRY 
 Drop Procedure dbo.[PRC_MasterTouroku_Tokuisaki]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    得意先マスタ
--       Program ID      MasterTouroku_Tokuisaki
--       Create date:    2020.3.11
--    ======================================================================
create PROCEDURE [dbo].[PRC_MasterTouroku_Tokuisaki]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）

    @CustomerCD varchar(13) ,
    @ChangeDate date ,
    @VariousFLG tinyint ,
    @CustomerName varchar(80) ,
    @LastName varchar(20) ,
    @FirstName varchar(20) ,
    @LongName1 varchar(50) ,
    @LongName2 varchar(50) ,
    @KanaName varchar(30) ,
    @StoreKBN tinyint ,
    @CustomerKBN tinyint ,
    @StoreTankaKBN tinyint ,
    @AliasKBN tinyint ,
    @BillingType tinyint ,
    @GroupName varchar(40) ,
    @BillingFLG tinyint ,
    @CollectFLG tinyint ,
    @BillingCD varchar(13) ,
    @CollectCD varchar(13) ,
    @Birthdate date ,
    @Sex tinyint ,
    @Tel11 varchar(5) ,
    @Tel12 varchar(4) ,
    @Tel13 varchar(4) ,
    @Tel21 varchar(5) ,
    @Tel22 varchar(4) ,
    @Tel23 varchar(4) ,
    @ZipCD1 varchar(3) ,
    @ZipCD2 varchar(4) ,
    @Address1 varchar(100) ,
    @Address2 varchar(100) ,
    @MailAddress varchar(100) ,
    @TankaCD varchar(13) ,
    @PointFLG tinyint ,
    @LastPoint money ,
    @WaitingPoint money ,
    @TotalPoint money ,
--    @TotalPurchase money ,
--    @UnpaidAmount money ,
--    @UnpaidCount money ,
--    @LastSalesDate date ,
--    @LastSalesStoreCD varchar(4) ,
    @MainStoreCD varchar(4) ,
    @StaffCD varchar(10) ,
    @AttentionFLG tinyint ,
    @ConfirmFLG tinyint ,
    @ConfirmComment varchar(50) ,
    @BillingCloseDate tinyint ,
    @CollectPlanMonth tinyint ,
    @CollectPlanDate tinyint ,
    @HolidayKBN tinyint ,
    @TaxTiming tinyint ,
    @TaxPrintKBN tinyint  ,
    @TaxFractionKBN tinyint ,
    @AmountFractionKBN tinyint ,
    @CreditLevel tinyint ,
    @CreditCard money ,
    @CreditInsurance money ,
    @CreditDeposit money ,
    @CreditETC money ,
    @CreditAmount money ,
    @CreditWarningAmount money ,
    @CreditAdditionAmount    money ,
    @PaymentMethodCD varchar(3) ,
    @KouzaCD varchar(3) ,
    @DisplayOrder int ,
    @PaymentUnit tinyint ,
    @NoInvoiceFlg tinyint ,
    @CountryKBN tinyint ,
    @CountryName varchar(30) ,
    @RegisteredNumber varchar(15) ,
    @DMFlg tinyint ,
    @RemarksOutStore varchar(500) ,
    @RemarksInStore varchar(500) ,
    @AnalyzeCD1  varchar(10),
    @AnalyzeCD2  varchar(10),
    @AnalyzeCD3  varchar(10),
    @DeleteFlg  tinyint ,
    @UsedFlg  tinyint ,
    
    @Operator  varchar(10),
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
    SET @KeyItem = @CustomerCD + ' ' + CONVERT(varchar, @ChangeDate,111);
    
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --テーブル転送仕様Ａ
        INSERT INTO [M_Customer]
           ([CustomerCD]
           ,[ChangeDate]
           ,[VariousFLG]
           ,[CustomerName]
           ,[LastName]
           ,[FirstName]
           ,[LongName1]
           ,[LongName2]
           ,[KanaName]
           ,[StoreKBN]
           ,[CustomerKBN]
           ,[StoreTankaKBN]
           ,[AliasKBN]
           ,[BillingType]
           ,[GroupName]
           ,[BillingFLG]
           ,[CollectFLG]
           ,[BillingCD]
           ,[CollectCD]
           ,[Birthdate]
           ,[Sex]
           ,[Tel11]
           ,[Tel12]
           ,[Tel13]
           ,[Tel21]
           ,[Tel22]
           ,[Tel23]
           ,[ZipCD1]
           ,[ZipCD2]
           ,[Address1]
           ,[Address2]
           ,[MailAddress]
           ,[TankaCD]
           ,[PointFLG]
           ,[LastPoint]
           ,[WaitingPoint]
           ,[TotalPoint]
           ,[TotalPurchase]
           ,[UnpaidAmount]
           ,[UnpaidCount]
           ,[LastSalesDate]
           ,[LastSalesStoreCD]
           ,[MainStoreCD]
           ,[StaffCD]
           ,[AttentionFLG]
           ,[ConfirmFLG]
           ,[ConfirmComment]
           ,[BillingCloseDate]
           ,[CollectPlanMonth]
           ,[CollectPlanDate]
           ,[HolidayKBN]
           ,[TaxTiming]
           ,[TaxPrintKBN]
           ,[TaxFractionKBN]
           ,[AmountFractionKBN]
           ,[CreditLevel]
           ,[CreditCard]
           ,[CreditInsurance]
           ,[CreditDeposit]
           ,[CreditETC]
           ,[CreditAmount]  
           ,[CreditWarningAmount]
           ,[CreditAdditionAmount]
           ,[PaymentMethodCD]
           ,[KouzaCD]
           ,[DisplayOrder]
           ,[PaymentUnit]
           ,[NoInvoiceFlg]
           ,[CountryKBN]
           ,[CountryName]
           ,[RegisteredNumber]
           ,[DMFlg]
           ,[RemarksOutStore]
           ,[RemarksInStore]
           ,[AnalyzeCD1]
           ,[AnalyzeCD2]
           ,[AnalyzeCD3]
           ,[DeleteFlg]
           ,[UsedFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        VALUES
           (@CustomerCD
           ,@ChangeDate
           ,@VariousFLG
           ,@CustomerName
           ,@LastName
           ,@FirstName
           ,@LongName1
           ,@LongName2
           ,@KanaName
           ,@StoreKBN
           ,@CustomerKBN
           ,@StoreTankaKBN
           ,@AliasKBN
           ,@BillingType
           ,@GroupName
           ,@BillingFLG
           ,@CollectFLG
           ,@BillingCD
           ,@CollectCD
           ,@Birthdate
           ,@Sex
           ,@Tel11
           ,@Tel12
           ,@Tel13
           ,@Tel21
           ,@Tel22
           ,@Tel23
           ,@ZipCD1
           ,@ZipCD2
           ,@Address1
           ,@Address2
           ,@MailAddress
           ,@TankaCD
           ,@PointFLG
           ,@LastPoint
           ,@WaitingPoint
           ,@TotalPoint
           ,0	--TotalPurchase
           ,0	--UnpaidAmount
           ,0	--UnpaidCount
           ,NULL	--LastSalesDate
           ,NULL	--LastSalesStoreCD
           ,@MainStoreCD
           ,@StaffCD
           ,@AttentionFLG
           ,@ConfirmFLG
           ,@ConfirmComment
           ,@BillingCloseDate
           ,@CollectPlanMonth
           ,@CollectPlanDate
           ,@HolidayKBN
           ,@TaxTiming
           ,@TaxPrintKBN
           ,@TaxFractionKBN
           ,@AmountFractionKBN
           ,@CreditLevel
           ,@CreditCard
           ,@CreditInsurance
           ,@CreditDeposit
           ,@CreditETC
           ,@CreditAmount
           ,@CreditWarningAmount
           ,@CreditAdditionAmount
           ,@PaymentMethodCD
           ,@KouzaCD
           ,@DisplayOrder
           ,@PaymentUnit
           ,@NoInvoiceFlg
           ,@CountryKBN
           ,@CountryName
           ,@RegisteredNumber
           ,@DMFlg
           ,@RemarksOutStore
           ,@RemarksInStore
           ,@AnalyzeCD1
           ,@AnalyzeCD2
           ,@AnalyzeCD3
           ,@DeleteFlg
           ,@UsedFlg
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
        );

    END;
    
    ELSE IF @OperateMode = 2 --変更--
    BEGIN
        SET @OperateModeNm = '変更';
        
        --テーブル転送仕様Ａ
        UPDATE [M_Customer] SET
           [VariousFLG]        = @VariousFLG
           ,[CustomerName]      = @CustomerName
           ,[LastName]          = @LastName
           ,[FirstName]         = @FirstName
           ,[LongName1]         = @LongName1
           ,[LongName2]         = @LongName2
           ,[KanaName]          = @KanaName
           ,[StoreKBN]          = @StoreKBN
           ,[CustomerKBN]       = @CustomerKBN
           ,[StoreTankaKBN]     = @StoreTankaKBN
           ,[AliasKBN]          = @AliasKBN
           ,[BillingType]       = @BillingType
           ,[GroupName]         = @GroupName
           ,[BillingFLG]        = @BillingFLG
           ,[CollectFLG]        = @CollectFLG
           ,[BillingCD]         = @BillingCD
           ,[CollectCD]         = @CollectCD
           ,[Birthdate]         = @Birthdate
           ,[Sex]               = @Sex
           ,[Tel11]             = @Tel11
           ,[Tel12]             = @Tel12
           ,[Tel13]             = @Tel13
           ,[Tel21]             = @Tel21
           ,[Tel22]             = @Tel22
           ,[Tel23]             = @Tel23
           ,[ZipCD1]            = @ZipCD1
           ,[ZipCD2]            = @ZipCD2
           ,[Address1]          = @Address1
           ,[Address2]          = @Address2
           ,[MailAddress]       = @MailAddress
           ,[TankaCD]           = @TankaCD
           ,[PointFLG]          = @PointFLG
           ,[LastPoint]         = @LastPoint
           ,[WaitingPoint]      = @WaitingPoint
           ,[TotalPoint]        = @TotalPoint
           --,[TotalPurchase]     = @TotalPurchase
           --,[UnpaidAmount]      = @UnpaidAmount
           --,[UnpaidCount]       = @UnpaidCount
           --,[LastSalesDate]     = @LastSalesDate
           --,[LastSalesStoreCD]  = @LastSalesStoreCD
           ,[MainStoreCD]       = @MainStoreCD
           ,[StaffCD]           = @StaffCD
           ,[AttentionFLG]      = @AttentionFLG
           ,[ConfirmFLG]        = @ConfirmFLG
           ,[ConfirmComment]    = @ConfirmComment
           ,[BillingCloseDate]  = @BillingCloseDate
           ,[CollectPlanMonth]  = @CollectPlanMonth
           ,[CollectPlanDate]   = @CollectPlanDate
           ,[HolidayKBN]        = @HolidayKBN
           ,[TaxTiming]         = @TaxTiming
           ,[TaxPrintKBN]       = @TaxPrintKBN
           ,[TaxFractionKBN]    = @TaxFractionKBN
           ,[AmountFractionKBN] = @AmountFractionKBN
           ,[CreditLevel]       = @CreditLevel
           ,[CreditAmount]      = @CreditAmount
           ,[CreditCard]        = @CreditCard
           ,[CreditInsurance]   = @CreditInsurance
           ,[CreditDeposit]     = @CreditDeposit
           ,[CreditETC]         = @CreditETC
           ,[CreditWarningAmount] = @CreditWarningAmount
           ,[CreditAdditionAmount] = @CreditAdditionAmount
           ,[PaymentMethodCD]   = @PaymentMethodCD
           ,[KouzaCD]           = @KouzaCD
           ,[DisplayOrder]      = @DisplayOrder
           ,[PaymentUnit]       = @PaymentUnit
           ,[NoInvoiceFlg]      = @NoInvoiceFlg
           ,[CountryKBN]        = @CountryKBN
           ,[CountryName]       = @CountryName
           ,[RegisteredNumber]  = @RegisteredNumber
           ,[DMFlg]             = @DMFlg
           ,[RemarksOutStore]   = @RemarksOutStore
           ,[RemarksInStore]    = @RemarksInStore
           ,[AnalyzeCD1]        = @AnalyzeCD1
           ,[AnalyzeCD2]        = @AnalyzeCD2
           ,[AnalyzeCD3]        = @AnalyzeCD3
           ,[DeleteFlg]         = @DeleteFlg
           ,[UsedFlg]           = @UsedFlg
          ,[UpdateOperator]     = @Operator  
          ,[UpdateDateTime]     = @SYSDATETIME
        WHERE [CustomerCD] = @CustomerCD  
       AND [ChangeDate] = @ChangeDate
       ;
        
    END

    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';

        DELETE FROM [M_Customer]
         WHERE [CustomerCD] = @CustomerCD  
           AND [ChangeDate] = @ChangeDate
           ;

    	DELETE FROM [M_CustomerMail]
        WHERE CustomerCD = @CustomerCD
        AND StoreCD = @MainStoreCD
        AND ChangeDate = @ChangeDate
        AND SEQ = 1
        ; 
    END

	IF @OperateMode <> 3 --削除以外--
	BEGIN
        IF ISNULL(@MailAddress,'') <> ''
        BEGIN
            UPDATE [M_CustomerMail] SET
                [MailAddress1]    = @MailAddress
                ,[DeleteFlg]      = @DeleteFlg
                ,[UpdateOperator] = @Operator  
                ,[UpdateDateTime] = @SYSDATETIME
            WHERE CustomerCD = @CustomerCD
            AND StoreCD = @MainStoreCD
            AND ChangeDate = @ChangeDate
            AND SEQ = 1
            ;
            
         	IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO [M_CustomerMail]
                   ([CustomerCD]
                   ,[StoreCD]
                   ,[ChangeDate]
                   ,[SEQ]
                   ,[MailAddress1]
                   ,[MailAddress2]
                   ,[MailAddress3]
                   ,[DeleteFlg]
                   ,[UsedFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                SELECT
                    @CustomerCD
                   ,@MainStoreCD
                   ,@ChangeDate
                   ,1 AS SEQ
                   ,@MailAddress
                   ,NULL AS MailAddress2
                   ,NULL AS MailAddress3
                   ,@DeleteFlg
                   ,0 AS UsedFlg
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ;
            END
        END
        ELSE
        BEGIN
        	DELETE FROM [M_CustomerMail]
            WHERE CustomerCD = @CustomerCD
            AND StoreCD = @MainStoreCD
            AND ChangeDate = @ChangeDate
            AND SEQ = 1
            ; 
        END
    END
    
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME ,
        @Operator,
        'MasterTouroku_Tokuisaki',
        @PC,
        @OperateModeNm,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


