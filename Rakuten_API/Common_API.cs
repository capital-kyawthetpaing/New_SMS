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
using System.Collections;

namespace Rakuten_API
{
    public class Common_API
    {
        static Base_Entity base_entity;
        static D_RakutenRequest_Entity DRakutenReq_entity = new D_RakutenRequest_Entity();
        static RakutenAPI_BL rakutenAPI_bl = new RakutenAPI_BL();

        static DataTable dtOrderList, dtSecretKey, dtJuChuuList, dtShippingList, dtCouponList, dtChangeReason, dtJuChuuDetail, dtShippingDetail;

        static ArrayList arrlstJuChu, arrlstOrderModel, arrlstsettlementModel, arrlstdeliveryModel, arrlstpointModel, arrlstwrapModel1, arrlstwrapModel2;
        static ArrayList arrlstPackageModel, arrlstSenderModel, arrlstDeliveryCvsModel;
        static ArrayList arrlstCouponModelList, arrlstChangeReason, arrlstJuChuuDetail, arrlstShippingDetail;

        static DateTime startDate, endDate;
        public void Search_GetOrderDetail(D_APIControl_Entity DApiControl_entity)
        {
            startDate = DateTime.Now.AddDays(-60);
            endDate = DateTime.Now.AddDays(1);

            dtSecretKey = rakutenAPI_bl.M_API_Select(DApiControl_entity);

            GetD_RakutenReqEntity();

            if (dtSecretKey.Rows.Count > 0)
            {
                JObject jObject = SearOrder();
                CreateSearchOrderData(jObject);
                DataTable dtOrderList = rakutenAPI_bl.InsertSelect_SearchOrderData(DRakutenReq_entity);

                if (dtOrderList.Rows.Count > 0)
                {
                    for (int row = 0; row < dtOrderList.Rows.Count; row++)
                    {
                        JObject jObjectInfo = GetOrder(dtOrderList.Rows[row]["RakutenOrderNumber"].ToString());
                        CreateGetOrderDataInfo(jObjectInfo, dtOrderList.Rows[row]["InportSEQ"].ToString(), dtOrderList.Rows[row]["InportSEQRows"].ToString());
                        //JObject jObjectInfo = GetOrder("a");
                        //CreateGetOrderDataInfo(jObjectInfo,"1",1);
                        rakutenAPI_bl.Insert_GetOrderData(base_entity);
                    }
                }
            }
        }

        public void GetD_RakutenReqEntity()
        {
            DRakutenReq_entity.APIKey = dtSecretKey.Rows[0]["APIKey"].ToString();
            DRakutenReq_entity.StoreCD = dtSecretKey.Rows[0]["StoreCD"].ToString();
            DRakutenReq_entity.LastUpdatedBefore = startDate.ToString();
            DRakutenReq_entity.LastUpdatedAfter = endDate.ToString();
            DRakutenReq_entity.InsertOperator = "Console";

        }

        public JObject SearOrder()
        {
            string re = string.Empty;
            String startdate = @"""" + startDate.ToString("yyyy-MM-dd'T'HH:mm:ssK").Remove(22, 1) + @"""";
            String enddate = @"""" + endDate.ToString("yyyy-MM-dd'T'HH:mm:ssK").Remove(22, 1) + @"""";
            //re = @"{ ""orderProgressList"": [100 ],
            //         ""dateType"": 1,
            //           ""startDatetime"": " + startdate + @",
            //            ""endDatetime"": " + enddate + @",
            //        ""PaginationResponseModel"": {
            //                                    ""requestRecordsAmount"":1000,
            //                                    ""requestPage"": 1

            //                                  }

            //}";
            re = @"{ ""orderProgressList"": [100, 200, 300, 400, 500, 600, 700, 800, 900 ],
                     ""dateType"": 1,
                       ""startDatetime"": ""2019-07-10T16:58:46+0900"",
                        ""endDatetime"": ""2019-07-30T22:58:46+0900"",
                     ""PaginationResponseModel"": {
                            ""requestRecordsAmount"":1000,
                            ""requestPage"": 1

                          }


                    }";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.rms.rakuten.co.jp/es/2.0/order/searchOrder/");

            Encoding encoding = Encoding.UTF8;
            Byte[] bytes = encoding.GetBytes(re);
            request.Accept = "application/json";
            request.Method = "POST";
            String encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(dtSecretKey.Rows[0]["ServiceSecret"].ToString() + ":" + dtSecretKey.Rows[0]["LicenseKey"].ToString()));
            request.Headers.Add("Authorization", "ESA " + encoded);
            request.ContentType = @"application/json; charset=utf-8";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string responseStr = new StreamReader(responseStream).ReadToEnd();

            return JObject.Parse(responseStr);

        }

        public static void CreateSearchOrderData(JObject jObject)
        {
            int i = 0;
            dtOrderList = new DataTable();
            dtOrderList.Columns.Add("SEQ", typeof(int));
            dtOrderList.Columns.Add("OrderNo", typeof(string));

            JArray ordernumber = (JArray)jObject.SelectToken("orderNumberList");
            string message = (string)jObject.SelectToken("MessageModelList[0].message");
            if (ordernumber.Count > 0 && message.Contains("注文検索に成功しました。"))
            {
                foreach (JToken order in ordernumber)
                {
                    dtOrderList.Rows.Add();
                    dtOrderList.Rows[i]["SEQ"] = i + 1;
                    dtOrderList.Rows[i++]["OrderNo"] = order.ToString();
                }

            }
            DRakutenReq_entity.dtOrderList = dtOrderList;
        }

        public static JObject GetOrder(string orderNo)
        {
            //string getRe = @"{ ""orderNumberList"": [""370407-20190729-00000831""],""version"" : ""2"" }";

            string getRe = @"{ ""orderNumberList"": [" + orderNo + @"],""version"" : ""2"" }";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("https://api.rms.rakuten.co.jp/es/2.0/order/getOrder/");

            Encoding encode = Encoding.UTF8;
            Byte[] bytes1 = encode.GetBytes(getRe);
            request1.Accept = "application/json";
            request1.Method = "POST";
            String getencoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(dtSecretKey.Rows[0]["ServiceSecret"].ToString() + ":" + dtSecretKey.Rows[0]["LicenseKey"].ToString()));
            request1.Headers.Add("Authorization", "ESA " + getencoded);
            request1.ContentType = @"application/json; charset=utf-8";
            Stream GetrequestStream = request1.GetRequestStream();
            GetrequestStream.Write(bytes1, 0, bytes1.Length);
            GetrequestStream.Close();

            HttpWebResponse Getresponse = (HttpWebResponse)request1.GetResponse();
            Stream GetresponseStream = Getresponse.GetResponseStream();
            string GetresponseStr = new StreamReader(GetresponseStream).ReadToEnd();

            return JObject.Parse(GetresponseStr);
        }

        public static void CreateGetOrderDataInfo(JObject jObjectInfo, string inportSEQ, string seq)
        {

            base_entity = new Base_Entity();

            CreateDataTable("JuChuu");
            CreateDataTable("Shipping");
            CreateDataTable("Coupon");
            CreateDataTable("ChangeReason");
            CreateDataTable("JuChuuDetail");
            CreateDataTable("ShippingDetail");

            JArray orderNoList = (JArray)jObjectInfo.SelectToken("OrderModelList");
            int row = 0;
            foreach (JToken order in orderNoList)
            {
                JObject orderModel = String.IsNullOrWhiteSpace(order.SelectToken("OrdererModel").ToString()) ? null : (JObject)order.SelectToken("OrdererModel");
                JObject settlementModel = String.IsNullOrWhiteSpace(order.SelectToken("SettlementModel").ToString()) ? null : (JObject)order.SelectToken("SettlementModel");
                JObject pointModel = String.IsNullOrWhiteSpace(order.SelectToken("PointModel").ToString()) ? null : (JObject)order.SelectToken("PointModel");
                JObject deliveryModel = String.IsNullOrWhiteSpace(order.SelectToken("DeliveryModel").ToString()) ? null : (JObject)order.SelectToken("DeliveryModel");
                JObject wrapModel1 = String.IsNullOrWhiteSpace(order.SelectToken("WrappingModel1").ToString()) ? null : (JObject)order.SelectToken("WrappingModel1");
                JObject wrapModel2 = String.IsNullOrWhiteSpace(order.SelectToken("WrappingModel2").ToString()) ? null : (JObject)order.SelectToken("WrappingModel2");

                #region For JuChuu
                dtJuChuuList.Rows.Add();
                dtJuChuuList.Rows[row]["InportSEQ"] = inportSEQ;
                dtJuChuuList.Rows[row]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                dtJuChuuList.Rows[row]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                dtJuChuuList.Rows[row]["InportSEQRows"] = seq;

                foreach (string jsonstr in arrlstJuChu)
                {
                    SelectFromJSon(dtJuChuuList.Rows[row], order, jsonstr);

                }
                if (orderModel != null)
                    foreach (string jsonstr in arrlstOrderModel)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], orderModel, jsonstr);

                    }
                if (settlementModel != null)
                    foreach (string jsonstr in arrlstsettlementModel)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], settlementModel, jsonstr);

                    }
                if (pointModel != null)
                    foreach (string jsonstr in arrlstpointModel)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], pointModel, jsonstr);

                    }
                if (deliveryModel != null)
                    foreach (string jsonstr in arrlstdeliveryModel)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], deliveryModel, jsonstr);

                    }
                if (wrapModel1 != null)
                    foreach (string jsonstr in arrlstwrapModel1)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], wrapModel1, jsonstr);

                    }
                if (wrapModel2 != null)
                    foreach (string jsonstr in arrlstwrapModel2)
                    {
                        SelectFromJSon(dtJuChuuList.Rows[row], wrapModel2, jsonstr);

                    }


                #endregion

                #region For Shipping
                JArray packageModelList = String.IsNullOrWhiteSpace(order.SelectToken("PackageModelList").ToString()) ? null : (JArray)order.SelectToken("PackageModelList");
                int rowp = 0;
                if (packageModelList != null)
                {
                    foreach (JToken package in packageModelList)
                    {
                        JObject senderModel = String.IsNullOrWhiteSpace(package.SelectToken("SenderModel").ToString()) ? null : (JObject)package.SelectToken("SenderModel");
                        JObject deliveryCvsModel = String.IsNullOrWhiteSpace(package.SelectToken("DeliveryCvsModel").ToString()) ? null : (JObject)package.SelectToken("DeliveryCvsModel");

                        dtShippingList.Rows.Add();
                        dtShippingList.Rows[rowp]["InportSEQ"] = inportSEQ;
                        dtShippingList.Rows[rowp]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                        dtShippingList.Rows[rowp]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                        dtShippingList.Rows[rowp]["InportSEQRows"] = seq;
                        dtShippingList.Rows[rowp]["orderNumber"] = String.IsNullOrWhiteSpace(order.SelectToken("orderNumber").ToString()) ? "null" : order.SelectToken("orderNumber").ToString();
                        dtShippingList.Rows[rowp]["basketRows"] = rowp + 1;

                        foreach (string jsonstr in arrlstPackageModel)
                        {
                            SelectFromJSon(dtShippingList.Rows[rowp], package, jsonstr);

                        }
                        if (senderModel != null)
                            foreach (string jsonstr in arrlstSenderModel)
                            {
                                SelectFromJSon(dtShippingList.Rows[rowp], senderModel, jsonstr);

                            }
                        if (deliveryCvsModel != null)
                            foreach (string jsonstr in arrlstDeliveryCvsModel)
                            {
                                SelectFromJSon(dtShippingList.Rows[rowp], deliveryCvsModel, jsonstr);

                            }

                        #region For JuChuuDetails
                        JArray juChuuDetails = String.IsNullOrWhiteSpace(package.SelectToken("ItemModelList").ToString()) ? null : (JArray)package.SelectToken("ItemModelList");
                        int rowj = 0;
                        if (juChuuDetails != null)
                        {
                            foreach (JToken juchuu in juChuuDetails)
                            {
                                dtJuChuuDetail.Rows.Add();
                                dtJuChuuDetail.Rows[rowj]["InportSEQ"] = inportSEQ;
                                dtJuChuuDetail.Rows[rowj]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                                dtJuChuuDetail.Rows[rowj]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                                dtJuChuuDetail.Rows[rowj]["InportSEQRows"] = seq;
                                dtJuChuuDetail.Rows[rowj]["orderNumber"] = String.IsNullOrWhiteSpace(order.SelectToken("orderNumber").ToString()) ? "null" : order.SelectToken("orderNumber").ToString();
                                dtJuChuuDetail.Rows[rowj]["basketRows"] = rowp + 1;
                                dtJuChuuDetail.Rows[rowj]["itemRows"] = rowj + 1;

                                foreach (string jsonstr in arrlstJuChuuDetail)
                                {
                                    SelectFromJSon(dtJuChuuDetail.Rows[rowj], juchuu, jsonstr);

                                }
                                rowj++;
                            }
                        }

                        #endregion

                        #region For Shipping Details
                        JArray shippingModelList = String.IsNullOrWhiteSpace(package.SelectToken("ShippingModelList").ToString()) ? null : (JArray)package.SelectToken("ShippingModelList");
                        int rows = 0;
                        if (shippingModelList != null)
                        {
                            foreach (JToken shipping in shippingModelList)
                            {
                                dtShippingDetail.Rows.Add();
                                dtShippingDetail.Rows[rows]["InportSEQ"] = inportSEQ;
                                dtShippingDetail.Rows[rows]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                                dtShippingDetail.Rows[rows]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                                dtShippingDetail.Rows[rows]["InportSEQRows"] = seq;
                                dtShippingDetail.Rows[rows]["orderNumber"] = String.IsNullOrWhiteSpace(order.SelectToken("orderNumber").ToString()) ? "null" : order.SelectToken("orderNumber").ToString();
                                dtShippingDetail.Rows[rows]["basketRows"] = rowp + 1;
                                dtShippingDetail.Rows[rows]["ShippingRows"] = rows + 1;//itemRow to shippingRow changed 2021/07/07

                                foreach (string jsonstr in arrlstShippingDetail)
                                {
                                    SelectFromJSon(dtShippingDetail.Rows[rows], shipping, jsonstr);
                                }
                                rows++;
                            }
                        }

                        rowp++;
                        #endregion
                    }
                    #endregion

                    #region For Coupon
                    JArray couponModelList = String.IsNullOrWhiteSpace(order.SelectToken("CouponModelList").ToString()) ? null : (JArray)order.SelectToken("CouponModelList");
                    int rowc = 0;
                    if (couponModelList != null)
                    {
                        foreach (JToken coupon in couponModelList)
                        {
                            dtCouponList.Rows.Add();
                            dtCouponList.Rows[rowc]["InportSEQ"] = inportSEQ;
                            dtCouponList.Rows[rowc]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                            dtCouponList.Rows[rowc]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                            dtCouponList.Rows[rowc]["InportSEQRows"] = seq;
                            dtCouponList.Rows[rowc]["orderNumber"] = String.IsNullOrWhiteSpace(order.SelectToken("orderNumber").ToString()) ? "null" : order.SelectToken("orderNumber").ToString();
                            dtCouponList.Rows[rowc]["couponRows"] = rowc + 1;

                            foreach (string jsonstr in arrlstCouponModelList)
                            {
                                SelectFromJSon(dtCouponList.Rows[rowc], coupon, jsonstr);

                            }
                            rowc++;
                        }

                    }

                    #endregion

                    #region For ChangeReason
                    JArray changeReasonModelList = String.IsNullOrWhiteSpace(order.SelectToken("ChangeReasonModelList").ToString()) ? null : (JArray)order.SelectToken("ChangeReasonModelList");
                    int rowr = 0;
                    if (changeReasonModelList != null)
                    {
                        foreach (JToken reason in changeReasonModelList)
                        {
                            dtChangeReason.Rows.Add();
                            dtChangeReason.Rows[rowr]["InportSEQ"] = inportSEQ;
                            dtChangeReason.Rows[rowr]["StoreCD"] = dtSecretKey.Rows[0]["StoreCD"].ToString();
                            dtChangeReason.Rows[rowr]["APIKey"] = dtSecretKey.Rows[0]["APIKey"].ToString();
                            dtChangeReason.Rows[rowr]["InportSEQRows"] = seq;
                            dtChangeReason.Rows[rowr]["orderNumber"] = String.IsNullOrWhiteSpace(order.SelectToken("orderNumber").ToString()) ? "null" : order.SelectToken("orderNumber").ToString();
                            dtChangeReason.Rows[rowr]["reasonRows"] = rowr + 1;

                            foreach (string jsonstr in arrlstChangeReason)
                            {
                                SelectFromJSon(dtChangeReason.Rows[rowr], reason, jsonstr);//changed rowc to rowr by ses
                            }
                            rowr++;
                        }

                    }
                    #endregion

                    row++;

                }
            }
            base_entity.dt1 = dtJuChuuList;
            base_entity.dt2 = dtShippingList;
            base_entity.dt3 = dtCouponList;
            base_entity.dt4 = dtChangeReason;
            base_entity.dt5 = dtJuChuuDetail;
            base_entity.dt6 = dtShippingDetail;
            base_entity.InsertOperator = "Console";
        }

        public static void SelectFromJSon(DataRow dr, JToken order, string jsonstr)
        {
            if (!String.IsNullOrWhiteSpace(order.SelectToken(jsonstr).ToString()))
                dr[jsonstr] = order.SelectToken(jsonstr).ToString();
        }

        public static void CreateDataTable(string tableType)
        {
            if (tableType == "JuChuu")
            {
                arrlstJuChu = new ArrayList();
                arrlstJuChu.Add("orderNumber");
                arrlstJuChu.Add("orderProgress");
                arrlstJuChu.Add("subStatusId");
                arrlstJuChu.Add("subStatusName");
                arrlstJuChu.Add("orderDatetime");
                arrlstJuChu.Add("shopOrderCfmDatetime");
                arrlstJuChu.Add("orderFixDatetime");
                arrlstJuChu.Add("shippingInstDatetime");
                arrlstJuChu.Add("shippingCmplRptDatetime");
                arrlstJuChu.Add("cancelDueDate");
                arrlstJuChu.Add("deliveryDate");
                arrlstJuChu.Add("shippingTerm");
                arrlstJuChu.Add("remarks");
                arrlstJuChu.Add("giftCheckFlag");
                arrlstJuChu.Add("severalSenderFlag");
                arrlstJuChu.Add("equalSenderFlag");
                arrlstJuChu.Add("isolatedIslandFlag");
                arrlstJuChu.Add("rakutenMemberFlag");
                arrlstJuChu.Add("carrierCode");
                arrlstJuChu.Add("emailCarrierCode");
                arrlstJuChu.Add("orderType");
                arrlstJuChu.Add("reserveNumber");
                arrlstJuChu.Add("reserveDeliveryCount");
                arrlstJuChu.Add("cautionDisplayType");
                arrlstJuChu.Add("rakutenConfirmFlag");
                arrlstJuChu.Add("goodsPrice");
                arrlstJuChu.Add("goodsTax");
                arrlstJuChu.Add("postagePrice");
                arrlstJuChu.Add("deliveryPrice");
                arrlstJuChu.Add("paymentCharge");
                arrlstJuChu.Add("totalPrice");
                arrlstJuChu.Add("requestPrice");
                arrlstJuChu.Add("couponAllTotalPrice");
                arrlstJuChu.Add("couponShopPrice");
                arrlstJuChu.Add("couponOtherPrice");
                arrlstJuChu.Add("additionalFeeOccurAmountToUser");
                arrlstJuChu.Add("additionalFeeOccurAmountToShop");
                arrlstJuChu.Add("asurakuFlag");
                arrlstJuChu.Add("drugFlag");
                arrlstJuChu.Add("dealFlag");
                arrlstJuChu.Add("membershipType");
                arrlstJuChu.Add("memo");
                arrlstJuChu.Add("operator");
                arrlstJuChu.Add("mailPlugSentence");
                arrlstJuChu.Add("modifyFlag");
                arrlstJuChu.Add("isTaxRecalc");

                arrlstOrderModel = new ArrayList();
                arrlstOrderModel.Add("zipCode1");
                arrlstOrderModel.Add("zipCode2");
                arrlstOrderModel.Add("prefecture");
                arrlstOrderModel.Add("city");
                arrlstOrderModel.Add("subAddress");
                arrlstOrderModel.Add("familyName");
                arrlstOrderModel.Add("firstName");
                arrlstOrderModel.Add("familyNameKana");
                arrlstOrderModel.Add("firstNameKana");
                arrlstOrderModel.Add("phoneNumber1");
                arrlstOrderModel.Add("phoneNumber2");
                arrlstOrderModel.Add("phoneNumber3");
                arrlstOrderModel.Add("emailAddress");
                arrlstOrderModel.Add("sex");
                arrlstOrderModel.Add("birthYear");
                arrlstOrderModel.Add("birthMonth");
                arrlstOrderModel.Add("birthDay");

                arrlstsettlementModel = new ArrayList();
                arrlstsettlementModel.Add("settlementMethod");
                arrlstsettlementModel.Add("rpaySettlementFlag");
                arrlstsettlementModel.Add("cardName");
                arrlstsettlementModel.Add("cardNumber");
                arrlstsettlementModel.Add("cardOwner");
                arrlstsettlementModel.Add("cardYm");
                arrlstsettlementModel.Add("cardPayType");
                arrlstsettlementModel.Add("cardInstallmentDesc");

                arrlstpointModel = new ArrayList();
                arrlstpointModel.Add("usedPoint");

                arrlstdeliveryModel = new ArrayList();
                arrlstdeliveryModel.Add("deliveryName");
                arrlstdeliveryModel.Add("deliveryClass");

                arrlstwrapModel1 = new ArrayList();
                arrlstwrapModel1.Add("wrappingtitle1");
                arrlstwrapModel1.Add("wrappingname1");
                arrlstwrapModel1.Add("wrappingprice1");
                arrlstwrapModel1.Add("wrappingincludeTaxFlag1");
                arrlstwrapModel1.Add("wrappingdeleteWrappingFlag1");

                arrlstwrapModel2 = new ArrayList();
                arrlstwrapModel2.Add("wrappingtitle2");
                arrlstwrapModel2.Add("wrappingname2");
                arrlstwrapModel2.Add("wrappingprice2");
                arrlstwrapModel2.Add("wrappingincludeTaxFlag2");
                arrlstwrapModel2.Add("wrappingdeleteWrappingFlag2");

                dtJuChuuList = new DataTable();
                dtJuChuuList.Columns.Add("InportSEQ", typeof(string));
                dtJuChuuList.Columns.Add("StoreCD", typeof(string));
                dtJuChuuList.Columns.Add("APIKey", typeof(string));
                dtJuChuuList.Columns.Add("InportSEQRows", typeof(string));
                foreach (string colname in arrlstJuChu)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstOrderModel)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstsettlementModel)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstpointModel)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstdeliveryModel)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstwrapModel1)
                    dtJuChuuList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstwrapModel2)
                    dtJuChuuList.Columns.Add(colname, typeof(string));


            }

            else if (tableType == "Shipping")
            {
                arrlstPackageModel = new ArrayList();
                arrlstPackageModel.Add("basketId");
                arrlstPackageModel.Add("postagePrice");
                arrlstPackageModel.Add("deliveryPrice");
                arrlstPackageModel.Add("goodsTax");
                arrlstPackageModel.Add("goodsPrice");
                arrlstPackageModel.Add("totalPrice");
                arrlstPackageModel.Add("noshi");
                arrlstPackageModel.Add("packageDeleteFlag");

                arrlstSenderModel = new ArrayList();
                arrlstSenderModel.Add("zipCode1");
                arrlstSenderModel.Add("zipCode2");
                arrlstSenderModel.Add("prefecture");
                arrlstSenderModel.Add("city");
                arrlstSenderModel.Add("subAddress");
                arrlstSenderModel.Add("familyName");
                arrlstSenderModel.Add("firstName");
                arrlstSenderModel.Add("familyNameKana");
                arrlstSenderModel.Add("firstNameKana");
                arrlstSenderModel.Add("phoneNumber1");
                arrlstSenderModel.Add("phoneNumber2");
                arrlstSenderModel.Add("phoneNumber3");
                arrlstSenderModel.Add("isolatedIslandFlag");

                arrlstDeliveryCvsModel = new ArrayList();
                arrlstDeliveryCvsModel.Add("cvsCode");
                arrlstDeliveryCvsModel.Add("cvsstoreGenreCode");
                arrlstDeliveryCvsModel.Add("cvsstoreCode");
                arrlstDeliveryCvsModel.Add("cvsstoreName");
                arrlstDeliveryCvsModel.Add("cvsstoreZip");
                arrlstDeliveryCvsModel.Add("cvsstorePrefecture");
                arrlstDeliveryCvsModel.Add("cvsstoreAddress");
                arrlstDeliveryCvsModel.Add("cvsareaCode");
                arrlstDeliveryCvsModel.Add("cvsdepo");
                arrlstDeliveryCvsModel.Add("cvsopenTime");
                arrlstDeliveryCvsModel.Add("cvscloseTime");
                arrlstDeliveryCvsModel.Add("cvsRemarks");

                dtShippingList = new DataTable();
                dtShippingList.Columns.Add("InportSEQ", typeof(string));
                dtShippingList.Columns.Add("StoreCD", typeof(string));
                dtShippingList.Columns.Add("APIKey", typeof(string));
                dtShippingList.Columns.Add("InportSEQRows", typeof(string));
                dtShippingList.Columns.Add("orderNumber", typeof(string));
                dtShippingList.Columns.Add("basketRows", typeof(string));
                foreach (string colname in arrlstPackageModel)
                    dtShippingList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstSenderModel)
                    dtShippingList.Columns.Add(colname, typeof(string));
                foreach (string colname in arrlstDeliveryCvsModel)
                    dtShippingList.Columns.Add(colname, typeof(string));

            }

            else if (tableType == "Coupon")
            {
                arrlstCouponModelList = new ArrayList();
                arrlstCouponModelList.Add("couponCode");
                arrlstCouponModelList.Add("itemId");
                arrlstCouponModelList.Add("couponName");
                arrlstCouponModelList.Add("couponSummary");
                arrlstCouponModelList.Add("couponCapital");
                arrlstCouponModelList.Add("expiryDate");
                arrlstCouponModelList.Add("couponPrice");
                arrlstCouponModelList.Add("couponUnit");
                arrlstCouponModelList.Add("couponTotalPrice");

                dtCouponList = new DataTable();
                dtCouponList.Columns.Add("InportSEQ", typeof(string));
                dtCouponList.Columns.Add("StoreCD", typeof(string));
                dtCouponList.Columns.Add("APIKey", typeof(string));
                dtCouponList.Columns.Add("InportSEQRows", typeof(string));
                dtCouponList.Columns.Add("orderNumber", typeof(string));
                dtCouponList.Columns.Add("couponRows", typeof(string));

                foreach (string colname in arrlstCouponModelList)
                    dtCouponList.Columns.Add(colname, typeof(string));
            }

            else if (tableType == "ChangeReason")
            {
                arrlstChangeReason = new ArrayList();
                arrlstChangeReason.Add("changeId");
                arrlstChangeReason.Add("changeType");
                arrlstChangeReason.Add("changeTypeDetail");
                arrlstChangeReason.Add("changeReason");
                arrlstChangeReason.Add("changeReasonDetail");
                arrlstChangeReason.Add("changeApplyDatetime");
                arrlstChangeReason.Add("changeFixDatetime");
                arrlstChangeReason.Add("changeCmplDatetime");

                dtChangeReason = new DataTable();
                dtChangeReason.Columns.Add("InportSEQ", typeof(string));
                dtChangeReason.Columns.Add("StoreCD", typeof(string));
                dtChangeReason.Columns.Add("APIKey", typeof(string));
                dtChangeReason.Columns.Add("InportSEQRows", typeof(string));
                dtChangeReason.Columns.Add("orderNumber", typeof(string));
                dtChangeReason.Columns.Add("reasonRows", typeof(string));

                foreach (string colname in arrlstChangeReason)
                    dtChangeReason.Columns.Add(colname, typeof(string));

            }

            else if (tableType == "JuChuuDetail")
            {
                arrlstJuChuuDetail = new ArrayList();
                arrlstJuChuuDetail.Add("itemDetailId");
                arrlstJuChuuDetail.Add("itemName");
                arrlstJuChuuDetail.Add("itemId");
                arrlstJuChuuDetail.Add("itemNumber");
                arrlstJuChuuDetail.Add("manageNumber");
                arrlstJuChuuDetail.Add("price");
                arrlstJuChuuDetail.Add("units");
                arrlstJuChuuDetail.Add("includePostageFlag");
                arrlstJuChuuDetail.Add("includeTaxFlag");
                arrlstJuChuuDetail.Add("includeCashOnDeliveryPostageFlag");
                arrlstJuChuuDetail.Add("selectedChoice");
                arrlstJuChuuDetail.Add("pointRate");
                arrlstJuChuuDetail.Add("pointType");
                arrlstJuChuuDetail.Add("inventoryType");
                arrlstJuChuuDetail.Add("delvdateInfo");
                arrlstJuChuuDetail.Add("restoreInventoryFlag");
                arrlstJuChuuDetail.Add("dealFlag");
                arrlstJuChuuDetail.Add("drugFlag");
                arrlstJuChuuDetail.Add("deleteItemFlag");

                dtJuChuuDetail = new DataTable();
                dtJuChuuDetail.Columns.Add("InportSEQ", typeof(string));
                dtJuChuuDetail.Columns.Add("StoreCD", typeof(string));
                dtJuChuuDetail.Columns.Add("APIKey", typeof(string));
                dtJuChuuDetail.Columns.Add("InportSEQRows", typeof(string));
                dtJuChuuDetail.Columns.Add("orderNumber", typeof(string));
                dtJuChuuDetail.Columns.Add("basketRows", typeof(string));
                dtJuChuuDetail.Columns.Add("itemRows", typeof(string));

                foreach (string colname in arrlstJuChuuDetail)
                    dtJuChuuDetail.Columns.Add(colname, typeof(string));

            }

            else if (tableType == "ShippingDetail")
            {
                arrlstShippingDetail = new ArrayList();
                arrlstShippingDetail.Add("shippingDetailId");
                arrlstShippingDetail.Add("shippingNumber");
                arrlstShippingDetail.Add("deliveryCompany");
                arrlstShippingDetail.Add("deliveryCompanyName");
                arrlstShippingDetail.Add("shippingDate");

                dtShippingDetail = new DataTable();
                dtShippingDetail.Columns.Add("InportSEQ", typeof(string));
                dtShippingDetail.Columns.Add("StoreCD", typeof(string));
                dtShippingDetail.Columns.Add("APIKey", typeof(string));
                dtShippingDetail.Columns.Add("InportSEQRows", typeof(string));
                dtShippingDetail.Columns.Add("orderNumber", typeof(string));
                dtShippingDetail.Columns.Add("basketRows", typeof(string));
                dtShippingDetail.Columns.Add("ShippingRows", typeof(string));
                //ses added 2021/07/08
                foreach (string colname in arrlstShippingDetail)
                    dtShippingDetail.Columns.Add(colname, typeof(string));
            }
        }

    }
}

