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
CREATE PROCEDURE [dbo].[Select_PaymentClose_ValueCheck]
	-- Add the parameters for the stored procedure here
	@PaymentCloseCD as varchar(13),
	@PaymentCloseDate as date,
	@Type varchar(1)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @Type=1
		begin	
			SELECT top 1 * 
			FROM D_PayPlan
			WHERE PayeeCD=@PaymentCloseCD
			and RecordedDate <= @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is null
		end

	else if @Type=2
		begin
			SELECT top 1 * 
			FROM D_PayCloseHistory
			WHERE  PayeeCD = @PaymentCloseCD
			and PayCloseDate = @PaymentCloseDate
			and DeleteOperator is null
			and DeleteDateTime is null
		end

  else if @Type=3
		begin	
			SELECT top 1 * 
			FROM D_PayPlan
			WHERE PayeeCD=@PaymentCloseCD
			and RecordedDate = @PaymentCloseDate
			and PayConfirmGaku=0
			and PayConfirmFinishedKBN=0
			and DeleteOperator is null
			and DeleteDateTime is null
			and PayCloseNO is not null
		end

   else 
		begin
			SELECT top 1 *
			FROM D_PayPlan
			where PayeeCD=@PaymentCloseCD
			and	 PayCloseDate=@PaymentCloseDate
			and  PayConfirmGaku > 0
		end

END

