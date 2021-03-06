 BEGIN TRY 
 Drop Procedure dbo.[PickingList_InsertUpdateSelect_Check3]
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
CREATE PROCEDURE [dbo].[PickingList_InsertUpdateSelect_Check3](
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@StoreCD varchar(6),
	@ShippingPlanDateFrom date,
	@ShippingPlanDateTo date,
	@Operator varchar(10)

	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	
	BEGIN TRY
	BEGIN TRAN

	Declare
	@DateTime datetime=getdate(),
	@countD int=0

	/* Data select For sheet D*/
	Select 
		--row_number() over (order by (select NULL)) as 'RowNo',
		dpd.ReserveNO,
		dpd.ShippingPlanDate,
		NULL as 'PickingDate',
		NULL as 'PickingDoneDateTime',
		NULL as 'StaffCD',
		@Operator as 'InsertOperator',
		@DateTime as 'InsertDateTime',
		@Operator as 'UpdateOperator',
		@DateTime as 'UpdateDateTime',
		NULL as 'DeleteOperator',
		NULL as 'DeleteDateTime',
		ds.RackNO,dr.JanCD
		Into #tempD
		from D_PickingDetails as dpd
		inner join D_Picking as dp on dp.PickingNO=dpd.PickingNO
									and dpd.DeleteDateTime is null
									and dpd.ReturnListDatetime is null
									and dpd.PickingDoneDateTime is not null
									and (@ShippingPlanDateFrom is null or dpd.ShippingPlanDate>= @ShippingPlanDateFrom)
									and (@ShippingPlanDateTo is null or dpd.ShippingPlanDate<= @ShippingPlanDateTo)
		inner join D_Reserve as dr on dr.ReserveNO=dpd.ReserveNO
									and dr.DeleteDateTime is null
									and dr.ReserveKBN=1
									and dr.SoukoCD=@SoukoCD
									and dr.ReserveSu>0
									and dr.ShippingPossibleSu>0
									and (@ShippingPlanDateFrom is null or dr.ShippingPlanDate>= @ShippingPlanDateFrom)
									and (@ShippingPlanDateTo is null or dr.ShippingPlanDate<= @ShippingPlanDateTo)
		inner join D_JuchuuDetails as djd on djd.JuchuuNO=dr.Number
											and djd.JuchuuRows=dr.NumberRows
											and djd.UpdateCancelKBN=9
		left outer join D_Stock as ds on ds.StockNO=dr.StockNO
									and ds.DeleteDateTime is null
									and ds.SoukoCD=@SoukoCD
									and ds.RackNO is not null
		where dp.DeleteDateTime is null
		and dp.SoukoCD=@SoukoCD
		and dp.PickingKBN=1
		Order by ds.RackNO,dr.JanCD

	select @countD=count(*) from #tempD

	declare @PickingNO as varchar(11),
	@shipDate varchar(10)=CONVERT(VARCHAR(10), @ShippingPlanDateTo, 111)
	
		IF @countD > 0
		Begin
			EXEC Fnc_GetNumber
				26,-------------in伝票種別 
				@shipDate,----in基準日
				@StoreCD,
				@Operator,
				@PickingNO OUTPUT
				;

			 IF ISNULL(@PickingNO,'') = ''
				BEGIN
					RETURN '1';
				END

			--For sheet テーブル転送仕様Ａ
			Insert into D_Picking(
			PickingNO 
			,PickingKBN 
			,SoukoCD 
			,PrintDateTime 
			,PrintStaffCD 
			,InsertOperator 
			,InsertDateTime 
			,UpdateOperator
			,UpdateDateTime 
			,DeleteOperator
			,DeleteDateTime)
			values(@PickingNO,
			2,
			@SoukoCD,
			NULL,
			NULL,
			@Operator,
			@DateTime,
			@Operator,
			@DateTime,
			NULL,
			NULL)

		End

		--For sheet テーブル転送仕様D

		If @countD>0
			Insert into D_PickingDetails(
			 PickingNO
			,PickingRows
			,ReserveNO
			,ShippingPlanDate
			,PickingDate
			,PickingDoneDateTime
			,StaffCD
			,InsertOperator
			,InsertDateTime
			,UpdateOperator
			,UpdateDateTime
			,DeleteOperator
			,DeleteDateTime
			)
			select 
			@PickingNO
			,row_number() over (order by RackNO,JanCD) as 'RowNo'
			,ReserveNO
			,ShippingPlanDate
			,convert(varchar,PickingDate,10)
			,PickingDoneDateTime
			,StaffCD
			,InsertOperator
			,InsertDateTime
			,UpdateOperator
			,UpdateDateTime
			,DeleteOperator
			,DeleteDateTime
			from #tempD
			order by RackNO,JanCD



		--For sheet テーブル転送仕様F
		Update D_PickingDetails
		set ReturnListDatetime=@DateTime
		where DeleteDateTime is null
		and ReturnListDatetime is null
		and PickingDoneDateTime is not null
		and (@ShippingPlanDateFrom is null or ShippingPlanDate>= @ShippingPlanDateFrom)
		and (@ShippingPlanDateTo is null or ShippingPlanDate<= @ShippingPlanDateTo)


		--Select Data for report
		select 
			dpd.PickingNO,
			dst.RackNO,
			ds.JanCD,
			fs.SKUName,
			convert(varchar(30),ds.ShippingPossibleSu) +' ' +mmp.Char1 as 'ShippingPossibleSu',
			mmp.Char1,
			ds.Number,
			ds.SKUCD,
			fs.ColorName,
			fs.SizeName,
			Replace(CONVERT(char(10), ds.ShippingPlanDate,23),'-','/') as 'ShippingPlanDate',
			mb.BrandName,
			Isnull(fst1.TagName,'') +' ' +Isnull(fst2.TagName,'')+' '+Isnull(fst3.TagName,'') as 'TagName',
			case when ddp.DeliveryKBN<>2 then ddp.DeliveryName
					when ddp.DeliveryKBN=2 then fsouko.SoukoName
			End as 'DeliveryName'
			from D_PickingDetails as dpd
			left outer join D_Picking as dp on dpd.PickingNO=dp.PickingNO 
											and dp.DeleteDateTime is null
			left outer join D_Reserve as ds on ds.ReserveNO=dpd.ReserveNO 
											and ds.DeleteDateTime is null
			left outer join D_Stock as dst on dst.StockNO=ds.StockNO 
											and ds.DeleteDateTime is null
			left outer join D_DeliveryPlan as ddp on ddp.Number=ds.Number
			left outer join F_Souko(@DateTime) as fsk on fsk.SoukoCD=ds.SoukoCD 
														and fsk.DeleteFlg=0
														and fsk.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKU(@DateTime) as fs on fs.AdminNO=ds.AdminNO 
													and fs.DeleteFlg=0
													and fs.ChangeDate<= ds.ShippingPlanDate
			left outer join M_Brand as mb on mb.BrandCD=fs.BrandCD
			left outer join F_ITEM(@DateTime) as fi on fi.ITemCD=fs.ITemCD 
													and fi.DeleteFlg=0
													and fi.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,1) as fst1 on fst1.AdminNO=ds.AdminNO
															and fst1.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,2) as fst2 on fst2.AdminNO=ds.AdminNO
															and fst2.ChangeDate<= ds.ShippingPlanDate
			left outer join F_SKUTag(@DateTime,3) as fst3 on fst3.AdminNO=ds.AdminNO
															and fst3.ChangeDate<= ds.ShippingPlanDate
			left outer join M_MultiPorpose as mmp on mmp.[Key]=fs.TaniCD 
												and mmp.ID='201'
			left outer join F_Souko(@DateTime) as fsouko on fsouko.SoukoCD=ddp.DeliverySoukoCD
														and fsouko.DeleteFlg=0
														and fsouko.ChangeDate<=ddp.DeliveryPlanDate
			where dpd.DeleteDateTime is null
			and dpd.PickingNO=@PickingNO 
			order by dpd.PickingNO,dpd.PickingRows


		if @countD >0
			drop table #tempD

	COMMIT TRAN
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN;
	END CATCH
END

