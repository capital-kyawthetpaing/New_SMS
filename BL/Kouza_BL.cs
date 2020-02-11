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
    public class Kouza_BL : Base_BL
    {
        M_Kouza_DL mmdl;
        public Kouza_BL()
        {
            mmdl = new M_Kouza_DL();
        }

        public DataTable M_Kouza_Select(M_Kouza_Entity mke)
        {
            return mmdl.M_Kouza_Select(mke);
        }

        public DataTable M_Kouza_SelectAll(M_Kouza_Entity mse)
        {
            return mmdl.M_Kouza_SelectAll(mse);
        }

        public DataTable M_Kouza_Bind(M_Kouza_Entity mse)
        {
            return mmdl.M_Kouza_Bind(mse);
        }

        //public bool Kouza_Exec(M_Kouza_Entity mse, short operationMode, string operatorNm, string pc)
        //{
        //    return mmdl.M_Kouza_Exec(mse, operationMode, operatorNm, pc);
        //}
    }
}
