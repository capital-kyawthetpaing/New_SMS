 BEGIN TRY 
 Drop Procedure dbo.[PickingList_InsertUpdateSelect_Check1]
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
CREATE PROCEDURE [dbo].[PickingList_InsertUpdateSelect_Check1](
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@StoreCD varchar(6),
	@ShippingPlanDateFrom varchar(10),
	@ShippingPlanDateTo varchar(10),
	@ShippingDate varchar(10),
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

	Declare @DateTime datetime=getdate(),
	@countB int=0,@countC int=0

			/* Data select For sheet B*/
		select 
		--row_number() over (order by (select NULL)) as 'RowNO',
		ReserveNO,
		ShippingPlanDate,
		PickingDate,
		PickingDoneDateTime,
		StaffCD,
		ReturnListDateTime,
		@Operator as 'InsertOperator',
		@DateTime as 'InsertDateTime',
		@Operator as 'UpdateOperator',
		@DateTime as 'UpdateDateTime',
		null as 'DeleteOperator' ,
		null as 'DeleteDateTime',
		RackNO,
		JanCD
		Into #tempB
		from 
		((select 
		dr.ReserveNO,
		dr.ShippingPlanDate,
		null as 'PickingDate',
		null as 'PickingDoneDateTime',
		null as 'StaffCD',
		null as 'ReturnListDateTime',
		ds.RackNO,
		dr.JanCD
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.StockNO 
										and ds.DeleteDateTime is null
										 and ds.RackNO is not null
		inner join D_JuchuuDetails as djc on djc.JuchuuNO=dr.Number 
											and djc.JuchuuRows=dr.NumberRows
											and djc.UpdateCancelKBN <> 9
											and djc.DeleteDateTime is null
		where dr.DeleteDateTime is null
		and dr.SoukoCD=@SoukoCD
		and (@ShippingPlanDateFrom is null or dr.ShippingPlanDate>= @ShippingPlanDateFrom)
		and (@ShippingPlanDateTo is null or dr.ShippingPlanDate<= @ShippingPlanDateTo)
		and dr.ReserveSu>0
		and dr.ShippingPossibleSu>0
		and dr.ReserveKBN=1
		and dr.PickingListDateTime is null
		)
		
		Union All

		(select 
		dr.ReserveNO,
		dr.ShippingPlanDate,
		null as 'PickingDate',
		null as 'PickingDoneDateTime',
		null as 'StaffCD',
		null as 'ReturnListDateTime',
		ds.RackNO,
		dr.JanCD
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.StockNO
									 and ds.DeleteDateTime is null
									 and ds.RackNO is not null
		where dr.DeleteDateTime is null
		and dr.SoukoCD=@SoukoCD
		and (@ShippingPlanDateFrom is null or dr.ShippingPlanDate>= @ShippingPlanDateFrom)
		and (@ShippingPlanDateTo is null or dr.ShippingPlanDate<= @ShippingPlanDateTo)
		and dr.ReserveSu > 0
		and dr.ShippingPossibleSu > 0
		and dr.ReserveKBN = 2
		and dr.PickingListDateTime is null
		))bb
		order by RackNO,JanCD

		select @countB=count(*) from #tempB

		/* Data select For sheet C*/
		select 
		--row_number() over (order by ds.RackNO,dr.JanCD) as 'RowNO',
		dr.ReserveNO,
		dr.ShippingPlanDate,
		NULL as 'PickingDate',
		NULL as 'PickingDoneDateTime',
		NULL as 'StaffCD',
		NULL as 'ReturnListDatetime',
		@Operator as 'InsertOperator',
		@DateTime as 'InsertDateTime',
		@Operator as 'UpdateOperator',
		@DateTime as 'UpdateDateTime',
		NULL as 'DeleteOperator',
		NULL as 'DeleteDateTime',
		ds.RackNO,
		dr.JanCD
		Into #tempC
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.StockNO
									 and ds.DeleteDateTime is null
		where dr.DeleteDateTime is null
		and dr.SoukoCD=@SoukoCD
		and dr.ReserveSu > 0
		and dr.ShippingPossibleSu > 0
		and dr.PickingListDateTime is null
		and dr.Number=(select Number
						from D_Reserve 
						where DeleteDateTime is null
						and ds.SoukoCD=@SoukoCD
						and PickingListDateTime is null
						Group by Number
						Having Max(ShippingPlanDate) =@ShippingDate
							and Min(ShippingPlanDate) = @ShippingDate)
		order by ds.RackNO,dr.JanCD

	select @countC=count(*) from #tempC

	declare @PickingNO as varchar(11),
	@shipDate varchar(10)
	if @ShippingPlanDateTo IS NOT NULL 
		set @shipDate=CONVERT(VARCHAR(10), @ShippingPlanDateTo, 111)
	if @ShippingPlanDateTo IS NULL AND @ShippingDate IS NOT NULL 
		set @shipDate=CONVERT(VARCHAR(10), @ShippingDate, 111)
	
	if @countB>0 OR @countC>0
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
			1,
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


		--for sheet テーブル転送仕様B
		if @countB>0
			insert into D_PickingDetails(
			 PickingNO
			,PickingRows
			,ReserveNO
			,ShippingPlanDate
			,PickingDate
			,PickingDoneDateTime
			,StaffCD
			,ReturnListDatetime
			,InsertOperator
			,InsertDateTime
			,UpdateOperator
			,UpdateDateTime
			,DeleteOperator
			,DeleteDateTime
			)
			Select  
			@PickingNO
			,row_number() over (order by RackNO,JanCD) as 'RowNO'
			,ReserveNO
			,ShippingPlanDate
			,convert(varchar,PickingDate,10)
			,PickingDoneDateTime
			,StaffCD
			,ReturnListDatetime
			,InsertOperator
			,InsertDateTime
			,UpdateOperator
			,UpdateDateTime
			,DeleteOperator
			,DeleteDateTime
			from #tempB
			order by RackNO,JanCD asc


		--For sheet テーブル転送仕様C
		if @countC>0
			insert into D_PickingDetails(
			PickingNO,
			PickingRows,
			ReserveNO,
			ShippingPlanDate,
			PickingDate,
			PickingDoneDateTime,
			StaffCD,
			ReturnListDatetime,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime,
			DeleteOperator,
			DeleteDateTime
			)
			select 
			@PickingNO,
			row_number() over (order by RackNO,JanCD) as 'RowNO',
			ReserveNO,
			ShippingPlanDate,
			convert(varchar,PickingDate,10),
			PickingDoneDateTime,
			StaffCD,
			ReturnListDatetime,
			InsertOperator,
			InsertDateTime,
			UpdateOperator,
			UpdateDateTime,
			DeleteOperator,
			DeleteDateTime
			from #tempC
			order by RackNO,JanCD asc


		--For sheet テーブル転送仕様E
			if @countB>0 
				Update D_Reserve
			set PickingListDateTime=@DateTime,
			UpdateOperator=@Operator,
			UpdateDateTime=@DateTime
			where DeleteDateTime is null
			and SoukoCD=@SoukoCD
			and (@ShippingPlanDateFrom is null or ShippingPlanDate>= @ShippingPlanDateFrom)
			and (@ShippingPlanDateTo is null or ShippingPlanDate<= @ShippingPlanDateTo)
			and ReserveSu > 0
			and ShippingPossibleSu > 0
			and PickingListDateTime is null

			if @countC>0
				Update D_Reserve
			set PickingListDateTime=@DateTime,
			UpdateOperator=@Operator,
			UpdateDateTime=@DateTime
			where DeleteDateTime is null
			and SoukoCD=@SoukoCD
			and ReserveSu > 0
			and ShippingPossibleSu > 0
			and PickingListDateTime is null
			and Number=(select Number
							from D_Reserve 
							where DeleteDateTime is null
							and SoukoCD=@SoukoCD
							and PickingListDateTime is null
							Group by Number
							Having Max(ShippingPlanDate) =@ShippingDate
								and Min(ShippingPlanDate) = @ShippingDate)

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


		if @countB>0
			drop table #tempB
		if @countC>0
			drop table #tempC

	COMMIT TRAN
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN;
	END CATCH
END

