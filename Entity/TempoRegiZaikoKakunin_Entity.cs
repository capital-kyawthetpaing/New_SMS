using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TempoRegiZaikoKakunin_Entity : Base_Entity
    {
        public string SoukoName { get; set; }
        public string JanCD { get; set; }

        public string StoreCD { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ArrivalPlanDate { get; set; }
        public string StockSu { get; set; }
        public string PlanSu { get; set; }
        public string AllowableSu { get; set; }
        public string AnotherStoreAllowableSu { get; set; }

        public string dataCheck { get; set; }
        public string ItemCD {get;set;}
        public string SKUName { get; set; }
    }
}
