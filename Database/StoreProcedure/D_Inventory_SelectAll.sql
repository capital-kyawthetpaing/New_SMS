 BEGIN TRY 
 Drop Procedure dbo.[D_Inventory_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [D_Inventory_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Inventory_SelectAll](
    -- Add the parameters for the stored procedure here
    @SoukoCD  varchar(6),
    @InventoryDate  varchar(10)
    
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
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


