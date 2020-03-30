using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class Search_PlanArrival_DL : Base_DL
   {
        public DataTable Search_PlanArrival(D_ArrivalPlan_Entity dap,M_SKU_Entity msku_entity,M_SKUTag_Entity msTag,M_SKUInfo_Entity msInfo_entity,D_Stock_Entity ds,String adminno,int Type)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
               { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.ChangeDate} },
               { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dap.SoukoCD} },
               { "@AdminNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = adminno} },
               { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.MainVendorCD } },
               { "@makerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.MakerVendorCD } },
               { "@brandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.BrandCD } },
               { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.SKUName } },
               { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.JanCD} },
               { "@SkuCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.SKUCD } },
               { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.MakerItem } },
               { "@ItemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.ITemCD} },
               { "@CommentInStore", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.CommentInStore } },
               { "@ReserveCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.ReserveCD } },
               { "@NticesCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.NoticesCD } },
               { "@PostageCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.PostageCD } },
               { "@OrderAttentionCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.OrderAttentionCD } },
               { "@SportsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku_entity.SportsCD } },
               { "@InsertDateTimeT", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.InputDateTo } },
               { "@InsertDateTimeF", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.InputDateFrom } },
               { "@UpdateDateTimeT", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.UpdateDateFrom } },
               { "@UpdateDateTimeF", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.UpdateDateTo } },
               { "@ApprovalDateF", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.ApprovalDateFrom } },
               { "@ApprovalDateT", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.ApprovalDateTo} },
               { "@YearTerm", new ValuePair { value1 = SqlDbType.VarChar, value2 = msInfo_entity.YearTerm } },
               { "@Season", new ValuePair { value1 = SqlDbType.VarChar, value2 = msInfo_entity.Season } },
               { "@CatalogNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = msInfo_entity.CatalogNO } },
               { "@InstructionsNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = msInfo_entity.InstructionsNO} },
               { "@TagName1", new ValuePair { value1 = SqlDbType.VarChar, value2 = msTag.TagName1} },
               { "@TagName2", new ValuePair { value1 = SqlDbType.VarChar, value2 = msTag.TagName2 } },
               { "@TagName3", new ValuePair { value1 = SqlDbType.VarChar, value2 = msTag.TagName3 } },
               { "@TagName4", new ValuePair { value1 = SqlDbType.VarChar, value2 = msTag.TagName4} },
               { "@TagName5", new ValuePair { value1 = SqlDbType.VarChar, value2 = msTag.TagName5 } },
               { "@type", new ValuePair { value1 = SqlDbType.Int, value2 = Type.ToString()} },
               { "@RackNoF", new ValuePair { value1 = SqlDbType.VarChar, value2 = ds.RackNOFrom } },
               { "@RackNoT", new ValuePair { value1 = SqlDbType.VarChar, value2 = ds.RackNOTo} },
            };
            return SelectData(dic, "Search_PlanArrival");
        }
    }
}
