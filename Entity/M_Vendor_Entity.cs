using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Vendor_Entity : Base_Entity
    {
        public string VendorCD { get; set; }
        public string VendorName { get; set; }
        public string VendorShortName { get; set; }
        public string ShoguchiFlg { get; set; }
        public string VendorLongName1 { get; set; }
        public string VendorLongName2 { get; set; }
        public string VendorPostName { get; set; }
        public string VendorPositionName { get; set; }
        public string VendorStaffName { get; set; }
        public string VendorKana { get; set; }
        public string VendorFlg { get; set; }
        public string PayeeFlg { get; set; }
        public string MoneyPayeeFlg { get; set; }
        public string PayeeCD { get; set; }
        public string payeeName { get; set; }
        public string MoneyPayeeCD { get; set; }
        public string moneypayeeName { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public string MailAddress3 { get; set; }
        public string TelephoneNO { get; set; }
        public string FaxNO { get; set; }
        public string PaymentCloseDay { get; set; }
        public string PaymentPlanKBN { get; set; }
        public string PaymentPlanDay { get; set; }
        public string HolidayKBN { get; set; }
        public string BankCD { get; set; }
        public string BranchCD { get; set; }
        public string KouzaKBN { get; set; }
        public string KouzaNO { get; set; }
        public string KouzaMeigi { get; set; }
        public string KouzaCD { get; set; }
        public string TaxTiming { get; set; }
        public string TaxFractionKBN { get; set; }
        public string AmountFractionKBN { get; set; }
        public string NetFlg { get; set; }
        public string EDIFlg { get; set; }
        public string EDIMail { get; set; }
        public string EDIVendorCD { get; set; }
        public string LastOrderDate { get; set; }
        public string StaffCD { get; set; }
        public string AnalyzeCD1 { get; set; }
        public string AnalyzeCD2 { get; set; }
        public string AnalyzeCD3 { get; set; }
        public string DisplayOrder { get; set; }
        public string NotDisplyNote { get; set; }
        public string DisplayNote { get; set; }

        //Search_Vendor
        public string VendorCDFrom { get; set; }
        public string VendorCDTo { get; set; }
        public string DisplayKBN { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string KouzaName { get; set; }
        public string StaffName { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string VendorKBN { get; set; }
        public string KeyWordType { get; set; }

    }
}
