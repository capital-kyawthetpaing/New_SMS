using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
   public class TempoShukkaNyuuryoku_BL : Base_BL
    {
        public TempoShukkaNyuuryoku_BL()
        {
        }

        public DataTable D_Juchu_Select(D_Juchuu_Entity de, short operationMode, string salesNo = "")
        {
            D_Juchuu_DL dl = new D_Juchuu_DL();
            return dl.D_Juchuu_SelectData_ForTempoShukkaNyuuryoku(de, operationMode, salesNo);
        }

        public bool PRC_TempoShukkaNyuuryoku(D_Sales_Entity dse, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            TempoShukkaNyuuryoku_DL dl = new TempoShukkaNyuuryoku_DL();
            return dl.PRC_TempoShukkaNyuuryoku(dse, dt, operationMode, operatorNm, pc);

        }

    }
}
