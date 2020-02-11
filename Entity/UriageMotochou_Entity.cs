using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public  class UriageMotochou_Entity : Base_Entity
    {
        public string YYYYMMFrom { get; set; }
        public string YYYYMMTo { get; set; }
        public string CustomerCD { get; set; }
        public string StoreCD { get; set; }
        public string ChkValue { get; set; }
        public string TargetDateFrom { get; set; }
        public string TargetDateTo { get; set; }
    }
}
