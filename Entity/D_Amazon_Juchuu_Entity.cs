using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Entity
{
   public class D_Amazon_Juchuu_Entity:Base_Entity
    {
        public string InportSEQ { get; set; }
        public string StoreCD { get; set; }
        public string APIKey { get; set; }
        public string InportSEQRows { get; set; }
        public string AmazonOrderId { get; set; }
        public string JuchuuNO { get; set; }
        public string SellerOrderId { get; set; }
        public string PurchaseDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string OrderStatus { get; set; }
        public string FulfillmentChannel { get; set; }
        public string SalesChannel { get; set; }
        public string OrderChannel { get; set; }
        public string ShipServiceLevel { get; set; }
        public string AddressName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressCounty { get; set; }
        public string AddressDistrict { get; set; }
        public string StateOrRegion { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
        public string NumberOfItemsShipped { get; set; }
        public string NumberOfItemsUnshipped { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodDetail { get; set; }
        public string PaymentCurrencyCode1 { get; set; }
        public string PaymentAmount1 { get; set; }
        public string PaymentMethod1 { get; set; }
        public string PaymentCurrencyCode2 { get; set; }
        public string PaymentAmount2 { get; set; }
        public string PaymentMethod2 { get; set; }
        public string PaymentCurrencyCode3 { get; set; }
        public string PaymentAmount3 { get; set; }
        public string PaymentMethod3 { get; set; }
        public string PaymentCurrencyCode4 { get; set; }
        public string PaymentAmount4 { get; set; }
        public string PaymentMethod4 { get; set; }
        public string IsReplacementOrder { get; set; }
        public string ReplacedOrderId { get; set; }
        public string MarketplaceId { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public string CompanyLegalName { get; set; }
        public string TaxingRegion { get; set; }
        public string TaxClassificationName { get; set; }
        public string TaxClassificationValue { get; set; }
        public string ShipmentServiceLevelCategory { get; set; }
        public string OrderType { get; set; }
        public string EarliestShipDate { get; set; }
        public string LatestShipDate { get; set; }
        public string EarliestDeliveryDate { get; set; }
        public string LatestDeliveryDate { get; set; }
        public string IsBusinessOrder { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string IsPrime { get; set; }
        public string IsPremiumOrder { get; set; }
        public string PromiseResponseDueDate { get; set; }
        public string IsEstimatedShipDateSet { get; set; }



    }
}
