
BEGIN TRY 
 Drop PROCEDURE dbo.[M_Control_CheckDateWithFisicalMonth]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_Control_CheckDateWithFisicalMonth]    */
CREATE PROCEDURE M_Control_CheckDateWithFisicalMonth
(
    -- Add the parameters for the stored procedure here
    @MainKey  tinyint,
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT M.[MainKey]
          ,M.[CompanyName]
          ,M.[ShortName]
          ,M.[ZipCD1]
          ,M.[ZipCD2]
          ,M.[Address1]
          ,M.[Address2]
          ,M.[TelephoneNO]
          ,M.[FaxNO]
          ,M.[PresidentName]
          ,M.[StartMonth]
          ,M.[FiscalYear]
          ,M.[FiscalYYYYMM]
          ,M.[CurrencyCD]
          ,M.[SeqUnit]
          ,M.[DeliveryDate]
          ,M.[PaymentTerms]
          ,M.[DeliveryPlace]
          ,M.[ValidityPeriod]
--          ,M.[Logo]
          ,M.[PostalAccountA]
          ,M.[PostalAccountB]
          ,M.[PostalAccountNo]
        ,M.[InsertOperator]
        ,CONVERT(varchar,M.[InsertDateTime]) AS InsertDateTime
        ,M.[UpdateOperator]
        ,CONVERT(varchar,M.[UpdateDateTime]) AS UpdateDateTime
     FROM M_Control M
     INNER JOIN M_FiscalMonth MF ON MF.FiscalYYYYMM = M.FiscalYYYYMM
     WHERE M.MainKey = @MainKey
     AND MF.FiscalStartDate <= CONVERT(DATE, @ChangeDate)
    AND MF.FiscalEndDate >= CONVERT(DATE, @ChangeDate)

    ;
END

GO

