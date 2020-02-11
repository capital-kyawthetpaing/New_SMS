using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;

namespace BL
{
    public class MasterTouroku_Calendar_BL : Base_BL
    {
        M_Calendar_DL mcdl;
        public MasterTouroku_Calendar_BL()
        {
            mcdl = new M_Calendar_DL();

        }

        public DataTable M_Calendar_Select(string month)
        {
            return mcdl.M_Calendar_Select(month);
        }

        public DataTable M_Calendar_SelectDayKBN(string date)
        {
            return mcdl.M_Calendar_SelectDayKBN(date);
        }

        public bool M_Calendar_Insert_Update(M_Calendar_Entity mce)
        {
            mce.DayOffXml = DataTableToXml(mce.dtDayOff);
            return mcdl.M_Calendar_Insert_Update(mce);
        }
    }
}
