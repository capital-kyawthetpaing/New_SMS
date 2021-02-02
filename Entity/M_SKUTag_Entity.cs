using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Entity
{
    public class M_SKUTag_Entity:Base_Entity
    {
        public string TagName1 { get; set; }
        public string TagName2 { get; set; }
        public string TagName3 { get; set; }
        public string TagName4 { get; set; }
        public string TagName5 { get; set; }
        public string CheckInfo { get; set; }

        public string ItemOpt { get; set; }
        public string ItemPC { get; set; }

        public string ItemProgram { get; set; }
    }
}
