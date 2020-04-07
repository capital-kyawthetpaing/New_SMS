 BEGIN TRY 
 Drop Procedure dbo.[M_Control_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_Control_Select]    */
Create PROCEDURE [dbo].[M_Control_Select]
(
    -- Add the parameters for the stored procedure here
    @MainKey  tinyint
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
          ,M.[Haspo]				--1:Haspo
          ,M.[MonthlySummuryKBN]	--1:全社、2:店舗
          ,M.[SeqUnit]				--1:永久連番、2:年連番、3:月連番
          ,M.[DeliveryDate]
          ,M.[PaymentTerms]
          ,M.[DeliveryPlace]
          ,M.[ValidityPeriod]
          ,M.[PostalAccountA]
          ,M.[PostalAccountB]
          ,M.[PostalAccountNo]
        ,M.[InsertOperator]
        ,CONVERT(varchar,M.[InsertDateTime]) AS InsertDateTime
        ,M.[UpdateOperator]
        ,CONVERT(varchar,M.[UpdateDateTime]) AS UpdateDateTime
     FROM M_Control M
     WHERE M.MainKey = @MainKey


    ;
END


