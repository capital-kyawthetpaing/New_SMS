 BEGIN TRY 
 Drop Procedure dbo.[Insert_Update_ShiharaiShori]
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
CREATE PROCEDURE [dbo].[Insert_Update_ShiharaiShori]
	-- Add the parameters for the stored procedure here

	@PaymentCloseCD as varchar(13),
	@PaymentCloseDate as date,
	@InsertType as tinyint,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100),
	@StoreCD varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
 

	 --Return ==>伝票種別「1７」 
	-- declare @Data as int=1,@datetime as datetime =getdate();,
	-- --execute @Data=dbo.Function_GetName_ReturnValue 2,17,@PaymentCloseDate,@PaymentCloseCD
	-- --select @Data
	--@DateTime datetime=getdate(),@shipDate varchar(10)=CONVERT(VARCHAR(10), @ShippingPlanDateTo, 111)

	 ----Step1
	 declare @PayCloseNo as Varchar(11),@datetime as datetime =getdate();


	 EXEC Fnc_GetNumber
            17,-------------in伝票種別 
            @PaymentCloseDate,----in基準日
            @Operator,
            @Operator,
            @PayCloseNo OUTPUT
            ;

		 IF ISNULL(@PayCloseNo,'') = ''
            BEGIN
                Return  '1' ;
            END
	
	
	 if @InsertType=1
		
		begin

				UPDATE D_PayPlan 
					SET	
						PayPlanKBN=PayPlanKBN,
						Number=Number,
						StoreCD=StoreCD,
						PayeeCD=PayeeCD,
						RecordedDate=RecordedDate,
						PayPlanDate=PayPlanDate,
						PayPlanGaku=PayPlanGaku,
						PayConfirmGaku=PayConfirmGaku,
						PayConfirmFinishedKBN=PayConfirmFinishedKBN,
						Program=Program,
						InsertOperator=InsertOperator,
						InsertDateTime=InsertDateTime,
						UpdateOperator=@Operator,
						UpdateDateTime=@datetime,
						PayCloseDate = @PaymentCloseDate,
						PayCloseNO=@PayCloseNo,
						DeleteOperator=Null,
						DeleteDateTime=Null

				WHERE	PayCloseNO is   null
						AND DeleteOperator is null
						AND DeleteDateTime is null
						AND PayeeCD=@PaymentCloseCD
						AND RecordedDate=@PaymentCloseDate
						
--Step2

				INSERT
					INTO	D_PayCloseHistory 
							(PayCloseNO,
							PayCloseDate,
							PayeeKBN,
							PayeeCD,
							ProcessingKBN,
							PayCloseProcessingDateTime,
							StaffCD,
							InsertOperator,
							InsertDateTime,
							UpdateOperator,
							UpdateDateTime,
							DeleteOperator,
							DeleteDateTime)
					VALUES				
							(@PayCloseNo,
							@PaymentCloseDate,
							1,
							@PaymentCloseCD,
							1,
							@datetime,
							@Operator,
							@Operator,
							@datetime,
							NULL,
							NULL,
							NULL,
							NULL)
	
	 end

	 else if @InsertType=2
			
			
			begin
	 --Step1
	

				UPDATE D_PayPlan 
							SET	
								PayPlanKBN=PayPlanKBN,
								Number=Number,
								StoreCD=StoreCD,
								PayeeCD=PayeeCD,
								RecordedDate=RecordedDate,
								PayPlanDate=PayPlanDate,
								PayPlanGaku=PayPlanGaku,
								PayConfirmGaku=PayConfirmGaku,
								PayConfirmFinishedKBN=PayConfirmFinishedKBN,
								Program=Program,
								InsertOperator=InsertOperator,
								InsertDateTime=InsertDateTime,
								UpdateOperator=UpdateOperator,
								UpdateDateTime=UpdateDateTime,
						        PayCloseDate = Null,
								PayCloseNO=Null,
								DeleteOperator=Null,
								DeleteDateTime=Null

						WHERE	PayCloseNO is not  null
								AND DeleteOperator is null
								AND DeleteDateTime is null
								AND PayCloseDate=@PaymentCloseDate
								AND PayeeCD=@PaymentCloseCD
								AND  PayConfirmGaku=0
								AND PayConfirmFinishedKBN=0
	--Step2
							

				UPDATE	D_PayCloseHistory
							SET	
								ProcessingKBN=ProcessingKBN,
								PayCloseProcessingDateTime=PayCloseProcessingDateTime,
								StaffCD=StaffCD,
								UpdateOperator=@Operator,
								UpdateDatetime=@datetime,
								DeleteOperator=@Operator,
								DeleteDateTime=@datetime
								
							FROM
								D_PayCloseHistory  dpch
							INNER	JOIN  D_PayPlan dp 
							ON	dp.PayCloseNO=dpch.PayCloseNO

							WHERE	
								dp.PayCloseNO IS NOT NULL
								AND dp.DeleteOperator IS NULL
								AND  dp.DeleteDateTime IS NULL
								AND dp.PayCloseDate =@PaymentCloseDate
								AND dp.PayeeCD=@PaymentCloseCD
								AND dp.PayConfirmGaku=0
								AND dp.PayConfirmFinishedKBN=0
			end
 
 		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@PayCloseNo

END

