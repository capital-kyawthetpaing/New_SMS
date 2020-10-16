using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class M_SKU_DL : Base_DL
    {
        public DataTable M_SKU_Select(M_SKU_Entity mse)
        {
            string sp = "M_SKU_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mse.AdminNO } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
            };
            return SelectData(dic, sp);
        }
        public DataTable M_SKU_SelectAll(M_SKU_Entity mse)
        {
            string sp = "M_SKU_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mse.AdminNO } },
                    { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                    { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                    { "@SetKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SetKBN } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                };

            return SelectData(dic, sp);
        }
        public DataTable M_SKU_SelectByMaker(M_SKU_Entity mse)
        {
            string sp = "M_SKU_SelectByMaker";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerItem } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectForSearchProduct(M_SKU_Entity mse, M_SKUInfo_Entity msie)
        {
            string sp = "M_SKU_SelectForSearchProduct";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@MainVendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MainVendorCD } },
                    { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.BrandCD } },
                    { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                    { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                    { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                    { "@CommentInStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.CommentInStore } },
                    { "@OrOrAnd", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.OrOrAnd } },
                    { "@ReserveCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ReserveCD } },
                    { "@NoticesCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.NoticesCD } },
                    { "@PostageCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PostageCD } },
                    { "@OrderAttentionCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.OrderAttentionCD } },
                    { "@SportsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SportsCD } },
                    { "@TagName1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TagName1 } },
                    { "@TagName2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TagName2 } },
                    { "@TagName3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TagName3 } },
                    { "@TagName4", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TagName4 } },
                    { "@TagName5", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.TagName5 } },
                    //{ "@ClassificationA", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationA } },
                    //{ "@ClassificationB", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationB } },
                    //{ "@ClassificationC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationC } },

                    { "@YearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.YearTerm } },
                    { "@Season", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.Season } },
                    { "@CatalogNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.CatalogNO } },
                    { "@InstructionsNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.InstructionsNO } },

                    { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerItem } },
                    { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@InputDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InputDateFrom } },
                { "@InputDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InputDateTo } },
                { "@UpdateDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.UpdateDateFrom } },
                { "@UpdateDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.UpdateDateTo } },
                { "@ApprovalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ApprovalDateFrom } },
                { "@ApprovalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ApprovalDateTo } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                    { "@SearchFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SearchFlg } },
                    { "@ItemOrMaker", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.ItemOrMaker } },
                };

            return SelectData(dic, sp);
        }
        public DataTable M_SKU_SelectAllForTempoShukka(M_SKU_Entity mse, string juchuuNo)
        {
            string sp = "M_SKU_SelectAllForTempoShukka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mse.AdminNO } },
                    { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                    { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                    { "@JuchuuNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = juchuuNo } },
                };

            return SelectData(dic, sp);
        }

        public DataTable Select_M_SKU_Data(string janCD)
        {
            string sp = "M_SKU_Select_ByJanCD";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = janCD } },

            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }
        public DataTable M_MakerVendor_DataSelect(TempoRegiZaikoKakunin_Entity kne)
        {
            string sp = "M_SKUMakerVendor_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@JanCD",new ValuePair { value1=SqlDbType.VarChar,value2=kne.JanCD} } ,
                {"@StoreCD",new ValuePair { value1=SqlDbType.VarChar,value2=kne.StoreCD} } ,
                {"@DataCheck",new ValuePair { value1=SqlDbType.TinyInt,value2=kne.dataCheck} } ,

            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectAllForTempoRegiShohin(M_SKU_Entity mse)
        {
            string sp = "M_SKU_SelectAllForTempoRegiShohin";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.BrandCD } },
                    { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                    { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
                };
            return SelectData(dic, sp);
        }

        /// <summary>	
        /// 商品マスタ更新処理	
        /// MasterTouroku_Syohinより更新時に使用	
        /// </summary>	
        /// <param name="me"></param>	
        /// <returns></returns>	
        public bool M_SKU_Exec(M_SKU_Entity me, short operationMode)
        {
            string sp = "PRC_MasterTouroku_Syohin";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = operationMode.ToString() } },
                { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = me. AdminNO} },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SKUCD} },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate} },
                { "@VariousFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.VariousFLG } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SKUName} },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.KanaName} },
                { "@SKUShortName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SKUShortName} },
                { "@EnglishName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.EnglishName } },
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ITemCD } },
                { "@ColorNO", new ValuePair { value1 = SqlDbType.Int, value2 = me.ColorNO} },
                { "@SizeNO", new ValuePair { value1 = SqlDbType.Int, value2 = me.SizeNO } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.JanCD} },
                { "@SetKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.SetKBN } },
                { "@PresentKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.PresentKBN } },
                { "@SampleKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.SampleKBN} },
                { "@DiscountKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DiscountKBN } },
                { "@ColorName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ColorName } },
                { "@SizeName", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SizeName} },
                { "@WebFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.WebFlg } },
                { "@RealStoreFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.RealStoreFlg} },
                { "@MainVendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MainVendorCD} },
                { "@MakerVendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MakerVendorCD } },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.BrandCD} },
                { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.MakerItem } },
                { "@TaniCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.TaniCD } },
                { "@SportsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SportsCD } },
                { "@ZaikoKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ZaikoKBN } },
                { "@Rack", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Rack } },
                { "@VirtualFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.VirtualFlg } },
                { "@DirectFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DirectFlg} },
                { "@ReserveCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ReserveCD } },
                { "@NoticesCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.NoticesCD } },
                { "@PostageCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PostageCD } },
                { "@ManufactCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ManufactCD} },
                { "@ConfirmCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ConfirmCD } },
                { "@WebStockFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.WebStockFlg } },
                { "@StopFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.StopFlg} },
                { "@DiscontinueFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DiscontinueFlg} },
                { "@InventoryAddFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.InventoryAddFlg } },
                { "@MakerAddFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.MakerAddFlg } },
                { "@StoreAddFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.StoreAddFlg } },
                { "@NoNetOrderFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.NoNetOrderFlg } },
                { "@EDIOrderFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.EDIOrderFlg } },
                { "@CatalogFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CatalogFlg } },
                { "@ParcelFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ParcelFlg} },
                { "@AutoOrderFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.AutoOrderFlg} },
                { "@TaxRateFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.TaxRateFLG } },
                { "@CostingKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.CostingKBN } },
                { "@SaleExcludedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.SaleExcludedFlg } },
                { "@PriceWithTax", new ValuePair { value1 = SqlDbType.Money, value2 = me. PriceWithTax} },
                { "@PriceOutTax", new ValuePair { value1 = SqlDbType.Money, value2 = me.PriceOutTax } },
                { "@OrderPriceWithTax", new ValuePair { value1 = SqlDbType.Money, value2 = me.OrderPriceWithTax } },
                { "@OrderPriceWithoutTax", new ValuePair { value1 = SqlDbType.Money, value2 = me. OrderPriceWithoutTax} },
                { "@SaleStartDate", new ValuePair { value1 = SqlDbType.Date, value2 = me. SaleStartDate } },
                { "@WebStartDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.WebStartDate} },
                { "@OrderAttentionCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.OrderAttentionCD} },
                { "@OrderAttentionNote", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.OrderAttentionNote} },
                { "@CommentInStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CommentInStore} },
                { "@CommentOutStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.CommentOutStore } },
                { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastYearTerm} },
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastSeason} },
                { "@LastCatalogNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastCatalogNO } },
                { "@LastCatalogPage", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastCatalogPage } },
                { "@LastCatalogText", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastCatalogText } },
                { "@LastInstructionsNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.LastInstructionsNO} },
                { "@LastInstructionsDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.LastInstructionsDate} },
                { "@WebAddress", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.WebAddress} },
                { "@SetAdminCD", new ValuePair { value1 = SqlDbType.Int, value2 = me.SetAdminCD } },
                { "@SetItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SetItemCD} },
                { "@SetSKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.SetSKUCD } },
                { "@SetSU", new ValuePair { value1 = SqlDbType.Int, value2 = me.SetSU} },
                { "@ApprovalDate",  new ValuePair { value1 = SqlDbType.Date, value2 = me.ApprovalDate} },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DeleteFlg} },
                { "@UsedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.UsedFlg} },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.DeleteFlg} },
                { "@UsedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.UsedFlg} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.UpdateOperator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PC} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        public DataTable M_SKU_SelectByItemCD(M_ITEM_Entity me)
        {
            string sp = "M_SKU_SelectByItemCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ITemCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.ITemCD} } ,
                {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=me.ChangeDate} } ,
            };
            return SelectData(dic, sp);
        }
        public DataTable M_SKU_SelectByJANCD(M_SKU_Entity me)
        {
            string sp = "M_SKU_SelectByJANCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ITemCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.ITemCD} } ,
                {"@JANCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.JanCD} } ,
                {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=me.ChangeDate} } ,
            };
            return SelectData(dic, sp);
        }
        public DataTable M_SKU_SelectBySKUCD(M_SKU_Entity me)
        {
            string sp = "M_SKU_SelectBySKUCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@SKUCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.SKUCD} } ,
                {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=me.ChangeDate} } ,
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_JanCDHenkou_Select(string xml)
        {
            string sp = "M_SKU_JanCDHenkou_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@xml",new ValuePair { value1=SqlDbType.Xml,value2=xml} } 
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectForSKSMasterUpdate()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };
            return SelectData(dic, "M_SKU_SelectForSKSMasterUpdate");
        }

        public DataTable M_SKUDataForHanbaiTankaKakeritu(M_SKU_Entity mskue, M_SKUPrice_Entity mskupe,string option)
        {
            string sp = "M_SKUSelectDataForHanbaiTankaKakeritu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StartDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ChangeDate } },
                { "@EndDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.EndDate } },
                { "@DateCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.DateCopy } },
                { "@TankaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskupe.TankaCD } },
                { "@TankaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskupe.TankaName } },
                { "@TankaCDCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskupe.TankaCDCopy } },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.BrandCD } },
                { "@BrandCDCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.BrandCDCopy } },
                { "@ExhibitionSegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ExhibitionSegmentCD } },
                { "@ExhibitionSegmentCDCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.ExhibitionSegmentCDCopy } },
                { "@LastYearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastYearTerm } },
                { "@LastYearTermCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastYearTermCopy } },
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastSeason } },
                { "@LastSeasonCopy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.LastSeasonCopy } },
                { "@PriceOutTaxFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.PriceOutTaxFrom } },
                { "@PriceOutTaxTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mskue.PriceOutTaxTo } },
                { "@Option", new ValuePair { value1 = SqlDbType.VarChar, value2 =option } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectByJanCD_ForTenzikaishouhin(M_SKU_Entity msk)
        {
            string sp = "M_SKU_SelectByJanCD_ForTenzikaishouhin";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msk.JanCD } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = msk.MainVendorCD } },

            };
            return SelectData(dic, sp);
        }
    }

}
