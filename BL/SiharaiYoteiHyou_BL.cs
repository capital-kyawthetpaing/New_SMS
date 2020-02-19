using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
    public class SiharaiYoteiHyou_BL:Base_BL
    {
        D_PayPlan_DL dpp_dl;
        public SiharaiYoteiHyou_BL()
        {
            dpp_dl = new D_PayPlan_DL();
        }
        public DataTable D_PayPlan_SelectForPrint(D_PayPlan_Entity dppe, int type)
        {
            return dpp_dl.D_PayPlan_SelectForPrint(dppe, type);
        }
    }
}
