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
    public class ZaikoYoteiHyou_BL:Base_BL
    {
        M_Staff_DL msdl = new M_Staff_DL();
        M_StoreAuthorizations_DL msadl = new M_StoreAuthorizations_DL();
        D_Order_DL dodl = new D_Order_DL();
        public DataTable M_Staff_Select(M_Staff_Entity mse)
        {
            return msdl.M_Staff_Select(mse);
        }

        public DataTable M_StoreAuthorizations_Select1(M_StoreAuthorizations_Entity msae)
        {
            return msadl.M_StoreAuthorizations_Select(msae);
        }

        public DataTable D_Order_Select(D_Order_Entity doe, D_Purchase_Entity dpe)
        {
            return dodl.D_Order_Select(doe, dpe);
        }

    }
}
