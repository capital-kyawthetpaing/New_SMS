using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_PaymentConfirm_Entity : Base_Entity
    {
        public string ConfirmNO{ get; set; }
        public string CollectNO { get; set; }
        public string CollectClearDate{ get; set; }
        public string StaffCD{ get; set; }
        public string ConfirmDateTime{ get; set; }
        public string ConfirmAmount{ get; set; }

        //検索用Entity
        public string CollectClearDateFrom { get; set; }
        public string CollectClearDateTo { get; set; }
        public string InputDateFrom { get; set; }
        public string InputDateTo { get; set; }

        //帳票用Entity
        public string StoreCD { get; set; }
    }
}
