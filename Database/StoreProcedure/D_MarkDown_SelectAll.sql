BEGIN TRY 
 Drop Procedure dbo.[D_MarkDown_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    マークダウン一覧
--       Program ID      D_MarkDown_SelectAll
--       Create date:    2020.06.07
--    ======================================================================
CREATE PROCEDURE [dbo].[D_MarkDown_SelectAll]
    @VendorCD as varchar(13),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @ChkNotAccount varchar(1),
    @ChkAccounted varchar(1),
    @CostingDateFrom as varchar(10),
    @CostingDateTo as varchar(10),
    @PurchaseDateFrom as varchar(10),
    @PurchaseDateTo as varchar(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SYSDATE Date;
    SET @SYSDATE = GETDATE();
    
    SELECT DH.VendorCD
      ,MV.VendorName
      ,DH.StaffCD          
      ,MS.StaffName
      ,DH.MarkDownDate
      ,DH.CostingDate
      ,CASE WHEN DH.PurchaseDate IS NULL THEN DM.MarkDownGaku ELSE DM.PurchaseGaku END AS MarkDownGaku
      ,DH.PurchaseDate
      ,DH.Comment
      ,DH.MarkDownNO
    FROM (
        SELECT DH.VendorCD
              ,DH.StaffCD  
              ,DH.MarkDownDate
              ,DH.CostingDate
              ,DH.PurchaseDate
              ,DH.Comment
              ,DH.MarkDownNO
              ,DH.StoreCD
              ,DH.InsertDateTime  
        FROM D_MarkDown DH
        WHERE DH.DeleteDateTime IS NULL
          AND (@ChkNotAccount = '1' AND DH.PurchaseDate IS NULL
                                     AND DH.CostingDate >= (CASE WHEN @CostingDateFrom <> '' THEN CONVERT(DATE, @CostingDateFrom) ELSE DH.CostingDate END)
                                     AND DH.CostingDate <= (CASE WHEN @CostingDateTo <> '' THEN CONVERT(DATE, @CostingDateTo) ELSE DH.CostingDate END))
        UNION ALL
        SELECT DH.VendorCD
              ,DH.StaffCD  
              ,DH.MarkDownDate
              ,DH.CostingDate
              ,DH.PurchaseDate
              ,DH.Comment
              ,DH.MarkDownNO
              ,DH.StoreCD
              ,DH.InsertDateTime  
        FROM D_MarkDown DH
        WHERE DH.DeleteDateTime IS NULL
          AND (@ChkAccounted = '1' AND DH.PurchaseDate IS NOT NULL
                                    AND DH.PurchaseDate >= (CASE WHEN @PurchaseDateFrom <> '' THEN CONVERT(DATE, @PurchaseDateFrom) ELSE DH.PurchaseDate END)
                                    AND DH.PurchaseDate <= (CASE WHEN @PurchaseDateTo <> '' THEN CONVERT(DATE, @PurchaseDateTo) ELSE DH.PurchaseDate END))
    ) DH
    LEFT JOIN F_Vendor(@SYSDATE) AS MV ON DH.VendorCD = MV.VendorCD 
    LEFT JOIN F_Staff(@SYSDATE) AS MS ON DH.StaffCD = MS.StaffCD 
    LEFT JOIN ( SELECT MarkDownNO , SUM(MarkDownGaku) AS MarkDownGaku, SUM(PurchaseGaku) AS PurchaseGaku
                  FROM D_MarkDownDetails
                 GROUP BY MarkDownNO ) DM ON DH.MarkDownNO = DM.MarkDownNO
    WHERE DH.StoreCD = @StoreCD 
      AND DH.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DH.VendorCD END)
      AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
    ORDER BY  DH.MarkDownDate
             ,DH.InsertDateTime                    
                    
END

