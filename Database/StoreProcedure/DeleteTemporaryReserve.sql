DROP  PROCEDURE [dbo].[DeleteTemporaryReserve]
GO


--  ======================================================================
--       Program Call    éÛíçì¸óÕ
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE DeleteTemporaryReserve
    (@JuchuuNO   varchar(11)
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN

    --ÅyTemporaryReserveÅz
    DELETE FROM D_TemporaryReserve
    WHERE [Number] = @JuchuuNO
    ;


END

GO
