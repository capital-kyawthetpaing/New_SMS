using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MonthlyClaims_Entity : Base_Entity
    {
        public string TargetDate { get; set; }
        public string CustomerCD { get; set; }
        public string BillType { get; set; }
        public string SaleType { get; set; }
        public string StoreCD { get; set; }
        public string chk_do { get; set; }
        public string YYYYMM { get; set; }
        public string PrintType { get; set; }


        //月次債務計算処理
        public int Mode { get; set; }

    }
}
