using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;

namespace BL
{
    public class TempoRegiGenkaKakunin_BL:Base_BL
    {
        M_SKU_DL msku_dl;
        public TempoRegiGenkaKakunin_BL()
        {
            msku_dl = new M_SKU_DL();
        }

        public DataTable Select_M_SKU_Data(string janCD)
        {
          return  msku_dl.Select_M_SKU_Data(janCD);
        }

       
        public DataTable M_MakerVendor_DataSelect(TempoRegiZaikoKakunin_Entity kne)
        {
            return msku_dl.M_MakerVendor_DataSelect(kne);
        }
    }
}
