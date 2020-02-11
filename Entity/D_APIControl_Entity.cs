using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_APIControl_Entity:Base_Entity
    {
        public string APIKey { get; set; }
        public string WebShopName { get; set; }
        public string State { get; set; }
        public string LastGetDateTime { get; set; }
    }
}
