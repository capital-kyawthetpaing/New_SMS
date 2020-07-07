BEGIN TRY 
 Drop Procedure [dbo].[D_Order_SelectByOrderProcessNO]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [D_Order_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Order_SelectByOrderProcessNO](
    @p_OrderProcessNO varchar(11) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT OrderNO
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
FROM D_Order
WHERE OrderProcessNO = @p_OrderProcessNO

END


