 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_SelectForPayPlanDate1]
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
CREATE PROCEDURE [dbo].[D_Pay_SelectForPayPlanDate1]
	-- Add the parameters for the stored procedure here

	@PayeePlanDateFrom date,
	@PayeePlanDateTo date,
	@PayeeCD varchar(11),
	@Operator varchar(11)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		select 
		--(select fs.StaffName from F_Staff(GETDATE()) as fs where fs.StaffCD=@Operator ) as 'StaffName',
		max(fs.StaffName) as 'StaffName',
		max(fz.KouzaCD) 'KouzaCD',
		max(dpp.PayeeCD) 'PayeeCD',
		max(fvp.VendorName) 'VendorName',
		Replace(CONVERT(char(10), dpp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		Isnull(sum(dpp.PayPlanGaku),0) 'PayPlanGaku',
		Isnull(sum(dpp.PayConfirmGaku),0) 'PayConfirmGaku',
		Isnull(sum(dpp.PayPlanGaku-dpp.PayConfirmGaku),0) 'PayGaku',
		Isnull(sum(dpp.PayPlanGaku-dpp.PayConfirmGaku),0) 'TransferGaku',
		Isnull(dbo.F_GetKouzaFee(max(fvpm.BankCD),max(fvpm.BranchCD),sum(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0)),max(fvpm.KouzaCD)),0) as 'TransferFeeGaku',
		'1' 'FeeKBN',
		'0' 'Gaku',
		Isnull(sum(dpp.PayPlanGaku-dpp.PayConfirmGaku),0) 'PayPlan',
		max(dpp.PayCloseNO) 'PayCloseNO',
		max(dpp.PayCloseDate) 'PayCloseDate',
		max(dpp.HontaiGaku8) 'HontaiGaku8',
		max(dpp.HontaiGaku10) 'HontaiGaku10',
		max(dpp.TaxGaku8) 'TaxGaku8',
		max(dpp.TaxGaku10) 'TaxGaku10'
		from D_PayPlan as dpp 
		left outer join F_Vendor(GETDATE()) as fvp on dpp.PayeeCD=fvp.VendorCD 
												and fvp.PayeeFlg=1 
												and fvp.DeleteFlg=0
		left outer join F_Vendor(GETDATE()) as fvpm on fvp.MoneyPayeeCD=fvpm.VendorCD 
												and fvpm.MoneyPayeeFlg=1
												and fvpm.DeleteFlg=0
		left outer join F_Kouza(GETDATE()) fz on fz.KouzaCD=fvpm.KouzaCD
												and fz.DeleteFlg=0
		left outer join F_Staff(GETDATE())fs on fs.StaffCD=@Operator  

		where dpp.PayConfirmFinishedKBN=0
		and dpp.DeleteDateTime is null
		and (@PayeePlanDateFrom is null) or (@PayeePlanDateFrom is not null and dpp.PayPlanDate>=@PayeePlanDateFrom)
		and (@PayeePlanDateTo is null) or (@PayeePlanDateTo is not null and dpp.PayPlanDate<=@PayeePlanDateTo)
		and fvp.MoneyPayeeCD=@PayeeCD
		Group by dpp.PayPlanDate
		order by PayPlanDate asc
	
END

