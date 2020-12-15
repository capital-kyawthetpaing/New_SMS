 BEGIN TRY 
 Drop Procedure dbo.[SelectDataForNyukin]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [SelectDataForNyukin]    */
CREATE PROCEDURE SelectDataForNyukin(
    -- Add the parameters for the stored procedure here
    @RdoSyubetsu tinyint,
    @WebCollectType varchar(3),
    @DateFrom       varchar(10),
    @DateTo         varchar(10),
    @StoreCD        varchar(4),
    @CustomerCD     varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @RdoSyubetsu = 2
    BEGIN
        --NyuukinNyuuryoku_“ü‹à“ü—Í@‡A–¾×’PˆÊ@b“ü‹àŒÚ‹q
        --‰æ–Ê“]‘—•\05 V‹K‚Å•\¦ƒ{ƒ^ƒ“‰Ÿ‰º
        SELECT CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate
              ,DB.BillingNO
              ,CONVERT(varchar,DS.SalesDate,111) As SalesDate
              ,DCM.SalesNO
              ,DCM.JuchuuNO
              ,DB.StoreCD             --“X•Ü
              ,(SELECT top 1 A.StoreName
                FROM M_Store A 
                WHERE A.StoreCD = DB.StoreCD 
                AND A.ChangeDate <= DB.BillingCloseDate
                AND A.DeleteFlg = 0
                ORDER BY A.ChangeDate desc) AS DetailStoreName

              ,DB.BillingCustomerCD
              ,(SELECT TOP 1 A.CustomerName
                FROM M_Customer AS A
                WHERE A.CustomerCD = DB.BillingCustomerCD 
                AND A.ChangeDate <= DB.BillingCloseDate
                AND A.DeleteFlg = 0
                ORDER BY A.ChangeDate DESC) AS BillingCustomerName
              ,SUM(ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0)) OVER() AS SumConfirmAmount 
              
              ,ISNULL(DBM.BillingGaku,0) AS BillingGaku                                    --¿‹Šz
              ,ISNULL(DCBD.CollectAmount,0) AS D_CollectAmount                             --“ü‹àÏŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS ConfirmAmount    --¡‰ñ“ü‹àŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS NowCollectAmount --¡‰ñ“ü‹àŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS Minyukin         --–¢“ü‹àŠz                                                               --–¢“ü‹àŠz(–¾×‚Ì¿‹Šz|“ü‹àÏŠz|¡‰ñ“ü‹àŠz)
              ,ISNULL(DSM.SKUCD,DJM.SKUCD) AS SKUCD
              ,ISNULL(DSM.SKUName,DJM.SKUName) AS SKUName
              ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) AS CommentInStore
              ,DCM.CollectPlanNO
              ,DCM.CollectPlanRows
              ,DC.FirstCollectPlanDate
              ,DC.NextCollectPlanDate
              
        FROM D_Billing AS DB

        LEFT OUTER JOIN D_BillingDetails AS DBM
        ON DBM.BillingNO = DB.BillingNO
        AND DBM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_CollectPlanDetails AS DCM
        ON DCM.CollectPlanNO = DBM.CollectPlanNO
        AND DCM.CollectPlanRows = DBM.CollectPlanRows
        AND DCM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_CollectPlan AS DC
        ON DC.CollectPlanNO = DCM.CollectPlanNO
        AND DC.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_SalesDetails AS DSM
        ON DSM.SalesNO = DCM.SalesNO
        AND DSM.SalesRows = DCM.SalesRows
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DCM.JuchuuNO
        AND DJM.JuchuuRows = DCM.JuchuuRows
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Sales AS DS
        ON DS.SalesNO = DCM.SalesNO
        AND DS.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN (SELECT SUM(DCBD.CollectAmount) AS CollectAmount
                              , DCBD.CollectPlanNO
                              , DCBD.CollectPlanRows
                         FROM D_CollectBillingDetails AS DCBD
                         INNER JOIN D_CollectBilling AS DCB
                         ON DCB.CollectPlanNO = DCBD.CollectPlanNO
                         AND DCB.ConfirmNO = DCBD.ConfirmNO
                         AND DCB.DeleteDateTime IS NULL
            
                         WHERE DCBD.DeleteDateTime IS NULL
                         GROUP BY DCBD.CollectPlanNO, DCBD.CollectPlanRows
        ) AS DCBD
        ON DCBD.CollectPlanNO = DCM.CollectPlanNO
        AND DCBD.CollectPlanRows = DCM.CollectPlanRows

        WHERE DB.DeleteDateTime IS NULL
        AND DB.BillingConfirmFlg = 1
        AND DB.BillingType = 2
        
        AND DB.BillingCustomerCD = @CustomerCD
        AND DB.StoreCD = @StoreCD
                            
        --“–‰‰ñû—\’è“ú,Ÿ‰ñû—\’è“ú,¿‹“ú,¿‹”Ô†   
        ORDER BY DC.FirstCollectPlanDate,DC.NextCollectPlanDate,DB.BillingCloseDate,DB.BillingNO
        ;
    END
    ELSE
    BEGIN
    	--‰æ–Ê“]‘—•\02
        --æí•Ê
        SELECT CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate
              ,DB.BillingNO
              ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate
              ,DCM.SalesNO
              ,DCM.JuchuuNO
              ,DWM.StoreCD             --“X•Ü
              ,(SELECT top 1 A.StoreName
                FROM M_Store A 
                WHERE A.StoreCD = DWM.StoreCD 
                AND A.ChangeDate <= DWM.WebCollectDate
                AND A.DeleteFlg = 0
                ORDER BY A.ChangeDate desc) AS DetailStoreName

              ,DB.BillingCustomerCD
              ,(SELECT TOP 1 A.CustomerName
                FROM M_Customer AS A
                WHERE A.CustomerCD = DB.BillingCustomerCD 
                AND A.ChangeDate <= DB.BillingCloseDate
                AND A.DeleteFlg = 0
                ORDER BY A.ChangeDate DESC) AS BillingCustomerName
              ,ISNULL(DBM.BillingGaku,0) AS BillingGaku                                    --¿‹Šz
              ,ISNULL(DCBD.CollectAmount,0) AS D_CollectAmount                             --“ü‹àÏŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS ConfirmAmount    --¡‰ñ“ü‹àŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS NowCollectAmount --¡‰ñ“ü‹àŠz
              ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS Minyukin         --–¢“ü‹àŠz
              ,ISNULL(DSM.SKUCD,DJM.SKUCD) AS SKUCD
              ,ISNULL(DSM.SKUName,DJM.SKUName) AS SKUName
              ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) AS CommentInStore
              ,DCM.CollectPlanNO
              ,DCM.CollectPlanRows
              ,DC.FirstCollectPlanDate
              ,DC.NextCollectPlanDate
              
              ,DW.ImportAmount
              ,DW.WebCollectNO
              ,DW.WebCollectType

        FROM D_WebCollect AS DW
        LEFT OUTER JOIN D_WebCollectDetails AS DWM
        ON DWM.WebCollectNO = DW.WebCollectNO
        AND DWM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_CollectPlanDetails AS DCM
        ON DCM.CollectPlanNO = DWM.CollectPlanNO
        AND DCM.CollectPlanRows = DWM.CollectPlanRows
        AND DCM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_CollectPlan AS DC
        ON DC.CollectPlanNO = DCM.CollectPlanNO
        AND DC.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_BillingDetails AS DBM
        ON DBM.CollectPlanNO = DCM.CollectPlanNO
        AND DBM.CollectPlanRows = DCM.CollectPlanRows
        AND DBM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Billing AS DB
        ON DB.BillingNO = DBM.BillingNO
        AND DB.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_SalesDetails AS DSM
        ON DSM.SalesNO = DCM.SalesNO
        AND DSM.SalesRows = DCM.SalesRows
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DCM.JuchuuNO
        AND DJM.JuchuuRows = DCM.JuchuuRows
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Sales AS DS
        ON DS.SalesNO = DCM.SalesNO
        AND DS.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN (SELECT SUM(DCBD.CollectAmount) AS CollectAmount
                              , DCBD.CollectPlanNO
                              , DCBD.CollectPlanRows
                         FROM D_CollectBillingDetails AS DCBD
                         INNER JOIN D_CollectBilling AS DCB
                         ON DCB.CollectPlanNO = DCBD.CollectPlanNO
                         AND DCB.ConfirmNO = DCBD.ConfirmNO
                         AND DCB.DeleteDateTime IS NULL
                         
                         WHERE DCBD.DeleteDateTime IS NULL
                         GROUP BY DCBD.CollectPlanNO, DCBD.CollectPlanRows
        ) AS DCBD
        ON DCBD.CollectPlanNO = DCM.CollectPlanNO
        AND DCBD.CollectPlanRows = DCM.CollectPlanRows

        WHERE DB.BillingConfirmFlg = 1
        AND DB.BillingType = 2
        AND DW.WebCollectType = (CASE WHEN @WebCollectType <> '' THEN @WebCollectType ELSE DW.WebCollectType END)

        AND CONVERT(DATE,DW.ImportDate) >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE CONVERT(DATE,DW.ImportDate) END)
        AND CONVERT(DATE,DW.ImportDate) <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE CONVERT(DATE,DW.ImportDate) END)
                    
        AND DWM.StoreCD = @StoreCD
        
        --“–‰‰ñû—\’è“ú,Ÿ‰ñû—\’è“ú,¿‹“ú,¿‹”Ô†   
        ORDER BY DC.FirstCollectPlanDate,DC.NextCollectPlanDate,DB.BillingCloseDate,DB.BillingNO
        ;
        
    END
END

GO
