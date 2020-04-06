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
    public class M_Site_DL : Base_DL
    {
        public DataTable M_Site_Select(M_Site_Entity mse)
        {
            string sp = "M_Site_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = mse.AdminNO } },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.APIKey } }
            };
            return SelectData(dic, sp);
        }

        public DataTable M_Site_SelectByItemCD(M_ITEM_Entity me)
        {
            string sp = "M_Site_SelectByItemCD";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ITemCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.ITemCD} } ,
                {"@ChangeDate",new ValuePair { value1=SqlDbType.VarChar,value2=me.ChangeDate} } ,
            };
            return SelectData(dic, sp);
        }

    }
}
