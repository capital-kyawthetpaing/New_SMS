using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;

namespace BL
{
   public  class TempoRegiShiharaiNyuuryoku_BL:Base_BL
    {
        D_DepositHistory_DL ddhdl = new D_DepositHistory_DL();
        public bool TempoRegiShiNyuuryoku_InsertUpdate(D_DepositHistory_Entity ddpe)
        {
            return ddhdl.D_DepositＨistory_Insert_Update(ddpe);
        }
    }
}
