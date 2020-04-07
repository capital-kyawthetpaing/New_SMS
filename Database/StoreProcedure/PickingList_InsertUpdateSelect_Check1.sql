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
	@ShippingPlanDateFrom date,
	@ShippingPlanDateTo date,
	@ShippingDate date,
	@Operator varchar(10)
	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	declare @PickingNO as varchar(11),
	@DateTime datetime=getdate(),@shipDate varchar(10)=CONVERT(VARCHAR(10), @ShippingPlanDateTo, 111)
	
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


		--for sheet テーブル転送仕様B
		Declare @rows int=0

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

		select 
		@PickingNO,
		row_number() over (order by (select NULL)),
		ReserveNO,
		ShippingPlanDate,
		PickingDate,
		PickingDoneDateTime,
		StaffCD,
		ReturnListDateTime,
		@Operator,
		@DateTime,
		@Operator,
		@DateTime,
		null,
		null
		from 
		((select 
		dr.ReserveNO,
		dr.ShippingPlanDate,
		null as 'PickingDate',
		null as 'PickingDoneDateTime',
		null as 'StaffCD',
		null as 'ReturnListDateTime'
		
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.StockNO and ds.DeleteDateTime is null
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
		null as 'ReturnListDateTime'
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.StockNO
									 and ds.DeleteDateTime is null
		inner join D_JuchuuDetails as djc on djc.JuchuuNO=dr.Number 
											and djc.JuchuuRows=dr.NumberRows
											and djc.UpdateCancelKBN <> 9
											and djc.DeleteDateTime is null
		where dr.DeleteDateTime is null
		and dr.SoukoCD=@SoukoCD
		and (@ShippingPlanDateFrom is null or dr.ShippingPlanDate>= @ShippingPlanDateFrom)
		and (@ShippingPlanDateTo is null or dr.ShippingPlanDate<= @ShippingPlanDateTo)
		and dr.ReserveSu > 0
		and dr.ShippingPossibleSu > 0
		and dr.ReserveKBN = 2
		and dr.PickingListDateTime is null
		))bb


		--For sheet テーブル転送仕様C
		set @rows=0
		
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
		row_number() over (order by (select NULL)),
		dr.ReserveNO,
		dr.ShippingPlanDate,
		NULL,
		NULL,
		NULL,
		NULL,
		@Operator,
		@DateTime,
		@Operator,
		@DateTime,
		NULL,
		NULL
		from D_Stock as ds 
		inner join D_Reserve as dr on ds.StockNO=dr.ReserveNO
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


		--For sheet テーブル転送仕様E
		Update D_Reserve
		set PickingListDateTime=@DateTime
		where SoukoCD=@SoukoCD
		and (@ShippingPlanDateFrom is null or ShippingPlanDate>= @ShippingPlanDateFrom)
		and (@ShippingPlanDateTo is null or ShippingPlanDate<= @ShippingPlanDateTo)
		and ReserveSu > 0
		and ShippingPossibleSu > 0
		and PickingListDateTime is null

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
			Replace(CONVERT(char(10), ds.ShippingPlanDate,126),'-','/') as 'ShippingPlanDate',
			mb.BrandName,
			Isnull(fst1.TagName,'') +' ' +Isnull(fst2.TagName,'')+' '+Isnull(fst3.TagName,'') as 'TagName',
			ddp.DeliveryName
			from D_PickingDetails as dpd
			left outer join D_Picking as dp on dpd.PickingNO=dp.PickingNO and dp.DeleteDateTime is null
			left outer join D_Reserve as ds on ds.ReserveNO=dpd.ReserveNO and ds.DeleteDateTime is null
			left outer join D_Stock as dst on dst.StockNO=ds.StockNO and ds.DeleteDateTime is null
			left outer join D_DeliveryPlan as ddp on ddp.Number=ds.Number
			left outer join F_SKU(@DateTime) as fs on fs.AdminNO=ds.AdminNO and fs.DeleteFlg=0
			left outer join M_Brand as mb on mb.BrandCD=fs.BrandCD
			left outer join F_ITEM(@DateTime) as fi on fi.ITemCD=fs.ITemCD and fi.DeleteFlg=0
			left outer join F_SKUTag(@DateTime,1) as fst1 on fst1.AdminNO=ds.AdminNO
			left outer join F_SKUTag(@DateTime,2) as fst2 on fst2.AdminNO=ds.AdminNO
			left outer join F_SKUTag(@DateTime,3) as fst3 on fst3.AdminNO=ds.AdminNO
			left outer join M_MultiPorpose as mmp on mmp.[Key]=fs.TaniCD and mmp.ID='201'
			where dpd.DeleteDateTime is null
			and dpd.PickingNO=@PickingNO


END

