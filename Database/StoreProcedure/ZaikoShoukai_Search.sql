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
CREATE PROCEDURE [dbo].[ZaikoShoukai_Search]
	-- Add the parameters for the stored procedure here
	@ChangeDate as date,
	@VendorCD as varchar(13),
	@makerCD as varchar(13),
	@brandCD as varchar(13),
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


	@SoukoCD as varchar(6),
	@RackNoF as varchar(11),
	@RackNoT as varchar(11)
	--@kijunbi as date =getdate
	--@makercd as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 IF (@YearTerm < 1)
		SET @YearTerm = null

	IF (@Season < 1)
		SET @Season = null

		IF (@ReserveCD < 1)
		SET @ReserveCD = null

	IF (@NticesCD < 1)
		SET @NticesCD = null

	IF (@PostageCD < 1)
		SET @PostageCD = null

	IF (@OrderAttentionCD < 1)
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

					where 
					
					 (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem=@MakerItem))
					and (@ItemCD is Null or (msku.ITemCD=@ItemCD))
					and DeleteFlg=0
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
					and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
							)
						and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and(@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or ( msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(
								select *
								from M_SKU as msk
								join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
								and msk.ChangeDate =mst.ChangeDate
								and( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
								or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
							))
		end

	if @type=2
	begin 
				insert into #Tmp_AdminNo
				select AdminNO from F_SKU(@ChangeDate)  where MakerItem in
				
				(select MakerItem
					From	F_SKU(@ChangeDate) as msku

					where 
					msku.ChangeDate <=@ChangeDate
					and (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem=@MakerItem))
					and (@ItemCD is Null or (msku.ITemCD=@ItemCD))
					and DeleteFlg=0
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
					and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
							)
						and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and(@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or ( msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(
								select *
								from M_SKU as msk
								join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
								and msk.ChangeDate =mst.ChangeDate
								and( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
								or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
							))
		end

	if @type=3
	begin 
				insert into #Tmp_AdminNo
				select AdminNO
					From	F_SKU(@ChangeDate) as msku

					where 
					 (@VendorCD is Null or (msku.MainVendorCD =@VendorCD))
					and ( @makerCD is Null or ( msku.MakerVendorCD = @makerCD))
					and (@brandCD is Null or  (msku.BrandCD =@brandCD))
					and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
					and (@JanCD is Null or ( msku.JanCD=@JanCD))
					and (@SkuCD is Null or (msku.SKUCD=@SkuCD))
					and (@MakerItem is Null or (msku.MakerItem=@MakerItem))
					and (@ItemCD is Null or (msku.ITemCD=@ItemCD))
					and DeleteFlg=0
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
					and (@ApprovalDateF is Null or ( msku.ApprovalDate >= @ApprovalDateF))		
					and (@ApprovalDateT is Null or (msku.ApprovalDate <= @ApprovalDateT))
					and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
							)
						and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and  (@Season is Null or(  msInfo.Season=@SeaSon))
								
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and(@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(	
								select *
								from M_SKU as msk
								join M_SKUInfo as msInfo on msk.AdminNO=msk.AdminNO
								and msk.ChangeDate = msInfo.ChangeDate
								and @InstructionsNO is Null or ( msInfo.InstructionsNO=@InstructionsNO)
							)
							and Exists
							(
								select *
								from M_SKU as msk
								join M_SKUTag as mst on msk.AdminNO=mst.AdminNo
								and msk.ChangeDate =mst.ChangeDate
								and( @TagName1 is Null or @TagName2 is Null or @TagName3 is Null or @TagName4 is Null or @TagName5 is Null 
								or( TagName In (@TagName1,@TagName2,@TagName3,@TagName4,@TagName5)))
							)
		
		end	

		
						(select			
								MAX(ds.AdminNO) as AdminNO,
								MAX(ds.SKUCD) as SKUCD,
								MAX(msk.SkuName) as 商品名,
								MAX(msk.ColorName) as カラー,
								MAX(msk.SizeName) as サイズ,

								MAX(ms.StoreCD) as 店舗CD,
								MAX(ms.StoreName) as 店舗名,
								
								MAX(msko.SoukoCD)as 倉庫CD,
								MAX(msko.SoukoName)as 倉庫名,

								MAX(ds.RackNO) as 棚番,
								MAX(ds.StockSu) as 在庫数,
								MAX(ds.PlanSu) as 入荷予定数,
								MAX(ds.AllowableSu) as 引当可能数,
								SUM(dms.UpdateSu) as メーカー在庫数,
								MAX(ds.JanCD) as JANCD,

								MAX(mb.BrandCD) as ブランドCD,
								MAX(mb.BrandName) as ブランド名,

								MAX(msk.ITemCD) as ITEM ,
								MAX(msk.MakerItem) as メーカー商品CD,
								MAX(ISNULL(Convert(varchar(10),dap.ArrivalPlanDate), Convert(varchar(10),dap.ArrivalPlanMonth) + mp.Char1)) as 最速入荷日,	
								MAX(dmi.MinimumInventory) as 基準在庫,
								MAX(msk.PriceWithTax) as 販売定価,
								MAX(msk.OrderPriceWithoutTax) as 標準原価		
					from		#Tmp_AdminNo as tmp
					
					inner Join D_Stock as ds on ds.AdminNO	IN(Select AdminNO from #Tmp_AdminNo)
					and ds.DeleteDateTime is Null
					and ds.SoukoCD=@SoukoCD
					and ds.RackNO >=@RackNoF
					and ds.RackNO <=@RackNoT
					and ds.StockSu  != '0'
					left outer join F_SKU(@ChangeDate) as msk on msk.AdminNO IN(Select AdminNO from #Tmp_AdminNo)
					left outer join F_Souko(@ChangeDate) as msko on  msko.SoukoCD=ds.SoukoCD
					left outer join F_Store(@ChangeDate) as ms on ms.StoreCD=msko.SoukoCD
					left outer join M_Brand as mb on mb.BrandCD =msk.BrandCD
					left outer join D_MinimumInventory as dmi on dmi.JanCD =ds.JanCD
														and dmi.SoukoCD=ds.SoukoCD
						
														and dmi.DeleteDateTime is Null

					left outer join F_ArrivalPlan(@ChangeDate) as dap on dap.AdminNO=ds.AdminNO


					left outer join M_MultiPorpose as mp on mp.[Key]=dap.ArrivalPlanCD
					and mp.ID='206'
					
					left outer join D_MakerStock as dms on dms.AdminCD=ds.AdminNO
					 Group by AdminCD
					)
					Union All						
					(select
							dms.AdminCD,
							dms.SKUCD,
							msku.SKUName,
							msku.ColorName,
							msku.SizeName,
							null,
							null,
							null,
							null,
							null,
							dms.UpdateSu,
							null,
							dms.UpdateSu,
							dms.UpdateSu,
							dms.JanCD,
							mb.BrandCD,
							mb.BrandName,
							msku.ITemCD,
							msku.MakerItem,
							null,
							null,
							msku.PriceWithTax,
							msku.OrderPriceWithoutTax
					from #Tmp_AdminNo as tmp
					inner join D_MakerStock as dms on dms.AdminCD IN(Select AdminNO from #Tmp_AdminNo)
					and dms.MakerCD=@makerCD
					and dms.UpdateSu != 0
					left outer join M_SKU as msku on msku.AdminNO IN(Select AdminNO from #Tmp_AdminNo)
					left outer join M_Brand as mb on mb.BrandCD=msku.BrandCD
					where 
					not  Exists
							(	
								select *
								from D_Stock as ds
								where ds.AdminNO IN(Select AdminNO from #Tmp_AdminNo)
								
							)
							)

				
			
				drop table  #Tmp_AdminNo;	
					
					
END

