 BEGIN TRY 
 Drop Procedure dbo.[M_Item_SelectForSKSMasterUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_Item_SelectForSKSMasterUpdate]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

Select
	t1.AdminNO as Item_AdminCode,

	mitem.ITemCD as Item_Code,
	ITemName as Item_Name,
	PriceWithTax as List_Pice,

	(select WebPriceWithTax
	from  F_ItemPrice(GetDate()) fip
	where fip.ITemCD=mitem.ITemCD) as Sale_Price,

	OrderPriceWithTax as Cost,
	SaleStartDate as Release_Date,
	WebStartDate as Post_Available_Date,
	LastYearTerm as 'Year',
	LastSeason as Season,
	BrandCD as Brand_Code,

	(select BrandName
	from M_Brand 
	where BrandCD=mitem.BrandCD) as BrandName,

	'NULL' as Brand_Code_Yahoo,
	SportsCD as Competition_Code,

	(select Char1
	from M_MultiPorpose
	where ID='202'
	and [Key]=mitem.SportsCD) as Competition_Name,

	SegmentCD as Classification_Code,

	(select Char1
	from M_MultiPorpose
	where ID='203'
	and [Key]=mitem.SegmentCD) as Class_Name,

	(select VendorName
	from F_Vendor(GetDate()) as fv
	where fv.VendorCD=mitem.MainVendorCD) as Company_Name,

	LastCatalogNO as Catalog_Information,
	NoticesCD as Special_Flag,
	ReserveCD as Reservation_Flag,
	LastInstructionsNO as Instruction_No,
	ApprovalDate as Approve_Date,
	CommentInStore as Remarks,
	MakerItem as Product_Code,
	GetDate() as SysDate


	from M_ITEM as mitem
	Left join(SELECT ITemCD, AdminNo = 
				STUFF((SELECT ', ' + cast(AdminNo as varchar(30))
				FROM M_SKU msku
				WHERE msku.ITemCD = msku1.ITemCD 
				FOR XML PATH('')), 1, 2, '')
				FROM M_SKU msku1
				GROUP BY ITemCD
				) as t1 on t1.ITemCD = mitem.ItemCD
	where mitem.SkSUpdateFlg=1
	Order by mitem.ITemCd


END
