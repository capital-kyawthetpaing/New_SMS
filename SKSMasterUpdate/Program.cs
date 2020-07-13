using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace SKSMasterUpdate
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static Base_BL basebl = new Base_BL();
        static SKSMasterUpdate_BL sksMasterUpdate_bl = new SKSMasterUpdate_BL();
        static void Main(string[] args)
        {
            Console.Title = "SKSMasterUpdate_SKS連携商基本商品";

            string jsResult = string.Empty;

            if (loginbl.ReadConfig() == true)
            {
                jsResult = "{";
                DataTable dtItem = sksMasterUpdate_bl.SKS_Item_Select();
                DataTable dtMasterItem = new DataTable();
                if (dtItem != null && dtItem.Rows.Count > 0)
                {
                    dtMasterItem = dtItem.DefaultView.ToTable(true, "Item_Code");
                    jsResult += "\"item\":" + datatableToJason(dtItem) + ",";
                    
                }

                else
                {
                    jsResult += "\"item\":null,";
                }


                DataTable dtSKU = sksMasterUpdate_bl.SKS_SKU_Select();
                DataTable dtMasterSKU = dtSKU.DefaultView.ToTable(true, "Item_AdminCode");
                if (dtSKU != null && dtSKU.Rows.Count > 0)
                {
                    jsResult += "\"sku\":" + datatableToJason(dtSKU);
                    
                }
                else
                {
                    jsResult += "\"sku\":null";
                }

                jsResult += "}";

                if (ImportToSKS(jsResult))
                {

                    sksMasterUpdate_bl.SKSUpdateFlg_ForItem(dtMasterItem,dtMasterSKU);

                }

            }

        }

        public static string datatableToJason(DataTable dt)
        {
            string JS = string.Empty;
            JS = JsonConvert.SerializeObject(dt);
            return JS;
        }

        private static bool ImportToSKS(string jsResult)
        {
            try
            {
                string SKSPath = loginbl.GetInformationOfIniFileByKey("SKS_Path");

                var http = (HttpWebRequest)WebRequest.Create(new Uri(SKSPath));
                http.Accept = "application/json";
                http.ContentType = "application/json";
                http.Method = "POST";
                string parsedContent = jsResult;
                Encoding encoding = Encoding.UTF8;
                Byte[] bytes = encoding.GetBytes(parsedContent);
                http.Timeout = System.Threading.Timeout.Infinite;

                http.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                Stream newStream = http.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

    }
}
 