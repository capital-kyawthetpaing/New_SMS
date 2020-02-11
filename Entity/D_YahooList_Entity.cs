using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class D_YahooList_Entity :Base_Entity
    {
        public D_YahooList_Entity()
        {
            ////string a1 = "OrderId,Version,ParentOrderId,ChildOrderId,DeviceType,MobileCarrierName,IsSeen,IsSplit,CancelReason,CancelReasonDetail";
            ////string a2 = ",IsRoyalty,IsRoyaltyFix,IsSeller,IsAffiliate,IsRatingB2s,NeedSnl,OrderTime,LastUpdateTime,SuspectMessage,OrderStatus,StoreStatus,RoyaltyFixTime,";
            ////string a3 = "SendConfirmTime,SendPayTime,PrintSlipTime,PrintDeliveryTime,PrintBillTime,BuyerComments,SellerComments,Notes,OperationUser,Referer";
            ////string b1 = ",EntryPoint,HistoryId,UsageId,UseCouponData,TotalCouponDiscount,ShippingCouponFlg,ShippingCouponDiscount,CampaignPoints,IsMultiShip,MultiShipId,";
            ////string b2 = "IsReadOnly,PayStatus,SettleStatus,PayType,PayKind,PayMethod,PayMethodName,SellerHandlingCharge,PayActionTime,PayDate,PayNotes,SettleId,";
            ////string b3 = "CardBrand,CardNumber,CardNumberLast4,CardExpireYear,CardExpireMonth,CardPayType,CardHolderName,CardPayCount,CardBirthDay,UseYahooCard,UseWallet,NeedBillSlip,";
            ////string c1 = "NeedDetailedSlip,NeedReceipt,AgeConfirmField,AgeConfirmCheck,BillAddressFrom,BillFirstName,BillFirstNameKana,BillLastName,BillLastNameKana,BillZipCode,BillPrefecture,BillCity";
            ////string c2 = ",BillCityKana,BillAddress1,BillAddress1Kana,BillAddress2,BillAddress2Kana,BillPhoneNumber,BillEmgPhoneNumber,BillMailAddress,BillSection1Field,BillSection1Value";
            ////string c3 = ",BillSection2Field,BillSection2Value,PayNo,PayNoIssueDate,ConfirmNumber,PaymentTerm,IsApplePay,ShipStatus,ShipMethod,ShipMethodName";
            ////string d1 = ",ShipRequestDate,ShipRequestTime,ShipNotes,ShipCompanyCode,ShipInvoiceNumber1,ShipInvoiceNumber2,ShipInvoiceNumberEmptyReason,ShipUrl,ArriveType";
            ////string d2 = ",ShipDate,ArrivalDate,NeedGiftWrap,GiftWrapType,GiftWrapMessage,NeedGiftWrapPaper,GiftWrapPaperType,GiftWrapName,Option1Field,Option1Type,Option1Value";
            ////string d3 = ",Option2Field,Option2Type,Option2Value,ShipFirstName,ShipFirstNameKana,ShipLastName,ShipLastNameKana,ShipZipCode,ShipPrefecture,ShipPrefectureKana,ShipCity,ShipCityKana,ShipAddress1";
            ////string e1 = ",ShipAddress1Kana,ShipAddress2,ShipAddress2Kana,ShipPhoneNumber,ShipEmgPhoneNumber,ShipSection1Field,ShipSection1Value,ShipSection2Field,ShipSection2Value,PayCharge,ShipCharge";
            ////string e2 = ",GiftWrapCharge,Discount,Adjustments,SettleAmount,UsePoint,TotalPrice,SettlePayAmount,TaxRatio,IsGetPointFixAll,TotalMallCouponDiscount,SellerId,IsLogin,FspLicenseCode,FspLicenseName,GuestAuthId,CombinedPayType,CombinedPayKind,CombinedPayMethod,PayMethodAmount,CombinedPayMethodName,CombinedPayMethodAmount";

            
        }
        string a1 = "OrderId,Version,ParentOrderId,ChildOrderId,DeviceType,MobileCarrierName,IsSeen,IsSplit,CancelReason,CancelReasonDetail";

        public string OrderId { get; set; }
        public string Version { get; set; }
        public string ParentOrderId { get; set; }
        public string ChildOrderId { get; set; }
        public string DeviceType { get; set; }
        public string MobileCarrierName { get; set; }
        public string IsSeen { get; set; }
        public string IsSplit { get; set; }
        public string CancelReason { get; set; }
        public string CancelReasonDetail { get; set; }

        string a2 = ",IsRoyalty,IsRoyaltyFix,IsSeller,IsAffiliate,IsRatingB2s,NeedSnl,OrderTime,LastUpdateTime,SuspectMessage,OrderStatus,StoreStatus,RoyaltyFixTime,";

        public string IsRoyalty { get; set; }
        public string IsRoyaltyFix { get; set; }
        public string IsSeller { get; set; }
        public string IsAffiliate { get; set; }
        public string IsRatingB2s { get; set; }
        public string NeedSnl { get; set; }
        public string OrderTime { get; set; }
        public string LastUpdateTime { get; set; }
        public string SuspectMessage { get; set; }
        public string OrderStatus { get; set; }
        public string StoreStatus { get; set; }
        public string RoyaltyFixTime { get; set; }

        string a3 = "SendConfirmTime,SendPayTime,PrintSlipTime,PrintDeliveryTime,PrintBillTime,BuyerComments,SellerComments,Notes,OperationUser,Referer";

        public string SendConfirmTime { get; set; }
        public string SendPayTime { get; set; }
        public string PrintSlipTime { get; set; }
        public string PrintDeliveryTime { get; set; }
        public string PrintBillTime { get; set; }
        public string BuyerComments { get; set; }
        public string SellerComments { get; set; }
        public string Notes { get; set; }
        public string OperationUser { get; set; }
        public string Referer { get; set; }

        string b1 = ",EntryPoint,HistoryId,UsageId,UseCouponData,TotalCouponDiscount,ShippingCouponFlg,ShippingCouponDiscount,CampaignPoints,IsMultiShip,MultiShipId,";


        public string EntryPoint { get; set; }
        public string HistoryId { get; set; }
        public string UsageId { get; set; }
        public string UseCouponData { get; set; }
        public string TotalCouponDiscount { get; set; }
        public string ShippingCouponFlg { get; set; }
        public string ShippingCouponDiscount { get; set; }
        public string CampaignPoints { get; set; }
        public string IsMultiShip { get; set; }
        public string MultiShipId { get; set; }

        string b2 = "IsReadOnly,PayStatus,SettleStatus,PayType,PayKind,PayMethod,PayMethodName,SellerHandlingCharge,PayActionTime,PayDate,PayNotes,SettleId,";

        public string IsReadOnly { get; set; }
        public string PayStatus { get; set; }
        public string SettleStatus { get; set; }
        public string PayType { get; set; }
        public string PayKind { get; set; }
        public string PayMethod { get; set; }
        public string PayMethodName { get; set; }
        public string SellerHandlingCharge { get; set; }
        public string PayActionTime { get; set; }
        public string PayDate { get; set; }
        public string PayNotes { get; set; }
        public string SettleId { get; set; }

        string b3 = "CardBrand,CardNumber,CardNumberLast4,CardExpireYear,CardExpireMonth,CardPayType,CardHolderName,CardPayCount,CardBirthDay,UseYahooCard,UseWallet,NeedBillSlip,";

        public string CardBrand { get; set; }
        public string CardNumber { get; set; }
        public string CardNumberLast4 { get; set; }
        public string CardExpireYear { get; set; }
        public string CardExpireMonth { get; set; }
        public string CardPayType { get; set; }
        public string CardHolderName { get; set; }
        public string CardPayCount { get; set; }
        public string CardBirthDay { get; set; }
        public string UseYahooCard { get; set; }
        public string UseWallet { get; set; }
        public string NeedBillSlip { get; set; }
        string c1 = "NeedDetailedSlip,NeedReceipt,AgeConfirmField,AgeConfirmCheck,BillAddressFrom,BillFirstName,BillFirstNameKana,BillLastName,BillLastNameKana,BillZipCode,BillPrefecture,BillCity";

        public string NeedDetailedSlip { get; set; }
        public string NeedReceipt { get; set; }
        public string AgeConfirmField { get; set; }
        public string AgeConfirmCheck { get; set; }
        public string BillAddressFrom { get; set; }
        public string BillFirstName { get; set; }
        public string BillFirstNameKana { get; set; }
        public string BillLastName { get; set; }
        public string BillLastNameKana { get; set; }
        public string BillZipCode { get; set; }
        public string BillPrefecture { get; set; }
        public string BillCity { get; set; }

        string c2 = ",BillCityKana,BillAddress1,BillAddress1Kana,BillAddress2,BillAddress2Kana,BillPhoneNumber,BillEmgPhoneNumber,BillMailAddress,BillSection1Field,BillSection1Value";


        public string BillCityKana { get; set; }
        public string BillAddress1 { get; set; }
        public string BillAddress1Kana { get; set; }
        public string BillAddress2 { get; set; }
        public string BillAddress2Kana { get; set; }
        public string BillPhoneNumber { get; set; }
        public string BillEmgPhoneNumber { get; set; }
        public string BillMailAddress { get; set; }
        public string BillSection1Field { get; set; }
        public string BillSection1Value { get; set; }

        string c3 = ",BillSection2Field,BillSection2Value,PayNo,PayNoIssueDate,ConfirmNumber,PaymentTerm,IsApplePay,ShipStatus,ShipMethod,ShipMethodName";

        public string BillSection2Field { get; set; }
        public string BillSection2Value { get; set; }
        public string PayNo { get; set; }
        public string PayNoIssueDate { get; set; }
        public string ConfirmNumber { get; set; }
        public string PaymentTerm { get; set; }
        public string IsApplePay { get; set; }
        public string ShipStatus { get; set; }
        public string ShipMethod { get; set; }
        public string ShipMethodName { get; set; }

        string d1 = ",ShipRequestDate,ShipRequestTime,ShipNotes,ShipCompanyCode,ShipInvoiceNumber1,ShipInvoiceNumber2,ShipInvoiceNumberEmptyReason,ShipUrl,ArriveType";

        public string ShipRequestDate { get; set; }
        public string ShipRequestTime { get; set; }
        public string ShipNotes { get; set; }
        public string ShipCompanyCode { get; set; }
        public string ShipInvoiceNumber1 { get; set; }
        public string ShipInvoiceNumber2 { get; set; }
        public string ShipInvoiceNumberEmptyReason { get; set; }
        public string ShipUrl { get; set; }
        public string ArriveType { get; set; }


        string d2 = ",ShipDate,ArrivalDate,NeedGiftWrap,GiftWrapType,GiftWrapMessage,NeedGiftWrapPaper,GiftWrapPaperType,GiftWrapName,Option1Field,Option1Type,Option1Value";

        public string ShipDate { get; set; }
        public string ArrivalDate { get; set; }
        public string NeedGiftWrap { get; set; }
        public string GiftWrapType { get; set; }
        public string GiftWrapMessage { get; set; }
        public string NeedGiftWrapPaper { get; set; }
        public string GiftWrapPaperType { get; set; }
        public string GiftWrapName { get; set; }
        public string Option1Field { get; set; }
        public string Option1Type { get; set; }
        public string Option1Value { get; set; }
        string d3 = ",Option2Field,Option2Type,Option2Value,ShipFirstName,ShipFirstNameKana,ShipLastName,ShipLastNameKana,ShipZipCode,ShipPrefecture,ShipPrefectureKana,ShipCity,ShipCityKana,ShipAddress1";


        public string Option2Field { get; set; }
        public string Option2Type { get; set; }
        public string Option2Value { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipFirstNameKana { get; set; }
        public string ShipLastName { get; set; }
        public string ShipLastNameKana { get; set; }
        public string ShipZipCode { get; set; }
        public string ShipPrefecture { get; set; }
        public string ShipPrefectureKana { get; set; }
        public string ShipCity { get; set; }
        public string ShipCityKana { get; set; }
        public string ShipAddress1 { get; set; }

        string e1 = ",ShipAddress1Kana,ShipAddress2,ShipAddress2Kana,ShipPhoneNumber,ShipEmgPhoneNumber,ShipSection1Field,ShipSection1Value,ShipSection2Field,ShipSection2Value,PayCharge,ShipCharge";

        public string ShipAddress1Kana { get; set; }
        public string ShipAddress2 { get; set; }
        public string ShipAddress2Kana { get; set; }
        public string ShipPhoneNumber { get; set; }
        public string ShipEmgPhoneNumber { get; set; }
        public string ShipSection1Field { get; set; }
        public string ShipSection1Value { get; set; }
        public string ShipSection2Field { get; set; }
        public string ShipSection2Value { get; set; }
        public string PayCharge { get; set; }
        public string ShipCharge { get; set; }

        string e2 = ",GiftWrapCharge,Discount,Adjustments,SettleAmount,UsePoint,TotalPrice,SettlePayAmount,TaxRatio,IsGetPointFixAll,TotalMallCouponDiscount,SellerId,IsLogin,FspLicenseCode,FspLicenseName,GuestAuthId,CombinedPayType,CombinedPayKind,CombinedPayMethod,PayMethodAmount,CombinedPayMethodName,CombinedPayMethodAmount";

        //CombinedPayType,CombinedPayKind,CombinedPayMethod,PayMethodAmount,CombinedPayMethodName,CombinedPayMethodAmount";
        public string GiftWrapCharge { get; set; }
        public string Discount { get; set; }
        public string Adjustments { get; set; }
        public string SettleAmount { get; set; }
        public string UsePoint { get; set; }
        public string TotalPrice { get; set; }
        public string SettlePayAmount { get; set; }
        public string TaxRatio { get; set; }
        public string IsGetPointFixAll { get; set; }
        public string TotalMallCouponDiscount { get; set; }
        public string SellerId { get; set; }
        public string IsLogin { get; set; }
        public string FspLicenseCode { get; set; }

        public string FspLicenseName { get; set; }
        public string GuestAuthId { get; set; }
        public string CombinedPayType { get; set; }
        public string CombinedPayKind { get; set; }
        public string CombinedPayMethod { get; set; }
        public string PayMethodAmount { get; set; }
        public string CombinedPayMethodName { get; set; }
        public string CombinedPayMethodAmount { get; set; }
    }
}
