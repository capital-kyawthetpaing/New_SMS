using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_YahooCount_Entity : Base_Entity
    {
        public string  InportSEQ { get; set; }
        public string  StoreCD { get; set; }
        public string  APIKey { get; set; }
        public string  OperateDate { get; set; }
        public string  OperateTime { get; set; }
        public string  Status { get; set; }
        public string  Count_NewOrder { get; set; }
        public string  Count_NewReserve { get; set; }
        public string  Count_WaitPayment { get; set; }
        public string  Count_WaitShipping { get; set; }
        public string  Count_Shipping { get; set; }
        public string  Count_Reserve { get; set; }
        public string  Count_Holding { get; set; }
        public string  Count_WaitDone { get; set; }
        public string  Count_Suspect { get; set; }
        public string  Count_SettleError { get; set; }
        public string  Count_Refund { get; set; }
        public string  Count_AutoDone { get; set; }
        public string  Count_AutoWorking { get; set; }
        public string  Count_Release { get; set; }
        public string  Count_NoPayNumber { get; set; }

    }
}
