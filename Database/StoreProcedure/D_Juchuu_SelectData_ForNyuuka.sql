BEGIN TRY 
 Drop Procedure [D_Juchuu_SelectData_ForNyuuka]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[D_Juchuu_SelectData_ForNyuuka]    Script Date: 2019/09/15 19:54:54 ******/

--  ======================================================================
--       Program Call    ì¸â◊ì¸óÕ éÛíçè∆âÔâÊñ 
--       Program ID      NyuukaNyuuryoku
--       Create date:    2019.11.15
--    ======================================================================
CREATE PROCEDURE [D_Juchuu_SelectData_ForNyuuka]
    (    @JuchuuNO   varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 èàóùäJénÅE                 --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DM.SKUCD
          ,DM.JanCD
          ,DM.SKUName
          ,DM.ColorName
          ,DM.SizeName
          ,DM.JuchuuSuu
          ,CONVERT(varchar,DO.ArrivePlanDate,111) AS ArrivePlanDate
          ,(CASE WHEN DO.DirectFLG = 1 THEN 'ÅZ' ELSE '' END) AS DirectFLG

      FROM D_JuchuuDetails AS DM
      LEFT OUTER JOIN D_OrderDetails AS DO
      ON DO.JuchuuNO = DM.JuchuuNO
      AND DO.JuchuuRows = DM.JuchuuRows
      AND DO.DeleteDateTime IS Null

      WHERE DM.JuchuuNO = @JuchuuNO               
      AND DM.DeleteDateTime IS Null
      ORDER BY DM.DisplayRows
      ;
END

GO

