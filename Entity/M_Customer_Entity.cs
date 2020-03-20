using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Customer_Entity : Base_Entity
    {
        public string CustomerCD { get; set; }
        public string StroeCD { get; set; }
        public string VariousFLG { get; set; }
        public string CustomerName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LongName1 { get; set; }
        public string LongName2 { get; set; }
        public string KanaName { get; set; }
        public string StoreKBN { get; set; }
        public string CustomerKBN { get; set; }
        public string StoreTankaKBN { get; set; }
        public string AliasKBN { get; set; }
        public string BillingType { get; set; }

       // public string BirthDate { get; set; }
        public string GroupName { get; set; }
        public string BillingFLG { get; set; }
        public string CollectFLG { get; set; }
        public string BillingCD { get; set; }
        public string CollectCD { get; set; }
        public string Birthdate { get; set; }
        public string Sex { get; set; }
        public string Tel11 { get; set; }
        public string Tel12 { get; set; }
        public string Tel13 { get; set; }
        public string Tel21 { get; set; }
        public string Tel22 { get; set; }
        public string Tel23 { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string MailAddress { get; set; }
        public string MailAddress2 { get; set; }
        public string TankaCD { get; set; }
        public string PointFLG { get; set; }
        public string LastPoint { get; set; }
        public string WaitingPoint { get; set; }
        public string TotalPoint { get; set; }
        public string TotalPurchase { get; set; }
        public string UnpaidAmount { get; set; }
        public string UnpaidCount { get; set; }
        public string LastSalesDate { get; set; }
        public string LastSalesStoreCD { get; set; }
        public string MainStoreCD { get; set; }
        public string StaffCD { get; set; }
        public string AttentionFLG { get; set; }
        public string ConfirmFLG { get; set; }
        public string ConfirmComment { get; set; }
        public string BillingCloseDate { get; set; }
        public string CollectPlanMonth { get; set; }
        public string CollectPlanDate { get; set; }
        public string HolidayKBN { get; set; }
        public string TaxTiming { get; set; }
        public string TaxPrintKBN { get; set; } //追加項目
        public string TaxFractionKBN { get; set; }
        public string AmountFractionKBN { get; set; }
        public string CreditLevel { get; set; }
        public string CreditCard { get; set; }
        public string CreditInsurance { get; set; }
        public string CreditDeposit { get; set; }
        public string CreditETC { get; set; }
        public string CreditAmount { get; set; }
        public string CreditWarningAmount { get; set; }
        public string CreditAdditionAmount { get; set; }
        public string PaymentMethodCD { get; set; }
        public string KouzaCD { get; set; }
        public string DisplayOrder { get; set; }
        public string PaymentUnit { get; set; }
        public string NoInvoiceFlg { get; set; }
        public string CountryKBN { get; set; }
        public string CountryName { get; set; }
        public string RegisteredNumber { get; set; }
        public string DMFlg { get; set; }
        public string RemarksOutStore { get; set; }
        public string RemarksInStore { get; set; }

        public string AnalyzeCD1 { get; set; }
        public string AnalyzeCD2 { get; set; }
        public string AnalyzeCD3 { get; set; }

        //TempoRegiKaiinTouroku
        public string TelephoneNo1 { get; set; }
        public string TelephoneNo2 { get; set; }
        public string TelephoneNo3 { get; set; }
        public string HomephoneNo1 { get; set; }
        public string HomephoneNo2 { get; set; }
        public string HomephoneNo3 { get; set; }

        //Customer_Search
        public string CustomerFrom { get; set; }
        public string CustomerTo { get; set; }
        public string RefDate { get; set; }
        public string TelephoneNo { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string chkStore { get; set; }
        public string chkWeb { get; set; }
        public string CustKBN { get; set; }
        public string KeyWordType { get; set; }
    }
}
