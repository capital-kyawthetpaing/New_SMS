 BEGIN TRY 
 Drop Procedure dbo.[ZaikoShoukai_Search]
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
Create PROCEDURE [dbo].[ZaikoShoukai_Search]
	-- Add the parameters for the stored procedure here
@ChangeDate as date,
	@VendorCD as varchar(13),
	@makerCD as varchar(13),
	@brandCD as varchar(6),
	@SKUName as varchar(100),
	@JanCD as varchar(13),
	@SkuCD as varchar(30),
	@MakerItem as varchar(50),
	@ItemCD as varchar(30),
	@CommentInStore as varchar(300),
	@ReserveCD as varchar(3),
	@NticesCD as varchar(3),
	@PostageCD as varchar(3),
	@OrderAttentionCD as varchar(3),
	@SportsCD as varchar(6),
	@InsertDateTimeT as datetime,
	@InsertDateTimeF as datetime,
	@UpdateDateTimeT as datetime,
	@UpdateDateTimeF as datetime,
	@ApprovalDateF as date,
	@ApprovalDateT as date,
	@YearTerm as varchar(6),
	@Season as varchar(6),
	@CatalogNo as varchar(20),
	@InstructionsNO as varchar(1000),
	@TagName1 as varchar(20),
	@TagName2 as varchar(20),
	@TagName3 as varchar(20),
	@TagName4 as varchar(20),
	@TagName5 as varchar(20),
	@type as tinyint,
	@chktype as tinyint,

	@SoukoCD as varchar(6),
	@RackNoF as varchar(11),
	@RackNoT as varchar(11),
	@chkUnapprove as tinyint
	--@kijunbi as date =getdate
	--@makercd as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 IF (@SoukoCD = -1)
		SET @SoukoCD = null

	 IF (@YearTerm = -1)
		SET @YearTerm = null

	IF (@Season = -1)
		SET @Season = null

		IF (@ReserveCD = -1)
		SET @ReserveCD = null

	IF (@NticesCD = -1)
		SET @NticesCD = null

	IF (@PostageCD = -1)
		SET @PostageCD = null

	IF (@OrderAttentionCD = -1)
		SET @OrderAttentionCD = null


	CREATE TABLE [dbo].[#Tmp_AdminNo](
	[AdminNO] [int] NULL,)



    -- Insert statements for procedure here
	if @type=1
	begin 
				insert into #Tmp_AdminNo
				select AdminNO from F_SKU(@ChangeDate)  where ITemCD in
				
				(select ITemCD
					From	F_SKU(@ChangeDate) as msku
					Left outer join M_SKUInfo as msInfo on msku.AdminNO=msInfo.AdminNO
														and msku.ChangeDate = msInfo.ChangeDate 
														and msku.DeleteFlg=0
					Left outer join  M_SKUTag as mst on msku.AdminNO=mst.AdminNo
														and msku.ChangeDate =mst.ChangeDate

					where 
					 (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem IN (Select * from SplitString(@MakerItem,','))))
					and (@ItemCD is Null or (msku.ITemCD  IN (Select * from SplitString(@ItemCD,','))))
					and (@CommentInStore is Null or(msku.CommentInStore  Like '%'  +@CommentInStore+ '%' ))
					and (@ReserveCD is Null or ( msku.ReserveCD=@ReserveCD))
					and (@NticesCD is Null or (msku.NoticesCD=@NticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttentionCD is Null or (msku.OrderAttentionCD=@OrderAttentionCD))
					and (@SportsCD is Null or (msku.SportsCD=@SportsCD))
					and (@InsertDateTimeF is Null or (msku.InsertDateTime >= @InsertDateTimeF))
					and (@InsertDateTimeT is Null or (msku.InsertDateTime <= @InsertDateTimeT))
					and (@UpdateDateTimeT is Null or (msku.UpdateDateTime >= @UpdateDateTimeT))
					and (@UpdateDateTimeF is Null or (msku.UpdateDateTime <= @UpdateDateTimeF))
					--and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					--and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and ((((@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
							and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))) ) or 
							(@chkUnapprove = 1 and msku.ApprovalDate is null)
							) 
					and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and  (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@TagName1 is Null or  TagName =@TagName1)
					and (@TagName2 is Null or  TagName =@TagName2)
					and (@TagName3 is Null or  TagName =@TagName3)
					and (@TagName4 is Null or  TagName =@TagName4)
					and (@TagName5 is Null or  TagName =@TagName5))
					--and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					--		)
					--	and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
					--		)
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					--		)
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and (@InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					--		)
						
					--		and Exists
					--		(
					--			select *
					--			from M_SKU as msk
					--			join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
					--			and msk.ChangeDate =mst.ChangeDate
					--			--and( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
					--			--or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
					--			and (@TagName1 is Null or  TagName =@TagName1)
					--			and (@TagName2 is Null or  TagName =@TagName2)
					--			and (@TagName3 is Null or  TagName =@TagName3)
					--			and (@TagName4 is Null or  TagName =@TagName4)
					--			and (@TagName5 is Null or  TagName =@TagName5)
					--		))
		end

	if @type=2
	begin 
				insert into #Tmp_AdminNo
				select AdminNO from F_SKU(@ChangeDate)  where MakerItem in
				
				(select MakerItem
					From	F_SKU(@ChangeDate) as msku
					Left outer join M_SKUInfo as msInfo on msku.AdminNO=msInfo.AdminNO
														and msku.ChangeDate = msInfo.ChangeDate 
														and msku.DeleteFlg=0
					Left outer join  M_SKUTag as mst on msku.AdminNO=mst.AdminNo
														and msku.ChangeDate =mst.ChangeDate

					where (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem IN (Select * from SplitString(@MakerItem,','))))
					and (@ItemCD is Null or (msku.ITemCD  IN (Select * from SplitString(@ItemCD,','))))
					and (@CommentInStore is Null or(msku.CommentInStore  Like '%'  +@CommentInStore+ '%' ))
					and (@ReserveCD is Null or ( msku.ReserveCD=@ReserveCD))
					and (@NticesCD is Null or (msku.NoticesCD=@NticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttentionCD is Null or (msku.OrderAttentionCD=@OrderAttentionCD))
					and (@SportsCD is Null or (msku.SportsCD=@SportsCD))
					and (@InsertDateTimeF is Null or (msku.InsertDateTime >= @InsertDateTimeF))
					and (@InsertDateTimeT is Null or (msku.InsertDateTime <= @InsertDateTimeT))
					and (@UpdateDateTimeT is Null or (msku.UpdateDateTime >= @UpdateDateTimeT))
					and (@UpdateDateTimeF is Null or (msku.UpdateDateTime <= @UpdateDateTimeF))
					--and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					--and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and ((((@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
							and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))) ) or 
							(@chkUnapprove = 1 and msku.ApprovalDate is null)
							) 
					and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and  (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@TagName1 is Null or  TagName =@TagName1)
					and (@TagName2 is Null or  TagName =@TagName2)
					and (@TagName3 is Null or  TagName =@TagName3)
					and (@TagName4 is Null or  TagName =@TagName4)
					and (@TagName5 is Null or  TagName =@TagName5))
					--and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					--		)
					--	and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
					--		)
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					--		)
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					--		)
							
					--		and Exists
					--		(
					--			select *
					--			from M_SKU as msk
					--			join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
					--			and msk.ChangeDate =mst.ChangeDate
					--			--and( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
					--			--or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
					--				and (@TagName1 is Null or  TagName =@TagName1)
					--				and (@TagName2 is Null or  TagName =@TagName2)
					--				and (@TagName3 is Null or  TagName =@TagName3)
					--				and (@TagName4 is Null or  TagName =@TagName4)
					--				and (@TagName5 is Null or  TagName =@TagName5)
					--		))
		end

	if @type=3
	begin 
				insert into #Tmp_AdminNo
				select msku.AdminNO
					From	F_SKU(@ChangeDate) as msku
					Left outer join M_SKUInfo as msInfo on msku.AdminNO=msInfo.AdminNO
														and msku.ChangeDate = msInfo.ChangeDate 
														and msku.DeleteFlg=0
					Left outer join  M_SKUTag as mst on msku.AdminNO=mst.AdminNo
														and msku.ChangeDate =mst.ChangeDate

					where (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem IN (Select * from SplitString(@MakerItem,','))))
					and (@ItemCD is Null or (msku.ITemCD  IN (Select * from SplitString(@ItemCD,','))))
					and (@CommentInStore is Null or(msku.CommentInStore  Like '%'  +@CommentInStore+ '%' ))
					and (@ReserveCD is Null or ( msku.ReserveCD=@ReserveCD))
					and (@NticesCD is Null or (msku.NoticesCD=@NticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttentionCD is Null or (msku.OrderAttentionCD=@OrderAttentionCD))
					and (@SportsCD is Null or (msku.SportsCD=@SportsCD))
					and (@InsertDateTimeF is Null or (msku.InsertDateTime >= @InsertDateTimeF))
					and (@InsertDateTimeT is Null or (msku.InsertDateTime <= @InsertDateTimeT))
					and (@UpdateDateTimeT is Null or (msku.UpdateDateTime >= @UpdateDateTimeT))
					and (@UpdateDateTimeF is Null or (msku.UpdateDateTime <= @UpdateDateTimeF))
					--and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					--and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and ((((@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
							and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))) ) or 
							(@chkUnapprove = 1 and msku.ApprovalDate is null)
							) 
					and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and  (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@TagName1 is Null or  TagName =@TagName1)
					and (@TagName2 is Null or  TagName =@TagName2)
					and (@TagName3 is Null or  TagName =@TagName3)
					and (@TagName4 is Null or  TagName =@TagName4)
					and (@TagName5 is Null or  TagName =@TagName5)

					--and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					--		)
					--	and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
					--		)
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					--		)
							
					--		and Exists
					--		(	
					--			select *
					--			from M_SKU as msk
					--			join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
					--			and msk.ChangeDate = msInfo.ChangeDate
					--			and (@InstructionsNO is Null or ( msInfo.InstructionsNO=@InstructionsNO))
					--		)
					--		and Exists
					--		(
					--			select *
					--			from M_SKU as msk
					--			join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
					--			and msk.ChangeDate =mst.ChangeDate
					--			--and ( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
					--			--or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
					--				and (@TagName1 is Null or  TagName =@TagName1)
					--				and (@TagName2 is Null or  TagName =@TagName2)
					--				and (@TagName3 is Null or  TagName =@TagName3)
					--				and (@TagName4 is Null or  TagName =@TagName4)
					--				and (@TagName5 is Null or  TagName =@TagName5)
					--		)
		
		end	

		
		select * from 
		(
		(select tmp.AdminNO,
		total.SKUCD,
		msk.SkuName as 商品名,
		msk.ColorName as カラー,
		msk.SizeName as サイズ,

		ms.StoreCD as 店舗CD,
		ms.StoreName as 店舗名,
							
		msko.SoukoCD as 倉庫CD,
		msko.SoukoName as 倉庫名,
		total.RackNO as 棚番,

		StockSu as 在庫数,
		PlanSu as 入荷予定数,
		AllowableSu  as 引当可能数,
		UpdateSu as メーカー在庫数,
		total.JanCD as JANCD,

		mb.BrandCD as ブランドCD,
		mb.BrandName as ブランド名,

		msk.ITemCD as ITEM ,
		msk.MakerItem as メーカー商品CD,


		dmi.MinimumInventory as 基準在庫,
		msk.PriceWithTax as 販売定価,
		msk.OrderPriceWithoutTax as 標準原価,
		ISNULL(Convert(varchar(10),total.ArrivalPlanDate), Convert(varchar(10),Isnull(total.ArrivalPlanMonth,'')) + mp.Char1)as 最速入荷日

		from #Tmp_AdminNo as tmp
		inner join 
		(select D_Stock1.AdminNO,SKUCD,JanCD,SoukoCD,RackNO,StockSu,PlanSu,AllowableSu,Isnull(UpdateSu,0) as UpdateSu,ArrivalPlanDate.ArrivalPlanCD,ArrivalPlanDate.ArrivalPlanMonth,ArrivalPlanDate from
		(
		select AdminNO,Max(SKUCD) as SKUCD,Max(JanCD) as JanCD,Max(SoukoCD)as SoukoCD,Max(RackNO)as RackNO,(sum(Isnull(StockSu,0))+ sum(Isnull(ReturnPlanSu,0)) - sum(isnull(ReturnSu,0))) as StockSu,Sum(Isnull(ds.PlanSu,0)) as PlanSu,Sum(Isnull(ds.AllowableSu,0)) as AllowableSu from D_Stock ds
		where ds.DeleteDateTime is Null
					and   (@SoukoCD is Null or (ds.SoukoCD=@SoukoCD))
					and  (@RackNoF is Null or ( ds.RackNO >=@RackNoF))
					and  (@RackNoT is Null or ( ds.RackNO <=@RackNoT))
		group by AdminNo,SoukoCD,RackNO
		--Having(sum(Isnull(StockSu,0)) != 0) D_Stock1
		Having (CASE WHEN @chktype ='0'
        THEN sum(Isnull(StockSu,0))+ sum(Isnull(ReturnPlanSu,0)) - sum(isnull(ReturnSu,0)) 
		ELSE 0  END) != 0 ) D_Stock1
		left outer join
		(select * from 
		(
		(select AdminCD,SUM(Isnull(UpdateSu,0)) as UpdateSu from D_MakerStock dms
		group by AdminCD )   
		) t1)D_MakerStock1 on D_Stock1.AdminNO = D_MakerStock1.AdminCD
		left outer join 
		(select * from
		(select AdminNO,Max(ArrivalPlanCD) as ArrivalPlanCD,Max(ArrivalPlanMonth) as ArrivalPlanMonth,Min(ArrivalPlanDate)as ArrivalPlanDate from F_ArrivalPlan(getdate()) fap
		where fap.DeleteDateTime is null
		group by AdminNO)t2
		)ArrivalPlanDate on D_Stock1.AdminNO=ArrivalPlanDate.AdminNO ) as total on total.AdminNO=tmp.AdminNO 

		left outer join F_SKU(@ChangeDate) as msk on msk.AdminNO =tmp.AdminNO
		left outer join F_Souko(@ChangeDate) as msko on  msko.SoukoCD=total.SoukoCD
		left outer join F_Store(@ChangeDate) as ms on ms.StoreCD=msko.StoreCD
		left outer join M_Brand as mb on mb.BrandCD =msk.BrandCD
		left outer join D_MinimumInventory as dmi on dmi.JanCD =total.JanCD
											and dmi.SoukoCD=total.SoukoCD
											and dmi.DeleteDateTime is Null
		left outer join M_MultiPorpose as mp on mp.[Key]=total.ArrivalPlanCD
		and mp.ID='206'
		)

		Union All		
						
		(Select Max(dms.AdminCD) AdminCD,
				Max(dms.SKUCD) SKUCD,
				Max(msku.SKUName) SKUName,
				Max(msku.ColorName),
				Max(msku.SizeName) ,
				null ,
				null,
				null ,
				null ,
				null,
				Sum(Isnull(dms.UpdateSu,0)),
				null ,
				Sum(Isnull(dms.UpdateSu,0)),
				Sum(Isnull(dms.UpdateSu,0)) ,
				Max(dms.JanCD),
				Max(mb.BrandCD),
				Max(mb.BrandName),
				Max(msku.ITemCD),
				Max(msku.MakerItem) ,
				null,
				null ,
				Max(msku.PriceWithTax),
				Max(Convert(varchar(10),msku.OrderPriceWithoutTax))	
		from #Tmp_AdminNo as tmp
		inner join D_MakerStock as dms on dms.AdminCD=tmp.AdminNO
		and   (@makerCD is Null or  (dms.MakerCD=@makerCD))
		left outer join M_SKU as msku on msku.AdminNO=tmp.AdminNO
		left outer join M_Brand as mb on mb.BrandCD=msku.BrandCD
		where 
		not  Exists
				(	
					select *
					from D_Stock as ds
					where ds.AdminNO=tmp.AdminNO
								
				)
			Group by AdminCD
		Having (CASE WHEN @chktype ='0'
        THEN sum(Isnull(dms.UpdateSu,0))
		ELSE 0  END) != 0 )  
		)result
		order by AdminNO
			
				drop table  #Tmp_AdminNo;
					
					
END


















