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
			if @display=0
		begin
		select 
				1 as 'TempKey',
				0 as 'CheckBox',
					
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
					 ti.MakerItem,
					 ItemCD,
					 (
						Select    ItemName
						from F_Item(@date) as mi
						where mi.ITemCD=ti.ITemCD
						and mi.ChangeDate <= ti.ChangeDate
					 ) as ItemName,
					 mp.ChangeDate,
					 ti.PriceOutTax,
					 mp.Rate,
					 mp.PriceWithoutTax,
					mp.InsertOperator,
					mp.InsertDateTime,
					mp.UpdateOperator,
					mp.UpdateDateTime
		--into #Tmp_item
		from M_ItemOrderPrice as mp
		left outer join F_ITEM(@date) as ti on mp.MakerItem=ti.MakerItem
		where mp.VendorCD=@vendorcd
		and mp.StoreCD=@storecd
		order by mp.VendorCD,
				mp.StoreCD,
				ti.BrandCD,
				ti.SportsCD,
				ti.SegmentCD,
				ti.LastYearTerm,
				ti.LastSeason,
				mp.MakerItem,
				ti.ITEMCD,
				mp.ChangeDate 
		end

		if @display=1
		begin
		select 
					1 as 'TempKey',
					0 as 'CheckBox',
					MakerItem,
					 ItemCD,
					 (
						Select    ItemName
						from F_Item(@date) as mi
						where mi.ITemCD=msku.ITemCD
						and mi.ChangeDate <= msku.ChangeDate
					 ) as ItemName,
					 SizeName,
					 ColorName,
					msku.SKUCD,

					 msku.ChangeDate,
					 PriceOutTax,
					 msku.Rate,
					 mj.PriceWithoutTax
				from M_JANOrderPrice as mj
				left outer join F_SKU(@date)as msku on msku.AdminNO=mj.AdminNO
				WHERE	
							
							 	mj.VendorCD=@vendorcd
							and		mj.StoreCD=@storecd
							--and	(@brandcd is Null or	msku.BrandCD=@brandcd)
							--and	(@sportcd is Null or	msku.SportsCD=@sportcd)
							--and	(@segmentcd is Null or	msku.SegmentCD=@segmentcd)
							--and	(@lastyearterm is Null or	msku.LastYearTerm=@lastyearterm)
							--and	(@lastseason is Null or	msku.LastSeason =@lastseason)
							--and	(@makeritem is Null or	msku.MakerItem=@makeritem)
							--and	 (@changedate is null  Or msku.ChangeDate=@changedate)   
							
							--and
							--case when @display=0  
							--then msku.ChangeDate 
							--end
							--<=
							--case when @heardate is not null
							--then @heardate
							--end
							
							--or
							
							--case when @display=1
							--then ti.ChangeDate 
							--end
							--=
							--case when @heardate is not null
							--then @heardate
							--end

							order by mj.VendorCD,
									 mj.StoreCD,
									 msku.BrandCD,
									 msku.SportsCD,
									 msku.SegmentCD,
									 msku.LastYearTerm,
									 msku.LastSeason,
									 msku.MakerItem,
									 msku.ITemCD,
									 msku.ChangeDate

		end



				-- select 
				--		MakerItem,
				--		 ItemCD,
				--		 (
				--			Select    ItemName
				--			from F_Item(@date) as mi
				--			where mi.ITemCD=ts.ITemCD
				--			and mi.ChangeDate <= ts.ChangeDate
				--		 ) as ItemName,
				--		 SizeName,
				--		 ColorName,
				--		 SKUCD,

				--		 ChangeDate,
				--		 PriceOutTax,
				--		 Rate,
				--		 PriceWithoutTax,
				--		 Rate,
				--		 TempKey
				--from	#Tmp_sku as ts
				
				--WHERE	
				--			VendorCD=@vendorcd								
				--			and	ts.StoreCD=@storecd
				--			and	(@brandcd is Null or	ts.BrandCD=@brandcd)
				--			and	(@sportcd is Null or	ts.SportsCD=@sportcd)
				--			and	(@segmentcd is Null or	ts.SegmentCD=@segmentcd)
				--			and	(@lastyearterm is Null or	ts.LastYearTerm=@lastyearterm)
				--			and	(@lastseason is Null or	ts.LastSeason =@lastseason)
				--			and	(@makeritem is Null or	ts.MakerItem=@makeritem)
				--			and
				--			case when @display=0  
				--			then ts.ChangeDate 
				--			end
				--			<=
				--			case when @heardate is not null
				--			then @heardate
				--			end
							
				--			or
							
				--			case when @display=1
				--			then ts.ChangeDate 
				--			end
				--			=
				--			case when @heardate is not null
				--			then @heardate
				--			end 
				--			order by ts.VendorCD,
				--					 ts.BrandCD,
				--					 ts.SportsCD,
				--					 ts.SegmentCD,
				--					 ts.LastYearTerm,
				--					 ts.LastSeason,
				--					 ts.MakerItem,
				--					 ts.ITemCD,
				--					 ts.ChangeDate

				

				--insert into  #Tmp_item
				--	 values(6,0,@vendorcd,@storecd,@itemcd,@makerItem,@brandcd,@sportcd,@segmentcd
				--	 ,@lastyearterm,@lastseason,@changedate,@rate,@priceouttax,@priceoutwithouttax,
				--	 @insertoperator,getdate(),@insertoperator,getdate())
				--	 select * from #Tmp_item;
				--drop table #Tmp_item
				--drop table #Tmp_sku

END
