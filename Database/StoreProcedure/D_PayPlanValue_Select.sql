BEGIN TRY 
 Drop Procedure [dbo].[D_PayPlanValue_Select]
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
Create PROCEDURE [dbo].[D_PayPlanValue_Select]
	-- Add the parameters for the stored procedure here
	@PayeeCD as varchar(13),
	@PaymentCloseDate as date,
	@Type as varchar(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			
	if @Type=1
		begin
			SELECT Number  
			FROM D_PayPlan
			WHERE PayCloseNO is null
			and (@PayeeCD is NUll or (PayeeCD=@PayeeCD))
			and RecordedDate <= @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null

	end


	else if @Type=2
		begin
			select PayCloseNo
			from D_PayPlan
			where PayCloseNO is not null
			and DeleteOperator is null 
			and DeleteDateTime is null
			and (@PayeeCD is Null or (PayeeCD=@PayeeCD))
			and PayCloseDate =@PaymentCloseDate
		end

		else if @Type=3
		begin
			SELECT  Distinct(PayeeCD) 
			FROM D_PayPlan
			where RecordedDate <= @PaymentCloseDate
			and (@PayeeCD is Null or (PayeeCD=@PayeeCD))
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is null
		end


	else if @Type=4
		begin
			SELECT   Distinct(PayeeCD) 
			FROM D_PayPlan
			WHERE (@PayeeCD is NULL or (PayeeCD=@PayeeCD))
			and PayCloseDate = @PaymentCloseDate
			and PayConfirmGaku=0
			and PayConfirmFinishedKBN=0
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is not null
			
		end

		

END
