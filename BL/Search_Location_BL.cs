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
    public class Search_Location_BL:Base_BL
    {
        M_Location_DL mldl = new M_Location_DL();
        M_Souko_DL msdl = new M_Souko_DL();
        public DataTable M_SoukoName_Select(M_Souko_Entity mse)
        {
            return msdl.M_SoukoName_Select(mse);
        }

        public DataTable M_Location_Search(M_Location_Entity mle)
        {
            return mldl.M_Location_Search(mle);
        }
    }
}
