using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class NyuukinYoteiHyou_BL : Base_BL
    {
        D_CollectPlan_DL mdl;
        public NyuukinYoteiHyou_BL()
        {
            mdl = new D_CollectPlan_DL();
        }

        public DataTable D_CollectPlan_SelectForPrint(D_CollectPlan_Entity dce, M_Customer_Entity mce)
        {
            return mdl.D_CollectPlan_SelectForPrint(dce, mce);
        }
    }
}
