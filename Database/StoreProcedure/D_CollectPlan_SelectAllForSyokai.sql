 BEGIN TRY 
 Drop Procedure dbo.[D_CollectPlan_SelectAllForSyokai]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_CollectPlan_SelectAllForSyokai]    */
CREATE PROCEDURE [dbo].[D_CollectPlan_SelectAllForSyokai](
    -- Add the parameters for the stored procedure here
    @ShippingDateFrom  varchar(10),
    @ShippingDateTo  varchar(10),
    @StoreCD  varchar(4),
    @PaymentMethodCD varchar(3),
    @Kbn tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    IF @Kbn = 1
    BEGIN
        SELECT DH.CollectPlanNO
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName

          ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
                    
          ,(SELECT M.DenominationName
             FROM M_DenominationKBN AS M
             WHERE M.DenominationCD = DH.PaymentMethodCD) AS DenominationName
          ,DH.CollectPlanGaku
          ,CONVERT(varchar,DP.CollectDate,111) AS CollectDate
          ,DP.CollectAmount
          ,DH.CollectPlanGaku - ISNULL(DP.CollectAmount,0) AS MinyukinGaku
          
          ,DH.JuchuuNO
          ,DH.CustomerCD
          ,(SELECT top 1 A.CustomerName
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD 
              AND A.DeleteFlg = 0 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              ORDER BY A.ChangeDate desc) AS CustomerName 
                 
           ,DJM.SKUName
            
        from D_CollectPlan AS DH
        LEFT OUTER JOIN D_Juchuu AS DJ
        ON DJ.JuchuuNO = DH.JuchuuNO
        AND DJ.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DH.JuchuuNO
        AND DJM.JuchuuRows = 1
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_ShippingDetails AS DSM
        ON DSM.Number = DJM.JuchuuNO
        AND DSM.NumberRows = DJM.JuchuuRows
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.ShippingNO = DSM.ShippingNO
        AND DS.DeleteDateTime IS NULL
        LEFT OUTER JOIN (
            SELECT DB.CollectPlanNO
                    ,MAX(DCT.CollectDate) AS CollectDate
                    ,SUM(DCT.CollectAmount) AS CollectAmount
            FROM D_PaymentConfirm AS DP
            LEFT OUTER JOIN D_CollectBilling AS DB
            ON DB.ConfirmNO = DP.ConfirmNO
            AND DB.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectBillingDetails AS DCM
            ON DCM.ConfirmNO = DB.ConfirmNO
            AND DCM.CollectPlanNO = DB.CollectPlanNO
            AND DCM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlanDetails AS DC
            ON DC.CollectPlanNO = DCM.CollectPlanNO
            AND DC.CollectPlanRows = DCM.CollectPlanRows
            AND DC.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlan AS DCP
            ON DCP.CollectPlanNO = DC.CollectPlanNO
            AND DCP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Juchuu AS DJ
            ON DJ.JuchuuNO = DCP.JuchuuNO
            AND DJ.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_JuchuuDetails AS DJM
            ON DJM.JuchuuNO = DCP.JuchuuNO
            AND DJM.JuchuuRows = 1
            AND DJM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_ShippingDetails AS DSM
            ON DSM.Number = DJM.JuchuuNO
            AND DSM.NumberRows = DJM.JuchuuRows
            AND DSM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Shipping AS DS
            ON DS.ShippingNO = DSM.ShippingNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Collect AS DCT
            ON DCT.CollectNO = DP.CollectNO
            AND DCT.DeleteDateTime IS NULL
            WHERE DP.DeleteDateTime IS NULL
            AND ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
            AND ISNULL(DS.ShippingDate,'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(DS.ShippingDate,'') END)
            AND DCP.StoreCD = @StoreCD
            AND DCP.PaymentMethodCD = @PaymentMethodCD
        	GROUP BY DB.CollectPlanNO
        ) AS DP
        ON DP.CollectPlanNO = DH.CollectPlanNO
        
        WHERE ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
        AND ISNULL(ISNULL(DS.ShippingDate,''),'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(ISNULL(DS.ShippingDate,''),'') END)
        AND DH.StoreCD = @StoreCD
        AND DH.PaymentMethodCD = @PaymentMethodCD
        AND DH.DeleteDateTime IS NULL   
  
        ORDER BY DH.StoreCD 
            ,DS.ShippingDate    
            ,DH.PaymentMethodCD 
            ,DH.CollectPlanGaku 
            ,DP.CollectDate
            ,DP.CollectAmount
            ,DH.JuchuuNO
        
    END
    
    --入金状況 入金データ無=ONかつ入金データ有、一部入金=OFFの場合、
    ELSE IF @Kbn = 2
    BEGIN
        SELECT DH.CollectPlanNO
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName

          ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
                    
          ,(SELECT M.DenominationName
             FROM M_DenominationKBN AS M
             WHERE M.DenominationCD = DH.PaymentMethodCD) AS DenominationName
          ,DH.CollectPlanGaku
          ,CONVERT(varchar,DP.CollectDate,111) AS CollectDate
          ,DP.CollectAmount

          ,DH.JuchuuNO
          ,DH.CustomerCD
          ,(SELECT top 1 A.CustomerName
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD 
              AND A.DeleteFlg = 0 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              ORDER BY A.ChangeDate desc) AS CustomerName 
                 
           ,DJM.SKUName
            
        from D_CollectPlan AS DH
        LEFT OUTER JOIN D_Juchuu AS DJ
        ON DJ.JuchuuNO = DH.JuchuuNO
        AND DJ.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DH.JuchuuNO
        AND DJM.JuchuuRows = 1
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_ShippingDetails AS DSM
        ON DSM.Number = DJM.JuchuuNO
        AND DSM.NumberRows = DJM.JuchuuRows
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.ShippingNO = DSM.ShippingNO
        AND DS.DeleteDateTime IS NULL
        LEFT OUTER JOIN (
            SELECT DB.CollectPlanNO
                    ,MAX(DCT.CollectDate) AS CollectDate
                    ,SUM(DCT.CollectAmount) AS CollectAmount
            FROM D_PaymentConfirm AS DP
            LEFT OUTER JOIN D_CollectBilling AS DB
            ON DB.ConfirmNO = DP.ConfirmNO
            AND DB.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectBillingDetails AS DCM
            ON DCM.ConfirmNO = DB.ConfirmNO
            AND DCM.CollectPlanNO = DB.CollectPlanNO
            AND DCM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlanDetails AS DC
            ON DC.CollectPlanNO = DCM.CollectPlanNO
            AND DC.CollectPlanRows = DCM.CollectPlanRows
            AND DC.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlan AS DCP
            ON DCP.CollectPlanNO = DC.CollectPlanNO
            AND DCP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Juchuu AS DJ
            ON DJ.JuchuuNO = DCP.JuchuuNO
            AND DJ.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_JuchuuDetails AS DJM
            ON DJM.JuchuuNO = DCP.JuchuuNO
            AND DJM.JuchuuRows = 1
            AND DJM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_ShippingDetails AS DSM
            ON DSM.Number = DJM.JuchuuNO
            AND DSM.NumberRows = DJM.JuchuuRows
            AND DSM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Shipping AS DS
            ON DS.ShippingNO = DSM.ShippingNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Collect AS DCT
            ON DCT.CollectNO = DP.CollectNO
            AND DCT.DeleteDateTime IS NULL
            WHERE DP.DeleteDateTime IS NULL
            AND ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
            AND ISNULL(DS.ShippingDate,'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(DS.ShippingDate,'') END)
            AND DCP.StoreCD = @StoreCD
            AND DCP.PaymentMethodCD = @PaymentMethodCD
        	GROUP BY DB.CollectPlanNO
        ) AS DP
        ON DP.CollectPlanNO = DH.CollectPlanNO
        
        WHERE ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
        AND ISNULL(DS.ShippingDate,'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(DS.ShippingDate,'') END)
        AND DH.StoreCD = @StoreCD
        AND DH.PaymentMethodCD = @PaymentMethodCD
        AND DH.DeleteDateTime IS NULL
        
        AND DP.CollectPlanNO IS NULL    
  
        ORDER BY DH.StoreCD 
            ,DS.ShippingDate    
            ,DH.PaymentMethodCD 
            ,DH.CollectPlanGaku 
            ,DH.JuchuuNO
        
    END
    
    --入金状況 入金データ無=OFFかつ入金データ有、一部入金=ONの場合
    ELSE IF @Kbn = 3
    BEGIN
        SELECT DH.CollectPlanNO
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName

          ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
                    
          ,(SELECT M.DenominationName
             FROM M_DenominationKBN AS M
             WHERE M.DenominationCD = DH.PaymentMethodCD) AS DenominationName
          ,DH.CollectPlanGaku
          ,CONVERT(varchar,DP.CollectDate,111) AS CollectDate
          ,DP.CollectAmount

          ,DH.JuchuuNO
          ,DH.CustomerCD
          ,(SELECT top 1 A.CustomerName
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD 
              AND A.DeleteFlg = 0 
              AND A.ChangeDate <= DH.FirstCollectPlanDate
              ORDER BY A.ChangeDate desc) AS CustomerName 
                 
           ,DJM.SKUName
            
        from D_CollectPlan AS DH

        LEFT OUTER JOIN D_Juchuu AS DJ
        ON DJ.JuchuuNO = DH.JuchuuNO
        AND DJ.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DH.JuchuuNO
        AND DJM.JuchuuRows = 1
        AND DJM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_ShippingDetails AS DSM
        ON DSM.Number = DJM.JuchuuNO
        AND DSM.NumberRows = DJM.JuchuuRows
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.ShippingNO = DSM.ShippingNO
        AND DS.DeleteDateTime IS NULL
        INNER JOIN (
            SELECT DB.CollectPlanNO
                    ,MAX(DCT.CollectDate) AS CollectDate
                    ,SUM(DCT.CollectAmount) AS CollectAmount
            FROM D_PaymentConfirm AS DP
            LEFT OUTER JOIN D_CollectBilling AS DB
            ON DB.ConfirmNO = DP.ConfirmNO
            AND DB.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectBillingDetails AS DCM
            ON DCM.ConfirmNO = DB.ConfirmNO
            AND DCM.CollectPlanNO = DB.CollectPlanNO
            AND DCM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlanDetails AS DC
            ON DC.CollectPlanNO = DCM.CollectPlanNO
            AND DC.CollectPlanRows = DCM.CollectPlanRows
            AND DC.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlan AS DCP
            ON DCP.CollectPlanNO = DC.CollectPlanNO
            AND DCP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Juchuu AS DJ
            ON DJ.JuchuuNO = DCP.JuchuuNO
            AND DJ.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_JuchuuDetails AS DJM
            ON DJM.JuchuuNO = DCP.JuchuuNO
            AND DJM.JuchuuRows = 1
            AND DJM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_ShippingDetails AS DSM
            ON DSM.Number = DJM.JuchuuNO
            AND DSM.NumberRows = DJM.JuchuuRows
            AND DSM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Shipping AS DS
            ON DS.ShippingNO = DSM.ShippingNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Collect AS DCT
            ON DCT.CollectNO = DP.CollectNO
            AND DCT.DeleteDateTime IS NULL
            WHERE DP.DeleteDateTime IS NULL
            AND ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
            AND ISNULL(DS.ShippingDate,'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(DS.ShippingDate,'') END)
            AND DCP.StoreCD = @StoreCD
            AND DCP.PaymentMethodCD = @PaymentMethodCD
        	GROUP BY DB.CollectPlanNO
        ) AS DP
        ON DP.CollectPlanNO = DH.CollectPlanNO
        
        WHERE ISNULL(DS.ShippingDate,'') >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE ISNULL(DS.ShippingDate,'') END)
        AND ISNULL(DS.ShippingDate,'') <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE ISNULL(DS.ShippingDate,'') END)
        AND DH.StoreCD = @StoreCD
        AND DH.PaymentMethodCD = @PaymentMethodCD
        AND DH.DeleteDateTime IS NULL    
        AND DP.CollectPlanNO IS NOT NULL    
  
        ORDER BY DH.StoreCD 
            ,DS.ShippingDate    
            ,DH.PaymentMethodCD 
            ,DH.CollectPlanGaku 
            ,DP.CollectDate
            ,DP.CollectAmount
            ,DH.JuchuuNO
        
    END
END


