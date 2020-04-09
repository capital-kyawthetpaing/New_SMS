 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_SelectForSiharaiNo]
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
CREATE PROCEDURE [dbo].[D_Pay_SelectForSiharaiNo]
	-- Add the parameters for the stored procedure here
	@PayDateFrom date,
	@PayDateTo date,
	@InputDateTimeFrom date,
	@InputDateTimeTo date,
	@PayeeCD varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	dp.LargePayNO ,
	dp.PayNO,
	dp.PayDate,
	dp.InputDateTime,
	dp.PayeeCD,
	mv.VendorName,
	dp.PayGaku,
	dp.TransferGaku,
	dp.PayGaku - dp.TransferGaku as total
	From D_Pay dp 
	Left Outer Join F_Vendor(getdate()) mv on mv.VendorCD = dp.PayeeCD and mv.MoneyPayeeFlg = 1
	Where dp.DeleteDateTime is null 
	and mv.DeleteFlg = 0
	and (@PayDateFrom is null or (dp.PayDate >=@PayDateFrom))  
	and (@PayDateTo is null or (dp.PayDate <= @PayDateTo)) 
	and (@InputDateTimeFrom is null or (dp.InputDateTime >= @InputDateTimeFrom)) 
	and (@InputDateTimeTo is null or (dp.InputDateTime <= @InputDateTimeTo))
	and (@PayeeCD is null or (dp.PayeeCD = @PayeeCD))
	Order by dp.LargePayNO ,dp.PayNO asc 
END

