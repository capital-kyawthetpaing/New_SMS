 BEGIN TRY 
 Drop Procedure dbo.[D_Sales_DataSelect]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_Sales_DataSelect]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @currentdate as date = getdate()

    -- Insert statements for procedure here
	Select 	
	Count(SalesNo) as SalesNo1 ,
	SUM(SalesGaku) as SalesGaku,
	SUM(SalesHontaiGaku8 + SalesTax8) as Amount8,
	SUM(SalesHontaiGaku10 + SalesTax10) as Amount10,
	SUM(SalesHontaiGaku0) as TaxAmount,
	SUM(SalesHontaiGaku8+SalesHontaiGaku10+SalesHontaiGaku0) as SaleTax,
	SUM (SalesTax8) as ForeignTax8 ,
	SUM (SalesTax10) as ForeignTax10
	From D_Sales
	Where DeleteDateTime is null and SalesDate = @currentdate and StoreCD = @StoreCD
    having count( SalesNO) > 0

END

