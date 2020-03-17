using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;

namespace BL
{
    public class UrikakekinTairyuuHyou_BL:Base_BL
    {
        M_StoreClose_DL mscdl;
        D_MonthlyClaims_DL dmcdl;

        public UrikakekinTairyuuHyou_BL()
        {
            mscdl = new M_StoreClose_DL();
            dmcdl = new D_MonthlyClaims_DL();
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }

        public DataTable Select_DataToExport(M_StoreClose_Entity msce)
        {
            return dmcdl.UrikakekinTairyuuHyou_DataToExport(msce);
        }
    }
}
