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
    public class TempoRegiNyuukinTouroku_BL : Base_BL
    {
        D_DepositHistory_DL ddhdl = new D_DepositHistory_DL();
        public bool TempoNyuukinTouroku_InsertUpdate(D_DepositHistory_Entity ddpe)
        {
            return ddhdl.D_DepositＨistory_Insert_Update(ddpe);
        }
    }
}
