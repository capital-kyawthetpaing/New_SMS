 BEGIN TRY 
 Drop Procedure dbo.[UPDATE_D_CollectBilling_10_9]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[UPDATE_D_CollectBilling_10_9]
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

    UPDATE [D_CollectBilling] SET
         [UpdateOperator]     =  @Operator  
        ,[UpdateDateTime]     =  @SYSDATETIME
        ,[DeleteOperator]     =  @Operator  
        ,[DeleteDateTime]     =  @SYSDATETIME
    FROM D_CollectPlan AS DC                                      
    WHERE DC.SalesNO = @SalesNO
    AND DC.DeleteDateTime IS NULL
    AND D_CollectBilling.CollectPlanNO = DC.CollectPlanNO
    AND D_CollectBilling.DeleteDateTime IS NULL
    ;
END



