using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class KeihiltiranHyou_BL:Base_BL
    {
        M_Calendar_DL mcdl = new M_Calendar_DL();
        D_Cost_DL dcdl = new D_Cost_DL();
        public DataTable CalendarSelect(M_Calendar_Entity mce)
        {
            return mcdl.M_Calendar_SelectCalencarDate(mce);
        }
        public DataTable getPrintData(D_Cost_Entity dce)
        {
            return dcdl.getPrintData(dce);
        }




    }
}
