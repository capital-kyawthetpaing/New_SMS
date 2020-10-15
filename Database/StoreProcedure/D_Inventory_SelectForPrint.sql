/****** Object:  StoredProcedure [dbo].[D_Inventory_SelectForPrint]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Inventory_SelectForPrint]
GO

/****** Object:  StoredProcedure [D_Inventory_SelectForPrint]    */
CREATE PROCEDURE D_Inventory_SelectForPrint(
    -- Add the parameters for the stored procedure here
    @SoukoCD        varchar(6),
    @InventoryDate  varchar(10),
    @ChkSaiOnly     tinyint,
    @KbnSai         tinyint,    -- ç∑àŸï\ÇÃèÍçáÇPÅAíIâµï\ÇÃèÍçáÇO
    @ChkKinyu       tinyint     -- íIâµãLì¸ï\ÇÃèÍçá2
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @KbnSai = 1
    BEGIN
        IF @ChkSaiOnly = 1
        BEGIN
            SELECT DI.SoukoCD
              ,DI.RackNO
              ,CONVERT(varchar,DI.InventoryDate,111) AS InventoryDate
              ,DI.SKUCD
              ,DI.AdminNO
              ,DI.JanCD
              ,DI.TheoreticalQuantity
              ,DI.ActualQuantity
              ,DI.DifferenceQuantity
              ,DI.InventoryNO
              ,(SELECT top 1 M.SKUName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS SKUName
              ,(SELECT top 1 M.ColorName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS ColorName
              ,(SELECT top 1 M.SizeName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS SizeName
              ,(SELECT top 1 MB.BrandName FROM M_SKU AS M 
                INNER JOIN M_Brand AS MB
                ON MB.BrandCD = M.BrandCD
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS BrandName
            
            FROM D_Inventory AS DI
            INNER JOIN D_InventoryControl AS DC
            ON DC.SoukoCD = DI.SoukoCD
            AND DC.RackNO = DI.RackNO
            AND DC.InventoryDate = DI.InventoryDate
            AND DC.InventoryNO = DI.InventoryNO
            AND DC.InventoryKBN <> 2

            WHERE DI.DeleteDateTime IS NULL
            AND DI.SoukoCD = @SoukoCD
            AND DI.InventoryDate = CONVERT(date, @InventoryDate)
            AND DI.DifferenceQuantity <> 0
            ORDER BY DI.RackNO, DI.JanCD ,DI.SKUCD
            ;
        END
        ELSE
        BEGIN
            SELECT DI.SoukoCD
              ,DI.RackNO
              ,CONVERT(varchar,DI.InventoryDate,111) AS InventoryDate
              ,DI.SKUCD
              ,DI.AdminNO
              ,DI.JanCD
              ,DI.TheoreticalQuantity
              ,DI.ActualQuantity
              ,DI.DifferenceQuantity
              ,DI.InventoryNO
              ,(SELECT top 1 M.SKUName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS SKUName
              ,(SELECT top 1 M.ColorName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS ColorName
              ,(SELECT top 1 M.SizeName FROM M_SKU AS M 
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS SizeName
              ,(SELECT top 1 MB.BrandName FROM M_SKU AS M 
                INNER JOIN M_Brand AS MB
                ON MB.BrandCD = M.BrandCD
                WHERE M.AdminNO = DI.AdminNO
                AND M.ChangeDate <= DI.InventoryDate
                AND M.DeleteFlg = 0 
                ORDER BY M.ChangeDate desc) AS BrandName
            
            FROM D_Inventory AS DI
            INNER JOIN D_InventoryControl AS DC
            ON DC.SoukoCD = DI.SoukoCD
            AND DC.RackNO = DI.RackNO
            AND DC.InventoryDate = DI.InventoryDate
            AND DC.InventoryNO = DI.InventoryNO
            AND DC.InventoryKBN = 1

            WHERE DI.DeleteDateTime IS NULL
            AND DI.SoukoCD = @SoukoCD
            AND DI.InventoryDate = CONVERT(date, @InventoryDate)

            ORDER BY DI.RackNO, DI.JanCD ,DI.SKUCD
            ;
        END
    END
    ELSE
    BEGIN
        SELECT DI.SoukoCD + ' ' + ISNULL((SELECT top 1 M.SoukoName FROM M_Souko AS M
                                    WHERE M.SoukoCD = DI.SoukoCD
                                    AND M.ChangeDate <= DI.InventoryDate
                                    AND M.DeleteFlg = 0
                                    ORDER BY M.ChangeDate desc),'') As Souko
          ,@ChkKinyu AS Kinyu
          ,(CASE @ChkKinyu WHEN 2 THEN 'íIâµï\ÅiãLì¸ï\)' ELSE 'íIâµï\' END) AS Title
          ,DI.RackNO
          ,CONVERT(varchar,DI.InventoryDate,111) AS InventoryDate
          ,DI.SKUCD
          ,DI.AdminNO
          ,DI.JanCD
          ,DI.TheoreticalQuantity
          ,(CASE @ChkKinyu WHEN 2 THEN NULL ELSE DI.ActualQuantity END) AS ActualQuantity
          ,DI.DifferenceQuantity
          ,DI.InventoryNO
          ,(SELECT top 1 M.SKUName FROM M_SKU AS M 
            WHERE M.AdminNO = DI.AdminNO
            AND M.ChangeDate <= DI.InventoryDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName FROM M_SKU AS M 
            WHERE M.AdminNO = DI.AdminNO
            AND M.ChangeDate <= DI.InventoryDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName FROM M_SKU AS M 
            WHERE M.AdminNO = DI.AdminNO
            AND M.ChangeDate <= DI.InventoryDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS SizeName
          ,(SELECT top 1 MB.BrandName FROM M_SKU AS M 
            INNER JOIN M_Brand AS MB
            ON MB.BrandCD = M.BrandCD
            WHERE M.AdminNO = DI.AdminNO
            AND M.ChangeDate <= DI.InventoryDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS BrandName
        
        FROM D_Inventory AS DI
        INNER JOIN D_InventoryControl AS DC
        ON DC.SoukoCD = DI.SoukoCD
        AND DC.RackNO = DI.RackNO
        AND DC.InventoryDate = DI.InventoryDate
        AND DC.InventoryNO = DI.InventoryNO
        AND DC.InventoryKBN <> 2

        WHERE DI.DeleteDateTime IS NULL
        AND DI.SoukoCD = @SoukoCD
        AND DI.InventoryDate = CONVERT(date, @InventoryDate)

        ORDER BY DI.RackNO, DI.JanCD ,DI.SKUCD
        ;
    END
END

GO
