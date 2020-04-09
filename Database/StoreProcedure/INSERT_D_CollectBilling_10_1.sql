 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectBilling_10_1]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectBilling_10_1]
(
	@ConfirmNO varchar(11),
	@CollectPlanNO int,
    @Operator  varchar(10),
    @SYSDATETIME  datetime
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
	--テーブル転送仕様10－①Insert入金消込請求 D_CollectBilling
	--店頭で入金なので今作る、回収予定ごと
    INSERT INTO [D_CollectBilling]
       ([ConfirmNO]
       ,[CollectPlanNO]
       ,[CollectAmount]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
    SELECT
        @ConfirmNO
       ,DC.CollectPlanNO
       ,DC.CollectPlanGaku   --CollectAmount
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


