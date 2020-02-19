using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_DepositHistory_Entity : Base_Entity
    {
        public string StoreCD { get; set; }
        public string DataKBN { get; set; }
        public string ExchangeMoney { get; set; }
        public string ExchangeDenomination { get; set; }
        public string ExchangeCount { get; set; }
        public string ExchangeCount2 { get; set; } 
        public string Remark { get; set; }
        public string DenominationCD { get; set; }
        public string DepositGaku{ get; set; }
        public string DepositKBN{ get; set; }
        public string DepositKBN1 { get; set; }
        public string SalesSU { get; set; }
        public string SalesUnitPrice { get; set; }
        public string SalesTax { get; set; }
        public string SalesGaku { get; set; }
        public string TotalGaku { get; set; }
        public string Refund { get; set; }
        public string AdminNO { get; set; }
        public string IsIssued { get; set; }
        public string CancelKBN { get; set; }
        public string RecoredKBN { get; set; }
        public string AccountingDate { get; set; }
        public string Number { get; set; }
        public string Rows { get; set; }
        public string SalesTaxRate { get; set; }
        public string YYYYMM { get; set; }
        public string ProperGaku { get; set; }
        public string DiscountGaku { get; set; }
        public string CustomerCD { get; set; }
    }
}
