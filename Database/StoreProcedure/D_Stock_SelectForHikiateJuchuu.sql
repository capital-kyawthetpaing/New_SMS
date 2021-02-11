IF OBJECT_ID ( 'D_Stock_SelectForHikiateJuchuu', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Stock_SelectForHikiateJuchuu]
GO

--  ======================================================================
--       Program Call    引当変更入力
--       Program ID      HikiateHenkouNyuuryoku
--       Create date:    2020.04.05
--    ======================================================================

--*********************************************--
--                                             --
--            受注/移動データ抽出              --
--                                             --
--*********************************************--
CREATE PROCEDURE D_Stock_SelectForHikiateJuchuu
    (@StoreCD               varchar(4), 
     @AdminNO               int,
     @OrderDateFrom         varchar(10),
     @OrderDateTo           varchar(10),
     @ArrivalPlanDateFrom   varchar(10),
     @ArrivalPlanDateTo     varchar(10),
     @ArrivalDateFrom       varchar(10),
     @ArrivalDateTo         varchar(10),
     @OrderCD               varchar(13),
     @OrderNO               varchar(11),
     @JuchuuDateFrom        varchar(10),
     @JuchuuDateTo          varchar(10),
     @CustomerCD            varchar(13),
     @JuchuuNO              varchar(11),
     @ChkNotReserve         varchar(1),
     @ChkReserved           varchar(1),
     @JuchuuKBN1            varchar(1),
     @JuchuuKBN2            varchar(1),
     @JuchuuKBN3            varchar(1),
     @JuchuuKBN4            varchar(1)
    )AS
BEGIN

    SET NOCOUNT ON;
    
     --在庫データ
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
    --受注データ
    , JUCHUU AS (
           SELECT DJD.JuchuuNO
                 ,DJD.JuchuuRows 
                 ,CONVERT(varchar,DJ.JuchuuDate,111) AS JuchuuDate
                 ,DJ.CustomerCD
                 ,DJ.CustomerName
                 ,DJD.JuchuuSuu
                 ,DJ.JuchuuKBN
                 ,DJD.SKUCD
                 ,DJD.AdminNo
                 ,DJD.JanCD
                 ,DR.ReserveSu AS AllReserveSu
                 ,DR.ShippingSu AS AllShippingSu    
             FROM D_JuchuuDetails DJD
             LEFT JOIN D_Juchuu AS DJ ON DJD.JuchuuNO = DJ.JuchuuNO
                                    AND DJ.DeleteDateTime IS NULL  
             LEFT OUTER JOIN ( SELECT [Number], NumberRows
                                     , SUM(ReserveSu) AS ReserveSu
                                     , SUM(ShippingSu) AS ShippingSu
                                  FROM D_Reserve 
                                 WHERE ReserveKBN = 1
                                   AND DeleteDateTime IS NULL
                                 GROUP BY [Number], NumberRows
                              ) DR ON DJD.JuchuuNO = DR.[Number]
                                  AND DJD.JuchuuRows = DR.NumberRows                       
             WHERE DJD.AdminNO = @AdminNO
               AND DJD.DeleteDateTime IS NULL 
               AND DJ.JuchuuDate >= (CASE WHEN ISNULL(@JuchuuDateFrom,'') <> '' THEN CONVERT(DATE, @JuchuuDateFrom) ELSE DJ.JuchuuDate END)
               AND DJ.JuchuuDate <= (CASE WHEN ISNULL(@JuchuuDateTo,'') <> '' THEN CONVERT(DATE, @JuchuuDateTo) ELSE DJ.JuchuuDate END) 
               AND DJ.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DJ.CustomerCD END)
               AND DJD.JuchuuNO = (CASE WHEN ISNULL(@JuchuuNO,'') <> '' THEN @JuchuuNO ELSE DJD.JuchuuNO END)
               AND ((@ChkNotReserve = '1' AND DJD.JuchuuSuu > ISNULL(DR.ReserveSu,0)) OR (@ChkReserved = '1' AND DJD.JuchuuSuu <= ISNULL(DR.ReserveSu,0)))
               AND DJD.JuchuuSuu > ISNULL(DR.ShippingSu,0)
               AND (DJ.JuchuuKBN = @JuchuuKBN1 OR DJ.JuchuuKBN = @JuchuuKBN2 OR DJ.JuchuuKBN = @JuchuuKBN3 OR DJ.JuchuuKBN = @JuchuuKBN4)
    )
    -- 移動データ
    , IDOU AS (
           SELECT DMD.MoveNO AS JuchuuNO
                 ,DMD.MoveRows AS JuchuuRows
                 ,DM.MoveDate AS JuchuuDate
                 ,DM.ToStoreCD AS CustomerCD
                 ,MS.SoukoName AS CustomerName
                 ,DMD.MoveSu AS JuchuuSuu
                 ,4 AS JuchuuKBN
                 ,DMD.SKUCD
                 ,DMD.AdminNo    
                 ,DMD.JanCD
                 ,DR.ReserveSu AS AllReserveSu
                 ,DR.ShippingSu AS AllShippingSu
            FROM D_MoveDetails AS DMD                                                                                            
            LEFT JOIN D_Move AS DM ON DMD.MoveNO = DM.MoveNO
                                  AND DM.DeleteDateTime IS NULL  
            LEFT JOIN M_Souko AS MS ON DM.ToSoukoCD = MS.SoukoCD
            LEFT OUTER JOIN ( SELECT [Number], NumberRows
                                    , SUM(ReserveSu) AS ReserveSu
                                    , SUM(ShippingSu) AS ShippingSu
                                FROM D_Reserve 
                                WHERE ReserveKBN = 2
                                    AND DeleteDateTime IS NULL
                                GROUP BY [Number], NumberRows
                            ) DR ON DMD.MoveNO = DR.[Number]
                                AND DMD.MoveRows = DR.NumberRows 
            WHERE DMD.AdminNO = @AdminNO
              AND DMD.DeleteDateTime IS NULL
              AND DM.MoveDate >= (CASE WHEN ISNULL(@JuchuuDateFrom,'') <> '' THEN CONVERT(DATE, @JuchuuDateFrom) ELSE DM.MoveDate END)
              AND DM.MoveDate <= (CASE WHEN ISNULL(@JuchuuDateTo,'') <> '' THEN CONVERT(DATE, @JuchuuDateTo) ELSE DM.MoveDate END) 
              --顧客CD、受注番号が指定された場合、移動からは抽出しない
              AND 0 = (CASE WHEN ISNULL(@CustomerCD,'') <> '' OR ISNULL(@JuchuuNO,'') <> '' THEN 1 ELSE 0 END)
              AND ((@ChkNotReserve = '1' AND DMD.MoveSu > ISNULL(DR.ReserveSu,0)) OR (@ChkReserved = '1' AND DMD.MoveSu <= ISNULL(DR.ReserveSu,0)))
              AND DMD.MoveSu > ISNULL(DR.ShippingSu,0)
              AND (4 = @JuchuuKBN4)
     )
    --メインSQL
    SELECT ROW_NUMBER() OVER (ORDER BY MEI.JuchuuDate,MEI.JuchuuNO,MEI.JuchuuRows) AS SEQ
          ,MEI.StockNO          
          ,MEI.ReserveNO
          ,MEI.ReserveKBN
          ,MEI.JuchuuNO
          ,MEI.JuchuuRows
          ,MEI.SoukoCD
          ,MEI.SKUCD
          ,MEI.AdminNO
          ,MEI.JanCD
          ,MEI.ReserveSu
          ,MEI.ReserveSu AS SelectReserveSu
          ,MEI.ShippingOrderNO
          ,MEI.ShippingOrderRows
          ,DID.InstructionSu
          ,MEI.ShippingSu
          ,MEI.ReturnKBN
          ,MEI.OriginalReserveNO
          ,CONVERT(varchar,MEI.JuchuuDate,111) AS JuchuuDate
          ,MEI.CustomerCD
          ,MEI.CustomerName
          ,MEI.JuchuuSuu
          ,MEI.AllReserveSu
          ,MEI.AllShippingSu
          ,MEI.JuchuuKBN
    FROM (
        SELECT ZAI.StockNO          
              ,RE.ReserveNO
              ,ISNULL(RE.ReserveKBN,1) AS ReserveKBN
              ,JUC.JuchuuNO
              ,JUC.JuchuuRows
              ,ZAI.SoukoCD
              ,JUC.SKUCD
              ,JUC.AdminNO
              ,JUC.JanCD
              ,ISNULL(RE.ReserveSu,0) AS ReserveSu
              ,ISNULL(RE.ReserveSu,0) AS SelectReserveSu
              ,RE.ShippingOrderNO
              ,ISNULL(RE.ShippingOrderRows,0) AS ShippingOrderRows
              ,ISNULL(RE.ShippingSu,0) AS ShippingSu
              ,ISNULL(RE.ReturnKBN,0) AS ReturnKBN
              ,RE.OriginalReserveNO
              ,CONVERT(varchar,JUC.JuchuuDate,111) AS JuchuuDate
              ,JUC.CustomerCD
              ,JUC.CustomerName
              ,JUC.JuchuuSuu
              ,ISNULL(JUC.AllReserveSu,0) AS AllReserveSu
              ,ISNULL(JUC.AllShippingSu,0) AS AllShippingSu
              ,JUC.JuchuuKBN
        FROM JUCHUU AS JUC
        LEFT JOIN ZAIKO AS ZAI ON JUC.AdminNO = ZAI.AdminNo 
        LEFT JOIN (
                    SELECT  DR.StockNO
                           ,DR.ReserveNO
                           ,DR.ReserveKBN
                           ,JUC.JuchuuNO
                           ,JUC.JuchuuRows
                           ,DR.ReserveSu
                           ,DR.ShippingOrderNO
                           ,DR.ShippingOrderRows
                           ,DR.ShippingSu
                           ,DR.ReturnKBN
                           ,DR.OriginalReserveNO
                      FROM JUCHUU AS JUC
                      LEFT OUTER JOIN D_Reserve AS DR ON JUC.JuchuuNO = DR.[Number]
                                                     AND JUC.JuchuuRows = DR.NumberRows
                                                     AND DR.StockNO IN ( SELECT StockNO FROM ZAIKO)
                                                     AND DR.ReserveKBN = 1
                                                     AND DR.DeleteDateTime IS NULL
                   ) RE ON JUC.JuchuuNO = RE.JuchuuNO
                       AND JUC.JuchuuRows = RE.JuchuuRows
                       AND ZAI.StockNO = RE.StockNO
        UNION ALL
        SELECT ZAI.StockNO          
              ,RE.ReserveNO
              ,ISNULL(RE.ReserveKBN,2) AS ReserveKBN
              ,IDO.JuchuuNO
              ,IDO.JuchuuRows
              ,ZAI.SoukoCD
              ,IDO.SKUCD
              ,IDO.AdminNO
              ,IDO.JanCD
              ,ISNULL(RE.ReserveSu,0) AS ReserveSu
              ,ISNULL(RE.ReserveSu,0) AS SelectReserveSu
              ,RE.ShippingOrderNO
              ,ISNULL(RE.ShippingOrderRows,0) AS ShippingOrderRows
              ,ISNULL(RE.ShippingSu,0) AS ShippingSu
              ,ISNULL(RE.ReturnKBN,0) AS ReturnKBN
              ,RE.OriginalReserveNO
              ,CONVERT(varchar,IDO.JuchuuDate,111) AS JuchuuDate
              ,IDO.CustomerCD
              ,IDO.CustomerName
              ,IDO.JuchuuSuu
              ,ISNULL(IDO.AllReserveSu,0) AS AllReserveSu
              ,ISNULL(IDO.AllShippingSu,0) AS AllShippingSu
              ,IDO.JuchuuKBN
        FROM IDOU AS IDO
        LEFT JOIN ZAIKO AS ZAI ON IDO.AdminNO = ZAI.AdminNo 
        LEFT JOIN (
                     SELECT  DR.StockNO
                            ,DR.ReserveNO
                            ,DR.ReserveKBN
                            ,IDO.JuchuuNO
                            ,IDO.JuchuuRows
                            ,DR.ReserveSu
                            ,DR.ShippingOrderNO
                            ,DR.ShippingOrderRows
                            ,DR.ShippingSu
                            ,DR.ReturnKBN
                            ,DR.OriginalReserveNO
                    FROM IDOU IDO
                    LEFT OUTER JOIN D_Reserve AS DR ON IDO.JuchuuNO = DR.[Number]
                                                   AND IDO.JuchuuRows = DR.NumberRows
                                                   AND DR.StockNO IN ( SELECT StockNO FROM ZAIKO)
                                                   AND DR.ReserveKBN = 2
                                                   AND DR.DeleteDateTime IS NULL
                  ) RE ON IDO.JuchuuNO = RE.JuchuuNO
                      AND IDO.JuchuuRows = RE.JuchuuRows
                      AND ZAI.StockNO = RE.StockNO
    ) MEI
     LEFT JOIN ( SELECT ReserveNO 
                       ,SUM(InstructionSu) AS InstructionSu
                   FROM D_InstructionDetails  
                  WHERE DeleteDateTime IS NULL
                  GROUP BY ReserveNO
               ) DID ON MEI.ReserveNO = DID.ReserveNO
     ORDER BY MEI.JuchuuDate
             ,MEI.JuchuuNO
             ,MEI.JuchuuRows


END

