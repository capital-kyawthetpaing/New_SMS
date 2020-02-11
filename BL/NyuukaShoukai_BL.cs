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
    public class NyuukaShoukai_BL:Base_BL
    {
        D_ArrivalPlan_DL dapdl = new D_ArrivalPlan_DL();
        M_Souko_DL msdl = new M_Souko_DL();
        M_Vendor_DL mvdl = new M_Vendor_DL();

        public DataTable D_ArrivalPlan_Select(D_ArrivalPlan_Entity dape,D_Arrival_Entity dae,D_Purchase_Entity dpe)
        {
            return dapdl.D_ArrivalPlan_Select(dape, dae, dpe);
        }

       public DataTable D_Souko_Select(M_Souko_Entity mse)
        {
            return msdl.M_SoukoWarehouse_Select(mse);
        }

        public DataTable M_Vendor_DataSelect(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_DataSelect(mve);
        }
    }
}
