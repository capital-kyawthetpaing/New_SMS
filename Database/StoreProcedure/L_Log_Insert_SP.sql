 BEGIN TRY 
 Drop Procedure dbo.[L_Log_Insert_SP]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [L_Log_Insert_SP]    */
/* ストアドよりCALLされるログ出力*/
CREATE PROCEDURE [dbo].[L_Log_Insert_SP](
    -- Add the parameters for the stored procedure here
    @OperateDateTime datetime,
    @InsertOperator  varchar(10),
    @Program         varchar(100),
    @PC              varchar(30),
    @OperateMode     varchar(50),
    @KeyItem         varchar(100)
)AS
BEGIN

DECLARE @OperateDate date;
DECLARE @OperateTime time(7);

SET @OperateDate = CONVERT (date, @OperateDateTime);
SET @OperateTime = CONVERT (time, @OperateDateTime);
 

INSERT INTO L_Log
           ([OperateDate]
           ,[OperateTime]
           ,[InsertOperator]
           ,[Program]
           ,[PC]
           ,[OperateMode]
           ,[KeyItem])
     VALUES
           (@OperateDate
           ,@OperateTime
           ,@InsertOperator
           ,@Program
           ,@PC
           ,@OperateMode
           ,@KeyItem
           );
END


