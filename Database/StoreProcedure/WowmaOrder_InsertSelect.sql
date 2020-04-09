 BEGIN TRY 
 Drop Procedure dbo.[WowmaOrder_InsertSelect]
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
CREATE PROCEDURE [dbo].[WowmaOrder_InsertSelect]
	-- Add the parameters for the stored procedure here
	@OrderListXml as xml,
	@APIKey tinyint,
	@StoreCD varchar(4),	
	@LastUpdatedBefore datetime,
	@LastUpdatedAfter datetime
AS
BEGIN



		CREATE TABLE [dbo].[#tempOrderList](	[SEQ] [int] NOT NULL,	[orderId][varchar](50) collate Japanese_CI_AS)

	
		declare @DocHandle int,
		@InsertDateTime datetime=getdate()

		exec sp_xml_preparedocument @DocHandle output, @OrderListXml

		insert into #tempOrderList(SEQ,orderId)
		select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
				with
				(
					SEQ Int,
					orderId varchar(50)
				)
		exec sp_xml_removedocument @DocHandle;


		Insert into D_APIRireki(StoreCD,APIKey,InsertDateTime)
		values(@StoreCD,@APIKey,@InsertDateTime)

		DECLARE @InportSEQ AS INT = SCOPE_IDENTITY();

		Insert into D_WowmaRequest(InportSEQ,StoreCD,APIKey,LastUpdatedAfter,LastUpdatedBefore,InsertDateTime,UpdateDateTime)
		values					(@InportSEQ,@StoreCD,@APIKey,@LastUpdatedAfter,@LastUpdatedBefore,@InsertDateTime,@InsertDateTime)


		Insert into D_APIDetail(InportSEQ,StoreCD,APIKey,SEQ,OrderId)
		select @InportSEQ,@StoreCD,@APIKey,SEQ,orderId
		from #tempOrderList


		Insert into D_WowmaList(InportSEQ,StoreCD,APIKey,InportSEQRows,LastUpdatedBefore,LastUpdatedAfter,WowmaOrderId,InsertDateTime,UpdateDateTime)
		select @InportSEQ,@StoreCD,@APIKey,SEQ,@LastUpdatedBefore,@LastUpdatedAfter,orderId,@InsertDateTime,@InsertDateTime
		from #Tmp_D_APIRireki

		select WowmaOrderId,StoreCD,APIKey,InportSEQ
		from D_WowmaList
		where InportSEQ=@InportSEQ

		drop table #tempOrderList
	
--else if @Type=2
--	begin

--declare @idoc int,
--@InsertDate datetime=getdate()

----declare @impxml as xml
----select replace(@OrderListXml,'UTF-8', 'UTF-16')
--exec sp_xml_preparedocument @idoc output, @OrderListXml


--SELECT * into #tempOrderInfo FROM openxml(@idoc,'/NewDataSet/test',2)
--with(
--InportSEQ int,
--StoreCD varchar(4),
--APIKey tinyint,
--InportSEQRows int,
--orderDate date,
--orderId varchar(50),
--sellMethodSegment tinyint,
--releaseDate	date,
--siteAndDevice	varchar(1000),
--mailAddress	varchar(128),
--rawMailAddress	varchar(128),
--ordererName	varchar(110),
--ordererKana	varchar(110),	
--ordererZipCode	varchar(8),
--ordererAddress	varchar(370),
--ordererPhoneNumber1	varchar(30),
--ordererPhoneNumber2	varchar(30)	,
--nickname	varchar(40),
--senderName	varchar(110),
--senderKana	varchar(110),
--senderZipCode	varchar(8)	,
--senderAddress	varchar(370),
--senderPhoneNumber1	varchar(30),
--senderPhoneNumber2	varchar(30),
--senderShopCD	varchar(4),
--orderOption	varchar(2000),
--settlementName	varchar(50)	,
--secureSegment	tinyint	,
--userComment	varchar(4000),
--memo	varchar(4000),
--orderStatus	varchar(32)	,
--contactStatus	varchar(1),
--contactDate	datetime,
--authorizationStatus	varchar(1)	,
--authorizationDate	datetime,
--paymentStatus	varchar(1),
--paymentDate	datetime,
--shipStatus	varchar(1),
--shipDate	datetime,
--printStatus	varchar(1),
--printDate	datetime,
--cancelStatus	varchar(1),
--cancelReason	varchar(2),
--cancelComment	varchar(100),
--cancelDate	datetime,
--totalSalePrice	money,
--totalSalePriceNormalTax	money,
--totalSalePriceReducedTax	money,
--totalSalePriceNoTax	money,
--totalSaleUnit	int,
--postagePrice	money,
--postagePriceTaxRate	varchar(5),
--chargePrice	money,
--chargePriceTaxRate	varchar(5),
--totalItemOptionPrice	money,
--totalItemOptionPriceTaxRate	varchar(5),
--totalGiftWrappingPrice	money,
--totalGiftWrappingPriceTaxRate	varchar(5),
--totalPrice	money,
--totalPriceNormalTax	money,
--totalPriceReducedTax	money,
--totalPriceNoTax	money,
--premiumType	varchar(1),
--premiumIssuePrice	money,
--premiumMallPrice	money,
--premiumShopPrice	money,
--couponTotalPrice	money,
--couponTotalPriceNormalTax	money,
--couponTotalPriceReducedTax	money,
--couponTotalPriceNoTax	money,
--usePoint	money,
--usePointNormalTax	money,
--usePointReducedTax	money,
--usePointNoTax	money,
--usePointCancel	varchar(1),
--useAuPointPrice	money,
--useAuPointPriceNormalTax	money,
--useAuPointPriceReducedTax	money,
--useAuPointPriceNoTax	money,
--useAuPoint	money,
--useAuPointCancel	varchar(1),
--requestPrice	money,
--requestPriceNormalTax	money,
--requestPriceReducedTax	money,
--requestPriceNoTax	money,
--pointFixedDate	date,
--pointFixedStatus	varchar(1),
--settleStatus	varchar(2),
--authoriTimelimitDate	date,
--pgResult	varchar(1),
--pgResponseCode	varchar(5),
--pgResponseDetail	varchar(255),
--pgOrderId	varchar(20),
--pgRequestPrice	money,
--pgRequestPriceNormalTax	money,
--pgRequestPriceReducedTax	money,
--pgRequestPriceNoTax	money,
--couponType	varchar(1),
--couponKey	varchar(32),
--cardJadgement	varchar(1),
--deliveryName	varchar(255),
--deliveryMethodId	varchar(10),
--deliveryId	varchar(10),
--deliveryRequestDay	date,
--deliveryRequestTime	varchar(80),
--shippingDate	date,
--shippingCarrier	varchar(1),
--shippingNumber	varchar(15),
--yamatoLnkMgtNo	varchar(70))

--exec sp_xml_removedocument @idoc
----DECLARE @InportSEQ AS INT = SCOPE_IDENTITY();
----declare @InportSEQRows int

--INSERT INTO D_WowmaJuchuu(
--InportSEQ,
--StoreCD,				
--APIKey,
--InportSEQRows ,
--orderDate,
--orderId,
--sellMethodSegment,
--releaseDate,
--siteAndDevice,
--mailAddress,
--rawMailAddress,
--ordererName,
--ordererKana,
--ordererZipCode,
--ordererAddress,
--ordererPhoneNumber1,
--ordererPhoneNumber2,
--nickname,
--senderName,
--senderKana,
--senderZipCode,
--senderAddress,
--senderPhoneNumber1,
--senderPhoneNumber2,
--senderShopCD,
--orderOption,
--settlementName,
--secureSegment,
--userComment,
--memo,
--orderStatus,
--contactStatus,
--contactDate,
--authorizationStatus,
--authorizationDate,
--paymentStatus,
--paymentDate,
--shipStatus,
--shipDate,
--printStatus,
--printDate,
--cancelStatus,
--cancelReason,
--cancelComment,
--cancelDate,
--totalSalePrice,
--totalSalePriceNormalTax,
--totalSalePriceReducedTax,
--totalSalePriceNoTax,
--totalSaleUnit,
--postagePrice,
--postagePriceTaxRate,
--chargePrice,
--chargePriceTaxRate,
--totalItemOptionPrice,
--totalItemOptionPriceTaxRate,
--totalGiftWrappingPrice,
--totalGiftWrappingPriceTaxRate,
--totalPrice,
--totalPriceNormalTax,
--totalPriceReducedTax,
--totalPriceNoTax,
--premiumType,
--premiumIssuePrice,
--premiumMallPrice,
--premiumShopPrice,
--couponTotalPrice,
--couponTotalPriceNormalTax,
--couponTotalPriceReducedTax,
--couponTotalPriceNoTax,
--usePoint,
--usePointNormalTax,
--usePointReducedTax,
--usePointNoTax,
--usePointCancel,
--useAuPointPrice,
--useAuPointPriceNormalTax,
--useAuPointPriceReducedTax,
--useAuPointPriceNoTax,
--useAuPoint,
--useAuPointCancel,
--requestPrice,
--requestPriceNormalTax,
--requestPriceReducedTax,
--requestPriceNoTax,
--pointFixedDate,
--pointFixedStatus,
--settleStatus,
--authoriTimelimitDate,
--pgResult,
--pgResponseCode,
--pgResponseDetail,
--pgOrderId,
--pgRequestPrice,
--pgRequestPriceNormalTax,
--pgRequestPriceReducedTax,
--pgRequestPriceNoTax,
--couponType,
--couponKey,
--cardJadgement,
--deliveryName,
--deliveryMethodId,
--deliveryId,
--deliveryRequestDay,
--deliveryRequestTime,
--shippingDate,
--shippingCarrier,
--shippingNumber,
--yamatoLnkMgtNo,
--InsertDateTime,
--UpdateDateTime)

--Select InportSEQ,
--StoreCD,
--APIKey,
--InportSEQRows,
--orderDate,
--orderId,
--sellMethodSegment,
--releaseDate,
--siteAndDevice,
--mailAddress,
--rawMailAddress,
--ordererName,
--ordererKana,
--ordererZipCode,
--ordererAddress,
--ordererPhoneNumber1,
--ordererPhoneNumber2,
--nickname,
--senderName,
--senderKana,
--senderZipCode,
--senderAddress,
--senderPhoneNumber1,
--senderPhoneNumber2,
--senderShopCD,
--orderOption,
--settlementName,
--secureSegment,
--userComment,
--memo,
--orderStatus,
--contactStatus,
--contactDate,
--authorizationStatus,
--authorizationDate,
--paymentStatus,
--paymentDate,
--shipStatus,
--shipDate,
--printStatus,
--printDate,
--cancelStatus,
--cancelReason,
--cancelComment,
--cancelDate,
--totalSalePrice,
--totalSalePriceNormalTax,
--totalSalePriceReducedTax,
--totalSalePriceNoTax,
--totalSaleUnit,
--postagePrice,
--postagePriceTaxRate,
--chargePrice,
--chargePriceTaxRate,
--totalItemOptionPrice,
--totalItemOptionPriceTaxRate,
--totalGiftWrappingPrice,
--totalGiftWrappingPriceTaxRate,
--totalPrice,
--totalPriceNormalTax,
--totalPriceReducedTax,
--totalPriceNoTax,
--premiumType,
--premiumIssuePrice,
--premiumMallPrice,
--premiumShopPrice,
--couponTotalPrice,
--couponTotalPriceNormalTax,
--couponTotalPriceReducedTax,
--couponTotalPriceNoTax,
--usePoint,
--usePointNormalTax,
--usePointReducedTax,
--usePointNoTax,
--usePointCancel,
--useAuPointPrice,
--useAuPointPriceNormalTax,
--useAuPointPriceReducedTax,
--useAuPointPriceNoTax,
--useAuPoint,
--useAuPointCancel,
--requestPrice,
--requestPriceNormalTax,
--requestPriceReducedTax,
--requestPriceNoTax,
--pointFixedDate,
--pointFixedStatus,
--settleStatus,
--authoriTimelimitDate,
--pgResult,
--pgResponseCode,
--pgResponseDetail,
--pgOrderId,
--pgRequestPrice,
--pgRequestPriceNormalTax,
--pgRequestPriceReducedTax,
--pgRequestPriceNoTax,
--couponType,
--couponKey,
--cardJadgement,
--deliveryName,
--deliveryMethodId,
--deliveryId,
--deliveryRequestDay,
--deliveryRequestTime,
--shippingDate,
--shippingCarrier,
--shippingNumber,
--yamatoLnkMgtNo,
--@InsertDate,
--@InsertDate

--from #tempOrderInfo

----update #tempOrderInfo 
----set  #tempOrderInfo.InportSEQ=dl.InportSEQ,
----	  #tempOrderInfo.StoreCD=dl.StoreCD,
----	  #tempOrderInfo.APIKey =dl.APIKey,
----	  #tempOrderInfo.InportSEQRows =dl.InportSEQRows
----from D_WowmaList dl



--	--select distinct  wowma.InportSEQ,api.StoreCD,api.APIKey from M_API as api
--	--inner join  D_WowmaList as wowma on wowma.APIKey=api.APIKey and wowma.StoreCD=api.StoreCD
--	--select StoreCD,APIKey,InportSEQ,InportSEQRows
--	--from D_WowmaList
--	--where InportSEQ=19
--	--order by InportSEQRows asc
--	end

--	Drop table #tempOrderInfo

END


