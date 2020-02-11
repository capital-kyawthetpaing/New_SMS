using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;

namespace BL
{
    public class ShiireShoukaiShiiresaki_BL :Base_BL
    {
        D_Purchase_DL dpur_dl;
        M_Vendor_DL mv_dl;
        public ShiireShoukaiShiiresaki_BL()
        {
            dpur_dl = new D_Purchase_DL();
            mv_dl = new M_Vendor_DL();
        }
        public DataTable D_Purchase_Search(D_Purchase_Entity dpure)
        {
           
            return dpur_dl.D_Purchase_Search(dpure);
        }
        public String SelectPayeeFlg(M_Vendor_Entity mve)
        {
            DataTable dt = mv_dl.M_Vendor_PayeeFlg_Select(mve);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["PayeeFlg"].ToString();
            }
            return string.Empty;
        }
    }
}
