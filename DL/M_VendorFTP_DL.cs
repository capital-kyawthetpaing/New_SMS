using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class M_VendorFTP_DL : Base_DL
    {
        public DataTable M_VendorFTP_Select(M_VendorFTP_Entity mve)
        {
            string sp = "M_VendorFTP_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =mve.VendorCD  } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mve.ChangeDate } }
            };
            return SelectData(dic, sp);
        }

        public DataTable M_VendorFTP_ForSelectFile()
        {
            string sp = "M_VendorFTP_ForSelectFile";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
              
            };
            return SelectData(dic, sp);
        }
    }
}
