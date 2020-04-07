 BEGIN TRY 
 Drop Procedure dbo.[M_Carrier_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Carrier_Insert_Update] 
	-- Add the parameters for the stored procedure here
	@ShippingCD as varchar(3),
	@ChangeDate as date,
	@ShippingName as varchar(40),
	@Identify  as tinyint,
	@YahooCD as varchar(20),
	@RakutenCD as varchar(20),
	@AmazonCD  as varchar(20),
	@WowmaCD as varchar(20),
	@PonpareCD as varchar(20),
	@OtherCD as varchar(20),
	@Remarks as varchar(500),
	@DeleteFlg as tinyint,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100),
	@Mode as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @currentDate as datetime = getdate();
    -- Insert statements for procedure here
	
			if @Mode = 1--insert mode
				begin										
					insert into M_Carrier
					(
						CarrierCD ,
						ChangeDate,
						CarrierName,
						CarrierFlg ,
						YahooCarrierCD ,
						RakutenCarrierCD,
						AmazonCarrierCD ,
						WowmaCarrierCD ,
						PonpareCarrierCD ,
						OtherCD ,
						Remarks ,
						DeleteFlg ,
						UsedFlg ,
						InsertOperator ,
						InsertDateTime,
						UpdateOperator ,
						UpdateDateTime
					)
					values
					(		
						@ShippingCD,
						@ChangeDate,
						@ShippingName,
						@Identify ,
						@YahooCD ,
						@RakutenCD,
						@AmazonCD,
						@WowmaCD,
						@PonpareCD,
						@OtherCD,
						@Remarks,
						@DeleteFlg,
						0,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)
				end
			else if @Mode = 2--update mode
				begin
					update M_Carrier
					set
						CarrierCD =	@ShippingCD,
						ChangeDate = @ChangeDate,
						CarrierName= @ShippingName,
						CarrierFlg = @Identify ,
						YahooCarrierCD = @YahooCD,
						RakutenCarrierCD= @RakutenCD,
						AmazonCarrierCD= @AmazonCD,
						WowmaCarrierCD = @WowmaCD,
						PonpareCarrierCD = @PonpareCD,
						OtherCD =@OtherCD,
						Remarks =@Remarks,
						DeleteFlg =	@DeleteFlg,
						UpdateOperator = @Operator,
						UpdateDateTime = @currentDate
										   
					where CarrierCD = @ShippingCD 	  
						and ChangeDate = @ChangeDate 	
				end

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

