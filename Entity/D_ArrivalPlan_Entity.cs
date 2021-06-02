using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Entity
{
   public class D_ArrivalPlan_Entity:Base_Entity
    {
        public string ArrivalPlanNO { get; set; }
        public string ArrivalPlanKBN { get; set; }
        public string Number { get; set; }
        public string NumberRows { get; set; }
        public string NumberSEQ { get; set; }
        public string ArrivalPlanDate { get; set; }
        public string ArrivalPlanMonth { get; set; }
        public string ArrivalPlanCD { get; set; }
        public string CalcuArrivalPlanDate { get; set; }
        public string ArrivalPlanUpdateDateTime { get; set; }
        public string StaffCD { get; set; }
        public string LastestFLG { get; set; }
        public string EDIImportNO { get; set; }
        public string SoukoCD { get; set; }
        public string SKUCD { get; set; }
        public string AdminNO { get; set; }
        public string JanCD { get; set; }
        public string ArrivalPlanSu { get; set; }
        public string ArrivalSu { get; set; }
        public string OriginalArrivalPlanNO { get; set; }
        public string OrderCD { get; set; }
        public string FromSoukoCD { get; set; }
        public string ToStoreCD { get; set; }

        public string DeleteDateTime { get; set; }
        public string CalcuArrivalPlanDate1 { get; set; }
        public string CalcuArrivalPlanDate2 { get; set; }
        public string FrmSoukoCD { get; set; }
        public string ITEMCD { get; set; }
        public string statusFlg { get; set; }
        public string DisplayFlg { get; set; }
        public string MakerItem { get; set; } 



    }
}
