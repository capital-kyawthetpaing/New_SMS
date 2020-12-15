using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Collect_Entity : Base_Entity
    {
        public string CollectNO { get; set; }
        public string InputKBN { get; set; }
        public string StoreCD { get; set; }
        public string StaffCD { get; set; }
        public string InputDatetime { get; set; }
        public string WebCollectNO { get; set; }
        public string WebCollectType { get; set; }
        public string CollectCustomerCD { get; set; }
        public string CollectDate { get; set; }
        public string PaymentMethodCD { get; set; }
        public string KouzaCD { get; set; }
        public string BillDate { get; set; }
        public string CollectAmount { get; set; }
        public string FeeDeduction { get; set; }
        public string Deduction1 { get; set; }
        public string Deduction2 { get; set; }
        public string DeductionConfirm { get; set; }
        public string ConfirmSource { get; set; }
        public string ConfirmAmount { get; set; }
        public string Remark { get; set; }

        public string AdvanceFLG { get; set; }
        //検索用Entity
        public string CollectDateFrom { get; set; }
        public string CollectDateTo { get; set; }
        public string InputDateFrom { get; set; }
        public string InputDateTo { get; set; }
        public int ChkZan { get; set; }


        //入金入力用Entitiy
        public int RdoSyubetsu { get; set; }
        public string ConfirmNO { get; set; }
        public string CollectClearDate { get; set; }
        public int KidouMode { get; set; }
    }
}
