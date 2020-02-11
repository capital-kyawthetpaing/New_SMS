using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Carrier_Entity : Base_Entity
    {
        public string CarrierCD {get;set;}
        public string CarrierName { get; set; }
        public string CarrierFlg { get; set; }
        public string YahooCarrierCD { get; set; }
        public string RakutenCarrierCD { get; set; }
        public string AmazonCarrierCD { get; set; }
        public string WowmaCarrierCD { get; set; }
        public string PonpareCarrierCD { get; set; }

    }
}
