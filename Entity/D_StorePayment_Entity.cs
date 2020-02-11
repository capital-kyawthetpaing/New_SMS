using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Entity
{
    public class D_StorePayment_Entity : Base_Entity
    {
        public string SalesNO { get; set; }
        public string StoreCD { get; set; }
        public string PurchaseAmount { get; set; }
        public string TaxAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string BillingAmount { get; set; }
        public string PointAmount { get; set; }
        public string CardDenominationCD { get; set; }
        public string CardAmount { get; set; }
        public string CashAmount { get; set; }
        public string DepositAmount { get; set; }
        public string RefundAmount { get; set; }
        public string CreditAmount { get; set; }
        public string Denomination1CD { get; set; }
        public string Denomination1Amount { get; set; }
        public string Denomination2CD { get; set; }
        public string Denomination2Amount { get; set; }
        public string AdvanceAmount { get; set; }
        public string TotalAmount { get; set; }
        public string SalesRate { get; set; }

        public string CardDenominationName { get; set; }
        public string DenominationName1 { get; set; }
        public string DenominationName2 { get; set; }
    }
}
