 BEGIN TRY 
 Drop Procedure dbo.[D_SKEN_InsertData]
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
Create PROCEDURE [dbo].[D_SKEN_InsertData]
	-- Add the parameters for the stored procedure here
	@VendorCD varchar(13),
	@xmlSKENDelivery xml,
	@xmlSKENDeliveryDetails xml,
	@Operator varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	--To Insert DSKENDelivery
	Declare @DocHandle int,
	@Date datetime =getdate()

	CREATE TABLE [dbo].[#tempSKENDelivery]
	(
				ImportFile	varchar(200) collate Japanese_CI_AS
				,ImportDetailsSu	int
				,ErrorSu	int
				,ErrorKBN	tinyint
				,ErrorText	varchar(30) collate Japanese_CI_AS
				,SKENRecordKBN	varchar(1) collate Japanese_CI_AS
				,SKENDataKBN varchar(2) collate Japanese_CI_AS
				,SKENTorihikisakiCD		varchar(13) collate Japanese_CI_AS
				,SKENTorihikisakiMei	varchar(20) collate Japanese_CI_AS
				,SKENNouhinmotoCD		varchar(13) collate Japanese_CI_AS
				,SKENNouhinmotoMei		varchar(20) collate Japanese_CI_AS
				,SKENHanbaitenCD		varchar(13) collate Japanese_CI_AS
				,SKENHanbaitenMei		varchar(20) collate Japanese_CI_AS
				,SKENSyukkasakiCD		varchar(13) collate Japanese_CI_AS
				,SKENSyukkasakiMei		varchar(20) collate Japanese_CI_AS
				,SKENNouhinshoNO		varchar(10) collate Japanese_CI_AS
				,SKENDenpyouKBN			varchar(3) collate Japanese_CI_AS
				,SKENJuchuuDate			varchar(8) collate Japanese_CI_AS
				,SKENSyukkaDate			varchar(8) collate Japanese_CI_AS
				,SKENNouhinDate			varchar(8) collate Japanese_CI_AS
				,SKENHacchuu			varchar(10) collate Japanese_CI_AS
				,SKENHacchuuKBN			varchar(4) collate Japanese_CI_AS
				,SKENDenpyoua		varchar(10) collate Japanese_CI_AS
				,SKENDenpyoub varchar(20) collate Japanese_CI_AS
				,SKENDenpyouc		varchar(10) collate Japanese_CI_AS
				,SKENDenpyoud		varchar(10) collate Japanese_CI_AS
				,SKENUnsouHouhou	varchar(12) collate Japanese_CI_AS
				,SKENKosuu			int
				,SKENUnchinKBN		varchar(2) collate Japanese_CI_AS
				,SKENSyogakari		money
				,SKENUnchin			money
				,SKENShinadai		money
				,SKENSyouhiZei		money
				,SKENSougoukei		money
				,SKENMakerDenpyou	varchar(10) collate Japanese_CI_AS
				,SKENMotoDenNO	varchar(10) collate Japanese_CI_AS
				,SKENYobi1		varchar(10) collate Japanese_CI_AS
				,SKENYobi2		varchar(10) collate Japanese_CI_AS
				,SKENYobi3		varchar(10) collate Japanese_CI_AS
				,SKENYobi4		varchar(10) collate Japanese_CI_AS
				,SKENYobi5		varchar(10) collate Japanese_CI_AS
	)

	exec sp_xml_preparedocument @DocHandle output, @xmlSKENDelivery
	insert into #tempSKENDelivery
	select * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
 
				ImportFile	varchar(200)
				,ImportDetailsSu	int
				,ErrorSu	int
				,ErrorKBN	tinyint
				,ErrorText	varchar(30)
				,SKENRecordKBN	varchar(1)
				,SKENDataKBN varchar(2)
				,SKENTorihikisakiCD		varchar(13)
				,SKENTorihikisakiMei	varchar(20)
				,SKENNouhinmotoCD		varchar(13)
				,SKENNouhinmotoMei		varchar(20)
				,SKENHanbaitenCD		varchar(13)
				,SKENHanbaitenMei		varchar(20)
				,SKENSyukkasakiCD		varchar(13)
				,SKENSyukkasakiMei		varchar(20)
				,SKENNouhinshoNO		varchar(10)
				,SKENDenpyouKBN			varchar(3)
				,SKENJuchuuDate			varchar(8)
				,SKENSyukkaDate			varchar(8)
				,SKENNouhinDate			varchar(8)
				,SKENHacchuu			varchar(10)
				,SKENHacchuuKBN			varchar(4)
				,SKENDenpyoua		varchar(10)
				,SKENDenpyoub varchar(20)
				,SKENDenpyouc		varchar(10)
				,SKENDenpyoud		varchar(10)
				,SKENUnsouHouhou	varchar(12)
				,SKENKosuu			int
				,SKENUnchinKBN		varchar(2)
				,SKENSyogakari		money
				,SKENUnchin			money
				,SKENShinadai		money
				,SKENSyouhiZei		money
				,SKENSougoukei		money
				,SKENMakerDenpyou	varchar(10)
				,SKENMotoDenNO	varchar(10)
				,SKENYobi1		varchar(10)
				,SKENYobi2		varchar(10)
				,SKENYobi3		varchar(10)
				,SKENYobi4		varchar(10)
				,SKENYobi5		varchar(10)


			)
			exec sp_xml_removedocument @DocHandle;

	Insert into D_SKENDelivery(

		ImportDateTime
		,StaffCD
		,VendorCD
		,ImportFile
		,ImportDetailsSu
		,ErrorSu
		,ErrorKBN
		,ErrorText
		,SKENRecordKBN
		,SKENDataKBN
		,SKENTorihikisakiCD
		,SKENTorihikisakiMei
		,SKENNouhinmotoCD
		,SKENNouhinmotoMei
		,SKENHanbaitenCD
		,SKENHanbaitenMei
		,SKENSyukkasakiCD
		,SKENSyukkasakiMei
		,SKENNouhinshoNO
		,SKENDenpyouKBN
		,SKENJuchuuDate
		,SKENSyukkaDate
		,SKENNouhinDate
		,SKENHacchuu
		,SKENHacchuuKBN
		,SKENDenpyoua
		,SKENDenpyoub
		,SKENDenpyouc
		,SKENDenpyoud
		,SKENUnsouHouhou
		,SKENKosuu
		,SKENUnchinKBN
		,SKENSyogakari
		,SKENUnchin
		,SKENShinadai
		,SKENSyouhiZei
		,SKENSougoukei
		,SKENMakerDenpyou
		,SKENMotoDenNO
		,SKENYobi1
		,SKENYobi2
		,SKENYobi3
		,SKENYobi4
		,SKENYobi5
		,InsertOperator
		,InsertDateTime
		,ProcessedDateTime

		)
		select 
		@Date
		,null
		,@VendorCD
		,ImportFile
		,ImportDetailsSu
		,ErrorSu
		,ErrorKBN
		,ErrorText
		,SKENRecordKBN
		,SKENDataKBN
		,SKENTorihikisakiCD
		,SKENTorihikisakiMei
		,SKENNouhinmotoCD
		,SKENNouhinmotoMei
		,SKENHanbaitenCD
		,SKENHanbaitenMei
		,SKENSyukkasakiCD
		,SKENSyukkasakiMei
		,SKENNouhinshoNO
		,SKENDenpyouKBN
		,SKENJuchuuDate
		,SKENSyukkaDate
		,SKENNouhinDate
		,SKENHacchuu
		,SKENHacchuuKBN
		,SKENDenpyoua
		,SKENDenpyoub
		,SKENDenpyouc
		,SKENDenpyoud
		,SKENUnsouHouhou
		,SKENKosuu
		,SKENUnchinKBN
		,SKENSyogakari
		,SKENUnchin
		,SKENShinadai
		,SKENSyouhiZei
		,SKENSougoukei
		,SKENMakerDenpyou
		,SKENMotoDenNO
		,SKENYobi1
		,SKENYobi2
		,SKENYobi3
		,SKENYobi4
		,SKENYobi5
		,@Operator
		,@Date
		,null
		from #tempSKENDelivery

	
	--To Insert DSKENDeliveryDetail
	CREATE TABLE [dbo].[#tempSKENDeliveryDetail]
	(
				ImportFile	varchar(200) collate Japanese_CI_AS
				,ErrorKBN	tinyint
				,ErrorText	varchar(30) collate Japanese_CI_AS
				,SKENRecordKBN	varchar(1) collate Japanese_CI_AS
				,SKENDataKBN	varchar(2) collate Japanese_CI_AS
				,SKENTorihikisakiCD	varchar(13) collate Japanese_CI_AS
				,SKENTorihikisakiMei	varchar(20) collate Japanese_CI_AS
				,SKENNouhinmotoCD	varchar(13) collate Japanese_CI_AS
				,SKENNouhinmotoMei	varchar(20) collate Japanese_CI_AS
				,SKENHanbaitenCD	varchar(13) collate Japanese_CI_AS
				,SKENHanbaitenMei	varchar(20) collate Japanese_CI_AS
				,SKENSyukkasakiCD	varchar(13) collate Japanese_CI_AS
				,SKENSyukkasakiMei	varchar(20) collate Japanese_CI_AS
				,SKENNouhinshoNO	varchar(10) collate Japanese_CI_AS
				,SKENNouhinshoNOGyou	tinyint
				,SKENNouhinshoNORetsu	tinyint
				,SKENDenpyouKBN	varchar(3) collate Japanese_CI_AS
				,SKENJuchuuDate	varchar(8) collate Japanese_CI_AS
				,SKENSyukkaDate	varchar(8) collate Japanese_CI_AS
				,SKENNouhinDate	varchar(8) collate Japanese_CI_AS
				,SKENHacchuu	varchar(10) collate Japanese_CI_AS
				,SKENHacchuuKBN	varchar(4) collate Japanese_CI_AS
				,SKENHacchuuShouhinCD	varchar(13) collate Japanese_CI_AS
				,SKENNouhinHinban	varchar(10) collate Japanese_CI_AS
				,SKENMakerKikaku1	varchar(5) collate Japanese_CI_AS
				,SKENMakerKikaku2	varchar(10) collate Japanese_CI_AS
				,SKENTani	varchar(3) collate Japanese_CI_AS
				,SKENTorihikiTanka	money
				,SKENHyoujyunJyoudai	money
				,SKENBrandmei	varchar(10)
				,SKENSyouhinmei	varchar(15)
				,SKENJanCD	varchar(13) collate Japanese_CI_AS
				,AdminNO	int
				,SKENNouhinSuu	int
				,SKENMakerDenpyou	varchar(10) collate Japanese_CI_AS
				,SKENMotoDenNO	varchar(10) collate Japanese_CI_AS
				,SKENYobi1	varchar(10) collate Japanese_CI_AS
				,SKENYobi2	varchar(10) collate Japanese_CI_AS
				,SKENYobi3	varchar(10) collate Japanese_CI_AS
				,SKENYobi4	varchar(10) collate Japanese_CI_AS
				,SKENYobi5	varchar(10) collate Japanese_CI_AS
				,DeliveryNo	int
	)
	Declare @DocHandle1 int
	exec sp_xml_preparedocument @DocHandle1 output, @xmlSKENDeliveryDetails
	insert into #tempSKENDeliveryDetail
	select *  FROM OPENXML (@DocHandle1, '/NewDataSet/test',2)
			with
			(

				ImportFile	varchar(200)
				,ErrorKBN	tinyint
				,ErrorText	varchar(30)
				,SKENRecordKBN	varchar(1)
				,SKENDataKBN	varchar(2)
				,SKENTorihikisakiCD	varchar(13)
				,SKENTorihikisakiMei	varchar(20)
				,SKENNouhinmotoCD	varchar(13)
				,SKENNouhinmotoMei	varchar(20)
				,SKENHanbaitenCD	varchar(13)
				,SKENHanbaitenMei	varchar(20)
				,SKENSyukkasakiCD	varchar(13)
				,SKENSyukkasakiMei	varchar(20)
				,SKENNouhinshoNO	varchar(10)
				,SKENNouhinshoNOGyou	tinyint
				,SKENNouhinshoNORetsu	tinyint
				,SKENDenpyouKBN	varchar(3)
				,SKENJuchuuDate	varchar(8)
				,SKENSyukkaDate	varchar(8)
				,SKENNouhinDate	varchar(8)
				,SKENHacchuu	varchar(10)
				,SKENHacchuuKBN	varchar(4)
				,SKENHacchuuShouhinCD	varchar(13)
				,SKENNouhinHinban	varchar(10)
				,SKENMakerKikaku1	varchar(5)
				,SKENMakerKikaku2	varchar(10)
				,SKENTani	varchar(3)
				,SKENTorihikiTanka	money
				,SKENHyoujyunJyoudai	money
				,SKENBrandmei	varchar(10)
				,SKENSyouhinmei	varchar(15)
				,SKENJanCD	varchar(13)
				,AdminNO	int
				,SKENNouhinSuu	int
				,SKENMakerDenpyou	varchar(10)
				,SKENMotoDenNO	varchar(10)
				,SKENYobi1	varchar(10)
				,SKENYobi2	varchar(10)
				,SKENYobi3	varchar(10)
				,SKENYobi4	varchar(10)
				,SKENYobi5	varchar(10)
				,DeliveryNo	int

			)
			exec sp_xml_removedocument @DocHandle1;

	Insert into D_SKENDeliveryDetails(
	ImportDateTime
	,StaffCD
	,VendorCD
	,ImportFile
	,ErrorKBN
	,ErrorText
	,SKENRecordKBN
	,SKENDataKBN
	,SKENTorihikisakiCD
	,SKENTorihikisakiMei
	,SKENNouhinmotoCD
	,SKENNouhinmotoMei
	,SKENHanbaitenCD
	,SKENHanbaitenMei
	,SKENSyukkasakiCD
	,SKENSyukkasakiMei
	,SKENNouhinshoNO
	,SKENNouhinshoNOGyou
	,SKENNouhinshoNORetsu
	,SKENDenpyouKBN
	,SKENJuchuuDate
	,SKENSyukkaDate
	,SKENNouhinDate
	,SKENHacchuu
	,SKENHacchuuKBN
	,SKENHacchuuShouhinCD
	,SKENNouhinHinban
	,SKENMakerKikaku1
	,SKENMakerKikaku2
	,SKENTani
	,SKENTorihikiTanka
	,SKENHyoujyunJyoudai
	,SKENBrandmei
	,SKENSyouhinmei
	,SKENJanCD
	,AdminNO
	,SKENNouhinSuu
	,SKENMakerDenpyou
	,SKENMotoDenNO
	,SKENYobi1
	,SKENYobi2
	,SKENYobi3
	,SKENYobi4
	,SKENYobi5
	,DeliveryNo
	,InsertOperator
	,InsertDateTime
	,ProcessedDateTime

	)
	select 
	@Date
	,null
	,@VendorCD
	,ImportFile
	,ErrorKBN
	,ErrorText
	,SKENRecordKBN
	,SKENDataKBN
	,SKENTorihikisakiCD
	,SKENTorihikisakiMei
	,SKENNouhinmotoCD
	,SKENNouhinmotoMei
	,SKENHanbaitenCD
	,SKENHanbaitenMei
	,SKENSyukkasakiCD
	,SKENSyukkasakiMei
	,SKENNouhinshoNO
	,SKENNouhinshoNOGyou
	,SKENNouhinshoNORetsu
	,SKENDenpyouKBN
	,SKENJuchuuDate
	,SKENSyukkaDate
	,SKENNouhinDate
	,SKENHacchuu
	,SKENHacchuuKBN
	,SKENHacchuuShouhinCD
	,SKENNouhinHinban
	,SKENMakerKikaku1
	,SKENMakerKikaku2
	,SKENTani
	,SKENTorihikiTanka
	,SKENHyoujyunJyoudai
	,SKENBrandmei
	,SKENSyouhinmei
	,SKENJanCD
	,AdminNO
	,SKENNouhinSuu
	,SKENMakerDenpyou
	,SKENMotoDenNO
	,SKENYobi1
	,SKENYobi2
	,SKENYobi3
	,SKENYobi4
	,SKENYobi5
	,DeliveryNo
	,@Operator
	,@Date
	,null
	from #tempSKENDeliveryDetail

	--Select Data from D_SKENDelivery with ProcessedDateTime is null
	select SKENBangouA,SKENNouhinshoNO
	into #tempSKENBangou
	from D_SKENDelivery 
	where ProcessedDateTime is null
	order by SKENBangouA

	--Check with M_Vendor
	Update D_SKENDelivery 
	set ErrorKBN=2
	from D_SKENDelivery as dskend 
	where not exists(select * from  F_Vendor(@Date) as fv 
						where dskend.VendorCD=fv.EDIVendorCD and fv.DeleteFlg is null)
	

	--Check Date
	Update D_SKENDelivery
	set ErrorKBN=5
	from D_SKENDelivery as dskend 
	where (SKENSyukkaDate is not null and ISDATE(SKENSyukkaDate)=0)
	and ImportDateTime=@Date
	and ErrorKBN=0

	--Update ErrorMsg to DSKENDelivery
	Update D_SKENDelivery
	set ErrorText=case when dskend.ErrorKBN=2 then N'発注先が正しくない' 
					when dskend.ErrorKBN=5 then N'入荷予定日が正しくない'
					end,
		ProcessedDateTime=@Date
	from D_SKENDelivery as dskend inner join #tempSKENBangou as tb on dskend.SKENBangou=tb.SKENBangouA


	--Select Data from D_SKENDeliveryDetails with ProcessedDateTime is null
	
	Select dskendd.SKENBangouB
	into #tempSKENBangouB
	from D_SKENDeliveryDetails as dskendd inner join #tempSKENBangou as tb on dskendd.SKENNouhinshoNO=tb.SKENNouhinshoNO
	where ProcessedDateTime is null
	order by SKENBangouB

	--Check OrderRows
	Update D_SKENDeliveryDetails
	set ErrorKBN=3
	from D_SKENDeliveryDetails as dskendd
	where not exists (select * from D_OrderDetails as dod 
					where dod.OrderNO=left(dskendd.SKENSyouhinmei,11)
					and dod.OrderRows=SUBSTRING(dskendd.SKENSyouhinmei,13,15))


	--Check JanCD
	Update D_SKENDeliveryDetails
	set ErrorKBN=4
	from D_SKENDeliveryDetails as dskendd 
	where not exists(select 1 from D_OrderDetails
					where JanCD=dskendd.SKENJanCD)

	--Check OrderSu
	Update D_SKENDeliveryDetails
	set ErrorKBN=6
	from D_SKENDeliveryDetails as dskendd 
	where  exists(select 1 from D_OrderDetails
					where OrderSu<dskendd.SKENNouhinSuu)

	--Update ErrorMsg to D_SKENDeliveryDetails
	Update D_SKENDeliveryDetails
	set ErrorText=case when dskendd.ErrorKBN=3 then N'発注番号・明細が存在しない' 
					when dskendd.ErrorKBN=4 then N'商品・数量が発注情報と異なっている'
					when dskendd.ErrorKBN=6 then N'発注数より入荷予定数が多い'
					end,
		ProcessedDateTime=@Date
	from D_SKENDeliveryDetails as dskendd inner join #tempSKENBangouB as tb on dskendd.SKENBangouB=tb.SKENBangouB

	--Insert to D_Delivery
	Declare 
	@SEQ int =0
	Insert into D_Delivery(
		VendorCD
		,ReceivedDate
		,ReceivedType
		,VendorDeliveryNo
		,VendorDeliveryType
		,VendorDeliveryDate
		,SEQ
		,OrderNO
		,OrderRows
		,MakerItem
		,SKUCD
		,AdminNO
		,JanCD
		,ItemName
		,ColorName
		,SizeName
		,DeliverySu
		,DeliveryUnitPrice
		,DeliveryPrice
		,PurchaseSu
		,Remark1
		,Remark2
		,Remark3
		,Remark4
		,Remark5
		,MatchingDatetime
		,MatchingFlg
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		,DeleteOperator
		,DeleteDateTime

	)
	select @VendorCD,
	@Date,
	1,
	dskend.SKENNouhinshoNO,
	dskend.SKENDenpyouKBN,
	dskend.SKENSyukkaDate,
	row_number() over (order by (select NULL)),
	left(dskendd.SKENSyouhinmei,11),
	SUBSTRING(dskendd.SKENSyouhinmei,13,15),
	dskendd.SKENNouhinHinban,
	fsku.SKUCD,
	fsku.AdminNO,
	dskendd.SKENJanCD,
	fsku.SKUName,
	fsku.ColorName,
	fsku.SizeName,
	dskendd.SKENNouhinSuu,
	dskendd.SKENTorihikiTanka,
	dskendd.SKENNouhinSuu * dskendd.SKENTorihikiTanka,
	0,
	dskendd.SKENYobi1,
	dskendd.SKENYobi2,
	dskendd.SKENYobi3,
	dskendd.SKENYobi4,
	dskendd.SKENYobi5,
	NUll,
	0,
	NULL,
	@Date,
	null,
	@Date,
	null,
	null

	from D_SKENDelivery as dskend 
	inner join D_SKENDeliveryDetails as dskendd on dskend.SKENNouhinshoNO=dskendd.SKENNouhinshoNO
	Cross apply F_SKUByJanCD(dskendd.SKENJanCD) as fsku
	where fsku.SetKBN=0



	Update D_SKENDelivery
	set ImportDetailsSu=(select count(*) from D_SKENDeliveryDetails as ds
										where ds.SKENNouhinshoNO=dskend.SKENNouhinshoNO
										and ErrorKBN=0),

	ErrorSu=(select count(*) from D_SKENDeliveryDetails as ds
										where ds.SKENNouhinshoNO=dskend.SKENNouhinshoNO
										and ErrorKBN<>0)
	from D_SKENDelivery as dskend inner join 
	#tempSKENBangou as tb on dskend.SKENBangou=tb.SKENBangou

	exec dbo.L_Log_Insert @Operator,'EDINouhinJouhonTouroku',null,null,NULL

	Drop table #tempSKENDelivery
	Drop table #tempSKENDeliveryDetail
	Drop table #tempSKENBangou
	Drop table #tempSKENBangouB
END

