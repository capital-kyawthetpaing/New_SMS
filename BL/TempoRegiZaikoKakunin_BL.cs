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
   public class TempoRegiZaikoKakunin_BL:Base_BL
    {
        D_Stock_DL dsdl = new D_Stock_DL();
        M_SKU_DL msku_dl = new M_SKU_DL();
        public DataTable D_Stock_DataSelect(TempoRegiZaikoKakunin_Entity kne)
        {
            return dsdl.D_Stock_DataSelect(kne);
        }

        public DataTable Select_M_SKU_Data(string janCD)
        {
            return msku_dl.Select_M_SKU_Data(janCD);
        }
    }
}
