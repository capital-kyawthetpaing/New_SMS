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
    public class MasterTouroku_Settouchi_BL : Base_BL
    {
        M_Store_DL MstoreDL;
        M_Prefix_DL MprefixDL;
        M_MultiPorpose_DL MmpDL;
        public MasterTouroku_Settouchi_BL()
        {
            MstoreDL = new M_Store_DL();
            MprefixDL = new M_Prefix_DL();
            MmpDL = new M_MultiPorpose_DL();
        }

        public DataTable BindStore(M_Store_Entity storedata)
        {
            return SimpleSelect1("25",storedata.ChangeDate);
        }

        public DataTable BindSeqKBN()
        {
            return SimpleSelect1("15",string.Empty,"304");
        }

        public DataTable ShowPrefix(M_Prefix_Entity prefix)
        {
            return MprefixDL.ShowPrefix(prefix);
        }

        public bool M_Prefix_Insert_Update(M_Prefix_Entity prefix, int mode)
        {
            return MprefixDL.M_Prefix_Insert_Update(prefix, mode);
        }

        public bool M_Prefix_Delete(M_Prefix_Entity prefix)
        {
            return MprefixDL.M_Prefix_Delete(prefix);
        }

        public DataTable StoreExist(string code,string changedate,string delflg)
        {
            return SimpleSelect1("27",changedate,code,delflg);
        }

        public DataTable prefixCheck(M_PrefixNumber_Entity prefixnumber)
        {
            return SimpleSelect1("18",string.Empty,prefixnumber.Prefix);
        }
    }
}
