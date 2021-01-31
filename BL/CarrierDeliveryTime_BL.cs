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
    public class CarrierDeliveryTime_BL : Base_BL
    {
        M_CarrierDeliveryTime_DL mdl;
        public CarrierDeliveryTime_BL()
        {
            mdl = new M_CarrierDeliveryTime_DL();
        }

        public DataTable M_CarrierDeliveryTime_Bind(M_CarrierDeliveryTime_Entity mce)
        {
            return mdl.M_CarrierDeliveryTime_Bind(mce);
        }
    }
}
