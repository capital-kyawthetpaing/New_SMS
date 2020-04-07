 BEGIN TRY 
 Drop Procedure dbo.[D_Sales_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗納品書
--       Program ID      TempoNouhinsyo
--       Create date:    2019.07.26
--    ======================================================================

/****** Object:  StoredProcedure [D_Sales_SelectForPrint]    */
CREATE PROCEDURE [dbo].[D_Sales_SelectForPrint](
    -- Add the parameters for the stored procedure here
    @SalesNO varchar(11),
    @SalesDateFrom  varchar(10),
    @SalesDateTo  varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80),
    @PrintFLG TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF ISNULL( @SalesNO,'') = ''
    BEGIN
        SELECT DH.SalesNO
              ,DM.SalesRows
              ,PrintDate AS PrintDateTime
			  ,CEILING(ROW_NUMBER() OVER(PARTITION BY DH.SalesNo ORDER BY DM.SalesRows)/14.0) AS PageNumber
              ,(CASE WHEN PrintDate IS NULL THEN '' ELSE '＊' END) PrintFlg --＊：納品書再発行
              ,(SELECT TOP 1 A.ZipCD1 + '-' + A.ZipCD2 AS ZipCD 
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS ZipCD
              ,(SELECT TOP 1 A.Address1
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS Address1
              ,(SELECT TOP 1 A.Address2
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS Address2
                        
              ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
              ,DH.CustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) --AS CustomerName 
              + (CASE ISNULL(DH.CustomerName2,'') WHEN '' THEN '' ELSE CHAR(13) + DH.CustomerName2 END) AS CustomerName
              ,(CASE WHEN (SELECT top 1 A.AliasKBN FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) = 1 THEN '様' ELSE '御中' END) AliasKBN
              
              ,DH.SalesGaku
              ,DH.StaffCD
              ,(SELECT top 1 A.StaffName 
              FROM M_Staff A 
              WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS StaffName
              
              ,(SELECT top 1 A.Print1 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print6
              
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
              ,DM.IndividualClientName  + '-' + DM.CommentOutStore AS IndividualClientName   
              ,DM.SalesSu

              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.SalesUnitPrice + ISNULL(W.SalesUnitPrice,0) AS SalesUnitPrice
              ,DM.SalesGaku + ISNULL(W.SalesGaku,0) AS SalesGaku1

              ,DH.SalesGaku AS SalesGaku2
              ,DH.SalesTax10
              ,DH.SalesTax8
              
              ,(SELECT top 1 B.Print1
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
	              FROM M_Store A 
	              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
	              ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
	          ORDER BY B.ChangeDate desc)
              AS KouzaPrint1
              ,(SELECT top 1 B.Print2
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
	              FROM M_Store A 
	              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
	              ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
	          ORDER BY B.ChangeDate desc)
              AS KouzaPrint2
              ,(SELECT top 1 B.Print3
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint3
              ,(SELECT top 1 B.Print4
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint4
              
              ,MAX(DS.NouhinsyoComment) OVER(PARTITION BY DH.SalesNO) AS NouhinsyoComment
              ,(SELECT [Picture] FROM M_Image WHERE ID=1) AS Picture
              
              ,(SELECT MAX(MONTH(DATEADD(MM,-1,CONVERT(date,CONVERT(varchar,D.YYYYMM*100+1))))) 
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD)  AS Label1    --前月請求残
              ,(SELECT MAX(MONTH(CONVERT(date,CONVERT(varchar,D.YYYYMM*100+1)))) 
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD)  AS Label2    --当月請求残
              ,(SELECT top 1 D.LastBalanceGaku
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD
                ORDER BY D.YYYYMM desc) AS LastBalanceGaku             
              ,(SELECT top 1 D.BalanceGaku
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD
                ORDER BY D.YYYYMM desc) AS BalanceGaku 

            from D_Sales DH
            LEFT OUTER JOIN D_SalesDetails AS DM 
            ON DM.SalesNO = DH.SalesNO
            AND DM.NotPrintFLG=0	--＝１なら、その行は印刷しない
            LEFT OUTER JOIN D_StoreJuchuu AS DS
            ON DS.JuchuuNO = DM.JuchuuNO

            LEFT OUTER JOIN (
                SELECT DM.SalesNO, DM.SyukeiRows
                    , SUM(SalesGaku) AS SalesGaku
                    , SUM(SalesUnitPrice) AS SalesUnitPrice
                FROM (
                   SELECT DM.SalesNO
                        ,DM.AddSalesRows AS SyukeiRows
                        ,DM.SalesRows
                        ,DM.SalesUnitPrice 
                        ,DM.SalesGaku 
                    FROM D_SalesDetails AS DM 
                    WHERE DM.NotPrintFLG=1
                ) AS DM
                GROUP BY DM.SalesNO, DM.SyukeiRows
            )AS W
            ON W.SalesNO = DM.SalesNO
            AND W.SyukeiRows = DM.SalesRows
            
            WHERE DH.SalesDate >= (CASE WHEN @SalesDateFrom <> '' THEN CONVERT(DATE, @SalesDateFrom) ELSE DH.SalesDate END)
            AND DH.SalesDate <= (CASE WHEN @SalesDateTo <> '' THEN CONVERT(DATE, @SalesDateTo) ELSE DH.SalesDate END)
            AND DH.StoreCD = @StoreCD
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)

            AND DH.DeleteDateTime IS NULL
        ORDER BY DH.SalesNO, DM.SalesRows
        ;
    END
    ELSE
    BEGIN
        SELECT DH.SalesNO
              ,DM.SalesRows
              ,PrintDate AS PrintDateTime
			  ,CEILING(ROW_NUMBER() OVER(PARTITION BY DH.SalesNo ORDER BY DM.SalesRows)/14.0) AS PageNumber
              ,(CASE WHEN PrintDate IS NULL THEN '' ELSE '＊' END) PrintFlg --＊：納品書再発行
              ,(SELECT TOP 1 A.ZipCD1 + '-' + A.ZipCD2 AS ZipCD 
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS ZipCD
              ,(SELECT TOP 1 A.Address1
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS Address1
              ,(SELECT TOP 1 A.Address2
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS Address2
                        
              ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
              ,DH.CustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) --AS CustomerName 
              + (CASE ISNULL(DH.CustomerName2,'') WHEN '' THEN '' ELSE CHAR(13) + DH.CustomerName2 END) AS CustomerName
              ,(CASE WHEN (SELECT top 1 A.AliasKBN FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) = 1 THEN '様' ELSE '御中' END) AliasKBN
              
              ,DH.SalesGaku
              ,DH.StaffCD
              ,(SELECT top 1 A.StaffName 
              FROM M_Staff A 
              WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS StaffName
              
              ,(SELECT top 1 A.Print1 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
              ORDER BY A.ChangeDate desc) AS Print6
              
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SizeName ELSE A.SizeName END) AS SizeName
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SizeName
              ,(SELECT top 1  (CASE A.VariousFLG WHEN 1 THEN DM.ColorName ELSE A.ColorName END) AS ColorName
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
              ,DM.IndividualClientName  + '-' + DM.CommentOutStore AS IndividualClientName   
              ,DM.SalesSu

              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.SalesUnitPrice + ISNULL(W.SalesUnitPrice,0) AS SalesUnitPrice
              ,DM.SalesGaku + ISNULL(W.SalesGaku,0) AS SalesGaku1

              ,DH.SalesGaku AS SalesGaku2
              ,DH.SalesTax10
              ,DH.SalesTax8
              
              ,(SELECT top 1 B.Print1
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint1
              ,(SELECT top 1 B.Print2
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint2
              ,(SELECT top 1 B.Print3
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint3
              ,(SELECT top 1 B.Print4
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.SalesDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint4
              
              ,MAX(DS.NouhinsyoComment) OVER(PARTITION BY DH.SalesNO) AS NouhinsyoComment
              ,(SELECT [Picture] FROM M_Image WHERE ID=1) AS Picture

              ,(SELECT MAX(MONTH(DATEADD(MM,-1,CONVERT(date,CONVERT(varchar,D.YYYYMM*100+1))))) 
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD)  AS Label1    --前月請求残
              ,(SELECT MAX(MONTH(CONVERT(date,CONVERT(varchar,D.YYYYMM*100+1)))) 
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD)  AS Label2    --当月請求残
              ,(SELECT top 1 D.LastBalanceGaku
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD
                ORDER BY D.YYYYMM desc) AS LastBalanceGaku             
              ,(SELECT top 1 D.BalanceGaku
                FROM D_MonthlyClaims AS D 
                WHERE D.YYYYMM <= LEFT(CONVERT(varchar, GETDATE(), 112),6)
                AND  D.StoreCD = DH.StoreCD AND D.CustomerCD = DH.CustomerCD
                ORDER BY D.YYYYMM desc) AS BalanceGaku

            from D_Sales DH
            LEFT OUTER JOIN D_SalesDetails AS DM 
            ON DM.SalesNO = DH.SalesNO
            AND DM.NotPrintFLG=0	--＝１なら、その行は印刷しない
            LEFT OUTER JOIN D_StoreJuchuu AS DS
            ON DS.JuchuuNO = DM.JuchuuNO

            LEFT OUTER JOIN (
                SELECT DM.SalesNO, DM.SyukeiRows
                    , SUM(SalesGaku) AS SalesGaku
                    , SUM(SalesUnitPrice) AS SalesUnitPrice
                FROM (
                   SELECT DM.SalesNO
                        ,DM.AddSalesRows AS SyukeiRows
                        ,DM.SalesRows
                        ,DM.SalesUnitPrice 
                        ,DM.SalesGaku 
                    FROM D_SalesDetails AS DM 
                    WHERE DM.NotPrintFLG=1
                ) AS DM
                GROUP BY DM.SalesNO, DM.SyukeiRows
            )AS W
            ON W.SalesNO = DM.SalesNO
            AND W.SyukeiRows = DM.SalesRows
	        
            WHERE DH.DeleteDateTime IS NULL
            AND DH.SalesNO = @SalesNO
        ORDER BY DH.SalesNO, DM.SalesRows
        ;
    
    END
  
END


