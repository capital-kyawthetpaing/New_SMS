 BEGIN TRY 
 Drop Procedure dbo.[Amazon_InsertOrderItemDetails]
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
CREATE PROCEDURE [dbo].[Amazon_InsertOrderItemDetails]
	-- Add the parameters for the stored procedure here
	          
	@StoreCD as varchar(10),   
		@API_Key as varchar(10),
		@xml as xml    

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			
					
						
			declare	@Date as DateTime= getdate() 
			
				 declare @val as int 
			     set @val = (select Max(IsNull(InportSEQ,0)) from  D_APIRireki);
	             if (@val is null)
	             Begin
	             set @val=1;
	             End

				 declare @DocHandle int
 	             exec sp_xml_preparedocument @DocHandle output, @Xml
	            select *  into #temp
	              FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			     with
			     (
				 StoreCD  varchar(4),
				 APIKey tinyint,
				 InportSEQRows int,
				 AmazonOrderId varchar(20),
				 [OrderRows]  int,
				 [ASIN] varchar(30),
				 OrderItemId varchar(50),
				 SellerSKU varchar(50),
				 CustomizedURL varchar(200),
				 Title varchar(100),
				 QuantityOrdered int,
				 QuantityShipped int,
				 PointsNumber int,----------------------------
				 PointsMonetaryCurrencyCode money,
				 PointsMonetaryValue money,
				 NumberOfItems int,
				 ItemPriceCurrencyCode varchar(3),
				 ItemPriceAmount money,
				 ShippingPriceCurrencyCode varchar(3),
				 ShippingPriceAmount money,
				 GiftWrapPriceCurrencyCode varchar(3),
				 GiftWrapPriceAmount money,
				 TaxModel varchar(30),
				 TaxResponsibleParty varchar(30),
				 ItemTaxCurrencyCode varchar(3),
				 ItemTaxCurrencyAmount money,
				 ShippingTaxCurrencyCode varchar(3),
				 ShippingTaxAmount money,
				 GiftWrapTaxCurrencyCode varchar(3),
				 GiftWrapTaxAmount money,
				 ShippingDiscountCurrencyCode varchar(3),
				 ShippingDiscountAmount money,
				 PromotionDiscountCurrencyCode varchar(3),
				 PromotionDiscountAmount money,
				 PromotionIds varchar(50),
				 CODFeeCurrencyCode varchar(3),
				 CODFeeAmount money,
				 CODFeeDiscountCurrencyCode varchar(3),
				 CODFeeDiscountAmount money,
				 IsGift tinyint,
				 GiftMessageText varchar(500),
				 GiftWrapLevel varchar(50),
				 ConditionNote varchar(30),
				 ConditionId varchar(30),
				 ConditionSubtypeId varchar(30),
				 ScheduledDeliveryStartDate datetime,
				 ScheduledDeliveryEndDate datetime,
				 PriceDesignation varchar(30)

			     )
				 exec sp_xml_removedocument @DocHandle;




				 insert into D_AmazonJuchuuDetails
				 select @val, *,Null,@Date,null, @Date from #temp



				--select * from D_AmazonJuchuuDetails

				 drop  table #temp




END

