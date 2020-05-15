using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Shipping_Entity : Base_Entity
    {
        public string ShippingCD { get; set; }
        public string ShippingCDFrom { get; set; }
        public string ShippingCDTo { get; set; }
        public string ShippingName { get; set; }
        public string CarrierFlg { get; set; }
        public string YahooCD { get; set; }
        public string RakutenCD { get; set; }
        public string AmazonCD { get; set; }
        public string WowmaCD { get; set; }
        public string PonpareCD { get; set; }
        public string OtherCD { get; set; }
        public string Remarks { get; set; }
        public string DisplayKBN { get; set; }
        public string NormalFlg { get; set; }
    }
}
