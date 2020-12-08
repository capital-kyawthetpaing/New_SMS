 BEGIN TRY 
 Drop Procedure [dbo].[M_SKUPrice_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_SKUPrice_Insert_Update]
@insertXML as xml,
@TankaCD as varchar(13),
@StartDate as Date,
@EndDate as Date,
@PriceWithoutTax as money,
@SalePriceOutTax as money,
@Remarks as varchar(500),
@Operator varchar(10),
@Program as varchar(30),
@PC as varchar(30),
@OperateMode as varchar(10),
@KeyItem as varchar(100),
@Mode as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--If @Mode=1
	
	Declare @DocHandle int
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @insertXml
	declare @currentDate as datetime = getdate();
	select * into #Temp
	from OPENXML(@DocHandle,'/NewDataSet/test',2)
	WITH(
	TanKaCD varchar(13),
	StoreCD varchar(4),
	AdminNO int,
	SKUCD varchar(30),
	StartChangeDate date,
	EndChangeDate date,
	PriceWithoutTax money,
	SalePriceOutTax money,
	Remarks varchar(500),
	DeleteFlg TinyInt,
	UsedFlg TinyInt,
	InsertOperartor varchar(10),
	InsertDateTime datetime
	--,
	--UpdateOperator varchar(10),
    --UpdateDateTime datetime
	)
	EXEC sp_xml_removedocument @DocHandle; 
	Declare
	@InsertDateTime as datetime=getdate();
	If @Mode=1
	Begin
	insert into M_SKUPrice (TanKaCD, StoreCD, AdminNO, SKUCD,ChangeDate, TekiyouShuuryouDate, PriceWithTax,PriceWithoutTax,GeneralRate,GeneralPriceWithTax,
	GeneralPriceOutTax,MemberRate,MemberPriceWithTax,MemberPriceOutTax,ClientRate,ClientPriceWithTax,ClientPriceOutTax,SaleRate,SalePriceWithTax,SalePriceOutTax,
	WebRate,WebPriceWithTax,WebPriceOutTax,Remarks,DeleteFlg,UsedFlg,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	select
	tp.TanKaCD,
	tp.StoreCD,
	tp.AdminNO,
	tp.SKUCD,
	tp.StartChangeDate,
	tp.EndChangeDate,
	'0',
	tp.PriceWithoutTax,
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	'0',
	tp.SalePriceOutTax,
	'0',
	'0',
	'0',
	tp.Remarks,
	'0',
	'0',
	tp.InsertOperartor,
	tp.InsertDateTime,
    tp.InsertOperartor,
	tp.InsertDateTime
	--tp.UpdateOperator,
	--tp.UpdateDateTime
	from #Temp as tp
	End

	Else  if @Mode = 2
	Begin
	--Update M_SKUPrice
	--Set
	--TankaCD=@TankaCD,
	--ChangeDate=@StartDate,
	--TekiyouShuuryouDate=@EndDate,
	--PriceWithoutTax=@PriceWithoutTax,
	--SalePriceOutTax=@SalePriceOutTax,
 --   Remarks=@Remarks

	--where TankaCD=@TankaCD

	 
update msk  set 
	msk.TankaCD= tp.TankaCD,
	msk.ChangeDate= tp.StartChangeDate,
	msk.TekiyouShuuryouDate =  tp.EndChangeDate,
	msk.PriceWithoutTax = tp.PriceWithoutTax,
	msk.SalePriceOutTax=tp.SalePriceOutTax,
	msk.Remarks = tp.Remarks,
	msk.UpdateOperator=@Operator,
	msk.UpdateDateTime=@InsertDateTime
	from M_SKUPrice msk
	inner join #Temp tp 
	on 
	msk.TankaCD = tp.TankaCD and
	msk.AdminNo = tp.AdminNo and
	--msk.StoreCD= tp.StoreCD and
	msk.ChangeDate= tp.ChangeDate  
	where msk.StoreCD='0000'

	
	End
	Else if @Mode = 3
	Begin
	        
       select * from #Temp
	DELETE FROM [M_SKUPrice]
         WHERE EXISTS(SELECT 1 FROM #Temp tbl
            WHERE 
			tbl.SKUCD = M_SKUPrice.SKUCD
         	and tbl.AdminNO=  M_SKUPrice.AdminNO
            AND tbl.StartChangeDate = M_SKUPrice.ChangeDate)
           -- AND M_SKUPrice.TankaCD =@TankaCD
            AND  M_SKUPrice.StoreCD ='0000'
	End
	drop table #Temp
END
	
	--end


	--select * from M_SKUPrice
  --exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem


GO

