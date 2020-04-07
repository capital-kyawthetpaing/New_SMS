 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectPlan_4_2_6]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectPlan_4_2_6]
(
	@KBN tinyint,	--2:現金,6:前受金
    @DataNo int,
    @Datatype tinyint,
    @Program varchar(50),
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @JuchuuNO varchar(11),
    @CollectPlanNO int OUTPUT,
    @BillingNO varchar(11) OUTPUT,
    @CollectNO varchar(11) OUTPUT,
    @W_ERR tinyint OUTPUT
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @D_CollectPlanInserted TABLE (
         CollectPlanNO int,
         StoreCD varchar(4)
         );

    DECLARE @StoreCD   varchar(4);
    DECLARE @SYSDATE date;
    SET @W_ERR = 0;
    SET @SYSDATE = CONVERT(date,@SYSDATETIME);
    
    --テーブル転送仕様４－②－a 回収予定(全額現金)
    --テーブル転送仕様４－②－b 回収予定(一部現金) 
    --テーブル転送仕様４－⑥－a 回収予定(全額前受金)
    --テーブル転送仕様４－⑥－b 回収予定(一部前受金)
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
    OUTPUT INSERTED.CollectPlanNO, INSERTED.StoreCD INTO @D_CollectPlanInserted(CollectPlanNO, StoreCD)
    SELECT
       --CollectPlanNO
       DH.SalesNO
       ,@JuchuuNO
       ,2   --JuchuuKBN 店舗
       ,DH.StoreCD
       ,DH.CustomerCD
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku ELSE 0 END)      --HontaiGaku
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku0 ELSE 0 END)     --HontaiGaku0
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku8 ELSE 0 END)     --HontaiGaku8
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku10 ELSE 0 END)    --HontaiGaku10
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax ELSE 0 END)     --Tax
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax8 ELSE 0 END)    --Tax8
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax10 ELSE 0 END)   --Tax10
       ,(CASE @Datatype WHEN 0 THEN DH.SalesGaku ELSE (CASE @KBN WHEN 2 THEN DH.CreditAmount
       													WHEN 6 THEN DH.AdvanceAmount END) END)    --CollectPlanGaku
       ,1   --<BillingType 1:即請求
       ,@SYSDATE	--BillingDate
       ,'9'		--BillingNO
       ,'9'	--MonthlyBillingNO
       ,(SELECT top 1 M.DenominationCD FROM M_DenominationKBN AS M
        	WHERE M.SystemKBN = (CASE @KBN WHEN 2 THEN 1
        								WHEN 6 THEN 13
        								END)	--現金
        	AND M.MainFLG = 1)		--代表KBN
       ,0   --CardProgressKBN
       ,0   --PaymentProgressKBN
       ,0   --InvalidFLG
       ,@SYSDATE    --BillingCloseDate, date,>
       ,@SYSDATE
       ,0   --ReminderFLG
       ,NULL    --NoReminderDate
       ,@SYSDATE    --NextCollectPlanDate
       ,NULL    --ActionCD
       ,NULL    --NextActionCD
       ,NULL    --LastReminderNO
       ,@Program  --Program
       ,0   --BillingConfirmFlg
       ,@Datatype	--1：店舗レジでの一部掛入金
       ,@Operator
       ,@SYSDATETIME
       ,@Operator
       ,@SYSDATETIME
       ,NULL --DeleteOperator
       ,NULL --DeleteDateTime
   FROM D_SalesTran AS DH
	WHERE DH.DataNo = @DataNo
   ;

    SET @CollectPlanNO = (SELECT CollectPlanNO FROM @D_CollectPlanInserted);
    SET @StoreCD = (SELECT StoreCD FROM @D_CollectPlanInserted);
    
    DECLARE @BillingDate varchar(10);
    SET @BillingDate = CONVERT(varchar,@SYSDATE,111);

    --テーブル転送仕様６－①Insert請求①　D_Billing
    EXEC INSERT_D_Billing_6_1
        @StoreCD       --in店舗CD
        ,@Program
        ,@Operator
        ,@SYSDATETIME
        ,@CollectPlanNO
        ,@BillingNO OUTPUT
        ,@W_ERR OUTPUT
        ;
        
	--テーブル転送仕様８－①Insert入金①D_Collect店頭で入金なので今作る、回収予定ごと
    --D_Collect
    --伝票番号採番
    EXEC Fnc_GetNumber
        7,             --in伝票種別 7
        @SYSDATE, --D_CollectPlan.BillingDate in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @CollectNO OUTPUT
        ;
    
    IF ISNULL(@CollectNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END

    INSERT INTO [D_Collect]
       ([CollectNO]
       ,[InputKBN]
       ,[StoreCD]
       ,[StaffCD]
       ,[InputDatetime]
       ,[WebCollectNO]
       ,[WebCollectType]
       ,[CollectCustomerCD]
       ,[CollectDate]
       ,[PaymentMethodCD]
       ,[KouzaCD]
       ,[BillDate]
       ,[CollectAmount]
       ,[FeeDeduction]
       ,[Deduction1]
       ,[Deduction2]
       ,[ConfirmSource]
       ,[ConfirmAmount]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
 	SELECT
        @CollectNO
       ,3   --InputKBN, tinyint     1:取込,3：POS
       ,DC.StoreCD
       ,@Operator
       ,@SYSDATETIME    --InputDatetime
       ,NULL	--WebCollectNO
       ,NULL	--WebCollectType
       ,DC.CustomerCD
       ,@SYSDATETIME
       ,DC.PaymentMethodCD
       ,NULL	--KouzaCD
       
       ,NULL	--BillDate
       ,DC.CollectPlanGaku     --CollectAmount
       ,0	--FeeDeduction
       ,0	--Deduction1
       ,0	--Deduction2
       ,DC.CollectPlanGaku	--ConfirmSource
       ,DC.CollectPlanGaku	--ConfirmAmount
       ,@Operator   --InsertOperator, varchar(10),>
       ,@SYSDATETIME    --InsertDateTime, datetime,>
       ,@Operator   --UpdateOperator, varchar(10),>
       ,@SYSDATETIME    --UpdateDateTime, datetime,>
       ,NULL    --DeleteOperator, varchar(10),>
       ,NULL    --DeleteDateTime, datetime,>
	FROM D_CollectPlan AS DC
    WHERE DC.CollectPlanNO = @CollectPlanNO
    ;

END



