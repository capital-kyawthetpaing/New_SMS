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
   public class Search_Carrier_BL:Base_BL
    {
        M_UnsouGaisya_DL mugdl = new M_UnsouGaisya_DL();

        public DataTable M_Carrier_Search(M_Shipping_Entity mse)
        {
            return mugdl.M_Carrier_Search(mse);
        }
    }
}
