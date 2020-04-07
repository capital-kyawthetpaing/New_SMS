 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_SalesTran]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_SalesTran]
(
    @SalesNO varchar(11),
    @ProcessKBN tinyint,
    @RecoredKBN tinyint,
    @SIGN int,
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @Keijobi  varchar(10)
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    INSERT INTO [D_SalesTran]
       ([ProcessKBN]
       ,[RecoredKBN]
       ,[SalesNO]
       ,[StoreCD]
       ,[SalesDate]
       ,[ShippingNO]
       ,[CustomerCD]
       ,[CustomerName]
       ,[CustomerName2]
       ,[BillingType]
       ,[SalesHontaiGaku]
       ,[SalesHontaiGaku0]
       ,[SalesHontaiGaku8]
       ,[SalesHontaiGaku10]
       ,[SalesTax]
       ,[SalesTax8]
       ,[SalesTax10]
       ,[SalesGaku]
       ,[LastPoint]
       ,[WaitingPoint]
       ,[StaffCD]
       ,[PrintDate]
       ,[PrintStaffCD]
       ,[StoreSalesUpdateFLG]
       ,[StoreSalesUpdatetime]
       ,[Discount]
       ,[Discount8]
       ,[Discount10]
       ,[DiscountTax]
       ,[DiscountTax8]
       ,[DiscountTax10]
       ,[DiscountAmount]
       ,[BillingAmount]
       ,[PointAmount]
       ,[CardDenominationCD]
       ,[CardAmount]
       ,[CashAmount]
       ,[DepositAmount]
       ,[RefundAmount]
       ,[CreditAmount]
       ,[Denomination1CD]
       ,[Denomination1Amount]
       ,[Denomination2CD]
       ,[Denomination2Amount]
       ,[AdvanceAmount]
       ,[TotalAmount]
       ,[InsertOperator]
       ,[InsertDateTime])
    SELECT
       @ProcessKBN	--ProcessKBN 1:追加、2:訂正、4:返品
       ,@RecoredKBN 	--RecoredKBN
       ,DS.SalesNO
       ,DS.StoreCD
       ,(CASE @RecoredKBN WHEN 1 THEN convert(date,@Keijobi) 
               ELSE convert(date,DS.SalesDate) END)
       ,DS.ShippingNO
       ,DS.CustomerCD
       ,DS.CustomerName
       ,DS.CustomerName2
       ,DS.BillingType
       ,@SIGN*DS.SalesHontaiGaku
       ,@SIGN*DS.SalesHontaiGaku0
       ,@SIGN*DS.SalesHontaiGaku8
       ,@SIGN*DS.SalesHontaiGaku10
       ,@SIGN*DS.SalesTax
       ,@SIGN*DS.SalesTax8
       ,@SIGN*DS.SalesTax10
       ,@SIGN*DS.SalesGaku
       ,DS.LastPoint
       ,DS.WaitingPoint
       ,DS.StaffCD
       ,DS.PrintDate
       ,DS.PrintStaffCD
       ,0		--StoreSalesUpdateFLG
       ,NULL	--StoreSalesUpdatetime
       ,@SIGN*DS.Discount
       ,@SIGN*DS.Discount8
       ,@SIGN*DS.Discount10
       ,@SIGN*DS.DiscountTax
       ,@SIGN*DS.DiscountTax8
       ,@SIGN*DS.DiscountTax10
       ,DP.DiscountAmount
       ,DP.BillingAmount
       ,DP.PointAmount
       ,DP.CardDenominationCD
       ,DP.CardAmount
       ,DP.CashAmount
       ,DP.DepositAmount
       ,DP.RefundAmount
       ,DP.CreditAmount
       ,DP.Denomination1CD
       ,DP.Denomination1Amount
       ,DP.Denomination2CD
       ,DP.Denomination2Amount
       ,DP.AdvanceAmount
       ,DP.TotalAmount
       ,@Operator	--InsertOperator
       ,@SYSDATETIME	--InsertDateTime
       FROM D_Sales AS DS
       LEFT OUTER JOIN D_StorePayment AS DP
       ON DP.SalesNO = DS.SalesNO
       WHERE DS.SalesNO = @SalesNO
       ;

    INSERT INTO [D_SalesDetailsTran]
       ([DataNo]
       ,[DataRows]
       ,[ProcessKBN]
       ,[RecoredKBN]
       ,[SalesNO]
       ,[SalesRows]
       ,[JuchuuNO]
       ,[JuchuuRows]
       ,[ShippingNO]
       ,[SKUCD]
       ,[AdminNO]
       ,[JanCD]
       ,[SKUName]
       ,[ColorName]
       ,[SizeName]
       ,[SalesSU]
       ,[SalesUnitPrice]
       ,[TaniCD]
       ,[SalesHontaiGaku]
       ,[SalesTax]
       ,[SalesGaku]
       ,[SalesTaxRitsu]
       ,[ProperGaku]	--2019.12.12 add
	   ,[DiscountGaku]	--2019.12.12 add
       ,[CommentOutStore]
       ,[CommentInStore]
       ,[IndividualClientName]
       ,[DeliveryNoteFLG]
       ,[BillingPrintFLG]
       ,[DeleteOperator]
       ,[DeleteDateTime]
       ,[InsertOperator]
       ,[InsertDateTime])
    SELECT
       (SELECT IDENT_CURRENT('D_SalesTran'))	--(SELECT top 1 D.DataNo FROM D_SalesTran AS D WHERE D.SalesNO = @SalesNO ORDER BY D.DataNo desc)
       ,DS.SalesRows	--DataRows
       ,@ProcessKBN	--1:追加 ProcessKBN 1:追加、2:訂正、4:返品
       ,@RecoredKBN 	--RecoredKBN
       ,DS.SalesNO
       ,DS.SalesRows
       ,DS.JuchuuNO
       ,DS.JuchuuRows
       ,DS.ShippingNO
       ,DS.SKUCD
       ,DS.AdminNO
       ,DS.JanCD
       ,DS.SKUName
       ,DS.ColorName
       ,DS.SizeName
       ,@SIGN*DS.SalesSU
       ,DS.SalesUnitPrice
       ,DS.TaniCD
       ,@SIGN*DS.SalesHontaiGaku
       ,@SIGN*DS.SalesTax
       ,@SIGN*DS.SalesGaku
       ,DS.SalesTaxRitsu
       ,@SIGN*DS.ProperGaku	--2019.12.12 add
	   ,@SIGN*DS.DiscountGaku	--2019.12.12 add
       ,DS.CommentOutStore
       ,DS.CommentInStore
       ,DS.IndividualClientName
       ,DS.DeliveryNoteFLG
       ,DS.BillingPrintFLG
       ,DS.DeleteOperator
       ,DS.DeleteDateTime
       ,@Operator
       ,@SYSDATETIME
       FROM D_SalesDetails AS DS
       WHERE DS.SalesNO = @SalesNO
       ;
END



