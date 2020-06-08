using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using Ponpare_API.OrderWsvc;
using System.Data;
namespace Ponpare_API
{
    public class CommonAPI
    {
        DataTable RirekiDetail = new DataTable();
        DataTable Juchuu = new DataTable();
        DataTable JuchuuDetails = new DataTable();
        DataTable Coupon = new DataTable();
        DataTable Enclose = new DataTable();
        _PonpareEntity pet = new _PonpareEntity();
        _Ponpare_BL pbl = new _Ponpare_BL();
        string ShopURL = "";
        string API_Key = "";
        string Key = "";
        string Password = "";
        string StoreCD = "";
        string fromDate = "";
        string toDate = "";
        public void GetOrder()
        {
            var dt = pbl.Ponpare_MAPI_Select();
            if (dt.Rows.Count > 0)
            {
                generateTabel();
                StoreCD = "0001";
                API_Key = "12";
                ShopURL = dt.Rows[0]["PonpareShopUrl"].ToString();
                Key = dt.Rows[0]["UserId"].ToString();
                Password = dt.Rows[0]["Password"].ToString();
                getOrderNo();
                
            }
        }


        protected void generateTabel()
        {
            RirekiDetail.Columns.Add("StoreCD");
            RirekiDetail.Columns.Add("APIKey");
            RirekiDetail.Columns.Add("SEQ");
            RirekiDetail.Columns.Add("OrderId");

            string[] cols = new string[] {
                "StoreCD",
                "APIKey",
                "InportSEQRows",
                "orderNo",
                "orderDateTime",
                "orderSts",
                "pymntSts",
                "depositDate",
                "sendDate",
                "dlvKind",
                "dlvDesiredDate",
                "dlvDesiredTimeZoneKind",
                "dlvDesiredTimeZoneFrom",
                "dlvDesiredTimeZoneTo",
                "shopUserNameInCharge",
                "orderMemo",
                "messageToCustomer",
                "useTerminal",
                "mailcarrierCode",
                "giftApplyFlg",
                "orderNote",
                "taxRate",
                "dlvAddrCautionFlg",
                "yellowUserFlg",
                "memberKind",
                "enclosableFlg",
                "itemAmount",
                "taxAmount",
                "dlvFee",
                "pymntFee",
                "totalAmount",
                "usePointAmount",
                "useCouponTotalAmount",
                "useCouponShopAmount",
                "useCouponOtherAmount",
                "useCouponTotalCnt",
                "useCouponShopCnt",
                "useCouponOtherCnt",
                "totalPymntAmount",
                "totalPymntAmountInit",
                "customerZip1",
                "customerZip2",
                "customerPref",
                "customerAddress",
                "customerLastName",
                "customerFirstName",
                "customerLastNameKana",
                "customerFirstNameKana",
                "customerTel",
                "customerEmail",
                "pymntMethodId",
                "pymntMethodName",
                "cardBrand",
                "cardNo",
                "cardSignature",
                "cardExpire",
                "cardPymntMethod",
                "dlvMethodId",
                "dlvMethodName",
                "dlvAddrZip1",
                "dlvAddrZip2",
                "dlvAddrPref",
                "dlvAddrAddress",
                "dlvAddrLastName",
                "dlvAddrFirstName",
                "dlvAddrLastNameKana",
                "dlvAddrFirstNameKana",
                "dlvAddrTel",
                "slipNo",
                "noshi",
                "wrappingKind1",
                "wrappingName1",
                "wrappingPrice1",
                "wrappingTaxKind1",
                "wrappingDelFlg1",
                "wrappingKind2",
                "wrappingName2",
                "wrappingPrice2",
                "wrappingTaxKind2",
                "wrappingDelFlg2",
                "encloseKind",
                "encloseOrderNo",
                "encloseItemAmount",
                "encloseTaxAmount",
                "encloseDlvFee",
                "enclosePymntFee",
                "encloseTotalAmount",
                "encloseUsePointAmount",
                "encloseUseCouponAmount",
                "encloseTotalPymntAmount",
                "cardUpdatingIconFlg",
                "cardUpdatedIconFlg",
                "fraudOrderAlert",
                "nxDayDlvFlg"
            };
            foreach (string col in cols)
            {
                Juchuu.Columns.Add(col, typeof(string));
            }
            string[] detailscols = new string[]
                {
                    "StoreCD",
                    "APIKey",
                    "InportSEQRows",
                    "orderNo",
                    "orderRows",
                    "orderItemSubNo",
                    "itemName",
                    "itemId",
                    "itemManageId",
                    "HSkuItemId",
                    "VSkuItemId",
                    "salePrice",
                    "itemCnt",
                    "incShippingFlg",
                    "taxKind",
                    "itemTaxRateKbn",
                    "itemTaxRate",
                    "incCodFeeFlg",
                    "getPointRate",
                    "getPoint",
                    "purchaseOption",
                    "invKind",
                    "itemDelFlg",
                };
            foreach (string col in detailscols)
            {
                JuchuuDetails.Columns.Add(col,typeof(string));
            }
            string[] CouponCols = new string[] {
                "StoreCD",
                "APIKey",
                "InportSEQRows",
                "orderNo",
                "couponRows",
                "couponCode",
                "orderItemSubNo",
                "itemManageId",
                "couponName",
                "couponCnt",
                "couponCapitalKind",
                "discountType",
                "expiryDate",
                "couponAmount",
            };
            foreach (var col in CouponCols)
            {
                Coupon.Columns.Add(col, typeof(string));
            }

            string[] Enclosecols = new string[] {
                "StoreCD",
                "APIKey",
                "InportSEQRows",
                "orderNo",
                "encloseRows",
                "encloseorderNo",

            };
            foreach (var col in Enclosecols)
            {
                Enclose.Columns.Add(col,typeof(string));
            }

        }
        protected void getOrderNo()
        {
            orderWsvcPortTypeClient service = new orderWsvcPortTypeClient("orderWsvcHttpsSoap12Endpoint");
            UserAuthBean userAuthBean = new UserAuthBean();
            userAuthBean.shopUrl = ShopURL;
            userAuthBean.userId = Key;
            userAuthBean.password = Password;
            OrderSearchCondBean orderSearchCond = new OrderSearchCondBean();
            fromDate= orderSearchCond.dateRangeFrom = DateTime.Now.AddDays(-60).ToString("yyyy/MM/dd").Replace("-","/");
            toDate=  orderSearchCond.dateRangeTo = DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/");
            orderSearchCond.dateRangeSearchColumn = "1"; 
            orderSearchCond.pymntMethodId = "4";
            //orderSearchCond.dlvMethodId = "2"; // 
            //orderSearchCond.orderSts = new string[1] { "新規受付" };// Not mention in Doc:
            GetOrderNoWsvcReqBean getOrderInfoWsvcReq = new GetOrderNoWsvcReqBean();
            getOrderInfoWsvcReq.userAuthBean = userAuthBean;
            GetOrderNoReqBean getOrderInfoReq = new GetOrderNoReqBean();
            getOrderInfoReq.orderSearchCond = orderSearchCond;
            getOrderInfoWsvcReq.getOrderNoReq = getOrderInfoReq;

            var OrderNos = service.getOrderNo(getOrderInfoWsvcReq);
            Console.WriteLine("Info Request Successed!!!!");

            
            GetOrderInfoWsvcReqBean getorderinfoWsvcreqbean = new GetOrderInfoWsvcReqBean();
            getorderinfoWsvcreqbean.userAuthBean = userAuthBean;
            GetOrderInfoReqBean getorderinforeqbean = new GetOrderInfoReqBean();
            getorderinforeqbean.orderSearchCond = orderSearchCond;
            getorderinforeqbean.orderNoList = OrderNos.orderNoList;
            getorderinfoWsvcreqbean.getOrderInfoReq = getorderinforeqbean;
            try
            {
                var OrderJuchuu = service.getOrderInfo(getorderinfoWsvcreqbean);

                InsertRirekiDetail(OrderNos);   /// ABCD
                Console.WriteLine("ABCD inserted!!!!!!!");
                InsertJuchuu(OrderJuchuu);           //E
                Console.WriteLine("Juchuu Prepared!");
                InsertJuchuuDetails(OrderJuchuu);    //F
                Console.WriteLine("JuchuuDetails Prepared!");
                InsertCouopon(OrderJuchuu);          //G
                Console.WriteLine("Coupon Prepared!");
                InserPonpareEnclose(OrderJuchuu);    //H
                Console.WriteLine("Enclose Prepared!");
                InsertEFGH();
                Console.WriteLine("EFGH inserted!!!!!");
                Console.Write("Finished!!!!!!");
            }
            catch (Exception ex)
            {

                var f = ex.Message;
            }

        }

        protected void InsertEFGH()
        {
            Base_BL bbl = new Base_BL();

            _PonpareEntity _pet=  GetPet();
            var InsertRireki = pbl.InsertEFGH(_pet);
        }
        protected _PonpareEntity GetPet()
        {
            Base_BL bbl = new Base_BL();
            pet = new _PonpareEntity()
            {
                StoreCD = StoreCD,
                APIKey = API_Key,
                fromDate = fromDate,
                toDate = toDate,
                Juchuu= bbl.DataTableToXml(Juchuu).Replace("]", "}").Replace("[", "{"),
                JuchuuDetails = bbl.DataTableToXml(JuchuuDetails).Replace("]", "}").Replace("[", "{"),
                Coupon= bbl.DataTableToXml(Coupon).Replace("]", "}").Replace("[", "{"),
                Enclose = bbl.DataTableToXml(Enclose).Replace("]", "}").Replace("[", "{"),
            };
            return pet;
        }
        protected void InserPonpareEnclose(GetOrderInfoWsvcRspBean o)
        {
            try
            {
                int k = 0;
                foreach (var olist in o.orderList)
                {
                    if (olist == null)
                    {
                        break;
                    }
                    //if (olist.encloseChildOrderList != null)
                    //{
                        k++;
                        int j = 0;
                    foreach (var elist in olist.encloseChildOrderList)
                    {
                        j++;
                        if (elist != null)
                        {
                            
                            Enclose.Rows.Add(new object[]
                                {
                                    StoreCD,
                                   API_Key,
                                   k.ToString(),
                                   elist.encloseOrderNo,
                                   j.ToString(),
                                   elist.orderNo,
                                });
                        }
                        else
                        {
                            Enclose.Rows.Add(new object[]
                            {
                                    StoreCD,
                                   API_Key,
                                   k.ToString(),
                                   null,
                                   j.ToString(),
                                   null,
                            });
                        }
                    }
                    //}
                }

            }
            catch (Exception ex)
            {
                var smsg = ex.Message;
            }

        }
        protected void InsertCouopon(GetOrderInfoWsvcRspBean o)
        {
            try {
                int k = 0;
                foreach (var olist in o.orderList)
                {
                    if (olist == null)
                    {
                        break;
                    }
                    k++;
                    int j = 0;
                    if (olist.useCouponList != null)
                    {
                        

                        foreach (var clist in olist.useCouponList)
                        {
                            j++;
                            if (clist != null)
                            {

                                Coupon.Rows.Add(new object[] {
                            StoreCD,
                            API_Key,
                            k.ToString(),
                            olist.orderNo,
                            j.ToString(),
                            clist.couponCode,
                            clist.orderItemSubNo == null? null:clist.orderItemSubNo,
                            clist.itemManageId,
                            clist.couponName,
                           clist.couponCnt== null? null: clist.couponCnt,
                             clist.couponCapitalKind ==null?null:clist.couponCapitalKind,
                           clist.discountType == null? null: clist.discountType,
                            clist.expiryDate,
                           IsMoney(clist.couponAmount),
                        });
                            }
                            else
                            {

                                Coupon.Rows.Add(new object[] {
                            StoreCD,
                            API_Key,
                            k.ToString(),
                            olist.orderNo,
                            j.ToString(),
                            null,
                            null,
                            null,
                           null,
                           null,
                            null,
                           null,
                            null,
                           null,
                        });
                            }
                        }
                    }
                    else
                    {
                        j++;
                        Coupon.Rows.Add(new object[] {
                            StoreCD,
                            API_Key,
                            k.ToString(),
                            olist.orderNo,
                            j.ToString(),
                            null,
                            null,
                            null,
                           null,
                           null,
                            null,
                           null,
                            null,
                           null,
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                var mdg = ex.Message;
            }

        }
        protected void InsertJuchuuDetails(GetOrderInfoWsvcRspBean O)
        {
            try
            {
                int j = 0;
              foreach (var olis in O.orderList)
                {
                    if (olis == null)
                    {
                        break;
                    }
                    j++;
                    int k = 0;
                    foreach (var ilist in olis.orderItemList)
                    {
                        k++;
                        if (ilist != null)
                        {
                          JuchuuDetails.Rows.Add(new object[] {
                          StoreCD,
                          API_Key,
                          k.ToString(),
                          olis.orderNo,
                          j.ToString(),
                          ilist.orderItemSubNo == null? null:ilist.orderItemSubNo.ToString(),
                          ilist.itemName,
                          ilist.itemId,
                          ilist.itemManageId,
                          ilist.HSkuItemId,
                          ilist.VSkuItemId,
                          IsMoney(ilist.salePrice),
                          ilist.itemCnt == null ? null: ilist.itemCnt.ToString(),
                          IsTrue(ilist.incShippingFlg),
                          ilist.taxKind,
                          ilist.itemTaxRateKbn,
                          ilist.itemTaxRate,
                          IsTrue(ilist.incCodFeeFlg),
                          ilist.getPointRate== null ? null: ilist.getPointRate.ToString(),
                          ilist.getPoint == null ? null: ilist.getPoint.ToString(),
                          ilist.purchaseOption,
                          ilist.invKind,
                          IsTrue(ilist.itemDelFlg),
                      });
                        }
                        else {
                            JuchuuDetails.Rows.Add(new object[] {
                           StoreCD,
                           API_Key,
                           k.ToString(),
                            olis.orderNo,
                           j.ToString(),
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                          null,
                          null,
                          null,
                          null,
                          null,
                         null,
                          null,
                          null,
                          null,
                          null,
                          null,
                         null,
                      });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msmg = ex.Message;
            }
        }
        protected void InsertJuchuu(GetOrderInfoWsvcRspBean orderinfo)
        {
            try
            {
                int i = 0;
                foreach (OrderBean obean in orderinfo.orderList)
                {
                    if (obean == null)
                    {
                        break;
                    }
                    i++;
                    //     var f = obean.orderPymnt.creditCard.cardBrand;
                    Juchuu.Rows.Add(new object[] {
                        StoreCD,
                        API_Key,
                        i.ToString(),
                        obean.orderNo,
                        obean.orderDateTime,
                        obean.orderSts,
                        obean.pymntSts,
                        obean.depositDate,
                        obean.sendDate,
                        obean.dlvKind,
                        obean.dlvDesiredDate,
                        obean.dlvDesiredTimeZoneKind,
                        obean.dlvDesiredTimeZoneFrom,
                        obean.dlvDesiredTimeZoneTo,
                        obean.shopUserNameInCharge,
                        obean.orderMemo,
                        obean.messageToCustomer,
                        obean.useTerminal,
                        obean.mailcarrierCode,
                        IsTrue(obean.giftApplyFlg),
                        obean.orderNote,
                        obean.taxRate,
                        IsTrue(obean.dlvAddrCautionFlg),
                        IsTrue(obean.yellowUserFlg),
                        obean.memberKind,
                        IsTrue(obean.enclosableFlg),
                        obean.itemAmount,
                        obean.taxAmount,
                        obean.dlvFee,
                        obean.pymntFee,
                        obean.totalAmount,
                        obean.usePointAmount,
                        obean.useCouponTotalAmount,
                        obean.useCouponShopAmount,
                        obean.useCouponOtherAmount,
                        obean.useCouponTotalCnt,
                        obean.useCouponShopCnt,
                        obean.useCouponOtherCnt,
                        IsMoney(obean.totalPymntAmount),
                        IsMoney(obean.totalPymntAmountInit),
                        obean.orderCustomer.customerZip1,
                        obean.orderCustomer.customerZip2,
                        obean.orderCustomer.customerPref,
                        obean.orderCustomer.customerAddress,
                        obean.orderCustomer.customerLastName,
                        obean.orderCustomer.customerFirstName,
                        obean.orderCustomer.customerLastNameKana,
                        obean.orderCustomer.customerFirstNameKana,
                        obean.orderCustomer.customerTel,
                        obean.orderCustomer.customerEmail,
                        obean.orderPymnt.pymntMethodId,
                        obean.orderPymnt.pymntMethodName,
                        IsCreditCard(obean.orderPymnt.creditCard,"1"),
                        IsCreditCard(obean.orderPymnt.creditCard,"2"),
                        IsCreditCard(obean.orderPymnt.creditCard,"3"),
                        IsCreditCard(obean.orderPymnt.creditCard,"4"),
                        IsCreditCard(obean.orderPymnt.creditCard,"5"),
                        obean.orderDlv == null ? null:obean.orderDlv.dlvMethodId,
                        obean.orderDlv == null ? null:obean.orderDlv.dlvMethodName,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrZip1,
                        obean.orderDlvAddr == null? null: obean.orderDlvAddr.dlvAddrZip2,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrPref,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrAddress,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrLastName,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrFirstName,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrLastNameKana,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrFirstNameKana,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.dlvAddrTel,
                        obean.orderDlvAddr == null? null:obean.orderDlvAddr.slipNo,
                        obean.noshi,
                        obean.wrapping1 == null ? null:obean.wrapping1.wrappingKind,
                        obean.wrapping1 == null ? null:obean.wrapping1.wrappingName,
                        IsMoney(obean.wrapping1 == null ? null:obean.wrapping1.wrappingPrice),
                        obean.wrapping1 == null ? null:obean.wrapping1.wrappingTaxKind,
                        IsTrue(obean.wrapping1 == null ? null:obean.wrapping1.wrappingDelFlg),
                        obean.wrapping2 == null ? null:obean.wrapping2.wrappingKind,
                        obean.wrapping2 == null ? null:obean.wrapping2.wrappingName,
                        IsMoney(obean.wrapping2 == null ? null:obean.wrapping2.wrappingPrice),
                        obean.wrapping2 == null ? null:obean.wrapping2.wrappingTaxKind,
                        IsTrue(obean.wrapping2 == null ? null:obean.wrapping2.wrappingDelFlg ),
                        obean.encloseKind,
                        obean.encloseOrderNo,
                        IsMoney(obean.encloseItemAmount),
                        IsMoney(obean.encloseTaxAmount),
                        IsMoney(obean.encloseDlvFee),
                        IsMoney(obean.enclosePymntFee),
                        IsMoney(obean.encloseTotalAmount),
                        IsMoney(obean.encloseUsePointAmount),
                        IsMoney(obean.encloseUseCouponAmount),
                        IsMoney(obean.encloseTotalPymntAmount),
                        IsTrue(obean.cardUpdatingIconFlg),
                        IsTrue(obean.cardUpdatedIconFlg),
                        obean.fraudOrderAlert,
                        IsTrue(obean.nxDayDlvFlg)

                    });
                }
            }
            catch (Exception ex)
            {

                var msg = ex.Message;
                return;
            }

        }
        protected string IsCreditCard(CreditCardBean ccb, string val)
        {
            if (ccb != null)
            {
                if (val == "1")
                {
                    return ccb.cardBrand;
                }
                else if (val == "2")
                {
                    return ccb.cardNo;
                }
                else if (val == "3")
                {
                    return ccb.cardSignature;
                }
                else if (val == "4")
                {
                    return ccb.cardExpire;
                }
                else 
                {
                    return ccb.cardPymntMethod;
                }
            }
            else
            {
                return null;
            }

        }
        protected string IsTrue(bool? flg)
        {
            if (flg == null)
            {
                return null;
            }
            else if(flg == true)
            {
                return "1";
            }
            else
            return "0";

        }
        protected string IsMoney(long? Amt)
        {
            if (Amt == null)
            {
                return null;
            }
            else
                 return Amt.ToString();

            

        }
        protected void InsertRirekiDetail(GetOrderNoWsvcRspBean OrderNos)  //ABCD OrderNos.orderNoList[0]
        {
            int i = 0;
            foreach (string order in OrderNos.orderNoList)
            {
                i++;
                RirekiDetail.Rows.Add(new object[] { StoreCD, API_Key, i.ToString(), order });
            }
            Base_BL bbl = new Base_BL();
            var InsertRireki = pbl.InsertRirekiDetail(StoreCD, API_Key, fromDate,toDate ,bbl.DataTableToXml(RirekiDetail));
        }
        

    }
}
