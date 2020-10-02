
DROP TRIGGER Trg_DJuchuu_Insert
GO
DROP TRIGGER Trg_DJuchuu_Update
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
           ,[JuchuuProcessNO]
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
           ,[DeliveryCD]
           ,[DeliveryName]
           ,[DeliveryName2]
           ,[DeliveryAliasKBN]
           ,[DeliveryZipCD1]
           ,[DeliveryZipCD2]
           ,[DeliveryAddress1]
           ,[DeliveryAddress2]
           ,[DeliveryTel11]
           ,[DeliveryTel12]
           ,[DeliveryTel13]
           ,[DeliveryTel21]
           ,[DeliveryTel22]
           ,[DeliveryTel23]
           ,[DeliveryKanaName]
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
           ,JuchuuProcessNO
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
           ,DeliveryCD
           ,DeliveryName
           ,DeliveryName2
           ,DeliveryAliasKBN
           ,DeliveryZipCD1
           ,DeliveryZipCD2
           ,DeliveryAddress1
           ,DeliveryAddress2
           ,DeliveryTel11
           ,DeliveryTel12
           ,DeliveryTel13
           ,DeliveryTel21
           ,DeliveryTel22
           ,DeliveryTel23
           ,DeliveryKanaName
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
           ,[JuchuuProcessNO]
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
           ,[DeliveryCD]
           ,[DeliveryName]
           ,[DeliveryName2]
           ,[DeliveryAliasKBN]
           ,[DeliveryZipCD1]
           ,[DeliveryZipCD2]
           ,[DeliveryAddress1]
           ,[DeliveryAddress2]
           ,[DeliveryTel11]
           ,[DeliveryTel12]
           ,[DeliveryTel13]
           ,[DeliveryTel21]
           ,[DeliveryTel22]
           ,[DeliveryTel23]
           ,[DeliveryKanaName]
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
           ,JuchuuProcessNO
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
           ,DeliveryCD
           ,DeliveryName
           ,DeliveryName2
           ,DeliveryAliasKBN
           ,DeliveryZipCD1
           ,DeliveryZipCD2
           ,DeliveryAddress1
           ,DeliveryAddress2
           ,DeliveryTel11
           ,DeliveryTel12
           ,DeliveryTel13
           ,DeliveryTel21
           ,DeliveryTel22
           ,DeliveryTel23
           ,DeliveryKanaName
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

