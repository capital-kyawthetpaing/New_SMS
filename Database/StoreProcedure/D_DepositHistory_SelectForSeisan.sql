

/****** Object:  StoredProcedure [dbo].[D_DepositHistory_SelectForSeisan]    Script Date: 2020/11/13 20:46:08 ******/
DROP PROCEDURE [dbo].[D_DepositHistory_SelectForSeisan]
GO

/****** Object:  StoredProcedure [dbo].[D_DepositHistory_SelectForSeisan]    Script Date: 2020/11/13 20:46:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_DepositHistory_SelectForSeisan]
	-- Add the parameters for the stored procedure here
	@StoreCD as VarChar(6) ,
	@Date as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create table #tmp_Deposit‚gistory
	(
		CashSale  Money,
		Gift  Money,
		CashDeposit Money,
		CashPayment Money,
		DepositTransfer Money,
		DepositCash Money,
		DepositCheck Money,
		DepositBill Money,
		DepositOffset Money,
		DepositAdjustment Money,
		DepositReturns Money,
		DepositDiscount Money,
		DepositCancel Money,
		PaymentTransfer Money,
		PaymentCash Money,
		Paymentcheck Money,
		PaymentBill Money,
		PaymentOffset Money,
		PaymentAdjustment Money,
		Change Money,
		DepositDateTime datetime
	)
		insert into #tmp_Deposit‚gistory(CashSale,Gift ,CashDeposit ,CashPayment ,DepositTransfer ,DepositCash ,DepositCheck ,DepositBill ,DepositOffset ,DepositAdjustment ,DepositReturns ,DepositDiscount ,DepositCancel ,
	PaymentTransfer ,PaymentCash ,Paymentcheck ,PaymentBill ,PaymentOffset,PaymentAdjustment,Change,DepositDateTime)
	SELECT 
		Sum(CASE 
			WHEN ddh.DepositKBN = 1 and ddh.DataKBN = 3 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku	
			else 0	
			END) AS CashSale,

		Sum(CASE 
			WHEN ddh.DepositKBN = 7 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
			else 0
			END) AS Gift,

		Sum(CASE 
			WHEN ddh.DepositKBN = 2  and ddh.DataKBN = 3 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
			else 0
			END) AS CashDeposit,	
		
		Sum(CASE 
			WHEN ddh.DepositKBN = 3 and ddh.DataKBN = 3 and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
			else 0
			END) AS CashPayment,
		
		Sum(CASE 
			WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and mdkbn.SystemKBN = 5 THEN ddh.DepositGaku
			else 0
			END) AS DepositTransfer,
	
		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN  = 0 or ddh.CancelKBN  = 3) and mdkbn.SystemKBN = 1 THEN ddh.DepositGaku
			else 0
		    END) AS DepositCash,

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN  = 0 or ddh.CancelKBN  = 3)  and mdkbn.SystemKBN = 6 THEN ddh.DepositGaku
			else 0
		    END) AS DepositCheck,		

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN  = 0 or ddh.CancelKBN  = 3)  and mdkbn.SystemKBN = 8 THEN ddh.DepositGaku
			else 0
		    END) AS DepositBill,		

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN  = 0 or ddh.CancelKBN  = 3)  and mdkbn.SystemKBN = 7 THEN ddh.DepositGaku
			else 0
		    END) AS DepositOffset,

		Sum(CASE 
		    WHEN  (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN  = 0 or ddh.CancelKBN  = 3)  and mdkbn.SystemKBN = 9 THEN ddh.DepositGaku
			else 0
		    END) AS DepositAdjustment,

		Sum(CASE 
			WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN = 0 or ddh.CancelKBN = 3) THEN ddh.DepositGaku
			else 0
			END) AS DepositReturns,

		Sum(CASE 
			WHEN ddh.DepositKBN = 2 and (ddh.CancelKBN = 0 or ddh.CancelKBN = 3) and mdkbn .SystemKBN = 10 THEN ddh.DepositGaku
			else 0
			END) AS DepositDiscount,
 
		Sum(CASE 
			WHEN (ddh.DepositKBN = 2 or ddh.DepositKBN = 4) and (ddh.CancelKBN = 0 or ddh.CancelKBN = 3) THEN ddh.DepositGaku
			else 0
			END) AS DepositCancel,

		Sum(CASE 
			WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 5 THEN ddh.DepositGaku
			else 0
			END) AS PaymentTransfer,	
		
		Sum(CASE 
			WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5 )and mdkbn .SystemKBN = 1 THEN ddh.DepositGaku
			else 0
			END) AS PaymentCash,
		
		Sum(CASE
			WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 6 THEN ddh.DepositGaku
			else 0
			END) AS Paymentcheck,

		Sum(CASE 
		    WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 8 THEN ddh.DepositGaku
			else 0
		    END) AS PaymentBill,

		Sum(CASE 
			WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 7 THEN ddh.DepositGaku
			else 0
			END) AS PaymentOffset,	
		
		Sum(CASE 
			WHEN (ddh.DepositKBN = 3 or ddh.DepositKBN = 5) and mdkbn .SystemKBN = 9 THEN ddh.DepositGaku
			else 0
			END) AS PaymentAdjustment,

		(select DepositGaku from D_DepositHistory
		where DepositNO = (select max(ddh1.DepositNO) from D_DepositHistory as ddh1
								where ddh1.DepositKBN=6
								And CAST(ddh1.AccountingDate AS DATE)=@date
								And ddh1.StoreCD = @StoreCD
								Group by CAST(ddh1.AccountingDate AS DATE)))as Change,

		 CAST (ddh.AccountingDate as Date) as DepositDateTime

	FROM D_DepositHistory ddh
	Left Outer Join M_DenominationKBN mdkbn on mdkbn.DenominationCD = ddh.DenominationCD
	Left Outer Join F_Store(cast(@Date as varchar(10))) fs on fs.StoreCD = ddh.StoreCD  and fs.DeleteFlg = 0
	--Left Outer Join M_Store ms on ms.StoreCD = ddh.StoreCD and ms.ChangeDate <= ddh.DepositDateTime 
	Where CAST(ddh.AccountingDate as Date) = @Date 
	and ddh.StoreCD = @StoreCD 
	and fs.DeleteFlg = 0
	and fs.StoreKBN = 1
	Group By CAST (ddh.AccountingDate as Date)

	Select
	IsNull(Cast( tmpddh.Change as int),0) as Change,	
	IsNull(Cast(tmpddh.CashSale as int),0) as CashSale,
	IsNull(Cast(tmpddh.Gift as int),0) as Gift,
	IsNull(Cast(tmpddh.CashDeposit as int),0) as CashDeposit,
	IsNull(Cast(tmpddh.CashPayment as int),0) as CashPayment,

---CashStorage
	IsNull(Cast(tmpddh.DepositTransfer as int),0) as DepositTransfer,
	IsNull(Cast(tmpddh.DepositCash as int),0)as DepositCash,
	IsNull(Cast(tmpddh.DepositCheck as int),0)as DepositCheck,
	IsNull(Cast(tmpddh.DepositBill as int),0)as DepositBill ,
	IsNull(Cast(tmpddh.DepositOffset as int),0)as DepositOffset,
	IsNull(Cast(tmpddh.DepositAdjustment as int),0)as DepositAdjustment,
	
    IsNull(Cast( tmpddh.DepositTransfer + tmpddh.DepositCash as int),0) as total,
	IsNull(Cast(tmpddh.DepositReturns as int),0)as DepositReturns ,
	IsNull(Cast(tmpddh.DepositDiscount as int),0)as DepositDiscount,
	IsNull(Cast(tmpddh.DepositCancel as int),0)as DepositCancel,
	IsNull(Cast(tmpddh.PaymentTransfer as int),0)as PaymentTransfer,
	IsNull(Cast(tmpddh.PaymentCash as int),0)as PaymentCash,
	IsNull(Cast(tmpddh.Paymentcheck as int),0)as PaymentCheck,
	IsNull(Cast(tmpddh.PaymentBill as int),0)as PaymentBill,
	IsNull(Cast(tmpddh.PaymentOffset as int),0)as PaymentOffset,
	IsNull(Cast(tmpddh.PaymentAdjustment as int),0) as PaymentAdjustment ,
    IsNull(Cast(tmpddh.PaymentTransfer + tmpddh.PaymentCash + tmpddh.Paymentcheck + tmpddh.PaymentBill + tmpddh.PaymentOffset + tmpddh.PaymentAdjustment as int),0) as TotalPayment

	
	From  #tmp_Deposit‚gistory tmpddh 
	Where tmpddh.DepositDateTime = @Date
	
	drop table #tmp_Deposit‚gistory
END
GO


