using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;

namespace BL
{
   public class TempoRegiRyougaeNyuuryoku_BL : Base_BL
    {
        D_DepositHistory_DL trrndl;
    
        public TempoRegiRyougaeNyuuryoku_BL()
        {
            trrndl = new D_DepositHistory_DL();
        }
        public bool TempoRegiRyougaeNyuuryoku_Insert_Update(D_DepositHistory_Entity mre)
        {
            return trrndl.TempoRegiRyougaeNyuuryoku_Insert(mre);
        }
     
    }
}
