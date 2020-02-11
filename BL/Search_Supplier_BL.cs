using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
using DL;

namespace BL
{
   public class Search_Supplier_BL:Base_BL
    {
        M_Vendor_DL mvdl = new M_Vendor_DL();

        public DataTable M_Vendor_Search(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_Search(mve);
        }
    }
}
