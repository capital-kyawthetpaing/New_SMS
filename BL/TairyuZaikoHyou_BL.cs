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
   public class TairyuZaikoHyou_BL:Base_BL
    {
        M_MultiPorpose_DL mmpdl = new M_MultiPorpose_DL();
        M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        M_StoreClose_DL mscdl = new M_StoreClose_DL();
        D_Stock_DL dsdl = new D_Stock_DL();

        public DataTable M_Multiporpose_CharSelect(M_MultiPorpose_Entity mmpe)
        {
            return mmpdl.M_MultiPorpose_Select(mmpe);
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }

        public DataTable D_StockSelectForTairyuzaikohyo(D_Stock_Entity dse,M_SKU_Entity mke,M_SKUInfo_Entity info,M_SKUTag_Entity tage)
        {
           return dsdl.D_StockSelectForTairyuzaikohyo(dse, mke, info, tage);
        }
    }
}
