
DROP TRIGGER Trg_DOrder_Insert
GO
DROP TRIGGER Trg_DOrder_Update
GO

DROP TRIGGER Trg_DOrderDetails_Insert
GO
DROP TRIGGER Trg_DOrderDetails_Update
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
CREATE TRIGGER Trg_DOrder_Insert
ON D_Order
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

        INSERT INTO [L_OrderHistory]
           ([OrderNO]
           ,[OrderProcessNO]
           ,[StoreCD]
           ,[OrderDate]
           ,[ReturnFLG]
           ,[OrderDataKBN]
           ,[OrderWayKBN]
           ,[OrderCD]
           ,[OrderPerson]
           ,[AliasKBN]
           ,[DestinationKBN]
           ,[DestinationName]
           ,[DestinationZip1CD]
           ,[DestinationZip2CD]
           ,[DestinationAddress1]
           ,[DestinationAddress2]
           ,[DestinationTelphoneNO]
           ,[DestinationFaxNO]
           ,[DestinationSoukoCD]
           ,[CurrencyCD]
           ,[OrderHontaiGaku]
           ,[OrderTax8]
           ,[OrderTax10]
           ,[OrderGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[StaffCD]
           ,[FirstArriveDate]
           ,[LastArriveDate]
           ,[ApprovalDate]
           ,[LastApprovalDate]
           ,[LastApprovalStaffCD]
           ,[ApprovalStageFLG]
           ,[FirstPrintDate]
           ,[LastPrintDate]

           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            OrderNO
           ,OrderProcessNO
           ,StoreCD
           ,OrderDate
           ,ReturnFLG
           ,OrderDataKBN
           ,OrderWayKBN
           ,OrderCD
           ,OrderPerson
           ,AliasKBN
           ,DestinationKBN
           ,DestinationName
           ,DestinationZip1CD
           ,DestinationZip2CD
           ,DestinationAddress1
           ,DestinationAddress2
           ,DestinationTelphoneNO
           ,DestinationFaxNO
           ,DestinationSoukoCD
           ,CurrencyCD
           ,OrderHontaiGaku
           ,OrderTax8
           ,OrderTax10
           ,OrderGaku
           ,CommentOutStore
           ,CommentInStore
           ,StaffCD
           ,FirstArriveDate
           ,LastArriveDate
           ,ApprovalDate
           ,LastApprovalDate
           ,LastApprovalStaffCD
           ,ApprovalStageFLG
           ,FirstPrintDate
           ,LastPrintDate
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DOrder_Update
ON D_Order
AFTER UPDATE
AS
BEGIN
    
        INSERT INTO [L_OrderHistory]
           ([OrderNO]
           ,[OrderProcessNO]
           ,[StoreCD]
           ,[OrderDate]
           ,[ReturnFLG]
           ,[OrderDataKBN]
           ,[OrderWayKBN]
           ,[OrderCD]
           ,[OrderPerson]
           ,[AliasKBN]
           ,[DestinationKBN]
           ,[DestinationName]
           ,[DestinationZip1CD]
           ,[DestinationZip2CD]
           ,[DestinationAddress1]
           ,[DestinationAddress2]
           ,[DestinationTelphoneNO]
           ,[DestinationFaxNO]
           ,[DestinationSoukoCD]
           ,[CurrencyCD]
           ,[OrderHontaiGaku]
           ,[OrderTax8]
           ,[OrderTax10]
           ,[OrderGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[StaffCD]
           ,[FirstArriveDate]
           ,[LastArriveDate]
           ,[ApprovalDate]
           ,[LastApprovalDate]
           ,[LastApprovalStaffCD]
           ,[ApprovalStageFLG]
           ,[FirstPrintDate]
           ,[LastPrintDate]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            OrderNO
           ,OrderProcessNO
           ,StoreCD
           ,OrderDate
           ,ReturnFLG
           ,OrderDataKBN
           ,OrderWayKBN
           ,OrderCD
           ,OrderPerson
           ,AliasKBN
           ,DestinationKBN
           ,DestinationName
           ,DestinationZip1CD
           ,DestinationZip2CD
           ,DestinationAddress1
           ,DestinationAddress2
           ,DestinationTelphoneNO
           ,DestinationFaxNO
           ,DestinationSoukoCD
           ,CurrencyCD
           ,OrderHontaiGaku
           ,OrderTax8
           ,OrderTax10
           ,OrderGaku
           ,CommentOutStore
           ,CommentInStore
           ,StaffCD
           ,FirstArriveDate
           ,LastArriveDate
           ,ApprovalDate
           ,LastApprovalDate
           ,LastApprovalStaffCD
           ,ApprovalStageFLG
           ,FirstPrintDate
           ,LastPrintDate
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;

END
GO

CREATE TRIGGER Trg_DOrderDetails_Insert
ON D_OrderDetails
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_OrderDetailsHistory]
           ([HistorySEQ]
           ,[HistorySEQRows]
           ,[OrderNO]
           ,[OrderRows]

           ,[DisplayRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[MakerItem]
           ,[ItemName]
           ,[ColorName]
           ,[SizeName]
           ,[Remarks]
           ,[OrderSu]
           ,[TaniCD]
           ,[PriceOutTax]
           ,[Rate]
           ,[OrderUnitPrice]
           ,[OrderHontaiGaku]
           ,[OrderTax]
           ,[OrderTaxRitsu]
           ,[OrderGaku]
           ,[SoukoCD]
           ,[DirectFLG]
           ,[NotNetFLG]
           ,[EDIFLG]
           ,[DesiredDeliveryDate]
           ,[ArrivePlanDate]
           ,[TotalArrivalSu]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[FirstOrderNO]
           ,[FirstOrderRows]
           ,[CancelOrderNO]
           ,[AnswerFLG]
           ,[EDIOutputDatetime]

           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
         SELECT (SELECT IDENT_CURRENT('L_OrderHistory'))
--               ,ROW_NUMBER() OVER(ORDER BY OrderRows) 
                ,OrderRows
               ,OrderNO
               ,OrderRows
               ,DisplayRows
               ,JuchuuNO
               ,JuchuuRows
               ,SKUCD
               ,AdminNO
               ,JanCD
               ,MakerItem
               ,ItemName
               ,ColorName
               ,SizeName
               ,Remarks
               ,OrderSu
               ,TaniCD
               ,PriceOutTax
               ,Rate
               ,OrderUnitPrice
               ,OrderHontaiGaku
               ,OrderTax
               ,OrderTaxRitsu
               ,OrderGaku
               ,SoukoCD
               ,DirectFLG
               ,NotNetFLG
               ,EDIFLG
               ,DesiredDeliveryDate
               ,ArrivePlanDate
               ,TotalArrivalSu
               ,CommentOutStore
               ,CommentInStore
               ,FirstOrderNO
               ,FirstOrderRows
               ,CancelOrderNO
               ,AnswerFLG
               ,EDIOutputDatetime
               ,InsertOperator
               ,InsertDateTime
               ,UpdateOperator
               ,UpdateDateTime
               ,DeleteOperator
               ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DOrderDetails_Update
ON D_OrderDetails
AFTER UPDATE
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_OrderDetailsHistory]
           ([HistorySEQ]
           ,[HistorySEQRows]
           ,[OrderNO]
           ,[OrderRows]
           ,[DisplayRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[MakerItem]
           ,[ItemName]
           ,[ColorName]
           ,[SizeName]
           ,[Remarks]
           ,[OrderSu]
           ,[TaniCD]
           ,[PriceOutTax]
           ,[Rate]
           ,[OrderUnitPrice]
           ,[OrderHontaiGaku]
           ,[OrderTax]
           ,[OrderTaxRitsu]
           ,[OrderGaku]
           ,[SoukoCD]
           ,[DirectFLG]
           ,[NotNetFLG]
           ,[EDIFLG]
           ,[DesiredDeliveryDate]
           ,[ArrivePlanDate]
           ,[TotalArrivalSu]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[FirstOrderNO]
           ,[FirstOrderRows]
           ,[CancelOrderNO]
           ,[AnswerFLG]
           ,[EDIOutputDatetime]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
         SELECT (SELECT IDENT_CURRENT('L_OrderHistory'))
--               ,ROW_NUMBER() OVER(ORDER BY OrderRows) 
				,OrderRows
               ,OrderNO
               ,OrderRows
               ,DisplayRows
               ,JuchuuNO
               ,JuchuuRows
               ,SKUCD
               ,AdminNO
               ,JanCD
               ,MakerItem
               ,ItemName
               ,ColorName
               ,SizeName
               ,Remarks
               ,OrderSu
               ,TaniCD
               ,PriceOutTax
               ,Rate
               ,OrderUnitPrice
               ,OrderHontaiGaku
               ,OrderTax
               ,OrderTaxRitsu
               ,OrderGaku
               ,SoukoCD
               ,DirectFLG
               ,NotNetFLG
               ,EDIFLG
               ,DesiredDeliveryDate
               ,ArrivePlanDate
               ,TotalArrivalSu
               ,CommentOutStore
               ,CommentInStore
               ,FirstOrderNO
               ,FirstOrderRows
               ,CancelOrderNO
               ,AnswerFLG
               ,EDIOutputDatetime
               ,InsertOperator
               ,InsertDateTime
               ,UpdateOperator
               ,UpdateDateTime
               ,DeleteOperator
               ,DeleteDateTime
           FROM    inserted;  
END
GO

