using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_JANOrderPrice_Entity : Base_Entity
    {
        public string JanCD {get;set;}
        public string VendorCD { get; set; }
        public string Rate { get; set; }
        public string PriceWithoutTax { get; set; }
        public string Remarks { get; set; }
    }
}
