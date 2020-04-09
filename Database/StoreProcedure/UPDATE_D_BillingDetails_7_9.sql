 BEGIN TRY 
 Drop Procedure dbo.[UPDATE_D_BillingDetails_7_9]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UPDATE_D_BillingDetails_7_9]
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

    UPDATE [D_BillingDetails] SET
         [UpdateOperator]     =  @Operator  
        ,[UpdateDateTime]     =  @SYSDATETIME
        ,[DeleteOperator]     =  @Operator  
        ,[DeleteDateTime]     =  @SYSDATETIME
    WHERE SalesNO = @SalesNO
    AND DeleteDateTime IS NULL
    ;
END



