using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BL;
using Entity;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;
using MarketplaceWebService;
using MarketplaceWebService.Mock;
using MarketplaceWebService.Model;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
namespace Amazon__API
{
    public class CommonAPI
    {
        Amazon__BL aml;
        Amazon__Entity ame;
        DateTime UpdatedTimeBefore;

        static string SellerId = "";
        static string MarketplaceId = "";
        static string AccessKeyId = "";
        static string MWSAuthToken = "";
        static string SecretKeyId = "";

        static string ApplicationVersion = "";
        static string ApplicationName = "";
        static int TokenNo = 0;
        static string APIKey = "";
        static string StoreCD = "";
        public void ListOrders()  
        {

            ame = new Amazon__Entity();
            aml = new Amazon__BL();

            ame = aml.MAPI_DRequest();
            //SellerId = CommonValue.strMerchantId;
            //MarketplaceId = CommonValue.strMarketplaceId;
            //AccessKeyId = CommonValue.strAccessKeyId;
            //MWSAuthToken = CommonValue.strMWSAuthToken;
            //SecretKeyId = CommonValue.strSecretKeyId;
            //ApplicationVersion = CommonValue.strApplicationVersion;
            //ApplicationName = CommonValue.strApplicationName;
            //string strbuff = string.Empty;
            SellerId = ame.strMerchantId;
            MarketplaceId = ame.strMarketplaceId;
            AccessKeyId = ame.strAccessKeyId;
            MWSAuthToken = ame.strMWSAuthToken;
            SecretKeyId = ame.strSecretKeyId;
            ApplicationVersion = CommonValue.strApplicationVersion;
            ApplicationName = CommonValue.strApplicationName;
            APIKey                               = ame.APIKey;
            StoreCD                              =ame.StoreCD;
  

            DataTable strbuff = new DataTable();
           
            var LastUpdatedBefore = aml.Amazon_DRequest();
            if (LastUpdatedBefore != null)
            {
                UpdatedTimeBefore = Convert.ToDateTime(LastUpdatedBefore).AddDays(-1);
            }
            else
            {
                UpdatedTimeBefore = DateTime.Now; ;
            }
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = CommonValue.strServiceURL;
            MarketplaceWebServiceOrders.MarketplaceWebServiceOrders client = new MarketplaceWebServiceOrdersClient(
                                                                                    AccessKeyId,
                                                                                    SecretKeyId,
                                                                                    ApplicationName,
                                                                                    ApplicationVersion,
                                                                                    config);
            ListOrdersRequest request = new ListOrdersRequest();
            request.SellerId = SellerId;
            request.LastUpdatedAfter = UpdatedTimeBefore;

            List<string> lstMarketplace = new List<string>();
            lstMarketplace.Add(MarketplaceId);
            request.MarketplaceId = lstMarketplace;
            request.MWSAuthToken = MWSAuthToken;
            request.MaxResultsPerPage = 40;
            ListOrdersResponse response = client.ListOrders(request);
            ListOrdersResult listOrdersResult = new ListOrdersResult();
            if (response.IsSetListOrdersResult())
            {
                 listOrdersResult = response.ListOrdersResult;
                if (listOrdersResult.IsSetOrders())
                {
                    List<Order> orders = listOrdersResult.Orders;
                    strbuff.Columns.Add("StoreCD");
                    strbuff.Columns.Add("APIKey");
                    strbuff.Columns.Add("SEQ");
                    strbuff.Columns.Add("OrderId");
                    int i = 0;
                    Amazon_Juchuu = D_AmazonJuchuu();
                    foreach (Order o in orders)
                    {
                        i++;
                        strbuff.Rows.Add(StoreCD, APIKey, i, o.AmazonOrderId);
                        GetListOrderdata(o, i);
                    }
                }
               // txtListOrders.Text = strbuff;
            }
            Base_BL bbl = new Base_BL();
            string OrderDetails = "";string AmazonOrderId="" ;

            OrderDetails = bbl.DataTableToXml(Amazon_Juchuu);
            AmazonOrderId = bbl.DataTableToXml(strbuff);
            TokenNo = TokenNo + 1;
            Insert_FirstToken(listOrdersResult, OrderDetails, AmazonOrderId);

            if (listOrdersResult.NextToken != null)
            {
                Insert_NextToken(response.ListOrdersResult.NextToken);
            }

            Insert_Items(client);

            //Console.Read();


            //
            //else
            //{
            //    Console.Write("Order Inserted Successfully!!!");
            //    Console.Read();
            //}
        }
    
        public void Insert_NextToken(string token =null)
        {
            Amazon_Juchuu_NextToken = D_AmazonJuchuu();
            Base_BL bbl = new Base_BL();
            DataTable strbuff = new DataTable();
            strbuff.Columns.Add("StoreCD");
            strbuff.Columns.Add("APIKey");
            strbuff.Columns.Add("SEQ");
            strbuff.Columns.Add("OrderId");
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = CommonValue.strServiceURL;
            MarketplaceWebServiceOrders.MarketplaceWebServiceOrders client = new MarketplaceWebServiceOrdersClient(
                                                                                    AccessKeyId,
                                                                                    SecretKeyId,
                                                                                    ApplicationName,
                                                                                    ApplicationVersion,
                                                                                    config);

            if (token != null)
            {
                ListOrdersByNextTokenRequest request1 = new ListOrdersByNextTokenRequest();
                ListOrdersByNextTokenResult listOrdersByNextResult = new ListOrdersByNextTokenResult();
                request1.SellerId = SellerId;
                request1.MWSAuthToken = MWSAuthToken;
                request1.NextToken = token;
                ListOrdersByNextTokenResponse response1 = client.ListOrdersByNextToken(request1);
                if (response1.IsSetListOrdersByNextTokenResult())
                {
                    listOrdersByNextResult = response1.ListOrdersByNextTokenResult;
                    if (listOrdersByNextResult.IsSetOrders())
                    {
                        List<Order> orders = listOrdersByNextResult.Orders;
                        int i = 0;
                        foreach (Order o in orders)
                        {
                            i++;
                            strbuff.Rows.Add(StoreCD,APIKey, i, o.AmazonOrderId);
                            GetListOrderdata(o, i, false);
                        }
                    }
                }
                Amazon__Entity ameDetails = new Amazon__Entity();
                ameDetails.StoreCD = StoreCD;
                ameDetails.APIKey = APIKey;
                ameDetails.LastUpdatedAfter = UpdatedTimeBefore.ToString();
                ameDetails.LastUpdatedBefore = listOrdersByNextResult.LastUpdatedBefore.ToString();
                ameDetails.Xmldetails = bbl.DataTableToXml(Amazon_Juchuu_NextToken);
                ameDetails.XmlOrder = bbl.DataTableToXml(strbuff);

                TokenNo = TokenNo + 1;
                if (aml.AmazonAPI_Insert_NextToken(ameDetails))
                {
                    Console.WriteLine("Successfully Inserted Token " + (TokenNo).ToString() + "Times");
                }
                else
                {
                    Console.WriteLine("Unfortunately Inserted Failed Token " + (TokenNo).ToString() + "Times");
                }
                Insert_NextToken(listOrdersByNextResult.NextToken);
            }
            else/// To be Continued. . . Insert  
            {
                Console.Write("Order inserted Successfully!!!");
                Console.Read();
            }
        }

        protected void Insert_Items(MarketplaceWebServiceOrders.MarketplaceWebServiceOrders client)
        {
            ListOrderItemsRequest request = new ListOrderItemsRequest();

            request.SellerId = SellerId;

            Base_BL bbl = new Base_BL();

            aml = new Amazon__BL();
            Amazon_Juchuu_Items = D_AmazonJuchuuItems();
            var dtAmazonOrderId = aml.Select_AllOrderByLastRireki();
            int j = 0; int i = 0;
            foreach (DataRow dr in dtAmazonOrderId.Rows)
            {
                j++;
                request.AmazonOrderId = dr["AmazonOrderId"].ToString();

                ListOrderItemsResponse response = client.ListOrderItems(request);
                if (response.IsSetListOrderItemsResult())
                {
                    ListOrderItemsResult listOrderItemsResult = response.ListOrderItemsResult;
                    if (listOrderItemsResult.IsSetOrderItems())
                    {
                        List<OrderItem> orderItems = listOrderItemsResult.OrderItems;
                      
                        foreach (OrderItem orderItem in orderItems)
                        {
                            i++;
                            Amazon_Juchuu_Items_Add(request.AmazonOrderId,orderItem, j,i);
                          //  strbuff += "商品名：" + orderItem.Title + System.Environment.NewLine;
                        }
                    }
                }
            }


            Amazon__Entity ameDetails = new Amazon__Entity();
            ameDetails.StoreCD = StoreCD;
            ameDetails.APIKey =  APIKey;
            ameDetails.XmlOrderItems = bbl.DataTableToXml(Amazon_Juchuu_Items);
            if (aml.Amazon_InsertOrderItemDetails(ameDetails))
            {
                Console.Write("All Items are Imported Successffully!!!");
              //  Console.ReadLine();
            }

        }

        protected DataTable Amazon_Juchuu_Items_Add(string orderId, OrderItem o, int j,int i)
        {
            try
            {
                Amazon_Juchuu_Items.Rows.Add(StoreCD, APIKey, i.ToString(), orderId, j,
                    (o.IsSetASIN()) ? o.ASIN : null,
                    o.IsSetOrderItemId() ? o.OrderItemId : null,
                    o.IsSetSellerSKU() ? o.SellerSKU : null,
                    o.IsSetBuyerCustomizedInfo() ? o.BuyerCustomizedInfo.CustomizedURL : null,
                    o.IsSetTitle() ? o.Title : null,
                    o.IsSetQuantityOrdered() ? (Convert.ToInt32(o.QuantityOrdered.ToString()) == 1).ToString()== "True"? "1":"0" : null,
                    o.IsSetQuantityShipped() ? (Convert.ToInt32(o.QuantityShipped.ToString()) == 1).ToString() == "True" ? "1" : "0" : null,
                    o.IsSetPointsGranted() ? (o.PointsGranted.IsSetPointsNumber()? Convert.ToInt32(o.PointsGranted.PointsNumber).ToString(): "0") : "0",
                    o.IsSetPointsGranted() ? (o.PointsGranted.IsSetPointsMonetaryValue() ? o.PointsGranted.PointsMonetaryValue.IsSetCurrencyCode() ? o.PointsGranted.PointsMonetaryValue.CurrencyCode : null : null) : null,
                    o.IsSetPointsGranted() ? (o.PointsGranted.IsSetPointsMonetaryValue() ? o.PointsGranted.PointsMonetaryValue.IsSetAmount() ? o.PointsGranted.PointsMonetaryValue.Amount : null : null) : null,
                    "0",// o.PromotionInfos.NumberOfItems. . .,
                    o.IsSetItemPrice() ? o.ItemPrice.IsSetCurrencyCode() ? o.ItemPrice.CurrencyCode : null : null,
                    o.IsSetItemPrice() ? o.ItemPrice.IsSetAmount() ? o.ItemPrice.Amount : null : null,
                    o.IsSetShippingPrice() ? o.ShippingPrice.IsSetCurrencyCode() ? o.ShippingPrice.CurrencyCode : null : null,
                    o.IsSetShippingPrice() ? o.ShippingPrice.IsSetAmount() ? o.ShippingPrice.Amount : null : null,
                    o.IsSetGiftWrapPrice() ? o.GiftWrapPrice.IsSetCurrencyCode() ? o.GiftWrapPrice.CurrencyCode : null : null,
                    o.IsSetGiftWrapPrice() ? o.GiftWrapPrice.IsSetAmount() ? o.GiftWrapPrice.Amount : null : null,
                    null,// o.IsSetTaxCollection.model. . . ,
                    null,// o.IsSetTaxCollection.responsibleParty. . . ,
                    o.IsSetItemTax() ? o.ItemTax.IsSetCurrencyCode() ? o.ItemTax.CurrencyCode : null : null,
                    o.IsSetItemTax() ? o.ItemTax.IsSetAmount() ? o.ItemTax.Amount : null : null,
                    o.IsSetShippingTax() ? o.ShippingTax.IsSetCurrencyCode() ? o.ShippingTax.CurrencyCode : null : null,
                    o.IsSetShippingTax() ? o.ShippingTax.IsSetAmount() ? o.ShippingTax.Amount : null : null,
                    o.IsSetGiftWrapTax() ? o.GiftWrapTax.IsSetCurrencyCode() ? o.GiftWrapTax.CurrencyCode : null : null,
                    o.IsSetGiftWrapTax() ? o.GiftWrapTax.IsSetAmount() ? o.GiftWrapTax.Amount : null : null,
                    o.IsSetShippingDiscount() ? o.ShippingDiscount.IsSetCurrencyCode() ? o.ShippingDiscount.CurrencyCode : null : null,
                    o.IsSetShippingDiscount() ? o.ShippingDiscount.IsSetAmount() ? o.ShippingDiscount.Amount : null : null,
                    o.IsSetPromotionDiscount() ? o.PromotionDiscount.IsSetCurrencyCode() ? o.PromotionDiscount.CurrencyCode : null : null,
                    o.IsSetPromotionDiscount() ? o.PromotionDiscount.IsSetAmount() ? o.PromotionDiscount.Amount : null : null,
                    null,//  o.PromotionIds. . .,
                    o.IsSetCODFee() ? o.CODFee.IsSetCurrencyCode() ? o.CODFee.CurrencyCode : null : null,
                    o.IsSetCODFee() ? o.CODFee.IsSetAmount() ? o.CODFee.Amount : null : null,
                    o.IsSetCODFeeDiscount() ? o.CODFeeDiscount.IsSetCurrencyCode() ? o.CODFeeDiscount.CurrencyCode : null : null,
                    o.IsSetCODFeeDiscount()? o.CODFeeDiscount.IsSetAmount()?o.CODFeeDiscount.Amount:null:null,
                    "0",//o.IsGift()? . . .,
                    o.IsSetGiftMessageText()? o.GiftMessageText  : null,
                    o.IsSetGiftWrapLevel()?o.GiftWrapLevel:null,
                    o.IsSetConditionNote()?o.ConditionNote:null,
                    o.IsSetConditionId()?o.ConditionId:null,
                    o.IsSetConditionSubtypeId()?o.ConditionSubtypeId:null,
                    o.IsSetScheduledDeliveryStartDate()? o.ScheduledDeliveryStartDate:null,
                    o.IsSetScheduledDeliveryEndDate()?o.ScheduledDeliveryEndDate:null,
                    o.IsSetPriceDesignation()? o.PriceDesignation:null



                    );
            }
            catch(Exception ex)
            {


            }
            


            return Amazon_Juchuu_Items;
        }
        protected void Insert_FirstToken(ListOrdersResult respone,string OrderDetails, string AmazonOrderId)
        {
           Amazon__Entity ameDetails = new Amazon__Entity();
            ameDetails.StoreCD = StoreCD;
            ameDetails.APIKey = APIKey;
            ameDetails.LastUpdatedAfter = UpdatedTimeBefore.ToString();
            ameDetails.LastUpdatedBefore = respone.LastUpdatedBefore.ToString();
            ameDetails.Xmldetails = OrderDetails;
            ameDetails.XmlOrder = AmazonOrderId;
            if (aml.AmazonAPI_InsertOrderDetails(ameDetails))
            {

                Console.WriteLine("Successfully Inserted Token " + (TokenNo).ToString() + "Times");
            }
            else
            {
                Console.WriteLine("Unfortunately Inserted Failed Token " + (TokenNo).ToString() + "Times");
            }
        }
        public DataTable Amazon_Juchuu_Items;
        public DataTable Amazon_Juchuu;
        public DataTable Amazon_Juchuu_NextToken;
        public DataTable GetListOrderdata(Order o,int i, bool IsFirsToken = true)
        {
            try
            {
                bool IsShippingNull=false, IsorderTotoal = false;
                if (o.ShippingAddress == null)
                {
                    IsShippingNull = true;
                }
                if (o.OrderTotal ==null)
                {
                    IsorderTotoal = true;
                }
                if (IsFirsToken)
                {
                    Amazon_Juchuu.Rows.Add(StoreCD, APIKey, i, o.AmazonOrderId,
                                 null,
                                 o.SellerOrderId, o.PurchaseDate, o.LastUpdateDate,
                                 getOrderStatus(o.OrderStatus),   // 
                                 o.FulfillmentChannel, o.SalesChannel, o.OrderChannel, o.ShipServiceLevel,
                                 IsShippingNull ? null : o.ShippingAddress.Name,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine1,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine2,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine3,
                                 IsShippingNull ? null : o.ShippingAddress.City,
                                 IsShippingNull ? null : o.ShippingAddress.County,
                                 IsShippingNull ? null : o.ShippingAddress.District,
                                 IsShippingNull ? null : o.ShippingAddress.StateOrRegion,
                                 IsShippingNull ? null : o.ShippingAddress.PostalCode,
                                 IsShippingNull ? null : o.ShippingAddress.CountryCode,
                                 IsShippingNull ? null : o.ShippingAddress.Phone,
                                 IsorderTotoal ? null : o.OrderTotal.CurrencyCode,
                                 IsorderTotoal ? null : o.OrderTotal.Amount,
                                 o.NumberOfItemsShipped,
                                 o.NumberOfItemsUnshipped,
                                 o.PaymentMethod,

                                 null, //o.PaymentMethodDetail,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.IsreplacementOrder,
                                 null, //o.ReplacementOrderId,
                                 o.MarketplaceId,
                                 o.BuyerEmail,
                                 o.BuyerName,

                                 null,//o.CompanyLegalName,
                                 null,//o.taxingregion,
                                 null,//o.Name,
                                 null,//o.values,
                                 o.ShipmentServiceLevelCategory,
                                getOrderType(o.OrderType),
                                 o.EarliestShipDate,
                                 o.LatestShipDate,
                                 o.EarliestDeliveryDate,
                                 o.LatestDeliveryDate,
                                 getFlag(o.IsBusinessOrder.ToString()),
                                 o.PurchaseOrderNumber,
                                 getFlag(o.IsPrime.ToString()),
                                 getFlag(o.IsPremiumOrder.ToString()),
                                 null,                    //o.PromiseReponseDueDate,
                                 null);                  //o.IsEstimatedShipDateSet
                }
                else
                {
                    Amazon_Juchuu_NextToken.Rows.Add(StoreCD, APIKey, i, o.AmazonOrderId,
                                 null,
                                 o.SellerOrderId, o.PurchaseDate, o.LastUpdateDate,
                                 getOrderStatus(o.OrderStatus),   // 
                                 o.FulfillmentChannel, o.SalesChannel, o.OrderChannel, o.ShipServiceLevel,
                                 IsShippingNull ? null : o.ShippingAddress.Name,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine1,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine2,
                                 IsShippingNull ? null : o.ShippingAddress.AddressLine3,
                                 IsShippingNull ? null : o.ShippingAddress.City,
                                 IsShippingNull ? null : o.ShippingAddress.County,
                                 IsShippingNull ? null : o.ShippingAddress.District,
                                 IsShippingNull ? null : o.ShippingAddress.StateOrRegion,
                                 IsShippingNull ? null : o.ShippingAddress.PostalCode,
                                 IsShippingNull ? null : o.ShippingAddress.CountryCode,
                                 IsShippingNull ? null : o.ShippingAddress.Phone,
                                 IsorderTotoal ? null : o.OrderTotal.CurrencyCode,
                                 IsorderTotoal ? null : o.OrderTotal.Amount,
                                 o.NumberOfItemsShipped,
                                 o.NumberOfItemsUnshipped,
                                 o.PaymentMethod,

                                 null, //o.PaymentMethodDetail,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.CurrencyCode,
                                 null, //o.Amount,
                                 null, //o.PaymentMethod,

                                 null, //o.IsreplacementOrder,
                                 null, //o.ReplacementOrderId,
                                 o.MarketplaceId,
                                 o.BuyerEmail,
                                 o.BuyerName,

                                 null,//o.CompanyLegalName,
                                 null,//o.taxingregion,
                                 null,//o.Name,
                                 null,//o.values,
                                 o.ShipmentServiceLevelCategory,
                                getOrderType(o.OrderType),
                                 o.EarliestShipDate,
                                 o.LatestShipDate,
                                 o.EarliestDeliveryDate,
                                 o.LatestDeliveryDate,
                                 getFlag(o.IsBusinessOrder.ToString()),
                                 o.PurchaseOrderNumber,
                                 getFlag(o.IsPrime.ToString()),
                                 getFlag(o.IsPremiumOrder.ToString()),
                                 null,                    //o.PromiseReponseDueDate,
                                 null);                  //o.IsEstimatedShipDateSet

                }

                //);
            }
            catch(Exception ex) {
                var msge = ex.Message;
            }
            return Amazon_Juchuu;
        }
        protected string getOrderStatus(string val)
        {
            if (val == "PendingAvailability")
            {
                return "1";
            }
            else if (val == "Pending")
            {
                return "2";
            }
            else if (val == "Unshipped")
            {
                return "3";
            }
            else if (val == "PartiallyShipped")
            {
                return "4";
            }
            else if (val == "Shipped")
            {
                return "5";
            }
            else if (val == "Canceled")
            {
                return "6";
            }
            else if (val == "PendingAvailability")
            {
                return "7";
            }
            return "0";
        }
        protected string getFlag(string val)
        {
            if (val == "True")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        protected string getOrderType(string val)
        {
            if (val == "StandardOrder")
            {
                return "1";
            }
            else if (val == "Preorder")
            {
                return "2";
            }
            else {
                return "3";
            }
        }
        public DataTable D_AmazonJuchuu()
        {
            string[] cols = new string[]{ "StoreCD", "APIKey", "InportSEQRows", "AmazonOrderId", "JuchuuNO", "SellerOrderId", "PurchaseDate", "LastUpdateDate", "OrderStatus",
                "FulfillmentChannel", "SalesChannel", "OrderChannel", "ShipServiceLevel", "AddressName", "AddressLine1", "AddressLine2", "AddressLine3", "AddressCity", "AddressCounty",
                "AddressDistrict", "StateOrRegion", "PostalCode", "CountryCode", "Phone", "CurrencyCode", "Amount", "NumberOfItemsShipped", "NumberOfItemsUnshipped", "PaymentMethod",
                "PaymentMethodDetail", "PaymentCurrencyCode1", "PaymentAmount1", "PaymentMethod1", "PaymentCurrencyCode2", "PaymentAmount2", "PaymentMethod2", "PaymentCurrencyCode3",
                "PaymentAmount3", "PaymentMethod3", "PaymentCurrencyCode4", "PaymentAmount4", "PaymentMethod4", "IsReplacementOrder", "ReplacedOrderId", "MarketplaceId",
                "BuyerEmail", "BuyerName", "CompanyLegalName", "TaxingRegion", "TaxClassificationName", "TaxClassificationValue", "ShipmentServiceLevelCategory", "OrderType",
                "EarliestShipDate", "LatestShipDate", "EarliestDeliveryDate", "LatestDeliveryDate", "IsBusinessOrder", "PurchaseOrderNumber", "IsPrime", "IsPremiumOrder",
                "PromiseResponseDueDate","IsEstimatedShipDateSet"};
            DataTable dt = new DataTable();
            foreach (string colname in cols)
            {
                dt.Columns.Add(colname);
            }
            return dt;
        }
        public DataTable D_AmazonJuchuuItems()
        {

            string[] cols = new string[] {
                   "StoreCD",
                   "APIKey",
                   "InportSEQRows",
                   "AmazonOrderId",
                   "OrderRows",
                   "ASIN",
                   "OrderItemId",
                   "SellerSKU",
                   "CustomizedURL",
                   "Title",
                   "QuantityOrdered",
                   "QuantityShipped",
                   "PointsNumber",
                   "PointsMonetaryCurrencyCode",
                   "PointsMonetaryValue",
                   "NumberOfItems",
                   "ItemPriceCurrencyCode",
                   "ItemPriceAmount",
                   "ShippingPriceCurrencyCode",
                   "ShippingPriceAmount",
                   "GiftWrapPriceCurrencyCode",
                   "GiftWrapPriceAmount",
                   "TaxModel",
                   "TaxResponsibleParty",
                   "ItemTaxCurrencyCode",
                   "ItemTaxCurrencyAmount",
                   "ShippingTaxCurrencyCode",
                   "ShippingTaxAmount",
                   "GiftWrapTaxCurrencyCode",
                   "GiftWrapTaxAmount",
                   "ShippingDiscountCurrencyCode",
                   "ShippingDiscountAmount",
                   "PromotionDiscountCurrencyCode",
                   "PromotionDiscountAmount",
                   "PromotionIds",
                   "CODFeeCurrencyCode",
                   "CODFeeAmount",
                   "CODFeeDiscountCurrencyCode",
                   "CODFeeDiscountAmount",
                   "IsGift",
                   "GiftMessageText",
                   "GiftWrapLevel",
                   "ConditionNote",
                   "ConditionId",
                   "ConditionSubtypeId",
                   "ScheduledDeliveryStartDate",
                   "ScheduledDeliveryEndDate",
                   "PriceDesignation"
            };
            DataTable dt = new DataTable();
            foreach (string colname in cols)
            {
                dt.Columns.Add(colname);
            }
            return dt;

        }

    }
}
