using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_StoreClose_Entity : Base_Entity
    {
        public string StoreCD { get; set; }
        public string FiscalYYYYMM { get; set; }
        public string ClosePosition1 { get; set; }
        public string ClosePosition2 { get; set; }
        public string ClosePosition3 { get; set; }
        public string ClosePosition4 { get; set; }
        public string ClosePosition5 { get; set; }

        public string MonthlyClaimsFLG { get; set; }
        public string MonthlyClaimsDateTime { get; set; }
        public string MonthlyDebtFLG { get; set; }
        public string MonthlyDebtDateTime { get; set; }
        public string MonthlyStockFLG { get; set; }
        public string MonthlyStockDateTime { get; set; }

        //月次締処理
        public int Mode { get; set; }
        public int Kbn { get; set; }
        public string OperateModeNm { get; set; }

    }
}
