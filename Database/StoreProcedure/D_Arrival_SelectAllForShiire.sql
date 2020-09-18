 BEGIN TRY 
 Drop Procedure dbo.[D_Arrival_SelectAllForShiire]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE D_Arrival_SelectAllForShiire
    (
    @ArrivalDateFrom  varchar(10),
    @ArrivalDateTo  varchar(10),
    @VendorCD  varchar(13),
    @StoreCD  varchar(4),
    @SoukoType tinyint,
    @DirectFLG tinyint,
    @OrderKbn tinyint	--Form.伝票番号順がONの時1,Form.入荷入力順がONの時2
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

	--画面項目転送表02（From D_Arrival When Click F11 Button）
    -- Insert statements for procedure here
    SELECT MAX(CONVERT(varchar,DH.ArrivalDate,111)) As ArrivalDate
          ,MAX(CASE WHEN DO.DirectFLG = 1 THEN '〇' ELSE '' END) AS DirectFLG
          ,DH.ArrivalNO
          ,MIN(DM.ArrivalRows) AS ArrivalRows
          ,MAX(DH.VendorDeliveryNo) AS DeliveryNo
          --,(SELECT top 1 M.MakerItem 
          --  FROM M_SKU AS M 
          --  WHERE M.ChangeDate <= DH.ArrivalDate
          --   AND M.AdminNO = DH.AdminNO
          --    AND M.DeleteFlg = 0
          --   ORDER BY M.ChangeDate desc) AS MakerItem
          ,MAX(DH.MakerItem) AS MakerItem
          ,MAX(DH.JanCD) AS JanCD
          ,(SELECT top 1 M.SKUName 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= MAX(DH.ArrivalDate)
                AND M.AdminNO = MAX(DH.AdminNO)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc) AS ItemName
          ,(SELECT top 1 M.ColorName 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= MAX(DH.ArrivalDate)
                AND M.AdminNO = MAX(DH.AdminNO)
                AND M.DeleteFlg = 0
               ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= MAX(DH.ArrivalDate)
                AND M.AdminNO = MAX(DH.AdminNO)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc) AS SizeName
          ,MAX(DO.CommentOutStore) AS D_CommentOutStore
          ,MAX(DO.CommentInStore) AS D_CommentInStore
          --,DH.ArrivalSu
          ,SUM(DM.ArrivalSu) AS ArrivalSu
          ,MAX(DO.OrderUnitPrice) AS DO_OrderUnitPrice
          --,DH.ArrivalSu - DH.PurchaseSu AS PurchaseSu
          ,SUM(DM.ArrivalSu - DM.PurchaseSu) AS PurchaseSu
          ,(SELECT top 1 M.TaniCD 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= MAX(DH.ArrivalDate)
                AND M.AdminNO = MAX(DH.AdminNO)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc) AS TaniCD
          ,(SELECT top 1 A.Char1 
                FROM M_SKU AS M 
                LEFT OUTER JOIN M_MultiPorpose AS A
                ON A.ID = 201
                AND A.[Key] = M.TaniCD 
                WHERE M.ChangeDate <= MAX(DH.ArrivalDate)
                AND M.AdminNO = MAX(DH.AdminNO)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc) AS TaniName
          ,MAX(DO.OrderUnitPrice) AS PurchaserUnitPrice
          --,(DH.ArrivalSu - DH.PurchaseSu)*DO.OrderUnitPrice AS D_CalculationGaku
          ,0 AS D_AdjustmentGaku
          --,(DH.ArrivalSu - DH.PurchaseSu)*DO.OrderUnitPrice AS D_PurchaseGaku
          
          ,DA.Number AS OrderNO
          ,DA.NumberRows AS OrderRows          
          ,MAX(DH.SKUCD) AS SKUCD
          ,MAX(DH.AdminNO) AS AdminNO
          ,MAX(DH.JANCD) AS JANCD
          ,(CASE WHEN @OrderKbn = 1 THEN '1' ELSE MAX(CONVERT(varchar,DH.ArrivalDate,111)) END) AS OrderKbn
          --,DH.PurchaseSu AS PurchaseZumiSu
          ,SUM(DM.PurchaseSu) AS PurchaseZumiSu

      FROM  D_Arrival AS DH
      INNER JOIN D_ArrivalDetails AS DM
      ON DH.ArrivalNO = DM.ArrivalNO
      AND DM.DeleteDateTime IS NULL
      INNER JOIN D_ArrivalPlan AS DA
      ON DA.ArrivalPlanNO = DM.ArrivalPlanNO
      AND DA.ArrivalPlanKBN = 1     --1:発注分、2:移動分
      AND DA.DeleteDateTime IS NULL
      INNER JOIN D_OrderDetails AS DO
      ON DO.OrderNO = DA.Number
      AND DO.OrderRows = DA.NumberRows
      AND DO.DeleteDateTime IS NULL
      
      LEFT OUTER JOIN  D_Delivery AS DD
      ON DD.VendorDeliveryNo = DH.VendorDeliveryNo
      AND DD.VendorCD = DH.VendorCD
      AND DD.JANCD = DH.JANCD
      AND DD.DeleteDateTime IS NULL
      
      WHERE DH.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DH.StoreCD END)
      AND DH.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DH.VendorCD END)
      AND DH.ArrivalKBN = 1     --

      AND ((DO.DirectFLG = 0
          AND DH.ArrivalDate >= (CASE WHEN @ArrivalDateFrom <> '' THEN CONVERT(DATE, @ArrivalDateFrom) ELSE DH.ArrivalDate END)
          AND DH.ArrivalDate <= (CASE WHEN @ArrivalDateTo <> '' THEN CONVERT(DATE, @ArrivalDateTo) ELSE DH.ArrivalDate END)
      ) OR  DO.DirectFLG = 1)
      
      AND (DO.DirectFLG = (CASE WHEN @DirectFLG=1 THEN @DirectFLG ELSE DO.DirectFLG END)
            OR EXISTS (SELECT 1 FROM M_Souko AS M WHERE M.SoukoCD = DH.SoukoCD
                    AND M.StoreCD = @StoreCD
                    AND M.ChangeDate <= DH.ArrivalDate      --Form.自社倉庫CheckBox=ONの時
                    AND M.SoukoType = (CASE WHEN @SoukoType = 3 THEN @SoukoType ELSE M.SoukoType END))
      )

      AND DH.ArrivalSu > DH.PurchaseSu
      AND DH.DeleteDateTime IS Null
      GROUP BY DH.ArrivalNO, DA.Number, DA.NumberRows
      ORDER BY OrderKbn desc, ArrivalNO, ArrivalRows
      ;
END

