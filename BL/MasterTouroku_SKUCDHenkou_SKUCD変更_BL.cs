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
   public class MasterTouroku_SKUCDHenkou_SKUCD変更_BL:Base_BL
    {
        M_ITEM_DL midl;
        public MasterTouroku_SKUCDHenkou_SKUCD変更_BL()
        {
            midl = new M_ITEM_DL();
        }

        public DataTable M_ITEM_NormalSelect(M_ITEM_Entity mie)
        {
            return  midl.M_ITEM_NormalSelect(mie);
        }

        public DataTable M_ITem_SelectForSKUCDHenkou01(M_ITEM_Entity mie)
        {
            return midl.M_ITem_SelectForSKUCDHenkou01(mie);
        }
        public bool SKUUpdate(string xml, string xml_1,string OPD, string OPT, string OPTR, string PGM, string PC, string OPM, string KI,string mode=null)
        {
            return midl.SKUUpdate(xml,xml_1,OPD,OPT, OPTR, PGM,PC,OPM,KI,mode);
        }


    }
}
