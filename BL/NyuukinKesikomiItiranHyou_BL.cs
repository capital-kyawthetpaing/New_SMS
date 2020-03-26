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
    public class NyuukinKesikomiItiranHyou_BL : Base_BL
    {
        D_CollectPlan_DL collectDL;

        public NyuukinKesikomiItiranHyou_BL()
        {
            collectDL = new D_CollectPlan_DL();
        }

        public DataTable NyuukinKesikomiItiranHyou_Report(D_Collect_Entity collect_data)
        {
            return collectDL.NyuukinKesikomiItiranHyou_Report(collect_data);
        }
    }
}
