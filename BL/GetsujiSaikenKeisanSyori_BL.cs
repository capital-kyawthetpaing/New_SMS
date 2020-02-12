using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;

namespace BL
{
  public  class GetsujiSaikenKeisanSyori_BL : Base_BL
    {
        D_MonthlyClaims_DL dmc_dl;
        public GetsujiSaikenKeisanSyori_BL()
        {
            dmc_dl = new D_MonthlyClaims_DL();
        }

    }
}
