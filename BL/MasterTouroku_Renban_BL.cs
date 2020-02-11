using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
    public class MasterTouroku_Renban_BL : Base_BL
    {
        M_Renban_DL mrdl;
        public MasterTouroku_Renban_BL()
        {
            mrdl = new M_Renban_DL();
        }
        public bool M_Renban_Insert_Update(M_Renban_Entity mre, int mode)
        {
            return mrdl.M_Renban_Insert_Update(mre, mode);
        }
        public bool M_Renban_Delete(M_Renban_Entity mre)
        {

            return mrdl.M_Renban_Delete(mre);
        }
        public M_Renban_Entity M_Renban_Select(M_Renban_Entity mre)
        {
            DataTable dtRenban = mrdl.M_Renban_Select(mre);
            if (dtRenban.Rows.Count > 0)
            {
                mre.PrefixValue = dtRenban.Rows[0]["Prefix"].ToString();
                mre.Continuous = dtRenban.Rows[0]["SeqNumber"].ToString();
                return mre;
            }
            return null;
        }
        public bool M_Renban_Exists(string prefixNo)
        {
            DataTable dtSouko = SimpleSelect1("18", string.Empty, prefixNo);
            return dtSouko.Rows.Count > 0 ? true : false;
        }
    }
}
