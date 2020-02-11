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
                    { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                    { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
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
    }

}
