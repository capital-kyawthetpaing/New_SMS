using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_ShippingDetails_Entity:Base_Entity
    {
        public string Number { get; set; }
        public string SKUCD { get; set; }
        public string JanCD { get; set; }
        public string ItemCD { get; set;}

    }
}
