 BEGIN TRY 
 Drop Procedure dbo.[D_Move_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  ======================================================================
--       Program Call    在庫移動入力
--       Program ID      ZaikoIdouNyuuryoku
--       Create date:    2019.11.20
--    ======================================================================

CREATE PROCEDURE [dbo].[D_Move_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @MoveNO varchar(11)
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
    SELECT DH.MoveNO
          ,DH.StoreCD
          ,CONVERT(varchar,DH.MoveDate,111) AS MoveDate
          ,DH.RequestNO
          ,(SELECT top 1 A.StoreCD 
              FROM M_Souko A 
              WHERE A.SoukoCD = DH.FromSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MoveDate
              ORDER BY A.ChangeDate desc) AS FromStoreCD
          ,DH.MovePurposeKBN
          ,DH.FromSoukoCD
          ,DH.ToStoreCD
          ,DH.ToSoukoCD
          ,DH.MoveInputDateTime
          ,DH.StaffCD
          ,DH.InsertOperator
          ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
          ,DH.UpdateOperator
          ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
          ,DH.DeleteOperator
          ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
          
          
          ,DM.MoveRows
          ,DM.SKUCD
          ,DM.AdminNO
          ,DM.JanCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
          
          ,DM.MoveSu
          ,DRM.RequestSu
          ,DM.EvaluationPrice
          ,DM.FromRackNO
          ,DM.ToRackNO
          ,DM.NewJanCD
          ,DM.NewAdminNO
          ,DM.NewSKUCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.NewAdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS NewSKUName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.NewAdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS NewColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.NewAdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS NewSizeName
          
          ,DM.DeliveryPlanNO
          ,CONVERT(varchar,DM.ExpectReturnDate,111) AS ExpectReturnDate
          ,DM.VendorCD
          ,DM.CommentInStore
          ,ISNULL(DM.RequestRows,0) AS RequestRows
          ,DM.TotalArrivalSu
          
          ,ISNULL(DRM.AnswerKBN,0) AS AnswerKBN
          ,CONVERT(varchar,DRM.ExpectedDate,111) AS ExpectedDate

          --回答済件数
          ,SUM(CASE WHEN ISNULL(DRM.AnswerKBN,0) > 0 THEN 1 ELSE 0 END) OVER() AS CNT_Ans
          --未回答件数
          ,SUM(CASE WHEN ISNULL(DRM.AnswerKBN,0) = 0 THEN 1 ELSE 0 END) OVER() AS CNT_UnAns

/*          ,(SELECT top 1 DW.WarehousingNO FROM D_Warehousing AS DW
            WHERE DW.Number = DM.MoveNO
            AND DW.NumberRow = DM.MoveRows
            AND DW.WarehousingKBN IN (11,13)
            AND DW.DeleteDateTime IS NULL
            ORDER BY DW.WarehousingNO desc ) AS WarehousingNO            
          ,(SELECT top 1 DW.StockNO FROM D_Warehousing AS DW
            WHERE DW.Number = DM.MoveNO
            AND DW.NumberRow = DM.MoveRows
            AND DW.WarehousingKBN IN (11,13)
            AND DW.DeleteDateTime IS NULL
            ORDER BY DW.WarehousingNO desc ) AS StockNO
          ,(SELECT top 1 DW.ToStockNO FROM D_Warehousing AS DW
            WHERE DW.Number = DM.MoveNO
            AND DW.NumberRow = DM.MoveRows
            AND DW.WarehousingKBN IN (11,13)
            AND DW.DeleteDateTime IS NULL
            ORDER BY DW.WarehousingNO desc ) AS ToStockNO*/
          ,(SELECT top 1 DR.ReserveNO FROM D_Warehousing AS DW
            INNER JOIN D_Reserve AS DR
            ON DR.StockNO = DW.StockNO
            AND DR.DeleteDateTime IS NULL
            WHERE DW.Number = DM.MoveNO
            AND DW.NumberRow = DM.MoveRows
            AND DW.WarehousingKBN IN (11,13)
            AND DW.DeleteDateTime IS NULL
            ORDER BY DW.WarehousingNO desc ) AS ReserveNO
          ,(SELECT DD.DeliveryPlanNO FROM D_DeliveryPlanDetails  AS DD
            WHERE DD.Number = DM.MoveNO
            AND DD.NumberRows = DM.MoveRows ) AS DeliveryPlanNO
          ,(SELECT DD.DeliveryPlanRows FROM D_DeliveryPlanDetails    AS DD
            WHERE DD.Number = DM.MoveNO
            AND DD.NumberRows = DM.MoveRows ) AS DeliveryPlanRows
          ,(SELECT DA.ArrivalPlanNO FROM D_ArrivalPlan AS DA
          	WHERE DA.Number = DM.MoveNO
          	AND DA.NumberRows = DM.MoveRows
          	AND DA.DeleteDateTime IS NULL) AS ArrivalPlanNO
      FROM D_Move DH
      LEFT OUTER JOIN D_MoveDetails AS DM 
      ON DH.MoveNO = DM.MoveNO 
      AND DM.DeleteDateTime IS NULL                     
      LEFT OUTER JOIN D_MoveRequest AS DR
      ON DR.RequestNO = DH.RequestNO
      AND DR.DeleteDateTime IS NULL
      LEFT OUTER JOIN D_MoveRequestDetailes AS DRM
      ON DRM.RequestNO = DH.RequestNO
      AND DRM.RequestRows = DM.RequestRows
      AND DRM.DeleteDateTime IS NULL
      
      WHERE DH.MoveNO = @MoveNO
--              AND DH.DeleteDateTime IS Null
      ORDER BY DH.MoveNO, DM.MoveRows
      ;
      
END


