 BEGIN TRY 
 Drop Procedure dbo.[D_Sales_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Sales_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Sales_SelectAll](
    -- Add the parameters for the stored procedure here
    @SalesDateFrom  varchar(10),
    @SalesDateTo  varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    SELECT DH.SalesNO
          ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
          ,CONVERT(varchar,DH.InsertDateTime,111) AS InsertDateTime
          ,DH.CustomerCD
--          ,DH.CustomerName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
          FROM M_Customer A 
          WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.SalesDate
          ORDER BY A.ChangeDate desc) AS CustomerName 
          ,DH.CustomerName2
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.SalesDate
          ORDER BY A.ChangeDate desc) AS StaffName
          ,(SELECT top 1 A.StoreName 
          FROM M_Store	 A 
          WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
          ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.SalesGaku 
          
          ,(SELECT top 1 M.SKUName 
            FROM D_SalesDetails AS DM
            LEFT OUTER JOIN M_SKU AS M ON M.AdminNO = DM.AdminNO
            WHERE DM.SalesNO = DH.SalesNO 
             AND DM.DeleteDateTime IS NULL
             AND M.ChangeDate <= DH.SalesDate
             ORDER BY DM.SalesRows, M.ChangeDate desc) AS SKUName
          
    from D_Sales DH
        WHERE DH.SalesDate >= (CASE WHEN @SalesDateFrom <> '' THEN CONVERT(DATE, @SalesDateFrom) ELSE DH.SalesDate END)
        AND DH.SalesDate <= (CASE WHEN @SalesDateTo <> '' THEN CONVERT(DATE, @SalesDateTo) ELSE DH.SalesDate END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
        
        AND DH.DeleteDateTime IS NULL

    ORDER BY DH.SalesNO
    ;

  
END


