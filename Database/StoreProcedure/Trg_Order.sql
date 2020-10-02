
DROP TRIGGER Trg_DOrder_Insert
GO
DROP TRIGGER Trg_DOrder_Update
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
--                 èàóùäJén                   --
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
           ,[ArrivalPlanDate]

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
           ,ArrivalPlanDate
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
           ,[ArrivalPlanDate]
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
           ,ArrivalPlanDate
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;

END
GO



