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
   public class SiiresakiZaikoYoteiHyou_BL : Base_BL
    {
        D_MonthlyPurchase_DL dmpdl;
        M_StoreClose_DL mscdl;
        public SiiresakiZaikoYoteiHyou_BL()
        {
            dmpdl = new D_MonthlyPurchase_DL();
            mscdl = new M_StoreClose_DL();
        }
        public DataTable RPC_SiiresakiZaikoYoteiHyou(D_MonthlyPurchase_Entity dmpe)
        {
            return dmpdl.RPC_SiiresakiZaikoYoteiHyou(dmpe);
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }
    }
  
}
