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
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    
    IF @UpdatingFlg = 1
    BEGIN
        UPDATE [M_JANCounter]
        SET [JanCount]       = JanCount + 1
           ,[UpdatingFlg]    = @UpdatingFlg
           ,[UpdateOperator] = @Operator  
           ,[UpdateDateTime] = SYSDATETIME()
        WHERE MainKEY = @MainKEY
        ;
    END
    ELSE
    BEGIN
        UPDATE [M_JANCounter]
        SET [JanCount]       = M_JANCounter.BeforeJanCount
           ,[UpdatingFlg]    = @UpdatingFlg
           ,[UpdateOperator] = @Operator  
           ,[UpdateDateTime] = SYSDATETIME()
        WHERE MainKEY = @MainKEY
        ;
    END
END

GO
