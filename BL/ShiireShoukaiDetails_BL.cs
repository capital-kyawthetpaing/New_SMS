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
  public  class ShiireShoukaiDetails_BL : Base_BL
    {
        D_Purchase_Details_DL dpd_dl;

        public ShiireShoukaiDetails_BL()
        {
            dpd_dl = new D_Purchase_Details_DL();
        }

        public DataTable ShiireShoukaiDetails_Select(D_Purchase_Details_Entity dpde)
        {
            return dpd_dl.ShiireShoukaiDetails_Select(dpde);
        }
    }
}
