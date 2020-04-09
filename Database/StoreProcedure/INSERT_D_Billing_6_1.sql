 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_Billing_6_1]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_Billing_6_1]
(
	@StoreCD varchar(10),
    @Program varchar(50),
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @CollectPlanNO int,
    @BillingNO varchar(11) OUTPUT,
    @W_ERR tinyint OUTPUT
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @BillingDate varchar(10);
    SET @BillingDate = CONVERT(varchar,@SYSDATETIME,111);
	SET @W_ERR = 0;
	
    --テーブル転送仕様６－①Insert請求①　D_Billing
    --請求は回収予定のレコードごとに１レコード
    --伝票番号採番
    EXEC Fnc_GetNumber
        15,             --in伝票種別 15
        @BillingDate,   --in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @BillingNO OUTPUT
        ;
    
    IF ISNULL(@BillingNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END
    
    --【D_Billing】
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
           ,1	--BillingType	1:即
           ,DC.StoreCD
           ,NULL	--★DC.SalesDate	--BillingCloseDate
           ,NULL	--★DC.SalesDate	--CollectPlanDate
           ,DC.CustomerCD                
           ,'9'    --ProcessingNO
           ,DC.HontaiGaku  --SumBillingHontaiGaku 
           ,DC.HontaiGaku0 --SumBillingHontaiGaku0 
           ,DC.HontaiGaku8 --SumBillingHontaiGaku8 
           ,DC.HontaiGaku10    --SumBillingHontaiGaku10 
           ,DC.Tax --SumBillingTax 
           ,DC.Tax8    --SumBillingTax8 
           ,DC.Tax10   --SumBillingTax10 
           ,DC.CollectPlanGaku    --SumBillingGaku 
           ,0	--AdjustHontaiGaku8 
           ,0	--AdjustHontaiGaku10 
           ,0	--AdjustTax8 
           ,0	--AdjustTax10 
           ,DC.HontaiGaku        
           ,DC.HontaiGaku0
           ,DC.HontaiGaku8
           ,DC.HontaiGaku10
           ,DC.Tax
           ,DC.Tax8
           ,DC.Tax10
           ,DC.CollectPlanGaku
           ,NULL    --PrintDateTime
           ,@Operator    --PrintStaffCD
           ,CONVERT(date,@SYSDATETIME)    --CollectDate
           ,CONVERT(date,@SYSDATETIME)    --LastCollectDate
           ,@Operator    --CollectStaffCD
           ,DC.CollectPlanGaku   --CollectGaku-
           ,DC.CollectPlanGaku   --LastBillingGaku
           ,DC.CollectPlanGaku   --LastCollectGaku 
           ,1   -- BillingConfirmFlg
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
       FROM D_CollectPlan AS DC
       WHERE DC.CollectPlanNO = @CollectPlanNO
       ;
END


