using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class M_SKUInitial_BL:Base_BL
   {
        MasterTorikomi_SKU_DL mdl;


        public M_SKUInitial_BL()
        {
            mdl = new MasterTorikomi_SKU_DL();
        }

        public DataTable M_SKUInitial_SelectAll()
        {
             return mdl.M_SKUInitial_SelectAll();
        }

        public DataTable M_MessageSelectAll()
        {
            return mdl.M_MessageSelectAll();
        }
   }
}
