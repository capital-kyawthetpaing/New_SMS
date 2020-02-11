using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Mitsumori_Entity : Base_Entity
    {
        public string MitsumoriNO { get; set; }
        public string StoreCD { get; set; }
        public string MitsumoriDate { get; set; }
        public string StaffCD { get; set; }
        public string CustomerCD { get; set; }
        public string CustomerName { get; set; }
        public string CustomerName2 { get; set; }
        public string AliasKBN { get; set; }
        public string ZipCD1 { get; set; }
        public string ZipCD2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Tel11 { get; set; }
        public string Tel12 { get; set; }
        public string Tel13 { get; set; }
        public string Tel21 { get; set; }
        public string Tel22 { get; set; }
        public string Tel23 { get; set; }
        public string JuchuuChanceKBN { get; set; }
        public string MitsumoriName { get; set; }
        public string DeliveryDate { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryPlace { get; set; }
        public string ValidityPeriod { get; set; }
        public string MitsumoriHontaiGaku { get; set; }
        public string MitsumoriTax8 { get; set; }
        public string MitsumoriTax10 { get; set; }
        public string MitsumoriGaku { get; set; }
        public string CostGaku { get; set; }
        public string ProfitGaku { get; set; }
        public string RemarksInStore { get; set; }
        public string RemarksOutStore { get; set; }
        public string PrintDateTime { get; set; }
        public string JuchuuFLG { get; set; }

        //検索用Entity
        public string MitsumoriDateFrom { get; set; }
        public string MitsumoriDateTo { get; set; }
        public string MitsumoriInputDateFrom { get; set; }
        public string MitsumoriInputDateTo { get; set; }
        public string JuchuuFLG1 { get; set; }
        public string JuchuuFLG2 { get; set; }

        //帳票用Entity
        public string PrintFLG { get; set; }
    }
}
