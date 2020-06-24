DROP  PROCEDURE [dbo].[DeleteTemporaryReserve]
GO


--  ======================================================================
--       Program Call    �󒍓���
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE DeleteTemporaryReserve
    (@JuchuuNO   varchar(11)
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN

    --�yTemporaryReserve�z
    DELETE FROM D_TemporaryReserve
    WHERE [Number] = @JuchuuNO
    ;


END

GO
