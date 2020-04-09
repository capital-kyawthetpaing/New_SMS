 BEGIN TRY 
 Drop Procedure dbo.[D_ArrivalPlan_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    入荷入力
--       Program ID      NyuukaNyuuryoku
--       Create date:    2019.11.09
--    ======================================================================
CREATE PROCEDURE [dbo].[D_ArrivalPlan_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @AdminNo int,
    @SoukoCD varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT --【引当】
               1 KBN
              ,DR.JuchuuNO
              ,DR.JuchuuRows
              ,DR.CustomerCD
              ,DR.CustomerName2
              ,NULL AS HachuSu
              ,DR.ReserveSu
              ,DR.DR_ArrivalSu
              ,DR.DirectFlg
              ,DR.DeliveryPlanDate
              ,DR.VendorCD
              ,DR.VendorName
              ,DR.ArrivalPlanNO
              ,DR.StockNO
              ,DR.ReserveNO
              ,DR.ArrivalPlanKBN

              ,DR.OrderUnitPrice
              ,DR.PriceOutTax
              ,DR.Rate
              ,DR.TaniCD
              ,DR.OrderTaxRitsu
              ,DR.OrderWayKBN
              ,DR.AliasKBN
                        
          FROM D_ArrivalPlan DH
          INNER JOIN 
          		(SELECT DP.ArrivalPlanNO
                        ,DJM.JuchuuNO
                        ,DJM.JuchuuRows
                        ,DJ.CustomerCD
                        ,ISNULL(DJ.CustomerName2,DJ.CustomerName) AS CustomerName2
                        ,DR.ReserveSu
                        ,0 AS DR_ArrivalSu
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg
                              FROM M_SKU A 
                              WHERE A.AdminNO = DP.AdminNO AND A.ChangeDate <= DP.ArrivalPlanDate 
                              AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc) AS DirectFlg
                        ,CONVERT(varchar,DD.DeliveryPlanDate,111) AS DeliveryPlanDate
                        ,DO.OrderCD AS VendorCD
                        ,(SELECT top 1 A.VendorName
                          FROM M_Vendor A 
                          WHERE A.VendorCD = DO.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DO.OrderDate
                          AND A.VendorFlg = 1
                          ORDER BY A.ChangeDate desc) AS VendorName
                        ,DS.StockNO
                        ,DR.ReserveNO
                        ,DP.ArrivalPlanKBN

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
                LEFT OUTER JOIN D_JuchuuDetails AS DJM
                ON DJM.JuchuuNO = DR.Number
                AND DJM.JuchuuRows = DR.NumberRows
                AND DJM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Juchuu AS DJ
                ON DJ.JuchuuNO = DJM.JuchuuNO
                AND DJ.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_DeliveryPlan AS DD
                ON DD.DeliveryPlanNO = DJM.DeliveryPlanNO
                LEFT OUTER JOIN  D_OrderDetails AS DOM
                ON DOM.OrderNO = DP.Number
                AND DOM.OrderRows = DP.NumberRows
                AND DOM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Order AS DO
                ON DO.OrderNO = DOM.OrderNO
                AND DO.DeleteDateTime IS NULL
                
                WHERE DP.DeleteDateTime IS NULL
                AND DP.AdminNO = @AdminNO
                AND DP.LastestFLG = 1
                AND DS.ArrivalYetFLG = 1	--★
                AND DR.ReserveNO IS NOT NULL
            ) AS DR
            ON DR.ArrivalPlanNO = DH.ArrivalPlanNO
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.DeleteDateTime IS NULL
    UNION ALL
        SELECT --【発注】
               2 KBN
              ,DO.OrderNO
              ,DO.OrderRows
              ,DO.OrderCD As CustomerCD
              ,DO.VendorName_Souko AS CustomerName
              ,DO.OrderSu
              ,DO.ReserveSu
              ,DO.ArrivalSu
              ,DO.DirectFlg_Order
              ,DO.ArrivalPlanDate
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
            ) AS DO
            ON DO.ArrivalPlanNO = DH.ArrivalPlanNO
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.DeleteDateTime IS NULL
    UNION ALL
        SELECT --【移動】
               3 KBN
              ,DM.MoveNO
              ,DM.MoveRows
              ,DM.FromSoukoCD As CustomerCD
              ,DM.SoukoName
              ,DM.MoveSu
              ,DM.ReserveSu
              ,DM.ArrivalSu
              ,DM.DirectFlg_Move
              ,DM.ArrivalPlanDate_Move
              ,DM.FromSoukoCD AS VendorCD
              ,DM.SoukoName AS VendorName
              ,DM.ArrivalPlanNO
              ,DM.StockNO
              ,DM.ReserveNO
              ,2 AS ArrivalPlanKBN	--移動

              ,NULL AS OrderUnitPrice
              ,NULL AS PriceOutTax
              ,NULL AS Rate
              ,NULL AS TaniCD
              ,NULL AS OrderTaxRitsu
              ,NULL AS OrderWayKBN
              ,NULL AS AliasKBN
                            
          FROM D_ArrivalPlan DH
          INNER JOIN 
                (SELECT DP.ArrivalPlanNO
                        ,DMM.MoveNO
                        ,DMM.MoveRows
                        ,DM.FromSoukoCD
                        ,(SELECT top 1 A.SoukoName
                          FROM M_Souko A 
                          WHERE A.SoukoCD = DM.FromSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DM.MoveDate
                          ORDER BY A.ChangeDate desc) AS SoukoName
                        ,DMM.MoveSu
                        ,DR.ReserveSu
                        ,0	AS ArrivalSu
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg
                              FROM M_SKU A 
                              WHERE A.AdminNO = DP.AdminNO AND A.ChangeDate <= DP.ArrivalPlanDate 
                              AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc) AS DirectFlg_Move
                        ,CONVERT(varchar,DP.ArrivalPlanDate,111) As ArrivalPlanDate_Move
                        ,DS.StockNO
                        ,DR.ReserveNO
		                                        
                FROM D_ArrivalPlan AS DP
                LEFT OUTER JOIN D_Stock AS DS
                ON DS.ArrivalPlanNO = DP.ArrivalPlanNO
                AND DS.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.ReserveKBN = 2	--移動引当
                AND DR.DeleteDateTime IS NULL
                INNER JOIN D_MoveDetails AS DMM
                ON DMM.MoveNO = DP.Number
                AND DMM.MoveRows = DP.NumberRows
                AND DMM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Move AS DM
                ON DM.MoveNO = DMM.MoveNO
                AND DM.DeleteDateTime IS NULL
                
                WHERE DP.DeleteDateTime IS NULL
                AND DP.AdminNO = @AdminNO
                AND DP.LastestFLG = 1
                AND DS.ArrivalYetFLG = 1	--★
            ) AS DM
            ON DM.ArrivalPlanNO = DH.ArrivalPlanNO
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.DeleteDateTime IS NULL
    
    ORDER BY KBN,JuchuuNO,JuchuuRows
    ;

END


