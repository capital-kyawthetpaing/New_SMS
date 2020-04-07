 BEGIN TRY 
 Drop Procedure dbo.[UPDATE_D_CollectPlanDetails_5_9]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[UPDATE_D_CollectPlanDetails_5_9]
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

    UPDATE [D_CollectPlanDetails] SET
        [UpdateOperator]     =  @Operator  
        ,[UpdateDateTime]     =  @SYSDATETIME
        ,[DeleteOperator]     =  @Operator  
        ,[DeleteDateTime]     =  @SYSDATETIME
    WHERE SalesNO = @SalesNO
    AND DeleteDateTime IS NULL
    ;
END



