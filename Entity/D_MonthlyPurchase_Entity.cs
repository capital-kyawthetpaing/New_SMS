using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MonthlyPurchase_Entity : Base_Entity
    {
        public string VendorCD { get; set; }
        public string StoreCD { get; set; }
        public string YYYYMM { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string JanCD { get; set; }
        public string LastMonthQuantity { get; set; }
        public string LastMonthAmount { get; set; }
        public string LastMonthCost { get; set; }
        public string ThisMonthPurchaseQ { get; set; }
        public string ThisMonthPurchaseA { get; set; }
        public string ThisMonthCustPurchaseQ { get; set; }
        public string ThisMonthCustPurchaseA { get; set; }
        public string ThisMonthPurchasePlanQ { get; set; }
        public string ThisMonthPurchasePlanA { get; set; }
        public string ThisMonthSalesQ { get; set; }
        public string ThisMonthSalesA { get; set; }
        public string ThisMonthCustSalesQ { get; set; }
        public string ThisMonthCustSalesA { get; set; }
        public string ThisMonthSalesPlanQ { get; set; }
        public string ThisMonthSalesPlanA { get; set; }
        public string ThisMonthReturnsQ { get; set; }
        public string ThisMonthReturnsA { get; set; }
        public string ThisMonthReturnsPlanQ { get; set; }
        public string ThisMonthReturnsPlanA { get; set; }
        public string ThisMonthPlanAmount { get; set; }
        public string ThisMonthPlanQuantity { get; set; }
        public string ThisMonthAmount { get; set; }
        public string ThisMonthQuantity { get; set; }
        public string ThisMonthCost { get; set; }

        public string YYYYMMS { get; set; }
        public string YYYYMME { get; set; }
        //月次仕入計算処理
        public int Mode { get; set; }
    }
}
