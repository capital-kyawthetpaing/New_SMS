

/****** Object:  StoredProcedure [dbo].[D_ArrivalPlan_SelectData]    Script Date: 2020/09/14 18:00:22 ******/
DROP PROCEDURE [dbo].[D_ArrivalPlan_SelectData]
GO

/****** Object:  StoredProcedure [dbo].[D_ArrivalPlan_SelectData]    Script Date: 2020/09/14 18:00:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--  ======================================================================
--       Program Call    ì¸â◊ì¸óÕ
--       Program ID      NyuukaNyuuryoku
--       Create date:    2019.11.09
--    ======================================================================
CREATE PROCEDURE [dbo].[D_ArrivalPlan_SelectData]
    (@OperateMode    tinyint,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
    @AdminNo int,
    @SoukoCD varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --âÊñ çÄñ⁄ì]ëóï\02
    SELECT --Åyà¯ìñÅz
           1 KBN
          ,DR.JuchuuNO
          ,DR.JuchuuRows
          ,DR.CustomerCD
          ,DR.CustomerName2
          --,NULL AS HachuSu
          ,DR.ReserveSu     --à¯ìñêîÅió\íËêîÅj
          ,DR.DR_ArrivalSu  --ì¸â◊êî
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
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN 'ÅZ' ELSE '' END) DirectFlg
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
                AND DR.ReserveKBN = 1	--éÛíçà¯ìñ
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
                AND DP.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
                AND DS.ArrivalYetFLG = 1	--Åö
                AND DR.ReserveNO IS NOT NULL
            ) AS DR
            ON DR.ArrivalPlanNO = DH.ArrivalPlanNO
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
        AND DH.DeleteDateTime IS NULL
    UNION ALL
        SELECT --Åyî≠íçÅz
           2 KBN
          ,DO.OrderNO
          ,DO.OrderRows
          ,DO.OrderCD As CustomerCD
          ,DO.VendorName_Souko AS CustomerName
          ,DO.OrderSu             --à¯ìñêîÅió\íËêîÅj
          --,DO.ReserveSu         
          --,ISNULL(DS.ReserveSu + DS.InstructionSu + DS.ShippingSU,0) AS ReserveSu --à¯ìñêî
          --,DO.ArrivalSu
          ,0 AS ArrivalSu   --DH.ArrivalSu      --ì¸â◊êî
          ,DO.DirectFlg_Order
          ,DO.ArrivalPlanDate
          ,DO.VendorCD
          ,DO.VendorName
          ,DO.ArrivalPlanNO
          ,DO.StockNO
          ,DO.ReserveNO
          ,1 AS ArrivalPlanKBN	--î≠íç

          ,DO.OrderUnitPrice
          ,DO.PriceOutTax
          ,DO.Rate
          ,DO.TaniCD
          ,DO.OrderTaxRitsu
          ,DO.OrderWayKBN
          ,DO.AliasKBN
              
          FROM D_ArrivalPlan DH
          INNER JOIN 
          --àÍéûÉèÅ[ÉNÉeÅ[ÉuÉãÅuD_OrderáAÅv(âÊñ ì]ëóï\02Ç≈ÅuD_OrderáAÅvÇ∆ÇµÇƒégóp)
                (SELECT DP.ArrivalPlanNO
                        ,DOM.OrderNO
                        ,DOM.OrderRows
                        ,DO.OrderCD
                        ,(SELECT top 1 A.VendorName
                          FROM M_Vendor A 
                          WHERE A.VendorCD = DO.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DO.OrderDate
                          AND A.VendorFlg = 1
                          ORDER BY A.ChangeDate desc) AS VendorName_Souko
                        ,DOM.OrderSu - isnull(W.ReserveSU,0) AS OrderSu
                        ,DR.ReserveSu
                        ,0 AS ArrivalSu
                        ,(CASE WHEN DOM.DirectFlg = 1 THEN 'ÅZ' ELSE '' END) DirectFlg_Order
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
                LEFT OUTER JOIN (SELECT D.StockNO, SUM(D.ReserveSU) AS ReserveSU
                				FROM D_Reserve AS D 
                                WHERE D.ReserveKBN = 1  --éÛíçà¯ìñ
                                AND D.DeleteDateTime IS NULL
                                GROUP BY D.StockNO) AS W
                ON W.StockNO = DS.StockNO
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.ReserveKBN = 1	--éÛíçà¯ìñ
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
                AND DP.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
                AND DS.ArrivalYetFLG = 1	--Åö
                --AND DO.LastApprovalDate IS NOT NULL
                AND DO.ApprovalStageFLG >= 9
                AND DP.ArrivalPlanSu <> DS.ReserveSu + DS.InstructionSu + DS.ShippingSU
            ) AS DO
            ON DO.ArrivalPlanNO = DH.ArrivalPlanNO
        LEFT OUTER JOIN D_Stock AS DS
        ON DS.ArrivalPlanNO = DH.ArrivalPlanNO
        AND DS.DeleteDateTime IS NULL
        
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
        AND DH.DeleteDateTime IS NULL
    UNION ALL
        SELECT --Åyà⁄ìÆÅz
           3 KBN
          ,DM.MoveNO
          ,DM.MoveRows
          ,DM.FromSoukoCD As CustomerCD
          ,DM.SoukoName
          ,DM.MoveSu                --ó\íËêî
          --,DH.ArrivalPlanSu AS MoveSu   
          --,DM.ReserveSu
          --,ISNULL(DS.ReserveSu + DS.InstructionSu + DS.ShippingSU,0) AS ReserveSu --à¯ìñêî
          --,DM.ArrivalSu
          ,0 AS ArrivalSu   --DH.ArrivalSu      --ì¸â◊êî
          ,DM.DirectFlg_Move
          ,DM.ArrivalPlanDate_Move
          ,DM.FromSoukoCD AS VendorCD
          ,DM.SoukoName AS VendorName
          ,DM.ArrivalPlanNO
          ,DM.StockNO
          ,DM.ReserveNO
          ,2 AS ArrivalPlanKBN	--à⁄ìÆ

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
                        ,DMM.MoveSu - isnull(W.ReserveSU,0) AS MoveSu
                        ,DR.ReserveSu
                        ,0	AS ArrivalSu
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN 'ÅZ' ELSE '' END) DirectFlg
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
                LEFT OUTER JOIN (SELECT D.StockNO, SUM(D.ReserveSU) AS ReserveSU
                				FROM D_Reserve AS D 
                                WHERE D.ReserveKBN = 2  --à⁄ìÆà¯ìñ
                                AND D.DeleteDateTime IS NULL
                                GROUP BY D.StockNO) AS W
                ON W.StockNO = DS.StockNO
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.ReserveKBN = 2	--à⁄ìÆà¯ìñ
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
                AND DP.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
                AND DS.ArrivalYetFLG = 1	--Åö
                AND DP.ArrivalPlanSu <> DS.ReserveSu + DS.InstructionSu + DS.ShippingSU
            ) AS DM
            ON DM.ArrivalPlanNO = DH.ArrivalPlanNO
        LEFT OUTER JOIN D_Stock AS DS
        ON DS.ArrivalPlanNO = DH.ArrivalPlanNO
        AND DS.DeleteDateTime IS NULL
        
        WHERE DH.SoukoCD = @SoukoCD
        AND DH.AdminNO = @AdminNO
        AND DH.LastestFLG = 1
        AND DH.ArrivalSu = 0
        AND DH.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
        AND DH.DeleteDateTime IS NULL
    
    ORDER BY KBN,JuchuuNO,JuchuuRows
    ;

END


GO


