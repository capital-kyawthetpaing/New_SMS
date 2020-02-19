using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;
using MarketplaceWebService;
using MarketplaceWebService.Mock;
using MarketplaceWebService.Model;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using System.Data;
namespace Amazon_API
{
    class Program
    {

        //static string consoleWriteLinePath = ConfigurationManager.AppSettings["ConsoleWriteLinePath"].ToString();
        //static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //static string accessKey = "AKIAJFRPIMOTC4CJGHLQ";
        //static string secretKey = "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";
        //static string appName = "ラスタスポーツ";
        //static string appVersion = "1.0";
        //static string serviceURL = "https://mws.amazonservices.jp";
        //static string MWSAuthToken = "amzn.mws.fea748c0-bfe0-4039-0cc0-88b6ce5c0058";
        //static string merchantId = "A3U1G59YKB47LS";
        //static string marketplaceId = "A1VC38T7YXB528";
        //static string responseXml;


        static DataTable dt ;
        static string strbuff = string.Empty;
        public static void Main(string[] args)
        {
            GetOrderList();
        }

        public static void GetOrderList()
        {
            dt = new DataTable();
            dt.Columns.Add("OrderId");
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            string SellerId = "A3U1G59YKB47LS";
            string MarketplaceId = "A1VC38T7YXB528";
            string AccessKeyId = "AKIAJFRPIMOTC4CJGHLQ";
            string SecretKeyId = "4KI9yuXr7Ni64iFpdjnW1dw3LNdNXIn4rgOnNrZQ";
            string ApplicationVersion = "1.0";
            string ApplicationName = "ラスタスポーツ";
            string MWSAuthToken = "amzn.mws.fea748c0-bfe0-4039-0cc0-88b6ce5c0058";
            string serviceURL = "https://mws.amazonservices.jp";
            string strbuff = string.Empty;

            config.ServiceURL = serviceURL;
            MarketplaceWebServiceOrders.MarketplaceWebServiceOrders client = new MarketplaceWebServiceOrdersClient(
                                                                                    AccessKeyId,
                                                                                    SecretKeyId,
                                                                                    ApplicationName,
                                                                                    ApplicationVersion,
                                                                                    config);


            //ListOrder
            try
            {
                ListOrdersRequest request = new ListOrdersRequest();
                request.SellerId = SellerId;
                request.CreatedAfter = DateTime.Now.AddDays(-1);
                List<string> lstMarketplace = new List<string>();
                lstMarketplace.Add(MarketplaceId);
                request.MarketplaceId = lstMarketplace;
                request.MWSAuthToken = MWSAuthToken;
                ListOrdersResponse response = client.ListOrders(request);
                if (response.IsSetListOrdersResult())
                {
                    ListOrdersResult listOrdersResult = response.ListOrdersResult;
                    if (listOrdersResult.IsSetOrders())
                    {
                        List<Order> orders = listOrdersResult.Orders;
                        foreach (Order order in orders)
                        {
                            dt.Rows.Add(order.AmazonOrderId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msge = ex.Message;
            }
            //// ListOrderByNextToken
            try
            {
                ListOrdersRequest request = new ListOrdersRequest();
                request.SellerId = SellerId;
                request.CreatedAfter = DateTime.Now.AddDays(-1);
                List<string> lstMarketplace = new List<string>();
                lstMarketplace.Add(MarketplaceId);
                request.MarketplaceId = lstMarketplace;
                request.MaxResultsPerPage = 14;
                request.MWSAuthToken = MWSAuthToken;
                ListOrdersResponse response = client.ListOrders(request);
                if (response.IsSetListOrdersResult())
                {
                    ListOrdersResult listOrdersResult = response.ListOrdersResult;
                    if (listOrdersResult.IsSetOrders())
                    {
                        if (listOrdersResult.NextToken != null)
                        {
                            ListOrdersByNextTokenRequest request1 = new ListOrdersByNextTokenRequest();

                            request1.SellerId = SellerId;
                            request1.MWSAuthToken = MWSAuthToken;
                            request1.NextToken = listOrdersResult.NextToken;
                            ListOrdersByNextTokenResponse response1 = client.ListOrdersByNextToken(request1);
                            if (response1.IsSetListOrdersByNextTokenResult())
                            {
                                ListOrdersByNextTokenResult listOrdersByNextResult = response1.ListOrdersByNextTokenResult;
                                if (listOrdersByNextResult.IsSetOrders())
                                {
                                    List<Order> orders = listOrdersByNextResult.Orders;
                                    foreach (Order order in orders)
                                    {
                                        dt.Rows.Add(order.AmazonOrderId);
                                     //   strbuff += order.AmazonOrderId + System.Environment.NewLine;
                                    }
                                }
                            }
                        }
                    }
                    var val = strbuff;
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {

            }

            // //List Order Item
            //try
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        ListOrderItemsRequest request = new ListOrderItemsRequest();
            //        request.SellerId = SellerId;
            //        request.AmazonOrderId = dr["OrderId"].ToString();
            //        request.MWSAuthToken = MWSAuthToken;
            //        ListOrderItemsResponse response = client.ListOrderItems(request);
            //        if (response.IsSetListOrderItemsResult())
            //        {
            //            ListOrderItemsResult listOrderItemsResult = response.ListOrderItemsResult;
            //            if (listOrderItemsResult.IsSetOrderItems())
            //            {
            //                List<OrderItem> orderItems = listOrderItemsResult.OrderItems;
            //                foreach (OrderItem orderItem in orderItems)
            //                {
            //                    strbuff += "商品名：" + orderItem.Title + System.Environment.NewLine;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //// ListOrderItem_byNextToken
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ListOrderItemsRequest request = new ListOrderItemsRequest();
                    request.SellerId = SellerId;
                    request.AmazonOrderId = dr["OrderId"].ToString();
                    request.MWSAuthToken = MWSAuthToken;
                    
                    ListOrderItemsResponse response = client.ListOrderItems(request);
                    if (response.IsSetListOrderItemsResult())
                    {
                        ListOrderItemsResult listOrderItemsResult = response.ListOrderItemsResult;
                        if (listOrderItemsResult.NextToken != null)
                        {
                            ListOrderItemsByNextTokenRequest request1 = new ListOrderItemsByNextTokenRequest();
                            request1.SellerId = SellerId;
                            request1.MWSAuthToken = MWSAuthToken;
                            request1.NextToken = listOrderItemsResult.NextToken;

                            ListOrderItemsByNextTokenResponse response1 = client.ListOrderItemsByNextToken(request1);
                            if (response1.IsSetListOrderItemsByNextTokenResult())
                            {
                                ListOrderItemsByNextTokenResult listOrderByNextItemsResult = response1.ListOrderItemsByNextTokenResult;
                                if (listOrderByNextItemsResult.IsSetOrderItems())
                                {
                                    List<OrderItem> orderItems = listOrderItemsResult.OrderItems;
                                    foreach (OrderItem orderItem in orderItems)
                                    {
                                        if (orderItem.IsSetOrderItemId())
                                        {
                                            strbuff += "商品名：" + orderItem.Title + System.Environment.NewLine;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //// GetOrder 
            //try
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        GetOrderRequest request = new GetOrderRequest();
            //        request.SellerId = SellerId;
            //        request.MWSAuthToken = MWSAuthToken;
            //        List<string> amazonorderId = new List<string>();
            //        amazonorderId.Add(dr["OrderId"].ToString());
            //        request.AmazonOrderId = amazonorderId;

            //        GetOrderResponse response = client.GetOrder(request);
            //        if (response.IsSetGetOrderResult())
            //        {
            //            List<Order> orders = response.GetOrderResult.Orders;
            //            foreach (Order order in orders)
            //            {
            //                strbuff += "購入者：" + order.AmazonOrderId + ","+ order.OrderStatus + System.Environment.NewLine;
            //            }
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{

            //}

            //// GetService Status
            //try
            //{
            //    MarketplaceWebServiceOrdersConfig config1 = new MarketplaceWebServiceOrdersConfig();
            //    config1.ServiceURL = serviceURL;
            //    MarketplaceWebServiceOrders.MarketplaceWebServiceOrders client1 = new MarketplaceWebServiceOrdersClient(
            //                                                                            AccessKeyId,
            //                                                                            SecretKeyId,
            //                                                                            ApplicationName,
            //                                                                            ApplicationVersion,
            //                                                                            config1);
            //    MarketplaceWebServiceOrders.Model.GetServiceStatusRequest  request = new MarketplaceWebServiceOrders.Model.GetServiceStatusRequest();
            //    request.SellerId = SellerId;
            //    request.MWSAuthToken = MWSAuthToken;
            //    // MarketplaceWebServiceOrders.Model.GetServiceStatusRequest
            //    var response = client1.GetServiceStatus(request);
            //    if (response.IsSetGetServiceStatusResult())
            //    {
            //        strbuff = "処理状況：" + response.GetServiceStatusResult.Status;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }

        

    }
}
