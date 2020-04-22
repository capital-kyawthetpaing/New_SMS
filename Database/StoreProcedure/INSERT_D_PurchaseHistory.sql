 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_PurchaseHistory]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [INSERT_D_PurchaseHistory]
(
    @PurchaseNO varchar(11),
    @RecordKBN tinyint,
    @SYSDATETIME  datetime,
    @Operator  varchar(10)
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

	INSERT INTO [D_PurchaseHistory]
           ([PurchaseNO]
           ,[RecordKBN]
           ,[HistoryDateTime]
           ,[HistoryOperator]
           ,[StoreCD]
           ,[PurchaseDate]
           ,[CancelFlg]
           ,[ProcessKBN]
           ,[ReturnsFlg]
           ,[VendorCD]
           ,[CalledVendorCD]
           ,[CalculationGaku]
           ,[AdjustmentGaku]
           ,[PurchaseGaku]
           ,[PurchaseTax]
           ,[TotalPurchaseGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[ExpectedDateFrom]
           ,[ExpectedDateTo]
           ,[InputDate]
           ,[StaffCD]
           ,[PaymentPlanDate]
           ,[PayPlanNO]
           ,[OutputDateTime]
           ,[StockAccountFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            DP.PurchaseNO
           ,@RecordKBN
           ,@SYSDATETIME	--HistoryDateTime
           ,@Operator	--HistoryOperator
           ,DP.StoreCD
           ,DP.PurchaseDate
           ,DP.CancelFlg
           ,DP.ProcessKBN
           ,DP.ReturnsFlg
           ,DP.VendorCD
           ,DP.CalledVendorCD
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.CalculationGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.AdjustmentGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.PurchaseGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.PurchaseTax
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.TotalPurchaseGaku
           ,DP.CommentOutStore
           ,DP.CommentInStore
           ,DP.ExpectedDateFrom
           ,DP.ExpectedDateTo
           ,DP.InputDate
           ,DP.StaffCD
           ,DP.PaymentPlanDate
           ,DP.PayPlanNO
           ,DP.OutputDateTime
           ,DP.StockAccountFlg
           ,DP.InsertOperator
           ,DP.InsertDateTime
           ,DP.UpdateOperator
           ,DP.UpdateDateTime
           ,DP.DeleteOperator
           ,DP.DeleteDateTime
	FROM D_Purchase AS DP
	WHERE DP.PurchaseNO = @PurchaseNO
	AND DP.DeleteDateTime IS NULL;

    --直前に採番された IDENTITY 列の値を取得する
    DECLARE @HistoryNO int;
    SET @HistoryNO = @@IDENTITY;
    
	INSERT INTO [D_PurchaseDetailsHistory]
           ([HistoryNO]
           ,[PurchaseNO]
           ,[RecordKBN]
           ,[HistoryDateTime]
           ,[HistoryOperator]
           ,[PurchaseRows]
           ,[DisplayRows]
           ,[ArrivalNO]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[ItemName]
           ,[ColorName]
           ,[SizeName]
           ,[Remark]
           ,[PurchaseSu]
           ,[TaniCD]
           ,[TaniName]
           ,[PurchaserUnitPrice]
           ,[CalculationGaku]
           ,[AdjustmentGaku]
           ,[PurchaseGaku]
           ,[PurchaseTax]
           ,[TotalPurchaseGaku]
           ,[CurrencyCD]
           ,[TaxRitsu]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[ReturnNO]
           ,[ReturnRows]
           ,[OrderUnitPrice]
           ,[OrderNO]
           ,[OrderRows]
           ,[StockNO]
           ,[DifferenceFlg]
           ,[DeliveryNo]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
	SELECT @HistoryNO
	       ,@PurchaseNO
           ,@RecordKBN
           ,@SYSDATETIME	--HistoryDateTime
           ,@Operator	--HistoryOperator
           ,DP.PurchaseRows
           ,DP.DisplayRows
           ,DP.ArrivalNO
           ,DP.SKUCD
           ,DP.AdminNO
           ,DP.JanCD
           ,DP.ItemName
           ,DP.ColorName
           ,DP.SizeName
           ,DP.Remark
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.PurchaseSu
           ,DP.TaniCD
           ,DP.TaniName
           ,DP.PurchaserUnitPrice
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.CalculationGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.AdjustmentGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.PurchaseGaku
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.PurchaseTax
           ,(CASE @RecordKBN WHEN 2 THEN -1 ELSE 1 END) * DP.TotalPurchaseGaku
           ,DP.CurrencyCD
           ,DP.TaxRitsu
           ,DP.CommentOutStore
           ,DP.CommentInStore
           ,DP.ReturnNO
           ,DP.ReturnRows
           ,DP.OrderUnitPrice
           ,DP.OrderNO
           ,DP.OrderRows
           ,DP.StockNO
           ,DP.DifferenceFlg
           ,DP.DeliveryNo
           ,DP.InsertOperator
           ,DP.InsertDateTime
           ,DP.UpdateOperator
           ,DP.UpdateDateTime
           ,DP.DeleteOperator
           ,DP.DeleteDateTime

    FROM D_PurchaseDetails AS DP
    WHERE DP.PurchaseNO = @PurchaseNO
    AND DP.DeleteDateTime IS NULL;


END


