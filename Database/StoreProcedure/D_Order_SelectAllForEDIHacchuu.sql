IF OBJECT_ID ( 'D_Order_SelectAllForEDIHacchuu', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Order_SelectAllForEDIHacchuu]
GO


--  ======================================================================
--       Program Call    EDIî≠íç
--       Program ID      D_Order_SelectAllForEDIHacchuu
--       Create date:    2019.11.16
--    ======================================================================
CREATE PROCEDURE D_Order_SelectAllForEDIHacchuu(
    @StoreCD  varchar(4),
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),    
    @StaffCD  varchar(10),
    @OrderCD  varchar(13),
    @OrderNO varchar(11),
    @ChkMisyonin varchar(1)
)AS
BEGIN
    SET NOCOUNT ON;

    SELECT DH.StoreCD
          ,DH.OrderCD   
          ,ISNULL(DH.DestinationSoukoCD,'')          
          ,DM.OrderNO
          ,DM.OrderRows
          ,(SELECT top 1 A.MakerItem 
              FROM M_SKU A 
             WHERE A.AdminNO = DM.AdminNO 
               AND A.ChangeDate <= DH.OrderDate 
               AND A.DeleteFlg = 0
             ORDER BY A.ChangeDate desc) AS MakerItem       
          ,DM.SizeName
          ,DM.ColorName
          ,DM.TaniCD
          ,(SELECT top 1 B.BrandKana 
              FROM M_SKU A 
              LEFT JOIN M_Brand B ON A.BrandCD = B.BrandCD
             WHERE A.AdminNO = DM.AdminNO 
               AND A.ChangeDate <= DH.OrderDate 
               AND A.DeleteFlg = 0
             ORDER BY A.ChangeDate desc) AS BrandKana
          ,DM.AdminNO
          ,DM.JanCD
          ,DM.OrderSu      
    
    FROM D_Order AS DH
    INNER JOIN D_OrderDetails AS DM ON DH.OrderNO = DM.OrderNO
                                   AND DM.DeleteDateTime IS NULL   
    WHERE DH.DeleteDateTime IS NULL
      AND DH.StoreCD = @StoreCD
      AND DH.DestinationKBN = 2
      AND DH.OrderWayKBN = 2
      AND DH.ReturnFLG = 0
      AND DM.EDIOutputDatetime IS NULL
      AND DM.EDIFLG = 1
      AND DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
      AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)         
      AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
      AND DH.OrderCD = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE DH.OrderCD END)
      AND DH.OrderNO = (CASE WHEN @OrderNO <> '' THEN @OrderNO ELSE DH.OrderNO END)
      AND ((@ChkMisyonin  = '1' AND DH.ApprovalStageFLG > 0) OR (@ChkMisyonin  <> '1' AND DH.ApprovalStageFLG >= 9))  
     ORDER BY  DH.StoreCD
              ,DH.OrderCD
              ,DH.OrderNO
     ;
END

GO
