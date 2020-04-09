 BEGIN TRY 
 Drop Procedure dbo.[D_WowmaJuchuu_Insert]
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
CREATE PROCEDURE [dbo].[D_WowmaJuchuu_Insert]
	-- Add the parameters for the stored procedure here
	@JuChuuXml  xml,
	@Insertdatetime datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
--begin

CREATE TABLE [dbo].[#tempOrderInfo](
	[APIKey][tinyint],
	[InportSEQRows][int],
	[orderDate][date],
	[orderId][varchar](50) collate Japanese_CI_AS,
	[sellMethodSegment][tinyint],
	[releaseDate][date],
	[siteAndDevice][varchar](1000) collate Japanese_CI_AS,
	[mailAddress][varchar](128) collate Japanese_CI_AS,
	[rawMailAddress][varchar](128) collate Japanese_CI_AS,
	[ordererName][varchar](110) collate Japanese_CI_AS,
	[ordererKana][varchar](110) collate Japanese_CI_AS,
	[ordererZipCode][varchar](8) collate Japanese_CI_AS,
	[ordererAddress][varchar](370) collate Japanese_CI_AS,
	[ordererPhoneNumber1][varchar](30) collate Japanese_CI_AS,
	[ordererPhoneNumber2][varchar](30) collate Japanese_CI_AS,
	[nickname][varchar](40) collate Japanese_CI_AS,
	[senderName][varchar](110) collate Japanese_CI_AS,
	[senderKana][varchar](110) collate Japanese_CI_AS,
	[senderZipCode][varchar](8) collate Japanese_CI_AS,
	[senderAddress][varchar](370) collate Japanese_CI_AS,
	[senderPhoneNumber1][varchar](30) collate Japanese_CI_AS,
	[senderPhoneNumber2][varchar](30) collate Japanese_CI_AS,
	[senderShopCD][varchar](4) collate Japanese_CI_AS,
	[orderOption][varchar](2000) collate Japanese_CI_AS,
	[settlementName][varchar](50) collate Japanese_CI_AS,
	[secureSegment][tinyint],
	[userComment][varchar](4000) collate Japanese_CI_AS,
	[memo][varchar](4000) collate Japanese_CI_AS,
	[orderStatus][varchar](32) collate Japanese_CI_AS,
	[contactStatus][varchar](1) collate Japanese_CI_AS,
	[contactDate][datetime],
	[authorizationStatus][varchar](1) collate Japanese_CI_AS,
	[authorizationDate][datetime],
	[paymentStatus][varchar](1) collate Japanese_CI_AS,
	[paymentDate][datetime],
	[shipStatus][varchar](1) collate Japanese_CI_AS,
	[shipDate][datetime],
	[printStatus][varchar](1) collate Japanese_CI_AS,
	[printDate][datetime],
	[cancelStatus][varchar](1) collate Japanese_CI_AS,
	[cancelReason][varchar](2) collate Japanese_CI_AS,
	[cancelComment][varchar](100) collate Japanese_CI_AS,
	[cancelDate][datetime],
	[totalSalePrice][money],
	[totalSalePriceNormalTax][money],
	[totalSalePriceReducedTax][money],
	[totalSalePriceNoTax][money],
	[totalSaleUnit][int],
	[postagePrice][money],
	[postagePriceTaxRate][varchar](5) collate Japanese_CI_AS,
	[chargePriceTaxRate][varchar](5)collate Japanese_CI_AS,
	[totalItemOptionPrice][money],
	[totalItemOptionPriceTaxRate][varchar](5)collate Japanese_CI_AS,
	[totalGiftWrappingPrice][money],
	[totalGiftWrappingPriceTaxRate][varchar](5)collate Japanese_CI_AS,
	[totalPrice][money],
	[totalPriceNormalTax][money],
	[totalPriceReducedTax][money],
	[totalPriceNoTax][money],
	[premiumType][varchar](1)collate Japanese_CI_AS,
	[premiumIssuePrice][money],
	[premiumMallPrice][money],
	[premiumShopPrice][money],
	[couponTotalPrice][money],
	[couponTotalPriceNormalTax	][money],
	[couponTotalPriceReducedTax][money],
	[couponTotalPriceNoTax][money],
	[usePoint][money],
	[usePointNormalTax][money],
	[usePointReducedTax][money],
	[usePointNoTax	][money],
	[usePointCancel][varchar](1)collate Japanese_CI_AS,
	[useAuPointPrice][money],
	[useAuPointPriceNormalTax][money],
	[useAuPointPriceReducedTax][money],
	[useAuPointPriceNoTax	][money],
	[useAuPoint][money],
	[useAuPointCancel][varchar](1)collate Japanese_CI_AS,
	[requestPrice	][money],
	[requestPriceNormalTax][money],
	[requestPriceReducedTax][money],
	[requestPriceNoTax][money],
	[pointFixedDate][date],
	[pointFixedStatus][varchar](1)collate Japanese_CI_AS,
	[settleStatus][varchar](2)collate Japanese_CI_AS,
	[authoriTimelimitDate][date],
	[pgResult][varchar](1)collate Japanese_CI_AS,
	[pgResponseCode][varchar](5)collate Japanese_CI_AS,
	[pgResponseDetail][varchar](255) collate Japanese_CI_AS,
	[pgOrderId][varchar](20) collate Japanese_CI_AS,
	[pgRequestPrice][money],
	[pgRequestPriceNormalTax][money],
	[pgRequestPriceReducedTax][money],
	[pgRequestPriceNoTax][money],
	[couponType][varchar](1)collate Japanese_CI_AS,
	[couponKey][varchar](32)collate Japanese_CI_AS,
	[cardJadgement][varchar](1)collate Japanese_CI_AS,
	[deliveryName][varchar](255)collate Japanese_CI_AS,
	[deliveryMethodId][varchar](10) collate Japanese_CI_AS,
	[deliveryId][varchar](10)collate Japanese_CI_AS,
	[deliveryRequestDay][date],
	[deliveryRequestTime][varchar](80)collate Japanese_CI_AS,
	[shippingDate][date],
	[shippingCarrier][varchar](1) collate Japanese_CI_AS,
	[shippingNumber][varchar](15) collate Japanese_CI_AS,
	[yamatoLnkMgtNo][varchar](70) collate Japanese_CI_AS)

		declare @idoc int,
		@InsertDate datetime=getdate()

		exec sp_xml_preparedocument @idoc output, @JuChuuXml
		insert into #tempOrderInfo
		SELECT *  FROM openxml(@idoc,'/NewDataSet/test',2)
		with(
		InportSEQ int,
		StoreCD varchar(4),
		APIKey tinyint,
		InportSEQRows int,
		orderDate date,
		orderId varchar(50),
		sellMethodSegment tinyint,
		releaseDate	date,
		siteAndDevice	varchar(1000),
		mailAddress	varchar(128),
		rawMailAddress	varchar(128),
		ordererName	varchar(110),
		ordererKana	varchar(110),	
		ordererZipCode	varchar(8),
		ordererAddress	varchar(370),
		ordererPhoneNumber1	varchar(30),
		ordererPhoneNumber2	varchar(30)	,
		nickname	varchar(40),
		senderName	varchar(110),
		senderKana	varchar(110),
		senderZipCode	varchar(8)	,
		senderAddress	varchar(370),
		senderPhoneNumber1	varchar(30),
		senderPhoneNumber2	varchar(30),
		senderShopCD	varchar(4),
		orderOption	varchar(2000),
		settlementName	varchar(50)	,
		secureSegment	tinyint	,
		userComment	varchar(4000),
		memo	varchar(4000),
		orderStatus	varchar(32)	,
		contactStatus	varchar(1),
		contactDate	datetime,
		authorizationStatus	varchar(1)	,
		authorizationDate	datetime,
		paymentStatus	varchar(1),
		paymentDate	datetime,
		shipStatus	varchar(1),
		shipDate	datetime,
		printStatus	varchar(1),
		printDate	datetime,
		cancelStatus	varchar(1),
		cancelReason	varchar(2),
		cancelComment	varchar(100),
		cancelDate	datetime,
		totalSalePrice	money,
		totalSalePriceNormalTax	money,
		totalSalePriceReducedTax	money,
		totalSalePriceNoTax	money,
		totalSaleUnit	int,
		postagePrice	money,
		postagePriceTaxRate	varchar(5),
		chargePrice	money,
		chargePriceTaxRate	varchar(5),
		totalItemOptionPrice	money,
		totalItemOptionPriceTaxRate	varchar(5),
		totalGiftWrappingPrice	money,
		totalGiftWrappingPriceTaxRate	varchar(5),
		totalPrice	money,
		totalPriceNormalTax	money,
		totalPriceReducedTax	money,
		totalPriceNoTax	money,
		premiumType	varchar(1),
		premiumIssuePrice	money,
		premiumMallPrice	money,
		premiumShopPrice	money,
		couponTotalPrice	money,
		couponTotalPriceNormalTax	money,
		couponTotalPriceReducedTax	money,
		couponTotalPriceNoTax	money,
		usePoint	money,
		usePointNormalTax	money,
		usePointReducedTax	money,
		usePointNoTax	money,
		usePointCancel	varchar(1),
		useAuPointPrice	money,
		useAuPointPriceNormalTax	money,
		useAuPointPriceReducedTax	money,
		useAuPointPriceNoTax	money,
		useAuPoint	money,
		useAuPointCancel	varchar(1),
		requestPrice	money,
		requestPriceNormalTax	money,
		requestPriceReducedTax	money,
		requestPriceNoTax	money,
		pointFixedDate	date,
		pointFixedStatus	varchar(1),
		settleStatus	varchar(2),
		authoriTimelimitDate	date,
		pgResult	varchar(1),
		pgResponseCode	varchar(5),
		pgResponseDetail	varchar(255),
		pgOrderId	varchar(20),
		pgRequestPrice	money,
		pgRequestPriceNormalTax	money,
		pgRequestPriceReducedTax	money,
		pgRequestPriceNoTax	money,
		couponType	varchar(1),
		couponKey	varchar(32),
		cardJadgement	varchar(1),
		deliveryName	varchar(255),
		deliveryMethodId	varchar(10),
		deliveryId	varchar(10),
		deliveryRequestDay	date,
		deliveryRequestTime	varchar(80),
		shippingDate	date,
		shippingCarrier	varchar(1),
		shippingNumber	varchar(15),
		yamatoLnkMgtNo	varchar(70))
		exec sp_xml_removedocument @idoc

		INSERT INTO D_WowmaJuchuu(
		InportSEQ,
		StoreCD,				
		APIKey,
		InportSEQRows ,
		orderDate,
		orderId,
		sellMethodSegment,
		releaseDate,
		siteAndDevice,
		mailAddress,
		rawMailAddress,
		ordererName,
		ordererKana,
		ordererZipCode,
		ordererAddress,
		ordererPhoneNumber1,
		ordererPhoneNumber2,
		nickname,
		senderName,
		senderKana,
		senderZipCode,
		senderAddress,
		senderPhoneNumber1,
		senderPhoneNumber2,
		senderShopCD,
		orderOption,
		settlementName,
		secureSegment,
		userComment,
		memo,
		orderStatus,
		contactStatus,
		contactDate,
		authorizationStatus,
		authorizationDate,
		paymentStatus,
		paymentDate,
		shipStatus,
		shipDate,
		printStatus,
		printDate,
		cancelStatus,
		cancelReason,
		cancelComment,
		cancelDate,
		totalSalePrice,
		totalSalePriceNormalTax,
		totalSalePriceReducedTax,
		totalSalePriceNoTax,
		totalSaleUnit,
		postagePrice,
		postagePriceTaxRate,
		chargePrice,
		chargePriceTaxRate,
		totalItemOptionPrice,
		totalItemOptionPriceTaxRate,
		totalGiftWrappingPrice,
		totalGiftWrappingPriceTaxRate,
		totalPrice,
		totalPriceNormalTax,
		totalPriceReducedTax,
		totalPriceNoTax,
		premiumType,
		premiumIssuePrice,
		premiumMallPrice,
		premiumShopPrice,
		couponTotalPrice,
		couponTotalPriceNormalTax,
		couponTotalPriceReducedTax,
		couponTotalPriceNoTax,
		usePoint,
		usePointNormalTax,
		usePointReducedTax,
		usePointNoTax,
		usePointCancel,
		useAuPointPrice,
		useAuPointPriceNormalTax,
		useAuPointPriceReducedTax,
		useAuPointPriceNoTax,
		useAuPoint,
		useAuPointCancel,
		requestPrice,
		requestPriceNormalTax,
		requestPriceReducedTax,
		requestPriceNoTax,
		pointFixedDate,
		pointFixedStatus,
		settleStatus,
		authoriTimelimitDate,
		pgResult,
		pgResponseCode,
		pgResponseDetail,
		pgOrderId,
		pgRequestPrice,
		pgRequestPriceNormalTax,
		pgRequestPriceReducedTax,
		pgRequestPriceNoTax,
		couponType,
		couponKey,
		cardJadgement,
		deliveryName,
		deliveryMethodId,
		deliveryId,
		deliveryRequestDay,
		deliveryRequestTime,
		shippingDate,
		shippingCarrier,
		shippingNumber,
		yamatoLnkMgtNo,
		InsertDateTime,
		UpdateDateTime)

		Select InportSEQ,
		StoreCD,
		APIKey,
		InportSEQRows,
		orderDate,
		orderId,
		sellMethodSegment,
		releaseDate,
		siteAndDevice,
		mailAddress,
		rawMailAddress,
		ordererName,
		ordererKana,
		ordererZipCode,
		ordererAddress,
		ordererPhoneNumber1,
		ordererPhoneNumber2,
		nickname,
		senderName,
		senderKana,
		senderZipCode,
		senderAddress,
		senderPhoneNumber1,
		senderPhoneNumber2,
		senderShopCD,
		orderOption,
		settlementName,
		secureSegment,
		userComment,
		memo,
		orderStatus,
		contactStatus,
		contactDate,
		authorizationStatus,
		authorizationDate,
		paymentStatus,
		paymentDate,
		shipStatus,
		shipDate,
		printStatus,
		printDate,
		cancelStatus,
		cancelReason,
		cancelComment,
		cancelDate,
		totalSalePrice,
		totalSalePriceNormalTax,
		totalSalePriceReducedTax,
		totalSalePriceNoTax,
		totalSaleUnit,
		postagePrice,
		postagePriceTaxRate,
		chargePrice,
		chargePriceTaxRate,
		totalItemOptionPrice,
		totalItemOptionPriceTaxRate,
		totalGiftWrappingPrice,
		totalGiftWrappingPriceTaxRate,
		totalPrice,
		totalPriceNormalTax,
		totalPriceReducedTax,
		totalPriceNoTax,
		premiumType,
		premiumIssuePrice,
		premiumMallPrice,
		premiumShopPrice,
		couponTotalPrice,
		couponTotalPriceNormalTax,
		couponTotalPriceReducedTax,
		couponTotalPriceNoTax,
		usePoint,
		usePointNormalTax,
		usePointReducedTax,
		usePointNoTax,
		usePointCancel,
		useAuPointPrice,
		useAuPointPriceNormalTax,
		useAuPointPriceReducedTax,
		useAuPointPriceNoTax,
		useAuPoint,
		useAuPointCancel,
		requestPrice,
		requestPriceNormalTax,
		requestPriceReducedTax,
		requestPriceNoTax,
		pointFixedDate,
		pointFixedStatus,
		settleStatus,
		authoriTimelimitDate,
		pgResult,
		pgResponseCode,
		pgResponseDetail,
		pgOrderId,
		pgRequestPrice,
		pgRequestPriceNormalTax,
		pgRequestPriceReducedTax,
		pgRequestPriceNoTax,
		couponType,
		couponKey,
		cardJadgement,
		deliveryName,
		deliveryMethodId,
		deliveryId,
		deliveryRequestDay,
		deliveryRequestTime,
		shippingDate,
		shippingCarrier,
		shippingNumber,
		yamatoLnkMgtNo,
		@Insertdatetime,
		@Insertdatetime

		from #tempOrderInfo

--end
	Drop table #tempOrderInfo
END
