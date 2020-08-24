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
   public class MasterTouroku_HanbaiTankaKakeritu_BL:Base_BL
    {
        M_SKU_DL skudl;
        public MasterTouroku_HanbaiTankaKakeritu_BL()
        {
            skudl = new M_SKU_DL();
        }

        public DataTable Select_SKUData(M_SKU_Entity mskue,M_SKUPrice_Entity mskupe,string option)
        {
            return skudl.M_SKUDataForHanbaiTankaKakeritu(mskue, mskupe,option);
        }
    }
}
