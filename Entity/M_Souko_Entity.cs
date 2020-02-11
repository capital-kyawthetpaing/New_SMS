using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Souko_Entity : Base_Entity
    {
        public string SoukoCD {get;set;}
        public string SoukoCDFrom { get; set; }
        public string SoukoCDTo { get; set; }
        public string SoukoName { get; set; }
        public string StoreCD { get; set; }
        public string HikiateOrder { get; set; }
        public string SoukoType { get; set; }
        public string UnitPriceCalcKBN { get; set; }
        public string MakerCD { get; set; }
        public string MakerName { get; set; }
        public string IdouCount { get; set; }
        public string StoreIdouCount { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TelephoneNO { get; set; }
        public string FaxNO { get; set; }
        public string Remarks { get; set; }

        public string LocationXml { get; set; }

    }
}
