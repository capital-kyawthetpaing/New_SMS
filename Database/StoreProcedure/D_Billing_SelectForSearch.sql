 BEGIN TRY 
 Drop Procedure dbo.[D_Billing_SelectForSearch]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Billing_SelectForSearch]    */
CREATE PROCEDURE D_Billing_SelectForSearch(
    -- Add the parameters for the stored procedure here
    @StoreCD       varchar(4),
    @CustomerName  varchar(80),
    
    @BillingGakuFrom  money,
    @BillingGakuTo    money,
    @CollectDateFrom  varchar(10),
    @CollectDateTo    varchar(10),
    @ChkMinyukin      tinyint,
    @ChkNyukinzumi    tinyint,
    @RdoDispKbn       tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --請求
    IF @RdoDispKbn = 1
    BEGIN
        SELECT MAX(W.StoreCD) AS StoreCD
              ,MAX(W.StoreName) AS StoreName
              ,W.CustomerCD
              ,MAX(W.CustomerName) AS CustomerName
              ,W.BillingCloseDate AS BillingDate
              ,W.BillingNO
              ,SUM(W.BillingGaku) AS BillingGaku
              ,MAX(W.CollectDate) AS CollectDate
        FROM (
            SELECT DB.StoreCD
                  ,(SELECT top 1 A.StoreName 
                    FROM M_Store A 
                    WHERE A.StoreCD = DB.StoreCD AND A.ChangeDate <= DB.BillingCloseDate
                    AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate desc) AS StoreName

                  ,DB.BillingCustomerCD AS CustomerCD
                  ,(SELECT top 1 A.CustomerName
                    FROM M_Customer A 
                    WHERE A.CustomerCD = DB.BillingCustomerCD AND A.ChangeDate <= DB.BillingCloseDate
                    AND A.DeleteFlg = 0 
                    ORDER BY A.ChangeDate desc) AS CustomerName 
                  ,CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate
                  ,DB.BillingNO
                  ,DBM.BillingGaku - ISNULL(DC.CollectAmount,0) AS BillingGaku
                  ,CONVERT(varchar,DB.CollectDate,111) AS CollectDate

            FROM D_Billing AS DB
            LEFT OUTER JOIN D_BillingDetails AS DBM
            ON DBM.BillingNO = DB.BillingNO
            AND DBM.DeleteDateTime IS NULL
            
            LEFT OUTER JOIN D_CollectBillingDetails AS DC
            ON DC.CollectPlanNO = DBM.CollectPlanNO
            AND DC.CollectPlanRows = DBM.CollectPlanRows
            AND DC.DeleteDateTime IS NULL

            WHERE DB.StoreCD = @StoreCD
            AND DB.BillingType = 2
            --AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            
            --入金状況：入金済の場合
            AND ((@ChkNyukinzumi = 1 
                AND ISNULL(DB.CollectDate,'') >= (CASE WHEN @CollectDateFrom <> '' THEN CONVERT(DATE, @CollectDateFrom) ELSE ISNULL(DB.CollectDate,'') END)
                AND ISNULL(DB.CollectDate,'') <= (CASE WHEN @CollectDateTo <> ''   THEN CONVERT(DATE, @CollectDateTo)   ELSE ISNULL(DB.CollectDate,'') END)
            --入金状況：未入金の場合
            ) OR (@ChkMinyukin = 1 
                AND DB.CollectDate IS NULL
            ))
            
            AND DB.DeleteDateTime IS NULL

            --    --入金状況：入金済の場合
            --AND (DB.BillingGaku = (CASE WHEN @ChkNyukinzumi = 1 THEN DB.CollectGaku ELSE DB.BillingGaku END)
            --    --入金状況：未入金の場合
            --    OR DB.BillingGaku > (CASE WHEN @ChkMinyukin = 1 THEN DB.CollectGaku ELSE DB.BillingGaku-1 END))
        
            AND EXISTS (SELECT MC.CustomerCD
                        FROM M_Customer AS MC 
                        WHERE MC.ChangeDate <= DB.BillingCloseDate
                        AND MC.CustomerName LIKE '%' + (CASE WHEN @CustomerName <> '' THEN @CustomerName ELSE MC.CustomerName END) + '%'
                        AND MC.DeleteFlg = 0
                        AND MC.CustomerCD = DB.BillingCustomerCD)
        ) AS W
        GROUP BY W.StoreCD, W.CustomerCD,W.BillingCloseDate,W.BillingNO
        HAVING SUM(W.BillingGaku) >= (CASE WHEN @BillingGakuFrom IS NOT NULL THEN @BillingGakuFrom ELSE SUM(W.BillingGaku) END)
        AND    SUM(W.BillingGaku) <= (CASE WHEN @BillingGakuTo   IS NOT NULL THEN @BillingGakuTo   ELSE SUM(W.BillingGaku) END)
        ORDER BY StoreCD, CustomerCD,BillingCloseDate,BillingNO, BillingGaku
        ;
    END

    --売上
    ELSE
    BEGIN
        SELECT MAX(W.StoreCD) AS StoreCD
              ,MAX(W.StoreName) AS StoreName
              ,W.CustomerCD
              ,MAX(W.CustomerName) AS CustomerName
              ,W.BillingCloseDate AS BillingDate
              ,W.BillingNO
              ,SUM(W.BillingGaku) AS BillingGaku
              ,MAX(W.CollectDate) AS CollectDate
        FROM (
            SELECT DB.StoreCD
                 ,(SELECT top 1 A.StoreName 
                   FROM M_Store A 
                   WHERE A.StoreCD = DB.StoreCD AND A.ChangeDate <= DB.BillingCloseDate
                   AND A.DeleteFlg = 0
                   ORDER BY A.ChangeDate desc) AS StoreName

                  ,DB.BillingCustomerCD AS CustomerCD
                  ,(SELECT top 1 A.CustomerName
                    FROM M_Customer A 
                    WHERE A.CustomerCD = DB.BillingCustomerCD AND A.ChangeDate <= DB.BillingCloseDate
                    AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate desc) AS CustomerName 
                   ,CONVERT(varchar,DS.SalesDate,111) AS BillingCloseDate
                   ,DS.SalesNO AS BillingNO
                   ,ISNULL(DSM.SalesGaku,0) AS BillingGaku
                   ,CONVERT(varchar,DB.CollectDate,111) AS CollectDate

            FROM D_Billing AS DB
            LEFT OUTER JOIN D_BillingDetails AS DBM
            ON DBM.BillingNO = DB.BillingNO
            AND DBM.DeleteDateTime IS NULL
            
            LEFT OUTER JOIN D_SalesDetails AS DSM
            ON DSM.SalesNO = DBM.SalesNO
            AND DSM.SalesRows = DBM.SalesRows
            AND DSM.DeleteDateTime IS NULL
            
            LEFT OUTER JOIN D_Sales AS DS
            ON DS.SalesNO = DSM.SalesNO
            AND DS.DeleteDateTime IS NULL

            WHERE DB.StoreCD = @StoreCD
            AND DB.BillingType = 2
            --AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            
            --入金状況：入金済の場合
            AND ((@ChkNyukinzumi = 1 
                AND ISNULL(DB.CollectDate,'') >= (CASE WHEN @CollectDateFrom <> '' THEN CONVERT(DATE, @CollectDateFrom) ELSE ISNULL(DB.CollectDate,'') END)
                AND ISNULL(DB.CollectDate,'') <= (CASE WHEN @CollectDateTo <> ''   THEN CONVERT(DATE, @CollectDateTo)   ELSE ISNULL(DB.CollectDate,'') END)
            --入金状況：未入金の場合
            ) OR (@ChkMinyukin = 1 
                AND DB.CollectDate IS NULL
            ))
            
            AND DB.DeleteDateTime IS NULL

            --    --入金状況：入金済の場合
            --AND (DB.BillingGaku = (CASE WHEN @ChkNyukinzumi = 1 THEN DB.CollectGaku ELSE DB.BillingGaku END)
            --    --入金状況：未入金の場合
            --    OR DB.BillingGaku > (CASE WHEN @ChkMinyukin = 1 THEN DB.CollectGaku ELSE DB.BillingGaku-1 END))
            
            AND EXISTS (SELECT MC.CustomerCD
                        FROM M_Customer AS MC 
                        WHERE MC.ChangeDate <= DB.BillingCloseDate
                        AND MC.CustomerName LIKE '%' + (CASE WHEN @CustomerName <> '' THEN @CustomerName ELSE MC.CustomerName END) + '%'
                        AND MC.DeleteFlg = 0
                        AND MC.CustomerCD = DB.BillingCustomerCD)
        ) AS W
        GROUP BY W.StoreCD, W.CustomerCD,W.BillingCloseDate,W.BillingNO
        HAVING SUM(W.BillingGaku) >= (CASE WHEN @BillingGakuFrom IS NOT NULL THEN @BillingGakuFrom ELSE SUM(W.BillingGaku) END)
        AND    SUM(W.BillingGaku) <= (CASE WHEN @BillingGakuTo   IS NOT NULL THEN @BillingGakuTo   ELSE SUM(W.BillingGaku) END)
        ORDER BY StoreCD, CustomerCD,BillingCloseDate,BillingNO, BillingGaku
        ;
    END
END

GO
