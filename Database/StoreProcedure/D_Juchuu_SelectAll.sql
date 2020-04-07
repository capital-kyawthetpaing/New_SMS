 BEGIN TRY 
 Drop Procedure dbo.[D_Juchuu_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Juchuu_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Juchuu_SelectAll](
    -- Add the parameters for the stored procedure here
    @JuchuuDateFrom  varchar(10),
    @JuchuuDateTo  varchar(10),
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

    SELECT DH.JuchuuNO
          ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
          ,CONVERT(varchar,DH.InsertDateTime,111) AS InsertDateTime
          ,DH.CustomerCD
--          ,DH.CustomerName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
          FROM M_Customer A 
          WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.JuchuuDate
          ORDER BY A.ChangeDate desc) AS CustomerName 
          ,DH.CustomerName2
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.JuchuuDate
          ORDER BY A.ChangeDate desc) AS StaffName
          ,(SELECT top 1 A.StoreName 
          FROM M_Store	 A 
          WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.JuchuuDate
          ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.JuchuuGaku 
          
          ,(SELECT top 1 M.SKUName 
            FROM D_JuchuuDetails AS DM
            LEFT OUTER JOIN M_SKU AS M ON M.AdminNO = DM.AdminNO
            WHERE DM.JuchuuNO = DH.JuchuuNO 
             AND DM.DeleteDateTime IS NULL
             AND M.ChangeDate <= DH.JuchuuDate
             ORDER BY DM.JuchuuRows, M.ChangeDate desc) AS SKUName
          
    from D_Juchuu DH
        WHERE DH.JuchuuDate >= (CASE WHEN @JuchuuDateFrom <> '' THEN CONVERT(DATE, @JuchuuDateFrom) ELSE DH.JuchuuDate END)
        AND DH.JuchuuDate <= (CASE WHEN @JuchuuDateTo <> '' THEN CONVERT(DATE, @JuchuuDateTo) ELSE DH.JuchuuDate END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
        
        AND DH.DeleteDateTime IS NULL

    ORDER BY DH.JuchuuNO
    ;

  
END


