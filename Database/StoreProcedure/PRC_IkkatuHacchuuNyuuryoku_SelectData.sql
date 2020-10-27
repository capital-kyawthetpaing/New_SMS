

/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectData]    Script Date: 2020/10/27 13:34:40 ******/
DROP PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectData]
GO

/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectData]    Script Date: 2020/10/27 13:34:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO



  
CREATE PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectData](  
 @p_IkkatuHacchuuMode        varchar(1) --0:Net”­’A1:FAX”­’  
,@p_HacchuuDate              varchar(10)  
,@p_VendorCD                 varchar(13)   
,@p_JuchuuStaffCD            varchar(13)   
,@p_StoreCD                  varchar(4)  
,@p_IsSaiHacchuu             varchar(1)  
)  
AS  
BEGIN  
  
    SELECT ROW_NUMBER()OVER(ORDER BY DJUD.VendorCD,DJUD.JuchuuNO,DJUD.DisplayRows) AS GyouNO    
          ,CASE WHEN @p_IkkatuHacchuuMode = '0' THEN CAST(1 AS bit)    
                                                ELSE CASE WHEN DORD.JuchuuRows IS NULL THEN CAST(1 AS bit)    
                                                                                       ELSE CAST(0 AS bit)    
                                                     END    
           END AS TaishouFLG    
          ,CAST(1 AS bit) AS TaishouFLG    
          ,DORD_Latest.OrderNO AS HacchuuNO    
          ,DJUD.VendorCD   AS SiiresakiCD    
          ,MVEN.VendorName AS SiiresakiName    
          ,CASE WHEN DJUD.DirectFlg = 1 THEN '›' ELSE NULL END AS ChokusouFLG    
          ,CASE WHEN MSKU.NoNetOrderFlg = 0 THEN '›' ELSE '~' END AS NetFLG    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN MSOU.SoukoName ELSE DDEP.DeliveryName END AS NounyuusakiName    
          ,DJUD.DirectFlg AS NounyuusakiKBN    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN MSOU.Address1 + ' ' + MSOU.Address2 ELSE DDEP.DeliveryAddress1 + ' ' + DDEP.DeliveryAddress2 END AS NounyuusakiJuusho    
          ,MSOU.SoukoCD    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryZip1CD END      AS NounyuusakiYuubinNO1    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryZip2CD END      AS NounyuusakiYuubinNO2    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryAddress1 END   AS NounyuusakiJuusho1    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryAddress2 END   AS NounyuusakiJuusho2    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryMailAddress END AS NounyuusakiMailAddress    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryTelphoneNO END  AS NounyuusakiTELNO    
          ,CASE WHEN DJUD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryFaxNO END       AS NounyuusakiFAXNO    
          ,DJUD.JuchuuNO    
          ,DJUD.JuchuuRows    
          ,DJUD.AdminNO    
          ,MSKU.SKUCD    
          ,MSKU.JanCD    
          ,MSKU.SKUName AS SKUName    
          ,MSKU.SKUName AS ShouhinName    
          ,MSKU.VariousFLG    
          ,MBRA.BrandName    
          ,MSKU.SizeName    
          ,MSKU.ColorName    
          ,MSKU.OrderAttentionNote AS HacchuuChuuiZikou    
          ,MSKU.EDIOrderFlg AS EDIFLG    
          ,MSKU.MakerItem AS MakerShouhinCD    
          ,FORMAT(DJUD.DesiredDeliveryDate,'yyyy/MM/dd') AS KibouNouki
          ,DJUD.CommentInStore  AS ShanaiBikou    
          ,DJUD.CommentOutStore AS ShagaiBikou    
          ,DJUD.TaniCD AS TaniCD    
          ,MMUP.Char1 AS TaniName    
          ,SUB_HacchuuSuu.Value AS HacchuuSuu    
          ,FORMAT(SUB_HacchuuTanka.Value,'###,###') AS HacchuuTanka    
          ,FORMAT(SUB_Hacchuugaku.Value,'###,###') AS Hacchuugaku    
          ,CASE WHEN MSKU.TaxRateFLG = 1 THEN MSAT.TaxRate1    
                WHEN MSKU.TaxRateFLG = 2 THEN MSAT.TaxRate2    
                                         ELSE 0    
           END AS TaxRate    
          ,CASE WHEN MVEN.TaxFractionKBN = 1 THEN FLOOR(SUB_Hacchuugaku.Value * SUB_TaxRate.Value)    
                WHEN MVEN.TaxFractionKBN = 2 THEN ROUND(SUB_Hacchuugaku.Value * SUB_TaxRate.Value,0)    
                WHEN MVEN.TaxFractionKBN = 3 THEN CEILING(SUB_Hacchuugaku.Value * SUB_TaxRate.Value)    
                                             ELSE 0    
           END AS HacchuuShouhizeigaku    
          ,MSKU.PriceOutTax AS Teika    
          ,SUB_HacchuuTanka.Value * 100 / NULLIF(MSKU.PriceOutTax,0) AS Kakeritu    
          ,CAST(1 as bit) AS IsYuukouTaishouFLG
          ,NULL AS OrderRows    
          ,CASE WHEN @p_IkkatuHacchuuMode = '0' THEN 1 ELSE 2 END AS OrderWayKBN
              
    FROM D_JuchuuDetails DJUD  
    LEFT JOIN F_SKU(@p_HacchuuDate)MSKU    
    ON MSKU.AdminNO = DJUD.AdminNO    
    LEFT JOIN (SELECT SUB.JuchuuNO    
                     ,SUB.JuchuuRows     
                     ,SUB.OrderNO    
                     ,SUB.OrderRows    
                 FROM (SELECT ROW_NUMBER()OVER(PARTITION BY DLOD.JuchuuNO,DLOD.JuchuuRows ORDER BY OrderSEQ DESC)num    
                             ,DLOD.JuchuuNO    
                             ,DLOD.JuchuuRows     
                             ,DLOD.OrderNO    
                             ,DLOD.OrderRows    
                         FROM D_LastOrder DLOD    
                       )SUB    
                WHERE num = 1    
              )SUB
    ON  SUB.JuchuuNO = DJUD.JuchuuNO    
    AND SUB.JuchuuRows = DJUD.JuchuuRows    
    LEFT JOIN D_OrderDetails DORD    
    ON  DORD.JuchuuNO = DJUD.JuchuuNO    
    AND DORD.JuchuuRows = DJUD.JuchuuRows    
    AND (SUB.OrderNO IS NULL    
         OR    
         (DORD.OrderNO = SUB.OrderNO    
          AND DORD.OrderRows = SUB.OrderRows)    
        )    
    AND DORD.DeleteDateTime IS NULL    
    LEFT JOIN D_Juchuu DJUH    
    ON  DJUH.JuchuuNO = DJUD.JuchuuNO    
    AND DJUH.DeleteDateTime IS NULL    
    LEFT JOIN Fnc_M_Store_SelectLatest(@p_HacchuuDate)MSTR    
    ON MSTR.StoreCD = DJUH.StoreCD    
    LEFT JOIN F_Vendor(@p_HacchuuDate)MVEN    
    ON MVEN.VendorCD = DJUD.VendorCD    
    LEFT JOIN Fnc_M_Souko_SelectLatest(@p_HacchuuDate)MSOU    
    ON  DJUD.DirectFlg = 0    
    AND MSOU.StoreCD = DJUH.StoreCD    
    AND MSOU.SoukoType in (1,3)    
    LEFT JOIN D_DeliveryPlan DDEP    
    ON DJUD.DirectFlg = 1    
    AND DDEP.DeliveryPlanNO = DJUD.DeliveryPlanNO    
    LEFT JOIN M_Brand MBRA    
    ON MBRA.BrandCD = MSKU.BrandCD    
    LEFT JOIN M_MultiPorpose MMUP    
    ON MMUP.ID = '201'    
    AND MMUP.[Key] = DJUD.TaniCD    
    LEFT JOIN Fnc_M_JANOrderPrice_SelectLatest(@p_HacchuuDate)MJOP_StoreSitei    
    ON MJOP_StoreSitei.AdminNO = DJUD.AdminNO    
    AND MJOP_StoreSitei.VendorCD = DJUD.VendorCD    
    AND MJOP_StoreSitei.StoreCD = DJUH.StoreCD
    LEFT JOIN Fnc_M_JANOrderPrice_SelectLatest(@p_HacchuuDate)MJOP_StoreNotSitei    
    ON MJOP_StoreNotSitei.AdminNO = DJUD.AdminNO   
    AND MJOP_StoreNotSitei.VendorCD = DJUD.VendorCD
    AND MJOP_StoreNotSitei.StoreCD = '0000'
    LEFT JOIN Fnc_M_ItemOrderPrice_SelectLatest(@p_HacchuuDate)MIOP_StoreSitei    
    ON  MIOP_StoreSitei.MakerItem = MSKU.MakerItem    
    AND MIOP_StoreSitei.VendorCD = DJUD.VendorCD    
    AND MIOP_StoreSitei.StoreCD = DJUH.StoreCD
    LEFT JOIN Fnc_M_ItemOrderPrice_SelectLatest(@p_HacchuuDate)MIOP_StoreNotSitei    
    ON  MIOP_StoreNotSitei.MakerItem = MSKU.MakerItem    
    AND MIOP_StoreNotSitei.VendorCD = DJUD.VendorCD
    AND MIOP_StoreNotSitei.StoreCD = '0000'
    CROSS JOIN Fnc_M_SalesTax_SelectLatest(@p_HacchuuDate) MSAT    
    OUTER APPLY(SELECT DJUD.OrderUnitPrice Value)SUB_HacchuuTanka    
    OUTER APPLY(SELECT DJUD.JuchuuSuu - DJUD.HikiateSu Value)SUB_HacchuuSuu    
    OUTER APPLY(SELECT SUB_HacchuuTanka.Value * SUB_HacchuuSuu.Value Value)SUB_Hacchuugaku
    OUTER APPLY(SELECT CASE WHEN MSKU.TaxRateFLG = 1 THEN MSAT.TaxRate1 / 100    
                            WHEN MSKU.TaxRateFLG = 2 THEN MSAT.TaxRate2 / 100    
                                                     ELSE 0    
                       END Value)SUB_TaxRate    
     --20200924 OUTER APPLY (SELECT ISNULL(SUM(DARP.ArrivalSu),0)ArrivalSu    
    OUTER APPLY (SELECT ISNULL(SUM(DARP.ArrivalPlanSu),0)ArrivalPlanSu    --
                 FROM D_ArrivalPlan DARP    
                  WHERE DARP.ArrivalPlanKBN  = 1    
                    AND DARP.Number          = DORD.OrderNO    
                    AND DARP.NumberRows      = DORD.OrderRows    
     --20200924               AND DARP.ArrivalPlanDate IS NULL     
     --20200924               AND DARP.DeleteDateTime  IS NOT NULL    
                    AND DARP.ArrivalPlanDate IS NOT NULL      --
                    AND DARP.DeleteDateTime  IS NULL		  --	

                )DARP    
    OUTER APPLY(SELECT TOP 1 DORD_SUB.OrderNO
                  FROM D_OrderDetails DORD_SUB
                 WHERE @p_IsSaiHacchuu = '1'
                   AND DJUD.JuchuuSuu > (DJUD.HikiateSu + DARP.ArrivalPlanSu)
                   AND DORD.JuchuuRows IS NOT NULL
                   AND DJUD.JuchuuNO = DORD_SUB.JuchuuNO
                   AND DJUD.JuchuuRows = DORD_SUB.JuchuuRows   
                 ORDER BY DORD_SUB.InsertDateTime DESC
               )DORD_Latest
    WHERE DJUD.DeleteDateTime IS NULL    
      AND (@p_VendorCD IS NULL OR DJUD.VendorCD = @p_VendorCD)
      AND DJUD.NotOrderFlg = 0
      AND (@p_JuchuuStaffCD IS NULL OR DJUH.StaffCD = @p_JuchuuStaffCD)
      AND ((MSTR.StoreKBN <> 3    
            AND    
            DJUH.StoreCD = @p_StoreCD    
           )    
           OR    
           (MSTR.StoreKBN = 3    
            AND    
            DJUH.StoreCD = (SELECT TOP 1 StoreCD    
                             FROM M_Store MSTR    
                            WHERE MSTR.StoreKBN = 2     
                              AND MSTR.DeleteFlg = 0    
                             AND MSTR.ChangeDate <= @p_HacchuuDate    
                            ORDER BY MSTR.ChangeDate DESC    
                           )    
           )    
          )    
      AND ((@p_IkkatuHacchuuMode = '0'     
            AND DJUD.DirectFlg = 0    
            AND MSKU.NoNetOrderFlg = 0    
            AND DORD.JuchuuRows IS NULL    
            AND DJUD.JuchuuSuu > DJUD.HikiateSu    
            AND MVEN.NetFlg = 1
           )    
            OR    
           (@p_IkkatuHacchuuMode = '1'    
            AND ((DJUD.JuchuuSuu > DJUD.HikiateSu    
                  AND (DJUD.DirectFlg = 1 OR MSKU.NoNetOrderFlg = 1 OR MVEN.NetFlg = 0)    
                  AND DORD.JuchuuRows IS NULL    
                 )    
                 OR    
     --20200924            (DJUD.JuchuuSuu > (DJUD.HikiateSu + DARP.ArrivalSu)    
                 (@p_IsSaiHacchuu = '1'
                  AND DJUD.JuchuuSuu > (DJUD.HikiateSu + DARP.ArrivalPlanSu)   --
                  AND DORD.JuchuuRows IS NOT NULL    
                 )    
                )    
           )    
          )    
    ORDER BY DJUD.VendorCD    
            ,DJUD.JuchuuNO    
            ,DJUD.DisplayRows    
  
END  
GO


