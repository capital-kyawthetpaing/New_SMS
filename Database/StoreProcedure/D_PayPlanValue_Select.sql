

/****** Object:  StoredProcedure [dbo].[D_PayPlanValue_Select]    Script Date: 2020/11/17 10:13:32 ******/
DROP PROCEDURE [dbo].[D_PayPlanValue_Select]
GO

/****** Object:  StoredProcedure [dbo].[D_PayPlanValue_Select]    Script Date: 2020/11/17 10:13:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================

-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_PayPlanValue_Select]
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
			
	if @Type=1                                --for D_Exclusive check	(for shime)
		begin
			SELECT Number  
			FROM D_PayPlan
			WHERE PayCloseNO is null
			and PayeeCD=@PayeeCD
			and RecordedDate <= @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null

	end


	else if @Type=2                             --for D_Exclusive check	(for shime)
		begin
			select PayCloseNo
			from D_PayPlan
			where PayCloseNO is not null
			and DeleteOperator is null 
			and DeleteDateTime is null
			and PayeeCD=@PayeeCD
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

			--for D_Exclusive check	(for shime)

	else if @Type=5
		begin
				select top 1 Program,Operator,PC
				from D_Exclusive
				where DataKBN IN(7,8,9,12)
				And Number = (
								Select top 1 Number
								from  D_PayPLan
								where PayCloseNo is null
								and PayeeCD=@PayeeCD
								and RecordedDate <= @PaymentCloseDate
								and DeleteOperator is null
								and DeleteDatetime is null
							)

		end
			 ---for D_Exclusive check (for shimecancel)

	 else if @Type=6
		begin
				select top 1 Program,Operator,PC
				from D_Exclusive
				where DataKBN =9
				And Number = (
								Select top 1 PayCloseNo
								from  D_PayPLan
								where PayCloseNo is not null
								and PayeeCD=@PayeeCD
								and PayCloseDate =@PaymentCloseDate
								and DeleteOperator is null
								and DeleteDatetime is null
								)
		end

END
GO


