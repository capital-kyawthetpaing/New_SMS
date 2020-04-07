 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Select2]
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
CREATE PROCEDURE [dbo].[D_Pay_Select2]
	-- Add the parameters for the stored procedure here
	@LargePayNo as varchar(11),
	@PayNo as varchar(11),
	@VendorCD varchar(6),
	@PayeeDate date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		select Replace(CONVERT(char(10), dp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		dp.PayeeCD,
		fv.VendorName,
		dp.TransferGaku,
		CAST(dp.TransferGaku AS int) as TransferGaku,
		dp.BankCD,
		fb.BankName,
		dp.BranchCD,
		fbs.BranchName,
		dp.KouzaKBN,
		dp.KouzaNO,
		dp.KouzaMeigi,
		dp.FeeKBN,
		--dp.TransferFeeGaku as 'Fee',
		CAST(dp.TransferFeeGaku AS int) as 'Fee',
		--dp.CashGaku,
		CAST(dp.CashGaku AS int) as CashGaku,
		--dp.OffsetGaku,
		CAST(dp.OffsetGaku AS int) as OffsetGaku,
		--dp.BillGaku,
		CAST(dp.BillGaku AS int) as BillGaku,
		dp.BillNO,
		Replace(CONVERT(char(10), dp.BillDate,126),'-','/') as 'BillDate',
		--dp.ERMCGaku,
		CAST(dp.ERMCGaku AS int) as ERMCGaku,
		dp.ERMCNO,
		Replace(CONVERT(char(10), dp.ERMCDate,126),'-','/') as 'ERMCDate',
		--dp.OtherGaku1,
		CAST(dp.OtherGaku1 AS int) as OtherGaku1,
		dp.Account1,
		mmp11.Char1 as start1,
		dp.SubAccount1,
		mmp21.Char3 as end1label,
		--dp.OtherGaku2,
		CAST(dp.OtherGaku2 AS int) as OtherGaku2,
		dp.Account2,
		mmp12.Char1 as start2,
		dp.SubAccount2,
		mmp22.Char3 as end2label

		--convert(varchar(20), dp.CashGaku, 0)

		from D_Pay as dp
		left outer join F_Vendor(getdate()) as fv on fv.VendorCD=dp.PayeeCD
											and fv.MoneyPayeeFlg=1
											and fv.DeleteFlg=0
		left outer join F_Bank(GETDATE()) as fb on fb.BankCD=dp.BankCD
											and fb.DeleteFlg=0
		left outer join F_BankShiten(GETDATE()) as fbs on fbs.BankCD=dp.BankCD
												and fbs.BranchCD=dp.BranchCD
												and fbs.DeleteFlg=0
		left outer join M_MultiPorpose as mmp11 on mmp11.ID = '217' 
												and mmp11.[Key] = dp.Account1 
		left Outer join M_MultiPorpose as mmp12 on mmp12.ID = '217' 
												and  mmp12.[Key] = dp.Account2
		left outer join M_MultiPorpose as mmp21 on mmp21.ID = '218' 
												and mmp21.Char1 = dp.Account1 
												and mmp21.Char2 = dp.SubAccount1
		left Outer join M_MultiPorpose as mmp22 on mmp22.ID = '218'		
												and  mmp22.Char1 = dp.Account2  
												and mmp22.Char2 = dp.SubAccount2

		where dp.DeleteDateTime is null
		and dp.FBCreateDate is null
		and (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
		and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
		and( @VendorCD is null or dp.PayeeCD=@VendorCD)
		and( @PayeeDate is null or dp.PayPlanDate=@PayeeDate)
		order by dp.PayNO asc
	
END

