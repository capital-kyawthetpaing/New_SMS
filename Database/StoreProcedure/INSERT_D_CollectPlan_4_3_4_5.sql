 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectPlan_4_3_4_5]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectPlan_4_3_4_5]
(
	@KBN tinyint,		--1:掛,2:現金,3:カード,4:その他①,5:その他
    @DataNo int,
    @Datatype tinyint,		--0:全額,1:一部
    @Program varchar(50),
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @FirstCollectPlanDate date,
    @JuchuuNO varchar(11),
    @CollectPlanNO int OUTPUT,
    @BillingNO varchar(11) OUTPUT,
--    @CollectNO varchar(11) OUTPUT,
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
    
    --テーブル転送仕様４－③－a 回収予定(全額カード等)
    --テーブル転送仕様４－④－a 回収予定(全額その他①)
    --テーブル転送仕様４－⑤－a 回収予定(全額その他②)
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
       ,(SELECT top 1 A.BillingCD FROM D_StorePayment AS DS 
            INNER JOIN M_DenominationKBN AS A
            ON A.DenominationCD = (CASE @KBN WHEN 3 THEN DS.CardDenominationCD
                                        WHEN 4 THEN DS.Denomination1CD
                                        WHEN 5 THEN DS.Denomination2CD
                                        END)
            WHERE DS.SalesNO = DH.SalesNO
            AND DS.StoreCD = DH.StoreCD
            ORDER BY DS.SalesNORows DESC
		)--CustomerCD
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku ELSE 0 END)      --HontaiGaku
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku0 ELSE 0 END)     --HontaiGaku0
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku8 ELSE 0 END)     --HontaiGaku8
       ,(CASE @Datatype WHEN 0 THEN DH.SalesHontaiGaku10 ELSE 0 END)    --HontaiGaku10
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax ELSE 0 END)     --Tax
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax8 ELSE 0 END)    --Tax8
       ,(CASE @Datatype WHEN 0 THEN DH.SalesTax10 ELSE 0 END)   --Tax10
       ,(CASE @Datatype WHEN 0 THEN DH.SalesGaku ELSE (CASE @KBN WHEN 3 THEN DH.CardAmount
                                        WHEN 4 THEN DH.Denomination1Amount
                                        WHEN 5 THEN DH.Denomination2Amount
                                        END) END)    --CollectPlanGaku
       ,1   --<BillingType 1:即請求
       ,@SYSDATE	--BillingDate
       ,'9'		--BillingNO
       ,'9' --MonthlyBillingNO
       ,(SELECT top 1 (CASE @KBN WHEN 3 THEN DS.CardDenominationCD
                                        WHEN 4 THEN DS.Denomination1CD
                                        WHEN 5 THEN DS.Denomination2CD
                                        END)
            FROM D_StorePayment AS DS
            WHERE DS.SalesNO = DH.SalesNO
            AND DS.StoreCD = DH.StoreCD
            ORDER BY DS.SalesNORows DESC
        )   --PaymentMethodCD
       ,1   --CardProgressKBN
       ,0   --PaymentProgressKBN
       ,0   --InvalidFLG
       ,@SYSDATE    --BillingCloseDate, date,>
       ,@FirstCollectPlanDate	--FirstCollectPlanDate
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
END



