 BEGIN TRY 
 Drop Procedure dbo.[Amazon_API_InsertOrderDetails]
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
CREATE PROCEDURE [dbo].[Amazon_API_InsertOrderDetails]
	-- Add the parameters for the stored procedure here
				@StoreCD as varchar(15) ,
				@API_Key as varchar(15) ,
				@LastUpdatedBefore as dateTime ,
				@LastUpdatedAfter as datetime ,
				@xmldetail as xml ,
				@xml as xml 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
			declare	@Date as DateTime= getdate() 
			
				 declare @val as int 
			     set @val = (select Max(IsNull(InportSEQ,0))+1 from  D_APIRireki);
	             if (@val is null)
	             Begin
	             set @val=1;
	             End
				 ---Author - PTK
				 ---Effective tables   
				 -- D_ApIRireki, D_AmazonRequest, D_APIDetail, D_AmazonList,
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--1			
	insert into D_APIRireki(StoreCD, APIKey,InsertDateTime)  
				values(@StoreCD,@API_Key,@Date)		
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--2  		
				 CREATE TABLE [dbo].[#temp](
			   	 --InportSEQ  int,		
                 StoreCD varchar(4) collate Japanese_CI_AS,					
                 APIKey tinyint,					
                 SEQ int,
                 OrderId varchar(50)	 collate Japanese_CI_AS)
	             declare @DocHandle int
 	             exec sp_xml_preparedocument @DocHandle output, @Xml
	             insert into #temp
	             select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			     with
			     (
			   	 --InportSEQ  int,		
                 StoreCD varchar(4),					
                 APIKey tinyint,					
                 SEQ int,
                 OrderId varchar(50)	
			     )
				 exec sp_xml_removedocument @DocHandle;

			 
		insert into D_APIDetail(InportSEQ,StoreCD,APIKey,SEQ,OrderId ) select @val, * from #temp
		
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--3			
		insert into D_AmazonRequest(InportSEQ, StoreCD,APIKey,LastUpdatedAfter,LastUpdatedBefore,InsertDateTime,UpdateDateTime)
				values(@val, @StoreCD,@API_Key,@LastUpdatedAfter,@LastUpdatedBefore, @Date,@Date)
			    
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--4		
	insert into D_AmazonList(InportSEQ,StoreCD,APIKey,InportSEQRows,AmazonOrderId,LastUpdatedAfter,LastUpdatedBefore, InsertOperator, InsertDateTime, UpdateOperator, UpdateDateTime) 
				(select @val, *,@LastUpdatedAfter,@LastUpdatedBefore ,Null,@Date,null, @Date  from #temp)
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--5	 

			--	declare @xmldetail as xml =N''
				 declare @DocHandledetails int;
 	             exec sp_xml_preparedocument @DocHandledetails output, @xmldetail
	             select * into #tmp
	               FROM OPENXML (@DocHandledetails, '/NewDataSet/test',2)
				 with (
				 InportSEQ int,
				 StoreCD varchar(15),
				 APIKey int,
				 InportSEQRows int,
				 AmazonOrderId varchar(20),
				 JuchuuNO varchar(11),
				 SellerOrderId varchar(50),
				 PurchaseDate datetime ,
				 LastUpdatedDate datetime,
				 OrderStatus varchar(50), --Changed tinyint 
				 FulfillmentChannel varchar(3),
				 SalesChannel varchar(100),
				 OrderChannel varchar(100),
				 ShipServiceLevel varchar(100),
				 AddressName varchar(100),
				 AddressLine1 varchar(100),
				 AddressLine2 varchar(100),
				 AddressLine3 varchar(100),
				 AddressCity varchar(30),
				 AddressCounty varchar(30),
				 AddressDistrict varchar(30),
				 StateorRegion varchar(100) ,
				 PostalCode varchar(10),
				 CountryCode varchar(2),
				 Phone varchar(30),
				 CurrencyCode varchar(3),
				 Amount money,
				 NumberOfItemsShipped int,
				 NumberOfItemsUnShipped int,
				 PaymentMethod varchar(30),
				 PaymentMethodDetail varchar(30),
				 
				 PaymentCurrencyCode1 varchar(3),
				 PaymentAmount1 money,
				 PaymentMethod1 varchar(30),

				 PaymentCurrencyCode2 varchar(3),
				 PaymentAmount2 money,
				 PaymentMethod2 varchar(30),

				 PaymentCurrencyCode3 varchar(3),
				 PaymentAmount3 money,
				 PaymentMethod3 varchar(30),

				 PaymentCurrencyCode4 varchar(3),
				 PaymentAmount4 money,
				 PaymentMethod4 varchar(30),

				 IsReplacementOrder tinyint,
				 ReplacedOrderId varchar(20),
				 MarketplaceId varchar(50),
				 BuyerEmail varchar(100),
				 BuyerName varchar(30),
				 CompanyLegalName varchar(30),
				 TaxingRegion varchar(30),
				 TaxClassificationName varchar(30),
				 TaxClassificationValue varchar(30),
				 ShipmentServiceLevelCategory varchar(30),
				 OrderType varchar(50), --Changed tinyint
				 EarliestShipDate date,
				 LatestShipDate date,
				 EarliestDeliveryDate date,
				 LatestDeliveryDate date,
				 IsBusinessOrder varchar(10), -- Changed tinyint
				 PurchaseOrderNumber varchar(30),
				 IsPrime varchar(10),  -- Changed tinyint
				 IsPremiumOrder varchar(10),  --Changed tinyint
				 PromiseResponseDueDate date,
				 IsEstimatedShipDateSet varchar(10) --Changed tinyint
				 )
				 exec sp_xml_removedocument @DocHandledetails;

				  update #tmp set inportSeq =@val
				 insert into [D_AmazonJuchuu] 
				 select *
				, Null as InsertOperator, @Date as InsertDateTime,Null as UpdateOperator, @Date as UpdateDateTime 
				  from #tmp

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Remove temp tables				
				 drop table #tmp
				 drop table #temp  


				-- truncate table D_AmazonJuchuu
				 --select * from D_AmazonJuchuu


END

