using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MonthlyStock_Entity : Base_Entity
    {
        public string AdminNO { get; set; }
        public string SoukoCD { get; set; }
        public string YYYYMM { get; set; }
        public string JanCD { get; set; }
        public string SKUCD { get; set; }
        public string StoreCD { get; set; }
        public string LastMonthQuantity { get; set; }
        public string LastMonthInventry { get; set; }
        public string LastMonthCost { get; set; }
        public string LastMonthAmount { get; set; }
        public string ThisMonthArrivalQ { get; set; }
        public string ThisMonthPurchaseQ { get; set; }
        public string ThisMonthPurchaseA { get; set; }
        public string ThisMonthReturnsQ { get; set; }
        public string ThisMonthReturnsA { get; set; }
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
        public string ThisMonthAdjustQ { get; set; }
        public string ThisMonthAdjustA { get; set; }
        public string ThisMonthMarkDownA { get; set; }
        public string ThisMonthQuantity { get; set; }
        public string ThisMonthInventry { get; set; }
        public string ThisMonthCalculationQ { get; set; }
        public string ThisMonthCalculationA { get; set; }
        public string ThisMonthCost { get; set; }
        public string ThisMonthAmount { get; set; }
        //月次在庫計算処理
        public int Mode { get; set; }
    }
}
