using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_SKUInfo_Entity : Base_Entity
    {
        public string AdminNO { get; set; }
       // public string SKUCD { get; set; }
        public string SEQ { get; set; }
        //public string DataFLG { get; set; }
        public string YearTerm { get; set; }
        public string Season { get; set; }
        public string CatalogNO { get; set; }
        public string CatalogPage { get; set; }
        public string CatalogText { get; set; }
        public string InstructionsNO { get; set; }
        public string InstructionsDate { get; set; }
        public string CustomerCD { get; set; }
    }
}
