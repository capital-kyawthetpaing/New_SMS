
/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO]    Script Date: 2020/09/24 11:37:42 ******/
DROP PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO]
GO

/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO]    Script Date: 2020/09/24 11:37:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO](
 @p_OrderNO                 varchar(11) 
,@p_OrderProcessNO          varchar(11) 
)
AS
BEGIN

    DECLARE @HacchuuDate varchar(10)
    SELECT @HacchuuDate = OrderDate
      FROM D_Order
    WHERE (OrderNO = @p_OrderNO
           OR
           OrderProcessNO = @p_OrderProcessNO
          )
      AND DeleteDateTime IS NULL

    SELECT ROW_NUMBER()OVER(ORDER BY DODD.OrderRows) AS GyouNO
          ,CAST(1 AS bit) AS TaishouFLG
          ,DODH.OrderNO AS HacchuuNO
          ,DODH.OrderCD   AS SiiresakiCD
          ,MVEN.VendorName AS SiiresakiName
          ,CASE WHEN DODD.DirectFLG = 1 THEN 'Åõ' ELSE NULL END AS ChokusouFLG
          ,CASE WHEN MSKU.NoNetOrderFlg = 0 THEN 'Åõ' ELSE 'Å~' END AS NetFLG
          ,CASE WHEN DODH.DestinationKBN = 1 THEN MSOU.SoukoName ELSE DODH.DestinationName END AS NounyuusakiName
          ,DODD.DirectFlg AS NounyuusakiKBN
          ,CASE WHEN DODD.DirectFlg = 0 THEN MSOU.Address1 + ' ' + MSOU.Address2 ELSE DODH.DestinationAddress1 + ' ' + DODH.DestinationAddress2 END AS NounyuusakiJuusho
          ,MSOU.SoukoCD
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryZip1CD END      AS NounyuusakiYuubinNO1
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryZip2CD END      AS NounyuusakiYuubinNO2
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryAddress1 END   AS NounyuusakiJuusho1
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryAddress2 END   AS NounyuusakiJuusho2
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryMailAddress END AS NounyuusakiMailAddress
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryTelphoneNO END  AS NounyuusakiTELNO
          ,CASE WHEN DODD.DirectFlg = 0 THEN NULL ELSE DDEP.DeliveryFaxNO END       AS NounyuusakiFAXNO
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
          ,DODD.EDIFLG AS EDIFLG
          ,MSKU.MakerItem AS MakerShouhinCD
          ,FORMAT(DODD.DesiredDeliveryDate,'yyyy/MM/dd') AS KibouNouki
          ,DODD.CommentInStore  AS ShanaiBikou
          ,DODD.CommentOutStore AS ShagaiBikou
          ,DJUD.TaniCD AS TaniCD
          ,MMUP.Char1 AS TaniName
          ,DODD.OrderSu AS HacchuuSuu
          ,FORMAT(DODD.OrderUnitPrice,'###,###') AS HacchuuTanka
          ,FORMAT(DODD.OrderHontaiGaku,'###,###') AS Hacchuugaku
          ,DODD.OrderTaxRitsu AS TaxRate
          ,DODD.OrderTax AS HacchuuShouhizeigaku
          ,MSKU.PriceOutTax AS Teika
          ,DODD.OrderUnitPrice * 100 / NULLIF(MSKU.PriceOutTax,0) AS Kakeritu
          ,CASE WHEN DODD.ArrivePlanDate IS NULL THEN CAST(1 as bit) ELSE CAST(0 as bit) END AS IsYuukouTaishouFLG
          ,DODD.OrderRows
          ,DODH.OrderWayKBN
          
    FROM D_Order DODH
    LEFT JOIN D_OrderDetails DODD
    ON DODH.OrderNO = DODD.OrderNO
    AND DODD.DeleteDateTime IS NULL
    LEFT JOIN Fnc_M_SKU_SelectLatest(@HacchuuDate) MSKU
    ON MSKU.AdminNO = DODD.AdminNO
    LEFT JOIN D_JuchuuDetails DJUD
    ON  DJUD.JuchuuNO = DODD.JuchuuNO
    AND DJUD.JuchuuRows = DODD.JuchuuRows
    AND DJUD.DeleteDateTime IS NULL
    LEFT JOIN D_Juchuu DJUH
    ON  DJUH.JuchuuNO = DJUD.JuchuuNO
    AND DJUH.DeleteDateTime IS NULL
    LEFT JOIN Fnc_M_Store_SelectLatest(@HacchuuDate)MSTR
    ON MSTR.StoreCD = DJUH.StoreCD
    LEFT JOIN Fnc_M_Vendor_SelectLatest(@HacchuuDate)MVEN
    ON MVEN.VendorCD = DODH.OrderCD
    LEFT JOIN Fnc_M_Souko_SelectLatest(@HacchuuDate)MSOU
    ON  DODD.DirectFlg = 0
    AND MSOU.StoreCD = DJUH.StoreCD
    AND MSOU.SoukoType in (1,3)
    LEFT JOIN D_DeliveryPlan DDEP
    ON DODD.DirectFlg = 1
    AND DDEP.DeliveryPlanNO = DJUD.DeliveryPlanNO
    LEFT JOIN M_Brand MBRA
    ON MBRA.BrandCD = MSKU.BrandCD
    LEFT JOIN M_MultiPorpose MMUP
    ON MMUP.ID = '201'
    AND MMUP.[Key] = DJUD.TaniCD
    
    WHERE (DODH.OrderNO = @p_OrderNO
           OR
           DODH.OrderProcessNO = @p_OrderProcessNO
          )
      AND DODH.DeleteDateTime IS NULL
      AND DODH.OrderProcessNO IS NOT NULL

    ORDER BY DODD.OrderNO, DODD.OrderRows

END
GO


