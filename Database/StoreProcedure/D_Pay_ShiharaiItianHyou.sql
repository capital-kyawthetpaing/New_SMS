 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_ShiharaiItianHyou]
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
CREATE PROCEDURE [dbo].[D_Pay_ShiharaiItianHyou]
	-- Add the parameters for the stored procedure here
	@PurchaseDateFrom  as date,
	@PurchaseDateTo as date,
	@VendorCD   varchar(13),
	@StaffCD  varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @Date as date=getdate();

select  CONVERT(varchar, dp.Paydate,111)  as PayDate
	,(fv.VendorCD +' '+fv.VendorName)as PurchaseData
	,cast(Floor(dp.PayGaku)as int) as PayGaku
	,dp.PayNO as PayNo
	,dp.LargePayNO as  LargePayNo

,(case when cast(dp.TransferFeeGaku as int ) <> 0 then '振込' else '' end)  as 'Transfer',
 (case when  cast(dp.CashGaku as int ) <> 0 then '現金' else '' end)  as 'Cash',
 (case when  cast(dp.BillGaku as int ) <> 0 then '手形' else '' end)  as 'Bill',
 (case when  cast(dp.ERMCGaku as int ) <> 0 then '電債' else '' end)  as 'Ebound' ,
 (case when  cast(dp.OffsetGaku as int ) <> 0 then '相殺' else '' end)  as 'Offset' ,
 (case when  cast(dp.OtherGaku1 as int ) <> 0 then 'その他①' else '' end)  as 'Other1' ,
 (case when  cast(dp.OtherGaku2 as int ) <> 0 then 'その他②' else '' end)  as 'Other2' ,

  (case when cast(dp.TransferFeeGaku as int) <> 0 then  cast(Floor(dp.TransferFeeGaku) as int) else '' end)  as 'Purchase1',
		(case when cast(dp.CashGaku as int) <>0 then cast(Floor(dp.CashGaku) as int ) else '' end ) as  'Purchase2',
	   (case when cast(dp.BillGaku as int) <>0 then cast(Floor(dp.BillGaku) as int ) else '' end ) as  'Purchase3',
	    (case when cast(dp.ERMCGaku as int) <>0 then cast(Floor(dp.ERMCGaku) as int ) else '' end ) as  'Purchase4',
		(case when cast(dp.OffsetGaku as int) <>0 then  cast(Floor(dp.OffsetGaku) as int )  else '' end ) as  'Purchase5',
		(case when cast(dp.OtherGaku1 as int) <>0 then cast(Floor(dp.OtherGaku1) as int ) else '' end ) as  'Purchase6',
		(case when cast(dp.OtherGaku2 as int) <>0 then cast(Floor(dp.OtherGaku2) as int ) else '' end ) as  'Purchase7',

   (case when dp.TransferFeeGaku <> 0 then fb.BankName else '' end)  as 'BankName',
   (case when cast(dp.CashGaku as int)  <> 0 then  NULL else ''end )as 'TransferNo' ,
    (case when cast(dp.BillGaku as int)  <> 0 then CONVERT(varchar,dp.BillDate,111)  else ''end )as 'BillDate' ,--9999/99/99
	 (case when cast(dp.ERMCGaku as int)  <> 0 then  CONVERT(varchar,dp.ERMCDate,111) else ''end )as 'ERMCDate' ,
	 (case when cast(dp.OffsetGaku as int)  <> 0 then NULL else ''end )as 'OffsetNo' ,		
		 (case when cast(dp.OtherGaku1 as int)  <> 0 then NULL else ''end )as 'OtherNo1' ,
		 (case when cast(dp.OtherGaku2 as int)  <> 0 then NULL else ''end )as 'OtherNo2' ,

    (case when dp.TransferFeeGaku <> 0 then fbsh.BranchName else '' end)  as 'BranchName',	 
	 (case when cast(dp.CashGaku as int)  <> 0 then  NULL else ''end )as 'TransferLargeNo',	
	 (case when cast(dp.BillGaku as int)  <> 0 then  NULL else ''end )as 'BillLargeNo',		
	(case when cast(dp.ERMCGaku as int)  <> 0 then  NULL else ''end )as 'ERMCLargeNo',		 
	 (case when cast(dp.OffsetGaku as int)  <> 0 then  NULL else ''end )as 'OffsetLargeNo',
	 (case when cast(dp.OtherGaku1 as int)  <> 0 then  NULL else ''end )as 'OtherLargeNo1',	
	(case when cast(dp.OtherGaku2 as int)  <> 0 then  NULL else ''end )as 'OtherLargeNo2'

 from D_Pay dp
 left outer join  F_Vendor(cast(@Date as varchar(10))) as fv
 on fv.VendorCD=dp.PayeeCD

 left outer join F_Bank(cast(@Date as varchar(10))) as fb
 on fb.BankCD=dp.BankCD

 left outer join F_BankShiten(cast(@Date as varchar(10))) as fbsh
 on fbsh.BranchCD=dp.BranchCD

 WHERE dp.DeleteDateTime is  null
 AND ISNULL(fv.DeleteFlg,0) =0
 AND ISNULL(fb.DeleteFlg,0)=0
 AND ISNULL(fbsh.DeleteFlg,0)=0
 AND (@PurchaseDateFrom is null or ( dp.PayDate >=@PurchaseDateFrom))--PurchaseDateFrom
 AND(@PurchaseDateTo is null or (  dp.PayDate <=@PurchaseDateTo))--PurchaseDateTo
AND (@VendorCD is null or (dp.PayeeCD=@VendorCD))
 AND(@StaffCD is null or ( dp.StaffCD=@StaffCD))

 ORDER BY dp.PayeeCD , dp.PayDate,dp.PayNO,dp.LargePayNO asc


 --exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END
