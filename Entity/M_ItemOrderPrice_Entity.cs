using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_ItemOrderPrice_Entity : Base_Entity
    {
        public string MakerItem {get;set;}
        public string VendorCD { get; set; }
        public string StoreCD { get; set; }
        public string Rate { get; set; }
        public string PriceWithoutTax { get; set; }
        public string Remarks { get; set; }
        public string Display { get; set; }

        public string InsertOperator { get; set; }
        public string Headerdate { get; set; }

        //仕入先別発注単価マスタ抽出条件用
        public string DispKbn { get; set; }         //1:現状　2：履歴
        public string BrandCD { get; set; }
        public string SportsCD { get; set; }
        public string SegmentCD { get; set; }
        public string LastYearTerm { get; set; }
        public string LastSeason { get; set; }
        public string BaseDate { get; set; }

    }
}
