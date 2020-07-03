DROP  PROCEDURE [dbo].[DeleteTemporaryReserve]
GO


--  ======================================================================
--       Program Call    �󒍓���
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE DeleteTemporaryReserve
    (@JuchuuNO   varchar(11)	--Tennic�̏ꍇ��JuchuuProcessNO���i�[����Ă���
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN

    DECLARE @Tennic tinyint;
	
    SET @Tennic = (SeLECT M.Tennic FROM M_Control AS M WHERE M.MainKey = 1);
    
    --�yTemporaryReserve�z
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
