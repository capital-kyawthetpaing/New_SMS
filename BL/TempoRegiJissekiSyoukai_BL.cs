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
   public class TempoRegiJissekiSyoukai_BL : Base_BL
    {
        public TempoRegiJissekiSyoukai_BL()
        {
        }
        public DataTable D_Sales_Select(D_Sales_Entity de)
        {
            D_Sales_DL dl = new D_Sales_DL();
            return dl.D_Sales_SelectData_ForTempoRegiJissekiSyoukai(de);
        }

    }
}
