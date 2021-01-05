 BEGIN TRY 
 Drop Procedure dbo.[M_JANCounter_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE M_JANCounter_Update
   (@MainKEY       tinyint,
    @UpdatingFlg   tinyint,
    @Operator      varchar(10)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    IF @UpdatingFlg = 1
    BEGIN
        UPDATE [M_JANCounter]
        SET [JanCount]       = JanCount + 1
           ,[UpdatingFlg]    = @UpdatingFlg
           ,[UpdateOperator] = @Operator  
           ,[UpdateDateTime] = SYSDATETIME()
        WHERE MainKEY = @MainKEY
        ;
        
        SELECT JanCount FROM [M_JANCounter]
        WHERE MainKEY = @MainKEY
        ;
    END
    ELSE IF @UpdatingFlg = 0  --リセット時
    BEGIN
        UPDATE [M_JANCounter]
        SET [JanCount]       = M_JANCounter.BeforeJanCount
           ,[UpdatingFlg]    = @UpdatingFlg
           ,[UpdateOperator] = @Operator  
           ,[UpdateDateTime] = SYSDATETIME()
        WHERE MainKEY = @MainKEY
        ;
        
        SELECT JanCount FROM [M_JANCounter]
        WHERE MainKEY = @MainKEY
        ;
    END
    ELSE IF @UpdatingFlg = 2  --更新時
    BEGIN
        UPDATE [M_JANCounter]
        SET [BeforeJanCount] = M_JANCounter.JanCount
           ,[UpdatingFlg]    = 0
           ,[UpdateOperator] = @Operator  
           ,[UpdateDateTime] = SYSDATETIME()
        WHERE MainKEY = @MainKEY
        ;
        
        SELECT JanCount FROM [M_JANCounter]
        WHERE MainKEY = @MainKEY
        ;
    END
END

GO
