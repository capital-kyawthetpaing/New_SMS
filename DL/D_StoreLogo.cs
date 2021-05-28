using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace DL
{
   public class D_StoreLogo : Base_DL
    {
        public DataTable GetLogo(string SCD)
        {

            string sp = "GetLogo";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =   SCD} }
            };

            return SelectData(dic, sp);
        }
    }
}
