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
    @Operator      varchar(10)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    
    UPDATE [M_JANCounter]
    SET [JanCount]       = JanCount + 1
       ,[UpdatingFlg]    = 1
       ,[UpdateOperator] = @Operator  
       ,[UpdateDateTime] = SYSDATETIME()
    WHERE MainKEY = @MainKEY
    ;
    

END

GO
