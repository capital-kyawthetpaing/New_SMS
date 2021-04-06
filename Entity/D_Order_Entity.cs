using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Order_Entity : Base_Entity
    {
        public string OrderNO { get; set; }
        public string OrderProcessNO { get; set; }
        public string StoreCD { get; set; }
        public string OrderDate { get; set; }
        public string ReturnFLG { get; set; }
        public string OrderDataKBN { get; set; }
        public string OrderWayKBN { get; set; }
        public string OrderCD { get; set; }
        public string OrderPerson { get; set; }
        public string AliasKBN { get; set; }
        public string DestinationKBN { get; set; }
        public string DestinationName { get; set; }
        public string DestinationZip1CD { get; set; }
        public string DestinationZip2CD { get; set; }
        public string DestinationAddress1 { get; set; }
        public string DestinationAddress2 { get; set; }
        public string DestinationTelphoneNO { get; set; }
        public string DestinationFaxNO { get; set; }
        public string DestinationSoukoCD { get; set; }
        public string CurrencyCD { get; set; }
        public string OrderHontaiGaku { get; set; }
        public string OrderTax8 { get; set; }
        public string OrderTax10 { get; set; }
        public string OrderGaku { get; set; }
        public string CommentOutStore { get; set; }
        public string CommentInStore { get; set; }
        public string StaffCD { get; set; }
        public string FirstArriveDate { get; set; }
        public string LastArriveDate { get; set; }
        public string ApprovalDate { get; set; }
        public string LastApprovalDate { get; set; }
        public string LastApprovalStaffCD { get; set; }
        public string ApprovalStageFLG { get; set; }
        public string FirstPrintDate { get; set; }
        public string LastPrintDate { get; set; }
        public string ArrivalPlanDate { get; set; }
        public string ApprovalEnabled { get; set; }

        //検索用Entity
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
      
        //承認用
        public int Misyonin { get; set; }
        public int SyoninZumi { get; set; }
        public int Kyakka { get; set; }

        //照会用
        public string ArrivalPlanDateFrom { get; set; }
        public string ArrivalPlanDateTo { get; set; }
        public string ArrivalDateFrom { get; set; }
        public string ArrivalDateTo { get; set; }
        public string PurchaseDateFrom { get; set; }
        public string PurchaseDateTo { get; set; }
        public int ChkMikakutei { get; set; }
        public int ChkKanbai { get; set; }
        public int ChkFuyo { get; set; }
        public int ChkNyukaZumi { get; set; }
        public int ChkMiNyuka { get; set; }
        public int ChkJuchuAri { get; set; }
        public int ChkZaiko { get; set; }
        public int ChkMisyonin { get; set; }
        public int ChkSyoninzumi { get; set; }
        public int ChkChokuso { get; set; }
        public int ChkSouko { get; set; }
        public int ChkNet { get; set; }
        public int ChkFax { get; set; }
        public int ChkEdi { get; set; }

        public string JuchuuNO { get; set; }
        public string ArrivalPlanCD { get; set; }

        //回答納期登録
        public  string ArrivalPlanMonthFrom { get; set; }
        public string ArrivalPlanMonthTo { get; set; }
        public string OrderNoFrom { get; set; }
        public string OrderNoTo { get; set; }
        public string EDIDate { get; set; }

        //EDI回答納期登録
        public string OrderRows { get; set; }
        public string OrderSu { get; set; }

        //入荷入力
        public string AdminNO { get; set; }
        public string MakerItem { get; set; }
        public string SKUCD { get; set; }
        public string SKUName { get; set; }
        public string JANCD { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string OrderSuu { get; set; }		
        public string OrderUnitPrice { get; set; }
        public string TaniCD { get; set; }
        public string PriceOutTax { get; set; }
        public string Rate { get; set; }
        public string OrderTax { get; set; }
        public string OrderTaxRitsu { get; set; }
        public string OriginalArrivalPlanNO        { get; set; }

    }
}
