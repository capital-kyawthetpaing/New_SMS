using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
    public class D_Warehousing_DL : Base_DL
    {
        public DataTable ZaikoMotochoulnsatsu_Report(M_SKU_Entity mse, D_MonthlyStock_Entity dms, int chk)
        {
            string sp = "ZaikoMotochoulnsatsu_Report";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@YYYYMMFrom", new ValuePair { value1 = SqlDbType.Int, value2 =  dms.YYYYMMFrom} },
                { "@YYYYMMTo", new ValuePair { value1 = SqlDbType.Int, value2 = dms.YYYYMMTo } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dms.SoukoCD  } },
                { "@itemcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@sku", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD  } },
                { "@jan", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD  } },
                { "@makeritem", new ValuePair { value1 = SqlDbType.VarChar, value2 =mse.MakerItem } },
                { "@skuName", new ValuePair { value1 = SqlDbType.VarChar, value2 =  mse.SKUName } },
                { "@related", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  chk.ToString() } },
                { "@targetDateFrom", new ValuePair { value1 = SqlDbType.Date, value2 = dms.TargetDateFrom} },
                { "@targerDateTo", new ValuePair { value1 = SqlDbType.Date, value2 =  dms.TargetDateTo} }
            };

            return SelectData(dic, sp);
        }
    }
}
