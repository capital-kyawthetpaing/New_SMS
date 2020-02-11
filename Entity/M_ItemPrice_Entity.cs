using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_ItemPrice_Entity : Base_Entity
    {
        public string ITemCD {get;set;}
        public string PriceWithTax { get; set; }
        public string PriceWithoutTax { get; set; }
        public string GeneralRate { get; set; }
        public string GeneralPriceWithTax { get; set; }
        public string GeneralPriceOutTax { get; set; }
        public string MemberRate { get; set; }
        public string MemberPriceWithTax { get; set; }
        public string MemberPriceOutTax { get; set; }
        public string ClientRate { get; set; }
        public string ClientPriceWithTax { get; set; }
        public string ClientPriceOutTax { get; set; }
        public string SaleRate { get; set; }
        public string SalePriceWithTax { get; set; }
        public string SalePriceOutTax { get; set; }
        public string WebRate { get; set; }
        public string WebPriceWithTax { get; set; }
        public string WebPriceOutTax { get; set; }
        public string Remarks { get; set; }


        //検索用
        public string SyoKBN { get; set; }
        public string ItemFrom { get; set; }
        public string ItemTo { get; set; }
        public string BrandCD { get; set; }
        public string ITemName { get; set; }

    }
}
