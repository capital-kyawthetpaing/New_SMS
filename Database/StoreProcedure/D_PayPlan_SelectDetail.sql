 BEGIN TRY 
 Drop Procedure dbo.[D_PayPlan_SelectDetail]
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
CREATE PROCEDURE [dbo].[D_PayPlan_SelectDetail]
    -- Add the parameters for the stored procedure here
    @PayeePlanDateFrom date,
    @PayeePlanDateTo date,
    @Operator varchar(11),
    @PayeeCD varchar(13)
    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    --支払登録
    --画面項目転送表05（第二画面明細部）
    -------画面項目転送表05（第二画面ヘッダ部・明細部）
    SELECT 
        dpp.PayPlanNO as 'PayPlanNO',
        CONVERT(varchar, dpp.PayPlanDate,111) as 'PayPlanDate',
        dpp.PayeeCD 'PayeeCD',
        fvpm.VendorName 'VendorName',
        --'' as 'TransferGaku',
        1 as Chk,
        dpp.Number 'Number',
        CONVERT(varchar, dpp.RecordedDate,111) as 'RecordedDate',
        dpp.PayPlanGaku 'PayPlanGaku',
        dpp.PayConfirmGaku 'PayConfirmGaku',
        Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) as 'UnpaidAmount1',
        --Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) as 'UnpaidAmount2'--,
        0 AS 'UnpaidAmount2',
        0 AS 'OldKingaku'
        /*
        CONVERT(varchar(10),LEFT(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0), CHARINDEX('.',Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0))-1)) as 'TransferGaku',
        fvpm.BankCD 'BankCD',
        fb.BankName 'BankName',
        fvpm.BranchCD 'BranchCD',
        fbs.BranchName 'BranchName',
        fvpm.KouzaKBN 'KouzaKBN',
        fvpm.KouzaNO 'KouzaNO',
        fvpm.KouzaMeigi 'KouzaMeigi',
        1 as 'FeeKBN',	--1：自社、2：相手負担
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
*/
    FROM D_PayPlan as dpp 
/*    inner join (
        select sum(dpp.PayPlanGaku-PayConfirmGaku) as PayPlanGaku,fvp.MoneyPayeeCD,dpp.PayPlanDate
        from D_PayPlan as dpp 
        left outer join F_Vendor(GETDATE()) as fvp 
        on dpp.PayeeCD=fvp.VendorCD 
        and fvp.PayeeFlg=1 
        and fvp.DeleteFlg=0
        left outer join F_Vendor(GETDATE()) as fvpm 
        on fvp.MoneyPayeeCD=fvpm.VendorCD 
        and fvpm.MoneyPayeeFlg=1 
        and fvpm.DeleteFlg=0
        left outer join F_Bank(GETDATE()) fb 
        on fb.BankCD=fvpm.BankCD 
        and fb.DeleteFlg=0
        left outer join F_BankShiten(GETDATE()) fbs 
        on fbs.BranchCD=fvpm.BranchCD 
        and fbs.BankCD=fvpm.BankCD
        and fbs.DeleteFlg=0
        where dpp.PayConfirmFinishedKBN=0
        and dpp.DeleteDateTime is null
        and fvp.MoneyPayeeCD=@PayeeCD
        and (@PayeePlanDateFrom is null) or (@PayeePlanDateFrom is not null and dpp.PayPlanDate>=@PayeePlanDateFrom)
        and (@PayeePlanDateTo is null) or (@PayeePlanDateTo is not null and dpp.PayPlanDate<=@PayeePlanDateTo)
    group by fvp.MoneyPayeeCD,dpp.PayPlanDate)  t1 
    on dpp.PayeeCD = t1.MoneyPayeeCD 
    and dpp.PayPlanDate = t1.PayPlanDate
*/
    left outer join F_Vendor(GETDATE()) as fvp 
    on dpp.PayeeCD=fvp.VendorCD 
    and fvp.PayeeFlg=1 
    and fvp.DeleteFlg=0
    
    left outer join F_Vendor(GETDATE()) as fvpm 
    on fvp.MoneyPayeeCD=fvpm.VendorCD 
    and fvpm.MoneyPayeeFlg=1 
    and fvpm.DeleteFlg=0
    /*
    left outer join F_Bank(GETDATE()) fb 
    on fb.BankCD=fvpm.BankCD 
    and fb.DeleteFlg=0
    
    left outer join F_BankShiten(GETDATE()) fbs 
    on fbs.BranchCD=fvpm.BranchCD
    and fbs.BankCD=fvpm.BankCD
    and fbs.DeleteFlg=0
    */
    WHERE dpp.PayConfirmFinishedKBN=0
    and dpp.DeleteDateTime is null
    and dpp.PayCloseNO IS NOT NULL
    and fvp.MoneyPayeeCD = (CASE WHEN @PayeeCD <> '' THEN @PayeeCD ELSE fvp.MoneyPayeeCD END)
    and ((@PayeePlanDateFrom is null) or (@PayeePlanDateFrom is not null and dpp.PayPlanDate>=@PayeePlanDateFrom))
    and ((@PayeePlanDateTo is null) or (@PayeePlanDateTo is not null and dpp.PayPlanDate<=@PayeePlanDateTo))
    ORDER by dpp.PayeeCD, dpp.PayPlanDate, dpp.RecordedDate,dpp.Number
    ;

END

