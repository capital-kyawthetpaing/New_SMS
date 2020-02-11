using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DL;
using Entity;

namespace BL
{
    public class SaimuKanriHyou_BL : Base_BL
    {
        D_MonthlyDebt_DL dmdl;
        M_StoreClose_DL mscdl;
        public SaimuKanriHyou_BL()
        {
            dmdl = new D_MonthlyDebt_DL();
            mscdl = new M_StoreClose_DL();
        }
            
        public DataTable D_MonthlyDebt_CSV_Report(D_MonthlyDebt_Entity dme, int chk)
        {
            return dmdl.D_MonthlyDebt_CSV_Report(dme, chk);
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce,string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }
    }
}
