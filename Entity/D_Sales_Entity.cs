using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Sales_Entity : Base_Entity
    {
        public string SalesNO { get; set; }
        public string StoreCD { get; set; }
        public string SalesDate { get; set; }
        public string ShippingNO { get; set; }
        public string CustomerCD { get; set; }
        public string BillingType { get; set; }
        public string Age { get; set; }
        public string SalesHontaiGaku { get; set; }
        public string SalesHontaiGaku0 { get; set; }
        public string SalesHontaiGaku8 { get; set; }
        public string SalesHontaiGaku10 { get; set; }
        public string SalesTax { get; set; }
        public string SalesTax8 { get; set; }
        public string SalesTax10 { get; set; }
        public string SalesGaku { get; set; }
        public string LastPoint { get; set; }
        public string WaitingPoint { get; set; }
        public string StaffCD { get; set; }
        public string PrintDate { get; set; }
        public string PrintStaffCD { get; set; }
        public string Discount { get; set; }
        public string Discount8 { get; set; }
        public string Discount10 { get; set; }
        public string DiscountTax { get; set; }
        public string DiscountTax8 { get; set; }
        public string DiscountTax10 { get; set; }

        //検索用Entity
        public string SalesDateFrom { get; set; }
        public string SalesDateTo { get; set; }
        //public string SalesFLG1 { get; set; }
        //public string SalesFLG2 { get; set; }

        //帳票用Entity
        public string CustomerName { get; set; }
        public string PrintFLG { get; set; }

        //店舗レジ出荷売上入力用Entity
        public string JuchuuNO { get; set; }
        public string FirstCollectPlanDate { get; set; }
    }
}
