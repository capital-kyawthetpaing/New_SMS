 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_SelectForFB]
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
CREATE PROCEDURE [dbo].[D_Pay_SelectForFB] 
	-- Add the parameters for the stored procedure here
	@MotoKouzaCD as varchar(3),
	@PayDate as date,
	@Flg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#Tmp_D_PaySelect](
	[PayDate] [Date],
	[KouzaName][varchar](50) collate Japanese_CI_AS,
	[PayeeCD][varchar](13) collate Japanese_CI_AS,
	[VendorName][varchar](50) collate Japanese_CI_AS,
	[BankCD][varchar](4) collate Japanese_CI_AS,
	[BankName][varchar](30) collate Japanese_CI_AS,
	[BranchCD][varchar](3) collate Japanese_CI_AS,
	[BranchName][varchar](30) collate Japanese_CI_AS,
	[KouzaKBN][tinyint],
	[KouzaNO][varchar](7) collate Japanese_CI_AS,
	[KouzaMeigi][varchar](40) collate Japanese_CI_AS,
	[transferAcc][varchar](60) collate Japanese_CI_AS,
	[TransferGaku] [money],
	[TransferFeeGaku][money],
	[FeeKBN][tinyint],
	[FeeKBN1][varchar](7) collate Japanese_CI_AS,
	[Errorflg] [tinyint]
	)

	insert into #Tmp_D_PaySelect

	Select 
	dp.PayDate,
	fk.KouzaName,
	dp.PayeeCD,
	fv.VendorName,
	dp.BankCD,
	fb.BankName,
	dp.BranchCD,
	fbs.BranchName,
	dp.KouzaKBN,
	dp.KouzaNO,
	dp.KouzaMeigi,
	case
	when dp.KouzaKBN = 1 then CONCAT(N'普通', ' ', dp.KouzaNO,' ', dp.KouzaMeigi)
	else CONCAT(N'当座', '', dp.KouzaNO,'', dp.KouzaMeigi)
	End as transferAcc,
	--dp.KouzaKBN,' 'dp.KouzaNO ' 'dp.KouzaMeigi as transferAcc,					
	dp.TransferGaku,
	dp.TransferFeeGaku,
	dp.FeeKBN,
	case 
	when dp.FeeKBN = 1 then N'自社'
	else N'相手負担'
	End as FeeKBN1,
	--0 as Error
	case 
	when fk.BankCD = dp.BankCD and fk.BranchCD = dp.BranchCD 
		    and dp.TransferGaku	< fk.Amount1
			and fk.Fee11 != dp.TransferFeeGaku then '1'
	when  dp.TransferGaku >= fk.Amount1 and fk.Fee12 != dp.TransferFeeGaku then '1'
	when fk.BankCD = dp.BankCD	and fk.BranchCD != dp.BranchCD	
		 and dp.TransferGaku	< fk.Amount2
		 and fk.Fee21 != dp.TransferFeeGaku then '1'
	when dp.TransferGaku	>=	fk.Amount2 and 
		 fk.Fee22 !=	dp.TransferFeeGaku then '1'
	when fk.BankCD != dp.BankCD and dp.TransferGaku	< fk.Amount3
		 and fk.Fee31	!= dp.TransferFeeGaku then '1'
	when dp.TransferGaku >=	fk.Amount3 and 
		fk.Fee32 !=	dp.TransferFeeGaku	then '1'
	else '0'
	End as Error
	From D_Pay dp 
	Inner Join F_Vendor(cast(@PayDate as varchar(10))) fv on fv.VendorCD = dp.PayeeCD
	and fv.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb on dp.BankCD = fb.BankCD
	and fb.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs	 on fbs.BankCD=dp.BankCD
	and dp.BranchCD=fbs.BranchCD and fbs.DeleteFlg = 0
	Left Outer Join F_Kouza(cast(@PayDate as varchar(10))) fk on fk.KouzaCD=dp.MotoKouzaCD 
	and fk.DeleteFlg = 0
	Where dp.DeleteDateTime is null 
	and dp.MotoKouzaCD = @MotoKouzaCD
	and dp.PayDate = @PayDate
	and dp.TransferGaku > 0
	--and case
	--	when @Flg = 0  then dp.FBCreateDate is null
	--	else dp.FBCreateDate is not null
	--end 
	and ((@Flg = 0 and dp.FBCreateDate is null) 
	or ((@Flg = 1  or @Flg = 2 )and dp.FBCreateDate is not null))
	Order by dp.PayeeCD asc
	
	Update  #Tmp_D_PaySelect 
	SET Errorflg = 1
	Where KouzaKBN NOT IN ('1', '2')
	or FeeKBN NOT IN ('1','2')
	or KouzaNO is null or KouzaMeigi is null

	select * from #Tmp_D_PaySelect

	drop table #Tmp_D_PaySelect

END