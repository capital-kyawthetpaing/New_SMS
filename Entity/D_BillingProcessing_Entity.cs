using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_BillingProcessing_Entity : Base_Entity
    {
        public string ProcessingNO { get; set; }
        public string StoreCD { get; set; }
        public string BillingDate { get; set; }
        public string CustomerCD { get; set; }
        public string ProcessingKBN { get; set; }
        public string ProcessingDateTime { get; set; }
        public string StaffCD { get; set; }

    }
}
