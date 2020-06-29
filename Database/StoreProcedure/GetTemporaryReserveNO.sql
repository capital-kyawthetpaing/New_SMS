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
        SET @OutNO = (SELECT RIGHT('0000000000' + CONVERT(varchar, M.ProcessingCounter + 1), 11)
                    FROM M_TemporaryReserve AS M
                    WHERE M.TemporaryReserveKey = 1
                    );
        
        UPDATE M_TemporaryReserve
        SET ProcessingCounter = ProcessingCounter + 1
        WHERE TemporaryReserveKey = 1
        ;
    END
    
    SELECT @OutNO AS OutNO;

END

GO
