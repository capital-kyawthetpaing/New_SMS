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
using BL;
namespace Amazon__API
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
        static Login_BL loginbl = new Login_BL();
        static DataTable dt;
        static string strbuff = string.Empty;
        public static void Main(string[] args)
        {

            //GetOrderList();

            Console.Title = "Amazon API Console. . . ";
            Console.WriteLine("Started " + "Amazon__API Started" + " . . . ");
            if (loginbl.ReadConfig() == true)
            {
                CommonAPI api = new CommonAPI();
                Amazon__BL abl = new Amazon__BL();
                if (abl.Allow_Check())
                {
                    api.ListOrders();
                }
                else
                {
                    Environment.Exit(0);
                }
            }



        }

  



    }
}
