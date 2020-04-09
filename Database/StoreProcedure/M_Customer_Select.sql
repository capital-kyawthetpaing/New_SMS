 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Customer_Select]    */
CREATE PROCEDURE [dbo].[M_Customer_Select](
    -- Add the parameters for the stored procedure here
    @CustomerCD  varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 [CustomerCD]
          ,CONVERT(varchar, ChangeDate,111) AS ChangeDate
          ,[CustomerName]
          ,[VariousFLG]
          ,[LastName]
          ,[FirstName]
          ,[LongName1]
          ,[LongName2]
          ,[KanaName]
          ,[StoreKBN]
          ,[CustomerKBN]
          ,[StoreTankaKBN]
          ,[AliasKBN]
          ,[BillingType]
          ,[GroupName]
          ,[BillingFLG]
          ,[CollectFLG]
          ,[BillingCD]
          ,[CollectCD]
          ,CONVERT(varchar, Birthdate,111) AS Birthdate
          ,[Sex]
          ,[Tel11]
          ,[Tel12]
          ,[Tel13]
          ,[Tel21]
          ,[Tel22]
          ,[Tel23]
          ,[ZipCD1]
          ,[ZipCD2]
          ,[Address1]
          ,[Address2]
          ,[MailAddress]
          ,[TankaCD]
          ,[PointFLG]
          ,[LastPoint]
          ,[WaitingPoint]
          ,[TotalPoint]
          ,[TotalPurchase]
          ,[UnpaidAmount]
          ,[UnpaidCount]
          ,CONVERT(varchar,[LastSalesDate],111) AS LastSalesDate
          ,[LastSalesStoreCD]
          ,[MainStoreCD]
          ,[StaffCD]
          ,[AttentionFLG]
          ,[ConfirmFLG]
          ,[ConfirmComment]
          ,[BillingCloseDate]
          ,[CollectPlanMonth]
          ,[CollectPlanDate]
          ,[HolidayKBN]
          ,[TaxTiming]
          ,[TaxFractionKBN]
          ,[AmountFractionKBN]
          ,[CreditLevel]
          ,[CreditCard]
          ,[CreditInsurance]
          ,[CreditDeposit]
          ,[CreditETC]
          ,[CreditAmount]
          ,[CreditWarningAmount]
          ,[CreditAdditionAmount]
          ,[PaymentMethodCD]
          ,[KouzaCD]
          ,[DisplayOrder]
          ,[PaymentUnit]
          ,[NoInvoiceFlg]
          ,[CountryKBN]
          ,[CountryName]
          ,[RegisteredNumber]
          ,[DMFlg]
          ,[RemarksOutStore]
          ,[RemarksInStore]
          ,[AnalyzeCD1]
          ,[AnalyzeCD2]
          ,[AnalyzeCD3]
	      ,[DeleteFlg]
          ,[UsedFlg]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
    FROM M_Customer

    WHERE [CustomerCD] = @CustomerCD
    AND [ChangeDate] <= CONVERT(DATE, @ChangeDate)
    ORDER BY ChangeDate desc
    ;
END


