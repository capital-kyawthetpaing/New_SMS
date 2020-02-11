using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_StorePayment_DL : Base_DL
    {
        public DataTable D_StorePayment_Select(D_StorePayment_Entity dse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.StoreCD } },
                   { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.SalesNO } },
               };
            return SelectData(dic, "D_StorePayment_Select");
        }

    }
}
