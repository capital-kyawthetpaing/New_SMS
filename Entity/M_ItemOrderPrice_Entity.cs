using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_ItemOrderPrice_Entity : Base_Entity
    {
        public string MakerItem {get;set;}
        public string VendorCD { get; set; }
        public string StoreCD { get; set; }
        public string Rate { get; set; }
        public string PriceWithoutTax { get; set; }
        public string Remarks { get; set; }
        public string Display { get; set; }

        public string InsertOperator { get; set; }
        public string Headerdate { get; set; }
    }
}
