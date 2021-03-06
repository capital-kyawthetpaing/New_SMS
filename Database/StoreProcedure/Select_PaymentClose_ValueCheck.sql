 BEGIN TRY 
 Drop Procedure dbo.[Select_PaymentClose_ValueCheck]
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
Create PROCEDURE [dbo].[Select_PaymentClose_ValueCheck]
	-- Add the parameters for the stored procedure here
	@PaymentCloseCD as varchar(13),
	@PaymentCloseDate as date,
	@Type varchar(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-----for shime支払締


	if @Type=1
		begin	
			SELECT top 1 * 
			FROM D_PayPlan
			WHERE (@PaymentCloseCD is NUll or (PayeeCD=@PaymentCloseCD))
			and RecordedDate <= @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is null
		end

	else if @Type=2
		begin
			SELECT top 1 * 
			FROM D_PayCloseHistory
			WHERE (@PaymentCloseCD is Null or  (PayeeCD = @PaymentCloseCD))
			and PayCloseDate = @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null
			and (select count(distinct PayeeCD) from
			D_PayPlan
			where 
				PayCloseNo is null
			and DeleteOperator is null 
			and DeleteDateTime is null
			and (@PaymentCloseCD is null or (PayeeCD = @PaymentCloseCD))
			and RecordedDate <= @PaymentCloseDate) = 0
		end

		---shime cancel支払締キャンセル

  else if @Type=3
		begin	
			SELECT top 1 * 
			FROM D_PayPlan
			WHERE (@PaymentCloseCD is NULL or (PayeeCD=@PaymentCloseCD))
			and PayCloseDate = @PaymentCloseDate
			and PayConfirmGaku=0
			and PayConfirmFinishedKBN=0
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is not null
		end

   else if @Type=4
		begin
			SELECT top 1 *
			FROM D_PayPlan
			where (@PaymentCloseCD is Null or (PayeeCD=@PaymentCloseCD))
			and	 PayCloseDate=@PaymentCloseDate
			and  PayConfirmGaku > 0
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
								and (@PaymentCloseCD is Null or (PayeeCD=@PaymentCloseCD))
								and RecordedDate =@PaymentCloseDate
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
								and (@PaymentCloseCD is Null or (PayeeCD=@PaymentCloseCD))
								and PayCloseDate =@PaymentCloseDate
								and DeleteOperator is null
								and DeleteDatetime is null
								)
		end

	
END


