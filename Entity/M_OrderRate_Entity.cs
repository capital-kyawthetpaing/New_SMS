using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public  class M_OrderRate_Entity:Base_Entity
    {
        public string VendorCD { get; set; }
        public string StoreCD { get; set; }
        public string BrandCD { get; set; }
        public string BrandName { get; set; }
        public string SportsCD { get; set; }
        public string SegmentCD { get; set; }
        public string LastYearTerm{ get; set; }
        public string LastSeason { get; set; }
        public string ChangeDate { get; set; }
        public string Rate { get; set; }
    }
}
