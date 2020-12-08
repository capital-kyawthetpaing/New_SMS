using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_CollectPlan_Entity : Base_Entity
    {
        public string CollectPlanNO { get; set; }
        public string SalesNO { get; set; }
        public string JuchuuNO { get; set; }
        public string JuchuuKBN { get; set; }
        public string StoreCD { get; set; }
        public string CustomerCD { get; set; }
        public string HontaiGaku { get; set; }
        public string HontaiGaku0 { get; set; }
        public string HontaiGaku8 { get; set; }
        public string HontaiGaku10 { get; set; }
        public string Tax { get; set; }
        public string Tax8 { get; set; }
        public string Tax10 { get; set; }
        public string CollectPlanGaku { get; set; }
        public string BillingType { get; set; }
        public string BillingDate { get; set; }
        public string BillingNO { get; set; }
        public string PaymentMethodCD { get; set; }
        public string CardProgressKBN { get; set; }
        public string PaymentProgressKBN { get; set; }
        public string InvalidFLG { get; set; }
        public string BillingCloseDate { get; set; }
        public string FirstCollectPlanDate { get; set; }
        public string ReminderFLG { get; set; }
        public string NoReminderDate { get; set; }
        public string NextCollectPlanDate { get; set; }
        public string ActionCD { get; set; }
        public string NextActionCD { get; set; }
        public string LastReminderNO { get; set; }
        public string Program { get; set; }
        public string BillingConfirmFlg { get; set; }

        #region for Message
        public string Syori { get; set; }
        public string MessageID { get; set; }
        #endregion

        //帳票用Entity
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string PrintFLG { get; set; }
        public string DetailOn { get; set; }
        public string StaffCD { get; set; }

        //未入金確認照会用Entity
        public int Kbn { get; set; }
    }
}
