using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Billing_Entity : Base_Entity
    {
        public string BillingNO { get; set; }
        public string StoreCD { get; set; }
        public string BillingCloseDate { get; set; }
        public string CollectPlanDate { get; set; }
        public string BillingCustomerCD { get; set; }
        public string ProcessingNO { get; set; }
        public string SumBillingHontaiGaku { get; set; }
        public string SumBillingHontaiGaku0 { get; set; }
        public string SumBillingHontaiGaku8 { get; set; }
        public string SumBillingHontaiGaku10 { get; set; }
        public string SumBillingTax { get; set; }
        public string SumBillingTax8 { get; set; }
        public string SumBillingTax10 { get; set; }
        public string SumBillingGaku { get; set; }
        public string SumBillingGaku0 { get; set; }
        public string AdjustHontaiGaku8 { get; set; }
        public string AdjustHontaiGaku10 { get; set; }
        public string AdjustTax8 { get; set; }
        public string AdjustTax10 { get; set; }
        public string TotalBillingHontaiGaku { get; set; }
        public string TotalBillingHontaiGaku0 { get; set; }
        public string TotalBillingHontaiGaku8 { get; set; }
        public string TotalBillingHontaiGaku10 { get; set; }
        public string TotalBillingTax { get; set; }
        public string TotalBillingTax8 { get; set; }
        public string TotalBillingTax10 { get; set; }
        public string BillingGaku { get; set; }
        public string PrintDateTime { get; set; }
        public string PrintStaffCD { get; set; }
        public string CollectDate { get; set; }
        public string LastCollectDate { get; set; }
        public string CollectStaffCD { get; set; }
        public string CollectGaku { get; set; }

        //帳票用Entity
        public string CustomerName { get; set; }
        public string PrintFLG { get; set; }

        //入金元検索用Entity

        public string BillingGakuFrom { get; set; }
        public string BillingGakuTo { get; set; }
        public string ChkMinyukin { get; set; }
        public string ChkNyukinzumi { get; set; }
        public string CollectDateFrom { get; set; }
        public string CollectDateTo { get; set; }
        public int RdoDispKbn { get; set; }
    }
}
