BEGIN TRY 
 Drop Procedure D_Purchase_SelectForPrint
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE D_Purchase_SelectForPrint
(
    @PurchaseDateFrom  varchar(10),
    @PurchaseDateTo  varchar(10),
    @StoreCD  varchar(4),
    @VendorCD  varchar(13),
    @Flg1 tinyint,
    @Flg2 tinyint
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --D:\GITs\New_SMS\Database\StoreProcedure\D_Purchase_SelectForPrint.sql
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    SELECT DH.PurchaseNO
          ,DH.StoreCD
          ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
          ,DH.CancelFlg
          ,DH.ProcessKBN
          ,DH.ReturnsFlg
          ,DH.VendorCD
    --      ,(SELECT top 1 A.VendorName
    --      FROM M_Vendor A 
    --      WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
		  --AND A.VendorFlg = 1
    --      ORDER BY A.ChangeDate desc) 
    --      + (CASE ISNULL(DO.AliasKBN,0) WHEN 2 THEN ' 御中' ELSE '' END) AS VendorName 
			, (Case 
			 when Do.AliasKBN = 2 and DO.OrderPerSon is null then
			 (SELECT top 1 A.VendorName FROM M_Vendor A   WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate AND A.VendorFlg = 1   ORDER BY A.ChangeDate desc) + ' 御中'
			 when Do.AliasKBN = 2 and DO.OrderPerSon is not null then
		   (SELECT top 1 A.VendorName FROM M_Vendor A   WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate AND A.VendorFlg = 1   ORDER BY A.ChangeDate desc) 
			 when Do.AliasKBN <> 2 and DO.OrderPerSon is null then
		    (SELECT top 1 A.VendorName FROM M_Vendor A   WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate AND A.VendorFlg = 1   ORDER BY A.ChangeDate desc) + ' 様'
			when Do.AliasKBN <> 2 and DO.OrderPerSon is not null then
		      (SELECT top 1 A.VendorName FROM M_Vendor A   WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate AND A.VendorFlg = 1   ORDER BY A.ChangeDate desc)
			end) as VendorName
			,ISNULL(DO.OrderPerson,'') +(CASE ISNULL(DO.OrderPerson,0) WHEN 1 THEN ' 様' ELSE '' END) as OrderPerson
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
          ,
		  
		  --DM.ItemName as MakerItem
		  DM.MakerItem
		 -- (SELECT top 1 M.MakerItem 
           -- FROM M_SKU AS M 
           -- WHERE M.ChangeDate <= DH.PurchaseDate
            -- AND M.AdminNO = DM.AdminNO
            --  AND M.DeleteFlg = 0
            -- ORDER BY M.ChangeDate desc) AS MakerItem
          ,DA.VendorDeliveryNo
        --  ,ISNULL(DO.OrderPerson,'') + (CASE ISNULL(DO.AliasKBN,0) WHEN 1 THEN ' 様' ELSE '' END) AS OrderPerson

          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='301' AND A.[Key] = DH.StoreCD) AS Print1
          ,(SELECT A.Char2 FROM M_MultiPorpose A WHERE A.ID='301' AND A.[Key] = DH.StoreCD) AS Print2
          ,(SELECT A.Char3 FROM M_MultiPorpose A WHERE A.ID='301' AND A.[Key] = DH.StoreCD) AS Print3
          ,(SELECT A.Char4 FROM M_MultiPorpose A WHERE A.ID='301' AND A.[Key] = DH.StoreCD) AS Print4
          ,(SELECT A.ZipCD1 + '-' + A.ZipCD2 + ' ' + Address1 + ' ' + Address2 FROM M_Control AS A WHERE A.[MainKey] = 1) AS ZIP
          ,(SELECT A.CompanyName FROM M_Control AS A WHERE A.[MainKey] = 1) AS CompanyName
          ,(SELECT '℡' +' '+ A.TelephoneNO + ' FAX' +' '+ A.FaxNO FROM M_Control AS A WHERE A.[MainKey] = 1) AS TEL
          
      FROM D_Purchase DH
      INNER JOIN D_PurchaseDetails AS DM 
      ON DH.PurchaseNO = DM.PurchaseNO 
      --AND DM.PurchaserUnitPrice <> DM.OrderUnitPrice
      --AND DM.DifferenceFlg = 0
      AND DM.DeleteDateTime IS NULL
      LEFT OUTER JOIN D_Order AS DO
      ON DO.OrderNO = DM.OrderNO
      AND DO.DeleteDateTime IS NULL
      LEFT OUTER JOIN D_Arrival AS DA
      ON DA.ArrivalNO = DM.ArrivalNO
      AND DA.DeleteDateTime IS NULL
        
      WHERE DH.PurchaseDate >= (CASE WHEN @PurchaseDateFrom <> '' THEN CONVERT(DATE, @PurchaseDateFrom) ELSE DH.PurchaseDate END)
      AND DH.PurchaseDate <= (CASE WHEN @PurchaseDateTo <> '' THEN CONVERT(DATE, @PurchaseDateTo) ELSE DH.PurchaseDate END)
      AND DH.ProcessKBN = 1
      AND DH.StoreCD = @StoreCD
      AND DH.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DH.VendorCD END)
      AND ((@Flg1=1 AND DH.OutputDateTime IS NULL) OR (@Flg2=1 AND DH.OutputDateTime IS NOT NULL))
      AND DM.PurchaserUnitPrice <> DM.OrderUnitPrice
      AND DM.DifferenceFlg = 0
      ORDER BY VendorCD, PurchaseDate, DeliveryNo, MakerItem
      ;

END



GO
