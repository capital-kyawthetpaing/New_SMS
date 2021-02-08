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
    public class M_CarrierBoxSize_DL : Base_DL
    { 

        public DataTable M_CarrierBoxSize_Bind(M_CarrierBoxSize_Entity mke)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CarrierCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mke.CarrierCD } },
            };
            return SelectData(dic, "M_CarrierBoxSize_Bind");
        }
    }
}
