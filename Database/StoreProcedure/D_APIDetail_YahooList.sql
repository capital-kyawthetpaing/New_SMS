 BEGIN TRY 
 Drop Procedure dbo.[D_APIDetail_YahooList]
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
CREATE PROCEDURE [dbo].[D_APIDetail_YahooList]
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
                 APIKey tinyint,					
                 SEQ int,
                 OrderId varchar(50)	 collate Japanese_CI_AS
			)
	declare @DocHandle int
 	exec sp_xml_preparedocument @DocHandle output, @JuChuuXml
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

			declare @val as int , @val1 as int, @Datetime as DateTime;
			set @DateTime = getdate();
			set @val = (select Max(IsNull(InportSEQ,0))+1 from  D_APIDetail);

	        if (@val is null)
	        Begin
	        set @val=1;
	        End

	insert into D_APIDetail(InportSEQ,StoreCD,APIKey,SEQ,OrderId ) select @val, * from #temp
	insert into D_yahooList(InportSEQ,StoreCD,APIKey,InportSEQRows,YahooOrderId, InsertOperator, InsertDateTime, UpdateOperator, UpdateDateTime) 
	(select @val, *, Null,@DateTime,null, @DateTime  from #temp)
		

			--truncate table D_yahooList

			--select * from D_APIDetail
			--select * from D_yahooList
			drop table #temp
END

