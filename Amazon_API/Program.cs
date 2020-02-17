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



        static string strbuff = string.Empty;
        public static void Main(string[] args)
        {
            GetOrderList();
        }
        
        public static void GetOrderList()
        {
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
            ListOrdersRequest request = new ListOrdersRequest();
            request.SellerId = SellerId;
            request.CreatedAfter = DateTime.Now.AddDays(-5);
            List<string> lstMarketplace = new List<string>();
            lstMarketplace.Add(MarketplaceId);
            request.MarketplaceId = lstMarketplace;
            request.MWSAuthToken = MWSAuthToken;

            try
            {
                ListOrdersResponse response = client.ListOrders(request);
                if (response.IsSetListOrdersResult())
                {
                    ListOrdersResult listOrdersResult = response.ListOrdersResult;
                    if (listOrdersResult.IsSetOrders())
                    {
                        List<Order> orders = listOrdersResult.Orders;
                        foreach (Order order in orders)
                        {
                            strbuff += order.AmazonOrderId + System.Environment.NewLine;
                        }
                    }
                    //   txtListOr//ders.Text = strbuff;
                }
            }
            catch(Exception ex)
            {
                var msge = ex.Message;
            }
        }

    }
}
