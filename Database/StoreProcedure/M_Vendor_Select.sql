 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Select]
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
CREATE PROCEDURE [dbo].[M_Vendor_Select] 
	-- Add the parameters for the stored procedure here
	@VendorCD varchar(12),
	@ChangeDate date
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
		mv.ShoguchiFlg,
		mv.VendorName,
		mv.VendorShortName, --Add By SawLay
		mv.VendorLongName1,
		mv.VendorLongName2,
		mv.VendorPostName,
		mv.VendorPositionName,
		mv.VendorStaffName,
		mv.VendorKana,
		mv.PayeeFlg,
		mv.MoneyPayeeFlg,
		mv.PayeeCD,
		mv.MoneyPayeeCD,
		mv.ZipCD1,
		mv.ZipCD2,
		mv.Address1,
		mv.Address2,
		mv.MailAddress1,
		mv.TelephoneNO,
		mv.FaxNO,
		mv.PaymentCloseDay,
		mv.PaymentPlanKBN,
		mv.PaymentPlanDay,
		mv.HolidayKBN, 
		mv.TaxTiming, --Add By SawLay
		mv.TaxFractionKBN, --Add By SawLay
		mv.AmountFractionKBN, --Add By SawLay
		mv.BankCD,
		fb.BankName,
		mv.BranchCD,
		fbs.BranchName,
		mv.KouzaKBN,
		mv.KouzaNO,
		mv.KouzaMeigi,
		mv.KouzaCD,
		mfk.KouzaName,
		mv.NetFlg,
		mv.EDIFlg,  --Add By SawLay
		mv.EDIVendorCD,  --Add By SawLay
		mv.LastOrderDate,
		mv.StaffCD,
		fs.StaffName,
		mv.AnalyzeCD1,
		mv.AnalyzeCD2,
		mv.AnalyzeCD3,
		mv.DisplayOrder,
		mv.NotDisplyNote,
		mv.DisplayNote,
		mv.DeleteFlg
	From M_Vendor mv 
	left join F_Bank(cast(@ChangeDate as varchar(10))) fb on mv.BankCD = fb.BankCD
	left join  F_BankShiten(cast(@ChangeDate as varchar(10))) fbs	 on mv.BranchCD=fbs.BranchCD and fbs.BankCD=mv.BankCD
	left join F_Kouza(cast(@ChangeDate as varchar(10))) mfk on mv.KouzaCD=mfk.KouzaCD
	left join F_Staff(cast(@ChangeDate as varchar(10))) fs on mv.StaffCD=fs.StaffCD
	--outer apply (select mbs.BranchCD,mbs.BranchName,mbs.ChangeDate from M_BankShiten mbs inner join F_BankShiten(mv.BankCD,cast(@ChangeDate as varchar(10))) fbs
	--	 on mbs.BranchCD=fbs.BranchCD) mfbs
	--left join ( Select mk.KouzaCD,mk.KouzaName,mk.DeleteFlg From M_Kouza mk inner join F_Kouza(cast(@ChangeDate as varchar(10))) fk 
	--on mk.KouzaCD = fk.KouzaCD) mfk on mv.KouzaCD  = mfk.KouzaCD
	--left join (Select ms.StaffCD,ms.StaffName,ms.LeaveDate ,ms.DeleteFlg From M_Staff ms inner join F_Staff(cast(@ChangeDate as varchar(10))) fs
	--on ms.StaffCD = fs.StaffCD) mfs on mv.StaffCD = mfs.StaffCD
	Where (@VendorCD is null or(mv.VendorCD = @VendorCD))
and (@ChangeDate is null or (mv.ChangeDate = @ChangeDate))
END


