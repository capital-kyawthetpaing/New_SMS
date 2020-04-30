IF OBJECT_ID ( 'D_Juchuu_SelectData_ForShukka', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Juchuu_SelectData_ForShukka]
GO

/****** Object:  StoredProcedure [dbo].[D_Juchuu_SelectData_ForShukka]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--  ======================================================================
--       Program Call    出荷入力　JANCDチェック
--       Program ID      ShukkaNyuuryoku
--       Create date:    2019.12.26
--    ======================================================================
CREATE PROCEDURE [D_Juchuu_SelectData_ForShukka]
    (    @JuchuuNO   varchar(11)
    )AS
    

BEGIN

    SET NOCOUNT ON;

    SELECT DH.JuchuuNO
      FROM D_Juchuu AS DH
      WHERE DH.JuchuuNO = @JuchuuNO               
      AND DH.DeleteDateTime IS Null
      AND DH.CancelDate IS Null
      ;
END

GO

