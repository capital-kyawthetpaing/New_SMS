using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_TenzikaiShouhin_Entity:Base_Entity
    {
        public string RefDate { get; set; }
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
        public string VendorCDFrom { get; set; }
        public string VendorCDTo { get; set; }
        public string SalePriceOutTaxF { get; set; }
        public string SalePriceOutTaxT { get; set; }
        public string NewRStartDate { get; set; }
        public string NewREndDate { get; set; }
        public string LastCStartDate { get; set; }
        public string LastCEndDate { get; set; }
        public string HanbaiYoteiDateMonth { get; set; }
        public string HanbaiYoteiDate { get; set; }
        public string SKUName { get; set; }
        public string InsertDateFrom { get; set; }
        public string InsertDateTo { get; set; }
        public string UpdateDateFrom { get; set; }
        public string UpdateDateTo { get; set; }

        public string TenzikaiNameCopy { get; set; }
        public string VendorCDCopy { get; set; }
        public string LastYearTermCopy { get; set; }
        public string LastSeasonCopy { get; set; }
        public string BrandCDCopy { get; set; }

        public string SegmentCDCopy { get; set; }

        public string Rank1 { get; set; }

        public string Rank2 { get; set; }
        public string Rank3 { get; set; }
        public string Rank4 { get; set; }
        public string Rank5 { get; set; }
        public string JoudaiTanka { get; set; }
        public string xml { get; set; }

    }
}
