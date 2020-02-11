using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Search_BL : Base_BL
    {
        public DataTable M_Vendor_IsExists(M_Vendor_Entity mve)
        {
            M_Vendor_DL mvdl = new M_Vendor_DL();
            return mvdl.M_Vendor_IsExists(mve);
        }
    }
}
