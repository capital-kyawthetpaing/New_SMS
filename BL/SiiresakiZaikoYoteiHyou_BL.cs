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
        public SiiresakiZaikoYoteiHyou_BL()
        {
            dmpdl = new D_MonthlyPurchase_DL();
        }
        public DataTable RPC_SiiresakiZaikoYoteiHyou(D_MonthlyPurchase_Entity dmpe)
        {
            return dmpdl.RPC_SiiresakiZaikoYoteiHyou(dmpe);
        }
    }
  
}
