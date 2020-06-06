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
    public class M_ItemOrderPrice_DL : Base_DL
    {
        public DataTable M_ItemOrderPrice_Select(M_ItemOrderPrice_Entity mie)
        {
            string sp = "M_ItemOrderPrice_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.MakerItem } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.VendorCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ChangeDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.StoreCD } },
            };
            return SelectData(dic, sp);
        }

       
    } 

}
