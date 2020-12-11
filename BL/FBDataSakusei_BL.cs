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

        public DataTable M_Calendar_SelectForFB(M_Calendar_Entity mce)
        {
           return mcdl.M_Calendar_SelectForFB(mce);
        }

    }
}
