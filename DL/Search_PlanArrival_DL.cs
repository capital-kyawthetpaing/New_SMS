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
        public DataTable Search_PlanArrival(D_ArrivalPlan_Entity dap,M_SKU_Entity msku_entity,String adminno)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
               { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = msku_entity.ChangeDate} },
               { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dap.SoukoCD} },
               { "@AdminNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = adminno} },
             
            };
            return SelectData(dic, "Search_PlanArrival");
        }
    }
}
