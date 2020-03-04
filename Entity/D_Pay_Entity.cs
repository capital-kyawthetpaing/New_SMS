using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Pay_Entity : Base_Entity
    {
        public string PayNo { get; set; }
        public string LargePayNO { get; set; }
        public string PayeeCD { get; set; }

        //支払い一覧表フォームのため、Entityデータ要件
        public string PurchaseDateFrom { get; set; }
        public string PurchaseDateTo { get; set; }
        public string StaffCD { get; set; }

        public string PayDate { get; set; }

        // Search-SiharaiShoriNO (pnz)
        public string PayDateFrom { get; set; }
        public string PayDateTo { get; set; }
        public string InputDateTimeFrom { get; set; }
        public string InputDateTimeTo { get; set; }
        public string PayPlanDate { get; set; }
        public string LocationXml { get; set; }
       
        public string StoreCD { get; set; }

    }
}