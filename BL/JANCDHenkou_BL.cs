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
        D_JANUpdate_DL janupdl;
        public JANCDHenkou_BL()
        {
            skudl = new M_SKU_DL();
            janupdl = new D_JANUpdate_DL();
        }

       public DataTable M_SKU_JanCDHenkou_Select(string xml)
       {
            return skudl.M_SKU_JanCDHenkou_Select(xml);
       }
        
        public bool JanCDHenkou_Insert(string xml, L_Log_Entity log_data)
        {
            return janupdl.JanCDHenkou_Insert(xml, log_data);
        }
    }
}
