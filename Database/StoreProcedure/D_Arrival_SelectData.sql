 BEGIN TRY 
 Drop Procedure dbo.[D_Arrival_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[D_Arrival_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
     @ArrivalNO varchar(11)
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

	--画面項目転送表01
    -- Insert statements for procedure here
        SELECT DH.ArrivalNO
              ,DH.VendorDeliveryNo
              ,DH.StoreCD
              ,DH.VendorCD
              ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                 WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.ArrivalDate
                   AND A.VendorFlg = 1
                 ORDER BY A.ChangeDate desc) AS VendorName 
              ,CONVERT(varchar,DH.ArrivalDate,111) AS ArrivalDate
              ,DH.InputDate
              ,DH.ArrivalKBN
              ,DH.StaffCD
              ,DH.SoukoCD
              ,DH.RackNO
              ,DH.JanCD
              ,DH.AdminNO
              ,DH.SKUCD
              ,DH.MakerItem
              ,(SELECT top 1 A.SKUName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                ORDER BY A.ChangeDate desc) AS SKUName
              ,DH.ArrivalSu
              ,DH.PurchaseSu
              ,(SELECT top 1 M.BrandName
                  FROM M_SKU A
                 INNER JOIN M_Brand AS M 
                    ON M.BrandCD = A.BrandCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS BrandName
              ,(SELECT top 1 A.ColorName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS ColorName
              ,(SELECT top 1 A.SizeName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS SizeName
              ,(SELECT top 1 M.Char1
                  FROM M_SKU A
                 INNER JOIN M_MultiPorpose AS M 
                    ON M.ID = 201
                   AND M.[Key] = A.TaniCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS TaniName
                  
              ,DH.InsertOperator
              ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
              ,DH.UpdateOperator
              ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
              ,DH.DeleteOperator
              ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
              
              --【引当】
              ,1 KBN
              ,DR.JuchuuNO
              ,DR.JuchuuRows
              ,DR.CustomerCD
              ,DR.CustomerName2
              ,NULL AS HachuSu
              ,DR.ReserveSu
              ,DR.DR_ArrivalSu
              ,DR.DirectFlg
              ,DR.DeliveryPlanDate
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
              
          FROM D_Arrival DH
          INNER JOIN 
                --一時ワークテーブル「D_Reserve①」(画面転送表01で「D_Reserve①」として使用)
                (SELECT DA.ArrivalNO
                        ,DJM.JuchuuNO
                        ,DJM.JuchuuRows
                        ,DJ.CustomerCD
                        ,ISNULL(DJ.CustomerName2,DJ.CustomerName) AS CustomerName2
                        ,DR.ReserveSu
                        ,DAM.ArrivalSu AS DR_ArrivalSu
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg
                              FROM M_SKU A 
                             WHERE A.AdminNO = DA.AdminNO AND A.ChangeDate <= DA.ArrivalDate 
                               AND A.DeleteFlg = 0
                             ORDER BY A.ChangeDate desc) AS DirectFlg
                        ,CONVERT(varchar,DD.DeliveryPlanDate,111) AS DeliveryPlanDate
                        ,DAM.ArrivalPlanNO		--★
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
          
                FROM D_Arrival AS DA
                LEFT OUTER JOIN D_ArrivalDetails AS DAM
                ON DAM.ArrivalNO = DA.ArrivalNO
                AND DAM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_ArrivalPlan AS DP
                ON DP.ArrivalPlanNO = DAM.ArrivalPlanNO
                AND DP.LastestFLG = 1
                AND DP.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Stock AS DS
                ON DS.ArrivalPlanNO = DP.ArrivalPlanNO
                AND DS.ArrivalYetFLG = 0
                AND DS.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.Number = DAM.Number		--7.16 add
                AND DR.ReserveNO IS NOT NULL
                AND DR.ReserveKBN = 1
                AND DR.DeleteDateTime IS NULL
                INNER JOIN D_JuchuuDetails AS DJM
                ON DJM.JuchuuNO = DR.Number
                AND DJM.JuchuuRows = DR.NumberRows
                AND DJM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Juchuu AS DJ
                ON DJ.JuchuuNO = DJM.JuchuuNO
                AND DJ.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_DeliveryPlan AS DD
                ON DD.DeliveryPlanNO = DJM.DeliveryPlanNO
                --AND DD.DeleteDateTime IS NULL		項目なし
                LEFT OUTER JOIN  D_OrderDetails AS DOM
                ON DOM.OrderNO = DP.Number
                AND DOM.OrderRows = DP.NumberRows
                AND DOM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Order AS DO
                ON DO.OrderNO = DOM.OrderNO
                AND DO.DeleteDateTime IS NULL
            
                WHERE DA.DeleteDateTime IS NULL
                AND DA.ArrivalNO = @ArrivalNO
            --  and dam.ArrivalRows = djm.JuchuuRows    --★仮にだけど必要
            ) AS DR
            ON DR.ArrivalNO = DH.ArrivalNO

          WHERE DH.ArrivalNO = @ArrivalNO 
	UNION ALL
        SELECT DH.ArrivalNO
              ,DH.VendorDeliveryNo
              ,DH.StoreCD
              ,DH.VendorCD
              ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                 WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.ArrivalDate
                   AND A.VendorFlg = 1
                 ORDER BY A.ChangeDate desc) AS VendorName 
              ,CONVERT(varchar,DH.ArrivalDate,111) AS ArrivalDate
              ,DH.InputDate
              ,DH.ArrivalKBN
              ,DH.StaffCD
              ,DH.SoukoCD
              ,DH.RackNO
              ,DH.JanCD
              ,DH.AdminNO
              ,DH.SKUCD
              ,DH.MakerItem
              ,(SELECT top 1 A.SKUName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS SKUName
              ,DH.ArrivalSu
              ,DH.PurchaseSu
              ,(SELECT top 1 M.BrandName
                  FROM M_SKU A
                 INNER JOIN M_Brand AS M 
                    ON M.BrandCD = A.BrandCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS BrandName

              ,(SELECT top 1 A.ColorName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS ColorName
              ,(SELECT top 1 A.SizeName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS SizeName
              ,(SELECT top 1 M.Char1
                  FROM M_SKU A
                 INNER JOIN M_MultiPorpose AS M 
                    ON M.ID = 201
                   AND M.[Key] = A.TaniCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS TaniName
                  
              ,DH.InsertOperator
              ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
              ,DH.UpdateOperator
              ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
              ,DH.DeleteOperator
              ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
              
              --【発注】
              ,2 KBN
              ,DO.OrderNO
              ,DO.OrderRows
              ,DO.OrderCD AS CustomerCD
              ,DO.VendorName_Souko
              ,DO.OrderSu AS HachuSu
              ,DO.ReserveSu AS ReserveSu
              ,DO.ArrivalSu AS DR_ArrivalSu
              ,DO.DirectFlg_Order
              ,DO.ArrivalPlanDate
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
              
          FROM D_Arrival DH
          INNER JOIN 
                --一時ワークテーブル「D_Order①」(画面転送表01で「D_Order①」として使用)
                (SELECT DA.ArrivalNO
                        ,DOM.OrderNO
                        ,DOM.OrderRows
                        ,DO.OrderCD
                        ,(SELECT top 1 A.VendorName
                            FROM M_Vendor A 
                           WHERE A.VendorCD = DO.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DO.OrderDate
                             AND A.VendorFlg = 1
                           ORDER BY A.ChangeDate desc) AS VendorName_Souko
                        --,DOM.OrderSu
                        ,DP.ArrivalPlanSu AS OrderSu
                        --,DS.ReserveSu
                        ,ISNULL(DS.ReserveSu + DS.InstructionSu + DS.ShippingSU,0) AS ReserveSu
                        --,DAM.ArrivalSu
                        ,DP.ArrivalSu
                        ,(CASE WHEN DOM.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg_Order
                        ,CONVERT(varchar,DP.ArrivalPlanDate,111) As ArrivalPlanDate
                        ,DAM.ArrivalPlanNO
                        ,DS.StockNO
                        ,DR.ReserveNO

                        ,DOM.OrderUnitPrice
                        ,DOM.PriceOutTax
                        ,DOM.Rate
                        ,DOM.TaniCD
                        ,DOM.OrderTaxRitsu
                        ,DO.OrderWayKBN
                        ,DO.AliasKBN
                                          
                FROM D_Arrival AS DA
                LEFT OUTER JOIN D_ArrivalDetails AS DAM
                ON DAM.ArrivalNO = DA.ArrivalNO
                AND DAM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_ArrivalPlan AS DP
                ON DP.ArrivalPlanNO = DAM.ArrivalPlanNO
                AND DP.LastestFLG = 1
                AND DP.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Stock AS DS
                ON DS.ArrivalPlanNO = DP.ArrivalPlanNO
                AND DS.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.Number = DAM.Number		--7.16 add
                AND DR.ReserveNO IS NOT NULL
                AND DR.ReserveKBN = 1
                AND DR.DeleteDateTime IS NULL
                INNER JOIN D_OrderDetails AS DOM
                ON DOM.OrderNO = DP.Number
                AND DOM.OrderRows = DP.NumberRows
                AND DOM.OrderNO = DAM.Number	--2020.12.2 add
                AND DOM.JuchuuNO IS NULL
                AND DOM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Order AS DO
                ON DO.OrderNO = DOM.OrderNO
                AND DO.DeleteDateTime IS NULL
                
                WHERE DA.DeleteDateTime IS NULL
                AND DA.ArrivalNO = @ArrivalNO
                AND DP.ArrivalPlanDate IS NOT NULL	--2020.09.16 add
                AND DOM.JuchuuNO IS NULL
                --and dam.ArrivalRows = DOM.OrderRows   --★仮にだけど必要
            ) AS DO
            ON DO.ArrivalNO = DH.ArrivalNO
          WHERE DH.ArrivalNO = @ArrivalNO 
    UNION ALL
        SELECT DH.ArrivalNO
              ,DH.VendorDeliveryNo
              ,DH.StoreCD
              ,DH.VendorCD
              ,(SELECT top 1 A.SoukoName
                  FROM M_Souko A 
                 WHERE A.SoukoCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.ArrivalDate
                 ORDER BY A.ChangeDate desc) AS VendorName 
              ,CONVERT(varchar,DH.ArrivalDate,111) AS ArrivalDate
              ,DH.InputDate
              ,DH.ArrivalKBN
              ,DH.StaffCD
              ,DH.SoukoCD
              ,DH.RackNO
              ,DH.JanCD
              ,DH.AdminNO
              ,DH.SKUCD
              ,DH.MakerItem
              ,(SELECT top 1 A.SKUName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS SKUName
              ,DH.ArrivalSu
              ,DH.PurchaseSu
              ,(SELECT top 1 M.BrandName
                  FROM M_SKU A
                 INNER JOIN M_Brand AS M 
                    ON M.BrandCD = A.BrandCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS BrandName

              ,(SELECT top 1 A.ColorName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS ColorName
              ,(SELECT top 1 A.SizeName
                  FROM M_SKU A 
                 WHERE A.AdminNO = DH.AdminNO AND A.ChangeDate <= DH.ArrivalDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS SizeName
              ,(SELECT top 1 M.Char1
                  FROM M_SKU A
                 INNER JOIN M_MultiPorpose AS M 
                    ON M.ID = 201
                   AND M.[Key] = A.TaniCD
                 WHERE A.AdminNO = DH.AdminNO 
                   AND A.ChangeDate <= DH.ArrivalDate
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate desc) AS TaniName
                  
              ,DH.InsertOperator
              ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
              ,DH.UpdateOperator
              ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
              ,DH.DeleteOperator
              ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
              
              --【移動】
              ,3 KBN
              ,DM.MoveNO
              ,DM.MoveRows
              ,DM.FromSoukoCD As CustomerCD
              ,DM.SoukoName
              ,DM.MoveSu AS HachuSu
              ,DM.ReserveSu AS ReserveSu
              ,DM.DM_ArrivalSu AS DR_ArrivalSu
              ,DM.DirectFlg_Move
              ,DM.ArrivalPlanDate_Move
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
              
          FROM D_Arrival DH
          INNER JOIN 
                --一時ワークテーブル「D_Move①」(画面転送表01で「D_Move①」として使用)
                (SELECT DA.ArrivalNO
                        ,DMM.MoveNO
                        ,DMM.MoveRows
                        ,DM.FromSoukoCD
                        ,(SELECT top 1 A.SoukoName
                            FROM M_Souko A 
                           WHERE A.SoukoCD = DM.FromSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DM.MoveDate
                           ORDER BY A.ChangeDate desc) AS SoukoName
                        --,DMM.MoveSu
                        ,DP.ArrivalPlanSu AS MoveSu
                        --,DS.ReserveSu
                        ,ISNULL(DS.ReserveSu + DS.InstructionSu + DS.ShippingSU,0) AS ReserveSu
                        --,DAM.ArrivalSu AS DM_ArrivalSu
                        ,DP.ArrivalSu AS DM_ArrivalSu
                          ,(SELECT top 1 (CASE WHEN A.DirectFlg = 1 THEN '〇' ELSE '' END) DirectFlg
                              FROM M_SKU A 
                             WHERE A.AdminNO = DA.AdminNO AND A.ChangeDate <= DA.ArrivalDate 
                               AND A.DeleteFlg = 0
                             ORDER BY A.ChangeDate desc) AS DirectFlg_Move
                        ,CONVERT(varchar,DP.ArrivalPlanDate,111) As ArrivalPlanDate_Move
                        ,DP.ArrivalPlanNO
                        ,DS.StockNO
                        ,DR.ReserveNO
                                                  
                FROM D_Arrival AS DA
                LEFT OUTER JOIN D_ArrivalDetails AS DAM
                ON DAM.ArrivalNO = DA.ArrivalNO
                AND DAM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_ArrivalPlan AS DP
                ON DP.ArrivalPlanNO = DAM.ArrivalPlanNO
                AND DP.LastestFLG = 1
                AND DP.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Stock AS DS
                ON DS.ArrivalPlanNO = DP.ArrivalPlanNO
                AND DS.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Reserve AS DR
                ON DR.StockNO = DS.StockNO
                AND DR.Number = DAM.Number		--7.16 add
                AND DR.ReserveNO IS NOT NULL
                AND DR.ReserveKBN = 2	--移動引当
                AND DR.DeleteDateTime IS NULL
                INNER JOIN D_MoveDetails AS DMM
                ON DMM.MoveNO = DP.Number
                AND DMM.MoveRows = DP.NumberRows
                AND DMM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Move AS DM
                ON DM.MoveNO = DMM.MoveNO
                AND DM.DeleteDateTime IS NULL
                
                WHERE DA.DeleteDateTime IS NULL
                AND DA.ArrivalNO = @ArrivalNO
                AND DP.ArrivalPlanDate IS NOT NULL  --2020.09.16 add
                --and dam.ArrivalRows = DMM.MoveRows    --★仮にだけど必要
            ) AS DM
            ON DM.ArrivalNO = DH.ArrivalNO
            
          WHERE DH.ArrivalNO = @ArrivalNO 
--              AND DH.DeleteDateTime IS Null
	
	ORDER BY KBN,ArrivalNO,JuchuuNO,JuchuuRows
            ;
--        END

END


