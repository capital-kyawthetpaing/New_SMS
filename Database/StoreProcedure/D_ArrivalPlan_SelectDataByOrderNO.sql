 BEGIN TRY 
 Drop Procedure dbo.[D_ArrivalPlan_SelectDataByOrderNO]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[D_ArrivalPlan_SelectDataByOrderNO]
    (@OrderNO varchar(11),
    @AdminNo int,
    @SoukoCD varchar(6)
    )AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    --D_ArrivalPlan_SelectDataと同じ内容（発注分だけ）
	SELECT --【発注】
           2 KBN
          ,DO.OrderNO AS JuchuuNO
          ,DO.OrderRows AS JuchuuRows
          ,DO.OrderCD As CustomerCD
          ,DO.VendorName_Souko AS CustomerName2
          ,DO.OrderSu AS HachuSu
          ,DO.ReserveSu
          ,DO.ArrivalSu AS DR_ArrivalSu
          ,DO.DirectFlg_Order AS DirectFlg
          ,DO.ArrivalPlanDate AS DeliveryPlanDate
          ,DO.VendorCD
          ,DO.VendorName
          ,DO.ArrivalPlanNO
          ,DO.StockNO
          ,DO.ReserveNO
          ,1 AS ArrivalPlanKBN	--発注

          ,DO.OrderUnitPrice
          ,DO.PriceOutTax
          ,DO.Rate
          ,DO.TaniCD
          ,DO.OrderTaxRitsu
          ,DO.OrderWayKBN
          ,DO.AliasKBN
                        
      FROM D_ArrivalPlan DH
      INNER JOIN 
      		(SELECT DP.ArrivalPlanNO
                    ,DOM.OrderNO
                    ,DOM.OrderRows
                    ,DO.OrderCD
                    ,(SELECT top 1 A.VendorName
                      FROM M_Vendor A 
                      WHERE A.VendorCD = DO.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DO.OrderDate
                      AND A.VendorFlg = 1
                      ORDER BY A.ChangeDate desc) AS VendorName_Souko
                    ,DOM.OrderSu
                    ,DR.ReserveSu
                    ,0 AS ArrivalSu
                    ,(CASE WHEN DOM.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg_Order
                    ,CONVERT(varchar,DP.ArrivalPlanDate,111) As ArrivalPlanDate
                    ,DO.OrderCD AS VendorCD
                    ,(SELECT top 1 A.VendorName
                      FROM M_Vendor A 
                      WHERE A.VendorCD = DO.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DO.OrderDate
                      AND A.VendorFlg = 1
                      ORDER BY A.ChangeDate desc) AS VendorName
                    ,DS.StockNO
                    ,DR.ReserveNO

                    ,DOM.OrderUnitPrice
                    ,DOM.PriceOutTax
                    ,DOM.Rate
                    ,DOM.TaniCD
                    ,DOM.OrderTaxRitsu
                    ,DO.OrderWayKBN
                    ,DO.AliasKBN
                    
            FROM D_ArrivalPlan AS DP
            LEFT OUTER JOIN D_Stock AS DS
            ON DS.ArrivalPlanNO = DP.ArrivalPlanNO
            AND DS.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Reserve AS DR
            ON DR.StockNO = DS.StockNO
            AND DR.ReserveKBN = 1	--受注引当
            AND DR.DeleteDateTime IS NULL
            INNER JOIN D_OrderDetails AS DOM
            ON DOM.OrderNO = DP.Number
            AND DOM.OrderRows = DP.NumberRows
            AND DOM.JuchuuNO IS NULL
            AND DOM.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Order AS DO
            ON DO.OrderNO = DOM.OrderNO
            AND DO.DeleteDateTime IS NULL
            
            WHERE DP.DeleteDateTime IS NULL
            AND DP.AdminNO = @AdminNO
            AND DP.LastestFLG = 1
            AND DS.ArrivalYetFLG = 1	--★
            AND DO.LastApprovalDate IS NOT NULL
    		AND DO.OrderNO = @OrderNO
        ) AS DO
        ON DO.ArrivalPlanNO = DH.ArrivalPlanNO
    WHERE DH.SoukoCD = @SoukoCD
    AND DH.AdminNO = @AdminNO
    AND DH.LastestFLG = 1
    AND DH.ArrivalSu = 0
    AND DH.DeleteDateTime IS NULL
    --AND DH.ArrivalPlanNO = @ArrivalPlanNO
	;        
	
END

