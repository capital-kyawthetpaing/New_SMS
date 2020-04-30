using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class JANCDHenkou_BL : Base_BL
    {
        M_SKU_DL skudl;
        public JANCDHenkou_BL()
        {
            skudl = new M_SKU_DL();
        }

       public DataTable M_SKU_JanCDHenkou_Select(string xml)
       {
            return skudl.M_SKU_JanCDHenkou_Select(xml);
       }
        
    }
}
