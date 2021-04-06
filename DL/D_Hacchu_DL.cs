using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class D_Hacchu_DL : Base_DL
    {
        public DataTable D_Order_Select(D_Order_Entity ode)
        {
            string sp = "D_Order_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = ode.OrderNO } },
                { "@OrderRows", new ValuePair { value1 = SqlDbType.Int, value2 = ode.OrderRows } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = ode.OrderCD } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_Order_SelectAll(D_Order_Entity doe, M_SKU_Entity mse)
        {
            string sp = "D_Order_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StaffCD } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerItem } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Order_SelectAllForShoukai(D_Order_Entity doe, M_SKU_Entity mse, string operatorNm, string pc)
        {
            string sp = "D_Order_SelectAllForShoukai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },
                { "@ArrivalPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateFrom } },
                { "@ArrivalPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateTo } },
                { "@ArrivalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalDateFrom } },
                { "@ArrivalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalDateTo } },
                { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PurchaseDateFrom } },
                { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PurchaseDateTo } },

                { "@ChkMikakutei", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMikakutei.ToString() } },
                { "@ArrivalPlan", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanCD } },
                { "@ChkKanbai", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkKanbai.ToString() } },
                { "@ChkFuyo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkFuyo.ToString() } },
                { "@ChkNyukaZumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkNyukaZumi.ToString() } },
                { "@ChkMiNyuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMiNyuka.ToString() } },
                { "@ChkJuchuAri", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkJuchuAri.ToString() } },
                { "@ChkZaiko", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkZaiko.ToString() } },
                { "@ChkMisyonin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMisyonin.ToString() } },
                { "@ChkSyoninzumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkSyoninzumi.ToString() } },
                { "@ChkChokuso", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkChokuso.ToString() } },
                { "@ChkSouko", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkSouko.ToString() } },
                { "@ChkNet", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkNet.ToString() } },
                { "@ChkFax", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkFax.ToString() } },
                { "@ChkEdi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkEdi.ToString() } },

                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                //{ "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StaffCD } },
                { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerItem } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },

                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.JuchuuNO } },
                { "@DestinationSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.DestinationSoukoCD } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorNm} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 発注承認入力データ取得処理
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_Order_SelectAllForSyonin(D_Order_Entity doe)
        {
            string sp = "D_Order_SelectAllForSyonin";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                { "@ApprovalStageFLG", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ApprovalStageFLG } },
                { "@Misyonin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.Misyonin.ToString() } },
                { "@SyoninZumi", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.SyoninZumi.ToString() } },
                { "@Kyakka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.Kyakka.ToString() } },
        };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 発注入力更新処理
        /// HacchuuNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Order_Exec(D_Order_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc )
        {
            string sp = "PRC_HacchuuNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command, "@OrderNO", SqlDbType.VarChar, dme.OrderNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command, "@OrderDate", SqlDbType.VarChar, dme.OrderDate);
            AddParam(command, "@ReturnFLG", SqlDbType.TinyInt, dme.ReturnFLG);
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, dme.DestinationSoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dme.StaffCD);
            AddParam(command, "@OrderCD", SqlDbType.VarChar, dme.OrderCD);
            AddParam(command, "@OrderPerson", SqlDbType.VarChar, dme.OrderPerson);
            AddParam(command, "@AliasKBN", SqlDbType.TinyInt, dme.AliasKBN);
            AddParam(command, "@DestinationKBN", SqlDbType.TinyInt, dme.DestinationKBN);
            AddParam(command, "@DestinationName", SqlDbType.VarChar, dme.DestinationName);
            AddParam(command, "@ZipCD1", SqlDbType.VarChar, dme.DestinationZip1CD);
            AddParam(command, "@ZipCD2", SqlDbType.VarChar, dme.DestinationZip2CD);
            AddParam(command, "@Address1", SqlDbType.VarChar, dme.DestinationAddress1);
            AddParam(command, "@Address2", SqlDbType.VarChar, dme.DestinationAddress2);
            AddParam(command, "@DestinationTelphoneNO", SqlDbType.VarChar, dme.DestinationTelphoneNO);
            AddParam(command, "@DestinationFaxNO", SqlDbType.VarChar, dme.DestinationFaxNO);
            AddParam(command, "@DestinationSoukoCD", SqlDbType.VarChar, dme.DestinationSoukoCD);

            AddParam(command, "@OrderHontaiGaku", SqlDbType.Money, dme.OrderHontaiGaku);
            AddParam(command, "@OrderTax8", SqlDbType.Money, dme.OrderTax8);
            AddParam(command, "@OrderTax10", SqlDbType.Money, dme.OrderTax10);
            AddParam(command, "@OrderGaku", SqlDbType.Money, dme.OrderGaku);
            AddParam(command, "@CommentOutStore", SqlDbType.VarChar, dme.CommentOutStore);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, dme.CommentInStore);
            AddParam(command, "@ArrivalPlanDate", SqlDbType.VarChar, dme.ArrivalPlanDate);
            AddParam(command, "@ApprovalEnabled", SqlDbType.TinyInt, dme.ApprovalEnabled);
            AddParam(command, "@ApprovalStageFLG", SqlDbType.Int, dme.ApprovalStageFLG);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command,"@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command,"@PC", SqlDbType.VarChar, pc);

            //OUTパラメータの追加
            string outPutParam = "@OutOrderNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.OrderNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 発注入力データ取得処理
        /// HacchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Order_SelectData(D_Order_Entity de, short operationMode)
        {
            string sp = "D_Order_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.OrderNO } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み警告
        /// </summary>
        /// <param name="hacchuNo"></param>
        /// <returns></returns>
        public DataTable CheckHacchuData(string hacchuNo)
        {
            string sp = "CheckHacchuData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = hacchuNo } },
            };

            return SelectData(dic, sp);
        }
        public DataTable CheckSyonin(string hacchuNo, string InOperatorCD)
        {
            string sp = "CheckSyonin";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = hacchuNo } },
                { "@Operator",new ValuePair { value1 = SqlDbType.VarChar, value2 = InOperatorCD } },
            };

            return SelectData(dic, sp);
        }
        public bool D_Hacchu_Update(D_Mitsumori_Entity dme, DataTable dt, string operatorNm, string pc)
        {
            string sp = "D_Hacchu_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command, "@PC", SqlDbType.VarChar, pc);

            UseTransaction = true;

            string outPutParam = "";    //未使用
           bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
        public DataTable D_Order_SelectDataForKaitouNouki(D_Order_Entity doe)
        {
            string sp = "D_Order_SelectDataForKaitouNouki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateFrom } },
                { "@ArrivalPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateTo } },
                { "@ArrivalPlanMonthFrom", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanMonthFrom } },
                { "@ArrivalPlanMonthTo", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanMonthTo } },
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },

                { "@OrderNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderNoFrom } },
                { "@OrderNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderNoTo } },
                { "@EDIDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.EDIDate } },

                { "@ChkMikakutei", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMikakutei.ToString() } },
                { "@ArrivalPlan", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanCD } },
                { "@ChkKanbai", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkKanbai.ToString() } },
                { "@ChkFuyo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkFuyo.ToString() } },

                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },

            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 回答納期登録更新処理
        /// KaitouNoukiTourokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Order_ExecForKaitouNouki(D_Order_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            string sp = "PRC_KaitouNoukiTouroku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command, "@OrderCD", SqlDbType.VarChar, dme.OrderCD);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command, "@PC", SqlDbType.VarChar, pc);

            ////OUTパラメータの追加
            //string outPutParam = "@OutOrderNo";
            //command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            //command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            //if (ret)
            //    dme.OrderNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 回答納期確認書よりデータ取得
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_ArrivalPlan_SelectForPrint(D_Order_Entity doe)
        {
            string sp = "D_ArrivalPlan_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ArrivalPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateFrom } },
                { "@ArrivalPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ArrivalPlanDateTo } },
                { "@ArrivalPlanMonthFrom", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanMonthFrom } },
                { "@ArrivalPlanMonthTo", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanMonthTo } },
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },

                { "@ChkMikakutei", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMikakutei.ToString() } },
                { "@ArrivalPlan", new ValuePair { value1 = SqlDbType.Int, value2 = doe.ArrivalPlanCD } },
                { "@ChkKanbai", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkKanbai.ToString() } },
                { "@ChkFuyo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkFuyo.ToString() } },

                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },

            };

            return SelectData(dic, sp);
        }

        public DataTable D_Order_Select(string orderNo)
        {
            string sp = "D_Order_SelectForEDIHacchuu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNo } },
                };

            return SelectData(dic, sp);
        }

        public DataTable D_Order_SelectAllForEDIHacchuu(D_Order_Entity doe)
        {
            string sp = "D_Order_SelectAllForEDIHacchuu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                    { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                    { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StaffCD } },
                    { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                    { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderNO } },
                    { "@ChkMisyonin", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ChkMisyonin.ToString() } },
                };

            return SelectData(dic, sp);
        }

        public bool D_Order_ExecForNyuka(D_Order_Entity de)
        {
            string sp = "D_Order_ExecForNyuka";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@StoreCD", SqlDbType.VarChar, de.StoreCD);
            AddParam(command, "@ChangeDate", SqlDbType.Date, de.ChangeDate);    //ArrivalDate
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, de.DestinationSoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, de.StaffCD);
            AddParam(command, "@OrderWayKBN", SqlDbType.TinyInt, de.OrderWayKBN);
            AddParam(command, "@OrderCD", SqlDbType.VarChar, de.OrderCD);
            AddParam(command, "@OrderPerson", SqlDbType.VarChar, de.OrderPerson);
            AddParam(command, "@AliasKBN", SqlDbType.TinyInt, de.AliasKBN);
            AddParam(command, "@AdminNO", SqlDbType.Int, de.AdminNO);
            AddParam(command, "@SKUCD", SqlDbType.VarChar, de.SKUCD);
            AddParam(command, "@SKUName", SqlDbType.VarChar, de.SKUName);
            AddParam(command, "@JANCD", SqlDbType.VarChar, de.JANCD);
            AddParam(command, "@MakerItem", SqlDbType.VarChar, de.MakerItem);
            AddParam(command, "@ColorName", SqlDbType.VarChar, de.ColorName);
            AddParam(command, "@SizeName", SqlDbType.VarChar, de.SizeName);
            AddParam(command, "@OrderSuu", SqlDbType.Int, de.OrderSuu);
            AddParam(command, "@OrderUnitPrice", SqlDbType.Money, de.OrderUnitPrice);
            AddParam(command, "@TaniCD", SqlDbType.VarChar, de.TaniCD);
            AddParam(command, "@PriceOutTax", SqlDbType.Money, de.PriceOutTax);
            AddParam(command, "@Rate", SqlDbType.Decimal, de.Rate);
            AddParam(command, "@OrderHontaiGaku", SqlDbType.Money, de.OrderHontaiGaku);
            AddParam(command, "@OrderTax", SqlDbType.Money, de.OrderTax);
            AddParam(command, "@OrderTaxRitsu", SqlDbType.Int, de.OrderTaxRitsu);
            AddParam(command, "@OriginalArrivalPlanNO", SqlDbType.VarChar, de.OriginalArrivalPlanNO);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.Operator);

            //OUTパラメータの追加
            string outPutParam = "@OutOrderNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;
            
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                de.OrderNO = outPutParam;

            return ret;
        }
    }
}
