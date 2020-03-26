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
    public class ZaikoMotochouInsatsu_BL : Base_BL
    {
        D_Warehousing_DL dwdl;
        M_StoreClose_DL mscdl;
        public ZaikoMotochouInsatsu_BL()
        {
            dwdl = new D_Warehousing_DL();
            mscdl = new M_StoreClose_DL();
        }

        public DataTable ZaikoMotochoulnsatsu_Report(M_SKU_Entity mse, D_MonthlyStock_Entity dms, int chk)
        {
            return dwdl.ZaikoMotochoulnsatsu_Report(mse, dms, chk);
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }
    }
}
