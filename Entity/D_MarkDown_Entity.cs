using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MarkDown_Entity : Base_Entity
    {
        public string MarkDownNO { get; set; }
        public string StoreCD { get; set; }
        public string SoukoCD { get; set; }
        public string MarkDownDate { get; set; }
        public string StockReplicaName { get; set; }
        public string StaffCD { get; set; }
        public string VendorCD { get; set; }
        public string CostingDate { get; set; }
        public string UnitPriceDate { get; set; }
        public string ExpectedPurchaseDate { get; set; }
        public string PurchaseDate { get; set; }
        public string Comment { get; set; }
        public string MDPurchaseNO { get; set; }
        public string PurchaseNO { get; set; }

        //照会用Entity
        public string CostingDateFrom { get; set; }
        public string CostingDateTo { get; set; }
        public string PurchaseDateFrom { get; set; }
        public string PurchaseDateTo { get; set; }
        public string ChkNotAccount { get; set; }
        public string ChkAccounted { get; set; }

        //更新用
        public string PurchaseGaku { get; set; }       

    }
}
