 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_SelectForText]
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
CREATE PROCEDURE [dbo].[D_Pay_SelectForText] 
	-- Add the parameters for the stored procedure here
	@MotoKouzaCD as varchar(3),
	@PayDate as date,
	@Flg as tinyint,
	@ActualDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 Select 
	Distinct 1 as データ区分,
	21 as 種別コード,
	0 as コード区分,
	fk.CompanyCD as 会社コード,
	fk.CompanyName as 会社名,
	CONVERT(varchar(10), @ActualDate) as 振込指定日,
	fk.BankCD as 仕向銀行番号,
	fb2.BankKana as 仕向銀行名,
	fk.BranchCD as 仕向支店番号,
	fbs2.BranchName as 仕向支店名,
	fk.KouzaKBN as 預金種目, 
	fk.KouzaNO as 口座番号,
	SPACE(1),
	SPACE(1),
	SPACE(1),
	SPACE(17) as ダミー
	
	From D_Pay dp
	Inner Join F_Vendor(cast(@PayDate as varchar(10))) fv on fv.VendorCD = dp.PayeeCD
	and fv.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb1 on dp.BankCD = fb1.BankCD
	and fb1.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs1 on fbs1.BankCD=dp.BankCD
	and dp.BranchCD=fbs1.BranchCD and fbs1.DeleteFlg = 0
	Left Outer Join F_Kouza(cast(@PayDate as varchar(10))) fk on fk.KouzaCD=dp.MotoKouzaCD 
	and fk.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb2 on fk.BankCD = fb2.BankCD
	and fb2.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs2 on fbs2.BankCD=fk.BankCD
	and fk.BranchCD=fbs2.BranchCD and fbs2.DeleteFlg = 0
	Where dp.DeleteDateTime is null 
	and dp.MotoKouzaCD = @MotoKouzaCD
	and dp.PayDate = @PayDate
	and dp.TransferGaku > 0
	and (@Flg = 0 and dp.FBCreateDate is null) 
	--Order by dp.PayeeCD asc

	UNION ALL

	Select 
	 Distinct 2 as データ区分,
	dp.BankCD as 被仕向銀行番号,
	fb1.BankKana as 被仕向銀行名,
	dp.BranchCD as	被仕向支店番号,				
	fbs1.BranchKana	 as	被仕向支店名,
	CONVERT(varchar(10), 0000) as 手形交換所番号,					
	CONVERT(varchar(10), dp.KouzaKBN)as 預金種目,				
	dp.KouzaNO as 口座番号,					
	dp.KouzaMeigi as 受取人名,					
	CONVERT(varchar(10), dp.TransferGaku)as 振込金額,				
	CONVERT(varchar(10), 0) as 新規コード,				
	SPACE(10) as 顧客コード1,					
	SPACE(10) as 顧客コード2,
	7 as 振込指定区分,					
	SPACE(1) as 識別表示,					
	SPACE(7) as ダミー
	From D_Pay dp
	Inner Join F_Vendor(cast(@PayDate as varchar(10))) fv on fv.VendorCD = dp.PayeeCD
	and fv.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb1 on dp.BankCD = fb1.BankCD
	and fb1.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs1 on fbs1.BankCD=dp.BankCD
	and dp.BranchCD=fbs1.BranchCD and fbs1.DeleteFlg = 0
	Left Outer Join F_Kouza(cast(@PayDate as varchar(10))) fk on fk.KouzaCD=dp.MotoKouzaCD 
	and fk.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb2 on fk.BankCD = fb2.BankCD
	and fb2.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs2 on fbs2.BankCD=fk.BankCD
	and fk.BranchCD=fbs2.BranchCD and fbs2.DeleteFlg = 0
	Where dp.DeleteDateTime is null 
	and dp.MotoKouzaCD = @MotoKouzaCD
	and dp.PayDate = @PayDate
	and dp.TransferGaku > 0
	and (@Flg = 0 and dp.FBCreateDate is null) 	


	UNION ALL

	Select 
	Distinct 8 as データ区分,
	CONVERT(varchar(10), 0) as 合計件数,
	CONVERT(varchar(10), 0) as 合計金額,
	SPACE(1) ,			
	SPACE(1) ,
	SPACE(1) ,					
	SPACE(1) ,		
	SPACE(1) ,			
	SPACE(1) ,			
	SPACE(1) ,		
	SPACE(1) ,		
	SPACE(1) ,				
	SPACE(1) ,
	SPACE(1) ,				
	SPACE(1) ,
	SPACE(101) as ダミー
	From D_Pay dp
	Inner Join F_Vendor(cast(@PayDate as varchar(10))) fv on fv.VendorCD = dp.PayeeCD
	and fv.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb1 on dp.BankCD = fb1.BankCD
	and fb1.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs1 on fbs1.BankCD=dp.BankCD
	and dp.BranchCD=fbs1.BranchCD and fbs1.DeleteFlg = 0
	Left Outer Join F_Kouza(cast(@PayDate as varchar(10))) fk on fk.KouzaCD=dp.MotoKouzaCD 
	and fk.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb2 on fk.BankCD = fb2.BankCD
	and fb2.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs2 on fbs2.BankCD=fk.BankCD
	and fk.BranchCD=fbs2.BranchCD and fbs2.DeleteFlg = 0
	Where dp.DeleteDateTime is null 
	and dp.MotoKouzaCD = @MotoKouzaCD
	and dp.PayDate = @PayDate
	and dp.TransferGaku > 0
	and (@Flg = 0 and dp.FBCreateDate is null) 	


	UNION ALL
	Select 
	Distinct 9 as データ区分,
	SPACE(1),
	SPACE(1),
	SPACE(1) ,			
	SPACE(1) ,
	SPACE(1) ,					
	SPACE(1) ,		
	SPACE(1) ,			
	SPACE(1) ,			
	SPACE(1) ,		
	SPACE(1) ,		
	SPACE(1) ,				
	SPACE(1) ,
	SPACE(1) ,				
	SPACE(1) ,
	SPACE(119) as ダミー
	From D_Pay dp
	Inner Join F_Vendor(cast(@PayDate as varchar(10))) fv on fv.VendorCD = dp.PayeeCD
	and fv.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb1 on dp.BankCD = fb1.BankCD
	and fb1.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs1 on fbs1.BankCD=dp.BankCD
	and dp.BranchCD=fbs1.BranchCD and fbs1.DeleteFlg = 0
	Left Outer Join F_Kouza(cast(@PayDate as varchar(10))) fk on fk.KouzaCD=dp.MotoKouzaCD 
	and fk.DeleteFlg = 0
	Left Outer Join F_Bank(cast(@PayDate as varchar(10))) fb2 on fk.BankCD = fb2.BankCD
	and fb2.DeleteFlg = 0
	Left Outer Join F_BankShiten(cast(@PayDate as varchar(10))) fbs2 on fbs2.BankCD=fk.BankCD
	and fk.BranchCD=fbs2.BranchCD and fbs2.DeleteFlg = 0
	Where dp.DeleteDateTime is null 
	and dp.MotoKouzaCD = @MotoKouzaCD
	and dp.PayDate = @PayDate
	and dp.TransferGaku > 0
	and (@Flg = 0 and dp.FBCreateDate is null) 

End