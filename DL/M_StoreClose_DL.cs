using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_StoreClose_DL : Base_DL
    {
        public DataTable M_StoreClose_Select(M_StoreClose_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@FiscalYYYYMM",new ValuePair {value1=SqlDbType.Int,value2=mse.FiscalYYYYMM} }

            };

            return SelectData(dic, "M_StoreClose_Select");
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity mse, string Mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@Mode",new ValuePair {value1=SqlDbType.TinyInt,value2 = Mode} },
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@YYYYMM",new ValuePair {value1=SqlDbType.Int,value2=mse.FiscalYYYYMM} }
            };

            return SelectData(dic, "M_StoreClose_Check");
        }
    }
}
