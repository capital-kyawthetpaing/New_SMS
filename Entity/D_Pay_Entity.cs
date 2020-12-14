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
        public string InputDateTime { get; set; }
        //public string StaffCD { get; set; }
        //public string PayDate { get; set; }
        //public string PayPlanDate { get; set; }
        public string HontaiGaku8 { get; set; }
        public string HontaiGaku10 { get; set; }
        public string TaxGaku8 { get; set; }
        public string TaxGaku10 { get; set; }
        public string PayGaku { get; set; }
        public string NotPaidGaku { get; set; }
        public string TransferGaku { get; set; }
        public string TransferFeeGaku { get; set; }
        public string FeeKBN { get; set; }
        public string MotoKouzaCD { get; set; }
        public string BankCD { get; set; }
        public string BranchCD { get; set; }
        public string KouzaKBN { get; set; }
        public string KouzaNO { get; set; }
        public string KouzaMeigi { get; set; }
        public string CashGaku { get; set; }
        public string BillGaku { get; set; }
        public string BillDate { get; set; }
        public string BillNO { get; set; }
        public string ERMCGaku { get; set; }
        public string ERMCDate { get; set; }
        public string ERMCNO { get; set; }
        public string CardGaku { get; set; }
        public string OffsetGaku { get; set; }
        public string OtherGaku1 { get; set; }
        public string Account1 { get; set; }
        public string SubAccount1 { get; set; }
        public string OtherGaku2 { get; set; }
        public string Account2 { get; set; }
        public string SubAccount2 { get; set; }
        public string FBCreateDate { get; set; }
        public string FBCreateNO { get; set; }
        //支払登録用Entitiy
        
        public string PayeeName { get; set; }

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
        public string PayGakuTotol { get; set; }
        public string StoreCD { get; set; }
        public string PaymentNum { get; set; }

        public string Flg { get; set; }

    }
}