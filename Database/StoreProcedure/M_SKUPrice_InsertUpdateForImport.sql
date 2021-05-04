BEGIN TRY 
 Drop Procedure dbo.M_SKUPrice_InsertUpdateForImport
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
Create PROCEDURE M_SKUPrice_InsertUpdateForImport
	-- Add the parameters for the stored procedure here
	@xml as Xml,
	@OperatorCD as varchar(10),
	@ProgramID as varchar(100),
	@PC as varchar(30),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

	Create table [#tmpSKUPrice](
				TankaCD [varchar](13) NOT NULL,
				StoreCD [varchar](4) NOT NULL,
				AdminNO [int] NOT NULL,
				SKUCD  [varchar](40) NULL,
				ChangeDate  [date] NOT NULL,
				TekiyouShuuryouDate  [date] NULL,
				PriceWithTax money,
				PriceWithoutTax money NULL,
				GeneralRate [varchar](10) NULL,
				GeneralPriceWithTax money,
				GeneralPriceOutTax money,
				MemberRate [varchar](10) NULL,
				MemberPriceWithTax money,
				MemberPriceOutTax money,
				ClientRate [varchar](10) NULL,
				ClientPriceWithTax money,
				ClientPriceOutTax money,
				SaleRate [varchar](10) NULL,
				SalePriceWithTax money,
				SalePriceOutTax money,
				WebRate [varchar](10) NULL,
				WebPriceWithTax money,
				WebPriceOutTax money,
				Remarks  [varchar](500) NULL,
				DeleteFlg [int]
				
				)

	declare @DocHandle1 int
	exec sp_xml_preparedocument @DocHandle1 output, @xml
				INSERT Into [#tmpSKUPrice]
				select *  FROM OPENXML (@DocHandle1, '/NewDataSet/test',2)
						with
						(
				�P���ݒ�CD varchar(13),
				�X��CD varchar(4),
				AdminNO int,
				SKUCD varchar(40),
				���i�� varchar(100),
				����� varchar(10),
				�K�p�I����  varchar(10),
				�ō��艿  varchar(10),
				�Ŕ��艿  varchar(10),
				��ʊ|��  varchar(10),
				�ō���ʒP�� money,
				�Ŕ���ʒP�� money,
				����|�� varchar(10),
				�ō�����P�� money,
				�Ŕ�����P�� money,
				�O���|�� varchar(10),
				�ō��O���P��  money,
				�Ŕ��O���P�� money,
				Sale�|��  varchar(10),
				�ō�Sale�P��  money,
				�Ŕ�Sale�P�� money,
				Web�|�� varchar(10),
				�ō�Web�P�� money,
				�Ŕ�Web�P�� money,
				���l varchar(500),
				�폜FLG tinyInt)

				exec sp_xml_removedocument @DocHandle1;


				Insert Into M_SKUPrice(
					TankaCD
					,StoreCD
					,AdminNO
					,SKUCD
					,ChangeDate
					,TekiyouShuuryouDate
					,PriceWithTax
					,PriceWithoutTax
					,GeneralRate
					,GeneralPriceWithTax
					,GeneralPriceOutTax
					,MemberRate
					,MemberPriceWithTax
					,MemberPriceOutTax
					,ClientRate
					,ClientPriceWithTax
					,ClientPriceOutTax
					,SaleRate
					,SalePriceWithTax
					,SalePriceOutTax
					,WebRate
					,WebPriceWithTax
					,WebPriceOutTax
					,Remarks
					,DeleteFlg
					,UsedFlg
					,InsertOperator
					,InsertDateTime
					)
					select 
					ts.TankaCD
					,ts.StoreCD
					,ts.AdminNO
					,ts.SKUCD
					,ts.ChangeDate
					,ts.TekiyouShuuryouDate
					,ts.PriceWithTax
					,ts.PriceWithoutTax
					,ts.GeneralRate
					,ts.GeneralPriceWithTax
					,ts.GeneralPriceOutTax
					,ts.MemberRate
					,ts.MemberPriceWithTax
					,ts.MemberPriceOutTax
					,ts.ClientRate
					,ts.ClientPriceWithTax
					,ts.ClientPriceOutTax
					,ts.SaleRate
					,ts.SalePriceWithTax
					,ts.SalePriceOutTax
					,ts.WebRate
					,ts.WebPriceWithTax
					,ts.WebPriceOutTax
					,ts.Remarks
					,ts.DeleteFlg
					,0
					,@OperatorCD
					,GETDATE()

					from #tmpSKUPrice as ts
						where not Exists(
											Select ms.AdminNO
											from M_SKUPrice as ms
											where ms.TankaCD=ts.TankaCD
											and ms.StoreCD=ts.StoreCD
											and ms.AdminNO=ts.AdminNO
											and ms.ChangeDate=ts.ChangeDate
										)
			Update ms
			set TekiyouShuuryouDate=ts.TekiyouShuuryouDate
					,PriceWithTax=ts.PriceWithTax
					,PriceWithoutTax=ts.PriceWithoutTax
					,GeneralRate=ts.GeneralRate
					,GeneralPriceWithTax=ts.GeneralPriceWithTax
					,GeneralPriceOutTax=ts.GeneralPriceOutTax
					,MemberRate=ts.MemberRate
					,MemberPriceWithTax=ts.MemberPriceWithTax
					,MemberPriceOutTax=ts.MemberPriceOutTax
					,ClientRate=ts.ClientRate
					,ClientPriceWithTax=ts.ClientPriceWithTax
					,ClientPriceOutTax=ts.ClientPriceOutTax
					,SaleRate=ts.SaleRate
					,SalePriceWithTax=ts.SalePriceWithTax
					,SalePriceOutTax=ts.SalePriceOutTax
					,WebRate=ts.WebRate
					,WebPriceWithTax=ts.WebPriceWithTax
					,WebPriceOutTax=ts.WebPriceOutTax
					,Remarks=ts.Remarks
					,DeleteFlg=ts.DeleteFlg
					,UpdateOperator=@OperatorCD
					,UpdateDateTime=GETDATE()
			from #tmpSKUPrice as ts inner join M_SKUPrice as ms
			on ts.TankaCD=ms.TankaCD
			and ts.StoreCD=ms.StoreCD
			and ts.AdminNO=ms.AdminNO
			and ts.ChangeDate=ms.ChangeDate

			 exec dbo.L_Log_Insert @OperatorCD,@ProgramID,@PC,Null,@KeyItem

			 Drop table #tmpSKUPrice

END
GO
