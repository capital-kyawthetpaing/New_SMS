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
    public class Search_TempoUriageNO_BL : Base_BL
    {
        D_Sales_DL ddl;
        public Search_TempoUriageNO_BL()
        {
            ddl = new D_Sales_DL();
        }

        public DataTable D_Sales_SelectAll(D_Sales_Entity dse)
        {
            return ddl.D_Sales_SelectAll(dse);
        }

    }
}
