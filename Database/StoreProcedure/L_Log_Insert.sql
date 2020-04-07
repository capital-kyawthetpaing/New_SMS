 BEGIN TRY 
 Drop Procedure dbo.[L_Log_Insert]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [L_Log_Insert]    */
/* DLクラスから呼ばれるストアド  */
/* 日付指定がない場合 */
CREATE PROCEDURE [dbo].[L_Log_Insert](
    -- Add the parameters for the stored procedure here
    @InsertOperator  varchar(10),
    @Program         varchar(100),
    @PC              varchar(30),
    @OperateMode     varchar(50),
    @KeyItem         varchar(100)
)AS
BEGIN

DECLARE @OperateDateTime datetime;

SET @OperateDateTime = SYSDATETIME();

    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @OperateDateTime ,
        @InsertOperator,
        @Program,
        @PC,
        @OperateMode,
        @KeyItem;
        
END


