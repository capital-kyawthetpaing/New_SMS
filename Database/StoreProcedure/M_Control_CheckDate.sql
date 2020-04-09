 BEGIN TRY 
 Drop Procedure dbo.[M_Control_CheckDate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_Control_CheckDate]    */
CREATE PROCEDURE [dbo].[M_Control_CheckDate]
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
     INNER JOIN M_FiscalYear MF ON MF.FiscalYear = M.FiscalYear
     WHERE M.MainKey = @MainKey
     AND MF.InputPossibleStartDate <= CONVERT(DATE, @ChangeDate)
    AND MF.InputPossibleEndDate >= CONVERT(DATE, @ChangeDate)

    ;
END


