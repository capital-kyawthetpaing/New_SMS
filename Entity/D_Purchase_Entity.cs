using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Purchase_Entity : Base_Entity
    {
        public string PurchaseNO { get; set; }
        public string StoreCD { get; set; }
        public string PurchaseDate { get; set; }
        public string CancelFlg { get; set; }
        public string ProcessKBN { get; set; }
        public string ReturnsFlg { get; set; }
        public string VendorCD { get; set; }
        public string CalledVendorCD { get; set; }
        public string CalculationGaku { get; set; }
        public string AdjustmentGaku { get; set; }
        public string PurchaseGaku { get; set; }
        public string PurchaseTax { get; set; }
        public string TotalPurchaseGaku { get; set; }
        public string CommentOutStore { get; set; }
        public string CommentInStore { get; set; }
        public string ExpectedDateFrom { get; set; }
        public string ExpectedDateTo { get; set; }
        public string InputDate { get; set; }
        public string StaffCD { get; set; }
        public string PaymentPlanDate { get; set; }
        public string PayPlanNO { get; set; }
        public string OutputDateTime { get; set; }
        public string StockAccountFlg { get; set; }

        //検索用Entity
        public string PurchaseDateFrom { get; set; }
        public string PurchaseDateTo { get; set; }

        public string PayeeCD { get; set; }

        //仕入入力用
        public string PurchaseRows { get; set; }
        public int OldPurchaseSu { get; set; }
        public string StockNO { get; set; }
        public string WarehousingNO { get; set; }
        public string ReserveNO { get; set; }

        public string SyukkazumiFlg { get; set; }
        public string SyukkaSijizumiFlg { get; set; }
        public string PickingzumiFlg { get; set; }
        public string HikiatezumiFlg { get; set; }

        //仕入単価訂正依頼書印刷用
        public int ChkMisyutsuryoku { get; set; }
        public int ChkSyutsuryokuZumi { get; set; }

        //pnz
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }
        public string PaymentDueDateFrom { get; set; }
        public string PaymentDueDateTo { get; set; }
        public string DeliveryNo { get; set; }
        public string PayeeFLg { get; set; }
        public string CheckValue { get; set; }
        public string Paid { get; set; }
        public string UnPaid { get; set; }



}
}
