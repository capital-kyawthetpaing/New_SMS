using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_PayPlan_Entity:Base_Entity
    {
        public string PayPlanNO { get; set; }
        public string PayPlanKBN { get; set; }
        public string Number { get; set; }
        public string StoreCD { get; set; }
        public string PayeeCD { get; set; }
        public string RecordedDate { get; set; }
        public string PayPlanDate { get; set; }
        public string HontaiGaku8 { get; set; }
        public string HontaiGaku10 { get; set; }
        public string TaxGaku8 { get; set; }
        public string TaxGaku10 { get; set; }
        public string PayPlanGaku { get; set; } 
        public string PayConfirmGaku { get; set; }
        public string PayConfirmFinishedKBN { get; set; }
        public string PayCloseDate { get; set; }
        public string PayCloseNO { get; set; }
        public string Program { get; set; }
        public string PayConfirmFinishedDate { get; set; }
        public string InsertOperator { get; set; }
        public string InsertDateTime { get; set; }
        public string UpdateOperator { get; set; }
        public string UpdateDateTime { get; set; }
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }
        public string PaymentDueDateFrom { get; set; }
        public string PaymenetDueDateTo { get; set; }
        public string CloseStatusSumi { get; set; }
        public string PaymentStatusUnpaid { get; set; }
        public string Purchase { get; set; }
        public string Expense { get; set; }
        public string PayPlanDateFrom { get; set; }
        public string PayPlanDateTo { get; set; }
        public string PaymentTotal { get; set; }
    }
}
