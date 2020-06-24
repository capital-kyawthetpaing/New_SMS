DROP  PROCEDURE [dbo].[DeleteTemporaryReserve]
GO


--  ======================================================================
--       Program Call    受注入力
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE DeleteTemporaryReserve
    (@JuchuuNO   varchar(11)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

    --【TemporaryReserve】
    DELETE FROM D_TemporaryReserve
    WHERE [Number] = @JuchuuNO
    ;


END

GO
