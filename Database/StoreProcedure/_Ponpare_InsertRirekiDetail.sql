 BEGIN TRY 
 Drop Procedure dbo.[_Ponpare_InsertRirekiDetail]
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
Create PROCEDURE [dbo].[_Ponpare_InsertRirekiDetail]
	-- Add the parameters for the stored procedure here
				@StoreCD as varchar(15),
				@API_Key as varchar(15),
				@LastUpdatedBefore as dateTime ,
				@LastUpdatedAfter as datetime ,
				@xml as xml


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
			
			declare	@Date as DateTime= getdate() --
			
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
		insert into D_PonpareRequest(InportSEQ, StoreCD,APIKey,LastUpdatedAfter,LastUpdatedBefore,InsertDateTime,UpdateDateTime)
				values(@val, @StoreCD,@API_Key,@LastUpdatedAfter,@LastUpdatedBefore, @Date,@Date)
			    
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--4		
	insert into D_PonpareList(InportSEQ,StoreCD,APIKey,InportSEQRows,PonpareOrderNo,LastUpdatedAfter,LastUpdatedBefore, InsertOperator, InsertDateTime, UpdateOperator, UpdateDateTime) 
				(select @val, *,@LastUpdatedAfter,@LastUpdatedBefore ,Null,@Date,null, @Date  from #temp)


	drop table #temp


END

