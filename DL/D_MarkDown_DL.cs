using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.IO;

using System.Data.SqlClient;

namespace DL
{
    public class D_MarkDown_DL : Base_DL
    {
        public DataTable D_MarkDown_SelectAll(D_MarkDown_Entity dme)
        {
            string sp = "D_MarkDown_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@ChkNotAccount", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.ChkNotAccount } },
                { "@ChkAccounted", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.ChkAccounted } },
                { "@CostingDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CostingDateFrom } },
                { "@CostingDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CostingDateTo } },
                { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.PurchaseDateFrom } },
                { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.PurchaseDateTo } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_MarkDown_SelectData(D_MarkDown_Entity dme)
        {
            string sp = "D_MarkDown_SelectData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MarkDownNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MarkDownNO } },
            };

            return SelectData(dic, sp);
        }
    }
}
