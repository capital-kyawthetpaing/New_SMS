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
    public class TenzikaiJuchuuNo_BL:Base_BL
    {
        M_Customer_DL mcdl = new M_Customer_DL();
        M_Customer_Entity mce = new M_Customer_Entity();
        D_TenzikaiJuchuu_DL dtjdl = new D_TenzikaiJuchuu_DL();
        M_Vendor_DL mvdl = new M_Vendor_DL();
        public DataTable M_Customer_SelectForTenzikai(M_Customer_Entity mce)
        {
            return mcdl.M_Customer_SelectForTenzikai(mce);
        }

        public DataTable D_TenzikaiJuchuu_SearchData(D_TenzikaiJuchuu_Entity dtje)
        {
            return dtjdl.D_TenzikaiJuchuu_SearchData(dtje);
        }
        public DataTable M_Vendor_SelectForJuchuu(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_SelectForJuchuu(mve);
        }
    }
}
