
/****** Object:  StoredProcedure [dbo].[M_SKU_SelectByMaker]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[M_SKU_SelectByMaker]
GO

/****** Object:  StoredProcedure [dbo].[M_SKU_SelectByMaker]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SKU_SelectByMaker]    */

CREATE PROCEDURE M_SKU_SelectByMaker(
    @MakerItem varchar(30),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.AdminNO
    	  ,MS.SKUName
          ,MS.SKUShortName

    from F_SKU(CONVERT(date,@ChangeDate)) MS

    WHERE MS.MakerItem = (CASE WHEN @MakerItem <> '' THEN @MakerItem ELSE MS.MakerItem END)
    AND MS.DeleteFlg = 0
    ORDER BY MS.ChangeDate desc
    ;
    
END

GO

