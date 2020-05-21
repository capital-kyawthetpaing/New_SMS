
DROP TRIGGER Trg_DJuchuu_Insert
GO
DROP TRIGGER Trg_DJuchuu_Update
GO

DROP TRIGGER Trg_DJuchuuDetails_Insert
GO
DROP TRIGGER Trg_DJuchuuDetails_Update
GO

DROP TRIGGER Trg_DStoreJuchuu_Insert
GO
DROP TRIGGER Trg_DStoreJuchuu_Update
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE TRIGGER Trg_DJuchuu_Insert
ON D_Juchuu
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

        INSERT INTO [L_JuchuuHistory]
           ([JuchuuNO]
           ,[StoreCD]
           ,[JuchuuDate]
           ,[JuchuuTime]
           ,[ReturnFLG]
           ,[SoukoCD]
           ,[JuchuuKBN]
           ,[SiteKBN]
           ,[SiteJuchuuDateTime]
           ,[SiteJuchuuNO]
           ,[InportErrFLG]
           ,[OnHoldFLG]
           ,[IdentificationFLG]
           ,[TorikomiDateTime]
           ,[StaffCD]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[AliasKBN]
           ,[ZipCD1]
           ,[ZipCD2]
           ,[Address1]
           ,[Address2]
           ,[Tel11]
           ,[Tel12]
           ,[Tel13]
           ,[Tel21]
           ,[Tel22]
           ,[Tel23]
           ,[CustomerKanaName]
           ,[JuchuuCarrierCD]
           ,[DecidedCarrierFLG]
           ,[LastCarrierCD]
           ,[NameSortingDateTime]
           ,[NameSortingStaffCD]
           ,[CurrencyCD]
           ,[JuchuuGaku]
           ,[Discount]
           ,[HanbaiHontaiGaku]
           ,[HanbaiTax8]
           ,[HanbaiTax10]
           ,[HanbaiGaku]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[Coupon]
           ,[Point]
           ,[PayCharge]
           ,[Adjustments]
           ,[Postage]
           ,[GiftWrapCharge]
           ,[InvoiceGaku]
           ,[PaymentMethodCD]
           ,[PaymentPlanNO]
           ,[CardProgressKBN]
           ,[CardCompany]
           ,[CardNumber]
           ,[PaymentProgressKBN]
           ,[PresentFLG]
           ,[SalesPlanDate]
           ,[FirstPaypentPlanDate]
           ,[LastPaymentPlanDate]
           ,[DemandProgressKBN]
           ,[CommentDemand]
           ,[CancelDate]
           ,[CancelReasonKBN]
           ,[CancelRemarks]
           ,[NoMailFLG]
           ,[IndividualContactKBN]
           ,[TelephoneContactKBN]
           ,[LastMailKBN]
           ,[LastMailPatternCD]
           ,[LastMailDatetime]
           ,[LastMailName]
           ,[NextMailKBN]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[LastDepositeDate]
           ,[LastOrderDate]
           ,[LastArriveDate]
           ,[LastSalesDate]
           ,[MitsumoriNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[JuchuuDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            JuchuuNO
           ,StoreCD
           ,JuchuuDate
           ,JuchuuTime
           ,ReturnFLG
           ,SoukoCD
           ,JuchuuKBN
           ,SiteKBN
           ,SiteJuchuuDateTime
           ,SiteJuchuuNO
           ,InportErrFLG
           ,OnHoldFLG
           ,IdentificationFLG
           ,TorikomiDateTime
           ,StaffCD
           ,CustomerCD
           ,CustomerName
           ,CustomerName2
           ,AliasKBN
           ,ZipCD1
           ,ZipCD2
           ,Address1
           ,Address2
           ,Tel11
           ,Tel12
           ,Tel13
           ,Tel21
           ,Tel22
           ,Tel23
           ,CustomerKanaName
           ,JuchuuCarrierCD
           ,DecidedCarrierFLG
           ,LastCarrierCD
           ,NameSortingDateTime
           ,NameSortingStaffCD
           ,CurrencyCD
           ,JuchuuGaku
           ,Discount
           ,HanbaiHontaiGaku
           ,HanbaiTax8
           ,HanbaiTax10
           ,HanbaiGaku
           ,CostGaku
           ,ProfitGaku
           ,Coupon
           ,Point
           ,PayCharge
           ,Adjustments
           ,Postage
           ,GiftWrapCharge
           ,InvoiceGaku
           ,PaymentMethodCD
           ,PaymentPlanNO
           ,CardProgressKBN
           ,CardCompany
           ,CardNumber
           ,PaymentProgressKBN
           ,PresentFLG
           ,SalesPlanDate
           ,FirstPaypentPlanDate
           ,LastPaymentPlanDate
           ,DemandProgressKBN
           ,CommentDemand
           ,CancelDate
           ,CancelReasonKBN
           ,CancelRemarks
           ,NoMailFLG
           ,IndividualContactKBN
           ,TelephoneContactKBN
           ,LastMailKBN
           ,LastMailPatternCD
           ,LastMailDatetime
           ,LastMailName
           ,NextMailKBN
           ,CommentOutStore
           ,CommentInStore
           ,LastDepositeDate
           ,LastOrderDate
           ,LastArriveDate
           ,LastSalesDate
           ,MitsumoriNO
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,JuchuuDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DJuchuu_Update
ON D_Juchuu
AFTER UPDATE
AS
BEGIN
    
        INSERT INTO [L_JuchuuHistory]
           ([JuchuuNO]
           ,[StoreCD]
           ,[JuchuuDate]
           ,[JuchuuTime]
           ,[ReturnFLG]
           ,[SoukoCD]
           ,[JuchuuKBN]
           ,[SiteKBN]
           ,[SiteJuchuuDateTime]
           ,[SiteJuchuuNO]
           ,[InportErrFLG]
           ,[OnHoldFLG]
           ,[IdentificationFLG]
           ,[TorikomiDateTime]
           ,[StaffCD]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[AliasKBN]
           ,[ZipCD1]
           ,[ZipCD2]
           ,[Address1]
           ,[Address2]
           ,[Tel11]
           ,[Tel12]
           ,[Tel13]
           ,[Tel21]
           ,[Tel22]
           ,[Tel23]
           ,[CustomerKanaName]
           ,[JuchuuCarrierCD]
           ,[DecidedCarrierFLG]
           ,[LastCarrierCD]
           ,[NameSortingDateTime]
           ,[NameSortingStaffCD]
           ,[CurrencyCD]
           ,[JuchuuGaku]
           ,[Discount]
           ,[HanbaiHontaiGaku]
           ,[HanbaiTax8]
           ,[HanbaiTax10]
           ,[HanbaiGaku]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[Coupon]
           ,[Point]
           ,[PayCharge]
           ,[Adjustments]
           ,[Postage]
           ,[GiftWrapCharge]
           ,[InvoiceGaku]
           ,[PaymentMethodCD]
           ,[PaymentPlanNO]
           ,[CardProgressKBN]
           ,[CardCompany]
           ,[CardNumber]
           ,[PaymentProgressKBN]
           ,[PresentFLG]
           ,[SalesPlanDate]
           ,[FirstPaypentPlanDate]
           ,[LastPaymentPlanDate]
           ,[DemandProgressKBN]
           ,[CommentDemand]
           ,[CancelDate]
           ,[CancelReasonKBN]
           ,[CancelRemarks]
           ,[NoMailFLG]
           ,[IndividualContactKBN]
           ,[TelephoneContactKBN]
           ,[LastMailKBN]
           ,[LastMailPatternCD]
           ,[LastMailDatetime]
           ,[LastMailName]
           ,[NextMailKBN]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[LastDepositeDate]
           ,[LastOrderDate]
           ,[LastArriveDate]
           ,[LastSalesDate]
           ,[MitsumoriNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[JuchuuDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            JuchuuNO
           ,StoreCD
           ,JuchuuDate
           ,JuchuuTime
           ,ReturnFLG
           ,SoukoCD
           ,JuchuuKBN
           ,SiteKBN
           ,SiteJuchuuDateTime
           ,SiteJuchuuNO
           ,InportErrFLG
           ,OnHoldFLG
           ,IdentificationFLG
           ,TorikomiDateTime
           ,StaffCD
           ,CustomerCD
           ,CustomerName
           ,CustomerName2
           ,AliasKBN
           ,ZipCD1
           ,ZipCD2
           ,Address1
           ,Address2
           ,Tel11
           ,Tel12
           ,Tel13
           ,Tel21
           ,Tel22
           ,Tel23
           ,CustomerKanaName
           ,JuchuuCarrierCD
           ,DecidedCarrierFLG
           ,LastCarrierCD
           ,NameSortingDateTime
           ,NameSortingStaffCD
           ,CurrencyCD
           ,JuchuuGaku
           ,Discount
           ,HanbaiHontaiGaku
           ,HanbaiTax8
           ,HanbaiTax10
           ,HanbaiGaku
           ,CostGaku
           ,ProfitGaku
           ,Coupon
           ,Point
           ,PayCharge
           ,Adjustments
           ,Postage
           ,GiftWrapCharge
           ,InvoiceGaku
           ,PaymentMethodCD
           ,PaymentPlanNO
           ,CardProgressKBN
           ,CardCompany
           ,CardNumber
           ,PaymentProgressKBN
           ,PresentFLG
           ,SalesPlanDate
           ,FirstPaypentPlanDate
           ,LastPaymentPlanDate
           ,DemandProgressKBN
           ,CommentDemand
           ,CancelDate
           ,CancelReasonKBN
           ,CancelRemarks
           ,NoMailFLG
           ,IndividualContactKBN
           ,TelephoneContactKBN
           ,LastMailKBN
           ,LastMailPatternCD
           ,LastMailDatetime
           ,LastMailName
           ,NextMailKBN
           ,CommentOutStore
           ,CommentInStore
           ,LastDepositeDate
           ,LastOrderDate
           ,LastArriveDate
           ,LastSalesDate
           ,MitsumoriNO
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,JuchuuDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;

END
GO

CREATE TRIGGER Trg_DJuchuuDetails_Insert
ON D_JuchuuDetails
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_JuchuuDetailsHistory]
           ([HistorySEQ]
           ,[HistorySEQRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[DisplayRows]
           ,[SiteJuchuuRows]
           ,[NotPrintFLG]
           ,[AddJuchuuRows]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SetKBN]
           ,[SetRows]
           ,[JuchuuSuu]
           ,[JuchuuUnitPrice]
           ,[TaniCD]
           ,[JuchuuGaku]
           ,[JuchuuHontaiGaku]
           ,[JuchuuTax]
           ,[JuchuuTaxRitsu]
           ,[CostUnitPrice]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[SoukoCD]
           ,[HikiateSu]
           ,[DeliveryOrderSu]
           ,[DeliverySu]
           ,[DirectFLG]
           ,[HikiateFLG]
           ,[JuchuuOrderNO]
           ,[VendorCD]
           ,[LastOrderNO]
           ,[LastOrderRows]
           ,[LastOrderDateTime]
           ,[DesiredDeliveryDate]
           ,[AnswerFLG]
           ,[ArrivePlanDate]
           ,[ArrivePlanNO]
           ,[ArriveDateTime]
           ,[ArriveNO]
           ,[ArribveNORows]
           ,[DeliveryPlanNO]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[SalesDate]
           ,[SalesNO]
           ,[DepositeDetailNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
         SELECT (SELECT IDENT_CURRENT('L_JuchuuHistory'))
--               ,ROW_NUMBER() OVER(ORDER BY JuchuuRows) 
				,JuchuuRows
               ,JuchuuNO
               ,JuchuuRows
               ,DisplayRows
               ,SiteJuchuuRows
               ,NotPrintFLG
               ,AddJuchuuRows
               ,AdminNO
               ,SKUCD
               ,JanCD
               ,SKUName
               ,ColorName
               ,SizeName
               ,SetKBN
               ,SetRows
               ,JuchuuSuu
               ,JuchuuUnitPrice
               ,TaniCD
               ,JuchuuGaku
               ,JuchuuHontaiGaku
               ,JuchuuTax
               ,JuchuuTaxRitsu
               ,CostUnitPrice
               ,CostGaku
               ,ProfitGaku
               ,SoukoCD
               ,HikiateSu
               ,DeliveryOrderSu
               ,DeliverySu
               ,DirectFLG
               ,HikiateFLG
               ,JuchuuOrderNO
               ,VendorCD
               ,LastOrderNO
               ,LastOrderRows
               ,LastOrderDateTime
               ,DesiredDeliveryDate
               ,AnswerFLG
               ,ArrivePlanDate
               ,ArrivePlanNO
               ,ArriveDateTime
               ,ArriveNO
               ,ArribveNORows
               ,DeliveryPlanNO
               ,CommentOutStore
               ,CommentInStore
               ,IndividualClientName
               ,SalesDate
               ,SalesNO
               ,DepositeDetailNO
               ,InsertOperator
               ,InsertDateTime
               ,UpdateOperator
               ,UpdateDateTime
               ,DeleteOperator
               ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DJuchuuDetails_Update
ON D_JuchuuDetails
AFTER UPDATE
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_JuchuuDetailsHistory]
           ([HistorySEQ]
           ,[HistorySEQRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[DisplayRows]
           ,[SiteJuchuuRows]
           ,[NotPrintFLG]
           ,[AddJuchuuRows]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SetKBN]
           ,[SetRows]
           ,[JuchuuSuu]
           ,[JuchuuUnitPrice]
           ,[TaniCD]
           ,[JuchuuGaku]
           ,[JuchuuHontaiGaku]
           ,[JuchuuTax]
           ,[JuchuuTaxRitsu]
           ,[CostUnitPrice]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[SoukoCD]
           ,[HikiateSu]
           ,[DeliveryOrderSu]
           ,[DeliverySu]
           ,[DirectFLG]
           ,[HikiateFLG]
           ,[JuchuuOrderNO]
           ,[VendorCD]
           ,[LastOrderNO]
           ,[LastOrderRows]
           ,[LastOrderDateTime]
           ,[DesiredDeliveryDate]
           ,[AnswerFLG]
           ,[ArrivePlanDate]
           ,[ArrivePlanNO]
           ,[ArriveDateTime]
           ,[ArriveNO]
           ,[ArribveNORows]
           ,[DeliveryPlanNO]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[SalesDate]
           ,[SalesNO]
           ,[DepositeDetailNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
         SELECT (SELECT IDENT_CURRENT('L_JuchuuHistory'))
--               ,ROW_NUMBER() OVER(ORDER BY JuchuuRows) 
				,JuchuuRows
               ,JuchuuNO
               ,JuchuuRows
               ,DisplayRows
               ,SiteJuchuuRows
	           ,NotPrintFLG
	           ,AddJuchuuRows
               ,AdminNO
               ,SKUCD
               ,JanCD
               ,SKUName
               ,ColorName
               ,SizeName
               ,SetKBN
               ,SetRows
               ,JuchuuSuu
               ,JuchuuUnitPrice
               ,TaniCD
               ,JuchuuGaku
               ,JuchuuHontaiGaku
               ,JuchuuTax
               ,JuchuuTaxRitsu
               ,CostUnitPrice
               ,CostGaku
               ,ProfitGaku
               ,SoukoCD
               ,HikiateSu
               ,DeliveryOrderSu
               ,DeliverySu
               ,DirectFLG
               ,HikiateFLG
               ,JuchuuOrderNO
               ,VendorCD
               ,LastOrderNO
               ,LastOrderRows
               ,LastOrderDateTime
               ,DesiredDeliveryDate
               ,AnswerFLG
               ,ArrivePlanDate
               ,ArrivePlanNO
               ,ArriveDateTime
               ,ArriveNO
               ,ArribveNORows
               ,DeliveryPlanNO
               ,CommentOutStore
               ,CommentInStore
               ,IndividualClientName
               ,SalesDate
               ,SalesNO
               ,DepositeDetailNO
               ,InsertOperator
               ,InsertDateTime
               ,UpdateOperator
               ,UpdateDateTime
               ,DeleteOperator
               ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DStoreJuchuu_Insert
ON D_StoreJuchuu
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

     INSERT INTO [L_StoreJuchuuHistory]
           ([JuchuuNO]
           ,[NouhinsyoComment])
     SELECT
            JuchuuNO
           ,NouhinsyoComment
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DStoreJuchuu_Update
ON D_StoreJuchuu
AFTER UPDATE
AS
BEGIN
    
     INSERT INTO [L_StoreJuchuuHistory]
           ([JuchuuNO]
           ,[NouhinsyoComment])
     SELECT
            JuchuuNO
           ,NouhinsyoComment
           FROM    inserted;  

END
GO

