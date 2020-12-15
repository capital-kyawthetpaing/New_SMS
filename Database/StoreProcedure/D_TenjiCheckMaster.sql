 BEGIN TRY 
 Drop Procedure dbo.[D_TenjiCheckMaster]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE D_TenjiCheckMaster
	-- Add the parameters for the stored procedure here
	--@xml as xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	----declare @DocHandle int
	--												exec sp_xml_preparedocument @DocHandle output, @Xml
	--												select * into #temp FROM
	--												OPENXML (@DocHandle, '/NewDataSet/test',2)
	--												with
	--												(
	--												仮JANCD varchar(50),
	--												SKUCD varchar(50),
	--												商品名 varchar(50),
	--												カラー名 varchar(8),
	--												サイズ名 varchar(8),
	--												販売予定日 varchar(20),
	--												即納数 varchar(20),
	--												希望日1 varchar(20),
	--												希望日2 varchar(20)
	--												)
	--												EXEc sp_xml_removedocument @DocHandle;
	--
	--select * from #temp t where (t.[仮JANCD]) in (select JanCD from M_TenzikaiShouhin) 
		select *  from M_TenzikaiShouhin where DeleteFlg ='0'
		-- drop table #temp
END
GO
