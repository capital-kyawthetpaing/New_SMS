
 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_Select_byCustomerSKUPrice]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [M_SKU_Select]    */
Create PROCEDURE [dbo].[M_SKU_Select_byCustomerSKUPrice](
    -- Add the parameters for the stored procedure here
   @AdminNO as varchar(15),
   @SKUCD as varchar(50),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.SKUCD
	,MS.AdminNO
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.VariousFLG
          ,MS.SKUName
          ,MS.KanaName
          ,MS.SKUShortName
          ,MS.EnglishName
          ,MS.ITemCD
          ,MS.ColorNO
          ,MS.SizeNO
          ,MS.JanCD
          ,MS.SetKBN
          ,MS.PresentKBN
          ,MS.SampleKBN
          ,MS.DiscountKBN
          ,MS.ColorName
          ,MS.SizeName
          ,MS.WebFlg
          ,MS.RealStoreFlg
          ,MS.MainVendorCD
          ,MS.MakerVendorCD
          ,MS.BrandCD
          ,MS.MakerItem
          ,MS.TaniCD
          ,MS.SportsCD
          ,MS.SegmentCD
          ,MS.ZaikoKBN
          ,MS.Rack
          ,MS.VirtualFlg
          ,MS.DirectFlg
          ,MS.ReserveCD
          ,MS.NoticesCD
          ,MS.PostageCD
          ,MS.ManufactCD
          ,MS.ConfirmCD
          ,MS.WebStockFlg
          ,MS.StopFlg
          ,MS.DiscontinueFlg
          ,MS.InventoryAddFlg
          ,MS.MakerAddFlg
          ,MS.StoreAddFlg
          ,MS.NoNetOrderFlg
          ,MS.EDIOrderFlg
          ,MS.CatalogFlg
          ,MS.ParcelFlg
          ,MS.AutoOrderFlg
          ,MS.TaxRateFLG
          ,MS.CostingKBN
          ,MS.SaleExcludedFlg
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
          ,MS.Rate
          ,CONVERT(varchar,MS.SaleStartDate,111) AS SaleStartDate
          ,CONVERT(varchar,MS.WebStartDate,111) AS WebStartDate
          ,MS.OrderAttentionCD
          ,MS.OrderAttentionNote
          ,MS.CommentInStore
          ,MS.CommentOutStore
          ,MS.LastYearTerm
          ,MS.LastSeason
          ,MS.LastCatalogNO
          ,MS.LastCatalogPage
          ,MS.LastCatalogText
          ,MS.LastInstructionsNO
          ,CONVERT(varchar,MS.LastInstructionsDate,111) AS LastInstructionsDate
          ,MS.WebAddress
          ,MS.SetAdminCD
          ,MS.SetItemCD
          ,MS.SetSKUCD
          ,MS.SetSU
          ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate 
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
		  
    from M_SKU MS
    
    WHERE 
	 Ms.AdminNo = @AdminNO and Ms.SKUCD = @SKUCD and
     MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    ;
END

