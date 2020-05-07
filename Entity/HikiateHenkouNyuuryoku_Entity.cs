using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class HikiateHenkouNyuuryoku_Entity : Base_Entity
    {
        //ヘッダ条件
        public string StoreCD { get; set; }
        public string AdminNO { get; set; }
        public string SKUCD { get; set; }

        //在庫から引当条件
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string ArrivalPlanDateFrom { get; set; }
        public string ArrivalPlanDateTo { get; set; }
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }
        public string OrderCD { get; set; }
        public string OrderNO { get; set; }


        //受注から引当条件
        public string JuchuuDateFrom { get; set; }
        public string JuchuuDateTo { get; set; }
        public string CustomerCD { get; set; }
        public string JuchuuNO { get; set; }
        public string ChkNotReserve { get; set; }
        public string ChkReserved { get; set; }
        public string JuchuuKBN1 { get; set; }
        public string JuchuuKBN2 { get; set; }
        public string JuchuuKBN3 { get; set; }
        public string JuchuuKBN4 { get; set; }
    }
}
