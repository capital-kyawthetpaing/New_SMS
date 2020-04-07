 BEGIN TRY 
 Drop Procedure dbo.[ImportYahooShippingDetail]
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
CREATE PROCEDURE [dbo].[ImportYahooShippingDetail]
	-- Add the parameters for the stored procedure here
@JuChuuXml as xml

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE [dbo].[#tempJuChuu]
	(		
				  StoreCD varchar (4)  collate Japanese_CI_AS,							
                  APIKey	 tinyint   ,						
                  InportSEQRows	int    ,					
                  YahooOrderId	varchar (50) collate Japanese_CI_AS ,					
                  OrderRows	int    ,				
                  LineId	tinyint,
                  ItemId	varchar(228) collate Japanese_CI_AS,					
                  Title		varchar(225) collate Japanese_CI_AS,				
                  SubCode	varchar(99) collate Japanese_CI_AS,						
                  SubCodeOption	varchar	(150) collate Japanese_CI_AS,			
                  IsUsed				tinyint,			
                  ImageId				varchar(160) collate Japanese_CI_AS,			
                  IsTaxable			tinyint,			
                  Jan						varchar(14) collate Japanese_CI_AS,	
                  ProductId			varchar(50) collate Japanese_CI_AS,	
                  CategoryId			varchar(12) collate Japanese_CI_AS,
                  AffiliateRatio_Store		decimal(3,1),			
                  AffiliateRatio_Yahoo	decimal(3,1),			
                  UnitPrice					money,		
                  Quantity						int,		
                  PointAvailQuantity		int,					
                  ReleaseDate				date,		
                  PointFspCode				int,					
                  PointRatioY					tinyint,					
                  PointRatioSeller			tinyint,					
                  UnitGetPoint				money,				
                  IsGetPointFix				tinyint,			
                    GetPointFixDate		date,			
                  CouponData				varchar(800) collate Japanese_CI_AS,	
                  CouponDiscount			money,				
                  CouponUseNum			int,	
                  OriginalPrice				money,
                  OriginalNum				int,
                  LeadTimeText				varchar(50) collate Japanese_CI_AS,
                  LeadTimeStart			date,
                  LeadTimeEnd				date,
                  PriceType					 tinyint,
	              ItemOption01Name		  varchar(1) collate Japanese_CI_AS,					
                  ItemOption01Value			varchar(90) collate Japanese_CI_AS,		
                  ItemOption01Price			money,
                  ItemOption01KBN			tinyint,
                  ItemOption02Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption02Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption02Price			money,
                  ItemOption02KBN			tinyint,
                  ItemOption03Name			varchar(1)collate Japanese_CI_AS,	
                  ItemOption03Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption03Price			money,
                  ItemOption03KBN			tinyint,
                  ItemOption04Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption04Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption04Price			money,
                  ItemOption04KBN			tinyint,
                  ItemOption05Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption05Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption05Price			money,
                  ItemOption05KBN			tinyint,
                  ItemOption06Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption06Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption06Price			money,
                  ItemOption06KBN			tinyint,
                  ItemOption07Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption07Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption07Price			money,
                  ItemOption07KBN			tinyint,
                  ItemOption08Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption08Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption08Price			money,
                  ItemOption08KBN			tinyint,
                  ItemOption09Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption09Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption09Price			money,
                  ItemOption09KBN			tinyint,
                  ItemOption10Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption10Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption10Price			money,
                  ItemOption10KBN			tinyint,
                  ItemOption11Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption11Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption11Price			money,
                  ItemOption11KBN			tinyint,
                  ItemOption12Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption12Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption12Price			money,
                  ItemOption12KBN			tinyint,
                  ItemOption13Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption13Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption13Price			money,
                  ItemOption13KBN			tinyint,
                  ItemOption14Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption14Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption14Price			money,
                  ItemOption14KBN			tinyint,
                  ItemOption15Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption15Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption15Price			money,
                  ItemOption15KBN			tinyint,
                  ItemOption16Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption16Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption16Price			money,
                  ItemOption16KBN			tinyint,
                  ItemOption17Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption17Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption17Price			money,
                  ItemOption17KBN			tinyint,
                  ItemOption18Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption18Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption18Price			money,
                  ItemOption18KBN			tinyint,
                  ItemOption19Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption19Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption19Price			money,
                  ItemOption19KBN			tinyint,
                  ItemOption20Name			varchar(1) collate Japanese_CI_AS,	
                  ItemOption20Value			varchar(90) collate Japanese_CI_AS,
                  ItemOption20Price			money,
                  ItemOption20KBN			tinyint,
				  OrderId		nvarchar(50)
			)
	
declare @DocHandle int;

	exec sp_xml_preparedocument @DocHandle output, @JuChuuXml
	insert into #tempJuChuu
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(				  

		               --   drop table #tempJuChuu
				  
				  
				  StoreCD varchar (4)  ,							
                  APIKey	 tinyint   ,						
                  InportSEQRows	int    ,					
                  YahooOrderId	varchar (50)  ,					
                  OrderRows	int    ,				
                  LineId	tinyint,
                  ItemId	varchar(228),					
                  Title		varchar(225),				
                  SubCode	varchar(99),						
                  SubCodeOption			varchar	(150),			
                  IsUsed				tinyint,			
                  ImageId				varchar(160),			
                  IsTaxable				tinyint,			
                  Jan						varchar(14),	
                  ProductId						varchar(50),	
                  CategoryId							varchar(12),
                  AffiliateRatio_Store				decimal(3,1),			
                  AffiliateRatio_Yahoo			decimal(3,1),			
                  UnitPrice					money,		
                  Quantity					int,		
                  PointAvailQuantity		int,					
                  ReleaseDate					date,		
                  PointFspCode		int,					
                  PointRatioY		tinyint,					
                  PointRatioSeller		tinyint,					
                  UnitGetPoint			money,				
                  IsGetPointFix				tinyint,			
                    GetPointFixDate				date,			
                  CouponData						varchar(800),	
                  CouponDiscount			money,				
                  CouponUseNum						int,	
                  OriginalPrice							money,
                  OriginalNum							int,
                  LeadTimeText							varchar(50),
                  LeadTimeStart							date,
                  LeadTimeEnd							date,
                  PriceType							    tinyint,
	              ItemOption01Name		                varchar(1),					
                  ItemOption01Value					    varchar(90),		
                  ItemOption01Price						money,
                  ItemOption01KBN						tinyint,
                  ItemOption02Name						varchar(1),	
                  ItemOption02Value						varchar(90),
                  ItemOption02Price						money,
                  ItemOption02KBN						tinyint,
                  ItemOption03Name						varchar(1),	
                  ItemOption03Value						varchar(90),
                  ItemOption03Price						money,
                  ItemOption03KBN						tinyint,
                  ItemOption04Name						varchar(1),	
                  ItemOption04Value						varchar(90),
                  ItemOption04Price						money,
                  ItemOption04KBN						tinyint,
                  ItemOption05Name						varchar(1),	
                  ItemOption05Value						varchar(90),
                  ItemOption05Price						money,
                  ItemOption05KBN						tinyint,
                  ItemOption06Name						varchar(1),	
                  ItemOption06Value						varchar(90),
                  ItemOption06Price						money,
                  ItemOption06KBN						tinyint,
                  ItemOption07Name						varchar(1),	
                  ItemOption07Value						varchar(90),
                  ItemOption07Price						money,
                  ItemOption07KBN						tinyint,
                  ItemOption08Name						varchar(1),	
                  ItemOption08Value						varchar(90),
                  ItemOption08Price						money,
                  ItemOption08KBN						tinyint,
                  ItemOption09Name						varchar(1),	
                  ItemOption09Value						varchar(90),
                  ItemOption09Price						money,
                  ItemOption09KBN						tinyint,
                  ItemOption10Name						varchar(1),	
                  ItemOption10Value						varchar(90),
                  ItemOption10Price						money,
                  ItemOption10KBN						tinyint,
                  ItemOption11Name						varchar(1),	
                  ItemOption11Value						varchar(90),
                  ItemOption11Price						money,
                  ItemOption11KBN						tinyint,
                  ItemOption12Name						varchar(1),	
                  ItemOption12Value						varchar(90),
                  ItemOption12Price						money,
                  ItemOption12KBN						tinyint,
                  ItemOption13Name						varchar(1),	
                  ItemOption13Value						varchar(90),
                  ItemOption13Price						money,
                  ItemOption13KBN						tinyint,
                  ItemOption14Name						varchar(1),	
                  ItemOption14Value						varchar(90),
                  ItemOption14Price						money,
                  ItemOption14KBN						tinyint,
                  ItemOption15Name						varchar(1),	
                  ItemOption15Value						varchar(90),
                  ItemOption15Price						money,
                  ItemOption15KBN						tinyint,
                  ItemOption16Name						varchar(1),	
                  ItemOption16Value						varchar(90),
                  ItemOption16Price						money,
                  ItemOption16KBN						tinyint,
                  ItemOption17Name						varchar(1),	
                  ItemOption17Value						varchar(90),
                  ItemOption17Price						money,
                  ItemOption17KBN						tinyint,
                  ItemOption18Name						varchar(1),	
                  ItemOption18Value						varchar(90),
                  ItemOption18Price						money,
                  ItemOption18KBN						tinyint,
                  ItemOption19Name						varchar(1),	
                  ItemOption19Value						varchar(90),
                  ItemOption19Price						money,
                  ItemOption19KBN						tinyint,
                  ItemOption20Name						varchar(1),	
                  ItemOption20Value						varchar(90),
                  ItemOption20Price						money,
                  ItemOption20KBN						tinyint,
				  OrderId		nvarchar(50)
			)
			exec sp_xml_removedocument @DocHandle;




	Declare @val as int,
	@valList as int,
	@DateTime as Datetime  ;
	set @val = (select Max(IsNull(InportSEQ,0))+1 from  D_YahooJuchuuDetails);

	if (@val is null)
	Begin
	set @val=1;
	End
	set @DateTime = getdate();
    -- Insert statements for procedure here
	--select * into #temp from #tempJuChuu d order by d.StoreCD asc

			select * into #temp from #tempJuChuu d order by d.StoreCD asc
			
			--select * from #temp1

			select a.StoreCD,InportSEQRows,ROW_NUMBER() OVER (PARTITION BY a.OrderId order by a.storeCD) Rnum, APIKey into #temp1  from #temp a

			Update #temp  set #temp.OrderRows = t1.Rnum, #temp.YahooOrderId = #temp.[OrderId] from #temp inner join  
			#temp1 t1 on #temp.InportSEQRows = t1.InportSEQRows and  #temp.StoreCD =t1.StoreCD and #temp.APIKey= t1.APIKey
		
		--select * from #temp

		--update #temp set #temp.YahooOrderId = #temp.[OrderID];
			Alter table #temp 	Drop Column [OrderId]

	insert into D_YahooJuchuuDetails select @val,*,Null,getdate(),Null,getdate() from #temp


	--select * from D_YahooJuchuuDetails

	--insert into D_YahooList(InportSEQ,StoreCD,APIKey,InportSEQRows,YahooOrderId,InsertOperator,InsertDateTime,UpdateOperator, UpdateDateTime)
	--select @valList,StoreCD,APIKey,SEQ,OrderInfo ,Null,@DateTime,null,@DateTime  from #temp
	Drop table #temp1
	Drop table #temp
	Drop table #tempJuChuu



END

