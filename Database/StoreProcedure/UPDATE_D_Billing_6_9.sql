 BEGIN TRY 
 Drop Procedure dbo.[UPDATE_D_Billing_6_9]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[UPDATE_D_Billing_6_9]
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
    UPDATE [D_Billing] SET
         [UpdateOperator]     =  @Operator  
        ,[UpdateDateTime]     =  @SYSDATETIME
        ,[DeleteOperator]     =  @Operator  
        ,[DeleteDateTime]     =  @SYSDATETIME
    FROM D_BillingDetails AS DB
    WHERE DB.SalesNO = @SalesNO
--    AND DB.DeleteDateTime IS NULL	削除済みのため 
    AND D_Billing.BillingNO = DB.BillingNO
    AND D_Billing.DeleteDateTime IS NULL
    ;
END


