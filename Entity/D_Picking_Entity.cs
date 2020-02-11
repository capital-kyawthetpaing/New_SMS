using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Picking_Entity : Base_Entity
    {
        public string PickingNO { get; set; }
        public string PickingKBN { get; set; }
        public string SoukoCD { get; set; }
        public string PrintDateTime { get; set; } 
        public string PrintStaffCD { get; set; }

        //検索用Entity
        public string PrintDateTimeFrom { get; set; }
        public string PrintDateTimeTo { get; set; }

        //ピッキング入力用Entity
        public string ShippingPlanDateFrom { get; set; }
        public string ShippingPlanDateTo { get; set; }
        public string JuchuuNO { get; set; }
        public string JanCD { get; set; }
        public string SKUCD { get; set; }
        public string PickingDate { get; set; }

        //added by ETZ for PickingList
        public string StoreCD { get; set; }
        public string ShippingDate { get; set; }
    }
}
