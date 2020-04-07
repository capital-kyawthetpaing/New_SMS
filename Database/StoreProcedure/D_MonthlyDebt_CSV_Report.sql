 BEGIN TRY 
 Drop Procedure dbo.[D_MonthlyDebt_CSV_Report]
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
CREATE PROCEDURE [dbo].[D_MonthlyDebt_CSV_Report]
	-- Add the parameters for the stored procedure here
	@chk as int,
	@ChangeDate date,
	@YYYYMM int,
	@StoreCD varchar(4),
	@PayeeCD varchar(13)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @chk = 0
	BEGIN
		SELECT 
			CONVERT(varchar(10), GETDATE(),111) AS Today,
			LEFT(CONVERT (varchar, GETDATE(), 108),5) AS 'Now',
			NULL as 'Date',
			MD.StoreCD ,
			fs.StoreName ,
			MD.PayeeCD ,
			fv.VendorName, 
			MD.LastBalanceGaku,
			MD.HontaiGaku,
			MD.TaxGaku,
			MD.PayGaku,
			MD.OffsetGaku,
			MD.BalanceGaku
			
		FROM D_MonthlyDebt MD

		INNER JOIN F_Store(EOMONTH(@ChangeDate)) fs on fs.StoreCD=MD.StoreCD
		INNER JOIN F_Vendor(EOMONTH(@ChangeDate)) fv on fv.VendorCD = MD.PayeeCD

		WHERE 
		MD.DeleteDateTime IS NULL 
		AND MD.YYYYMM = @YYYYMM
		AND MD.StoreCD = @StoreCD
		AND MD.PayeeCD = @PayeeCD
		AND 
		(
			MD.LastBalanceGaku <> 0
			OR MD.HontaiGaku <> 0 
			OR MD.TaxGaku <> 0 
			OR MD.PayGaku <> 0
			OR MD.OffsetGaku <> 0
			OR MD.BalanceGaku <> 0
		)
	END
	ELSE
	BEGIN
		SELECT 
			CONVERT(varchar(10), GETDATE(),111) AS Today,
			LEFT(CONVERT (varchar, GETDATE(), 108),5) AS 'Now',
			NULL as 'Date',
			MD.StoreCD ,
			fs.StoreName ,
			MD.PayeeCD ,
			fv.VendorName, 
			MD.LastBalanceGaku,
			MD.HontaiGaku,
			MD.TaxGaku,
			MD.PayGaku,
			MD.OffsetGaku,
			MD.BalanceGaku
			
		FROM D_MonthlyDebt MD

		INNER JOIN F_Store(EOMONTH(@ChangeDate)) fs on fs.StoreCD=MD.StoreCD
		INNER JOIN F_Vendor(EOMONTH(@ChangeDate)) fv on fv.VendorCD = MD.PayeeCD

		WHERE 
		MD.DeleteDateTime IS NULL 
		AND MD.YYYYMM = @YYYYMM
		AND MD.StoreCD = @StoreCD
		AND MD.PayeeCD = @PayeeCD
	END
END

