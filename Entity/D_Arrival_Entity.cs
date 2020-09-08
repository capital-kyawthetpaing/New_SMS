using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Entity
{
   public class D_Arrival_Entity:Base_Entity
    {
        public string ArrivalNO { get; set; }
        public string VendorDeliveryNo { get; set; }
        public string StoreCD { get; set; }
        public string VendorCD { get; set; }
        public string ArrivalDate { get; set; }
        public string InputDate { get; set; }
        public string ArrivalKBN { get; set; }
        public string StaffCD { get; set; }
        public string SoukoCD { get; set; }
        public string RackNO { get; set; }
        public string JanCD { get; set; }
        public string AdminNO { get; set; }
        public string SKUCD { get; set; }
        public string MakerItem { get; set; }
        public string ArrivalSu { get; set; }
        public string PurchaseSu { get; set; }

        //検索用Entity
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }

        public string SoukoType { get; set; }

        public string DirectFLG { get; set; }

        public string OrderKbn { get; set; }

        //pnz
        public string DeleteDateTime { get; set; }
        public string ArrivalDate1 { get; set; }
        public string ArrivalDate2 { get; set; }
    }
}
