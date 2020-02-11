using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace DL
{
   public class TempoRegiFurikomiYoushi_DL : Base_DL
    {

        
        public DataTable SelectPrintData(string printno,string lblStoreName)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@StoreName", new ValuePair { value1 = SqlDbType.VarChar, value2 = lblStoreName } },
                   { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = printno } }
               };
            return SelectData(dic, "SelectPrintData");

        }

    }
}
