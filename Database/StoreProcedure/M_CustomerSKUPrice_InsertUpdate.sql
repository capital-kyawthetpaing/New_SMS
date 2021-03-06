
 BEGIN TRY 
 Drop Procedure dbo.[M_CustomerSKUPrice_InsertUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[M_CustomerSKUPrice_InsertUpdate]
	-- Add the parameters for the stored procedure here
	@xmlCustSKUPrice as  xml,
	@Operator as varchar(10),
	@Program as varchar(30),
	@Pc as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100),
	@Mode as tinyint-- 1 - insert, 2 - update
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare
	@InsertDateTime as datetime=getdate()

	CREATE TABLE [dbo].[#tempCustomerSKUPrice]
	(
				CustomerCD varchar(13) collate Japanese_CI_AS,
				CustomerName varchar(80) collate Japanese_CI_AS,
				TekiyouKaisiDate varchar(10) collate Japanese_CI_AS,
				TekiyouShuuryouDate varchar(10) collate Japanese_CI_AS,
				AdminNO int,
				JanCD  varchar(13) collate Japanese_CI_AS,
				SKUCD varchar(30) collate Japanese_CI_AS,
				SKUName varchar(100) collate Japanese_CI_AS,
				SalePriceOutTax decimal,
				Remarks varchar(500) collate Japanese_CI_AS

	)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @xmlCustSKUPrice
	insert into #tempCustomerSKUPrice
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				CustomerCD varchar(13) collate Japanese_CI_AS,
				CustomerName varchar(80) collate Japanese_CI_AS,
				TekiyouKaisiDate varchar(10) collate Japanese_CI_AS,
				TekiyouShuuryouDate varchar(10) collate Japanese_CI_AS,
				AdminNO int,
				JanCD  varchar(13) collate Japanese_CI_AS,
				SKUCD varchar(30) collate Japanese_CI_AS,
				SKUName varchar(100) collate Japanese_CI_AS,
				SalePriceOutTax decimal,
				Remarks varchar(500) collate Japanese_CI_AS
			)
			exec sp_xml_removedocument @DocHandle;

		IF @Mode=1
		begin

			Insert into M_CustomerSKUPrice(
						CustomerCD
						,CustomerName
						,TekiyouKaisiDate
						,TekiyouShuuryouDate
						,AdminNO
						,JanCD
						,SKUCD
						,SKUName
						,SalePriceOutTax
						,Remarks
						,DeleteFlg
						,InsertOperator
						,InsertDateTime
						,UpdateOperator
						,UpdateDateTime
						)
			select tempSKU.CustomerCD,
				tempSKU.CustomerName,
				tempSKU.TekiyouKaisiDate ,
				tempSKU.TekiyouShuuryouDate,
				tempSKU.AdminNO			 ,
				tempSKU.JanCD  			 ,
				tempSKU.SKUCD 			 ,
				tempSKU.SKUName			 ,
				tempSKU.SalePriceOutTax  ,
				tempSKU.Remarks 		,
				0,
				@Operator,
				@InsertDateTime ,
				@Operator,
				@InsertDateTime 
			From #tempCustomerSKUPrice as tempSKU
			where NOt Exists(
							select *
							from M_CustomerSKUPrice as mSKU 
							where tempSKU.CustomerCD=mSKU.CustomerCD
							and tempSKU.TekiyouKaisiDate=mSKU.TekiyouKaisiDate
							and tempSKU.AdminNO=mSKU.AdminNO) 

		End

	Else If @Mode=2
	Begin 
		
		Update mSKU
		set mSKU.TekiyouShuuryouDate=tempSKU.TekiyouShuuryouDate,
			mSKU.SalePriceOutTax=tempSKU.SalePriceOutTax,
			mSKU.Remarks=tempSKU.Remarks,
			mSKU.UpdateOperator=@Operator,
			mSKU.UpdateDateTime=@InsertDateTime

		From M_CustomerSKUPrice  as mSKU inner join #tempCustomerSKUPrice as tempSKU on tempSKU.CustomerCD=mSKU.CustomerCD
		and tempSKU.TekiyouKaisiDate=mSKU.TekiyouKaisiDate
		and tempSKU.AdminNO=mSKU.AdminNO

	End
		Else If @Mode=3
	Begin 
		
		Delete mSKU
		From M_CustomerSKUPrice as mSKU inner join #tempCustomerSKUPrice as tempSKU on tempSKU.CustomerCD=mSKU.CustomerCD
		and tempSKU.TekiyouKaisiDate=mSKU.TekiyouKaisiDate
		and tempSKU.AdminNO=mSKU.AdminNO

	End

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem


END
