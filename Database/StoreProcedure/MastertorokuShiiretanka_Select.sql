BEGIN TRY 
 Drop Procedure [dbo].[MastertorokuShiiretanka_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




Create PROCEDURE [dbo].[MastertorokuShiiretanka_Select]
	-- Add the parameters for the stored procedure here
	@vendorcd as varchar(13),
	@storecd as varchar(4),
	@changedate as date,
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
					fio.VendorCD,
					fio.StoreCD,
					fio.MakerItem,
					fi.BrandCD,
					 (
						Select BrandName   
						from M_Brand as mb
						where mb.BrandCD=fi.BrandCD
					 ) as BrandName,
					 fi.SportsCD,
					 (
						Select Char1   
						from M_Multiporpose as mp
						where mp.ID=202
						and mp.[Key]=fi.SportsCD
					 ) as Char1,
					 fi.SegmentCD,
					 (
						Select Char1   
						from M_Multiporpose as mp
						where mp.ID=203
						and mp.[Key]=fi.SegmentCD
					 ) as SegmentCDName,
					 fi.LastYearTerm,
					 fi.LastSeason,
					 fi.ITemCD,
					 (
						Select    mi.ITemName
						from F_Item(@date) as mi
						where mi.ITemCD=fi.ITemCD
						--and fi.ChangeDate <= fi.ChangeDate
					 ) as ITemName,
					CONVERT(VARCHAR(10),fio.ChangeDate  , 111) as ChangeDate,
					IsNUll(fi.PriceOutTax,0) as PriceOutTax,
					IsNUll(fio.Rate,0) as Rate,
					fio.PriceWithoutTax,
					fio.InsertOperator,
					fio.InsertDateTime,
					fio.UpdateOperator,
					fio.UpdateDateTime
			
	from F_ItemOrderPrice(@date)  as fio
	left outer join F_Item(@date) as fi on fi.MakerItem =fio.MakerITem   and SetKBN=0 and ItemCD not like '%-k'
	where fio.VendorCD=@vendorCD
	and		fio.StoreCD=@storecd
	--and fio.ChangeDate <= @changedate
	
	 order by fio.VendorCD,
				fio.StoreCD,
				fi.BrandCD,
				fi.SportsCD,
				fi.SegmentCD,
				fi.LastYearTerm,
				fi.LastSeason,
				fio.MakerItem,
				fi.ITemCD,
				fio.ChangeDate
			
		end

		if @display=1
		begin
			select 
					1 as 'TempKey',
					0 as 'CheckBox',
					mj.VendorCD,
					mj.StoreCD
					,
					mj.AdminNO,
					mj.SKUCD,
					msku.MakerItem,
					 msku.ITemCD,
					 (
						Select    mi.ITemName
						from F_Item(@date) as mi
						where mi.ITemCD=msku.ITemCD
						--and mi.ChangeDate <= msku.ChangeDate
					 ) as ITemName,
					msku.SizeName,
					msku. ColorName,
					msku.BrandCD,
					msku.SportsCD,
					msku.SegmentCD,
					msku.LastYearTerm,
					msku.LastSeason,
					CONVERT(VARCHAR(10),mj.ChangeDate, 111) as ChangeDate,
					IsNULl(msku.PriceOutTax,0) as PriceOutTax,
					IsNULl(	mj.Rate,0) as Rate,
					mj.PriceWithoutTax,
					mj.InsertOperator,
					mj.InsertDateTime,
					mj.UpdateOperator,
					mj.UpdateDateTime
				from M_JANOrderPrice as mj 
				left outer join F_SKU(@date)as msku on msku.AdminNO=mj.AdminNO  and SetKBN=0 and ItemCD not like '%-k'
				WHERE	
							
							 	mj.VendorCD=@vendorcd
							and		mj.StoreCD=@storecd
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

			

END