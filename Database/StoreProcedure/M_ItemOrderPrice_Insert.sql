BEGIN TRY 
 Drop Procedure [dbo].[M_ItemOrderPrice_Insert]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




Create  PROCEDURE [dbo].[M_ItemOrderPrice_Insert]
	-- Add the parameters for the stored procedure here
	@vendorcd as varchar(13),
	@storecd as varchar(4),
	@brandcd as varchar(6),
	@sportcd as varchar(6),
	@segmentcd as varchar(6),
	@lastyearterm as varchar(6),
	@lastseason as varchar(6),
	@changedate as date,
	@makerItem as varchar(30),
	@heardate as date,
	@display as int

	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	declare @date as date=getdate()
			 -- Insert statements for procedure here
			--CREATE TABLE [dbo].[#Tmp_Item](
			--		[VendorCD] [varchar] NULL,
			--		[MakerItem] [varchar] NULL,
			--		[ChangeDate] [date] NULL,
			--		[Rate] [varchar] NULL,
			--		[PriceWithoutTax] [varchar] NULL,
			--		[DeleteFlg] [varchar] NULL,
			--		[UsedFlg] [varchar] NULL,
			--		[InsertOperator] [varchar] NULL,
			--		[InsertDateTime] [varchar] NULL,
			--		[UpdateOperator] [varchar] NULL,
			--		[UpdateDateTime] [varchar] NULL,)

		select 
				1 as 'TempKey',
				0 as CheckBox,
				mp.VendorCD,
				mp.StoreCD,
				mi.ItemCD,
				mp.MakerItem,
				mi.BrandCD,
				mi.SportsCD,
				mi.SegmentCD,
				mi.LastYearTerm,
				mi.LastSeason,
				mp.Changedate,
				mp.Rate,
				mi.PriceOutTax,
				mp.PriceWithoutTax,
				mp.InsertOperator,
				mp.InsertDateTime,
				mp.UpdateOperator,
				mp.UpdateDateTime
		into #Tmp_item
		from M_ItemOrderPrice as mp
		left outer join F_ITEM(@date) as mi on mp.MakerItem=mi.MakerItem
		where mp.VendorCD=@vendorcd
		and mp.StoreCD=@storecd
		order by mp.VendorCD,
				mp.StoreCD,
				mi.BrandCD,
				mi.SportsCD,
				mi.SegmentCD,
				mi.LastYearTerm,
				mi.LastSeason,
				mp.MakerItem,
				mi.ITEMCD,
				mp.ChangeDate

		select 
		1 as 'TempKey',
		0 as CheckBox,
		mj.VendorCD,
		mj.StoreCD,
		msku.ItemCD,
		mj.AdminNO,
		mj.SKUCD,
		msku.SizeName,
		msku.ColorName,
		msku.MakerItem,
		msku.BrandCD,
		msku.SportsCD,
		msku.SegmentCD,
		msku.LastYearTerm,
		msku.LastSeason,
		mj.ChangeDate,
		mj.Rate,
		msku.PriceOutTax,
		mj.PriceWithoutTax,
		mj.InsertOperator,
		mj.InsertDateTime,
		mj.UpdateOperator,
		mj.UpdateDateTime
		into #Tmp_sku
		from M_JANOrderPrice as mj
		left outer join F_SKU(@date)as msku on msku.AdminNO=mj.AdminNO
		where mj.vendorCD=@vendorcd
		and mj.storecd=@storecd
		order by mj.vendorCD,
				mj.StoreCD,
				msku.BrandCD,
				msku.SportsCD,
				msku.SegmentCD,
				msku.LastYearTerm,
				msku.LastSeason,
				msku.MakerItem,
				msku.ItemCD,
				mj.Skucd,
				mj.ChangeDate



					select 
					BrandCD,
					 (
						Select BrandName   
						from M_Brand as mb
						where mb.BrandCD=ti.BrandCD
					 ) as BrandName,
					 SportsCD,
					 (
						Select Char1   
						from M_Multiporpose as mp
						where mp.ID=202
						and mp.[Key]=ti.SportsCD
					 ) as Char1,
					 SegmentCD,
					 (
						Select Char1   
						from M_Multiporpose as mp
						where mp.ID=203
						and mp.[Key]=ti.SegmentCD
					 ) as SegmentCDName,
					 LastYearTerm,
					 LastSeason,
					 MakerItem,
					 ItemCD,
					 (
						Select    ItemName
						from F_Item(@date) as mi
						where mi.ITemCD=ti.ITemCD
						and mi.ChangeDate <= ti.ChangeDate
					 ) as ItemName,
					 ChangeDate,
					 PriceOutTax,
					 Rate,
					 PriceWithoutTax,
					 Rate,
					 TempKey
				from #Tmp_item as ti
			
				WHERE	
							
							 	ti.VendorCD=@vendorcd
							and		ti.StoreCD=@storecd
							and	(@brandcd is Null or	ti.BrandCD=@brandcd)
							and	(@sportcd is Null or	ti.SportsCD=@sportcd)
							and	(@segmentcd is Null or	ti.SegmentCD=@segmentcd)
							and	(@lastyearterm is Null or	ti.LastYearTerm=@lastyearterm)
							and	(@lastseason is Null or	ti.LastSeason =@lastseason)
							and	(@makeritem is Null or	ti.MakerItem=@makeritem)
							and	 (@changedate is null  Or ti.ChangeDate=@changedate)   
							
							and
							case when @display=0  
							then ti.ChangeDate 
							end
							<=
							case when @heardate is not null
							then @heardate
							end
							
							or
							
							case when @display=1
							then ti.ChangeDate 
							end
							=
							case when @heardate is not null
							then @heardate
							end

							order by ti.VendorCD,
									 ti.StoreCD,
									 ti.BrandCD,
									 ti.SportsCD,
									 ti.SegmentCD,
									 ti.LastYearTerm,
									 ti.LastSeason,
									 ti.MakerItem,
									 ti.ITemCD,
									 ti.ChangeDate





				 select 
						MakerItem,
						 ItemCD,
						 (
							Select    ItemName
							from F_Item(@date) as mi
							where mi.ITemCD=ts.ITemCD
							and mi.ChangeDate <= ts.ChangeDate
						 ) as ItemName,
						 SizeName,
						 ColorName,
						 SKUCD,

						 ChangeDate,
						 PriceOutTax,
						 Rate,
						 PriceWithoutTax,
						 Rate,
						 TempKey
				from	#Tmp_sku as ts
				
				WHERE	
							VendorCD=@vendorcd								
							and	ts.StoreCD=@storecd
							and	(@brandcd is Null or	ts.BrandCD=@brandcd)
							and	(@sportcd is Null or	ts.SportsCD=@sportcd)
							and	(@segmentcd is Null or	ts.SegmentCD=@segmentcd)
							and	(@lastyearterm is Null or	ts.LastYearTerm=@lastyearterm)
							and	(@lastseason is Null or	ts.LastSeason =@lastseason)
							and	(@makeritem is Null or	ts.MakerItem=@makeritem)
							and
							case when @display=0  
							then ti.ChangeDate 
							end
							<=
							case when @heardate is not null
							then @heardate
							end
							
							or
							
							case when @display=1
							then ti.ChangeDate 
							end
							=
							case when @heardate is not null
							then @heardate
							end 
							order by ts.VendorCD,
									 ts.BrandCD,
									 ts.SportsCD,
									 ts.SegmentCD,
									 ts.LastYearTerm,
									 ts.LastSeason,
									 ts.MakerItem,
									 ts.ITemCD,
									 ts.ChangeDate



				drop table #Tmp_item
				drop table #Tmp_sku

END
