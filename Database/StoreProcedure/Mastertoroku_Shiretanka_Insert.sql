BEGIN TRY 
 Drop Procedure [dbo].[Mastertoroku_Shiretanka_Insert]
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
Create PROCEDURE [dbo].[Mastertoroku_Shiretanka_Insert]
	
	@tbsku as Xml,
	@tbitem as Xml,
	@tbdelItem as Xml,
	@tbdelJan as Xml,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @date as datetime=getdate()

	DECLARE @DocDel int	
	EXEC sp_xml_preparedocument @DocDel OUTPUT, @tbdelItem

	 SELECT *
	  Into #tmpdel
	  FROM OPENXML (@DocDel, '/NewDataSet/test',2)
	WITH(
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4),
	MakerItem VARCHAR(30),
	ChangeDate date
	)

	EXEC sp_xml_removedocument @DocDel; 


Delete m
FROM M_ItemOrderPrice as m
INNER JOIN #tmpdel as d
  ON d.MakerItem=m.MakerItem

WHERE d.VendorCD=m.VendorCD
and d.StoreCD=m.StoreCD
and d.ChangeDate=m.ChangeDate
	 

	 DECLARE @DocHandle int	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @tbitem

	 SELECT *
	  Into #tmpit
	  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
	WITH(
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4),
	MakerItem VARCHAR(30),
	ChangeDate date,
	Rate decimal(5,2),
	PriceWithoutTax money
	)

	EXEC sp_xml_removedocument @DocHandle; 



	Insert 
	Into M_ItemOrderPrice
	(
	Vendorcd,
	StoreCD,
	MakerItem,
	ChangeDate,
	Rate,
	PriceWithoutTax,
	DeleteFlg,
	UsedFlg,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime

	)
	Select
	VendorCD,
	StoreCD,
	MakerItem,
	CONVERT(VARCHAR(10),ChangeDate, 111) as ChangeDate,
	Rate,
	PriceWithoutTax,
	0 as 'DeleteFlg',
	0 as 'UsedFlg',
	@operator  as 'InsertOperator',
	getdate() as 'InsertDateTime',
	'pc' as 'UpdateOperator',
	getdate() as 'UpdateDateTime'
	
	From #tmpit   





	DECLARE @DodelJan int	
	EXEC sp_xml_preparedocument @DodelJan OUTPUT, @tbdelJan
	 SELECT * 
	 into #tmpdeljan
	 FROM OPENXML (@DodelJan, '/NewDataSet/test',3)
	WITH(
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4),
	AdminNO int,
	ChangeDate date
	)
	EXEC sp_xml_removedocument @DodelJan; 


	
	Delete J
	FROM M_JanorderPrice as J
	INNER JOIN #tmpdeljan as d
	  ON d.AdminNO=J.AdminNO
	
	WHERE d.VendorCD=J.VendorCD
	and d.StoreCD=J.StoreCD
	and d.ChangeDate=J.ChangeDate

	

	DECLARE @Docsku int	
	EXEC sp_xml_preparedocument @Docsku OUTPUT, @tbsku
	 SELECT * 
	 into #tmpsk
	 FROM OPENXML (@Docsku, '/NewDataSet/test',3)
	WITH(
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4),
	AdminNO int,
	SKUCD varchar(40),
	CheckBox  Varchar,
	SizeName Varchar(20),
	ColorName Varchar(20),
	ITemCD Varchar(30),
	MakerItem VARCHAR(30),
	Rate decimal(5,2),
	ChangeDate date,
	PriceOutTax money,
	PriceWithoutTax money
	)
	EXEC sp_xml_removedocument @Docsku; 

	

	Insert 
	Into M_JanOrderPrice
	(
	Vendorcd,
	StoreCD,
	AdminNO,
	ChangeDate,
	SKUCD,
	Rate,
	PriceWithoutTax,
	Remarks,
	DeleteFlg,
	UsedFlg,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime
	)
	Select
	Vendorcd,
	StoreCD,
	tmp.AdminNO,
	CONVERT(VARCHAR(10),ChangeDate, 111) as ChangeDate,
	tmp.SKUCD,
	tmp.Rate,
	tmp.PriceWithoutTax,
	NUll,
	0 as 'DeleteFlg',
	0 as 'UsedFlg',
	@operator as 'InsertOperator',
	getdate() as  'InsertDateTime',
	'pc' as 'UpdateOperator',
	getdate() as 'UpdateDateTime'


	From #tmpsk as tmp


drop Table #tmpsk
drop Table #tmpit
drop Table #tmpdel
drop Table #tmpdeljan

		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END
