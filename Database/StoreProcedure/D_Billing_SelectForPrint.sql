 BEGIN TRY 
 Drop Procedure dbo.[D_Billing_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�ܐ�����
--       Program ID      SeikyuuSho
--       Create date:    2019.08.10
--    ======================================================================

/****** Object:  StoredProcedure [D_Billing_SelectForPrint]    */
CREATE PROCEDURE D_Billing_SelectForPrint(
    -- Add the parameters for the stored procedure here
    @BillingCloseDate  varchar(10),
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

    IF @PrintFLG = 1    --�S��
        -- Insert statements for procedure here
        SELECT DH.BillingNO AS BillingNO
              ,DM.BillingRows
           --   ,CEILING(ROW_NUMBER() OVER(PARTITION BY DH.BillingNO ORDER BY DM.BillingRows)/14.0) AS PageNumber
              ,DH.PrintDateTime
              ,(CASE WHEN DH.PrintDateTime IS NULL THEN '' ELSE '��' END) PrintFlg --���F�������Ĕ��s
              ,(SELECT TOP 1 A.ZipCD1 + '-' + A.ZipCD2 AS ZipCD 
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS ZipCD
              ,(SELECT TOP 1 A.Address1
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS Address1
              ,(SELECT TOP 1 A.Address2
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS Address2
                        
              --,CONVERT(varchar,DH.BillingCloseDate,111) AS BillingCloseDate	--9999�N99��99����
              ,left(STUFF(STUFF(convert(varchar,DH.BillingCloseDate,120)+'����',8,1,'��'),5,1,'�N'),12) AS BillingCloseDate
              ,DH.BillingCustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 A.LongName1 + (CASE ISNULL(A.LongName2,'') WHEN '' THEN '' ELSE CHAR(13) + A.LongName2 END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CustomerName 
                        
              ,(CASE WHEN (SELECT top 1 A.AliasKBN FROM M_Customer A 
              WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) = 1 THEN '�l' ELSE '�䒆' END) AliasKBN
              
              ,(SELECT top 1 A.Print1 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print6
              
              ,DH.LastBillingGaku   --�O�񂲐����z
              ,DH.LastCollectGaku   --�������z
              ,DH.LastBillingGaku-DH.LastCollectGaku AS KurikoshiGaku   --�J�z���z
              ,DH.BillingGaku   --���񂨔���z
              ,DH.TotalBillingHontaiGaku10  --�ŗ��i10%�j�{�̊z
              ,DH.TotalBillingTax10 --�ŗ��i10%�j����Ŋz
              ,DH.TotalBillingHontaiGaku8   --�ŗ��i8%�j�{�̊z
              ,DH.TotalBillingTax8  --�ŗ��i8%�j����Ŋz
              ,DH.LastBillingGaku-DH.LastCollectGaku+DH.BillingGaku AS SeikyuGaku--���񂲐����z=�J�z���z�{���񂨔���z
              --,ISNULL((SELECT TOP 1 '�o�^�ԍ� '+A.RegisteredNumber
              --          FROM M_Customer AS A
              --          WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
              --          AND A.DeleteFlg = 0
              --          ORDER BY A.ChangeDate DESC)
              ,(SELECT '�o�^�ԍ� '+ M.RegisteredNumber FROM M_Control AS M WHERE M.MainKey =1) AS RegisteredNumber
              
              ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate
              ,DS.SalesNO
              ,(SELECT top 1 A.CustomerName FROM M_Customer A 
              WHERE A.CustomerCD = DS.CustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) CustomerName_Sales    --�[�i��
              ,DSM.SKUName
              ,DSM.SalesSU
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DSM.TaniCD) AS TaniName
              ,DSM.SalesUnitPrice + ISNULL(W.SalesUnitPrice,0) AS SalesUnitPrice
              ,DSM.SalesGaku + ISNULL(W.SalesGaku,0) AS SalesGaku
              ,(CASE WHEN DSM.SalesTaxRitsu = 8 THEN '*' ELSE '' END) AS SalesTaxRitsu

              ,(SELECT top 1 A.Remarks 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Remarks
              
              ,(SELECT top 1 B.Print1
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint1
              ,(SELECT top 1 B.Print2
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint2
              ,(SELECT top 1 B.Print3
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint3
              ,(SELECT top 1 B.Print4
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint4
              
              ,(SELECT [Picture] FROM M_Image WHERE ID=1) AS Picture

            from D_Billing DH
            LEFT OUTER JOIN D_BillingDetails AS DM 
            ON DM.BillingNO = DH.BillingNO
            AND DM.DeleteDateTime IS NULL
            AND DM.InvoiceFLG = 0
            LEFT OUTER JOIN D_Sales AS DS
            ON DS.SalesNO = DM.SalesNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_SalesDetails AS DSM
            ON DSM.SalesNO = DM.SalesNO
            AND DSM.SalesRows = DM.SalesRows
            AND DSM.DeleteDateTime IS NULL

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
            ON W.SalesNO = DSM.SalesNO
            AND W.SyukeiRows = DSM.SalesRows

            WHERE DH.BillingCloseDate = (CASE WHEN @BillingCloseDate <> '' THEN CONVERT(DATE, @BillingCloseDate) ELSE DH.BillingCloseDate END)
            AND DH.StoreCD = @StoreCD
            AND EXISTS(SELECT 1 FROM M_Customer AS M
                       WHERE M.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE M.StaffCD END)
                       AND M.CustomerCD = DH.BillingCustomerCD AND M.ChangeDate <= DH.BillingCloseDate
                       AND M.DeleteFlg = 0
                       )
            AND DH.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.BillingCustomerCD END)

            AND DH.DeleteDateTime IS NULL
            
            AND DH.BillingType = 2	--2019.10.26 add
            AND DSM.BillingPrintFLG = 0
            AND DSM.NotPrintFLG=0	--���P�Ȃ�A���̍s�͈�����Ȃ�

        ORDER BY DH.BillingCustomerCD, DH.BillingNO, DS.SalesDate, DS.SalesNO, DSM.SalesRows
        ;
    ELSE    --�����
        SELECT DH.BillingNO AS BillingNO
              ,DM.BillingRows
           --   ,CEILING(ROW_NUMBER() OVER(PARTITION BY DH.BillingNO ORDER BY DM.BillingRows)/14.0) AS PageNumber
              ,DH.PrintDateTime
              ,(CASE WHEN DH.PrintDateTime IS NULL THEN '' ELSE '��' END) PrintFlg --���F�������Ĕ��s
              ,(SELECT TOP 1 A.ZipCD1 + '-' + A.ZipCD2 AS ZipCD 
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS ZipCD
              ,(SELECT TOP 1 A.Address1
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS Address1
              ,(SELECT TOP 1 A.Address2
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS Address2
                        
              ,left(STUFF(STUFF(convert(varchar,DH.BillingCloseDate,120)+'����',8,1,'��'),5,1,'�N'),12) AS BillingCloseDate
              ,DH.BillingCustomerCD
    --          ,DH.CustomerName
              ,(SELECT top 1 A.LongName1 + (CASE ISNULL(A.LongName2,'') WHEN '' THEN '' ELSE CHAR(13) + A.LongName2 END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CustomerName 
                        
              ,(CASE WHEN (SELECT top 1 A.AliasKBN FROM M_Customer A 
              WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) = 1 THEN '�l' ELSE '�䒆' END) AliasKBN
              
              ,(SELECT top 1 A.Print1 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print1
              ,(SELECT top 1 A.Print2 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print2
              ,(SELECT top 1 A.Print3 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print3
              ,(SELECT top 1 A.Print4 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print4
              ,(SELECT top 1 A.Print5 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print5
              ,(SELECT top 1 A.Print6 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Print6
              
              ,DH.LastBillingGaku   --�O�񂲐����z
              ,DH.LastCollectGaku   --�������z
              ,DH.LastBillingGaku-DH.LastCollectGaku AS KurikoshiGaku   --�J�z���z
              ,DH.BillingGaku   --���񂨔���z
              ,DH.TotalBillingHontaiGaku10  --�ŗ��i10%�j�{�̊z
              ,DH.TotalBillingTax10 --�ŗ��i10%�j����Ŋz
              ,DH.TotalBillingHontaiGaku8   --�ŗ��i8%�j�{�̊z
              ,DH.TotalBillingTax8  --�ŗ��i8%�j����Ŋz
              ,DH.LastBillingGaku-DH.LastCollectGaku+DH.BillingGaku AS SeikyuGaku--���񂲐����z=�J�z���z�{���񂨔���z
              ,(SELECT TOP 1 '�o�^�ԍ� '+A.RegisteredNumber
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.BillingCustomerCD AND A.ChangeDate <= DH.BillingCloseDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS RegisteredNumber
              
              ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate
              ,DS.SalesNO
              ,(SELECT top 1 A.CustomerName FROM M_Customer A 
              WHERE A.CustomerCD = DS.CustomerCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) CustomerName_Sales    --�[�i��
              ,DSM.SKUName
              ,DSM.SalesSU
              ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='201' AND A.[Key] = DSM.TaniCD) AS TaniName
              ,DSM.SalesUnitPrice + ISNULL(W.SalesUnitPrice,0) AS SalesUnitPrice
              ,DSM.SalesGaku + ISNULL(W.SalesGaku,0) AS SalesGaku
              ,(CASE WHEN DSM.SalesTaxRitsu = 8 THEN '*' ELSE '' END) AS SalesTaxRitsu

              ,(SELECT top 1 A.Remarks 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS Remarks
              
              ,(SELECT top 1 B.Print1
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint1
              ,(SELECT top 1 B.Print2
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint2
              ,(SELECT top 1 B.Print3
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint3
              ,(SELECT top 1 B.Print4
              FROM M_Kouza AS B 
              WHERE B.KouzaCD = (SELECT top 1 A.KouzaCD 
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.BillingCloseDate
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc)
              AND B.ChangeDate <= DH.BillingCloseDate
              ORDER BY B.ChangeDate desc)
              AS KouzaPrint4
              
              ,(SELECT [Picture] FROM M_Image WHERE ID=1) AS Picture

            from D_Billing DH
            LEFT OUTER JOIN D_BillingDetails AS DM 
            ON DM.BillingNO = DH.BillingNO
            AND DM.DeleteDateTime IS NULL
            AND DM.InvoiceFLG = 0
            LEFT OUTER JOIN D_Sales AS DS
            ON DS.SalesNO = DM.SalesNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_SalesDetails AS DSM
            ON DSM.SalesNO = DM.SalesNO
            AND DSM.SalesRows = DM.SalesRows
            AND DSM.DeleteDateTime IS NULL

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
            ON W.SalesNO = DSM.SalesNO
            AND W.SyukeiRows = DSM.SalesRows
            
            WHERE DH.BillingCloseDate = (CASE WHEN @BillingCloseDate <> '' THEN CONVERT(DATE, @BillingCloseDate) ELSE DH.BillingCloseDate END)
            AND DH.StoreCD = @StoreCD
            AND EXISTS(SELECT 1 FROM M_Customer AS M
                       WHERE M.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE M.StaffCD END)
                       AND M.CustomerCD = DH.BillingCustomerCD AND M.ChangeDate <= DH.BillingCloseDate
                       AND M.DeleteFlg = 0
                       )
            AND DH.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.BillingCustomerCD END)

            AND DH.DeleteDateTime IS NULL
            
            AND DH.BillingType = 2	--2019.10.26 add
            AND DSM.BillingPrintFLG = 0
            AND DSM.NotPrintFLG=0	--���P�Ȃ�A���̍s�͈�����Ȃ�
            
            AND DH.PrintDateTime IS NULL	--��������̂Ƃ��݂̂̏����@

        ORDER BY DH.BillingCustomerCD, DH.BillingNO, DS.SalesDate, DS.SalesNO, DSM.SalesRows
        ;
  
END

GO
