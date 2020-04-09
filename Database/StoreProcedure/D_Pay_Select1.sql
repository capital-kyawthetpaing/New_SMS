 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Select1]
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
CREATE PROCEDURE [dbo].[D_Pay_Select1]
	-- Add the parameters for the stored procedure here
	@LargePayNo as varchar(11),
	@PayNo as varchar(11)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--Select * 
	--From D_Pay dp
	--Where dp.DeleteDateTime is null
	--and dp.FBCreateDate is null
	--and (@LargePayNo is Null) or (@LargePayNo is not Null) and dp.LargePayNO = @LargePayNo
	--and (@PayNo is null) or (@PayNo is not null) and dp.PayNO = @PayNo
	--Order by dp.PayNO asc
	CREATE TABLE [dbo].[#Temp_D_PaySelect](
	[LargePayNO][varchar](11) collate Japanese_CI_AS,
	[PayNO][varchar](11) collate Japanese_CI_AS,
	[PayPlanGaku][money],
	[PayConfirmGaku][money])


		Insert Into #Temp_D_PaySelect
		select 
		dp.LargePayNO,
		dp.PayNO,
		Sum(dpp.PayPlanGaku) PayPlanGaku,
		Sum(dpp.PayConfirmGaku) PayConfirmGaku
		from D_PayDetails dpd
		left outer join D_PayPlan dpp on dpp.PayPlanNO = dpd.PayPlanNO 
		left Outer join D_Pay dp on dp.PayNO = dpd.PayNO
		where dpd.DeleteDateTime is null 
		and dpp.DeleteDateTime is null 
		and dp.DeleteDateTime is null
		and dp.FBCreateDate is null 
		and (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
		and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
		group by dp.LargePayNO,dp.PayNO


		select 
		Replace(CONVERT(char(10), dp.PayDate,126),'-','/') as 'PayDate',
		dp.StaffCD,
		(select StaffName from F_Staff(GETDATE()) where StaffCD=dp.StaffCD and DeleteFlg=0) as 'StaffName',
		dp.PayeeCD,
		fv.VendorName,
		Replace(CONVERT(char(10), dp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		Isnull(#tdp.PayPlanGaku,0) as PayPlanGaku,
		Isnull(#tdp.PayConfirmGaku,0) as PayConfirmGaku ,
		Isnull(#tdp.PayPlanGaku,0)- Isnull(#tdp.PayConfirmGaku,0) as 'PayGaku',
		Isnull(dp.TransferGaku,0) 'TransferGaku',
		Isnull(dp.TransferFeeGaku,0) 'TransferFeeGaku',
		case when dp.FeeKBN=1 then N'自社' else N'相手負担' end 'FeeKBN',
		Isnull(dp.CashGaku,0) + Isnull(dp.BillGaku,0) + Isnull(dp.ERMCGaku,0) + Isnull(dp.CardGaku,0) + Isnull(dp.OffsetGaku,0) + Isnull(dp.OtherGaku1,0) + Isnull(dp.OtherGaku2,0) as 'Gaku',
		Isnull(#tdp.PayPlanGaku,0)- Isnull(#tdp.PayConfirmGaku,0) as 'PayPlan',
		Isnull(dp.PayCloseNO,0) as 'PayCloseNO',
		'' 'PayCloseDate',
		'' 'HontaiGaku8',
		'' 'HontaiGaku10',
		'' 'TaxGaku8',
		'' 'TaxGaku10'
		
		from D_Pay as dp
		left outer join #Temp_D_PaySelect as #tdp on #tdp.LargePayNO=dp.LargePayNO 
												and #tdp.PayNO=dp.PayNO
		left outer join F_Vendor(getdate()) as fv on fv.VendorCD=dp.PayeeCD
											and fv.MoneyPayeeFlg=1
		where dp.DeleteDateTime is null
		and dp.FBCreateDate is null
		and fv.DeleteFlg=0
		and (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
		and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
		order by dp.PayNO asc
	
END

