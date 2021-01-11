using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_SKU_Entity : Base_Entity
    {
        public string AdminNO { get; set; }
        public string SKUCD {get;set;}
        public string SKUCDFrom { get; set; }//ses
        public string SKUCDTo { get; set; }//ses
        public string VariousFLG { get; set; }
        public string SKUName { get; set; }
        public string KanaName { get; set; }
        public string SKUShortName { get; set; }
        public string EnglishName { get; set; }
        public string ITemCD { get; set; }
        public string ColorNO { get; set; }
        public string SizeNO { get; set; }
        public string JanCD { get; set; }
        public string SetKBN { get; set; }
        public string PresentKBN { get; set; }
        public string SampleKBN { get; set; }
        public string DiscountKBN { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string WebFlg { get; set; }
        public string RealStoreFlg { get; set; }
        public string MainVendorCD { get; set; }
        public string MakerVendorCD { get; set; }
        public string BrandCD { get; set; }
        public string MakerItem { get; set; }
        public string TaniCD { get; set; }
        public string SportsCD { get; set; }
        public string SegmentCD { get; set; }
        public string ZaikoKBN { get; set; }
        public string Rack { get; set; }
        public string VirtualFlg { get; set; }
        public string DirectFlg { get; set; }
        public string ReserveCD { get; set; }
        public string NoticesCD { get; set; }
        public string PostageCD { get; set; }
        public string ManufactCD { get; set; }
        public string ConfirmCD { get; set; }
        public string WebStockFlg { get; set; }
        public string StopFlg { get; set; }
        public string DiscontinueFlg { get; set; }
        public string SoldOutFlg { get; set; }
        public string InventoryAddFlg { get; set; }
        public string MakerAddFlg { get; set; }
        public string StoreAddFlg { get; set; }
        public string NoNetOrderFlg { get; set; }
        public string EDIOrderFlg { get; set; }
        public string CatalogFlg { get; set; }
        public string ParcelFlg { get; set; }
        public string AutoOrderFlg { get; set; }
        public string TaxRateFLG { get; set; }
        public string CostingKBN { get; set; }
        public string SaleExcludedFlg { get; set; }
        public string PriceWithTax { get; set; }
        public string PriceOutTax { get; set; }
        public string OrderPriceWithTax { get; set; }
        public string OrderPriceWithoutTax { get; set; }
        public string Rate { get; set; }
        public string SaleStartDate { get; set; }
        public string WebStartDate { get; set; }
        public string OrderAttentionCD        { get; set; }
        public string OrderAttentionNote { get; set; }
        public string CommentInStore { get; set; }
        public string CommentOutStore { get; set; }
        public string ExhibitionSegmentCD { get; set; }
        public string OrderLot { get; set; }
        public string ExhibitionCommonCD { get; set; }

        public string LastYearTerm { get; set; }
        public string LastSeason { get; set; }
        public string LastCatalogNO { get; set; }
        public string LastCatalogPage { get; set; }
        public string LastCatalogText { get; set; }
        public string LastInstructionsNO { get; set; }
        public string LastInstructionsDate { get; set; }
        public string WebAddress { get; set; }
        public string SetAdminCD { get; set; }
        public string SetItemCD { get; set; }
        public string SetSKUCD { get; set; }
        public string SetSU { get; set; }
        public string ApprovalDate { get; set; }
        //M_SKUTag
        public string TagName1 { get; set; }
        public string TagName2 { get; set; }
        public string TagName3 { get; set; }
        public string TagName4 { get; set; }
        public string TagName5 { get; set; }
        public string TagName6 { get; set; }
        public string TagName7 { get; set; }
        public string TagName8 { get; set; }
        public string TagName9 { get; set; }
        public string TagName10 { get; set; }
        public string CountOfSKUCD { get; set; }

        //検索用Entity
        public string OrOrAnd { get; set; }
        public string SearchFlg { get; set; }
        public string ItemOrMaker { get; set; }
        public string InputDateFrom { get; set; }
        public string InputDateTo { get; set; }
        public string UpdateDateFrom { get; set; }
        public string UpdateDateTo { get; set; }
        public string ApprovalDateFrom { get; set; }
        public string ApprovalDateTo { get; set; }

        //Master用Entity
        public string BrandName { get; set; }
        public string TaniName { get; set; }
        public string SegmentName { get; set; }
        public string SportsName { get; set; }
        public string ReserveName { get; set; }
        public string NoticesName { get; set; }
        public string PostageName { get; set; }
        public string ManufactName { get; set; }
        public string ConfirmName { get; set; }
        public string TaxRateFLGName { get; set; }
        public string CostingKBNName { get; set; }
        public string ShouhinCD { get; set; }

        //HanbaiTankaKakeritu Entity
        public string DateCopy { get; set; }
        public string BrandCDCopy { get; set; }
        public string ExhibitionSegmentCDCopy { get; set; }
        public string LastYearTermCopy { get; set; }
        public string LastSeasonCopy { get; set; }
        public string EndDate { get; set; }
        public string PriceOutTaxFrom { get; set; }
        public string PriceOutTaxTo { get; set; }
        //Mastertouroku_Tenzikaishouhin Entity
        public string InStoreCD { get; set; }


    }
}
