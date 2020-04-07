
/****** Object:  StoredProcedure [dbo].[M_Site_SelectByItemCD]    Script Date: 6/11/2019 2:21:19 PM *****/
DROP PROCEDURE [dbo].[M_Site_SelectByItemCD]
GO

/****** Object:  StoredProcedure [dbo].[M_Site_SelectByItemCD]    Script Date: 6/11/2019 2:21:19 PM *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_Site_SelectByItemCD]    */
CREATE PROCEDURE M_Site_SelectByItemCD(
    -- Add the parameters for the stored procedure here
    @ITemCD varchar(30),
    @ChangeDate varchar(10)    
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.AdminNO
--          ,MS.SKUCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.ColorNO
          ,MS.SizeNO
          
          ,FS.StoreCD
          ,FS.StoreName
          ,FS.APIKey
          ,MT.ShouhinCD
          ,MT.SiteURL

    from F_Store(@ChangeDate) AS FS
    FULL JOIN M_SKU AS MS
    ON MS.ITemCD = (CASE WHEN @ITemCD <> '' THEN @ITemCD ELSE ISNULL(MS.ITemCD,'') END)
    AND MS.ChangeDate = CONVERT(DATE, @ChangeDate)
    
    LEFT OUTER JOIN M_Site AS MT
	ON MT.APIKey = FS.APIKey
    AND MS.AdminNO = MT.AdminNO
    
    WHERE FS.DeleteFlg = 0
    AND FS.StoreKBN = 2
    ORDER BY FS.APIKey, FS.StoreCD, MS.ColorNO, MS.SizeNO
    ;
END

GO
