BEGIN TRY 
 Drop Procedure dbo.[M_SKUPrice_Export]
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
Create PROCEDURE [dbo].[M_SKUPrice_Export]
	-- Add the parameters for the stored procedure here
	@ChangeDate as date,
	@VendorCD as varchar(13),
	@MakerCD as varchar(13),
	@BrandCD as varchar(6),
	@SKUName as varchar(100),
	@JanCD as varchar(13),
	@SkuCD as varchar(30),
	@MakerItem as varchar(50),
	@ItemCD as varchar(30),
	@CommentInStore as varchar(300),
	@ReserveCD as varchar(3),
	@NoticesCD as varchar(3),
	@PostageCD as varchar(3),
	@OrderAttention as varchar(3),
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
	@Tag1 as varchar(20),
	@Tag2 as varchar(20),
	@Tag3 as varchar(20),
	@Tag4 as varchar(20),
	@Tag5 as varchar(20),
	@mode as tinyint,
	--@checkflg as tinyint,
	@chkUnapprove as tinyint,
	@Program as varchar(100),
	@PC as varchar(30),
	@ProcessMode as varchar(50),
	@KeyItem as varchar(100),
	@Operator as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	IF (@YearTerm = -1)
		SET @YearTerm = null

	IF (@Season = -1)
		SET @Season = null

		IF (@ReserveCD = -1)
		SET @ReserveCD = null

	IF (@NoticesCD = -1)
		SET @NoticesCD = null

	IF (@PostageCD = -1)
		SET @PostageCD = null

	IF (@OrderAttention = -1)
		SET @OrderAttention = null


	CREATE TABLE [dbo].[#TAdminNo](
	[AdminNO] [int] NULL,)



    -- Insert statements for procedure here
	if @mode=1
	begin 
				insert into #TAdminNo
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
					and (@NoticesCD is Null or (msku.NoticesCD=@NoticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttention is Null or (msku.OrderAttentionCD=@OrderAttention))
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
					and (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and  (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@Tag1 is Null or  TagName =@Tag1)
					and (@Tag2 is Null or  TagName =@Tag2)
					and (@Tag3 is Null or  TagName =@Tag3)
					and (@Tag4 is Null or  TagName =@Tag4)
					and (@Tag5 is Null or  TagName =@Tag5))
					
		end

	
	if @mode=2
	begin 
				insert into #TAdminNo
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
					and (@NoticesCD is Null or (msku.NoticesCD=@NoticesCD))
					and  (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttention is Null or (msku.OrderAttentionCD=@OrderAttention))
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
					and (@YearTerm is Null or( msInfo.YearTerm=@YearTerm))
					and (@Season is Null or(  msInfo.Season=@SeaSon))
					and (@CatalogNo is Null or ( msInfo.CatalogNO=@CatalogNo))
					and ( @InstructionsNO is Null or (msInfo.InstructionsNO=@InstructionsNO))
					and (@Tag1 is Null or  TagName =@Tag1)
					and (@Tag2 is Null or  TagName =@Tag2)
					and (@Tag3 is Null or  TagName =@Tag3)
					and (@Tag4 is Null or  TagName =@Tag4)
					and (@Tag5 is Null or  TagName =@Tag5))
					
		end

	if @mode=3
	begin 
				insert into #TAdminNo
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
					and (@NoticesCD is Null or (msku.NoticesCD=@NoticesCD))
					and (@PostageCD is Null or (msku.PostageCD=@PostageCD))
					and (@OrderAttention is Null or (msku.OrderAttentionCD=@OrderAttention))
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
					and (@Tag1 is Null or  TagName =@Tag1)
					and (@Tag2 is Null or  TagName =@Tag2)
					and (@Tag3 is Null or  TagName =@Tag3)
					and (@Tag4 is Null or  TagName =@Tag4)
					and (@Tag5 is Null or  TagName =@Tag5)

					
		end	

	
		
	
		select 
			1 				'データ区分',
			fskup.TankaCD	'単価設定CD',
			fskup.StoreCD	'店舗CD',
			fskup.AdminNO	,
			fskup.SKUCD	,
			fs.SKUName	'商品名',
			Replace(fskup.ChangeDate,'-','/') '改定日',
			Replace(TekiyouShuuryouDate	,'-','/') '適用終了日',
			fskup.PriceWithTax	'税込定価',
			fskup.PriceWithoutTax		 '税抜定価',
			fskup.GeneralRate			 '一般掛率',
			fskup.GeneralPriceWithTax	 '税込一般単価',
			fskup.GeneralPriceOutTax	 '税抜一般単価',
			fskup.MemberRate			 '会員掛率',
			fskup.MemberPriceWithTax	 '税込会員単価',
			fskup.MemberPriceOutTax		 '税抜会員単価',
			fskup.ClientRate			 '外商掛率',
			fskup.ClientPriceWithTax	 '税込外商単価',
			fskup.ClientPriceOutTax		 '税抜外商単価',
			fskup.SaleRate				 'Sale掛率',
			fskup.SalePriceWithTax		 '税込Sale単価',
			fskup.SalePriceOutTax		 '税抜Sale単価',
			fskup.WebRate				 'Web掛率',
			fskup.WebPriceWithTax		 '税込Web単価',
			fskup.WebPriceOutTax		 '税抜Web単価',
			fskup.Remarks				 '備考',
			fskup.DeleteFlg				 '削除FLG'

		from #TAdminNo as tmp
			left outer join F_SKU(@ChangeDate) as fs on tmp.AdminNO=fs.AdminNO
			left outer join F_SKUPrice(@ChangeDate) as fskup on tmp.AdminNO=fskup.AdminNO
			order by fskup.TankaCD,fskup.StoreCD,fskup.SKUCD,fskup.ChangeDate
			
				drop table  #TAdminNo;
				exec dbo.L_Log_Insert @Operator,@Program,@PC,@ProcessMode,@KeyItem
					
END









































































































































