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
    public class M_JANOrderPrice_DL : Base_DL
    {
        public DataTable M_JANOrderPrice_Select(M_JANOrderPrice_Entity mje)
        {
            string sp = "M_JANOrderPrice_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mje.AdminNO } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mje.VendorCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mje.ChangeDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mje.StoreCD } },
            };
            return SelectData(dic, sp);
        }
    }

}
