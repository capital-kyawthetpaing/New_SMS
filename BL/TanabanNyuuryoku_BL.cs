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
   public class TanabanNyuuryoku_BL:Base_BL
    {
        M_Souko_DL msdl = new M_Souko_DL();
        M_Location_DL mldl = new M_Location_DL();
   
        public DataTable D_Souko_Select(M_Souko_Entity mse)
        {
            return msdl.M_SoukoWarehouse_Select(mse);
        }

        public DataTable M_LocationTana_Select(M_Location_Entity mle)
        {
            return mldl.M_LocationTana_Select(mle);
        }

        public DataTable M_Location_DataSelect(D_Stock_Entity dse)
        {
            return mldl.M_Location_DataSelect(dse);
        }

        public bool M_Location_InsertUpdate(D_Stock_Entity dse,M_Location_Entity mle)
        {           
                dse.xml1 = DataTableToXml(dse.dt1);
            return mldl.M_Location_InsertUpdate(dse, mle);
        }
    }
}
