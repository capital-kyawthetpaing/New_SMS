using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_JANOrderPrice_Entity : Base_Entity
    {
        public string JanCD {get;set;}  //削除
        public string VendorCD { get; set; }
        public string StoreCD { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }

        public string Rate { get; set; }
        public string PriceWithoutTax { get; set; }
        public string Remarks { get; set; }
    }
}
