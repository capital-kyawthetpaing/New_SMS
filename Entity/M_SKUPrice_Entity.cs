using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_SKUPrice_Entity : Base_Entity
    {
        public string DisplayKBN { get; set; }
        public string TankaCD {get;set;}
        public string StoreCD { get; set; }
        public string AdminNO { get; set; }
        public string SKUCD { get; set; }
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
        //ses
        public string UnitPrice { get; set; }
        public string StandardSalesUnitPrice { get; set; }
        public string Rank1UnitPrice { get; set; }
        public string Rank2UnitPrice { get; set; }
        public string Rank3UnitPrice { get; set; }
        public string Rank4UnitPrice { get; set; }
        public string Rank5UnitPrice { get; set; }
        public string ItemName { get; set; }
        public string CostUnitPrice { get; set; }
        public string StartChangeDate { get; set; }
        public string EndChangeDate { get; set; }
        //ses
        public string Remarks { get; set; }

        //検索用
        public string SyoKBN { get; set; }
        public string ItemFrom { get; set; }
        public string ItemTo { get; set; }
        public string BrandCD { get; set; }
        public string ITemName { get; set; }
        public string ProcessMode { get; set; }
        //HanbaiTankaKakeritu Entity
        public string TankaCDCopy { get; set; }
        public string TankaName { get; set; }
    }
}
