 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_SelectForPayPlanDate2]
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
CREATE PROCEDURE [dbo].[D_Pay_SelectForPayPlanDate2]
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
		--select * from
		--(select 
		--max(dpp.PayPlanNO) as 'PayPlanNO',
		--Replace(CONVERT(char(10), dpp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		--max(dpp.PayeeCD) 'PayeeCD',
		--max(fvpm.VendorName) 'VendorName',
		--max(dpp.Number) 'Number',
		--max(Replace(CONVERT(char(10), dpp.RecordedDate,126),'-','/')) as 'RecordedDate',
		--max(dpp.PayPlanGaku) 'PayPlanGaku',
		--max(dpp.PayConfirmGaku) 'PayConfirmGaku',
		--max(CONVERT(varchar(10),LEFT(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0), CHARINDEX('.',Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0))-1))) as 'UnpaidAmount1',
		--max(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0)) as 'UnpaidAmount2',
		--max(fvpm.BankCD) 'BankCD',
		--max(fb.BankName) 'BankName',
		--max(fvpm.BranchCD) 'BranchCD',
		--max(fbs.BranchName) 'BranchName',
		--max(fvpm.KouzaKBN) 'KouzaKBN',
		--max(fvpm.KouzaNO) 'KouzaNO',
		--max(fvpm.KouzaMeigi) 'KouzaMeigi',
		--'1' as 'FeeKBN',
		--dbo.F_GetKouzaFee(max(fvpm.BankCD),max(fvpm.BranchCD),sum(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0)),max(fvpm.KouzaCD)) as 'Fee',
		--0 as 'CashGaku',
		--0 as 'OffsetGaku',
		--0 as 'BillGaku',
		--Null as 'BillDate',
		--Null as 'BillNO',
		--0 as 'ERMCGaku',
		--Null as 'ERMCNO',
		--Null as 'ERMCDate',
		--0 as 'OtherGaku1',
		--Null as 'Account1',
		--Null as 'SubAccount1',
		--0 as 'OtherGaku2',
		--Null as 'Account2',
		--Null as 'SubAccount2'


		--from D_PayPlan as dpp 
		--left outer join F_Vendor(GETDATE()) as fvp on dpp.PayeeCD=fvp.VendorCD 
		--										and fvp.PayeeFlg=1
		--left outer join F_Vendor(GETDATE()) as fvpm on fvp.MoneyPayeeCD=fvpm.VendorCD 
		--										and fvpm.MoneyPayeeFlg=1
		--left outer join F_Bank(GETDATE()) fb on fb.BankCD=fvpm.BankCD
		--left outer join F_BankShiten(GETDATE()) fbs on fbs.BranchCD=fvpm.BranchCD
		--where dpp.PayConfirmFinishedKBN=0
		--and dpp.DeleteDateTime is null
		--and fvp.DeleteFlg=0
		--and fvpm.DeleteFlg=0
		--and fb.DeleteFlg=0
		--and fbs.DeleteFlg=0
		--and fvp.MoneyPayeeCD=@PayeeCD
		--and dpp.PayPlanDate=@PayPlanDate
		--Group by dpp.PayPlanDate)as t1
		--order by t1.RecordedDate,t1.Number asc

		select 
		dpp.PayPlanNO as 'PayPlanNO',
		Replace(CONVERT(char(10), dpp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		dpp.PayeeCD 'PayeeCD',
		fvpm.VendorName 'VendorName',
		--'' as 'TransferGaku',
		dpp.Number 'Number',
		Replace(CONVERT(char(10), dpp.RecordedDate,126),'-','/') as 'RecordedDate',
		dpp.PayPlanGaku 'PayPlanGaku',
		dpp.PayConfirmGaku 'PayConfirmGaku',
		Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) as 'UnpaidAmount1',
		Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) as 'UnpaidAmount2',
		CONVERT(varchar(10),LEFT(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0), CHARINDEX('.',Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0))-1)) as 'TransferGaku',
		fvpm.BankCD 'BankCD',
		fb.BankName 'BankName',
		fvpm.BranchCD 'BranchCD',
		fbs.BranchName 'BranchName',
		fvpm.KouzaKBN 'KouzaKBN',
		fvpm.KouzaNO 'KouzaNO',
		fvpm.KouzaMeigi 'KouzaMeigi',
		'1' as 'FeeKBN',
		dbo.F_GetKouzaFee(fvpm.BankCD,fvpm.BranchCD,t1.PayPlanGaku,fvpm.KouzaCD) as 'Fee',
		0 as 'CashGaku',
		0 as 'OffsetGaku',
		0 as 'BillGaku',
		'' as 'BillDate',
		'' as 'BillNO',
		0 as 'ERMCGaku',
		'' as 'ERMCNO',
		'' as 'ERMCDate',
		0 as 'OtherGaku1',
		'' as 'Account1',
		'' as 'start1',
		'' as 'SubAccount1',
		'' as 'end1label',
		0 as 'OtherGaku2',
		'' as 'Account2',
		'' as 'start2',
		'' as 'SubAccount2' ,
		'' as 'end2label'

from D_PayPlan as dpp 
inner join (
select sum(dpp.PayPlanGaku-PayConfirmGaku) as PayPlanGaku,fvp.MoneyPayeeCD,dpp.PayPlanDate
from D_PayPlan as dpp 
		left outer join F_Vendor(GETDATE()) as fvp on dpp.PayeeCD=fvp.VendorCD 
												and fvp.PayeeFlg=1 and fvp.DeleteFlg=0
		left outer join F_Vendor(GETDATE()) as fvpm on fvp.MoneyPayeeCD=fvpm.VendorCD 
												and fvpm.MoneyPayeeFlg=1 and fvpm.DeleteFlg=0
		left outer join F_Bank(GETDATE()) fb on fb.BankCD=fvpm.BankCD and fb.DeleteFlg=0
		left outer join F_BankShiten(GETDATE()) fbs on fbs.BranchCD=fvpm.BranchCD and fbs.DeleteFlg=0
		where dpp.PayConfirmFinishedKBN=0
		and dpp.DeleteDateTime is null
		and fvp.MoneyPayeeCD=@PayeeCD
		and (@PayeePlanDateFrom is null) or (@PayeePlanDateFrom is not null and dpp.PayPlanDate>=@PayeePlanDateFrom)
		and (@PayeePlanDateTo is null) or (@PayeePlanDateTo is not null and dpp.PayPlanDate<=@PayeePlanDateTo)
group by fvp.MoneyPayeeCD,dpp.PayPlanDate)  t1 on dpp.PayeeCD = t1.MoneyPayeeCD and dpp.PayPlanDate = t1.PayPlanDate
		left outer join F_Vendor(GETDATE()) as fvp on dpp.PayeeCD=fvp.VendorCD 
												and fvp.PayeeFlg=1 and fvp.DeleteFlg=0
		left outer join F_Vendor(GETDATE()) as fvpm on fvp.MoneyPayeeCD=fvpm.VendorCD 
												and fvpm.MoneyPayeeFlg=1 and fvpm.DeleteFlg=0
		left outer join F_Bank(GETDATE()) fb on fb.BankCD=fvpm.BankCD and fb.DeleteFlg=0
		left outer join F_BankShiten(GETDATE()) fbs on fbs.BranchCD=fvpm.BranchCD and fbs.DeleteFlg=0
		where dpp.PayConfirmFinishedKBN=0
		and dpp.DeleteDateTime is null
		and fvp.MoneyPayeeCD=@PayeeCD
		and (@PayeePlanDateFrom is null) or (@PayeePlanDateFrom is not null and dpp.PayPlanDate>=@PayeePlanDateFrom)
		and (@PayeePlanDateTo is null) or (@PayeePlanDateTo is not null and dpp.PayPlanDate<=@PayeePlanDateTo)
		order by dpp.RecordedDate,dpp.Number



	
END

