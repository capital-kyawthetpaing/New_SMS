using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Cost_Entity : Base_Entity
    {
        public string CostNO { get; set; }
        public string PayeeCD { get; set; }
        public string RecordedDate { get; set; }
        public string PayPlanDate { get; set; }
        public string InputDateTime { get; set; }
        public string StaffCD { get; set; }
        public string RegularlyFLG { get; set; }
        public string TotalGaku { get; set; }
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }
        public string RecordedDateFrom { get; set; }
        public string RecordedDateTo { get; set; }
        public string ExpanseEntryDateFrom { get; set; }
        public string ExpanseEntryDateTo { get; set; }
        public string PaymentDueDateFrom { get; set; }
        public string PaymentDueDateTo { get; set; }
        public string PaymentDateFrom { get; set; }
        public string PaymentDateTo { get; set; }
        public string Unpaid { get; set; }
        public string Paid { get; set; }
       






        // ptk 2019-09-26 for keihiltiranhanyou
        public string PrintDate { get; set; }
        public string PrintTime { get; set; }
        public string Page { get; set; }
        public string Store { get; set; }
        public string VendorCD { get; set; }
        public string VendorName { get; set; }
        public string Summary { get; set; }
        public string Char1 { get; set; }
        public string CostGaku { get; set; }
        public string Expense_TimeFrom { get; set; }
        public string ExPense_TimeTo { get; set; }

        public string PrintTarget { get; set; } // 1 , 0, ""
    }
}
