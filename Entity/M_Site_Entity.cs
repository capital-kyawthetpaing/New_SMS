using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Site_Entity : Base_Entity
    {
        public string AdminNO { get; set; }
        public string ItemSKUCD { get; set; }
        public string APIKey { get; set; }
        //public string ItemSKUFLG {get;set;}
        public string SiteURL { get; set; }


    }
}
