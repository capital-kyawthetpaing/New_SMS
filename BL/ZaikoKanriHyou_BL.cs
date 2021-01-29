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
  public  class ZaikoKanriHyou_BL : Base_BL
    {
        ZaikoKanriHyou_DL zkhdl;
        M_StoreClose_DL mscdl;

        public ZaikoKanriHyou_BL()
        {
            zkhdl = new ZaikoKanriHyou_DL();
            mscdl = new M_StoreClose_DL();
        }
        public DataTable RPC_ZaikoKanriHyou(D_Purchase_Details_Entity dpde,D_MonthlyStock_Entity dmse,int chk)
        {
            return zkhdl.RPC_ZaikoKanriHyou(dpde,dmse,chk);
        }
        /// <summary>
        /// Added by ETZ
        /// </summary>
        /// <param name="dpde"></param>
        /// <param name="dmse"></param>
        /// <param name="chk"></param>
        /// <returns></returns>
        public DataTable ZaikoKanriHyou_Export(D_Purchase_Details_Entity dpde, D_MonthlyStock_Entity dmse, int chk)
        {
            return zkhdl.ZaikoKanriHyou_Export(dpde, dmse, chk);
        }
        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }

    }
}
