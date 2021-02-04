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
    public class M_ITEM_DL : Base_DL
    {
        public DataTable M_ITEM_Select(M_ITEM_Entity me)
        {
            string sp = "M_ITEM_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ITemCD } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_ITEM_SelectTop1(M_ITEM_Entity me)
        {
            string sp = "M_ITEM_SelectTop1";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ITemCD } },
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.ChangeDate } },
            };
            return SelectData(dic, sp);
        }
        //public DataTable M_SKU_SelectAll(M_SKU_Entity mse)
        //{
        //    string sp = "M_SKU_SelectAll";

        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //        {
        //            { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mse.AdminNO } },
        //            { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
        //            { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
        //            { "@SetKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SetKBN } },
        //            { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
        //        };

        //    return SelectData(dic,sp);
        //}

        public bool ImportItem(M_ITEM_Entity mie)
        {
 //           @xml as xml
	//,@Opt as varchar(20)
	//,@PG as varchar(100)
	//,@PC as varchar(20)
	//,@Mode as varchar(20)
	//,@KeyItem as varchar(200)
	//,@MainFlg as tinyint
            string sp = "_M_ItemImport";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@xml", SqlDbType.Xml,mie.xml1);
            AddParam(command, "@Opt", SqlDbType.VarChar, mie.Operator);
            AddParam(command, "@PG", SqlDbType.VarChar, mie.ProgramID);
            AddParam(command, "@PC", SqlDbType.VarChar, mie.PC);
            AddParam(command, "@Mode", SqlDbType.VarChar,mie.ProcessMode);
            AddParam(command, "@KeyItem", SqlDbType.VarChar, mie.Key);
            AddParam(command, "@MainFlg", SqlDbType.TinyInt, mie.MainFlg);
            string outPutParam = "";
            UseTransaction = true;
            return InsertUpdateDeleteData(sp, ref outPutParam);
        }

        /// <summary>
        /// 商品マスタ更新処理
        /// MasterTouroku_Syohinより更新時に使用
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public bool M_ITEM_Exec(M_ITEM_Entity me, DataTable dt, DataTable dtSite, short operationMode)
        {
            string sp = "PRC_MasterTouroku_Syouhin";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.VarChar, operationMode.ToString());

            AddParam(command, "@ITemCD", SqlDbType.VarChar, me.ITemCD);
            AddParam(command, "@ChangeDate", SqlDbType.VarChar, me.ChangeDate);
            AddParam(command, "@VariousFLG", SqlDbType.TinyInt, me.VariousFLG);
            AddParam(command, "@ITemName", SqlDbType.VarChar, me.ITemName);
            AddParam(command, "@KanaName", SqlDbType.VarChar, me.KanaName);
            AddParam(command, "@SKUShortName", SqlDbType.VarChar, me.SKUShortName);
            AddParam(command, "@EnglishName", SqlDbType.VarChar, me.EnglishName);
            AddParam(command, "@SetKBN", SqlDbType.TinyInt, me.SetKBN);
            AddParam(command, "@PresentKBN", SqlDbType.TinyInt, me.PresentKBN);
            AddParam(command, "@SampleKBN", SqlDbType.TinyInt, me.SampleKBN);
            AddParam(command, "@DiscountKBN", SqlDbType.TinyInt, me.DiscountKBN);
            AddParam(command, "@ColorName", SqlDbType.VarChar, me.ColorName);
            AddParam(command, "@SizeName", SqlDbType.VarChar, me.SizeName);
            AddParam(command, "@WebFlg", SqlDbType.TinyInt, me.WebFlg);
            AddParam(command, "@RealStoreFlg", SqlDbType.TinyInt, me.RealStoreFlg);
            AddParam(command, "@MainVendorCD", SqlDbType.VarChar, me.MainVendorCD);
            //AddParam(command, "@MakerVendorCD", SqlDbType.VarChar, me.MakerVendorCD);
            AddParam(command, "@BrandCD", SqlDbType.VarChar, me.BrandCD);
            AddParam(command, "@MakerItem", SqlDbType.VarChar, me.MakerItem);
            AddParam(command, "@TaniCD", SqlDbType.VarChar, me.TaniCD);
            AddParam(command, "@SportsCD", SqlDbType.VarChar, me.SportsCD);
            AddParam(command, "@SegmentCD", SqlDbType.VarChar, me.SegmentCD);
            AddParam(command, "@ZaikoKBN", SqlDbType.TinyInt, me.ZaikoKBN);
            AddParam(command, "@Rack", SqlDbType.VarChar, me.Rack);
            AddParam(command, "@VirtualFlg", SqlDbType.TinyInt, me.VirtualFlg);
            AddParam(command, "@DirectFlg", SqlDbType.TinyInt, me.DirectFlg);
            AddParam(command, "@ReserveCD", SqlDbType.VarChar, me.ReserveCD);
            AddParam(command, "@NoticesCD", SqlDbType.VarChar, me.NoticesCD);
            AddParam(command, "@PostageCD", SqlDbType.VarChar, me.PostageCD);
            AddParam(command, "@ManufactCD", SqlDbType.VarChar, me.ManufactCD);
            AddParam(command, "@ConfirmCD", SqlDbType.VarChar, me.ConfirmCD);
            AddParam(command, "@WebStockFlg", SqlDbType.TinyInt, me.WebStockFlg);
            AddParam(command, "@StopFlg", SqlDbType.TinyInt, me.StopFlg);
            AddParam(command, "@DiscontinueFlg", SqlDbType.TinyInt, me.DiscontinueFlg);
            AddParam(command, "@InventoryAddFlg", SqlDbType.TinyInt, me.InventoryAddFlg);
            AddParam(command, "@MakerAddFlg", SqlDbType.TinyInt, me.MakerAddFlg);
            AddParam(command, "@StoreAddFlg", SqlDbType.TinyInt, me.StoreAddFlg);
            AddParam(command, "@NoNetOrderFlg", SqlDbType.TinyInt, me.NoNetOrderFlg);
            AddParam(command, "@EDIOrderFlg", SqlDbType.TinyInt, me.EDIOrderFlg);
            AddParam(command, "@CatalogFlg", SqlDbType.TinyInt, me.CatalogFlg);
            AddParam(command, "@ParcelFlg", SqlDbType.TinyInt, me.ParcelFlg);
            AddParam(command, "@SoldOutFlg", SqlDbType.TinyInt, me.SoldOutFlg);
            AddParam(command, "@AutoOrderFlg", SqlDbType.TinyInt, me.AutoOrderFlg);
            AddParam(command, "@TaxRateFLG", SqlDbType.TinyInt, me.TaxRateFLG);
            AddParam(command, "@CostingKBN", SqlDbType.TinyInt, me.CostingKBN);
            AddParam(command, "@SaleExcludedFlg", SqlDbType.TinyInt, me.SaleExcludedFlg);
            AddParam(command, "@PriceWithTax", SqlDbType.Money, me.PriceWithTax);
            AddParam(command, "@PriceOutTax", SqlDbType.Money, me.PriceOutTax);
            AddParam(command, "@OrderPriceWithTax", SqlDbType.Money, me.OrderPriceWithTax);
            AddParam(command, "@OrderPriceWithoutTax", SqlDbType.Money, me.OrderPriceWithoutTax);
            AddParam(command, "@Rate", SqlDbType.Decimal, me.Rate);
            AddParam(command, "@SaleStartDate", SqlDbType.Date, me.SaleStartDate);
            AddParam(command, "@WebStartDate", SqlDbType.Date, me.WebStartDate);
            AddParam(command, "@OrderAttentionCD", SqlDbType.VarChar, me.OrderAttentionCD);
            AddParam(command, "@OrderAttentionNote", SqlDbType.VarChar, me.OrderAttentionNote);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, me.CommentInStore);
            AddParam(command, "@CommentOutStore", SqlDbType.VarChar, me.CommentOutStore);
            AddParam(command, "@ExhibitionSegmentCD", SqlDbType.VarChar, me.ExhibitionSegmentCD);
            AddParam(command, "@OrderLot", SqlDbType.VarChar, me.OrderLot);
            AddParam(command, "@LastYearTerm", SqlDbType.VarChar, me.LastYearTerm);
            AddParam(command, "@LastSeason", SqlDbType.VarChar, me.LastSeason);
            AddParam(command, "@LastCatalogNO", SqlDbType.VarChar, me.LastCatalogNO);
            AddParam(command, "@LastCatalogPage", SqlDbType.VarChar, me.LastCatalogPage);
            AddParam(command, "@LastCatalogText", SqlDbType.VarChar, me.LastCatalogText);
            AddParam(command, "@LastInstructionsNO", SqlDbType.VarChar, me.LastInstructionsNO);
            AddParam(command, "@LastInstructionsDate", SqlDbType.Date, me.LastInstructionsDate);
            AddParam(command, "@WebAddress", SqlDbType.VarChar, me.WebAddress);
            AddParam(command, "@ApprovalDate", SqlDbType.Date, me.ApprovalDate);
            AddParam(command, "@DeleteFlg", SqlDbType.TinyInt, me.DeleteFlg);
            AddParam(command, "@UsedFlg", SqlDbType.TinyInt, me.UsedFlg);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParamForDataTable(command, "@SiteTable", SqlDbType.Structured, dtSite);
            AddParam(command, "@Operator", SqlDbType.VarChar, me.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, me.PC);

            string outPutParam = "";
           UseTransaction = true;

            return InsertUpdateDeleteData(sp,ref outPutParam);
        }

        public DataTable M_ITEM_NormalSelect (M_ITEM_Entity mie)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ITemCD } },
                 { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mie.ChangeDate } },
            };
            return SelectData(dic, "M_ITEM_NormalSelect");
        }

        public DataTable M_ITem_SelectForSKUCDHenkou01(M_ITEM_Entity mie)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@Item", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ITemCD } },
                 { "@StartDate", new ValuePair { value1 = SqlDbType.Date, value2 = mie.ChangeDate } },
            };
            return SelectData(dic, "M_Sku_henkouSelect");
        }

        public DataTable M_Item_SelectForSKSMasterUpdate()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
             
            };
            return SelectData(dic, "M_Item_SelectForSKSMasterUpdate");
        }


        public bool M_ITem_SKSUpdateFlg(string xmlMasterItem, string xmlMasterSKU)
        {
            string sp = "M_ITem_SKSUpdateFlg";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xmlMasterItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = xmlMasterItem} },
                { "@xmlMasterSKU", new ValuePair { value1 = SqlDbType.VarChar, value2 = xmlMasterSKU} },
            };

            return InsertUpdateDeleteData(dic, sp);
        }

        public bool SKUUpdate(string xml, string xml_1, string OPD, string OPT, string OPTR, string PGM, string PC, string OPM, string KI,string Mode)
        {
            string sp = "M_SKUHenKou";
            if (!String.IsNullOrEmpty(Mode))
            {
                sp = "M_SKUHenKou_Insert";
            }
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@OPTDate", new ValuePair { value1 = SqlDbType.Date, value2 = OPD} },
                  { "@OPTTime", new ValuePair { value1 = SqlDbType.Time, value2 = OPT} },
                   { "@InsertOPT", new ValuePair { value1 = SqlDbType.VarChar, value2 = OPTR} },
                    { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = PGM} },
                     { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = PC} },
                  { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = OPM} },
                  { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = KI} },
                { "@xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = xml} },
                 { "@xml_1", new ValuePair { value1 = SqlDbType.VarChar, value2 = xml_1} },
            };
            return InsertUpdateDeleteData(dic, sp);
        }
    }

}
