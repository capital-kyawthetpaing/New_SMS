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
   public class Search_PlanArrival_BL
    {
        Search_PlanArrival_DL pa_dl;
        public Search_PlanArrival_BL()
        {
            pa_dl = new Search_PlanArrival_DL();
        }
        public DataTable Search_PlanArrival(D_ArrivalPlan_Entity dap,M_SKU_Entity ms,String adminno)
        {
            return pa_dl.Search_PlanArrival(dap,ms,adminno);
        }
    }
}
