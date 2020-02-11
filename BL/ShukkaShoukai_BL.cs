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
   public class ShukkaShoukai_BL:Base_BL
    {
        D_Juchuu_DL djdl = new D_Juchuu_DL();
        M_StoreAuthorizations_DL msadl = new M_StoreAuthorizations_DL();
        D_Shipping_DL dsdl = new D_Shipping_DL();
        D_Juchuu_Entity dje = new D_Juchuu_Entity();
        M_StoreAuthorizations_Entity msae = new M_StoreAuthorizations_Entity();
        D_ShippingDetails_Entity dsde = new D_ShippingDetails_Entity();
        D_Shipping_Entity dhse = new D_Shipping_Entity();
        D_Instruction_Entity die = new D_Instruction_Entity();
        
        public DataTable D_Juchuu_DataSelect_ForShukkaShoukai(D_Juchuu_Entity dje)
        {
            return djdl.D_Juchuu_DataSelect_ForShukkaShoukai(dje);
        }

        public DataTable M_StoreAuthorizations_Select1(M_StoreAuthorizations_Entity msae)
        {
            return msadl.M_StoreAuthorizations_Select(msae);
        }

        public DataTable D_Shipping_Select(D_Shipping_Entity dhse, D_ShippingDetails_Entity dsde ,D_Instruction_Entity die)
        {
            return dsdl.D_Shipping_Select(dhse, dsde, die);
        }
    }
}
