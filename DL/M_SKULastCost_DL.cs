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
    public class M_SKULastCost_DL : Base_DL
    {
        public DataTable M_SKULastCost_Select(M_SKULastCost_Entity mse)
        {
            string sp = "M_SKULastCost_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@AdminNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.AdminNO } },
            };
            return SelectData(dic, sp);
        }

        //public DataTable M_SKU_SelectAll(M_SKU_Entity mse)
        //{
        //    string sp = "M_SKU_SelectAll";

        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //        {
        //            { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
        //            { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
        //            { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
        //        };
            
        //    return SelectData(dic,sp);
        //}

        //public DataTable M_SKU_SelectForSearchProduct(M_SKU_Entity mse, M_SKUInfo_Entity msie)
        //{
        //    string sp = "M_SKU_SelectForSearchProduct";

        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //        {
        //            { "@MainVendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MainVendorCD } },
        //            { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.BrandCD } },
        //            { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
        //            { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
        //            { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
        //            { "@CommentInStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.CommentInStore } },
        //            { "@ReserveCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ReserveCD } },
        //            { "@NoticesCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.NoticesCD } },
        //            { "@PostageCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.PostageCD } },
        //            { "@SportsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SportsCD } },
        //            { "@ClassificationA", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationA } },
        //            { "@ClassificationB", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationB } },
        //            { "@ClassificationC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ClassificationC } },

        //            { "@YearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.YearTerm } },
        //            { "@Season", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.Season } },
        //            { "@CatalogNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.CatalogNO } },
        //            { "@InstructionsNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = msie.InstructionsNO } },

        //        { "@InputDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InputDateFrom } },
        //        { "@InputDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.InputDateTo } },
        //        { "@UpdateDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.UpdateDateFrom } },
        //        { "@UpdateDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.UpdateDateTo } },
        //        { "@ApprovalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ApprovalDateFrom } },
        //        { "@ApprovalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ApprovalDateTo } },
        //            { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ChangeDate } },
        //            { "@SearchFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SearchFlg } },
        //            { "@ItemOrMaker", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.ItemOrMaker } },
        //        };

        //    return SelectData(dic, sp);
        //}


    }

}
