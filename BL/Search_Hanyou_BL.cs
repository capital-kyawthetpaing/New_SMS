using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;

namespace BL
{
   public class Search_Hanyou_BL : Base_BL
    {
        M_Hanyou_DL mhdl;

        public Search_Hanyou_BL ()
        {
            mhdl = new M_Hanyou_DL();
        }

        public DataTable M_Hanyou_IDSearch(M_Hanyou_Entity mhe)
        {
            return mhdl.M_Hanyou_IDSearch(mhe);
        }

        public DataTable M_Hanyou_KeySearch(M_Hanyou_Entity mhe)
        {
            return mhdl.M_Hanyou_KeySearch(mhe);
        }
      
    }
}
