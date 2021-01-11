using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;

namespace BL
{
   public class ZaikoShoukai_BL
   {
        ZaikoShoukai_DL zaidl = new ZaikoShoukai_DL();
        M_Souko_DL msdl = new M_Souko_DL();
        public DataTable ZaikoShoukai_Search(M_SKU_Entity msku,M_SKUInfo_Entity msInfo,M_SKUTag_Entity mstag, D_Stock_Entity ds_Entity,int Type,int chktype,int chkunchekApprove)
        {
            return zaidl.ZaikoShoukai_Search(msku, msInfo, mstag, ds_Entity,Type,chktype,chkunchekApprove);
        }

        public DataTable M_Souko_BindForZaikoshoukai(M_Souko_Entity ms)
        {
            return msdl.M_Souko_BindForZaikoshoukai(ms);
        }
    }
}
