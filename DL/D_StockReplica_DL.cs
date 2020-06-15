using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_StockReplica_DL : Base_DL
    {
        public DataTable D_StockReplica_Bind()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();
            return SelectData(dic, "D_StockReplica_Bind");
        }        
    }
}
