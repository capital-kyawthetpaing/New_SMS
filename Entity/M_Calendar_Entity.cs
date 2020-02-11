using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Entity
{
    public class M_Calendar_Entity : Base_Entity
    {
        public string CalendarDate { get;set;}
        public string DayKBN { get; set; }
        public string BankDayOff { get; set; }
        public string DayOff1 { get; set; }
        public string DayOff2 { get; set; }
        public string DayOff3 { get; set; }
        public string DayOff4 { get; set; }
        public string DayOff5 { get; set; }
        public string DayOff6 { get; set; }
        public string DayOff7 { get; set; }












        public string DayOff8 { get; set; }
        public string DayOff9 { get; set; }

        public DataTable dtDayOff { get; set; }
        public string DayOffXml { get; set; }
    }
}
