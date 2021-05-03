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
   public class MasterTorikomi_SKUPrice_BL : Base_BL
    {
        M_SKUPrice_DL mskupdl;
        M_TankaCD_DL mtkdl;
        M_Store_DL msdl;
        public MasterTorikomi_SKUPrice_BL()
        {
            mskupdl = new M_SKUPrice_DL();
            mtkdl = new M_TankaCD_DL();
            msdl = new M_Store_DL();
        }

        public bool MasterTorikomi_SKUPrice_Insert_Update( M_SKUPrice_Entity mskup)
        {
            mskup.xml1 = DataTableToXml(mskup.dt1);
            return mskupdl.M_SKUPrice_InsertUpdate(mskup);
        }
        public DataTable M_TankaCD_SelectAll_NoPara()
        {
            return mtkdl.M_TankaCD_SelectAll_NoPara();
        }

        public DataTable M_StoreCD_SelectAll_NoPara()
        {
            return msdl.M_StoreCD_SelectAll_NoPara();
        }
    }
}
