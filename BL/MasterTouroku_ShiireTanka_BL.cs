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
   public class MasterTouroku_ShiireTanka_BL
    {
        MasterTouroku_ShiireTanka_DL dl=new MasterTouroku_ShiireTanka_DL();
        public DataTable M_ItemOrderPrice_Insert(M_ItemOrderPrice_Entity mie,M_ITEM_Entity mi)
        {
            return dl.M_ItemOrderPrice_Insert(mie,mi);
        }
        public DataTable  M_ITem_ItemNandPriceoutTax_Select( M_ITEM_Entity mi)
        {
            return dl.M_ITem_ItemNandPriceoutTax_Select(mi);
        }
    }
}
