using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MonthlyDebt_Entity : Base_Entity
    {
        public string YYYYMM { get; set; }
        public string StoreCD { get; set; }
        public string PayeeCD { get; set; }
        public string LastBalanceGaku { get; set; }
        public string HontaiGaku0 { get; set; }
        public string HontaiGaku8 { get; set; }
        public string HontaiGaku10 { get; set; }
        public string HontaiGaku { get; set; }
        public string TaxGaku8 { get; set; }
        public string TaxGaku10 { get; set; }
        public string TaxGaku { get; set; }
        public string DebtGaku { get; set; }
        public string PayGaku { get; set; }
        public string OffsetGaku { get; set; }
        public string BalanceGaku { get; set; }
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }

        //月次債務計算処理
        public int Mode { get; set; }

    }
}
