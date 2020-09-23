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
   public class MasterTouroku_CustomerSKUPrice_BL:Base_BL
    {
        M_CustomerSKUPrice_DL mcskupdl;
        public  MasterTouroku_CustomerSKUPrice_BL()
        {
            mcskupdl = new M_CustomerSKUPrice_DL();
        }
    }
}
