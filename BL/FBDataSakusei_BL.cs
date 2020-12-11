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
    public class FBDataSakusei_BL:Base_BL
    {
        M_Calendar_DL mcdl = new M_Calendar_DL();
        M_Calendar_Entity mce = new M_Calendar_Entity();
        D_Pay_DL dpdl = new D_Pay_DL();
        D_Pay_Entity dpe = new D_Pay_Entity();

        public DataTable M_Calendar_SelectForFB(M_Calendar_Entity mce)
        {
           return mcdl.M_Calendar_SelectForFB(mce);
        }

        public DataTable D_Pay_SelectForFB(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_SelectForFB(dpe);
        }

    }
}
