 BEGIN TRY 
 Drop Procedure dbo.[D_Mitsumori_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Mitsumori_SelectForPrint]    */
CREATE PROCEDURE [dbo].[D_Mitsumori_SelectForPrint](
    -- Add the parameters for the stored procedure here
    @MitsumoriNO varchar(11),
    @MitsumoriDateFrom  varchar(10),
    @MitsumoriDateTo  varchar(10),
    @MitsumoriInputDateFrom varchar(10),
    @MitsumoriInputDateTo varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80),
    @MitsumoriName  varchar(100),
    @PrintFLG TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    IF ISNULL( @MitsumoriNO,'') = ''
    BEGIN
        SELECT DH.MitsumoriNO
              ,DM.DisplayRows
              ,PrintDateTime
              ,(CASE WHEN PrintDateTime IS NULL THEN '' ELSE '＊' END) PrintFlg --＊：見積書再発行
              ,DH.ZipCD1 + '-' + DH.ZipCD2 AS ZipCD
              ,DH.Address1
              ,DH.Address2
              ,CONVERT(varchar,DH.MitsumoriDate,111) AS MitsumoriDate
              ,DH.CustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) --AS CustomerName 
              + (CASE ISNULL(DH.CustomerName2,'') WHEN '' THEN '' ELSE CHAR(13) + DH.CustomerName2 END) AS CustomerName
              ,(CASE WHEN AliasKBN = 1 THEN '様' ELSE '御中' END) AliasKBN
              
              ,DH.MitsumoriName
              ,DH.StaffCD
              ,(SELECT top 1 A.StaffName 
              FROM M_Staff A 
              WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS StaffName
              
              ,(SELECT top 1 A.Print1 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) AS Print6
              
              ,DH.DeliveryDate
              ,DH.PaymentTerms
              ,DH.DeliveryPlace
              ,DH.ValidityPeriod
              ,DH.MitsumoriGaku 
              ,DH.MitsumoriTax8 + MitsumoriTax10 AS Zei
              
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.MitsumoriDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
              ,DM.IndividualClientName  
              ,DM.CommentOutStore   
              ,DM.MitsumoriSuu
              
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.MitsumoriUnitPrice + ISNULL(W.MitsumoriUnitPrice,0) AS MitsumoriUnitPrice
              ,DM.MitsumoriGaku + ISNULL(W.MitsumoriGaku,0) AS MitsumoriGaku1

              ,DH.MitsumoriGaku AS MitsumoriGaku2
              ,DH.MitsumoriTax10
              ,DH.MitsumoriTax8
              ,DH.RemarksOutStore
              
        from D_Mitsumori DH
        LEFT OUTER JOIN D_MitsumoriDetails AS DM 
        ON DM.MitsumoriNO = DH.MitsumoriNO
        AND DM.NotPrintFLG=0	--＝１なら、その行は印刷しない

        LEFT OUTER JOIN (
            SELECT DM.MitsumoriNO, DM.SyukeiRows
            	, SUM(MitsumoriGaku) AS MitsumoriGaku
            	, SUM(MitsumoriUnitPrice) AS MitsumoriUnitPrice
                FROM (
                    SELECT DM.MitsumoriNO
                        ,MAX(DMM.MitsumoriRows) AS SyukeiRows
                        ,DM.MitsumoriRows
                        ,MAX(DM.MitsumoriUnitPrice) AS MitsumoriUnitPrice
                        ,MAX(DM.MitsumoriGaku) AS MitsumoriGaku
                    FROM D_MitsumoriDetails AS DM 
                    INNER JOIN D_MitsumoriDetails AS DMM 
                    ON DMM.MitsumoriNO = DM.MitsumoriNO
                    WHERE DM.NotPrintFLG=1
                    AND DMM.NotPrintFLG=0
                    AND DMM.MitsumoriRows < DM.MitsumoriRows
                    GROUP BY DM.MitsumoriNO,DM.MitsumoriRows
        ) AS DM
        GROUP BY DM.MitsumoriNO, DM.SyukeiRows
        )AS W
        ON W.MitsumoriNO = DM.MitsumoriNO
        AND W.SyukeiRows = DM.MitsumoriRows
        
            WHERE DH.MitsumoriDate >= (CASE WHEN @MitsumoriDateFrom <> '' THEN CONVERT(DATE, @MitsumoriDateFrom) ELSE DH.MitsumoriDate END)
            AND DH.MitsumoriDate <= (CASE WHEN @MitsumoriDateTo <> '' THEN CONVERT(DATE, @MitsumoriDateTo) ELSE DH.MitsumoriDate END)
            AND DH.InsertDateTime >= (CASE WHEN @MitsumoriInputDateFrom <> '' THEN CONVERT(DATE, @MitsumoriInputDateFrom) ELSE DH.InsertDateTime END)
            AND DH.InsertDateTime <= (CASE WHEN @MitsumoriInputDateTo <> '' THEN CONVERT(DATE, @MitsumoriInputDateTo) ELSE DH.InsertDateTime END)
            AND DH.StoreCD = @StoreCD
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
            AND DH.MitsumoriName LIKE '%' + CASE WHEN @MitsumoriName <> '' THEN @MitsumoriName ELSE DH.MitsumoriName END + '%'

            AND DH.DeleteDateTime IS NULL
        ORDER BY DH.MitsumoriNO, DM.DisplayRows
        ;
    END
    ELSE
    BEGIN
		SELECT DH.MitsumoriNO
              ,DM.DisplayRows
              ,PrintDateTime
              ,(CASE WHEN PrintDateTime IS NULL THEN '' ELSE '＊' END) PrintFlg --＊：見積書再発行
              ,DH.ZipCD1 + '-' + DH.ZipCD2 AS ZipCD
              ,DH.Address1
              ,DH.Address2
              ,CONVERT(varchar,DH.MitsumoriDate,111) AS MitsumoriDate
              ,DH.CustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MitsumoriDate
              ORDER BY A.ChangeDate desc) --AS CustomerName 
              + (CASE ISNULL(DH.CustomerName2,'') WHEN '' THEN '' ELSE CHAR(13) + DH.CustomerName2 END) AS CustomerName
              ,(CASE WHEN AliasKBN = 1 THEN '様' ELSE '御中' END) AliasKBN
              
              ,DH.MitsumoriName
              ,DH.StaffCD
              ,(SELECT top 1 A.StaffName 
                  FROM M_Staff A 
                  WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS StaffName
              
              ,(SELECT top 1 A.Print1 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
                  ORDER BY A.ChangeDate desc) AS Print6
              
              ,DH.DeliveryDate
              ,DH.PaymentTerms
              ,DH.DeliveryPlace
              ,DH.ValidityPeriod
              ,DH.MitsumoriGaku 
              ,DH.MitsumoriTax8 + MitsumoriTax10 AS Zei
              
              ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
                  FROM M_SKU A 
                  WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.MitsumoriDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc) AS SKUName
              ,DM.IndividualClientName  
              ,DM.CommentOutStore   
              ,DM.MitsumoriSuu
              
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DM.TaniCD) AS TaniName
              ,DM.MitsumoriUnitPrice + ISNULL(W.MitsumoriUnitPrice,0) AS MitsumoriUnitPrice
              ,DM.MitsumoriGaku + ISNULL(W.MitsumoriGaku,0) AS MitsumoriGaku1

              ,DH.MitsumoriGaku AS MitsumoriGaku2
              ,DH.MitsumoriTax10
              ,DH.MitsumoriTax8
              ,DH.RemarksOutStore
              
        from D_Mitsumori DH
        LEFT OUTER JOIN D_MitsumoriDetails AS DM 
        ON DM.MitsumoriNO = DH.MitsumoriNO
        AND DM.NotPrintFLG=0	--＝１なら、その行は印刷しない

        LEFT OUTER JOIN (
            SELECT DM.MitsumoriNO, DM.SyukeiRows
            	, SUM(MitsumoriUnitPrice) AS MitsumoriUnitPrice
            	, SUM(MitsumoriGaku) AS MitsumoriGaku
                FROM (
                    SELECT DM.MitsumoriNO
                        ,MAX(DMM.MitsumoriRows) AS SyukeiRows
                        ,DM.MitsumoriRows
                        ,MAX(DM.MitsumoriUnitPrice) AS MitsumoriUnitPrice
                        ,MAX(DM.MitsumoriGaku) AS MitsumoriGaku
                    FROM D_MitsumoriDetails AS DM 
                    INNER JOIN D_MitsumoriDetails AS DMM 
                    ON DMM.MitsumoriNO = DM.MitsumoriNO
                    WHERE DM.NotPrintFLG=1
                    AND DMM.NotPrintFLG=0
                    AND DMM.MitsumoriRows < DM.MitsumoriRows
                    GROUP BY DM.MitsumoriNO,DM.MitsumoriRows
	        ) AS DM
	        GROUP BY DM.MitsumoriNO, DM.SyukeiRows
        )AS W
        ON W.MitsumoriNO = DM.MitsumoriNO
        AND W.SyukeiRows = DM.MitsumoriRows
        
        WHERE DH.DeleteDateTime IS NULL
        AND DH.MitsumoriNO = @MitsumoriNO
        ORDER BY DH.MitsumoriNO, DM.DisplayRows
        ;
    
    END
  
END


