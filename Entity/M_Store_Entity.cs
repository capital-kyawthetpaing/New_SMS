using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Store_Entity : Base_Entity
    {
        public string StoreCD { get; set; }
        public string StoreName { get; set; }
        public string StoreKBN { get; set; }
        public string StorePlaceKBN { get; set; }
        public string MallCD { get; set; }
        public string APIKey { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TelphoneNO { get; set; }
        public string FaxNO { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public string MailAddress3 { get; set; }
        public string ApprovalStaffCD11 { get; set; }
        public string ApprovalStaffCD12 { get; set; }
        public string ApprovalStaffCD21 { get; set; }
        public string ApprovalStaffCD22 { get; set; }
        public string ApprovalStaffCD31 { get; set; }
        public string ApprovalStaffCD32 { get; set; }
        public string DeliveryDate { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryPlace { get; set; }
        public string ValidityPeriod { get; set; }
        public string Print1 { get; set; }
        public string Print2 { get; set; }
        public string Print3 { get; set; }
        public string Print4 { get; set; }
        public string Print5 { get; set; }
        public string Print6 { get; set; }
        public string KouzaCD { get; set; }
        public string ReceiptPrint { get; set; }
        public string MoveMailPatternCD { get; set; }
        public string InvoiceNotation { get; set; }
        public string Remarks { get; set; }
        public string FiscalYYYYMM { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string StoreCDFrom { get; set; }
        public string StoreCDTo { get; set; }
        public string StoreKBN1 { get; set; }
        public string StoreKBN2 { get; set; }
        public string StoreKBN3 { get; set; }

        public string SysDate { get; set; }
        public string Type { get; set; }
    }
}
