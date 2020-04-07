 BEGIN TRY 
 Drop Procedure dbo.[D_WowmaJuchuuDetails_Insert]
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
CREATE PROCEDURE[dbo].[D_WowmaJuchuuDetails_Insert]
	-- Add the parameters for the stored procedure here
	@DetailXml  xml,
	@Insertdatetime datetime
AS
BEGIN

CREATE TABLE [dbo].[#tempOrderDetail](
			InportSEQ	int,
			StoreCD	varchar(4),
			APIKey	tinyint,
			InportSEQRows	int,
			orderId	varchar(50),
			orderRows	int,
			orderDetailId	int,
			itemManagementId	varchar(1000),
			itemCode	varchar(256),
			lotnumber	int,
			itemName	varchar(128),
			itemOption	varchar(7500),
			itemOptionCommission	varchar(7500),
			itemOptionPrice	money,
			giftWrappingType	varchar(20),
			giftWrappingPrice	money,
			giftMessage	varchar(200),
			noshiType	varchar(20),
			noshiPresenterName1	varchar(100),
			noshiPresenterName2	varchar(100),
			noshiPresenterName3	varchar(100),
			itemCancelStatus	varchar(1),
			itemCancelDate	date,
			beforeDiscount	money,
			discount	money,
			itemPrice	money,
			unit	int,
			totalItemPrice	money,
			totalItemChargePrice	money,
			taxType	varchar(1),
			reducedTax	varchar(1),
			taxRate	varchar(5),
			giftPoint	money,
			shippingDayDispText	varchar(40),
			shippingTimelimitDate	date,
			InsertOperator	varchar(10),
			InsertDateTime	datetime,
			UpdateOperator	varchar(10),
			UpdateDateTime	datetime)
		declare @idoc int,
		@InsertDate datetime=getdate()

		exec sp_xml_preparedocument @idoc output, @DetailXml
		insert into #tempOrderDetail
		SELECT *  FROM openxml(@idoc,'/NewDataSet/test',2)
		with(InportSEQ	int,
			[StoreCD][varchar](4) collate Japanese_CI_AS,
			[APIKey][tinyint],
			[InportSEQRows][int],
			[orderId][varchar](50) collate Japanese_CI_AS,
			[orderRows][int],
			[orderDetailId][int],
			[itemManagementId][varchar](1000) collate Japanese_CI_AS,
			[itemCode][varchar](256) collate Japanese_CI_AS,
			[lotnumber][int],
			[itemName][varchar](128) collate Japanese_CI_AS,
			[itemOption][varchar](7500) collate Japanese_CI_AS,
			[itemOptionCommission][varchar](7500) collate Japanese_CI_AS,
			[itemOptionPrice][money],
			[giftWrappingType	][varchar](20) collate Japanese_CI_AS,
			[giftWrappingPrice][money],
			[giftMessage][varchar](200) collate Japanese_CI_AS,
			[noshiType][varchar](20) collate Japanese_CI_AS,
			[noshiPresenterName1][varchar](100) collate Japanese_CI_AS,
			[noshiPresenterName2][varchar](100) collate Japanese_CI_AS,
			[noshiPresenterName3][varchar](100) collate Japanese_CI_AS,
			[itemCancelStatus][varchar](1) collate Japanese_CI_AS,
			[itemCancelDate][date],
			[beforeDiscount][money],
			[discount][money],
			[itemPrice][money],
			[unit][int],
			[totalItemPrice][money],
			[totalItemChargePrice][money],
			[taxType][varchar](1) collate Japanese_CI_AS,
			[reducedTax][varchar](1) collate Japanese_CI_AS,
			[taxRate][varchar](5) collate Japanese_CI_AS,
			[giftPoint][money],
			[shippingDayDispText][varchar](40) collate Japanese_CI_AS,
			[shippingTimelimitDate][date],
			[InsertOperator][varchar](10) collate Japanese_CI_AS,
			[InsertDateTime][datetime],
			[UpdateOperator][varchar](10) collate Japanese_CI_AS,
			[UpdateDateTime][datetime])
exec sp_xml_removedocument @idoc

		INSERT INTO D_WowmaJuchuuDetails(InportSEQ,StoreCD,APIKey,InportSEQRows,orderId,orderRows,orderDetailId,itemManagementId,itemCode,lotnumber,itemName,itemOption,
		itemOptionCommission,itemOptionPrice,giftWrappingType,giftWrappingPrice,giftMessage,noshiType,noshiPresenterName1,noshiPresenterName2,noshiPresenterName3,
		itemCancelStatus,itemCancelDate,beforeDiscount,discount,itemPrice,unit,totalItemPrice,totalItemChargePrice,taxType,reducedTax,taxRate,giftPoint,shippingDayDispText,
		shippingTimelimitDate,InsertDateTime,UpdateDateTime)

		Select	InportSEQ,StoreCD,APIKey,InportSEQRows,orderId,orderRows,orderDetailId,itemManagementId,itemCode,lotnumber,itemName,itemOption,itemOptionCommission,
				itemOptionPrice,giftWrappingType,giftWrappingPrice,giftMessage,noshiType,noshiPresenterName1,noshiPresenterName2,noshiPresenterName3,itemCancelStatus,
				itemCancelDate,beforeDiscount,discount,itemPrice,unit,totalItemPrice,totalItemChargePrice,taxType,reducedTax,taxRate,giftPoint,shippingDayDispText,
				shippingTimelimitDate,@Insertdatetime,@Insertdatetime

		FROM  #tempOrderDetail

		DROP TABLE #tempOrderDetail


END

