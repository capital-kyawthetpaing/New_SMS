using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class MasterTorikomi_SKU_BL:Base_BL
   {
        M_SKUCounter_DL sdl;
        M_JANCounter_DL mdl;

        public MasterTorikomi_SKU_BL()
        {
            sdl = new M_SKUCounter_DL();
            mdl = new M_JANCounter_DL();
        }


        public DataTable M_SKUCounter_Update()
        {
            return sdl.M_SKUCounter_Update();
        }

        public DataTable M_JANCounter_JanContUpdate()
        {
            return mdl.M_JANCounter_JanContUpdate();
        }
    }
}
