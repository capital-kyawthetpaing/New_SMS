using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using BL;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;

namespace Yahoo_API
{
    public class Common_API
    {
        DataTable dtSecretKey;
        YahooAPI_BL yahooAPI_bl = new YahooAPI_BL();
        M_API_Entity api = new M_API_Entity();
        D_APIRireki_Entity apiRireki = new D_APIRireki_Entity();
        D_YahooCount_Entity yahooCount = new D_YahooCount_Entity();
        D_YahooList_Entity yahoolist;
        DataTable dtorder= new DataTable();
        HttpWebRequest countRequest;
        HttpWebRequest listRequest;
        string accessToken = string.Empty;
         DataTable YahooList, YahooDetail, YahooItem ;
        D_APIControl_Entity dae = new D_APIControl_Entity();
        public string ShopName {
            get;set;
        }
        //string storeCD = string.Empty;
        //string APIKey = string.Empty;
        //string AppID = string.Empty;
        //string ChageDate = string.Empty;
        //string SecretKey = string.Empty;
        //string RefreshToken = string.Empty;
        //string sellerID = string.Empty;
        // bool productionMode = false;
        string NowDatTime = string.Empty;
        DateTime LastGetDateTime;
        public void Search_GetOrderDetail(D_APIControl_Entity DApiControl_entity)
        {
            dae = DApiControl_entity;
            YahooItem = new DataTable();
            YahooDetail = new DataTable();
            YahooList = GetOrderListTable();
            for (int i = 0; i < 20; i++)
            {
               
                string num = (i + 1).ToString().PadLeft(2, '0');

                YahooList.Columns.Add("ItemOption" + num + "Name");
                YahooList.Columns.Add("ItemOption" + num + "Value");
                YahooList.Columns.Add("ItemOption" + num + "Price");
                YahooList.Columns.Add("ItemOption" + num + "KBN");
            }
            bool res = false;
            for (int i = 0;i<3;i++)
            {
                LastGetDateTime = Convert.ToDateTime(DApiControl_entity.LastGetDateTime);
                dtSecretKey = new DataTable();
                dtSecretKey = yahooAPI_bl.M_API_Select(DApiControl_entity);

                api.StoreCD= dtSecretKey.Rows[0]["StoreCD"].ToString();
                api.APIKey = dtSecretKey.Rows[0]["APIKey"].ToString();
                api.TESTMode = dtSecretKey.Rows[0]["TESTMode"].ToString();
                if (api.TESTMode.Equals("0"))
                {
                    api.ApplicationID= dtSecretKey.Rows[0]["ApplicationID0"].ToString();
                    api.SellerId = dtSecretKey.Rows[0]["StoreAccount0"].ToString();
                   /// productionMode = true;
                }
                else if (api.TESTMode.Equals("1"))
                {
                    api.ApplicationID = dtSecretKey.Rows[0]["ApplicationID1"].ToString();
                    api.SellerId = dtSecretKey.Rows[0]["StoreAccount1"].ToString();
                    //productionMode = false;
                }
                api.ApplicationID0 = dtSecretKey.Rows[0]["ApplicationID0"].ToString();
                api.ApplicationID1 = dtSecretKey.Rows[0]["ApplicationID1"].ToString();
                api.ChangeDate = dtSecretKey.Rows[0]["ChangeDate"].ToString();
                api.YahooSecretKey = dtSecretKey.Rows[0]["YahooSecretKey"].ToString();
                api.RefreshToken= dtSecretKey.Rows[0]["RefreshToken"].ToString();
                //Found:
                if (YahooAPIAuth(api).Equals("ng"))
                {
                    string url = "https://auth.login.yahoo.co.jp/yconnect/v1/authorization?response_type=code&client_id=" + api.ApplicationID + "&redirect_uri=http://shopping.geocities.jp/racket/index.html&state=xyz";
                    string code = RedirectPath(url);
                    if (!String.IsNullOrWhiteSpace(code))
                    {
                        GetNewRefreshToken(code);
                    }
                    res = false;
                }
                else
                {
                    res = true;
                    break;
                }
            }
            if (res)
                SearOrder(api);
            else
            { 
                //error
            }
        }

        public string YahooAPIAuth(M_API_Entity api)
        {
            //api.ApplicationID = "dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-";
            //api.YahooSecretKey = "08285f0c90ec9c0428edf248bcbd71b515bebec5";
            try
            {
                
                var postData = HttpUtility.ParseQueryString(string.Empty);
                postData.Add(new NameValueCollection
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", api.RefreshToken }
                });
                var webRequest = (HttpWebRequest)WebRequest.Create("https://auth.login.yahoo.co.jp/yconnect/v1/token");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(api.ApplicationID + ":" + api.YahooSecretKey));
                webRequest.Headers.Add("Authorization", "Basic " + encoded);
                using (var s = webRequest.GetRequestStream())
                using (var sw = new StreamWriter(s))
                    sw.Write(postData.ToString());
                using (var webResponse = webRequest.GetResponse())
                {
                    var responseStream = webResponse.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        var response = reader.ReadToEnd();
                        var json = JObject.Parse(response);
                        accessToken = json.Value<string>("access_token");
                    }
                }
                return "success";
            }
            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                var f = sr.ReadToEnd();
                return "ng";

            }
        }

        public static string RedirectPath(string url)
        {
            string line = "";
            FirefoxOptions option = new FirefoxOptions();
            var service = FirefoxDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);

            using (IWebDriver firefox = new FirefoxDriver(service, option, TimeSpan.FromMinutes(3)))
            {
                string username = "capitalkmm"; string password = "capital1234";
                firefox.Url = url;
                string title = firefox.Title;
                firefox.FindElement(By.Name("login")).SendKeys(username);
                firefox.FindElement(By.Name("btnNext")).Submit();
                Thread.Sleep(3000);
                firefox.FindElement(By.Name("passwd")).SendKeys(password);
                firefox.FindElement(By.Name("btnNext")).Submit();
                Thread.Sleep(5000);

                try
                {
                    IWebElement btn = firefox.FindElement(By.Id(".save"));
                    if (btn != null)
                    {
                        btn.Submit();
                        Thread.Sleep(5000);
                    }
                }
                catch (Exception) { }
                
                string redirecturl = firefox.Url;
                if (redirecturl.Contains("code"))
                {
                    int start = redirecturl.IndexOf("code");
                    int end = redirecturl.IndexOf("&state=xyz", start);
                    line = redirecturl.Substring(start, end - start);
                    if (line.Contains("code="))
                    {
                        line = line.Replace("code=", "");
                    }
                }
                firefox.Close();
            }
            return line;

        }
        public string GetNewRefreshToken(string code)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string RefreshToken = "";
                var postData = HttpUtility.ParseQueryString(string.Empty);
                postData.Add(new NameValueCollection
                {
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", "http://shopping.geocities.jp/racket/index.html" },
                    {"code",code}
                });
                var webRequest = (HttpWebRequest)WebRequest.Create("https://auth.login.yahoo.co.jp/yconnect/v1/token");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(api.ApplicationID + ":" + api.YahooSecretKey));
                webRequest.Headers.Add("Authorization", "Basic " + encoded);
                using (var s = webRequest.GetRequestStream())
                {
                    using (var sw = new StreamWriter(s))
                    {
                        sw.Write(postData.ToString());
                        try
                        {
                            using (var webResponse = webRequest.GetResponse())
                            {
                                var responseStream = webResponse.GetResponseStream();
                                using (var reader = new StreamReader(responseStream))
                                {
                                    var response = reader.ReadToEnd();
                                    var json = JObject.Parse(response);
                                    //string accessToken = "";
                                    accessToken = json.Value<string>("access_token");
                                    RefreshToken = json.Value<string>("refresh_token");
                                    InsertRefreshTokenToShop(RefreshToken);
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                            var f = sr.ReadToEnd();
                            // wresponse = (WebResponse)listRequest.GetResponse();
                        }
                    }
                }
                return "success";
            }
            catch (WebException e)
            {
                string error = "";
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                            if (error.Contains("Error"))
                            {
                                XmlDocument xd = new XmlDocument();
                                xd.LoadXml(error);
                                error = xd.InnerText;
                            }
                        }
                    }
                }
                return error;
            }
        }
        private void InsertRefreshTokenToShop(string ReToken)
        {
            //api = new M_API_Entity();
            //api.APIKey = APIKey;
            //api.ChangeDate = ChageDate;
            api.RefreshToken = ReToken;
            yahooAPI_bl.InsertRefreshToken(api);
        }
        public void SearOrder(M_API_Entity api) 
        {
           
            if (api.TESTMode.Equals("1"))
                countRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderCount?seller_id={0}", api.SellerId));
            else
                countRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://test.circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderCount?seller_id={0}", api.SellerId));
            countRequest.Method = "GET";
            countRequest.ContentType = "application/x-www-form-urlencoded";
            countRequest.Headers.Add("Authorization", "Bearer " + accessToken);
          
            String encoded = Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(api.ApplicationID + ":" + api.YahooSecretKey));
            try
            {
                using (var countResponse = countRequest.GetResponse())
                {
                    var responseStream = countResponse.GetResponseStream();

                    using (var reader = new StreamReader(responseStream))
                    {
                        var response = reader.ReadToEnd();
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(response);
                        response = xd.InnerText;
                        XmlNodeList list1 = xd.GetElementsByTagName("NewOrder");
                        XmlNodeList list2 = xd.GetElementsByTagName("NewReserve");
                        if ((Convert.ToInt32(list1[0].InnerText) > 0) || (Convert.ToInt32(list2[0].InnerText) > 0))
                        {
                            NowDatTime = System.DateTime.Now.ToString();

                            apiRireki = GetApiRireki(); // 3

                            yahooCount = GetOrderCount(xd); //3
                            if (yahooAPI_bl.D_APIRireki_D_YahooCount_Insert(apiRireki, yahooCount))
                            {
                                Console.WriteLine("D_APIRireki_D_YahooCount_Insert . . . 324");
                                GetOrderList(); //4
                            }
                        }
                    }
                }
            }

            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                var f = sr.ReadToEnd();//The remote server returned an error: (401) Unauthorized.
               // var wresponse = (WebResponse)listRequest.GetResponse();
                if (f == "" && ex.Message == "The remote server returned an error: (401) Unauthorized.")
                {
                     First_NewRefreshToken();
                    Search_GetOrderDetail(dae);
                    return;
                }
               
                string d = "";
            }
        }
        public void First_NewRefreshToken()
        {
            try
            {
                string url = "https://auth.login.yahoo.co.jp/yconnect/v1/authorization?response_type=code&client_id=" + api.ApplicationID + "&redirect_uri=http://shopping.geocities.jp/racket/index.html&state=xyz";
                string code = RedirectPath(url);
                if (!String.IsNullOrWhiteSpace(code))
                {
                    GetNewRefreshToken(code);

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void GetOrderList()     // 4
        {
            byte[] OrderlistReq = OrderListParameter();
            if (api.TESTMode.Equals("1"))
                listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderList"));
            else
                listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://test.circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderList"));
            byte[] bytes;
            bytes = System.Text.Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(OrderlistReq));
            listRequest.ContentType = "application/x-www-form-urlencoded";
            listRequest.ContentLength = bytes.Length;
            listRequest.Method = "POST";
            listRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            Stream requeststream =  listRequest.GetRequestStream();
            requeststream.Write(bytes, 0, bytes.Length);
            requeststream.Close();
            HttpWebResponse response;
            WebResponse wresponse;
            try
            {
                response = listRequest.GetResponse() as HttpWebResponse;
                var str = new StreamReader(response.GetResponseStream());
                var xml = str.ReadToEnd();
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xml);
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                try { 
                dtorder = ds.Tables[2];     ///// Put Logged   by PTK
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + "Not Current data existed");
                    return;
                }
                dtorder.ParentRelations.Clear();
                dtorder.Constraints.Clear();
                var frireki = apiRireki;
                try
                {
                    if (yahooAPI_bl.D_APIDetail_YahooList(frireki, YahooOrderList(dtorder, frireki)))
                    {
                        Console.WriteLine("D_APIDetail_YahooList . . . 399");
                        OrderInfo(frireki); // 6
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Object"))
                    {
                        IsItemOptionNode = false;
                        OrderInfo(frireki); // 6
                        return;
                    }
                    //Skipped if not existed on module 2
                }
            }

            catch (WebException ex)
            {
                
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                var f = sr.ReadToEnd();
                wresponse = (WebResponse)listRequest.GetResponse();
            }
        }

        public static bool IsItemOptionNode = true; 
        public DataTable YahooOrderList(DataTable yo, D_APIRireki_Entity dae )
        {
            yo.Columns.Remove("Index");
            try
            {
                yo.Columns.Remove("Search_Id");
            }
            catch(Exception ex) {

            }

            yo.Columns.Add("StoreCD");
            yo.Columns.Add("APIKey");
            yo.Columns.Add("SEQ");
            
            int i = 0;
            foreach (DataRow dr in yo.Rows)
            {
                i++;
                
                dr["StoreCD"] =dae.StoreCD;
                dr["APIKey"] =  dae.APIKey;
                dr["SEQ"] = i.ToString();

            }
            yo.AcceptChanges();
            return yo;
        }
        public static DataTable YSD;
        protected void OrderInfo(D_APIRireki_Entity dai) //6
        {
            DataSet ds = new DataSet();
             YSD = YahooJuChuuDetails();
            foreach (DataRow dr in dtorder.Rows)
            {
                byte[] OrderlistReq = OrderInfoParameter(dr["OrderId"].ToString());
                if (api.TESTMode.Equals("1"))
                    listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderInfo"));
                else
                    listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://test.circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderInfo"));


               // string d = "<Req><Target><OrderId>testseller - 10000001 </OrderId><Field> PayStatus,SettleStatus </Field></Target> <SellerId> testseller </SellerId> </Req>";
                   byte[] bytes;
                bytes = System.Text.Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(OrderlistReq));
                listRequest.ContentType = "application/x-www-form-urlencoded";
                listRequest.ContentLength = bytes.Length;
                listRequest.Method = "POST";
                listRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                Stream requeststream = listRequest.GetRequestStream();
                requeststream.Write(bytes, 0, bytes.Length);
                requeststream.Close();
                HttpWebResponse response;
                try
                {
                    response = listRequest.GetResponse() as HttpWebResponse;
                    var str = new StreamReader(response.GetResponseStream());
                    var xml = str.ReadToEnd();
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xml);
                    var Itemoption = xd.SelectSingleNode("/ResultSet/Result/OrderInfo/Item/ItemOption").InnerText;
                    ds.ReadXml(new XmlNodeReader(xd));// 7
                    AddOrderInfo_List(ds, dr["OrderId"].ToString());
                    ds.Clear();
                }
                catch (WebException ex)
                {
                    StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                    var f = sr.ReadToEnd();
                   // wresponse = (WebResponse)listRequest.GetResponse();
                }
            }

            //Add CD and Key
            
            for (int i = 0; i < YahooList.Rows.Count; i++)
            {
                YahooList.Rows[i]["StoreCD"] = dai.StoreCD;
                YahooList.Rows[i]["APIKey"] = dai.APIKey;
                YahooList.Rows[i]["InportSEQRows"] = (i+1).ToString();
            }
            YahooList.Columns["ChildOrderId"].ColumnName = "ChildOrderId1";
            
            var Main = YahooJucchuu();
            foreach (DataRow ydr in YahooList.Rows)
            {
                Main.Rows.Add();
                foreach (DataColumn ydc in YahooList.Columns) {

                    for (int i = 0; i < Main.Columns.Count; i++)
                    {
                        if (Main.Columns[i].ColumnName == ydc.ColumnName)
                        {
                            string val = YahooList.Rows[Main.Rows.Count - 1][ydc.ColumnName].ToString();
                            if (val == "true")
                            {
                                val = "1";
                            }
                            else if (val == "false")
                            {
                                val = "0";
                            }
                            Main.Rows[Main.Rows.Count - 1][ydc.ColumnName] = val;
                            break;
                        }
                    }
                }
            }

            var MainShipping = YahooShipping();
            foreach (DataRow ydr in YahooList.Rows)
            {
                MainShipping.Rows.Add();
                foreach (DataColumn ydc in YahooList.Columns)
                {

                    for (int i = 0; i < MainShipping.Columns.Count; i++)
                    {
                        if (MainShipping.Columns[i].ColumnName == ydc.ColumnName)
                        {
                            string val = YahooList.Rows[MainShipping.Rows.Count - 1][ydc.ColumnName].ToString();
                            if (val == "true")
                            {
                                val = "1";
                            }
                            else if (val == "false")
                            {
                                val = "0";
                            }
                            MainShipping.Rows[MainShipping.Rows.Count - 1][ydc.ColumnName] = val;
                            break;
                        }
                    }
                }
            }

            

            ///  YahooShipping Operation
            /// YahooDetail.Select("Name='サイズ' or Name='カラー'").ToList().ForEach(x => x.Delete());    // /rewrapped by Ptk
            /// 
            for (int i = 0; i < 20; i++)    //Add for Option 20 Columns
            {     
                string paf = (i+1).ToString().PadLeft(2, '0');
                YahooDetail.Columns.Add("ItemOption"+ paf +"Name");
                YahooDetail.Columns.Add("ItemOption"+ paf + "Value");
                YahooDetail.Columns.Add("ItemOption"+ paf + "Price");
                YahooDetail.Columns.Add("ItemOption"+ paf + "KBN");
            }
            var YahooDT = YahooDetail.AsEnumerable().GroupBy(x => x.Field<string>("Item_Id")).Select(g => g.First()).ToList().CopyToDataTable();  // get Item_Id to Insert

            // Put Logic for Column options
            for (int i = 0; i < YahooDT.Rows.Count; i++)
            {
                string val = YahooDT.Rows[i]["Item_Id"].ToString();
                var colSeries = YahooDetail.Select("Item_Id = '" + val + "'").CopyToDataTable();

                for (int j = 0; j < colSeries.Rows.Count; j++)    // Add Col 1 - 20 by ptk (very fukuzatsu)
                {
                    string colname = "ItemOption" + (j + 1).ToString().PadLeft(2, '0');
                    YahooDT.Rows[i][colname + "Name"] = colSeries.Rows[j]["Name"].ToString();
                    YahooDT.Rows[i][colname + "Value"] = colSeries.Rows[j]["Value"].ToString();
                    YahooDT.Rows[i][colname + "Price"] = colSeries.Rows[j]["Price"].ToString();
                    YahooDT.Rows[i][colname + "KBN"] = (IsItemOptionNode) ? "1" : "2";
                }

            }

            YahooDT.Columns.Add("AffiliateRatio_Store");
            YahooDT.Columns.Add("AffiliateRatio_Yahoo");
            YahooDT.Columns.Add("StoreCD");
            YahooDT.Columns.Add("APIKey");
            YahooDT.Columns.Add("InportSEQRows");


            for (int i = 0; i < YahooDT.Rows.Count; i++)
            {
                YahooDT.Rows[i]["StoreCD"] = dai.StoreCD;
                YahooDT.Rows[i]["APIKey"] = dai.APIKey;
                YahooDT.Rows[i]["InportSEQRows"] = (i + 1).ToString();
                YahooDT.Rows[i]["AffiliateRatio_Yahoo"] = YahooDT.Rows[i]["AffiliateRatio_Store"] = YahooDT.Rows[i]["AffiliateRatio"].ToString();
            }
            YahooDT.AcceptChanges();

            var ShipDetail = YahooJuChuuDetails();
            foreach (DataRow ydr in YahooDT.Rows)
            {
                ShipDetail.Rows.Add();
                foreach (DataColumn ydc in YahooDT.Columns)
                {
                    for (int i = 0; i < ShipDetail.Columns.Count; i++)
                    {
                        if (ShipDetail.Columns[i].ColumnName == ydc.ColumnName)
                        {
                            string val = YahooDT.Rows[ShipDetail.Rows.Count - 1][ydc.ColumnName].ToString();
                            if (val == "true")
                            {
                                val = "1";
                            }
                            else if (val == "false")
                            {
                                val = "0";
                            }
                            ShipDetail.Rows[ShipDetail.Rows.Count - 1][ydc.ColumnName] = val;
                            break;
                        }
                    }
                }
            }

            if (yahooAPI_bl.ImportYahooJuuChuu(Main))   // Step 7 Insert
            {
                Console.WriteLine("ImportYahooJuuChuu . . . 634");
                if (yahooAPI_bl.ImportYahooShipping(MainShipping))
                {
                    Console.WriteLine("ImportYahooShipping . . . 639");
                    if (yahooAPI_bl.ImportYahooShippingDetail(ShipDetail))
                    {
                        Console.WriteLine("ImportYahooShippingDetail . . . 643");
                        Console.WriteLine("Finished All Steps. . . ");
                    }
                    //Insert YahoojuchuuDetails
                }
            }
            // OrderChange(); // 8

        }

        public void AddOrderInfo_List(DataSet ds, string OrderId) 
        {
         

            YahooList.Rows.Add();
            DataTable dtinfo = ds.Tables[2];   // OrderInfo
            for (int i = 0; i < dtinfo.Columns.Count;i++)
            {
                if (GetAllColNames().Split(',').Contains(dtinfo.Columns[i].ColumnName))
                {
                    for (int h =0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtinfo.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtinfo.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }
            DataTable dtpay = ds.Tables[3];     // OrderPay
            for (int i = 0; i < dtpay.Columns.Count; i++)
            {
                if (GetAllColNames().Split(',').Contains(dtpay.Columns[i].ColumnName))
                {
                    for (int h = 0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtpay.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtpay.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }
            DataTable dtShip = ds.Tables[4];     // OrderShip
            for (int i = 0; i < dtShip.Columns.Count; i++)
            {
                if (GetAllColNames().Split(',').Contains(dtShip.Columns[i].ColumnName))
                {
                    for (int h = 0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtShip.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtShip.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }

            DataTable dtSeller = ds.Tables[5];     // OrderSeller
            for (int i = 0; i < dtSeller.Columns.Count; i++)
            {
                if (GetAllColNames().Split(',').Contains(dtSeller.Columns[i].ColumnName))
                {
                    for (int h = 0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtSeller.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtSeller.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }
            DataTable dtBuyer = ds.Tables[6];     // OrderBuyer
            for (int i = 0; i < dtBuyer.Columns.Count; i++)
            {
                if (GetAllColNames().Split(',').Contains(dtBuyer.Columns[i].ColumnName))
                {
                    for (int h = 0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtBuyer.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtBuyer.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }


            //////////////////////////////////////////////Shipping Detail Operations!!!!!!!!!
            DataTable dtItem=YahooItem = ds.Tables[7];
            DataTable dtItemOption = ds.Tables[8];
            var dtResult = new DataTable();
            //dtResult.Columns.Add("OrderId");
            //dtResult.Columns.Add("StoreCD");
            //dtResult.Columns.Add("APIKey");
            //dtResult.Columns.Add("InportSEQRows");
            dtResult.Columns.Add("Name");
            dtResult.Columns.Add("Value");
            dtResult.Columns.Add("Price");
            dtResult.Columns.Add("Item_Id");
            
            //dtResult.Columns.Add("AffiliateRatio_Store");

            for (int i = 0; i < dtItem.Columns.Count; i++)
            {
                if (dtItem.Columns[i].ColumnName != "Item_Id")
                {
                    dtResult.Columns.Add(dtItem.Columns[i].ColumnName);
                }
            }
          

                var result = (from dataRows1 in dtItemOption.AsEnumerable()
                              join dataRows2 in dtItem.AsEnumerable()
                              on dataRows1.Field<int>("Item_Id") equals dataRows2.Field<int>("Item_Id") into lj
                              from r in lj.DefaultIfEmpty()
                                  //select (lj)).ToList() ;
                              select dtResult.LoadDataRow(new object[]
                              {
                                   dataRows1.Field<string>("Name"),
                                   dataRows1.Field<string>("Value"),
                                   dataRows1.Field<string>("Price"),
                                      r.Field<int>("Item_Id"),
                                  r == null ? "" : r.Field<string>("LineId"),
                                  r == null ? "" : r.Field<string>("ItemId"),
                                   r == null ? "" : r.Field<string>("Title"),
                                 r == null ? "" : r.Field<string>("SubCode"),
                           r == null ? "" : r.Field<string>("SubCodeOption"),
                                  r == null ? "" : r.Field<string>("IsUsed"),
                                 r == null ? "" : r.Field<string>("ImageId"),
                               r == null ? "" : r.Field<string>("IsTaxable"),
                                     r == null ? "" : r.Field<string>("Jan"),
                               r == null ? "" : r.Field<string>("ProductId"),
                              r == null ? "" : r.Field<string>("CategoryId"),
                              r == null ? "" : r.Field<string>("AffiliateRatio"),
                    //r == null ? "" : r.Field<string>("AffiliateRatio_Store"),
                    //r == null ? "" : r.Field<string>("AffiliateRatio_Yahoo"),
                               r == null ? "" : r.Field<string>("UnitPrice"),
                                r == null ? "" : r.Field<string>("Quantity"),
                      r == null ? "" : r.Field<string>("PointAvailQuantity"),
                             r == null ? "" : r.Field<string>("ReleaseDate"),
                            r == null ? "" : r.Field<string>("PointFspCode"),
                             r == null ? "" : r.Field<string>("PointRatioY"),
                        r == null ? "" : r.Field<string>("PointRatioSeller"),
                            r == null ? "" : r.Field<string>("UnitGetPoint"),
                           r == null ? "" : r.Field<string>("IsGetPointFix"),
                           r == null ? "" : r.Field<string>("GetPointFixDate"),
                           r == null ? "" : r.Field<string>("CouponData"),
                           r == null ? "" : r.Field<string>("CouponDiscount"),
                           r == null ? "" : r.Field<string>("CouponUseNum"),
                           r == null ? "" : r.Field<string>("OriginalPrice"),
                           r == null ? "" : r.Field<string>("OriginalNum"),
                           r == null ? "" : r.Field<string>("LeadTimeStart"),
                           r == null ? "" : r.Field<string>("LeadTimeEnd"),
                           r == null ? "" : r.Field<string>("LeadTimeText"),
                           r == null ? "" : r.Field<string>("PriceType")

                               }, false)).CopyToDataTable();


            dtResult.Columns.Add("OrderId");
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtResult.Rows[i]["OrderId"] = OrderId;
            }
            if (YahooDetail.Columns.Count == 0)
            {
                for (int i = 0; i < dtResult.Columns.Count; i++)
                {
                    YahooDetail.Columns.Add(dtResult.Columns[i].ColumnName);
                }
            }
            foreach (DataRow dr in dtResult.Rows)
                YahooDetail.ImportRow(dr);

           // DataTable dtItem = ds.Tables[7];     // OrderItem
            //for (int i = 0; i < dtResult.Rows.Count; i++)
            //{
            //    YSD.Rows.Add();
            //    for (int j = 0; j < dtResult.Columns.Count; j++)
            //    {
            //        if (YSD.Columns.Contains(dtResult.Columns[i].ColumnName))
            //        {
            //          YSD.Rows[YSD.Rows.Count - 1][dtResult.Columns[i].ColumnName] = dtResult.Rows[i][j].ToString();
            //        }
            //    }
            //}


            //  DataTable dtItem = ds.Tables[7];     // OrderItem
            //for (int i = 0; i < dtItem.Columns.Count; i++)
            //{
            //    if (GetAllColNames().Split(',').Contains(dtItem.Columns[i].ColumnName))
            //    {
            //        for (int h = 0; h < YahooList.Columns.Count; h++)
            //        {
            //            if (YahooList.Columns[h].ColumnName == dtItem.Columns[i].ColumnName)
            //            {
            //                YahooList.Rows[YahooList.Rows.Count - 1][h] = dtItem.Rows[0][i].ToString();
            //                break;
            //            }
            //        }
            //    }
            //}
            // DataTable dtItemOption = ds.Tables[8];     // dtItemOption


            // '.'Added 20th 4 pairs of Columns on this


            //for (int i = 0; i < dtItemOption.Rows.Count; i++)
            //{
            //    if ((i) < 20)    // limit It with respect to the database columns   by ptk comfirm by fukuda san  (******* Important 12/17/2019 ******)
            //    {
            ////////////////////////////////////////////////////////////////if (GetAllColNames().Split(',').Contains(dtItemOption.Columns[i].ColumnName))
            ////////////////////////////////////////////////////////////////{

            ////////////////////////////////////////////////////////////////for (int h = 0; h < YahooList.Columns.Count; h++)
            ////////////////////////////////////////////////////////////////{
            ////////////////////////////////////////////////////////////////    if (YahooList.Columns[h].ColumnName == dtItemOption.Columns[i].ColumnName)
            ////////////////////////////////////////////////////////////////    {
            ////////////////////////////////////////////////////////////////        YahooList.Rows[YahooList.Rows.Count - 1][h] = dtItemOption.Rows[0][i].ToString();
            ////////////////////////////////////////////////////////////////        break;
            ////////////////////////////////////////////////////////////////    }
            ////////////////////////////////////////////////////////////////}
            //string colname = "ItemOption" + (i + 1).ToString().PadLeft(2, '0');
            //for (int h = 0; h < dtItemOption.Columns.Count; h++)
            //{
            //    if (dtItemOption.Columns[h].ToString() == "Name")
            //    {
            //        YahooList.Rows[YahooList.Rows.Count - 1][colname + "Name"] = dtItemOption.Rows[i][h].ToString();
            //    }
            //    else if (dtItemOption.Columns[h].ToString().ToLower() == "value")
            //    {
            //        YahooList.Rows[YahooList.Rows.Count - 1][colname + "Value"] = dtItemOption.Rows[i][h].ToString();
            //    }
            //    else if (dtItemOption.Columns[h].ToString() == "Price")
            //    {
            //        YahooList.Rows[YahooList.Rows.Count - 1][colname + "Price"] = dtItemOption.Rows[i][h].ToString();
            //    }
            //}
            //YahooList.Rows[YahooList.Rows.Count - 1][colname + "KBN"] =(IsItemOptionNode)? "1" : "2";

            //    }


            //    //}
            //}
            DataTable dtDetail = ds.Tables[9];     // OrderDetail
            for (int i = 0; i < dtDetail.Columns.Count; i++)
            {
                if (GetAllColNames().Split(',').Contains(dtDetail.Columns[i].ColumnName))
                {
                    for (int h = 0; h < YahooList.Columns.Count; h++)
                    {
                        if (YahooList.Columns[h].ColumnName == dtDetail.Columns[i].ColumnName)
                        {
                            YahooList.Rows[YahooList.Rows.Count - 1][h] = dtDetail.Rows[0][i].ToString();
                            break;
                        }
                    }
                }
            }

        }
        protected void OrderChange()  // 8
        {
            //foreach (DataRow dr in dtorder.Rows)
            //{
                byte[] OrderlistReq = OrderInfoParameter("racket-10096568");   //test order but not confirm by ptk
                if (api.TESTMode.Equals("1"))
                    listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderChange"));
                else
                    listRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://test.circus.shopping.yahooapis.jp/ShoppingWebService/V1/orderChange"));
                byte[] bytes;
                bytes = System.Text.Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(OrderlistReq));
                listRequest.ContentType = "application/x-www-form-urlencoded";
                listRequest.ContentLength = bytes.Length;
                listRequest.Method = "POST";
                listRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                Stream requeststream = listRequest.GetRequestStream();
                requeststream.Write(bytes, 0, bytes.Length);
                requeststream.Close();
                HttpWebResponse response;
                try
                {
                    response = listRequest.GetResponse() as HttpWebResponse;
                    var str = new StreamReader(response.GetResponseStream());
                    var xml = str.ReadToEnd();
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xml);
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(xd));
                }
                catch (WebException ex)
                {
                    StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                    var f = sr.ReadToEnd();
                    // wresponse = (WebResponse)listRequest.GetResponse();
                }

            //}

            }
        protected byte[] OrderListParameter()
        {
            MemoryStream meme= new MemoryStream();
            XmlWriterSettings Setting = new XmlWriterSettings();
            Setting.Encoding = Encoding.UTF8;
            Setting.ConformanceLevel = ConformanceLevel.Document;
            Setting.Indent = true;
            XmlWriter writer = XmlWriter.Create(meme, Setting);
            writer.WriteStartElement("Req");
            writer.WriteStartElement("Search");
            writer.WriteElementString("Result", "500");
            writer.WriteElementString("Start", "1");
            writer.WriteElementString("Sort", "+order_time");
            writer.WriteStartElement("Condition");/////Condition
             writer.WriteElementString("OrderTimeFrom", LastGetDateTime.ToString("yyyyMMddHHmmss"));/////Condition
            //writer.WriteElementString("OrderTimeFrom",Convert.ToDateTime("2019/11/12").ToString("yyyyMMddHHmmss"));/////Condition
            writer.WriteElementString("OrderTimeTo", DateTime.Now.ToString("yyyyMMddHHmmss"));/////Condition
            writer.WriteElementString("IsSeen", "true");/////Condition
            writer.WriteElementString("OrderStatus", "1,2");/////Condition
            writer.WriteEndElement();
            writer.WriteElementString("Field", "OrderId");
            writer.WriteEndElement();
            if (api.TESTMode.Equals("1"))
                 writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            else
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            writer.WriteEndElement();
            writer.Flush();
            return meme.ToArray();
        }

        protected byte[] OrderInfoParameter(string Oid)
        {
            MemoryStream meme = new MemoryStream();
            XmlWriterSettings Setting = new XmlWriterSettings();
            Setting.Encoding = Encoding.UTF8;
            Setting.ConformanceLevel = ConformanceLevel.Document;
            Setting.Indent = true;
            XmlWriter writer = XmlWriter.Create(meme, Setting);
            writer.WriteStartElement("Req");
            writer.WriteStartElement("Target");
            writer.WriteElementString("OrderId", Oid.Trim());
            writer.WriteElementString("Field", GetAllColNames());
            writer.WriteEndElement();
            if (api.TESTMode.Equals("1"))
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            else
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            writer.WriteEndElement();
            writer.Flush();
            return meme.ToArray();
        }

        protected byte[] OrderChangeParameter(string Oid)
        {
            MemoryStream meme = new MemoryStream();
            XmlWriterSettings Setting = new XmlWriterSettings();
            Setting.Encoding = Encoding.UTF8;
            Setting.ConformanceLevel = ConformanceLevel.Document;
            Setting.Indent = true;
            XmlWriter writer = XmlWriter.Create(meme, Setting);
            writer.WriteStartElement("Req");
            writer.WriteStartElement("Target");
            writer.WriteElementString("OrderId", Oid.Trim());
            if (api.TESTMode.Equals("1"))
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            else
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Order");
            writer.WriteElementString("IsSeen","True");
            writer.WriteEndElement();
            if (api.TESTMode.Equals("1"))
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            else
                writer.WriteElementString("SellerId", ShopName);//dj0zaiZpPTR4TTIzMWtNbzcxUyZzPWNvbnN1bWVyc2VjcmV0Jng9MDc-
            writer.WriteEndElement();
            writer.Flush();
            return meme.ToArray();

        }
        private D_APIRireki_Entity GetApiRireki()
        {
            apiRireki = new D_APIRireki_Entity()
            {
                 StoreCD = api.StoreCD,
                 APIKey = api.APIKey
            };
            return apiRireki;
        }
        private D_YahooCount_Entity GetOrderCount(XmlDocument xd)
        {
            yahooCount = new D_YahooCount_Entity
            {
                StoreCD = api.StoreCD,
                APIKey = api.APIKey,
                Status = xd.SelectSingleNode("/ResultSet/Result/Status").InnerText,
                Count_NewOrder = xd.SelectSingleNode("/ResultSet/Result/Count/NewOrder").InnerText,
                Count_NewReserve = xd.SelectSingleNode("/ResultSet/Result/Count/NewReserve").InnerText,
                Count_WaitPayment = xd.SelectSingleNode("/ResultSet/Result/Count/WaitPayment").InnerText,
                Count_WaitShipping = xd.SelectSingleNode("/ResultSet/Result/Count/WaitShipping").InnerText,
                Count_Shipping = xd.SelectSingleNode("/ResultSet/Result/Count/Shipping").InnerText,
                Count_Reserve = xd.SelectSingleNode("/ResultSet/Result/Count/Reserve").InnerText,
                Count_Holding = xd.SelectSingleNode("/ResultSet/Result/Count/Holding").InnerText,
                Count_WaitDone = xd.SelectSingleNode("/ResultSet/Result/Count/WaitDone").InnerText,
                Count_Suspect = xd.SelectSingleNode("/ResultSet/Result/Count/Suspect").InnerText,
                Count_SettleError = xd.SelectSingleNode("/ResultSet/Result/Count/SettleError").InnerText,
                Count_Refund = xd.SelectSingleNode("/ResultSet/Result/Count/Refund").InnerText,
                Count_AutoDone = xd.SelectSingleNode("/ResultSet/Result/Count/AutoDone").InnerText,
                Count_AutoWorking = xd.SelectSingleNode("/ResultSet/Result/Count/AutoWorking").InnerText,
                Count_Release = xd.SelectSingleNode("/ResultSet/Result/Count/Release").InnerText,
                Count_NoPayNumber = xd.SelectSingleNode("/ResultSet/Result/Count/NoPayNumber").InnerText

            };
            return yahooCount;
        }

        private D_YahooList_Entity GetOrderInfo(XmlDocument xd)
        {
            yahoolist = new D_YahooList_Entity
            {
           

            };
            
            return yahoolist;
        }


        protected string GetAllColNames()
        {
            string a1 = "OrderId,Version,ParentOrderId,ChildOrderId,DeviceType,MobileCarrierName,IsSeen,IsSplit,CancelReason,CancelReasonDetail";
            string a2 = ",IsRoyalty,IsRoyaltyFix,IsSeller,IsAffiliate,IsRatingB2s,NeedSnl,OrderTime,LastUpdateTime,SuspectMessage,OrderStatus,StoreStatus,RoyaltyFixTime,";
            string a3 = "SendConfirmTime,SendPayTime,PrintSlipTime,PrintDeliveryTime,PrintBillTime,BuyerComments,SellerComments,Notes,OperationUser,Referer";
            string b1 = ",EntryPoint,HistoryId,UsageId,UseCouponData,TotalCouponDiscount,ShippingCouponFlg,ShippingCouponDiscount,CampaignPoints,IsMultiShip,MultiShipId,";
            string b2 = "IsReadOnly,PayStatus,SettleStatus,PayType,PayKind,PayMethod,PayMethodName,SellerHandlingCharge,PayActionTime,PayDate,PayNotes,SettleId,";
            string b3 = "CardBrand,CardNumber,CardNumberLast4,CardExpireYear,CardExpireMonth,CardPayType,CardHolderName,CardPayCount,CardBirthDay,UseYahooCard,UseWallet,NeedBillSlip,";
            string c1 = "NeedDetailedSlip,NeedReceipt,AgeConfirmField,AgeConfirmCheck,BillAddressFrom,BillFirstName,BillFirstNameKana,BillLastName,BillLastNameKana,BillZipCode,BillPrefecture,BillCity";
            string c2 = ",BillCityKana,BillAddress1,BillAddress1Kana,BillAddress2,BillAddress2Kana,BillPhoneNumber,BillEmgPhoneNumber,BillMailAddress,BillSection1Field,BillSection1Value";
            string c3 = ",BillSection2Field,BillSection2Value,PayNo,PayNoIssueDate,ConfirmNumber,PaymentTerm,IsApplePay,ShipStatus,ShipMethod,ShipMethodName";
            string d1 = ",ShipRequestDate,ShipRequestTime,ShipNotes,ShipCompanyCode,ShipInvoiceNumber1,ShipInvoiceNumber2,ShipInvoiceNumberEmptyReason,ShipUrl,ArriveType";
            string d2 = ",ShipDate,ArrivalDate,NeedGiftWrap,GiftWrapType,GiftWrapMessage,NeedGiftWrapPaper,GiftWrapPaperType,GiftWrapName,Option1Field,Option1Type,Option1Value";
            string d3 = ",Option2Field,Option2Type,Option2Value,ShipFirstName,ShipFirstNameKana,ShipLastName,ShipLastNameKana,ShipZipCode,ShipPrefecture,ShipPrefectureKana,ShipCity,ShipCityKana,ShipAddress1";
            string e1 = ",ShipAddress1Kana,ShipAddress2,ShipAddress2Kana,ShipPhoneNumber,ShipEmgPhoneNumber,ShipSection1Field,ShipSection1Value,ShipSection2Field,ShipSection2Value,PayCharge,ShipCharge";
            string e2 = ",GiftWrapCharge,Discount,Adjustments,SettleAmount,UsePoint,TotalPrice,SettlePayAmount,TaxRatio,IsGetPointFixAll,TotalMallCouponDiscount,SellerId,IsLogin,FspLicenseCode,FspLicenseName,GuestAuthId,CombinedPayType,CombinedPayKind,CombinedPayMethod,PayMethodAmount,CombinedPayMethodName,CombinedPayMethodAmount";
            string e3 = ",LineId,ItemId,Title,SubCode,SubCodeOption,IsUsed,ImageId,IsTaxable,Jan,ProductId,CategoryId,AffiliateRatio,UnitPrice,Quantity,PointAvailQuantity,ReleaseDate";
            string f1 = ",PointFspCode,PointRatioY,PointRatioSeller,UnitGetPoint,IsGetPointFix,GetPointFixDate,CouponData,CouponDiscount,CouponUseNum,OriginalPrice,OriginalNum,LeadTimeText,LeadTimeStart,LeadTimeEnd,PriceType";
            string f2 = ",ItemOption";
            string f3 = ",Inscription";
            if (IsItemOptionNode)
            return a1 + a2 + a3 + b1 + b2 + b3 + c1 + c2 + c3 + d1 + d2 + d3 + e1 + e2 + e3 + f1 + f2;
            else
                return a1 + a2 + a3 + b1 + b2 + b3 + c1 + c2 + c3 + d1 + d2 + d3 + e1 + e2 + e3 + f1 + f3;
        }

        public DataTable GetOrderListTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StoreCD");
            dt.Columns.Add("APIKey");
            dt.Columns.Add("InportSEQRows");
            foreach (string cols in GetAllColNames().Split(','))
            {
                dt.Columns.Add(cols);
            }
            return dt;
        }

        public DataTable YahooJucchuu()
        {
           DataTable dt= new DataTable();
           dt.Columns.Add("StoreCD");
            dt.Columns.Add("APIKey");
            dt.Columns.Add("InportSEQRows");
            dt.Columns.Add("YahooOrderId");
            dt.Columns.Add("ParentOrderId");
            dt.Columns.Add("ChildOrderId1");
            dt.Columns.Add("ChildOrderId2");
            dt.Columns.Add("DeviceType");
            dt.Columns.Add("MobileCarrierName");
            dt.Columns.Add("IsSeen");
            dt.Columns.Add("IsSplit");
            dt.Columns.Add("CancelReason");
            dt.Columns.Add("CancelReasonDetail");
            dt.Columns.Add("IsRoyalty");
            dt.Columns.Add("IsRoyaltyFix");
            dt.Columns.Add("IsSeller");
            dt.Columns.Add("IsAffiliate");
            dt.Columns.Add("IsRatingB2s");
            dt.Columns.Add("NeedSnl");
            dt.Columns.Add("OrderTime");
            dt.Columns.Add("LastUpdateTime");
            dt.Columns.Add("Suspect");
            dt.Columns.Add("SuspectMessage");
            dt.Columns.Add("OrderStatus");
            dt.Columns.Add("StoreStatus");
            dt.Columns.Add("RoyaltyFixTime");
            dt.Columns.Add("SendConfirmTime");
            dt.Columns.Add("SendPayTime");
            dt.Columns.Add("PrintSlipTime");
            dt.Columns.Add("PrintDeliveryTime");
            dt.Columns.Add("PrintBillTime");
            dt.Columns.Add("BuyerComments");
            dt.Columns.Add("SellerComments");
            dt.Columns.Add("Notes");
            dt.Columns.Add("OperationUser");
            dt.Columns.Add("Referer");
            dt.Columns.Add("EntryPoint");
            dt.Columns.Add("HistoryId");
            dt.Columns.Add("UsageId");
            dt.Columns.Add("UseCouponData");
            dt.Columns.Add("TotalCouponDiscount");
            dt.Columns.Add("ShippingCouponFlg");
            dt.Columns.Add("ShippingCouponDiscount");
            dt.Columns.Add("CampaignPoints");
            dt.Columns.Add("IsMultiShip");
            dt.Columns.Add("MultiShipId");
            dt.Columns.Add("IsReadOnly");
            dt.Columns.Add("PayStatus");
            dt.Columns.Add("SettleStatus");
            dt.Columns.Add("PayType");
            dt.Columns.Add("PayKind");
            dt.Columns.Add("PayMethod");
            dt.Columns.Add("PayMethodName");
            dt.Columns.Add("SellerHandlingCharge");
            dt.Columns.Add("PayActionTime");
            dt.Columns.Add("PayDate");
            dt.Columns.Add("PayNotes");
            dt.Columns.Add("SettleId");
            dt.Columns.Add("CardBrand");
            dt.Columns.Add("CardNumber");
            dt.Columns.Add("CardNumberLast4");
            dt.Columns.Add("CardExpireYear");
            dt.Columns.Add("CardExpireMonth");
            dt.Columns.Add("CardPayType");
            dt.Columns.Add("CardHolderName");
            dt.Columns.Add("CardPayCount");
            dt.Columns.Add("CardBirthDay");
            dt.Columns.Add("UseYahooCard");
            dt.Columns.Add("UseWallet");
            dt.Columns.Add("NeedBillSlip");
            dt.Columns.Add("NeedDetailedSlip");
            dt.Columns.Add("NeedReceipt");
            dt.Columns.Add("AgeConfirmField");
            dt.Columns.Add("AgeConfirmValue");
            dt.Columns.Add("AgeConfirmCheck");
            dt.Columns.Add("BillAddressFrom");
            dt.Columns.Add("BillFirstName");
            dt.Columns.Add("BillFirstNameKana");
            dt.Columns.Add("BillLastName");
            dt.Columns.Add("BillLastNameKana");
            dt.Columns.Add("BillZipCode");
            dt.Columns.Add("BillPrefecture");
            dt.Columns.Add("BillPrefectureKana");
            dt.Columns.Add("BillCity");
            dt.Columns.Add("BillCityKana");
            dt.Columns.Add("BillAddress1");
            dt.Columns.Add("BillAddress1Kana");
            dt.Columns.Add("BillAddress2");
            dt.Columns.Add("BillAddress2Kana");
            dt.Columns.Add("BillPhoneNumber");
            dt.Columns.Add("BillEmgPhoneNumber");
            dt.Columns.Add("BillMailAddress");
            dt.Columns.Add("BillSection1Field");
            dt.Columns.Add("BillSection1Value");
            dt.Columns.Add("BillSection2Field");
            dt.Columns.Add("BillSection2Value");
            dt.Columns.Add("PayNo");
            dt.Columns.Add("PayNoIssueDate");
            dt.Columns.Add("ConfirmNumber");
            dt.Columns.Add("PaymentTerm");
            dt.Columns.Add("IsApplePay");
            dt.Columns.Add("PayCharge");
            dt.Columns.Add("ShipCharge");
            dt.Columns.Add("GiftWrapCharge");
            dt.Columns.Add("Discount");
            dt.Columns.Add("Adjustments");
            dt.Columns.Add("SettleAmount");
            dt.Columns.Add("UsePoint");
            dt.Columns.Add("TotalPrice");
            dt.Columns.Add("SettlePayAmount");
            dt.Columns.Add("TaxRatio");
            dt.Columns.Add("IsGetPointFixAll");
            dt.Columns.Add("TotalMallCouponDiscount");
            dt.Columns.Add("SellerId");
            dt.Columns.Add("IsLogin");
            dt.Columns.Add("FspLicenseCode");
            dt.Columns.Add("FspLicenseName");
            dt.Columns.Add("GuestAuthId");
            dt.Columns.Add("CombinedPayType");
            dt.Columns.Add("CombinedPayKind");
            dt.Columns.Add("CombinedPayMethod");
            dt.Columns.Add("PayMethodAmount");
            dt.Columns.Add("CombinedPayMethodName");
            dt.Columns.Add("CombinedPayMethodAmount");
            dt.Columns.Add("OrderId");
            return dt;
        }

        public DataTable YahooShipping()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StoreCD");
            dt.Columns.Add("APIKey");
            dt.Columns.Add("InportSEQRows");
            dt.Columns.Add("YahooOrderId");
            dt.Columns.Add("ShipRows");
            dt.Columns.Add("ShipStatus");
            dt.Columns.Add("ShipMethod");
            dt.Columns.Add("ShipMethodName");
            dt.Columns.Add("ShipRequestDate");
            dt.Columns.Add("ShipRequestTime");
            dt.Columns.Add("ShipNotes");
            dt.Columns.Add("ShipCompanyCode");
            dt.Columns.Add("ShipInvoiceNumber1");
            dt.Columns.Add("ShipInvoiceNumber2");
            dt.Columns.Add("ShipInvoiceNumberEmptyReason");
            dt.Columns.Add("ShipUrl");
            dt.Columns.Add("ArriveType");
            dt.Columns.Add("ShipDate");
            dt.Columns.Add("ArrivalDate");
            dt.Columns.Add("NeedGiftWrap");
            dt.Columns.Add("GiftWrapType");
            dt.Columns.Add("GiftWrapMessage");
            dt.Columns.Add("NeedGiftWrapPaper");
            dt.Columns.Add("GiftWrapPaperType");
            dt.Columns.Add("GiftWrapName");
            dt.Columns.Add("Option1Field");
            dt.Columns.Add("Option1Type");
            dt.Columns.Add("Option1Value");
            dt.Columns.Add("Option2Field");
            dt.Columns.Add("Option2Type");
            dt.Columns.Add("Option2Value");
            dt.Columns.Add("ShipFirstName");
            dt.Columns.Add("ShipFirstNameKana");
            dt.Columns.Add("ShipLastName");
            dt.Columns.Add("ShipLastNameKana");
            dt.Columns.Add("ShipZipCode");
            dt.Columns.Add("ShipPrefecture");
            dt.Columns.Add("ShipPrefectureKana");
            dt.Columns.Add("ShipCity");
            dt.Columns.Add("ShipCityKana");
            dt.Columns.Add("ShipAddress1");
            dt.Columns.Add("ShipAddress1Kana");
            dt.Columns.Add("ShipAddress2");
            dt.Columns.Add("ShipAddress2Kana");
            dt.Columns.Add("ShipPhoneNumber");
            dt.Columns.Add("ShipEmgPhoneNumber");
            dt.Columns.Add("ShipSection1Field");
            dt.Columns.Add("ShipSection1Value");
            dt.Columns.Add("ShipSection2Field");
            dt.Columns.Add("ShipSection2Value");
            dt.Columns.Add("OrderId");
            return dt;
        }

        public DataTable YahooJuChuuDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StoreCD");
            dt.Columns.Add("APIKey");
            dt.Columns.Add("InportSEQRows");
            dt.Columns.Add("YahooOrderId");
            dt.Columns.Add("OrderRows");
            dt.Columns.Add("LineId");
            dt.Columns.Add("ItemId");
            dt.Columns.Add("Title");
            dt.Columns.Add("SubCode");
            dt.Columns.Add("SubCodeOption");
            dt.Columns.Add("IsUsed");
            dt.Columns.Add("ImageId");
            dt.Columns.Add("IsTaxable");
            dt.Columns.Add("Jan");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("CategoryId");
            dt.Columns.Add("AffiliateRatio_Store");
            dt.Columns.Add("AffiliateRatio_Yahoo");
            dt.Columns.Add("UnitPrice");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("PointAvailQuantity");
            dt.Columns.Add("ReleaseDate");
            dt.Columns.Add("PointFspCode");
            dt.Columns.Add("PointRatioY");
            dt.Columns.Add("PointRatioSeller");
            dt.Columns.Add("UnitGetPoint");
            dt.Columns.Add("IsGetPointFix");
            dt.Columns.Add("GetPointFixDate");
            dt.Columns.Add("CouponData");
            dt.Columns.Add("CouponDiscount");
            dt.Columns.Add("CouponUseNum");
            dt.Columns.Add("OriginalPrice");
            dt.Columns.Add("OriginalNum");
            dt.Columns.Add("LeadTimeText");
            dt.Columns.Add("LeadTimeStart");
            dt.Columns.Add("LeadTimeEnd");
            dt.Columns.Add("PriceType");
            dt.Columns.Add("ItemOption01Name");
            dt.Columns.Add("ItemOption01Value");
            dt.Columns.Add("ItemOption01Price");
            dt.Columns.Add("ItemOption01KBN");
            dt.Columns.Add("ItemOption02Name");
            dt.Columns.Add("ItemOption02Value");
            dt.Columns.Add("ItemOption02Price");
            dt.Columns.Add("ItemOption02KBN");
            dt.Columns.Add("ItemOption03Name");
            dt.Columns.Add("ItemOption03Value");
            dt.Columns.Add("ItemOption03Price");
            dt.Columns.Add("ItemOption03KBN");
            dt.Columns.Add("ItemOption04Name");
            dt.Columns.Add("ItemOption04Value");
            dt.Columns.Add("ItemOption04Price");
            dt.Columns.Add("ItemOption04KBN");
            dt.Columns.Add("ItemOption05Name");
            dt.Columns.Add("ItemOption05Value");
            dt.Columns.Add("ItemOption05Price");
            dt.Columns.Add("ItemOption05KBN");
            dt.Columns.Add("ItemOption06Name");
            dt.Columns.Add("ItemOption06Value");
            dt.Columns.Add("ItemOption06Price");
            dt.Columns.Add("ItemOption06KBN");
            dt.Columns.Add("ItemOption07Name");
            dt.Columns.Add("ItemOption07Value");
            dt.Columns.Add("ItemOption07Price");
            dt.Columns.Add("ItemOption07KBN");
            dt.Columns.Add("ItemOption08Name");
            dt.Columns.Add("ItemOption08Value");
            dt.Columns.Add("ItemOption08Price");
            dt.Columns.Add("ItemOption08KBN");
            dt.Columns.Add("ItemOption09Name");
            dt.Columns.Add("ItemOption09Value");
            dt.Columns.Add("ItemOption09Price");
            dt.Columns.Add("ItemOption09KBN");
            dt.Columns.Add("ItemOption10Name");
            dt.Columns.Add("ItemOption10Value");
            dt.Columns.Add("ItemOption10Price");
            dt.Columns.Add("ItemOption10KBN");
            dt.Columns.Add("ItemOption11Name");
            dt.Columns.Add("ItemOption11Value");
            dt.Columns.Add("ItemOption11Price");
            dt.Columns.Add("ItemOption11KBN");
            dt.Columns.Add("ItemOption12Name");
            dt.Columns.Add("ItemOption12Value");
            dt.Columns.Add("ItemOption12Price");
            dt.Columns.Add("ItemOption12KBN");
            dt.Columns.Add("ItemOption13Name");
            dt.Columns.Add("ItemOption13Value");
            dt.Columns.Add("ItemOption13Price");
            dt.Columns.Add("ItemOption13KBN");
            dt.Columns.Add("ItemOption14Name");
            dt.Columns.Add("ItemOption14Value");
            dt.Columns.Add("ItemOption14Price");
            dt.Columns.Add("ItemOption14KBN");
            dt.Columns.Add("ItemOption15Name");
            dt.Columns.Add("ItemOption15Value");
            dt.Columns.Add("ItemOption15Price");
            dt.Columns.Add("ItemOption15KBN");
            dt.Columns.Add("ItemOption16Name");
            dt.Columns.Add("ItemOption16Value");
            dt.Columns.Add("ItemOption16Price");
            dt.Columns.Add("ItemOption16KBN");
            dt.Columns.Add("ItemOption17Name");
            dt.Columns.Add("ItemOption17Value");
            dt.Columns.Add("ItemOption17Price");
            dt.Columns.Add("ItemOption17KBN");
            dt.Columns.Add("ItemOption18Name");
            dt.Columns.Add("ItemOption18Value");
            dt.Columns.Add("ItemOption18Price");
            dt.Columns.Add("ItemOption18KBN");
            dt.Columns.Add("ItemOption19Name");
            dt.Columns.Add("ItemOption19Value");
            dt.Columns.Add("ItemOption19Price");
            dt.Columns.Add("ItemOption19KBN");
            dt.Columns.Add("ItemOption20Name");
            dt.Columns.Add("ItemOption20Value");
            dt.Columns.Add("ItemOption20Price");
            dt.Columns.Add("ItemOption20KBN");
            dt.Columns.Add("OrderId");
            return dt;
        }

    }
}
