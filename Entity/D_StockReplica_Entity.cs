using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_StockReplica_Entity:Base_Entity
    {
        public string ReplicaNO { get; set; }
        public string ReplicaDate { get; set; }
        public string ReplicaTime { get; set; }
        public string StockNO { get; set; }
        public string SoukoCD { get; set; }
        public string RackNO { get; set; }
        public string ArrivalPlanNO { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string JanCD { get; set; }
        public string ArrivalYetFLG { get; set; }
        public string ArrivalPlanKBN { get; set; }
        public string ArrivalPlanDate { get; set; }
        public string ArrivalDate { get; set; }
        
        public string StockSu { get; set; }
        public string PlanSu { get; set; }
        public string AllowableSu { get; set; }
        public string AnotherStoreAllowableSu { get; set; }
        public string ReserveSu { get; set; }
        public string InstructionSu { get; set; }
        public string ShippingSu { get; set; }
        public string OriginalStockNO { get; set; }
        public string ExpectReturnDate { get; set; }
        public string ReturnPlanSu { get; set; }
        public string VendorCD { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnSu { get; set; }
        public string LastCost { get; set; }

        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }

    }
}
