using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_PayDetail_DL:Base_DL
    {
        D_Pay_Entity dpe = new D_Pay_Entity();

        public DataTable D_PayDetail_Select(D_Pay_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@LargePayNo", new ValuePair {value1 = SqlDbType.Int,value2 = dpe.LargePayNO} },
                {"@PayNo", new ValuePair {value1 = SqlDbType.Int,value2 = dpe.PayNo} }
            };
            return SelectData(dic, "D_PayDetail_Select");
        }
    }
}
