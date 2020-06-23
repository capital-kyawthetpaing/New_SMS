DROP  PROCEDURE [dbo].[GetTemporaryReserveNO]
GO

--  ======================================================================
--       Program Call    éÛíçì¸óÕ
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE GetTemporaryReserveNO
    (@JuchuuNO   varchar(11)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    DECLARE @OutNO      varchar(11);
    SET @OutNO = '';
    
    IF ISNULL(@JuchuuNO ,'') <> ''
    BEGIN
        SET @OutNO = @JuchuuNO
    END
    ELSE
    BEGIN
        EXEC Fnc_GetNumber
            13  --@SeqKBN      tinyint,
            ,NULL --@ChangeDate  varchar(10),
            ,NULL   --@StoreCD     varchar(4),
            ,NULL   --@Operator  varchar(10),
            ,@OutNO  OUTPUT 
            ;
    END
    
    SELECT @OutNO AS OutNO;

END

GO
