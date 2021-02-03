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
    public class CarrierBoxSize_BL : Base_BL
    {
        M_CarrierBoxSize_DL mdl;
        public CarrierBoxSize_BL()
        {
            mdl = new M_CarrierBoxSize_DL();
        }

        public DataTable M_CarrierBoxSize_Bind(M_CarrierBoxSize_Entity mce)
        {
            return mdl.M_CarrierBoxSize_Bind(mce);
        }
    }
}
