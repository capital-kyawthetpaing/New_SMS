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
    public class Search_Souko_BL : Base_BL
    {
        M_Store_DL msdl;
        M_Souko_DL msoukodl;
        public Search_Souko_BL()
        {
            msdl = new M_Store_DL();
            msoukodl = new M_Souko_DL();
        }

        public DataTable M_Store_Bind(M_Store_Entity mse)
        {
            return msdl.M_Store_Bind(mse);
        }

        public DataTable M_Souko_Search(M_Souko_Entity mse)
        {
            return msoukodl.M_Souko_Search(mse);
        }

        public DataTable M_Souko_Bind(M_Souko_Entity mske)
        {
            return msoukodl.M_Souko_Bind(mske);
        }

        public DataTable M_Souko_BindAll(M_Souko_Entity mse)
        {
            return msoukodl.M_Souko_BindAll(mse);
        }

        public DataTable M_Souko_AllBind(M_Souko_Entity mse)
        {
            return msoukodl.M_Souko_AllBind(mse);
        }
    }
}
