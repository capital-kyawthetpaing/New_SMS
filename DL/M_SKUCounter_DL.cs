using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
   public class M_SKUCounter_DL:Base_DL
    {
        public DataTable M_SKUCounter_Update()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, "M_SKUCounter_Update");
        }
    }
}
