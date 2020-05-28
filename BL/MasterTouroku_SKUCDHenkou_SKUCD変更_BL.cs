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
   public class MasterTouroku_SKUCDHenkou_SKUCD変更_BL:Base_BL
    {
        M_ITEM_DL midl;
        public MasterTouroku_SKUCDHenkou_SKUCD変更_BL()
        {
            midl = new M_ITEM_DL();
        }

        public DataTable M_ITEM_NormalSelect(M_ITEM_Entity mie)
        {
            return  midl.M_ITEM_NormalSelect(mie);
        }
          

    }
}
