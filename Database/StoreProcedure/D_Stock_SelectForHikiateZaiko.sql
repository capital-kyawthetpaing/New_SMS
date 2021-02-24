IF OBJECT_ID ( 'D_Stock_SelectForHikiateZaiko', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Stock_SelectForHikiateZaiko]
GO

--  ======================================================================
--       Program Call    引当変更入力
--       Program ID      HikiateHenkouNyuuryoku
--       Create date:    2020.04.05
--    ======================================================================


--****************************************--
--                                        --
--            在庫データ抽出              --
--                                        --
--****************************************--
CREATE PROCEDURE D_Stock_SelectForHikiateZaiko
    (@StoreCD               varchar(4), 
     @AdminNO               int,
     @OrderDateFrom         varchar(10),
     @OrderDateTo           varchar(10),
     @ArrivalPlanDateFrom   varchar(10),
     @ArrivalPlanDateTo     varchar(10),
     @ArrivalDateFrom       varchar(10),
     @ArrivalDateTo         varchar(10),
     @OrderCD               varchar(13),
     @OrderNO               varchar(11)
    )AS
BEGIN

    SET NOCOUNT ON;
    
    WITH ZAIKO AS (
        SELECT *
          FROM (
                SELECT  DST.StockNO
                       ,DST.SoukoCD
                       ,(SELECT top 1 A.SoukoName
                           FROM M_Souko A 
                          WHERE A.SoukoCD = DST.SoukoCD 
                            AND A.ChangeDate <= GETDATE() 
                            AND A.DeleteFlg = 0
                          ORDER BY A.ChangeDate desc) AS SoukoName
                       ,DST.ArrivalPlanNO
                       ,DST.SKUCD
                       ,DST.AdminNO
                       ,DST.JanCD
                       ,DST.ArrivalYetFLG
                       ,(CASE DST.ArrivalPlanKBN WHEN 3 THEN DM.MoveDate WHEN 4 THEN NULL ELSE DO.OrderDate END) AS OrderDate
                       ,DST.ArrivalPlanDate
                       ,DST.ArrivalDate
                       ,DST.StockSu
                       ,DST.PlanSu
                       ,DST.AllowableSu
                       ,DST.AnotherStoreAllowableSu
                       ,DST.ReserveSu
                       ,DST.ReserveSu AS SelectReserveSu
                       ,DST.InstructionSu
                       ,DST.ArrivalPlanKBN
                       ,(CASE DST.ArrivalPlanKBN WHEN 3 THEN DM.MoveNO WHEN 4 THEN NULL ELSE DO.OrderNO END) AS OrderNO
                       ,(CASE DST.ArrivalPlanKBN WHEN 3 THEN DM.StoreCD WHEN 4 THEN DP.VendorCD ELSE DO.OrderCD END) AS OrderCD
                       ,(CASE DST.ArrivalPlanKBN WHEN 3 THEN 
                             (SELECT top 1 A.StoreName
                               FROM M_Store A 
                              WHERE A.StoreCD = DM.StoreCD 
                                AND A.ChangeDate <= GETDATE() 
                                AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc)
                         ELSE 
                             (SELECT top 1 A.VendorName
                               FROM M_Vendor A 
                              WHERE A.VendorCD = (CASE WHEN DST.ArrivalPlanKBN = 4 THEN DP.VendorCD ELSE DO.OrderCD END)
                                AND A.ChangeDate <= GETDATE() 
                                AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc)
                         END) AS VendorName
                FROM D_Stock AS DST
                INNER JOIN F_Souko(GETDATE()) MS ON MS.StoreCD = @StoreCD AND MS.DeleteFlg = 0 AND MS.SoukoCD = DST.SoukoCD
                LEFT OUTER JOIN D_ArrivalPlan AS DAP ON DST.ArrivalPlanNO = DAP.ArrivalPlanNO
                                                    AND DAP.LastestFLG = 1
                                                    AND DAP.DeleteDateTime IS NULL                                                     
                LEFT OUTER JOIN D_OrderDetails AS DOD ON DAP.[Number] = DOD.OrderNO
                                                     AND DAP.NumberRows = DOD.OrderRows
                                                     AND DAP.ArrivalPlanKBN = 1
                                                     AND DOD.DeleteDateTime IS NULL                                                          
                LEFT OUTER JOIN D_Order AS DO ON DOD.OrderNO = DO.OrderNO
                                             AND DO.DeleteDateTime IS NULL  
                LEFT OUTER JOIN D_MoveDetails AS DMM ON DAP.[Number] = DMM.MoveNO
                                                    AND DAP.NumberRows = DMM.MoveRows
                                                    AND DAP.ArrivalPlanKBN = 2
                                                    AND DMM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Move AS DM ON DMM.MoveNO = DM.MoveNO
                                            AND DM.DeleteDateTime IS NULL     
                LEFT OUTER JOIN D_PurchaseDetails AS DPM ON DST.StockNO = DPM.StockNO
                                                        AND DST.ArrivalPlanKBN = 4
                                                        AND DPM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Purchase AS DP ON DPM.PurchaseNO = DP.PurchaseNO
                                                AND DP.DeleteDateTime IS NULL     
                WHERE DST.DeleteDateTime IS NULL
                  AND DST.AdminNO = @AdminNO
              ) ZAI
            WHERE ISNULL(ZAI.OrderDate,'') >= (CASE WHEN ISNULL(@OrderDateFrom,'') <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE ISNULL(ZAI.OrderDate,'') END)
              AND ISNULL(ZAI.OrderDate,'') <= (CASE WHEN ISNULL(@OrderDateTo,'') <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE ISNULL(ZAI.OrderDate,'') END)               
              AND ((ISNULL(ZAI.ArrivalPlanDate,'') >= (CASE WHEN ISNULL(@ArrivalPlanDateFrom,'') <> '' THEN CONVERT(DATE, @ArrivalPlanDateFrom) ELSE ISNULL(ZAI.ArrivalPlanDate,'') END)
                   AND ISNULL(ZAI.ArrivalPlanDate,'') <= (CASE WHEN ISNULL(@ArrivalPlanDateTo,'') <> '' THEN CONVERT(DATE, @ArrivalPlanDateTo) ELSE ISNULL(ZAI.ArrivalPlanDate,'') END)
                   AND ZAI.ArrivalYetFLG = (CASE WHEN ISNULL(@ArrivalDateFrom,'') <> '' OR ISNULL(@ArrivalDateTo,'') <> '' THEN 1 ELSE ZAI.ArrivalYetFLG END)
                   --入荷日のみが指定されている場合、入荷予定日の条件は無効とする
                   AND 0 = (CASE WHEN ISNULL(@ArrivalPlanDateFrom,'') = '' AND ISNULL(@ArrivalPlanDateTo,'') = '' AND (ISNULL(@ArrivalDateFrom,'') <> '' OR ISNULL(@ArrivalDateTo,'') <> '') THEN 1 ELSE 0 END)
                   )
               OR (ISNULL(ZAI.ArrivalDate,'') >= (CASE WHEN ISNULL(@ArrivalDateFrom,'') <> '' THEN CONVERT(DATE, @ArrivalDateFrom) ELSE ISNULL(ZAI.ArrivalDate,'') END)
                   AND ISNULL(ZAI.ArrivalDate,'') <= (CASE WHEN ISNULL(@ArrivalDateTo,'') <> '' THEN CONVERT(DATE, @ArrivalDateTo) ELSE ISNULL(ZAI.ArrivalDate,'') END)
                   AND ZAI.ArrivalYetFLG = (CASE WHEN ISNULL(@ArrivalDateFrom,'') <> '' OR ISNULL(@ArrivalDateTo,'') <> '' THEN 0 ELSE ZAI.ArrivalYetFLG END)
                   --入荷予定日のみが指定されている場合、入荷日の条件は無効とする
                   AND 0 = (CASE WHEN ISNULL(@ArrivalDateFrom,'') = '' AND ISNULL(@ArrivalDateTo,'') = '' AND (ISNULL(@ArrivalPlanDateFrom,'') <> '' OR ISNULL(@ArrivalPlanDateTo,'') <> '') THEN 1 ELSE 0 END)
                  ))              
              AND ISNULL(ZAI.OrderCD,'') = (CASE WHEN ISNULL(@OrderCD,'') <> '' THEN @OrderCD ELSE ISNULL(ZAI.OrderCD,'') END)
              AND ISNULL(ZAI.OrderNO,'') = (CASE WHEN ISNULL(@OrderNO,'') <> '' THEN @OrderNO ELSE ISNULL(ZAI.OrderNO,'') END)
              AND (((ISNULL(@OrderCD,'') <> '' or ISNULL(@OrderNO,'') <> '') AND ISNULL(ZAI.ArrivalPlanKBN,0) <> 3) OR (0 = 0))
    )
    SELECT ZAI.StockNO
          ,ZAI.SoukoCD
          ,ZAI.SoukoName
          ,ZAI.ArrivalPlanNO
          ,ZAI.SKUCD
          ,ZAI.AdminNO
          ,ZAI.JanCD
          ,ZAI.ArrivalYetFLG
          ,CONVERT(varchar,ZAI.OrderDate,111) AS OrderDate
          ,CONVERT(varchar,ZAI.ArrivalPlanDate,111) AS ArrivalPlanDate
          ,CONVERT(varchar,ZAI.ArrivalDate,111) AS ArrivalDate
          ,ZAI.StockSu
          ,ZAI.PlanSu
          ,ZAI.AllowableSu
          ,ZAI.AllowableSu AS SelectAllowableSu
          ,ZAI.AnotherStoreAllowableSu
          ,ZAI.ReserveSu
          ,ZAI.ReserveSu AS SelectReserveSu
          ,ZAI.InstructionSu
          ,ZAI.ArrivalPlanKBN
          ,ZAI.OrderNO
          ,ZAI.OrderCD
          ,ZAI.VendorName
    FROM ZAIKO AS ZAI    
    ORDER BY ISNULL(ZAI.ArrivalDate, ZAI.ArrivalPlanDate)
            ,ZAI.StockNO   
    ;

END

