 BEGIN TRY 
 Drop Procedure dbo.[PRC_ZaikoIdou_A2B2]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[PRC_ZaikoIdou_A2B2]
   (@MoveNO   varchar(11),
    @Operator  varchar(10),
    @SYSDATETIME  datetime
)AS
BEGIN

    --【D_Move】在庫移動　テーブル転送仕様A②
    UPDATE [D_Move]
        SET [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
           ,[DeleteOperator]     =  @Operator  
           ,[DeleteDateTime]     =  @SYSDATETIME
     WHERE [MoveNO] = @MoveNO
     ;
    
    --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ②
    UPDATE [D_MoveDetails]
        SET [DeleteOperator]     =  @Operator  
           ,[DeleteDateTime]     =  @SYSDATETIME
     WHERE [MoveNO] = @MoveNO
     AND [DeleteDateTime] IS NULL
     ;
         
END


