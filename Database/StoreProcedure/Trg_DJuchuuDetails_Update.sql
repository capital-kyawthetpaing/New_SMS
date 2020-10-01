

/****** Object:  Trigger [Trg_DJuchuuDetails_Update]    Script Date: 2020/09/24 11:14:16 ******/
DROP TRIGGER [dbo].[Trg_DJuchuuDetails_Update]
GO

/****** Object:  Trigger [dbo].[Trg_DJuchuuDetails_Update]    Script Date: 2020/09/24 11:14:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[Trg_DJuchuuDetails_Update]
ON [dbo].[D_JuchuuDetails]
AFTER UPDATE
AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_JuchuuDetailsHistory]
           ([HistorySEQ]
    --       ,[HistorySEQRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[DisplayRows]
           ,[SiteJuchuuRows]
           ,[NotPrintFLG]
           ,[AddJuchuuRows]
           ,[NotOrderFLG]
           ,[ExpressFLG]
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
           ,[OrderUnitPrice]
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
           ,[ShippingPlanDate]
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
--				,JuchuuRows
               ,JuchuuNO
               ,JuchuuRows
               ,DisplayRows
               ,SiteJuchuuRows
	           ,NotPrintFLG
	           ,AddJuchuuRows
               ,NotOrderFLG
               ,ExpressFLG
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
               ,OrderUnitPrice
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
               ,ShippingPlanDate
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

ALTER TABLE [dbo].[D_JuchuuDetails] ENABLE TRIGGER [Trg_DJuchuuDetails_Update]
GO


