using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public  class D_PayCloseHistory_Entity : Base_Entity
    {
        public string PayCloseNO { get; set; }
        public string PayCloseDate { get; set; }
        public string PayeeKBN { get; set; }
        public string PayeeCD { get; set; }
        public string ProcessingKBN { get; set; }
        public string PayCloseProcessingDateTime { get; set; }
        public string StaffCD { get; set; }

        public string StoreCD { get; set; }
        //ssa
        public string ProcessType { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentCD { get; set; }
    }
}
