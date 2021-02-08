using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public   class API_BL:Base_BL
    {
        MasterTorikomi_SKU_DL mtdl;
        public API_BL()
        {
            mtdl = new MasterTorikomi_SKU_DL();
        }

        public DataTable M_API_Select()
        {
            return mtdl.M_API_Select();
        }
    }
}
