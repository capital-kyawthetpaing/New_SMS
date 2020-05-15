IF OBJECT_ID ( 'D_EDIOrder_SelectAll', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_EDIOrder_SelectAll]
GO

/****** Object:  StoredProcedure [D_Order_SelectAll]    */
CREATE PROCEDURE D_EDIOrder_SelectAll(
    -- Add the parameters for the stored procedure here
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),
    @StoreCD  varchar(4),
    @VendorCD  varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT DH.EDIOrderNO           
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,DH.VendorCD
          ,(SELECT top 1 A.VendorName
              FROM M_Vendor A 
             WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
             ORDER BY A.ChangeDate desc) AS VendorName 
          ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
             WHERE M.ChangeDate <= DH.OrderDate
               AND M.AdminNO = DM.AdminNO
               AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
    FROM D_EDIOrder AS DH
    LEFT JOIN ( SELECT EDIOrderNO
                      ,MIN(EDIOrderRows) AS MinRow
                  FROM D_EDIOrderDetails
                 GROUP BY EDIOrderNO
               ) AS MM ON DH.EDIOrderNO = MM.EDIOrderNO
    LEFT OUTER JOIN D_EDIOrderDetails AS DM ON MM.EDIOrderNO = DM.EDIOrderNO AND MM.MinRow = DM.EDIOrderRows
    WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
      AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
      AND DH.StoreCD = @StoreCD
      AND DH.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DH.VendorCD END)
    ORDER BY DH.EDIOrderNO
    ;
END

GO
