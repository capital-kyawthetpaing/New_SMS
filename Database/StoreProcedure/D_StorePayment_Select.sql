BEGIN TRY 
 Drop Procedure D_StorePayment_Select
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [D_StorePayment_Select    */
CREATE PROCEDURE D_StorePayment_Select(
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @SalesNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 
           DS.SalesNO
          ,DS.StoreCD
          ,DS.PurchaseAmount
          ,DS.TaxAmount
          ,DS.DiscountAmount
          ,DS.BillingAmount
          ,DS.PointAmount
          ,DS.CardDenominationCD
          ,(SELECT M.DenominationName FROM M_DenominationKBN AS M 
          WHERE M.DenominationCD = DS.CardDenominationCD) AS CardDenominationName
          ,DS.CardAmount
          ,DS.CashAmount
          ,DS.DepositAmount
          ,DS.RefundAmount
          ,DS.CreditAmount
          ,DS.Denomination1CD
          ,(SELECT M.DenominationName FROM M_DenominationKBN AS M 
          WHERE M.DenominationCD = DS.Denomination1CD) AS DenominationName1
          ,DS.Denomination1Amount
          ,DS.Denomination2CD
          ,(SELECT M.DenominationName FROM M_DenominationKBN AS M 
          WHERE M.DenominationCD = DS.Denomination2CD) AS DenominationName2
          ,DS.Denomination2Amount
          ,DS.AdvanceAmount
          ,DS.TotalAmount
          ,DS.SalesRate
          ,DS.InsertOperator
          ,CONVERT(varchar,DS.InsertDateTime) AS InsertDateTime
          ,DS.UpdateOperator
          ,CONVERT(varchar,DS.UpdateDateTime) AS UpdateDateTime
    from D_StorePayment DS
    WHERE DS.SalesNO = @SalesNO
    /*AND DS.SalesNORows = --(SELECT MAX(M.SalesRows) FROM D_SalesDetails AS M
    					(SELECT MAX(M.DataNo) FROM D_SalesTran AS M
                            WHERE M.SalesNO = @SalesNO
                           -- AND M.DeleteDatetime IS NULL
                           )*/
    AND DS.StoreCD = @StoreCD
    ORDER BY DS.SalesNORows desc
    ;
END

GO


