 BEGIN TRY 
 Drop Procedure dbo.[UPDATE_D_PaymentConfirm_9_9]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[UPDATE_D_PaymentConfirm_9_9]
(
    @SalesNO varchar(11),
    @Operator  varchar(10),
    @SYSDATETIME  datetime
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

    UPDATE [D_PaymentConfirm] SET
         [UpdateOperator]     =  @Operator  
        ,[UpdateDateTime]     =  @SYSDATETIME
        ,[DeleteOperator]     =  @Operator  
        ,[DeleteDateTime]     =  @SYSDATETIME
    FROM D_CollectPlan AS DC                    
    INNER JOIN D_CollectBilling AS DCB
    ON DCB.CollectPlanNO = DC.CollectPlanNO
    --AND DCB.DeleteDateTime IS NULL	削除済みのため
    WHERE DC.SalesNO = @SalesNO
    AND DC.DeleteDateTime IS NULL
    AND [D_PaymentConfirm].ConfirmNO = DCB.ConfirmNO
    AND [D_PaymentConfirm].DeleteDateTime IS NULL
    ;
END



