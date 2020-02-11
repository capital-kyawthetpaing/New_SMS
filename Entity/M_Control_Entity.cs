using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Control_Entity : Base_Entity
    {
        public string MainKey { get; set; }
        public string CompanyName { get; set; }
        public string ShortName { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string PresidentName { get; set; }
        public string StartMonth { get; set; }
        public string FiscalYear { get; set; }
        public string FiscalYYYYMM { get; set; }
        public string CurrencyCD { get; set; }
        public string SeqUnit { get; set; }
        public string DeliveryDate { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryPlace { get; set; }
        public string ValidityPeriod { get; set; }
        public string PostalAccountA { get; set; }
        public string PostalAccountB { get; set; }
        public string PostalAccountNo { get; set; }
    }
}
