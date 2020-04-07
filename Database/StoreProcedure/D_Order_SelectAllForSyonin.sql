 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectAllForSyonin]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Order_SelectAllForSyonin]    */
CREATE PROCEDURE [dbo].[D_Order_SelectAllForSyonin](
    -- Add the parameters for the stored procedure here
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),
    @StoreCD  varchar(4),
    @ApprovalStageFLG int,
    @Misyonin tinyint,
    @SyoninZumi tinyint,
    @Kyakka tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
      
    SELECT DH.OrderNO
            ,(CASE DH.ApprovalStageFLG WHEN 9 THEN '承認済'
                                        WHEN 1 THEN '申請'
                                        WHEN 0 THEN '却下'
                                        ELSE '承認中' END) AS ApprovalStageFLG
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,CONVERT(varchar,DH.LastApprovalDate,111) AS LastApprovalDate
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.LastApprovalStaffCD AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS LastApprovalStaffName
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS StoreName
                    ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.InsertOperator AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS StaffName          
          ,(SELECT top 1 A.VendorName
	          FROM M_Vendor A 
	          WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
	          ORDER BY A.ChangeDate desc) AS VendorName 
          ,(SELECT top 1 A.TaxFractionKBN
	          FROM M_Vendor A 
	          WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
	          ORDER BY A.ChangeDate desc) AS TaxFractionKBN

        ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SKUName ELSE DM.ItemName END) AS SKUName
	        FROM M_SKU AS M
	        LEFT OUTER JOIN D_OrderDetails AS DM 
	        ON M.AdminNO = DM.AdminNO
	        WHERE M.ChangeDate <= DH.OrderDate
	         AND DM.OrderNO = DH.OrderNO 
	         AND DM.DeleteDateTime IS NULL
	         ORDER BY DM.DisplayRows, M.ChangeDate desc) AS SKUName
	    
    from D_Order AS DH

    WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
    AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
    AND DH.StoreCD = @StoreCD
    
    AND DH.DeleteDateTime IS NULL
    
    AND DH.ApprovalStageFLG <= 9	--承認必要な発注(10は承認不要）
    AND DH.ApprovalStageFLG < @ApprovalStageFLG
    AND DH.ApprovalStageFLG > 0
    AND 1 = @Misyonin
    
    UNION ALL 

    SELECT DH.OrderNO
            ,(CASE DH.ApprovalStageFLG WHEN 9 THEN '承認済'
                                        WHEN 1 THEN '申請'
                                        WHEN 0 THEN '却下'
                                        ELSE '承認中' END) AS ApprovalStageFLG
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,CONVERT(varchar,DH.LastApprovalDate,111) AS LastApprovalDate
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.LastApprovalStaffCD AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS LastApprovalStaffName
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS StoreName
                    ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.InsertOperator AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS StaffName          
          ,(SELECT top 1 A.VendorName
              FROM M_Vendor A 
              WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS VendorName 
          ,(SELECT top 1 A.TaxFractionKBN
              FROM M_Vendor A 
              WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS TaxFractionKBN

        ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SKUName ELSE DM.ItemName END) AS SKUName
            FROM M_SKU AS M
            LEFT OUTER JOIN D_OrderDetails AS DM 
            ON M.AdminNO = DM.AdminNO
            WHERE M.ChangeDate <= DH.OrderDate
             AND DM.OrderNO = DH.OrderNO 
             AND DM.DeleteDateTime IS NULL
             ORDER BY DM.DisplayRows, M.ChangeDate desc) AS SKUName
        
    from D_Order AS DH

    WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
    AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
    AND DH.StoreCD = @StoreCD
    
    AND DH.DeleteDateTime IS NULL
    
    AND DH.ApprovalStageFLG <= 9    --承認必要な発注(10は承認不要）
    AND DH.ApprovalStageFLG = @ApprovalStageFLG
    AND 1 = @SyoninZumi

    
    UNION ALL 

    SELECT DH.OrderNO
            ,(CASE DH.ApprovalStageFLG WHEN 9 THEN '承認済'
                                        WHEN 1 THEN '申請'
                                        WHEN 0 THEN '却下'
                                        ELSE '承認中' END) AS ApprovalStageFLG
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,CONVERT(varchar,DH.LastApprovalDate,111) AS LastApprovalDate
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.LastApprovalStaffCD AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS LastApprovalStaffName
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS StoreName
                    ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.InsertOperator AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS StaffName          
          ,(SELECT top 1 A.VendorName
              FROM M_Vendor A 
              WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS VendorName 
          ,(SELECT top 1 A.TaxFractionKBN
              FROM M_Vendor A 
              WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
              ORDER BY A.ChangeDate desc) AS TaxFractionKBN

        ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SKUName ELSE DM.ItemName END) AS SKUName
            FROM M_SKU AS M
            LEFT OUTER JOIN D_OrderDetails AS DM 
            ON M.AdminNO = DM.AdminNO
            WHERE M.ChangeDate <= DH.OrderDate
             AND DM.OrderNO = DH.OrderNO 
             AND DM.DeleteDateTime IS NULL
             ORDER BY DM.DisplayRows, M.ChangeDate desc) AS SKUName
        
    from D_Order AS DH

    WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
    AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
    AND DH.StoreCD = @StoreCD
    
    AND DH.DeleteDateTime IS NULL
    
    AND DH.ApprovalStageFLG <= 9    --承認必要な発注(10は承認不要）
    AND DH.ApprovalStageFLG = 0
    AND 1 = @Kyakka
    
    ORDER BY OrderDate, OrderNO
    ;

END


