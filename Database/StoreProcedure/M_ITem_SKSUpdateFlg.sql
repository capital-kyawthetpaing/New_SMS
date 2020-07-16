
 BEGIN TRY 
 Drop Procedure dbo.M_ITem_SKSUpdateFlg
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE M_ITem_SKSUpdateFlg
	-- Add the parameters for the stored procedure here
	@xmlMasterItem as xml,
	@xmlMasterSKU as xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Declare 
	@InsertDateTime datetime=getdate()

	---For M_Item

	CREATE TABLE [dbo].[#tempItem]
			(
				
				[Item_Code][varchar](30) collate Japanese_CI_AS,
				
			)

	declare @idoc  int

	exec sp_xml_preparedocument @idoc output, @xmlMasterItem
			insert into #tempItem
			SELECT *  FROM openxml(@idoc,'/NewDataSet/test',2)
			WITH
			(
				[Item_Code][varchar](30) collate Japanese_CI_AS
			)
			exec sp_xml_removedocument @idoc


		Update mitem
		set mitem.SKSUpdateFlg=0,
		SKSUpdateDateTime=@InsertDateTime
		From M_ITem as mitem inner join #tempItem as t on mitem.ITemCD=t.Item_Code
														and mitem.SKSUpdateFlg=1

	----For M_SKU

		CREATE TABLE [dbo].[#tempSKU]
			(
				
				[Item_AdminCode] [int]
				
			)

		declare @idoc1  int

		exec sp_xml_preparedocument @idoc output, @xmlMasterSKU
				insert into #tempSKU
				SELECT *  FROM openxml(@idoc,'/NewDataSet/test',2)
				WITH
				(
					[Item_AdminCode] [int] 
				)
				exec sp_xml_removedocument @idoc1


		Update msku
		set msku.SKUUpdateFlg=0,
		SKSUpdateDateTime=@InsertDateTime
		From M_SKU as msku inner join #tempSKU as tsku on msku.AdminNO=tsku.Item_AdminCode
														and  msku.SKSUpdateFlg=1


			drop table #tempItem
			drop table #tempSKU
END
GO
