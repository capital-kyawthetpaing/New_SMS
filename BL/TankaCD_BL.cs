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
    public class TankaCD_BL : Base_BL
    {
        M_TankaCD_DL mmdl;
        public TankaCD_BL()
        {
            mmdl = new M_TankaCD_DL();
        }

        public DataTable M_TankaCD_Select(M_TankaCD_Entity mke)
        {
            return mmdl.M_TankaCD_Select(mke);
        }

        public DataTable M_TankaCD_SelectAll(M_TankaCD_Entity mse)
        {
            return mmdl.M_TankaCD_SelectAll(mse);
        }

        //public bool TankaCD_Exec(M_Kouza_Entity mse, short operationMode, string operatorNm, string pc)
        //{
        //    return mmdl.M_TankaCD_Exec(mse, operationMode, operatorNm, pc);
        //}
    }
}
