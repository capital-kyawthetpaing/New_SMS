 BEGIN TRY 
 Drop Procedure dbo.[D_RakutenJuchuuDetails_Insert]
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
CREATE PROCEDURE [dbo].[D_RakutenJuchuuDetails_Insert](
	-- Add the parameters for the stored procedure here
	@JuChuuDetailXml as xml,
	@Operator as varchar(10),
	@InsertDateTime as datetime)
AS
BEGIN
	CREATE TABLE [dbo].[#tempJuChuuDetail]
				(
				InportSEQ int,
				StoreCD varchar(4) collate Japanese_CI_AS,
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50) collate Japanese_CI_AS,
				basketRows		int,
				itemRows		int,
				itemDetailId	int,
				itemName		varchar(3072) collate Japanese_CI_AS,
				itemId			int,
				itemNumber		varchar(382) collate Japanese_CI_AS,
				manageNumber	varchar(382) collate Japanese_CI_AS,
				price			int,
				units			int,
				includePostageFlag	tinyint,
				includeTaxFlag		tinyint,
				includeCashOnDeliveryPostageFlag tinyint,
				selectedChoice	varchar(500) collate Japanese_CI_AS,
				pointRate	int,
				pointType	smallint,
				inventoryType	tinyint,
				delvdateInfo	varchar(96) collate Japanese_CI_AS,
				restoreInventoryFlag  tinyint,
				dealFlag			  tinyint,
				drugFlag			  tinyint,
				deleteItemFlag		  tinyint				
			)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @JuChuuDetailXml
	insert into #tempJuChuuDetail
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				InportSEQ int,
				StoreCD varchar(4),
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50),
				basketRows		int,
				itemRows		int,
				itemDetailId	int,
				itemName		varchar(3072),
				itemId			int,
				itemNumber		varchar(382),
				manageNumber	varchar(382),
				price			int,
				units			int,
				includePostageFlag	tinyint,
				includeTaxFlag		tinyint,
				includeCashOnDeliveryPostageFlag tinyint,
				selectedChoice	varchar(500),
				pointRate	int,
				pointType	smallint,
				inventoryType	tinyint,
				delvdateInfo	varchar(96),
				restoreInventoryFlag  tinyint,
				dealFlag			  tinyint,
				drugFlag			  tinyint,
				deleteItemFlag		  tinyint				
			)
			exec sp_xml_removedocument @DocHandle;

			INSERT INTO D_RakutenJuchuuDetails(InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,itemRows,itemDetailId,itemName,itemId,itemNumber,manageNumber,price,units,includePostageFlag,
				includeTaxFlag,includeCashOnDeliveryPostageFlag,selectedChoice,pointRate,pointType,inventoryType,delvdateInfo,restoreInventoryFlag,dealFlag,drugFlag,deleteItemFlag,
				InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
			select InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,itemRows,itemDetailId,itemName,itemId,itemNumber,manageNumber,price,units,includePostageFlag,
				includeTaxFlag,includeCashOnDeliveryPostageFlag,selectedChoice,pointRate,pointType,inventoryType,delvdateInfo,restoreInventoryFlag,dealFlag,drugFlag,deleteItemFlag,
			@Operator,@InsertDateTime,@Operator,@InsertDateTime
			from #tempJuChuuDetail

			drop table #tempJuChuuDetail
	
END

