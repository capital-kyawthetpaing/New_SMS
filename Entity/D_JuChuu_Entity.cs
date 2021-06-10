using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Juchuu_Entity : Base_Entity
    {
        public string JuchuuNO { get; set; }
        public string JuchuuProcessNO { get; set; }
        public string StoreCD { get; set; }
        public string JuchuuDate { get; set; }
        public string JuchuuTime { get; set; }
        public string ReturnFLG { get; set; }
        public string SoukoCD { get; set; }
        public string JuchuuKBN { get; set; }
        public string SiteKBN { get; set; }
        public string SiteJuchuuDateTime { get; set; }
        public string SiteJuchuuNO { get; set; }
        public string InportErrFLG { get; set; }
        public string OnHoldFLG { get; set; }
        public string IdentificationFLG { get; set; }
        public string TorikomiDateTime { get; set; }
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
        public string CustomerKanaName { get; set; }
        public string DeliveryCD { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryName2 { get; set; }
        public string DeliveryAliasKBN { get; set; }
        public string DeliveryZipCD1 { get; set; }
        public string DeliveryZipCD2 { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryTel11 { get; set; }
        public string DeliveryTel12 { get; set; }
        public string DeliveryTel13 { get; set; }
        public string DeliveryTel21 { get; set; }
        public string DeliveryTel22 { get; set; }
        public string DeliveryTel23 { get; set; }
        public string DeliveryKanaName { get; set; }
        public string JuchuuCarrierCD { get; set; }
        public string DecidedCarrierFLG { get; set; }
        public string LastCarrierCD { get; set; }
        public string NameSortingDateTime { get; set; }
        public string NameSortingStaffCD { get; set; }
        public string CurrencyCD { get; set; }
        public string JuchuuGaku { get; set; }
        public string Discount { get; set; }
        public string HanbaiHontaiGaku { get; set; }
        public string HanbaiTax8 { get; set; }
        public string HanbaiTax10 { get; set; }
        public string HanbaiGaku { get; set; }
        public string CostGaku { get; set; }
        public string ProfitGaku { get; set; }
        public string Coupon { get; set; }
        public string Point { get; set; }
        public string PayCharge { get; set; }
        public string Adjustments { get; set; }
        public string Postage { get; set; }
        public string GiftWrapCharge { get; set; }
        public string InvoiceGaku { get; set; }
        public string PaymentMethodCD { get; set; }
        public string PaymentPlanNO { get; set; }
        public string CardProgressKBN { get; set; }
        public string CardCompany { get; set; }
        public string CardNumber { get; set; }
        public string PaymentProgressKBN { get; set; }
        public string PresentFLG { get; set; }
        public string SalesPlanDate { get; set; }
        public string FirstPaypentPlanDate { get; set; }
        public string LastPaymentPlanDate { get; set; }
        public string DemandProgressKBN { get; set; }
        public string CommentDemand { get; set; }
        public string CancelDate { get; set; }
        public string CancelReasonKBN { get; set; }
        public string CancelRemarks { get; set; }
        public string NoMailFLG { get; set; }
        public string IndividualContactKBN { get; set; }
        public string TelephoneContactKBN { get; set; }
        public string LastMailKBN { get; set; }
        public string LastMailPatternCD { get; set; }
        public string LastMailDatetime { get; set; }
        public string LastMailName { get; set; }
        public string NextMailKBN { get; set; }
        public string CommentOutStore { get; set; }
        public string CommentInStore { get; set; }
        public string LastDepositeDate { get; set; }
        public string LastOrderDate { get; set; }
        public string LastArriveDate { get; set; }
        public string LastSalesDate { get; set; }
        public string MitsumoriNO { get; set; }
        public string TenzikaiJuchuuNO { get; set; }
        public string MailAddress { get; set; }
        public string CommentCustomer { get; set; }
        public string CommentCapital { get; set; }
        public string KaolaetcFLG { get; set; }
        public string NayoseKekkaTourokuDate { get; set; }

        public string JuchuuDateTime { get; set; }

        //[D_StoreJuchuu]
        public string NouhinsyoComment { get; set; }

        //検索用Entity
        public string JuchuDateFrom { get; set; }
        public string JuchuDateTo { get; set; }

        public int ValGaisho { get; set; }
        public int ValTento { get; set; }
        public int ValWeb { get; set; }
        public string VendorName { get; set; }
        //帳票用Entity
        public string PrintFLG { get; set; }

        //照会用Entity
        public string KanaName { get; set; }
        public string VendorCD { get; set; }
        public string JuchuuProcessNOFrom { get; set; }
        public string JuchuuProcessNOTo { get; set; }
        public string JuchuuNOFrom { get; set; }
        public string JuchuuNOTo { get; set; }
        public string SalesDateFrom { get; set; }
        public string SalesDateTo { get; set; }
        public string BillingCloseDateFrom { get; set; }
        public string BillingCloseDateTo { get; set; }
        public string CollectClearDateFrom { get; set; }
        public string CollectClearDateTo { get; set; }

        public int ChkMihikiate { get; set; }
        public int ChkMiuriage { get; set; }
        public int ChkMiseikyu { get; set; }
        public int ChkMinyukin { get; set; }
        public int ChkAll { get; set; }
        public int ChkReji { get; set; }
        public int ChkGaisho { get; set; }
        public int ChkTujo { get; set; }
        public int ChkHenpin { get; set; }

        public int ChkMihachu { get; set; }
        public int ChkNokiKaito { get; set; }
        public int ChkMinyuka { get; set; }
        public int ChkMisiire { get; set; }
        public int ChkHachuAll { get; set; }

        //WEB受注確認用
        public string SiteJuchuuDateFrom { get; set; }
        public string SiteJuchuuDateTo { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string NyukinDateFrom { get; set; }
        public string NyukinDateTo { get; set; }

        public string DecidedDeliveryDateFrom { get; set; }
        public string DecidedDeliveryDateTo { get; set; }
        public string DeliveryPlanDateFrom { get; set; }
        public string DeliveryPlanDateTo { get; set; }
        public string DeliveryDateFrom { get; set; }
        public string DeliveryDateTo { get; set; }
        public string InvoiceNOFrom { get; set; }
        public string InvoiceNOTo { get; set; }
        public string ComboBox8 { get; set; }//メール発送状況							
        public string ComboBox7 { get; set; }//警告状況							
        public string ComboBox6 { get; set; }//配送方法						
        public string ComboBox5 { get; set; }//入金状況					

        public string IncludeFLG { get; set; }
        public string GiftFLG { get; set; }
        public string NoshiFLG { get; set; }
        public string NouhinsyoFLG { get; set; }//SeikyuusyoFLG
        public string RyousyusyoFLG { get; set; }
        public string SonotoFLG { get; set; }


        public string CancelFLG { get; set; }

    }
}
