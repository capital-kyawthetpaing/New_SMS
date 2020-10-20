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
   public class MasterTouroku_TenzikaiShouhin_BL:Base_BL
   {
        MasterTouroku_TenzikaiShouhin_DL dl = new MasterTouroku_TenzikaiShouhin_DL();
       public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt)
       {
            return dl.Mastertoroku_Tenzikaishouhin_Select(mt);
       }
        public DataTable M_Tenzikaishouhin_SelectForJancd(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_Tenzikaishouhin_SelectForJancd(mt);
        }
        public DataTable M_SKU_SelectForSKUCheck(M_SKU_Entity msku)
        {
            return dl.M_SKU_SelectForSKUCheck(msku);
        }
    }
}
