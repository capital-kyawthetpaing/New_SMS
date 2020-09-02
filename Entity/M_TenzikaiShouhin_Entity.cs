using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_TenzikaiShouhin_Entity:Base_Entity
    {
        public string VendorCD { get; set; }
        public string LastYearTerm { get; set; }
        public string LastSeason { get; set; }
        public string BranCDFrom { get; set; }
        public string BrandCDTo { get; set; }
        public string SegmentCDFrom { get; set; }
        public string SegmentCDTo { get; set; }
        public string TenzikaiName { get; set; }
        public string JANCD { get; set; }
        public string DeleteFlg { get; set; }
        public string TanKaCD { get; set; }

        public string SalePriceOutTaxF { get; set; }
        public string SalePriceOutTaxT { get; set; }

        public string InsertOperator { get; set; }
    }
}
