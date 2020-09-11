using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
   public class D_Order_DL:Base_DL
    {
        public DataTable D_Order_Select(D_Order_Entity doe, D_Purchase_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
               {"@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.DestinationSoukoCD } },
               {"@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2= doe.StoreCD} },
               {"@TargetDateFrom" , new ValuePair {value1 = SqlDbType.VarChar ,value2 = dpe.PurchaseDateFrom} },
               {"@TargetDateTo", new ValuePair {value1 = SqlDbType.VarChar ,value2 = dpe.PurchaseDateTo} },
            };
            UseTransaction = true;
            return SelectData(dic, "D_Order_SelectForReport");
        }
        public DataTable D_Order_SelectByOrderProcessNO(string orderProcessNO)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
               {"@p_OrderProcessNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderProcessNO } },
            };
            UseTransaction = true;
            return SelectData(dic, "D_Order_SelectByOrderProcessNO");
        }
    }
}
