 BEGIN TRY 
 Drop Procedure dbo.[ImportYahooShipping]
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
CREATE PROCEDURE [dbo].[ImportYahooShipping]
	-- Add the parameters for the stored procedure here
@JuChuuXml as xml

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE [dbo].[#temp]
	(
			   --InportSEQ  int,		
				 StoreCD varchar(4) collate Japanese_CI_AS,			
				 APIKey		int,
                 InportSEQRows	int,						
                 YahooOrderId	varchar(150) collate Japanese_CI_AS,						
                 ShipRows int,							
                 ShipStatus	 tinyint,						
                 ShipMethod		varchar(9) collate Japanese_CI_AS,					
                 ShipMethodName	varchar(150) collate Japanese_CI_AS,			
                 ShipRequestDate				date,			
                 ShipRequestTime		varchar(13) collate Japanese_CI_AS,		
                 ShipNotes				varchar(500) collate Japanese_CI_AS,
                 ShipCompanyCode		varchar(4) collate Japanese_CI_AS,
                 ShipInvoiceNumber1	varchar(30) collate Japanese_CI_AS,
                 ShipInvoiceNumber2	varchar(30) collate Japanese_CI_AS,
                 ShipInvoiceNumberEmptyReason	varchar(3) collate Japanese_CI_AS,
				 ShipUrl		varchar(100) collate Japanese_CI_AS,							
                 ArriveType	tinyint,				
                 ShipDate			date,	
                 ArrivalDate		date,	
                 NeedGiftWrap		tinyint,
                 GiftWrapType	    varchar(30) collate Japanese_CI_AS,
                 GiftWrapMessage	 varchar(300) collate Japanese_CI_AS,
                 NeedGiftWrapPaper	tinyint,
                 GiftWrapPaperType	varchar(30) collate Japanese_CI_AS,
                 GiftWrapName		varchar(300) collate Japanese_CI_AS,
                 Option1Field		varchar(150) collate Japanese_CI_AS,
                 Option1Type		tinyint,
                 Option1Value		varchar(1500) collate Japanese_CI_AS,
                 Option2Field		varchar(150) collate Japanese_CI_AS,
                 Option2Type		tinyint,
                 Option2Value		varchar(1500) collate Japanese_CI_AS,
                 ShipFirstName				varchar(300) collate Japanese_CI_AS,
                 ShipFirstNameKana		varchar(300) collate Japanese_CI_AS,
                 ShipLastName				varchar(300) collate Japanese_CI_AS,
                 ShipLastNameKana		varchar(300) collate Japanese_CI_AS,
                 ShipZipCode				varchar(10) collate Japanese_CI_AS,
                 ShipPrefecture				varchar(12) collate Japanese_CI_AS,
                 ShipPrefectureKana		varchar(18) collate Japanese_CI_AS,	
                 ShipCity						varchar(300) collate Japanese_CI_AS,
                 ShipCityKana				varchar(300) collate Japanese_CI_AS,
                 ShipAddress1				varchar(300) collate Japanese_CI_AS,
                 ShipAddress1Kana		varchar(300) collate Japanese_CI_AS,
                 ShipAddress2				varchar(300) collate Japanese_CI_AS,
                 ShipAddress2Kana			varchar(300) collate Japanese_CI_AS,
                 ShipPhoneNumber			varchar(14) collate Japanese_CI_AS,
                 ShipEmgPhoneNumber	varchar(14) collate Japanese_CI_AS,
                 ShipSection1Field			varchar(300) collate Japanese_CI_AS,
                 ShipSection1Value			varchar(300) collate Japanese_CI_AS,
                 ShipSection2Field			varchar(300) collate Japanese_CI_AS,
                 ShipSection2Value			varchar(300) collate Japanese_CI_AS,
					OrderId varchar(50) collate Japanese_CI_AS
			)
	
	        declare @DocHandle int
 	        exec sp_xml_preparedocument @DocHandle output, @JuChuuXml
			insert into #temp
	        select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
			   	                              --InportSEQ  int,		
				                              StoreCD varchar(4),			
				                              APIKey		int,
                                              InportSEQRows	int,						
                                              YahooOrderId	varchar(150),						
                                              ShipRows int,							
                                              ShipStatus	 tinyint,						
                                              ShipMethod		varchar(9),					
                                              ShipMethodName				varchar(150),			
                                              ShipRequestDate				date,			
                                              ShipRequestTime					varchar(13),		
                                              ShipNotes							varchar(500),
                                              ShipCompanyCode							varchar(4),
                                              ShipInvoiceNumber1							varchar(30),
                                              ShipInvoiceNumber2							varchar(30),
                                              ShipInvoiceNumberEmptyReason							varchar(3),
											  ShipUrl							varchar(100),							
                                              ArriveType							tinyint,				
                                              ShipDate									date,	
                                              ArrivalDate								date,	
                                              NeedGiftWrap										tinyint,
                                              GiftWrapType							varchar(30),
                                              GiftWrapMessage							varchar(300),
                                              NeedGiftWrapPaper							tinyint,
                                              GiftWrapPaperType							varchar(30),
                                              GiftWrapName							varchar(300),
                                              Option1Field							varchar(150),
                                              Option1Type							tinyint,
                                              Option1Value							varchar(1500),
                                              Option2Field							varchar(150),
                                              Option2Type							tinyint,
                                              Option2Value							varchar(1500),
                                              ShipFirstName							varchar(300),
                                              ShipFirstNameKana							varchar(300),
                                              ShipLastName							varchar(300),
                                              ShipLastNameKana							varchar(300),
                                              ShipZipCode							varchar(10),
                                              ShipPrefecture							varchar(12),
                                              ShipPrefectureKana						varchar(18),	
                                              ShipCity							varchar(300),
                                              ShipCityKana							varchar(300),
                                              ShipAddress1							varchar(300),
                                              ShipAddress1Kana							varchar(300),
                                              ShipAddress2							varchar(300),
                                              ShipAddress2Kana							varchar(300),
                                              ShipPhoneNumber							varchar(14),
                                              ShipEmgPhoneNumber							varchar(14),
                                              ShipSection1Field							varchar(300),
                                              ShipSection1Value							varchar(300),
                                              ShipSection2Field							varchar(300),
                                              ShipSection2Value							varchar(300),
												OrderId varchar(50)
			)
			exec sp_xml_removedocument @DocHandle;

			declare @val as int , @val1 as int, @Datetime as DateTime;
			set @DateTime = getdate();
			set @val = (select Max(IsNull(InportSEQ,0))+1 from D_YahooShipping);

	        if (@val is null)
	        Begin
	        set @val=1;
	        End

		select * into #temp1  from #temp order by #temp.StoreCD asc

		



		--select * from #temp1
		select a.StoreCD,InportSEQRows,ROW_NUMBER() OVER (PARTITION BY a.OrderId order by a.storeCD) Rnum, APIKey into #temp2  from #temp1 a

	
		select * from #temp1

		Update t  set t.ShipRows = t1.Rnum, t.YahooOrderId = t.[OrderId] from #temp1 t inner join  
		#temp2 t1 on t.InportSEQRows = t1.InportSEQRows and  t.StoreCD =t1.StoreCD and t.APIKey= t1.APIKey
			
		Alter table #temp1 	Drop Column [OrderId]



	         insert into D_YahooShipping
			select @val,*  from #temp1
			--truncate table D_yahooList
			--select * from #temp1
			--select * from D_yahooList
			drop table #temp
			drop table #temp1
			drop table #temp2

END

