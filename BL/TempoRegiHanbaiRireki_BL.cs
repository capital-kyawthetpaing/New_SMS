using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
   public class TempoRegiHanbaiRireki_BL : Base_BL
    {
        public TempoRegiHanbaiRireki_BL()
        {
        }

        public DataTable D_Juchu_Select(D_Juchuu_Entity de)
        {
            D_Juchuu_DL dl = new D_Juchuu_DL();
            return dl.D_Juchuu_SelectData_ForTempoRegiHanbaiRireki(de);
        }
        public DataTable D_Sales_Select(D_Sales_Entity de)
        {
            D_Sales_DL dl = new D_Sales_DL();
            return dl.D_Sales_SelectData_ForTempoRegiHanbaiRireki(de);
        }


    }
}
