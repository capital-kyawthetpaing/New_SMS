using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class TempoRegiTsurisenJyunbi_BL : Base_BL
    {
        D_DepositHistory_DL trrndl;
        Base_BL bbl = new Base_BL();

        public TempoRegiTsurisenJyunbi_BL()
        {
            trrndl = new D_DepositHistory_DL();
        }
        public bool TempoRegiTsurisenJyunbi_Insert_Update(D_DepositHistory_Entity mre)
        {
            return trrndl.D_DepositＨistory_Insert_Update(mre);
        }
    }
}
