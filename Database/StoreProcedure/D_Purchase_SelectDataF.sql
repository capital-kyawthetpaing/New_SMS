 BEGIN TRY 
 Drop Procedure dbo.[D_Purchase_SelectDataF]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    仕入入力
--       Program ID      ShiireNyuuryokuF
--       Create date:    2019.11.20
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Purchase_SelectDataF]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @PurchaseNO varchar(11)
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

--        IF @OperateMode = 2   --修正時
--        BEGIN
            SELECT DH.PurchaseNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
                  ,DH.CancelFlg
                  ,DH.ProcessKBN
                  ,DH.ReturnsFlg
                  ,DH.VendorCD
                  ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                  WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
				  AND A.VendorFlg = 1
                  ORDER BY A.ChangeDate desc) AS VendorName 
                  ,DH.CalledVendorCD
                  ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                  WHERE A.VendorCD = DH.CalledVendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
                  ORDER BY A.ChangeDate desc) AS CalledVendorName 
                  ,DH.CalculationGaku
                  ,DH.AdjustmentGaku
                  ,DH.PurchaseGaku
                  ,DH.PurchaseTax
                  ,DH.TotalPurchaseGaku
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  ,DH.ExpectedDateFrom
                  ,DH.ExpectedDateTo
                  ,DH.InputDate
                  ,DH.StaffCD
                  ,CONVERT(varchar,DH.PaymentPlanDate,111) AS PaymentPlanDate
                  ,DH.PayPlanNO
                  ,DH.OutputDateTime
                  ,DH.StockAccountFlg
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.PurchaseRows
                  ,DM.DisplayRows
                  ,DM.ArrivalNO
                  ,DM.SKUCD
                  ,DM.AdminNO
                  ,DM.JanCD
                  ,DM.ItemName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.Remark
                  ,DM.PurchaseSu
                  ,DM.TaniCD
                  ,DM.TaniName
                  ,DM.PurchaserUnitPrice
                  ,DM.CalculationGaku AS D_CalculationGaku
                  ,DM.AdjustmentGaku As D_AdjustmentGaku
                  ,DM.PurchaseGaku AS D_PurchaseGaku
                  ,DM.PurchaseTax AS D_PurchaseTax
                  ,DM.TaxRitsu
                  ,DM.TotalPurchaseGaku AS D_TotalPurchaseGaku
                  ,DM.CurrencyCD
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore
                  ,DM.ReturnNO
                  ,DM.ReturnRows
                  ,DM.OrderUnitPrice
                  ,DM.OrderNO
                  ,DM.OrderRows
                  ,DM.DifferenceFlg
                  ,DM.DeliveryNo
                  ,CONVERT(varchar,DA.ArrivalDate,111) AS ArrivalDate
                  ,(CASE WHEN DO.DirectFLG = 1 THEN '〇' ELSE '' END) AS DirectFLG
                  ,DA.ArrivalSu
                  ,DO.OrderUnitPrice AS DO_OrderUnitPrice
                  ,(SELECT top 1 M.MakerItem 
                    FROM M_SKU AS M 
                    WHERE M.ChangeDate <= DH.PurchaseDate
                     AND M.AdminNO = DM.AdminNO
                      AND M.DeleteFlg = 0
                     ORDER BY M.ChangeDate desc) AS MakerItem
                     
              FROM D_Purchase DH
              LEFT OUTER JOIN D_PurchaseDetails AS DM 
              ON DH.PurchaseNO = DM.PurchaseNO 
              AND DM.DeleteDateTime IS NULL
           	  LEFT OUTER JOIN D_Arrival AS DA
           	  ON DA.ArrivalNO = DM.ArrivalNO
              AND DA.DeleteDateTime IS NULL
              LEFT OUTER JOIN D_OrderDetails AS DO
              ON DO.OrderNO = DM.OrderNO
              AND DO.OrderRows = DM.OrderRows
              AND DO.DeleteDateTime IS NULL
              WHERE DH.PurchaseNO = @PurchaseNO 
              AND DH.ProcessKBN = 1
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.PurchaseNO, DM.DisplayRows
                ;
--        END

END


