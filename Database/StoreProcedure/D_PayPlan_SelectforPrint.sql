 BEGIN TRY 
 Drop Procedure dbo.[D_PayPlan_SelectforPrint]
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
CREATE PROCEDURE [dbo].[D_PayPlan_SelectforPrint]
	-- Add the parameters for the stored procedure here
	
	@PaymentDueDateFrom as date,
	@PaymentDueDateTo as date,
	@PaymentCD as varchar(13),
	@ClosedStatusSumi as tinyint,
	@PaymentStatusUnpaid as tinyint,
	@Purchase as tinyint,
	@Expense as tinyint,
	@StoreCD as varchar(6),--dpp
	@Type as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

Declare @Date as date=getDate();

IF @Type=1--PrintType
begin
 Select DPP.PayPlanNO AS PayPlanNO
			,fv.VendorCD+'  '+fv.VendorName AS Supplier
			,DPP.PayPlanGaku AS PayPlanAmount
			,Convert(varchar,DPP.PayPlanDate,111) AS PayPlanDate
			,DPP.PayConfirmGaku AS PaidAmount
			,DPP.PayPlanGaku-PayConfirmGaku AS UnpaidAmount

--,SUM(DPP.PayPlanGaku) AS TotalPaymentAmtFrom
--,SUM(DPP.PayPlanGaku)-SUM(DPP.PayConfirmGaku) AS TotalUnpaidAmt

 From D_PayPlan AS DPP

left outer join  F_Vendor(cast(@Date as varchar(10))) as fv on DPP.PayeeCD = fv.VendorCD

WHERE DPP.DeleteDateTime is NULL
AND  fv.ChangeDate <= dpp.RecordedDate 
AND fv.PayeeFlg=1
AND ISNULL(FV.DeleteFlg,0)=0
AND (@PaymentDueDateFrom is null or (DPP.PayPlanDate>=@PaymentDueDateFrom))
AND (@PaymentDueDateTo is null or  (DPP.PayPlanDate<=@PaymentDueDateTo))
AND (DPP.PayeeCD=@PaymentCD)
AND (@ClosedStatusSumi=0 OR (@ClosedStatusSumi=1 AND DPP.PayCloseNO IS NOT NULL))
AND (@PaymentStatusUnpaid=0 OR (@PaymentStatusUnpaid=1 AND DPP.PayConfirmFinishedKBN=0))	
AND ((@Purchase=1 AND @Expense=1 AND DPP.PayPlanKBN IN (1,2)) OR (@Purchase=1  AND DPP.PayPlanKBN=1) OR (@Expense=1 AND DPP.PayPlanKBN=2))

--Group By DPP.PayPlanNO,DPP.Number,DPP.PayPlanGaku, DPP.PayPlanDate,DPP.PayConfirmGaku,fv.VendorCD,fv.VendorName,DPP.StoreCD,DPP.RecordedDate,DPP.PayeeCD

Order by DPP.PayeeCD,DPP.PayPlanDate ASC
end

ELSE IF @Type=2--ExportCSV
begin
	Select @StoreCD as N'店舗'
	,(select fs.StoreName from F_Store(@Date) fs where fs.StoreCD=@StoreCD) as N'店舗名'
	,(Convert(varchar,@PaymentDueDateFrom,111)) as N'支払予定日FROM'
	,(Convert(varchar,@PaymentDueDateTo,111)) as [TO]
	, fv.VendorCD as N'仕入先CD'
	,fv.VendorName AS N'仕入先名'
	 ,DPP.PayPlanGaku AS N'支払予定額'
	,Convert(varchar,DPP.PayPlanDate,111) AS N'支払予定日'
	,DPP.PayConfirmGaku AS N'支払済額'
	,(DPP.PayPlanGaku-PayConfirmGaku) AS N'未支払額'
	 From D_PayPlan AS DPP
	left outer join  F_Vendor(cast(@Date as varchar(10))) as fv on DPP.PayeeCD = fv.VendorCD
	WHERE DPP.DeleteDateTime is NULL
	AND  fv.ChangeDate <= dpp.RecordedDate 
	AND fv.PayeeFlg=1
	AND ISNULL(FV.DeleteFlg,0)=0
	AND (@PaymentDueDateFrom is null or (DPP.PayPlanDate>=@PaymentDueDateFrom))
	AND (@PaymentDueDateTo is null or  (DPP.PayPlanDate<=@PaymentDueDateTo))
	AND (DPP.PayeeCD=@PaymentCD)
	AND (@ClosedStatusSumi=0 OR (@ClosedStatusSumi=1 AND DPP.PayCloseNO IS NOT NULL))
	AND (@PaymentStatusUnpaid=0 OR (@PaymentStatusUnpaid=1 AND DPP.PayConfirmFinishedKBN=0))	
	AND ((@Purchase=1 AND @Expense=1 AND DPP.PayPlanKBN IN (1,2)) OR (@Purchase=1  AND DPP.PayPlanKBN=1) OR (@Expense=1 AND DPP.PayPlanKBN=2))
	Order by DPP.PayeeCD,DPP.PayPlanDate ASC
end
END

