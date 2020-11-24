using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_Stock_Entity:Base_Entity
    {
        public string StockNO { get; set; }
        public string SoukoCD { get; set; }
        public string RackNO { get; set; }
        public string RackNOFrom { get; set; }
        public string RackNOTo { get; set; }
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

	    public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }

        //棚番選択用
        public string SKUName { get; set; }
        public string SoukoName { get; set; }


        ///Add by ETZ
        public string ArrivalStartDate { get; set; }
        public string ArrivalEndDate { get; set; }
        public string RegisterFlg { get; set; }
        public string UnregisterFlg { get; set; }
        public string LocationFlg { get; set; }

        //返品入力
        public string ExpectReturnDateFrom { get; set; }
        public string ExpectReturnDateTo { get; set; }
        public int F10 { get; set; }

        public string StoreCD { get; set; }


        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string type { get; set; }

        public string Rdotype { get; set; }

    }
}
