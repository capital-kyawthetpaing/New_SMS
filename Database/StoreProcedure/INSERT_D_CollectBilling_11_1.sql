 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectBilling_11_1]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectBilling_11_1]
(
	@ConfirmNO varchar(11),
	@CollectPlanNO int,
	@CollectPlanRows int,
    @Operator  varchar(10),
    @SYSDATETIME  datetime
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
	--テーブル転送仕様11－①Insert入金消込明細 D_CollectBillingDetails
    --店頭で入金なので今作る、回収予定明細ごと
    INSERT INTO [D_CollectBillingDetails]
       ([ConfirmNO]
       ,[CollectPlanNO]
       ,[CollectPlanRows]
       ,[CollectAmount]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
    SELECT
        @ConfirmNO
       ,DM.CollectPlanNO
       ,DM.CollectPlanRows
       ,DM.CollectPlanGaku  --CollectAmount
       ,@Operator   --InsertOperator, varchar(10),>
       ,@SYSDATETIME    --InsertDateTime, datetime,>
       ,@Operator   --UpdateOperator, varchar(10),>
       ,@SYSDATETIME    --UpdateDateTime, datetime,>
       ,NULL    --DeleteOperator, varchar(10),>
       ,NULL    --DeleteDateTime, datetime,>
    FROM D_CollectPlanDetails AS DM
    WHERE DM.CollectPlanNO = @CollectPlanNO       
    AND DM.CollectPlanRows = @CollectPlanRows
    AND DM.DeleteDateTime IS Null
    AND NOT EXISTS(SELECT 1 FROM D_CollectBillingDetails AS A
        WHERE A.ConfirmNO = @ConfirmNO
        AND A.CollectPlanNO = @CollectPlanNO 
        AND A.CollectPlanRows = @CollectPlanRows)               
    ;
   
END


