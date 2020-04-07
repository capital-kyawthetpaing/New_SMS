 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectPlan_4_1]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectPlan_4_1]
(
    @DataNo int,
    @Datatype tinyint,		--0:全額,1:一部
    @Program varchar(50),
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @FirstCollectPlanDate date,
    @JuchuuNO varchar(11),
    @CollectPlanNO int OUTPUT
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @D_CollectPlanInserted TABLE (
         CollectPlanNO int
         );
         
	--テーブル転送仕様４－①－a 回収予定(全額掛)
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
    OUTPUT INSERTED.CollectPlanNO INTO @D_CollectPlanInserted(CollectPlanNO)
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
       ,(CASE @Datatype WHEN 0 THEN DH.SalesGaku ELSE DH.CreditAmount END)    --CollectPlanGaku
       ,2   --<BillingType 2:締請求
       ,NULL	--BillingDate
       ,NULL	--BillingNO
       ,NULL
       ,(SELECT top 1 M.DenominationCD FROM M_DenominationKBN AS M
        	WHERE M.SystemKBN = 9	--掛
        	AND M.MainFLG = 1)		--代表KBN
       ,0   --CardProgressKBN
       ,0   --PaymentProgressKBN
       ,0   --InvalidFLG
       ,NULL    --BillingCloseDate, date,>
       ,@FirstCollectPlanDate
       ,0   --ReminderFLG
       ,NULL    --NoReminderDate
       ,@FirstCollectPlanDate    --NextCollectPlanDate
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
	
END



