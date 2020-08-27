/****** Object:  StoredProcedure [dbo].[D_Juchu_SelectDataForTempoUriage]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Juchu_SelectDataForTempoUriage]
GO

/****** Object:  StoredProcedure [dbo].[D_Juchu_SelectDataForTempoUriage]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Juchu_SelectDataForTempoUriage]    */
CREATE PROCEDURE D_Juchu_SelectDataForTempoUriage(
    -- Add the parameters for the stored procedure here
    @OperateMode    tinyint,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
    @DateFrom  varchar(10),
    @DateTo  varchar(10),
    
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80),
    @KanaName varchar(30) ,
    @StoreCD  varchar(4),
    @StaffCD varchar(30) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    --TempoUriageNyuuryoku_ìXï‹îÑè„ì¸óÕ
    IF @OperateMode = 1
    BEGIN
        --âÊñ ì]ëóï\01
        SELECT DH.JuchuuNO
              , DM.JuchuuRows
              ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
              ,DH.CustomerCD
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
                  FROM M_Customer A 
                  WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.JuchuuDate
                  ORDER BY A.ChangeDate desc) AS CustomerName 
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName2 ELSE A.KanaName END)
                  FROM M_Customer A 
                  WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.JuchuuDate
                  ORDER BY A.ChangeDate desc) AS CustomerName2

              ,DM.JanCD
              ,DM.SKUName
              ,DM.ColorName
              ,DM.SizeName
              /*
                ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.ColorName ELSE DM.ColorName END) AS ColorSizeName
                   FROM M_SKU AS M
                    WHERE M.AdminNO = DM.AdminNO
                     AND M.ChangeDate <= DH.JuchuuDate
                     AND M.DeleteFlg = 0 
                     ORDER BY M.ChangeDate desc) AS ColorName
                ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SizeName ELSE DM.SizeName END) AS ColorSizeName
                   FROM M_SKU AS M
                    WHERE M.AdminNO = DM.AdminNO
                     AND M.ChangeDate <= DH.JuchuuDate
                     AND M.DeleteFlg = 0 
                     ORDER BY M.ChangeDate desc) AS SizeName
                     */
              ,NULL AS SalesNO
              ,NULL As SalesRows
              ,NULL AS SalesDate
              ,DM.ColorName
              ,DM.SizeName
              ,DM.JuchuuSuu
              ,NULL AS SalesSu
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.JuchuuUnitPrice AS UnitPrice
              ,DM.JuchuuGaku AS Kingaku
              ,NULL AS BillingNO

        from D_Juchuu AS DH

        LEFT OUTER JOIN D_JuchuuDetails AS DM 
        ON  DM.JuchuuNO = DH.JuchuuNO
        AND DM.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_Reserve AS DR 
        ON DR.Number = DM.JuchuuNO  
        AND DR.NumberRows = DM.JuchuuRows
        AND DR.ReserveKBN = 1
        AND DR.DeleteDateTime IS Null
                         
        WHERE DH.JuchuuDate >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE DH.JuchuuDate END)
            AND DH.JuchuuDate <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE DH.JuchuuDate END)
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
            AND DH.StoreCD = @StoreCD
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND EXISTS (SELECT MC.CustomerCD
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= DH.JuchuuDate 
                AND MC.CustomerName LIKE '%' + (CASE WHEN @CustomerName <> '' THEN @CustomerName ELSE MC.CustomerName END) + '%'
                AND MC.KanaName LIKE '%' + (CASE WHEN @KanaName <> '' THEN @KanaName ELSE MC.KanaName END) + '%'
                AND MC.DeleteFlg = 0
                AND MC.CustomerCD = DH.CustomerCD)
            AND DH.DeleteDateTime IS NULL
            
            AND DM.JuchuuSuu = DR.ReserveSu     --HikiateSu
            AND DR.ShippingSu = 0

        ORDER BY DH.JuchuuNO, DM.JuchuuRows
        ;
    END
    ELSE
    BEGIN
        --âÊñ ì]ëóï\02
        SELECT NULL AS JuchuuNO
              ,NULL AS JuchuuRows
              ,NULL AS JuchuuDate
              ,DH.CustomerCD
              ,DH.CustomerName
              ,DH.CustomerName2

              ,DM.JanCD
              ,DM.SKUName
              ,DM.ColorName
              ,DM.SizeName
              /*
                ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.ColorName ELSE DM.ColorName END) AS ColorSizeName
                   FROM M_SKU AS M
                    WHERE M.AdminNO = DM.AdminNO
                     AND M.ChangeDate <= DH.JuchuuDate
                     AND M.DeleteFlg = 0 
                     ORDER BY M.ChangeDate desc) AS ColorName
                ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SizeName ELSE DM.SizeName END) AS ColorSizeName
                   FROM M_SKU AS M
                    WHERE M.AdminNO = DM.AdminNO
                     AND M.ChangeDate <= DH.JuchuuDate
                     AND M.DeleteFlg = 0 
                     ORDER BY M.ChangeDate desc) AS SizeName
                     */
              ,DH.SalesNO
              ,DM.SalesRows
              ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
              ,DM.ColorName
              ,DM.SizeName
              ,NULL AS JuchuuSuu
              ,DM.SalesSu
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.SalesUnitPrice AS UnitPrice
              ,DM.SalesGaku AS Kingaku
              ,DC.BillingNO

        from D_Sales AS DH

        LEFT OUTER JOIN D_SalesDetails AS DM 
        ON  DM.SalesNO = DH.SalesNO
        AND DM.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_CollectPlanDetails AS DCD 
        ON DCD.SalesNO = DM.SalesNO  
        AND DCD.SalesRows = DM.SalesRows
        AND DCD.DeleteDateTime IS Null
        
        LEFT OUTER JOIN D_CollectPlan AS DC
        ON DC.CollectPlanNO = DCD.CollectPlanNO  
        AND DC.DeleteDateTime IS Null
        
        LEFT OUTER JOIN D_BillingDetails AS DBD
        ON DBD.CollectPlanNO = DCD.CollectPlanNO  
        AND DBD.CollectPlanRows = DCD.CollectPlanRows  
        AND DBD.DeleteDateTime IS Null            
        
        LEFT OUTER JOIN D_Billing AS DB
        ON DB.BillingNO = DBD.BillingNO  
        AND DB.DeleteDateTime IS Null             
                    
        LEFT OUTER JOIN D_BillingControl AS DBC
        ON DBC.ProcessingNO = DB.ProcessingNO
        AND DBC.DeleteDateTime IS Null  
           
        WHERE DH.SalesDate >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE DH.SalesDate END)
            AND DH.SalesDate <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE DH.SalesDate END)
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
            AND DH.StoreCD = @StoreCD
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND EXISTS (SELECT MC.CustomerCD
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= DH.SalesDate 
                AND MC.CustomerName LIKE '%' + (CASE WHEN @CustomerName <> '' THEN @CustomerName ELSE MC.CustomerName END) + '%'
                AND MC.KanaName LIKE '%' + (CASE WHEN @KanaName <> '' THEN @KanaName ELSE MC.KanaName END) + '%'
                AND MC.DeleteFlg = 0
                AND MC.CustomerCD = DH.CustomerCD)
            AND DH.DeleteDateTime IS NULL
            
            AND DC.PaymentProgressKBN = 0
            AND ISNULL(DBC.ProcessingKBN,0) <> 3
            AND DH.SalesEntryKBN = 0	--2020.08.27 add

        ORDER BY DH.SalesNO, DM.SalesRows
        ;

    END
END

GO
