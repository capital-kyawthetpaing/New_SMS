 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Select3]
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
CREATE PROCEDURE [dbo].[D_Pay_Select3]
	-- Add the parameters for the stored procedure here
	@LargePayNo as varchar(11),
	@PayNo as varchar(11)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		select dpp.PayPlanNO,
		dpp.PayeeCD,
		Replace(CONVERT(char(10), dpp.PayPlanDate,126),'-','/') as 'PayPlanDate',
		dpp.Number 'Number',
		Replace(CONVERT(char(10), dpp.RecordedDate,126),'-','/') as 'RecordedDate',
		--dpp.PayPlanGaku 'PayPlanGaku',
		CAST(dpp.PayPlanGaku AS int) as PayPlanGaku,
		--dpp.PayConfirmGaku 'PayConfirmGaku',
		CAST(dpp.PayConfirmGaku AS int) as PayConfirmGaku,
		--dpd.PayGaku  'UnpaidAmount1',
		CAST(dpd.PayGaku AS int) as UnpaidAmount1,
		--Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) as UnpaidAmount2
		CAST(Isnull(dpp.PayPlanGaku,0)- Isnull(dpp.PayConfirmGaku,0) AS int) as UnpaidAmount2
		from D_PayDetails as dpd
		left outer join D_PayPlan as dpp on dpp.PayPlanNO=dpd.PayPlanNO
		left outer join D_Pay as dp on dp.PayNO=dpd.PayNO
		where dp.DeleteDateTime is null
		and dpp.DeleteDateTime is null
		and dp.FBCreateDate is null
		and (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
		and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
		order by dpp.RecordedDate,dpp.Number asc
	
END

