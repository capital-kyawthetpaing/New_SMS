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
    public class TempoRegiNyuukinNyuuryoku_BL : Base_BL
    {
        D_DepositHistory_DL ddhdl;
        D_Collect_DL coldl;
       
        public TempoRegiNyuukinNyuuryoku_BL()
        {
            ddhdl = new D_DepositHistory_DL();
            coldl = new D_Collect_DL();
        }
        public bool TempoNyuukinTouroku_D_DepositHistory_InsertUpdate(D_DepositHistory_Entity ddpe)
        {
            return ddhdl.D_DepositＨistory_Insert_Update(ddpe);
        }

        public bool TempoNyuukinTouroku_D_Collect_Insert(D_Collect_Entity dce)
        {
            return coldl.D_Collect_Insert(dce);
        }
    }
}
