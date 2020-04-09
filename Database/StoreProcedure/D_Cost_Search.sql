 BEGIN TRY 
 Drop Procedure dbo.[D_Cost_Search]
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
CREATE PROCEDURE [dbo].[D_Cost_Search]
	-- Add the parameters for the stored procedure here
	@RecordDateFrom as varchar(10),
	@RecordDateTo as varchar(10),
	@ExpanseEntryDateFrom as varchar(10),
	@ExpanseEntryDateTo as varchar(10),
	@PaymentDueDateFrom as varchar(10),
	@PaymentDueDateTo as varchar(10),
	@StaffCD as varchar(10),
	@PaymentDestinationCD as varchar(13),
	@paid as tinyint,
	@unpaid as tinyint,
	@RegFlg as tinyint,
	@PaymentDateFrom as varchar(10),
	@PaymentDateTo as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--ZCO
    -- Insert statements for procedure here
	--declare @query as varchar(1000)
	--select dcost.CostNo,dcost.RecordedDate,InputDateTime,CONVERT(varchar(10), RegularlyFLG) AS RegularlyFLG,
	--(select VendorName from F_Vendor(dcost.RecordedDate) where VendorCD = dcost.PayeeCD) as VendorName,
	--(select VendorCD from F_Vendor(dcost.RecordedDate) where VendorCD = dcost.PayeeCD) as VendorCD,
	--(select StaffName from F_Staff(dcost.RecordedDate) where StaffCD = dcost.StaffCD) as StaffName,dcost.StaffCD, dcost.PayPlanDate ,dpplan.PayPlanDate AS PayDate,dcostd.CostGaku
	--from D_Cost as dcost
	--Left Outer Join D_PayPlan as dpplan
	--ON dpplan.PayPlanKBN =2
	--AND dpplan.Number=dcost.CostNO
	--Inner JOIN D_CostDetails AS dcostd
	--ON dcostd.CostNO=dcost.CostNO
	--WHERE dcost.DeleteDateTime IS NULL
	--AND (@RecordDateFrom is null or (dcost.RecordedDate>=CONVERT(DATE, @RecordDateFrom)))
	--AND (@RecordDateTo is null or  (dcost.RecordedDate<=CONVERT(DATE, @RecordDateTo)))
	--AND (@ExpanseEntryDateFrom is null or (dcost.InputDateTime>=CONVERT(DATETIME, @ExpanseEntryDateFrom)))
	--AND (@ExpanseEntryDateTo is null or  (dcost.InputDateTime<=CONVERT(DATETIME, @ExpanseEntryDateTo)))
	--AND (@PaymentDueDateFrom is null or  (dcost.PayPlanDate>=CONVERT(DATE, @PaymentDueDateFrom)))
	--AND (@PaymentDueDateTo is null or  (dcost.PayPlanDate<=CONVERT(DATE, @PaymentDueDateTo)))
	--AND (@StaffCD is null or  (dcost.StaffCD=@StaffCD))
	--AND (@PaymentDestinationCD is null or (dcost.PayeeCD=@PaymentDestinationCD))
	--AND (@paid is null or (dpplan.PayConfirmFinishedKBN=@paid))
	--AND (@unpaid is null or (dpplan.PayConfirmFinishedKBN=@unpaid))
	--AND (@PaymentDateFrom is null or  (dpplan.PayPlanDate>=CONVERT(DATE,@PaymentDateFrom)))
	--AND (@PaymentDateTo is null or  (dpplan.PayPlanDate<=CONVERT(DATE, @PaymentDateTo)))
	--Order by
	--dcost.CostNO ASC

SELECT dc.CostNO,CONVERT(VARCHAR(10),dc.RecordedDate,111) as RecordedDate,
CONVERT(varchar(10),dc.InputDateTime,111) as ExpenseEntryDate,cast(dc.RegularlyFLG as varchar)as RegularlyFLG,
dc.PayeeCD as VendorCD,
(select VendorName from F_Vendor(dc.RecordedDate) where VendorCD = dc.PayeeCD) as VendorName,
dc.StaffCD,
(select StaffName from F_Staff(dc.RecordedDate) where StaffCD = dc.StaffCD) as StaffName,
CONVERT(VARCHAR(10),dc.PayPlanDate,111) as PayPlanDate,
CONVERT(VARCHAR(10),dpp.PayPlanDate ,111)as PayDate,
ISNULL(FORMAT(CONVERT(int,FLOOR(dcd.CostGaku)),'#,#'),0) as CostGaku
--cast(Floor(dcd.CostGaku) as int) as CostGaku
 from D_Cost dc
 left outer join D_PayPlan dpp
 on dpp.PayPlanKBN=2 
 and dpp.Number=dc.CostNO
inner join D_CostDetails dcd
 on dcd.CostNO=dc.CostNO

 where dc.DeleteDateTime is null
 AND  dc.RegularlyFlg =@RegFlg
 AND   (@RecordDateFrom is null or (dc.RecordedDate>=@RecordDateFrom))
 AND    (@RecordDateTo is null or (dc.RecordedDate<=@RecordDateTo))
 AND (@ExpanseEntryDateFrom is null or (convert(varchar(10),dc.InputDateTime,111)>=@ExpanseEntryDateTo))
AND (@ExpanseEntryDateTo is null or  (convert(varchar(10),dc.InputDateTime,111)<=@ExpanseEntryDateTo))
AND (@PaymentDueDateFrom is null or  (dc.PayPlanDate>=@PaymentDueDateFrom))
AND (@PaymentDueDateTo is null or  (dc.PayPlanDate<=@PaymentDueDateTo))
AND (@StaffCD is null or  (dc.StaffCD=@StaffCD))
AND (@PaymentDestinationCD is null or (dc.PayeeCD=@PaymentDestinationCD))
	and  ((dpp.PayConfirmFinishedKBN=case when  @paid =1 then  1  end)
		or (dpp.PayConfirmFinishedKBN=case when  @unpaid =0 then  0 end))
AND (@PaymentDateFrom is null or  (dpp.PayPlanDate>=@PaymentDateFrom))
AND (@PaymentDateTo is null or  (dpp.PayPlanDate<=@PaymentDateTo))

	Order by
	dc.CostNO ASC
END

