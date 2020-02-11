using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DL;
using Entity;

namespace BL
{
    public class Search_SiharaiNO_BL:Base_BL
    {
        M_Vendor_DL mvdl = new M_Vendor_DL();
        D_Pay_DL dpdl = new D_Pay_DL();
        M_Vendor_Entity mve = new M_Vendor_Entity();

        public DataTable M_Vendor_Select(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_SelectForSiharaiNo(mve);
        }

        public DataTable D_Pay_SelectForSiharaiNo(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_SelectForSiharaiNo(dpe);
        }

    }
}
