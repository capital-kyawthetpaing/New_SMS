 BEGIN TRY 
 Drop Procedure dbo.[D_RakutenShippingDetails_Insert]
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
CREATE PROCEDURE [dbo].[D_RakutenShippingDetails_Insert](
	-- Add the parameters for the stored procedure here
	@ShippingDetailXml as xml,
	@Operator as varchar(10),
	@InsertDateTime as datetime)
AS
BEGIN
	
	CREATE TABLE [dbo].[#tempShippingDetail]
	(
				InportSEQ int,
				StoreCD varchar(4) collate Japanese_CI_AS,
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50) collate Japanese_CI_AS,
				basketRows int,
				ShippingRows int,
				shippingDetailId	int,
				shippingNumber varchar(100) collate Japanese_CI_AS,
				deliveryCompany varchar(10) collate Japanese_CI_AS,
				deliveryCompanyName	varchar(64) collate Japanese_CI_AS,
				shippingDate date			
			)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @ShippingDetailXml
	insert into #tempShippingDetail
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				InportSEQ int,
				StoreCD varchar(4),
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50),
				basketRows int,
				ShippingRows int,
				shippingDetailId	int,
				shippingNumber varchar(100),
				deliveryCompany varchar(10),
				deliveryCompanyName	varchar(64),
				shippingDate date			
			)
			exec sp_xml_removedocument @DocHandle;

			INSERT INTO D_RakutenShippingDetails(InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,ShippingRows,shippingDetailId,shippingNumber,deliveryCompany,deliveryCompanyName,shippingDate,
			InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
			select InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,ShippingRows,shippingDetailId,shippingNumber,deliveryCompany,deliveryCompanyName,shippingDate,
			@Operator,@InsertDateTime,@Operator,@InsertDateTime
			from #tempShippingDetail

			drop table #tempShippingDetail

	
END

