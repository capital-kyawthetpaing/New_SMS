 BEGIN TRY 
 Drop Procedure dbo.[D_Juchuu_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  ======================================================================
--       Program Call    受注入力
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE D_Juchuu_SelectData
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO varchar(11)
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
            SELECT DH.JuchuuNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
                  ,DH.JuchuuTime
                  ,DH.ReturnFLG
                  ,DH.SoukoCD 
                  ,DH.JuchuuKBN
                  ,DH.SiteKBN
                  ,CONVERT(varchar,DH.SiteJuchuuDateTime,111) AS SiteJuchuuDateTime
                  ,DH.SiteJuchuuNO
                  ,DH.InportErrFLG
                  ,DH.OnHoldFLG
                  ,DH.IdentificationFLG
                  ,CONVERT(varchar,DH.TorikomiDateTime,111) AS TorikomiDateTime
                  ,DH.StaffCD
                  ,DH.CustomerCD
                  ,DH.CustomerName
                  ,DH.CustomerName2
                  ,DH.AliasKBN
                  ,DH.ZipCD1
                  ,DH.ZipCD2
                  ,DH.Address1
                  ,DH.Address2
                  ,DH.Tel11
                  ,DH.Tel12
                  ,DH.Tel13
                  ,DH.Tel21
                  ,DH.Tel22
                  ,DH.Tel23
                  ,DH.CustomerKanaName
                  ,DH.DeliveryCD
                  ,DH.DeliveryName
                  ,DH.DeliveryName2
                  ,DH.DeliveryAliasKBN
                  ,DH.DeliveryZipCD1
                  ,DH.DeliveryZipCD2
                  ,DH.DeliveryAddress1
                  ,DH.DeliveryAddress2
                  ,DH.DeliveryTel11
                  ,DH.DeliveryTel12
                  ,DH.DeliveryTel13 
                  ,DH.JuchuuCarrierCD
                  ,DH.DecidedCarrierFLG
                  ,DH.LastCarrierCD
                  ,CONVERT(varchar,DH.NameSortingDateTime,111) AS NameSortingDateTime
                  ,DH.NameSortingStaffCD
                  ,DH.CurrencyCD
                  ,DH.JuchuuGaku AS SUM_JuchuuGaku
                  ,DH.Discount
                  ,DH.HanbaiHontaiGaku
                  ,DH.HanbaiTax8
                  ,DH.HanbaiTax10
                  ,DH.HanbaiGaku
                  ,DH.CostGaku AS SUM_CostGaku
                  ,DH.ProfitGaku AS SUM_ProfitGaku
                  ,DH.Coupon
                  ,DH.Point
                  ,DH.PayCharge
                  ,DH.Adjustments
                  ,DH.Postage
                  ,DH.GiftWrapCharge
                  ,DH.InvoiceGaku
                  ,DH.PaymentMethodCD
                  ,DH.PaymentPlanNO
                  ,DH.CardProgressKBN
                  ,DH.CardCompany
                  ,DH.CardNumber
                  ,DH.PaymentProgressKBN
                  ,DH.PresentFLG
                  ,CONVERT(varchar,DH.SalesPlanDate,111) AS SalesPlanDate
                  ,CONVERT(varchar,DH.FirstPaypentPlanDate,111) AS FirstPaypentPlanDate
                  ,CONVERT(varchar,DH.LastPaymentPlanDate,111) AS LastPaymentPlanDate
                  ,DH.DemandProgressKBN
                  ,DH.CommentDemand
                  ,CONVERT(varchar,DH.CancelDate,111) AS CancelDate
                  ,DH.CancelReasonKBN
                  ,DH.CancelRemarks
                  ,DH.NoMailFLG
                  ,DH.IndividualContactKBN
                  ,DH.TelephoneContactKBN
                  ,DH.LastMailKBN
                  ,DH.LastMailPatternCD
                  ,CONVERT(varchar,DH.LastMailDatetime,111) AS LastMailDatetime
                  ,DH.LastMailName
                  ,DH.NextMailKBN
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  ,CONVERT(varchar,DH.LastDepositeDate,111) AS LastDepositeDate
                  ,CONVERT(varchar,DH.LastOrderDate,111) AS LastOrderDate
                  ,CONVERT(varchar,DH.LastArriveDate,111) AS LastArriveDate
                  ,CONVERT(varchar,DH.LastSalesDate,111) AS LastSalesDate
                  ,DH.MitsumoriNO
                  ,CONVERT(varchar,DH.JuchuuDateTime,111) AS JuchuuDateTime
                  
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.JuchuuRows
                  ,DM.DisplayRows
                  ,DM.SiteJuchuuRows
                  ,DM.AdminNO AS SKUNO
                  ,DM.SKUCD
                  ,DM.JanCD
                  ,DM.SKUName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.NotPrintFLG
                  ,DM.NotOrderFLG
                  ,DM.DirectFLG
                  ,DM.SetKBN
                  ,DM.SetRows
                  ,DM.JuchuuSuu
                  ,DM.JuchuuUnitPrice
                  ,DM.TaniCD
                  ,DM.JuchuuGaku
                  ,DM.JuchuuHontaiGaku
                  ,DM.JuchuuTax
                  ,DM.JuchuuTaxRitsu
                  ,DM.CostUnitPrice
                  ,DM.CostGaku
                  ,DM.ProfitGaku
                  ,DM.SoukoCD AS M_SoukoCD
                  ,DM.HikiateSu
                  ,DM.DeliveryOrderSu
                  ,DM.DeliverySu
                  ,DM.HikiateFLG
                  ,(SELECT DO.OrderNO 
                  	FROM D_OrderDetails AS DO 
                    WHERE DO.OrderNO = DM.LastOrderNO
                    AND DO.OrderRows = DM.LastOrderRows
                    AND DO.DeleteDateTime IS NULL
                  ) AS JuchuuOrderNO
                  ,DM.VendorCD
                  ,DM.LastOrderNO
                  ,DM.LastOrderRows
                  ,CONVERT(varchar,DM.LastOrderDateTime,111) AS LastOrderDateTime
                  ,CONVERT(varchar,DM.DesiredDeliveryDate,111) AS DesiredDeliveryDate
                  ,DM.AnswerFLG
                  ,(SELECT ISNULL(CONVERT(varchar,DO.ArrivePlanDate,111), CONVERT(varchar,DM.ArrivePlanDate,111))
                    FROM D_OrderDetails AS DO 
                    WHERE DO.OrderNO = DM.LastOrderNO
                    AND DO.OrderRows = DM.LastOrderRows
                    AND DO.DeleteDateTime IS NULL
                  ) AS ArrivePlanDate
                  
                  ,(SELECT CONVERT(varchar,DP.PaymentPlanDate,111) 
                    FROM D_PurchaseDetails AS DPD 
                    LEFT OUTER JOIN D_Purchase AS DP 
                    ON DP.PurchaseNO = DPD.PurchaseNO
                    AND DP.DeleteDateTime IS NULL
                    WHERE DPD.OrderNO = DM.LastOrderNO
                    AND DPD.OrderRows = DM.LastOrderRows 
                    AND DPD.DeleteDateTime IS NULL
                  )AS PaymentPlanDate	--明細部.支払予定日

                  ,CONVERT(varchar,DPC.CollectClearDate,111) AS D_CollectClearDate	--明細部.入金日

                  ,DM.ArrivePlanNO
                  ,CONVERT(varchar,DM.ArriveDateTime,111) AS ArriveDateTime
                  ,DM.ArriveNO
                  ,DM.ArribveNORows
                  ,DM.DeliveryPlanNO
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore
                  ,DM.IndividualClientName
                  ,CONVERT(varchar,DM.SalesDate,111) AS SalesDate
                  ,DM.SalesNO
                  ,DM.DepositeDetailNO
                  
                  --【Data Area Footer】
                  ,DS.NouhinsyoComment
                  ,MAX(CONVERT(varchar,A.SalesDate,111)) OVER() AS D_Sales_SalesDate	--売上日
                  ,SUM(B.CollectGaku) OVER() AS CollectGaku	--入金額
                  ,MAX(CONVERT(varchar,C.BillingCloseDate,111)) OVER() AS BillingCloseDate	--請求締日
                  ,MAX(CONVERT(varchar,C.CollectPlanDate,111)) OVER() AS CollectPlanDate	--（請求時）入金予定日
                  ,MAX(CONVERT(varchar,D.CollectClearDate,111)) OVER() AS CollectClearDate --最終入金日

              FROM D_Juchuu DH

              LEFT OUTER JOIN D_JuchuuDetails AS DM ON DH.JuchuuNO = DM.JuchuuNO AND DM.DeleteDateTime IS NULL
              LEFT OUTER JOIN D_StoreJuchuu AS DS ON DH.JuchuuNO = DS.JuchuuNO
              LEFT OUTER JOIN (
                SELECT M.JuchuuNO,M.JuchuuRows, MAX(H.SalesDate) AS SalesDate 
                  FROM D_SalesDetails AS M
                  INNER JOIN D_Sales AS H ON H.SalesNO = M.SalesNO AND H.DeleteDateTime IS NULL
                    WHERE M.DeleteDateTime IS NULL
                    GROUP BY M.JuchuuNO,M.JuchuuRows
                  )AS A ON A.JuchuuNO = DM.JuchuuNO AND A.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN(
--                SELECT M.JuchuuNO,M.JuchuuRows,SUM(H.CollectGaku) AS CollectGaku	2019.10.23 chg
                SELECT M.JuchuuNO,M.JuchuuRows,SUM(H.CollectAmount) AS CollectGaku
                FROM D_CollectBilling AS H
                INNER JOIN D_CollectPlanDetails AS M ON M.CollectPlanNO = H.CollectPlanNO
                AND H.DeleteDateTime IS NULL
                WHERE M.DeleteDateTime IS NULL
                GROUP BY M.JuchuuNO,M.JuchuuRows
                ) AS B ON B.JuchuuNO = DM.JuchuuNO
              AND B.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN (
                  SELECT M.JuchuuNO,M.JuchuuRows,Max(H.BillingCloseDate) AS BillingCloseDate
                  ,Max(H.CollectPlanDate) AS CollectPlanDate
                  FROM D_Billing AS H
                  INNER JOIN D_BillingDetails AS HM ON HM.BillingNO = H.BillingNO
                  AND HM.DeleteDateTime IS NULL
                  INNER JOIN D_CollectPlanDetails M ON M.CollectPlanNO = HM.CollectPlanNO
                  AND M.CollectPlanRows = HM.CollectPlanRows
                  AND M.DeleteDateTime IS NULL
                  WHERE H.DeleteDateTime IS NULL
                  GROUP BY M.JuchuuNO,M.JuchuuRows
              ) AS C ON C.JuchuuNO = DM.JuchuuNO
              AND C.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN (
                  SELECT DCB.JuchuuNO,DCB.JuchuuRows,Max(DPC.CollectClearDate) AS CollectClearDate
                    FROM D_CollectPlanDetails AS DCB
                  LEFT OUTER JOIN D_CollectBilling AS DCB2 ON DCB2.CollectPlanNO = DCB.CollectPlanNO
                    AND DCB2.DeleteDateTime IS NULL
                      LEFT OUTER JOIN D_PaymentConfirm AS DPC ON DPC.ConfirmNO = DCB2.ConfirmNO 
		                AND DPC.DeleteDateTime IS NULL
                  WHERE DCB.DeleteDateTime IS NULL
                  GROUP BY DCB.JuchuuNO,DCB.JuchuuRows
              ) AS D ON D.JuchuuNO = DM.JuchuuNO
              AND D.JuchuuRows = DM.JuchuuRows
                
              --明細部の入金日を求めるためのサブクエリ
              LEFT OUTER JOIN (SELECT DC.JuchuuNO,DC.JuchuuRows,MAX(DPC.CollectClearDate) AS CollectClearDate
                            FROM D_CollectPlanDetails AS DC 
                              LEFT OUTER JOIN D_CollectBillingDetails AS DCB ON DCB.CollectPlanNO = DC.CollectPlanNO
                                AND DCB.CollectPlanRows = DC.CollectPlanRows 
                                AND DCB.DeleteDateTime IS NULL
                              LEFT OUTER JOIN D_CollectBilling AS DCB2 ON DCB2.CollectPlanNO = DCB.CollectPlanNO
                                AND DCB2.DeleteDateTime IS NULL
                              LEFT OUTER JOIN D_PaymentConfirm AS DPC ON DPC.ConfirmNO = DCB2.ConfirmNO 
                                AND DPC.DeleteDateTime IS NULL
                            
                            WHERE DC.DeleteDateTime IS NULL
                            GROUP BY DC.JuchuuNO,DC.JuchuuRows
               )AS DPC
               ON DPC.JuchuuNO = DM.JuchuuNO
               AND DPC.JuchuuRows = DM.JuchuuRows 
                
              WHERE DH.JuchuuNO = @JuchuuNO 
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.JuchuuNO, DM.DisplayRows
                ;
--        END

END

