DROP  PROCEDURE [dbo].[DeleteTemporaryReserve]
GO


--  ======================================================================
--       Program Call    éÛíçì¸óÕ
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE DeleteTemporaryReserve
    (@JuchuuNO   varchar(11)	--TennicÇÃèÍçáÇÕJuchuuProcessNOÇ™äiî[Ç≥ÇÍÇƒÇ¢ÇÈ
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN

    DECLARE @Tennic tinyint;
	
    SET @Tennic = (SeLECT M.Tennic FROM M_Control AS M WHERE M.MainKey = 1);
    
    --ÅyTemporaryReserveÅz
    IF @Tennic = 1
    BEGIN
        DELETE FROM D_TemporaryReserve
        WHERE [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                            WHERE H.JuchuuProcessNO = @JuchuuNO)
        ;
    END
    ELSE
    BEGIN
        DELETE FROM D_TemporaryReserve
        WHERE [Number] = @JuchuuNO
        ;
    END
END

GO
