USE [CapitalSMS]
GO
/****** Object:  StoredProcedure [dbo].[M_SKUHenKou]    Script Date: 2020-08-27 14:28:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,> drop procedure  M_SKUHenKou
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[M_SKUHenKou]
	-- Add the parameters for the stored procedure here
	
					@OPTDate as Date,
					@OPTTime as Time,
					@InsertOPT as varchar(50),
					@Program as varchar(50),
					@PC as varchar(50),
					@OperateMode as varchar(50),
					@KeyItem as varchar(100),
					@xml as xml,
					@xml_1 xml 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Begin
	 declare @DocHandle int
     exec sp_xml_preparedocument @DocHandle output, @Xml
	
	  
     select * into #temp FROM
     OPENXML (@DocHandle, '/NewDataSet/test',2)
	 with
	(InsertOperator varchar(50),
	InsertDateTime datetime,
	AdminNO varchar(13) ,
	JanCD varchar(13),
	NewSKUCD varchar(30),
	SKUCD varchar(13),
	ChangeDate datetime ,
	DeleteFlg varchar(20) ,
	NewSizeNo varchar(13),
	SizeNo varchar(13),
	NewColorNo varchar(13),
	ColorNo varchar(13))
	EXEc sp_xml_removedocument @DocHandle; 		
	
	--*D_SKUUpdate*
	insert into D_SKUUpdate(InsertOperator, InsertDateTime, AdminNo,JanCD, NewSKUCD, SKUCD, ChangeDate, DeleteFlag)	select a.InsertOperator, InsertDateTime, AdminNo, JanCD,((NewSKUCD))as NewSKUCD , SKUCD, ChangeDate, DeleteFlg  from 	(select * from #temp where (NewSizeNo  <> SizeNo )or (NewColorNo <> ColorNo)) a

	--*M_SKU*
	update ms set ms.SKUCD=t.NewSKUCD, ms.SizeNo= t.NewSizeNO, ms.ColorNo= t.NewColorNo, ms.DeleteFlg= t.DeleteFlg from M_SKU ms right join  #temp t on ms.AdminNo = t.AdminNo and ms.ChangeDate = t.Changedate
					

	--*M_SKUChange

	 declare @DocHandle_1 int
     exec sp_xml_preparedocument @DocHandle_1 output, @Xml_1
	
	  
     select * into #temp_1 FROM
     OPENXML (@DocHandle_1, '/NewDataSet/test',2)
	 with(
	 InsertOperator varchar (50),
	 InsertDateTime varchar(50),
	 ItemCD varchar(30),
	 SizeColorKBN tinyint,
	 NewNo tinyint,
	 [No] int,
	 ChangeDate Datetime,
	 DeleteFlg tinyint
	 )
	 	EXEc sp_xml_removedocument @DocHandle_1; 
					insert into D_SKUChange (InsertOperator, InsertDateTime, ItemCD, SizeColorKBN,NewNo, [No],ChangeDate, DeleteFlg) select * from #Temp_1

					insert into L_Log(OperateDate, OPerateTime, InsertOperator, Program,PC,OperateMode, KeyItem) values(
					@OPTDate,
					@OPTTime,
					@InsertOPT,
					@Program,
					@PC,
					@OperateMode,
					@KeyItem
					)


					drop table #temp
					drop table #Temp_1
					End
END
