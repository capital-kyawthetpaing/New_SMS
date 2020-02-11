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
   public class TanaireList_BL:Base_BL
    {
        D_Stock_DL dsdl;
        public TanaireList_BL()
        {
            dsdl = new D_Stock_DL();
        }

        public DataTable SelectDataForPrint(D_Stock_Entity dse,string option)
        {
            return dsdl.SelectDataForPrint(dse,option);
        }

    }
}
