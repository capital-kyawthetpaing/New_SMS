using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

using System.Data.SqlClient;

namespace DL
{
    public class D_Purchase_DL :Base_DL
    {

        public DataTable D_Purchase_SelectAll(D_Purchase_Entity dpe, M_SKU_Entity mse)
        {
            string sp = "D_Purchase_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PurchaseDateFrom } },
                { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PurchaseDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.StaffCD } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.VendorCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },
                { "@MakerItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.MakerItem } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Purchase_Search(D_Purchase_Entity dp_e)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                   { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.VendorCD  } },
                   { "@DeliveryNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.DeliveryNo  } },
                   { "@ArrivalDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.ArrivalDateFrom  } },
                   { "@ArrivalDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.ArrivalDateTo  } },
                   { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.PurchaseDateFrom } },
                   { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.PurchaseDateTo } },
                   { "@PaymentDueDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.PaymentDueDateFrom  } },
                   { "@PaymentDueDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.PaymentDueDateTo  } },
                   {"@ChkValue",new ValuePair{value1=SqlDbType.VarChar,value2=dp_e.CheckValue } },
                   //{ "@Paid", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dp_e.Paid  } },
                   //{ "@Unpaid", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dp_e.UnPaid  } },
                   { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.StaffCD  } },
                   { "@PayeeFLg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dp_e.PayeeFLg  } },
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dp_e.StoreCD  } },

            };
            return SelectData(dic, "D_Purchase_Select");
        }

        /// <summary>
        /// 仕入入力更新処理(入荷から)
        /// ShiireNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dpe"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Purchase_ExecF(D_Purchase_Entity dpe, DataTable dt, short operationMode)
        {
            string sp = "PRC_ShiireNyuuryokuF";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@PurchaseNO", SqlDbType.VarChar, dpe.PurchaseNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dpe.StoreCD);
            AddParam(command, "@PurchaseDate", SqlDbType.VarChar, dpe.PurchaseDate);
            AddParam(command, "@PaymentPlanDate", SqlDbType.VarChar, dpe.PaymentPlanDate);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dpe.StaffCD);
            AddParam(command, "@VendorCD", SqlDbType.VarChar, dpe.VendorCD);
            AddParam(command, "@CalledVendorCD", SqlDbType.VarChar, dpe.CalledVendorCD);
            AddParam(command, "@PayeeCD", SqlDbType.VarChar, dpe.PayeeCD);
            AddParam(command, "@CalculationGaku", SqlDbType.Money, dpe.CalculationGaku);
            AddParam(command, "@AdjustmentGaku", SqlDbType.Money, dpe.AdjustmentGaku);
            AddParam(command, "@PurchaseGaku", SqlDbType.Money, dpe.PurchaseGaku);
            AddParam(command, "@PurchaseTax", SqlDbType.Money, dpe.PurchaseTax);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, dpe.CommentInStore);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dpe.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dpe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutPurchaseNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dpe.PurchaseNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 仕入入力データ取得処理(入荷から)
        /// HacchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Purchase_SelectDataF(D_Purchase_Entity de, short operationMode)
        {
            string sp = "D_Purchase_SelectDataF";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PurchaseNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 返品入力更新処理
        /// HenpinNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dpe"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Purchase_ExecH(D_Purchase_Entity dpe, DataTable dt, short operationMode)
        {
            string sp = "PRC_HenpinNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@PurchaseNO", SqlDbType.VarChar, dpe.PurchaseNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dpe.StoreCD);
            AddParam(command, "@PurchaseDate", SqlDbType.VarChar, dpe.PurchaseDate);
            AddParam(command, "@PaymentPlanDate", SqlDbType.VarChar, dpe.PaymentPlanDate);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dpe.StaffCD);
            AddParam(command, "@CalledVendorCD", SqlDbType.VarChar, dpe.CalledVendorCD);
            AddParam(command, "@PayeeCD", SqlDbType.VarChar, dpe.PayeeCD);
            AddParam(command, "@CalculationGaku", SqlDbType.Money, dpe.CalculationGaku);
            AddParam(command, "@AdjustmentGaku", SqlDbType.Money, dpe.AdjustmentGaku);
            AddParam(command, "@PurchaseGaku", SqlDbType.Money, dpe.PurchaseGaku);
            AddParam(command, "@PurchaseTax", SqlDbType.Money, dpe.PurchaseTax);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, dpe.CommentInStore);
            AddParam(command, "@ExpectedDateFrom", SqlDbType.VarChar, dpe.ExpectedDateFrom);
            AddParam(command, "@ExpectedDateTo", SqlDbType.VarChar, dpe.ExpectedDateTo);


            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dpe.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dpe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutPurchaseNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dpe.PurchaseNO = outPutParam;

            return ret;
        }
        /// <summary>
        /// 返品入力データ取得処理
        /// HenpinNyuuryokuよりデータ抽出時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public DataTable D_Purchase_SelectDataH(D_Purchase_Entity de, short operationMode)
        {
            string sp = "D_Purchase_SelectDataH";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PurchaseNO } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 仕入入力更新処理(入荷から)
        /// ShiireNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dpe"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        /// <summary>
        /// 仕入入力更新処理（発注・入荷なし）
        /// ShiireNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dpe"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Purchase_Exec(D_Purchase_Entity dpe, DataTable dt, short operationMode)
        {
            string sp = "PRC_ShiireNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@PurchaseNO", SqlDbType.VarChar, dpe.PurchaseNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dpe.StoreCD);
            AddParam(command, "@PurchaseDate", SqlDbType.VarChar, dpe.PurchaseDate);
            AddParam(command, "@PaymentPlanDate", SqlDbType.VarChar, dpe.PaymentPlanDate);
            AddParam(command, "@StockAccountFlg", SqlDbType.TinyInt, dpe.StockAccountFlg);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dpe.StaffCD);
            AddParam(command, "@VendorCD", SqlDbType.VarChar, dpe.VendorCD);
            AddParam(command, "@PayeeCD", SqlDbType.VarChar, dpe.PayeeCD);
            AddParam(command, "@CalculationGaku", SqlDbType.Money, dpe.CalculationGaku);
            AddParam(command, "@AdjustmentGaku", SqlDbType.Money, dpe.AdjustmentGaku);
            AddParam(command, "@PurchaseGaku", SqlDbType.Money, dpe.PurchaseGaku);
            AddParam(command, "@PurchaseTax", SqlDbType.Money, dpe.PurchaseTax);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, dpe.CommentInStore);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dpe.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dpe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutPurchaseNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dpe.PurchaseNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 仕入入力データ取得処理（発注・入荷なし）
        /// HacchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Purchase_SelectData(D_Purchase_Entity de, short operationMode)
        {
            string sp = "D_Purchase_SelectData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PurchaseNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 既に出荷済みチェック
        /// </summary>
        /// <returns></returns>
        public DataTable CheckShippingData(D_Purchase_Entity dpe)
        {
            string sp = "CheckShippingData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpe.PurchaseNO } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 仕入単価訂正依頼書印刷よりデータ取得
        /// </summary>
        /// <param name="dpe"></param>
        /// <returns></returns>
        public DataTable D_Purchase_SelectForPrint(D_Purchase_Entity doe)
        {
            string sp = "D_Purchase_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PurchaseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PurchaseDateFrom } },
                { "@PurchaseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PurchaseDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.VendorCD } },
                { "@Flg1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMisyutsuryoku.ToString() } },
                { "@Flg2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkSyutsuryokuZumi.ToString() } },

            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 仕入単価訂正依頼書印刷時のフラグ更新処理
        /// フラグ更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="dt"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Purchase_Update(D_Purchase_Entity dme, DataTable dt)
        {
            string sp = "D_Purchase_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dme.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dme.PC);

            UseTransaction = true;

            string outPutParam = "";    //未使用

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
    }
}
