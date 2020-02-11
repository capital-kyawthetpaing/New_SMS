using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using BL;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;

namespace Wowma_API
{

    public class Common_API
    {
        static Base_Entity be = new Base_Entity();
        static D_WowmaRequest_Entity wowma_entity = new D_WowmaRequest_Entity();
        static WowmaAPI_BL wowma_bl = new WowmaAPI_BL();
        static DataTable dtOrderList,dtResult, dtJuChuuList, dtJuChuuDetail;
        static DateTime s_date = DateTime.Now.AddDays(-60), e_date = DateTime.Now.AddDays(1);
        static ArrayList arrlstJuChu,arrlstJuChuuDetail;
        static int seq = 1,Deseq=1;

        public void GetOrderDetail_Wowma(D_APIControl_Entity dapic_entity)
        {
            dtOrderList = wowma_bl.M_API_Select(dapic_entity);
            Get_WowmaEntity(dtOrderList);
            SetOrderList(wowma_entity);
        }
        private void Get_WowmaEntity(DataTable dtOrderList)
        {
            wowma_entity.APIKey = dtOrderList.Rows[0]["APIKey"].ToString();
            wowma_entity.StoreCD = dtOrderList.Rows[0]["StoreCD"].ToString();
            wowma_entity.LastUpdatedBefore = s_date.ToString();
            wowma_entity.LastUpdatedAfter = e_date.ToString();
            wowma_entity.ApplicationID0 = dtOrderList.Rows[0]["ApplicationID0"].ToString();
        }
        public void SetOrderList(D_WowmaRequest_Entity wow_entity)
        {
            try
            {
                string sdate = s_date.ToString("yyyyMMdd");
                string edate = e_date.ToString("yyyyMMdd");
                string APIKey = wowma_entity.ApplicationID0;

                var webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.manager.wowma.jp/wmshopapi/searchTradeInfoListProc?shopId={0}&totalCount={1}&startDate{2}&startDate={3}&endDate={4}", "39998948", "1000", "1", sdate, edate));
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "GET";
                webRequest.Headers.Add("Authorization", "Bearer " + APIKey);
                HttpWebResponse response;
                response = (HttpWebResponse)webRequest.GetResponse();
                XmlDocument resultXml = new XmlDocument();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resultXml.Load(reader);
                }

                dtResult = GetOrderIDData(resultXml);
                wowma_entity.dtOrderList = dtResult;

                DataTable dtData = wowma_bl.InsertSelect_OrderData(wowma_entity);

                SetOrderInfo(dtData, APIKey); //OrderDetailList
               
            }
            catch (WebException e)
            {
                using (WebResponse response1 = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response1;
                    using (Stream data = response1.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        if (text.Contains("message"))
                        {
                            XmlDocument xd = new XmlDocument();
                            xd.LoadXml(text);
                            text = xd.InnerText;
                        }
                    }

                }
            }
        }
        private void SetOrderInfo(DataTable dtResult, string APIKey)
        {            
            for (int j = 0; j < dtResult.Rows.Count; j++)
            {
                try
                {
                    int orderID = Convert.ToInt32(dtResult.Rows[j]["WowmaOrderId"].ToString());
                    var webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.manager.wowma.jp/wmshopapi/searchTradeInfoProc?shopId={0}&orderId={1}", "39998948", orderID));
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.Method = "GET";
                    webRequest.Headers.Add("Authorization", "Bearer " + APIKey);
                    HttpWebResponse response;
                    response = (HttpWebResponse)webRequest.GetResponse();
                    //Encoding ecoding = Encoding.GetEncoding(("UTF-16"));
                    XmlDocument resultXml = new XmlDocument();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        resultXml.Load(reader);                       
                    }

                    wowma_entity = new D_WowmaRequest_Entity();
                    wowma_entity.OrderListXml = resultXml.InnerXml;
                    wowma_entity.APIKey = dtResult.Rows[j]["APIKey"].ToString();
                    wowma_entity.StoreCD = dtResult.Rows[j]["StoreCD"].ToString();
                    wowma_entity.InportSEQRows = dtResult.Rows[j]["InportSEQ"].ToString();

                    CreateGetOrderData(resultXml, wowma_entity);
                    wowma_bl.InsertSelect_OrderDetail(be);


                }
                catch (WebException e)
                {
                    using (WebResponse response1 = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response1;
                        using (Stream data = response1.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();
                            if (text.Contains("message"))
                            {
                                XmlDocument xd = new XmlDocument();
                                xd.LoadXml(text);
                                text = xd.InnerText;

                            }
                        }

                    }
                }
            }
        }
        private DataTable GetOrderIDData(XmlDocument xml)
        {           
            XmlNodeList nodes = xml.SelectNodes("/response/orderInfo/orderId");

            DataTable dt = new DataTable();
            dt.Columns.Add("SEQ");
            dt.Columns.Add("orderId");
            DataRow rows;
            int i = 1;
            foreach (XmlNode xn in nodes)
            {
                int OrderId = Convert.ToInt32(xn.InnerText);
                rows = dt.NewRow();
                rows["SEQ"] = i++;
                rows["orderId"] = OrderId;
                dt.Rows.Add(rows);               
            }

            return dt;
        }
        private void CreateGetOrderData(XmlDocument Xml,D_WowmaRequest_Entity we)
        {
            CreateTable("JuChuu");
            CreateTable("JuChuuDetail");           
            int row = 0,odrow=1;

            #region For JuChuu
            XmlNodeList xnList = Xml.SelectNodes("/response/orderInfo");
            foreach (XmlNode xn in xnList)
            {
                dtJuChuuList.Rows.Add();
                dtJuChuuList.Rows[row]["InportSEQ"] = we.InportSEQRows;
                dtJuChuuList.Rows[row]["StoreCD"] = we.StoreCD;
                dtJuChuuList.Rows[row]["APIKey"] = we.APIKey;
                dtJuChuuList.Rows[row]["InportSEQRows"] = seq++;
                dtJuChuuList.Rows[row]["orderDate"] = xn["orderDate"] == null ? "" : xn["orderDate"].InnerText;
                dtJuChuuList.Rows[row]["orderId"] = xn["orderId"] == null ? "" : xn["orderId"].InnerText;
                dtJuChuuList.Rows[row]["sellMethodSegment"] = xn["sellMethodSegment"] == null ? "" : xn["sellMethodSegment"].InnerText;
                dtJuChuuList.Rows[row]["releaseDate"] = xn["releaseDate"] == null ? "" : xn["releaseDate"].InnerText;
                dtJuChuuList.Rows[row]["siteAndDevice"] = xn["siteAndDevice"] == null ? "" : xn["siteAndDevice"].InnerText;
                dtJuChuuList.Rows[row]["mailAddress"] = xn["mailAddress"] == null ? "" : xn["mailAddress"].InnerText;
                dtJuChuuList.Rows[row]["rawMailAddress"] = xn["rawMailAddress"]==null ? "" : xn["rawMailAddress"].InnerText;
                dtJuChuuList.Rows[row]["ordererName"] = xn["ordererName"] == null ? "" : xn["ordererName"].InnerText;
                dtJuChuuList.Rows[row]["ordererKana"] = xn["ordererKana"] == null ? "" : xn["ordererKana"].InnerText;
                dtJuChuuList.Rows[row]["ordererZipCode"] = xn["ordererZipCode"] == null ? "" : xn["ordererZipCode"].InnerText;
                dtJuChuuList.Rows[row]["ordererAddress"] = xn["ordererAddress"] == null ? "" : xn["ordererAddress"].InnerText;
                dtJuChuuList.Rows[row]["ordererPhoneNumber1"] = xn["ordererPhoneNumber1"] == null ? "" : xn["ordererPhoneNumber1"].InnerText;
                dtJuChuuList.Rows[row]["ordererPhoneNumber2"] = xn["ordererPhoneNumber2"] == null ? "NULL" : xn["ordererPhoneNumber2"].InnerText;
                dtJuChuuList.Rows[row]["nickname"] = xn["nickname"] == null ? "" : xn["nickname"].InnerText;
                dtJuChuuList.Rows[row]["senderName"] = xn["senderName"] == null ? "" : xn["senderName"].InnerText;
                dtJuChuuList.Rows[row]["senderKana"] = xn["senderKana"] == null ? "" : xn["senderKana"].InnerText;
                dtJuChuuList.Rows[row]["senderZipCode"] = xn["senderZipCode"] == null ? "" : xn["senderZipCode"].InnerText;
                dtJuChuuList.Rows[row]["senderAddress"] = xn["senderAddress"] == null ? "" : xn["senderAddress"].InnerText;
                dtJuChuuList.Rows[row]["senderPhoneNumber1"] = xn["senderPhoneNumber1"] == null ? "" : xn["senderPhoneNumber1"].InnerText;
                dtJuChuuList.Rows[row]["senderPhoneNumber2"] = xn["senderPhoneNumber2"] == null ? "NULL" : xn["senderPhoneNumber2"].InnerText;
                dtJuChuuList.Rows[row]["senderShopCD"] = xn["senderShopCD"] == null ? "" : xn["senderShopCD"].InnerText;
                dtJuChuuList.Rows[row]["orderOption"] = xn["orderOption"] == null ? "" : xn["orderOption"].InnerText;
                dtJuChuuList.Rows[row]["settlementName"] = xn["settlementName"] == null ? "" : xn["settlementName"].InnerText;
                dtJuChuuList.Rows[row]["secureSegment"] = xn["secureSegment"] == null ? "" : xn["secureSegment"].InnerText;
                dtJuChuuList.Rows[row]["userComment"] = xn["userComment"] == null ? "" : xn["userComment"].InnerText;
                dtJuChuuList.Rows[row]["memo"] = xn["memo"] == null ? "" : xn["memo"].InnerText;
                dtJuChuuList.Rows[row]["orderStatus"] = xn["orderStatus"] == null ? "" : xn["orderStatus"].InnerText;
                dtJuChuuList.Rows[row]["contactStatus"] = xn["contactStatus"] == null ? "" : xn["contactStatus"].InnerText;
                dtJuChuuList.Rows[row]["contactDate"] = xn["contactDate"] == null ? "" : xn["contactDate"].InnerText;
                dtJuChuuList.Rows[row]["authorizationStatus"] = xn["authorizationStatus"] == null ? "" : xn["authorizationStatus"].InnerText;
                dtJuChuuList.Rows[row]["authorizationDate"] = xn["authorizationDate"] == null ? "" : xn["authorizationDate"].InnerText;
                dtJuChuuList.Rows[row]["paymentStatus"] = xn["paymentStatus"] == null ? "" : xn["paymentStatus"].InnerText;
                dtJuChuuList.Rows[row]["paymentDate"] = xn["paymentDate"] == null ? "" : xn["paymentDate"].InnerText;
                dtJuChuuList.Rows[row]["shipStatus"] = xn["shipStatus"] == null ? "" : xn["shipStatus"].InnerText;
                dtJuChuuList.Rows[row]["shipDate"] = xn["shipDate"] == null ? "" : xn["shipDate"].InnerText;
                dtJuChuuList.Rows[row]["printStatus"] = xn["printStatus"] == null ? "" : xn["printStatus"].InnerText;
                dtJuChuuList.Rows[row]["printDate"] = xn["printDate"] == null ? "" : xn["printDate"].InnerText;
                dtJuChuuList.Rows[row]["cancelStatus"] = xn["cancelStatus"] == null ? "" : xn["cancelStatus"].InnerText;
                dtJuChuuList.Rows[row]["cancelReason"] = xn["cancelReason"] == null ? "" : xn["cancelReason"].InnerText;
                dtJuChuuList.Rows[row]["cancelComment"] = xn["cancelComment"] == null ? "" : xn["cancelComment"].InnerText;
                dtJuChuuList.Rows[row]["cancelDate"] = xn["cancelDate"] == null ? "" : xn["cancelDate"].InnerText;
                dtJuChuuList.Rows[row]["totalSalePrice"] = xn["totalSalePrice"] == null ? "" : xn["totalSalePrice"].InnerText;
                dtJuChuuList.Rows[row]["totalSalePriceNormalTax"] = xn["totalSalePriceNormalTax"] == null ? "" : xn["totalSalePriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["totalSalePriceReducedTax"] = xn["totalSalePriceReducedTax"] == null ? "" : xn["totalSalePriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["totalSalePriceNoTax"] = xn["totalSalePriceNoTax"] == null ? "" : xn["totalSalePriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["totalSaleUnit"] = xn["totalSaleUnit"] == null ? "" : xn["totalSaleUnit"].InnerText;
                dtJuChuuList.Rows[row]["postagePrice"] = xn["postagePrice"] == null ? "" : xn["postagePrice"].InnerText;
                dtJuChuuList.Rows[row]["postagePriceTaxRate"] = xn["postagePriceTaxRate"] == null ? "" : xn["postagePriceTaxRate"].InnerText;
                dtJuChuuList.Rows[row]["chargePrice"] = xn["chargePrice"] == null ? "" : xn["chargePrice"].InnerText;
                dtJuChuuList.Rows[row]["chargePriceTaxRate"] = xn["chargePriceTaxRate"] == null ? "" : xn["chargePriceTaxRate"].InnerText;
                dtJuChuuList.Rows[row]["totalItemOptionPrice"] = xn["totalItemOptionPrice"] == null ? "" : xn["totalItemOptionPrice"].InnerText;
                dtJuChuuList.Rows[row]["totalItemOptionPriceTaxRate"] = xn["totalItemOptionPriceTaxRate"] == null ? "" : xn["totalItemOptionPriceTaxRate"].InnerText;
                dtJuChuuList.Rows[row]["totalGiftWrappingPrice"] = xn["totalGiftWrappingPrice"] == null ? "" : xn["totalGiftWrappingPrice"].InnerText;
                dtJuChuuList.Rows[row]["totalGiftWrappingPriceTaxRate"] = xn["totalGiftWrappingPriceTaxRate"] == null ? "" : xn["totalGiftWrappingPriceTaxRate"].InnerText;
                dtJuChuuList.Rows[row]["totalPrice"] = xn["totalPrice"] == null ? "" : xn["totalPrice"].InnerText;
                dtJuChuuList.Rows[row]["totalPriceNormalTax"] = xn["totalPriceNormalTax"] == null ? "" : xn["totalPriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["totalPriceReducedTax"] = xn["totalPriceReducedTax"] == null ? "" : xn["totalPriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["totalPriceNoTax"] = xn["totalPriceNoTax"] == null ? "" : xn["totalPriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["premiumType"] = xn["premiumType"] == null ? "" : xn["premiumType"].InnerText;
                dtJuChuuList.Rows[row]["premiumIssuePrice"] = xn["premiumIssuePrice"] == null ? "" : xn["premiumIssuePrice"].InnerText;
                dtJuChuuList.Rows[row]["premiumMallPrice"] = xn["premiumMallPrice"] == null ? "" : xn["premiumMallPrice"].InnerText;
                dtJuChuuList.Rows[row]["premiumShopPrice"] = xn["premiumShopPrice"] == null ? "" : xn["premiumShopPrice"].InnerText;
                dtJuChuuList.Rows[row]["couponTotalPrice"] = xn["couponTotalPrice"] == null ? "" : xn["couponTotalPrice"].InnerText;
                dtJuChuuList.Rows[row]["couponTotalPriceNormalTax"] = xn["couponTotalPriceNormalTax"] == null ? "" : xn["couponTotalPriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["couponTotalPriceReducedTax"] = xn["couponTotalPriceReducedTax"] == null ? "" : xn["couponTotalPriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["couponTotalPriceNoTax"] = xn["couponTotalPriceNoTax"] == null ? "" : xn["couponTotalPriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["usePoint"] = xn["usePoint"] == null ? "" : xn["usePoint"].InnerText;
                dtJuChuuList.Rows[row]["usePointNormalTax"] = xn["usePointNormalTax"] == null ? "" : xn["usePointNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["usePointReducedTax"] = xn["usePointReducedTax"] == null ? "" : xn["usePointReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["usePointNoTax"] = xn["usePointNoTax"] == null ? "" : xn["usePointNoTax"].InnerText;
                dtJuChuuList.Rows[row]["usePointCancel"] = xn["usePointCancel"] == null ? "" : xn["usePointCancel"].InnerText;
                dtJuChuuList.Rows[row]["useAuPointPrice"] = xn["useAuPointPrice"] == null ? "" : xn["useAuPointPrice"].InnerText;
                dtJuChuuList.Rows[row]["useAuPointPriceNormalTax"] = xn["useAuPointPriceNormalTax"] == null ? "" : xn["useAuPointPriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["useAuPointPriceReducedTax"] = xn["useAuPointPriceReducedTax"] == null ? "" : xn["useAuPointPriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["useAuPointPriceNoTax"] = xn["useAuPointPriceNoTax"] == null ? "" : xn["useAuPointPriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["useAuPoint"] = xn["useAuPoint"] == null ? "" : xn["useAuPoint"].InnerText;
                dtJuChuuList.Rows[row]["useAuPointCancel"] = xn["useAuPointCancel"] == null ? "" : xn["useAuPointCancel"].InnerText;
                dtJuChuuList.Rows[row]["requestPrice"] = xn["requestPrice"] == null ? "" : xn["requestPrice"].InnerText;
                dtJuChuuList.Rows[row]["requestPriceNormalTax"] = xn["requestPriceNormalTax"] == null ? "" : xn["requestPriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["requestPriceReducedTax"] = xn["requestPriceReducedTax"] == null ? "" : xn["requestPriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["requestPriceNoTax"] = xn["requestPriceNoTax"] == null ? "" : xn["requestPriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["pointFixedDate"] = xn["pointFixedDate"] == null ? "" : xn["pointFixedDate"].InnerText;
                dtJuChuuList.Rows[row]["pointFixedStatus"] = xn["pointFixedStatus"] == null ? "" : xn["pointFixedStatus"].InnerText;
                dtJuChuuList.Rows[row]["settleStatus"] = xn["settleStatus"] == null ? "" : xn["settleStatus"].InnerText;
                dtJuChuuList.Rows[row]["authoriTimelimitDate"] = xn["authoriTimelimitDate"] == null ? "" : xn["authoriTimelimitDate"].InnerText;
                dtJuChuuList.Rows[row]["pgResult"] = xn["pgResult"] == null ? "" : xn["pgResult"].InnerText;
                dtJuChuuList.Rows[row]["pgResponseCode"] = xn["pgResponseCode"] == null ? "" : xn["pgResponseCode"].InnerText;
                dtJuChuuList.Rows[row]["pgResponseDetail"] = xn["pgResponseDetail"] == null ? "" : xn["pgResponseDetail"].InnerText;
                dtJuChuuList.Rows[row]["pgOrderId"] = xn["pgOrderId"] == null ? "" : xn["pgOrderId"].InnerText;
                dtJuChuuList.Rows[row]["pgRequestPrice"] = xn["pgRequestPrice"] == null ? "" : xn["pgRequestPrice"].InnerText;
                dtJuChuuList.Rows[row]["pgRequestPriceNormalTax"] = xn["pgRequestPriceNormalTax"] == null ? "" : xn["pgRequestPriceNormalTax"].InnerText;
                dtJuChuuList.Rows[row]["pgRequestPriceReducedTax"] = xn["pgRequestPriceReducedTax"] == null ? "" : xn["pgRequestPriceReducedTax"].InnerText;
                dtJuChuuList.Rows[row]["pgRequestPriceNoTax"] = xn["pgRequestPriceNoTax"] == null ? "" : xn["pgRequestPriceNoTax"].InnerText;
                dtJuChuuList.Rows[row]["couponType"] = xn["couponType"] == null ? "" : xn["couponType"].InnerText;
                dtJuChuuList.Rows[row]["couponKey"] = xn["couponKey"] == null ? "" : xn["couponKey"].InnerText;
                dtJuChuuList.Rows[row]["cardJadgement"] = xn["cardJadgement"] == null ? "" : xn["cardJadgement"].InnerText;
                dtJuChuuList.Rows[row]["deliveryName"] = xn["deliveryName"] == null ? "" : xn["deliveryName"].InnerText;
                dtJuChuuList.Rows[row]["deliveryMethodId"] = xn["deliveryMethodId"] == null ? "" : xn["deliveryMethodId"].InnerText;
                dtJuChuuList.Rows[row]["deliveryId"] = xn["deliveryId"] == null ? "" : xn["deliveryId"].InnerText;
                dtJuChuuList.Rows[row]["deliveryRequestDay"] = xn["deliveryRequestDay"] == null ? "" : xn["deliveryRequestDay"].InnerText;
                dtJuChuuList.Rows[row]["deliveryRequestTime"] = xn["deliveryRequestTime"] == null ? "" : xn["deliveryRequestTime"].InnerText;
                dtJuChuuList.Rows[row]["shippingDate"] = xn["shippingDate"] == null ? "" : xn["shippingDate"].InnerText;
                dtJuChuuList.Rows[row]["shippingCarrier"] = xn["shippingCarrier"] == null ? "" : xn["shippingCarrier"].InnerText;
                dtJuChuuList.Rows[row]["shippingNumber"] = xn["shippingNumber"] == null ? "" : xn["shippingNumber"].InnerText;
                dtJuChuuList.Rows[row]["yamatoLnkMgtNo"] = xn["yamatoLnkMgtNo"] == null ? "" : xn["yamatoLnkMgtNo"].InnerText;

                row++;
            }
            #endregion

            #region For JuChuuDetail
            row = 0;
            XmlNodeList xndetail = Xml.SelectNodes("/response/orderInfo/detail");            
            foreach (XmlNode xnn in xndetail)
            {
                //DteailList
                dtJuChuuDetail.Rows.Add();
                dtJuChuuDetail.Rows[row]["InportSEQ"] = we.InportSEQRows;
                dtJuChuuDetail.Rows[row]["StoreCD"] = we.StoreCD;
                dtJuChuuDetail.Rows[row]["APIKey"] = we.APIKey;
                dtJuChuuDetail.Rows[row]["InportSEQRows"] = Deseq++;
                dtJuChuuDetail.Rows[row]["orderId"] = dtJuChuuList.Rows[0]["orderId"].ToString();
                dtJuChuuDetail.Rows[row]["orderRows"] = odrow++;
                dtJuChuuDetail.Rows[row]["orderDetailId"] = xnn["orderDetailId"] == null ? "" : xnn["orderDetailId"].InnerText;
                dtJuChuuDetail.Rows[row]["itemManagementId"] = xnn["itemManagementId"] == null ? "" : xnn["itemManagementId"].InnerText;
                dtJuChuuDetail.Rows[row]["itemCode"] = xnn["itemCode"] == null ? "" : xnn["itemCode"].InnerText;
                dtJuChuuDetail.Rows[row]["lotnumber"] = xnn["lotnumber"] == null ? "" : xnn["lotnumber"].InnerText;
                dtJuChuuDetail.Rows[row]["itemName"] = xnn["itemName"] == null ? "" : xnn["itemName"].InnerText;
                dtJuChuuDetail.Rows[row]["itemOption"] = xnn["itemOption"] == null ? "" : xnn["itemOption"].InnerText;
                dtJuChuuDetail.Rows[row]["itemOptionCommission"] = xnn["itemOptionCommission"] == null ? "" : xnn["itemOptionCommission"].InnerText;
                dtJuChuuDetail.Rows[row]["itemOptionPrice"] = xnn["itemOptionPrice"] == null ? "" : xnn["itemOptionPrice"].InnerText;
                dtJuChuuDetail.Rows[row]["giftWrappingType"] = xnn["giftWrappingType"] == null ? "" : xnn["giftWrappingType"].InnerText;
                dtJuChuuDetail.Rows[row]["giftWrappingPrice"] = xnn["giftWrappingPrice"] == null ? "" : xnn["giftWrappingPrice"].InnerText;
                dtJuChuuDetail.Rows[row]["giftMessage"] = xnn["giftMessage"] == null ? "" : xnn["giftMessage"].InnerText;
                dtJuChuuDetail.Rows[row]["noshiType"] = xnn["noshiType"] == null ? "" : xnn["noshiType"].InnerText;
                dtJuChuuDetail.Rows[row]["noshiPresenterName1"] = xnn["noshiPresenterName1"] == null ? "" : xnn["noshiPresenterName1"].InnerText;
                dtJuChuuDetail.Rows[row]["noshiPresenterName2"] = xnn["noshiPresenterName2"] == null ? "" : xnn["noshiPresenterName2"].InnerText;
                dtJuChuuDetail.Rows[row]["noshiPresenterName3"] = xnn["noshiPresenterName3"] == null ? "" : xnn["noshiPresenterName3"].InnerText;
                dtJuChuuDetail.Rows[row]["itemCancelStatus"] = xnn["itemCancelStatus"] == null ? "" : xnn["itemCancelStatus"].InnerText;
                dtJuChuuDetail.Rows[row]["itemCancelDate"] = xnn["itemCancelDate"] == null ? "" : xnn["itemCancelDate"].InnerText;
                dtJuChuuDetail.Rows[row]["beforeDiscount"] = xnn["beforeDiscount"] == null ? "" : xnn["beforeDiscount"].InnerText;
                dtJuChuuDetail.Rows[row]["discount"] = xnn["discount"] == null ? "" : xnn["discount"].InnerText;
                dtJuChuuDetail.Rows[row]["itemPrice"] = xnn["itemPrice"] == null ? "" : xnn["itemPrice"].InnerText;
                dtJuChuuDetail.Rows[row]["unit"] = xnn["unit"] == null ? "" : xnn["unit"].InnerText;
                dtJuChuuDetail.Rows[row]["totalItemPrice"] = xnn["totalItemPrice"] == null ? "" : xnn["totalItemPrice"].InnerText;
                dtJuChuuDetail.Rows[row]["totalItemChargePrice"] = xnn["totalItemChargePrice"] == null ? "" : xnn["totalItemChargePrice"].InnerText;
                dtJuChuuDetail.Rows[row]["taxType"] = xnn["taxType"] == null ? "" : xnn["taxType"].InnerText;
                dtJuChuuDetail.Rows[row]["reducedTax"] = xnn["reducedTax"] == null ? "" : xnn["reducedTax"].InnerText;
                dtJuChuuDetail.Rows[row]["taxRate"] = xnn["taxRate"] == null ? "" : xnn["taxRate"].InnerText;
                dtJuChuuDetail.Rows[row]["giftPoint"] = xnn["giftPoint"] == null ? "" : xnn["giftPoint"].InnerText;
                dtJuChuuDetail.Rows[row]["shippingDayDispText"] = xnn["shippingDayDispText"] == null ? "" : xnn["shippingDayDispText"].InnerText;
                dtJuChuuDetail.Rows[row]["shippingTimelimitDate"] = xnn["shippingTimelimitDate"] == null ? "" : xnn["shippingTimelimitDate"].InnerText;

                row++;
            }           
            #endregion

            be.dt1 = dtJuChuuList;
            be.dt2 = dtJuChuuDetail;

           
        }       
        public static void CreateTable(string tableType)
        {
           if (tableType == "JuChuu")
            {
                arrlstJuChu = new ArrayList();
                //arrlstJuChu.Add("SEQ");
                arrlstJuChu.Add("orderDate");
                arrlstJuChu.Add("orderId");
                arrlstJuChu.Add("sellMethodSegment");
                arrlstJuChu.Add("releaseDate");
                arrlstJuChu.Add("siteAndDevice");
                arrlstJuChu.Add("mailAddress");
                arrlstJuChu.Add("rawMailAddress");
                arrlstJuChu.Add("ordererName");
                arrlstJuChu.Add("ordererKana");
                arrlstJuChu.Add("ordererZipCode");
                arrlstJuChu.Add("ordererAddress");
                arrlstJuChu.Add("ordererPhoneNumber1");
                arrlstJuChu.Add("ordererPhoneNumber2");
                arrlstJuChu.Add("nickname");
                arrlstJuChu.Add("senderName");
                arrlstJuChu.Add("senderKana");
                arrlstJuChu.Add("senderZipCode");
                arrlstJuChu.Add("senderAddress");
                arrlstJuChu.Add("senderPhoneNumber1");
                arrlstJuChu.Add("senderPhoneNumber2");
                arrlstJuChu.Add("senderShopCD");
                arrlstJuChu.Add("orderOption");
                arrlstJuChu.Add("settlementName");
                arrlstJuChu.Add("secureSegment");
                arrlstJuChu.Add("userComment");
                arrlstJuChu.Add("memo");
                arrlstJuChu.Add("orderStatus");
                arrlstJuChu.Add("contactStatus");
                arrlstJuChu.Add("contactDate");
                arrlstJuChu.Add("authorizationStatus");
                arrlstJuChu.Add("authorizationDate");
                arrlstJuChu.Add("paymentStatus");
                arrlstJuChu.Add("paymentDate");
                arrlstJuChu.Add("shipStatus");
                arrlstJuChu.Add("shipDate");
                arrlstJuChu.Add("printStatus");
                arrlstJuChu.Add("printDate");
                arrlstJuChu.Add("cancelStatus");
                arrlstJuChu.Add("cancelReason");
                arrlstJuChu.Add("cancelComment");
                arrlstJuChu.Add("cancelDate");
                arrlstJuChu.Add("totalSalePrice");
                arrlstJuChu.Add("totalSalePriceNormalTax");
                arrlstJuChu.Add("totalSalePriceReducedTax");
                arrlstJuChu.Add("totalSalePriceNoTax");
                arrlstJuChu.Add("totalSaleUnit");
                arrlstJuChu.Add("postagePrice");
                arrlstJuChu.Add("postagePriceTaxRate");
                arrlstJuChu.Add("chargePrice");
                arrlstJuChu.Add("chargePriceTaxRate");
                arrlstJuChu.Add("totalItemOptionPrice");
                arrlstJuChu.Add("totalItemOptionPriceTaxRate");
                arrlstJuChu.Add("totalGiftWrappingPrice");
                arrlstJuChu.Add("totalGiftWrappingPriceTaxRate");
                arrlstJuChu.Add("totalPrice");
                arrlstJuChu.Add("totalPriceNormalTax");
                arrlstJuChu.Add("totalPriceReducedTax");
                arrlstJuChu.Add("totalPriceNoTax");
                arrlstJuChu.Add("premiumType");
                arrlstJuChu.Add("premiumIssuePrice");
                arrlstJuChu.Add("premiumMallPrice");
                arrlstJuChu.Add("premiumShopPrice");
                arrlstJuChu.Add("couponTotalPrice");
                arrlstJuChu.Add("couponTotalPriceNormalTax");
                arrlstJuChu.Add("couponTotalPriceReducedTax");
                arrlstJuChu.Add("couponTotalPriceNoTax");
                arrlstJuChu.Add("usePoint");
                arrlstJuChu.Add("usePointNormalTax");
                arrlstJuChu.Add("usePointReducedTax");
                arrlstJuChu.Add("usePointNoTax");
                arrlstJuChu.Add("usePointCancel");
                arrlstJuChu.Add("useAuPointPrice");
                arrlstJuChu.Add("useAuPointPriceNormalTax");
                arrlstJuChu.Add("useAuPointPriceReducedTax");
                arrlstJuChu.Add("useAuPointPriceNoTax");
                arrlstJuChu.Add("useAuPoint");
                arrlstJuChu.Add("useAuPointCancel");
                arrlstJuChu.Add("requestPrice");
                arrlstJuChu.Add("requestPriceNormalTax");
                arrlstJuChu.Add("requestPriceReducedTax");
                arrlstJuChu.Add("requestPriceNoTax");
                arrlstJuChu.Add("pointFixedDate");
                arrlstJuChu.Add("pointFixedStatus");
                arrlstJuChu.Add("settleStatus");
                arrlstJuChu.Add("authoriTimelimitDate");
                arrlstJuChu.Add("pgResult");
                arrlstJuChu.Add("pgResponseCode");
                arrlstJuChu.Add("pgResponseDetail");
                arrlstJuChu.Add("pgOrderId");
                arrlstJuChu.Add("pgRequestPrice");
                arrlstJuChu.Add("pgRequestPriceNormalTax");
                arrlstJuChu.Add("pgRequestPriceReducedTax");
                arrlstJuChu.Add("pgRequestPriceNoTax");
                arrlstJuChu.Add("couponType");
                arrlstJuChu.Add("couponKey");
                arrlstJuChu.Add("cardJadgement");
                arrlstJuChu.Add("deliveryName");
                arrlstJuChu.Add("deliveryMethodId");
                arrlstJuChu.Add("deliveryId");
                arrlstJuChu.Add("deliveryRequestDay");
                arrlstJuChu.Add("deliveryRequestTime");
                arrlstJuChu.Add("shippingDate");
                arrlstJuChu.Add("shippingCarrier");
                arrlstJuChu.Add("shippingNumber");
                arrlstJuChu.Add("yamatoLnkMgtNo");

                dtJuChuuList = new DataTable();
                dtJuChuuList.Columns.Add("InportSEQ", typeof(string));
                dtJuChuuList.Columns.Add("StoreCD", typeof(string));
                dtJuChuuList.Columns.Add("APIKey", typeof(string));
                dtJuChuuList.Columns.Add("InportSEQRows", typeof(string));

                foreach (string colname in arrlstJuChu)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                
            }

            else  if(tableType=="JuChuuDetail")
            {
                arrlstJuChuuDetail = new ArrayList();
                arrlstJuChuuDetail.Add("orderId");
                arrlstJuChuuDetail.Add("orderRows");
                arrlstJuChuuDetail.Add("orderDetailId");
                arrlstJuChuuDetail.Add("itemManagementId");
                arrlstJuChuuDetail.Add("itemCode");
                arrlstJuChuuDetail.Add("lotnumber");
                arrlstJuChuuDetail.Add("itemName");
                arrlstJuChuuDetail.Add("itemOption");
                arrlstJuChuuDetail.Add("itemOptionCommission");
                arrlstJuChuuDetail.Add("itemOptionPrice");
                arrlstJuChuuDetail.Add("giftWrappingType");
                arrlstJuChuuDetail.Add("giftWrappingPrice");
                arrlstJuChuuDetail.Add("giftMessage");
                arrlstJuChuuDetail.Add("noshiType");
                arrlstJuChuuDetail.Add("noshiPresenterName1");
                arrlstJuChuuDetail.Add("noshiPresenterName2");
                arrlstJuChuuDetail.Add("noshiPresenterName3");
                arrlstJuChuuDetail.Add("itemCancelStatus");
                arrlstJuChuuDetail.Add("itemCancelDate");
                arrlstJuChuuDetail.Add("beforeDiscount");
                arrlstJuChuuDetail.Add("discount");
                arrlstJuChuuDetail.Add("itemPrice");
                arrlstJuChuuDetail.Add("unit");
                arrlstJuChuuDetail.Add("totalItemPrice");
                arrlstJuChuuDetail.Add("totalItemChargePrice");
                arrlstJuChuuDetail.Add("taxType");
                arrlstJuChuuDetail.Add("reducedTax");
                arrlstJuChuuDetail.Add("taxRate");
                arrlstJuChuuDetail.Add("giftPoint");
                arrlstJuChuuDetail.Add("shippingDayDispText");
                arrlstJuChuuDetail.Add("shippingTimelimitDate");

                dtJuChuuDetail = new DataTable();
                dtJuChuuDetail.Columns.Add("InportSEQ", typeof(string));
                dtJuChuuDetail.Columns.Add("StoreCD", typeof(string));
                dtJuChuuDetail.Columns.Add("APIKey", typeof(string));
                dtJuChuuDetail.Columns.Add("InportSEQRows", typeof(string));
                foreach (string colname in arrlstJuChuuDetail)
                    dtJuChuuDetail.Columns.Add(colname, typeof(string));
            }

        }
       
    }
}