 BEGIN TRY 
 Drop Procedure dbo.[D_MonthlyClaims_SelectALL]
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
CREATE PROCEDURE [dbo].[D_MonthlyClaims_SelectALL]
	-- Add the parameters for the stored procedure here
	@TargetDate as Date,
	@CustomerCD as varchar(13),
	--@RdoBill as tinyint,
	--@RdoSale as tinyint,
	@Chkdo as tinyint,
	@StoreCD as varchar(4),
	@PrintType as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @PrintType=1--RdoBillType
	begin
	select 
	left(MAX(dmc.YYYYMM) ,4) + '/' + RIGHT(MAX(dmc.YYYYMM) ,2) as N'年月',
	MAX(dmc.StoreCD) as N'店舗CD',
	MAX(fs.StoreName) as N'店舗名',
	case when @printtype = 0 then MAX(fc.BillingCD) else MAX(fc.CustomerCD) end as CustomerCD,
	case when @printtype = 0 then (select CustomerName from F_Customer(@TargetDate) where CustomerCD = fc.BillingCD)  else MAX(fc.CustomerName) end as N'顧客名',
	SUM(CAST(dmc.LastBalanceGaku AS INT)) as N'前月債権残額',
	SUM(CAST(dmc.SalesHontaiGaku AS INT)) as N'売上額',
	SUM(CAST(dmc.SalesTax AS INT)) as N'消費税額',
	SUM(CAST(dmc.CollectGaku AS INT)) as N'当月入金額',
	SUM(CAST(dmc.OffsetGaku AS INT)) as N'うち相殺額',
	SUM(CAST(dmc.BalanceGaku AS INT)) as N'当月債権額'
	from D_MonthlyClaims dmc	
	inner join F_Store(@TargetDate) fs on fs.StoreCD = dmc.StoreCD
	inner join F_Customer(@TargetDate) fc on fc.CustomerCD = dmc.CustomerCD 
	and (@CustomerCD is null or (fc.BillingCD=@CustomerCD))

	where dmc.DeleteDateTime is null
	and dmc.YYYYMM=CONVERT(VARCHAR(6), @TargetDate, 112)
	and fs.StoreCD=@StoreCD	
	and(@Chkdo=0 OR (@Chkdo=1 AND (dmc.LastBalanceGaku !=0 OR  dmc.SalesHontaiGaku !=0 OR  dmc.SalesTax != 0 OR  dmc.CollectGaku !=0 OR  dmc.OffsetGaku !=0 OR dmc.BalanceGaku !=0)))--checkbox

	GROUP BY fc.BillingCD
	ORDER BY CustomerCD,fc.BillingCD  ASC

	end

	else if @PrintType=2--RdoSaletype
	begin
	select 
	left(dmc.YYYYMM,4) + '/' + RIGHT(dmc.YYYYMM,2) as N'年月',
	dmc.StoreCD as N'店舗CD',
	fs.StoreName as N'店舗名',
	case when @printtype = 0 then fc.BillingCD else fc.CustomerCD end as CustomerCD,
	case when @printtype = 0 then (select CustomerName from F_Customer(@TargetDate) where CustomerCD = fc.BillingCD)  else fc.CustomerName end as N'顧客名',
	CAST(dmc.LastBalanceGaku AS INT) as N'前月債権残額',
	CAST(dmc.SalesHontaiGaku AS INT) as N'売上額',
	CAST(dmc.SalesTax  AS INT) as N'消費税額',
	CAST(dmc.CollectGaku AS INT) as N'当月入金額',
	CAST(dmc.OffsetGaku AS INT) as N'うち相殺額',
	CAST(dmc.BalanceGaku AS INT) as N'当月債権額'
	from D_MonthlyClaims dmc	
	inner join F_Store(@TargetDate) fs on fs.StoreCD = dmc.StoreCD
	inner join F_Customer(@TargetDate) fc on fc.CustomerCD = dmc.CustomerCD

	where dmc.DeleteDateTime is null
	and dmc.YYYYMM=CONVERT(VARCHAR(6), @TargetDate, 112)
	and fs.StoreCD=@StoreCD
	and dmc.CustomerCD=CASE WHEN @PrintType=2 and  @CustomerCD is not null THEN  @CustomerCD ELSE dmc.CustomerCD end--RdoSale
	and(@Chkdo=0 OR (@Chkdo=1 AND (dmc.LastBalanceGaku !=0 OR  dmc.SalesHontaiGaku !=0 OR  dmc.SalesTax != 0 OR  dmc.CollectGaku !=0 OR  dmc.OffsetGaku !=0 OR dmc.BalanceGaku !=0)))--checkbox

	ORDER BY dmc.CustomerCD,fc.BillingCD ASC
	end

END
--GO

