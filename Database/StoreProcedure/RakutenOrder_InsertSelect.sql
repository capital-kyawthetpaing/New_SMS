 BEGIN TRY 
 Drop Procedure dbo.[RakutenOrder_InsertSelect]
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
CREATE PROCEDURE [dbo].[RakutenOrder_InsertSelect] 
	-- Add the parameters for the stored procedure here
	@OrderListXml as xml,
	@APIKey tinyint,
	@StoreCD varchar(4),
	@LastUpdatedBefore datetime,
	@LastUpdatedAfter datetime,
	@Operator varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#tempOrderList]
	(			
				[SEQ][Int] NOT NULL,
				[OrderNo][varchar](50) collate Japanese_CI_AS)
	
	declare @DocHandle int,
	@InsertDateTime datetime=getdate()

	exec sp_xml_preparedocument @DocHandle output, @OrderListXml
	insert into #tempOrderList(SEQ,OrderNo)
	select * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				SEQ Int,
				OrderNo varchar(50)
			)
			exec sp_xml_removedocument @DocHandle;

	
	Insert into D_APIRireki(StoreCD,APIKey,InsertOperator,InsertDateTime)
	values(@StoreCD,@APIKey,@Operator,@InsertDateTime)
	DECLARE @InportSEQ AS INT = SCOPE_IDENTITY();

	Insert into D_RakutenRequest(InportSEQ,StoreCD,APIKey,LastUpdatedBefore,LastUpdatedAfter,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	values(@InportSEQ,@StoreCD,@APIKey,@LastUpdatedBefore,@LastUpdatedAfter,@Operator,@InsertDateTime,@Operator,@InsertDateTime)

	Insert into D_APIDetail(InportSEQ,StoreCD,APIKey,SEQ,OrderId)
	select @InportSEQ,@StoreCD,@APIKey,SEQ,OrderNo
	from #tempOrderList

	Insert into D_RakutenList(InportSEQ,StoreCD,APIKey,InportSEQRows,LastUpdatedBefore,LastUpdatedAfter,RakutenOrderNumber,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	select @InportSEQ,@StoreCD,@APIKey,SEQ,@LastUpdatedBefore,@LastUpdatedAfter,OrderNo,@Operator,@InsertDateTime,@Operator,@InsertDateTime
	from #tempOrderList

	select RakutenOrderNumber,InportSEQ,InportSEQRows
	from D_RakutenList
	where InportSEQ=@InportSEQ
	order by InportSEQRows asc


	Drop table #tempOrderList

END

