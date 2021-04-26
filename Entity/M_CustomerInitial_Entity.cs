using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_CustomerInitial_Entity : Base_Entity
    {
        public string StoreKBN { get; set; }
        public string CustomerKBN { get; set; }
        public string StoreTankaKBN { get; set; }
        public string TankaCD { get; set; }
        public string PointFLG { get; set; }
        public string MainStoreCD { get; set; }
        public string StaffCD { get; set; }
        public string HolidayKBN { get; set; }
        public string TaxTiming { get; set; }
        public string TaxPrintKBN { get; set; }
        public string TaxFractionKBN { get; set; }
        public string AmountFractionKBN { get; set; }
        public string CreditLevel { get; set; }
        public string CreditCheckKBN { get; set; }
        public string PaymentMethodCD { get; set; }
        public string KouzaCD { get; set; }
        public string DisplayOrder { get; set; }
        public string PaymentUnit { get; set; }
        public string NoInvoiceFlg { get; set; }
        public string DMFlg { get; set; }
    }
}
