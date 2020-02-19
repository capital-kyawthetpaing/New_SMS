using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MonthlyStock_Entity : Base_Entity
    {
        public string JanCD { get; set; }
        public string SoukoCD { get; set; }
        public string YYYYMM { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string LastMonthQuantity { get; set; }
        public string LastMonthCost { get; set; }
        public string LastMonthAmount { get; set; }
        public string ThisMonthArrivalQ { get; set; }
        public string ThisMonthPurchaseQ { get; set; }
        public string ThisMonthPurchaseA { get; set; }
        public string ThisMonthShippingQ { get; set; }
        public string ThisMonthSalesQ { get; set; }
        public string ThisMonthSalesA { get; set; }
        public string ThisMonthMoveOutQ { get; set; }
        public string ThisMonthMoveOutA { get; set; }
        public string ThisMonthMoveInQ { get; set; }
        public string ThisMonthMoveInA { get; set; }
        public string ThisMonthAnyOutQ { get; set; }
        public string ThisMonthAnyOutA { get; set; }
        public string ThisMonthAnyInQ { get; set; }
        public string ThisMonthAnyInA { get; set; }
        public string ThisMonthMarkDownA { get; set; }
        public string ThisMonthQuantity { get; set; }
        public string ThisMonthCost { get; set; }
        public string ThisMonthAmount { get; set; }
        public string YYYYMMFrom { get; set; }
        public string YYYYMMTo { get; set; }
        public string TargetDateFrom { get; set; }
        public string TargetDateTo { get; set; }
        public string StoreCD { get; set; }
        //月次在庫計算処理
        public int Mode { get; set; }
    }
}
