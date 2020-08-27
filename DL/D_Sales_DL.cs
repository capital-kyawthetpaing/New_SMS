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
    public class D_Sales_DL : Base_DL
    {
        public DataTable D_Sale_DataSelect(D_Sales_Entity dse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.StoreCD } },
               };
            return SelectData(dic, "D_Sales_DataSelect");
        }

        /// <summary>
        /// 売上番号検索よりデータ抽出時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_Sales_SelectAll(D_Sales_Entity dme)
        {
            string sp = "D_Sales_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.SalesDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerName} },
            };


            if (!string.IsNullOrWhiteSpace(dme.CustomerName))
            {
                DataTable dt = SelectData(dic, sp);
                DataRow[] drs = dt.Select(" CustomerName LIKE '%" + dme.CustomerName + "%'");
                DataTable newdt = dt.Clone();

                foreach (var dr in drs)
                {
                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow = newdt.NewRow();
                    newrow.ItemArray = dr.ItemArray;
                    newdt.Rows.Add(newrow);
                }
                return newdt;
            }
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗納品書よりデータ抽出時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_Sales_SelectForPrint(D_Sales_Entity dme)
        {
            string sp = "D_Sales_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesNO", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dme.SalesNO } },
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.SalesDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerName} },
                { "@PrintFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dme.PrintFLG } },
            };


            if (!string.IsNullOrWhiteSpace(dme.CustomerName))
            {
                DataTable dt = SelectData(dic, sp);
                DataRow[] drs = dt.Select(" CustomerName LIKE '%" + dme.CustomerName + "%'");
                DataTable newdt = dt.Clone();

                foreach (var dr in drs)
                {
                    if (dme.PrintFLG == "1")
                    {
                        if (string.IsNullOrWhiteSpace(dr["PrintDateTime"].ToString()))
                            continue;
                    }
                    else if (dme.PrintFLG == "0")
                    {
                        if (!string.IsNullOrWhiteSpace(dr["PrintDateTime"].ToString()))
                            continue;
                    }

                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow = newdt.NewRow();
                    newrow.ItemArray = dr.ItemArray;
                    newdt.Rows.Add(newrow);
                }
                return newdt;
            }
            else if (dme.PrintFLG == "1")
            {
                DataTable dt1 = SelectData(dic, sp);
                DataRow[] drs1 = dt1.Select(" PrintDateTime IS NOT NULL");
                DataTable newdt1 = dt1.Clone();

                foreach (var dr in drs1)
                {
                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow1 = newdt1.NewRow();
                    newrow1.ItemArray = dr.ItemArray;
                    newdt1.Rows.Add(newrow1);
                }
                return newdt1;
            }
            else if (dme.PrintFLG == "0")
            {
                DataTable dt1 = SelectData(dic, sp);
                DataRow[] drs1 = dt1.Select(" PrintDateTime IS NULL");
                DataTable newdt1 = dt1.Clone();

                foreach (var dr in drs1)
                {
                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow1 = newdt1.NewRow();
                    newrow1.ItemArray = dr.ItemArray;
                    newdt1.Rows.Add(newrow1);
                }
                return newdt1;

            }

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 売上データ取得処理
        /// 納品書で使用するためのストアドとして作成
        /// </summary>
        public DataTable D_Sales_SelectData(D_Sales_Entity mie, short operationMode)
        {
            string sp = "D_Sales_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.SalesNO } },
            };

            return SelectData(dic, sp);
        }

        public bool D_Sales_Update(D_Sales_Entity dse, DataTable dt, string operatorNm, string pc)
        {
            string sp = "D_Sales_Update";

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
        /// <summary>
        /// 店舗レジ販売履歴照会データ取得処理
        /// TempoRegiHanbaiRirekiよりデータ抽出時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Sales_SelectData_ForTempoRegiHanbaiRireki(D_Sales_Entity de)
        {
            string sp = "D_Sales_SelectData_ForTempoRegiHanbaiRireki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗レジ実績照会データ取得処理
        /// TempoRegiJissekiSyoukaiよりデータ抽出時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Sales_SelectData_ForTempoRegiJissekiSyoukai(D_Sales_Entity ds)
        {
            string sp = "D_Sales_SelectData_ForTempoRegiJisseki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = ds.SalesDate } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 売上データ取得処理
        /// 店舗レジ　販売登録で使用するためのストアドとして作成
        /// </summary>
        public DataTable D_Sales_SelectForRegi(D_Sales_Entity mie, short operationMode)
        {
            string sp = "D_Sales_SelectForRegi";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.SalesNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷売上データ更新処理
        /// </summary>
        /// <param name="dse"></param>
        /// <returns></returns>
        public bool ShukkaUriageUpdate(D_Sales_Entity dse)
        {
            string sp = "PRC_ShukkaUriageUpdate";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.PC} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        public DataTable D_Sale_SelectForSeisan(D_Sales_Entity dse)
        {
            string sp = "D_Sale_SelectForSeisan";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.StoreCD} },
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = dse.ChangeDate} },
            };
            UseTransaction = true;
            return SelectData(dic, sp);
        }
        /// <summary>
        /// 売上データ取得処理
        /// </summary>
        public DataTable D_Sales_SelectDataForUriageNyuuryoku(D_Sales_Entity mie, short operationMode, short tennic = 0)
        {
            string sp = "D_Sales_SelectDataForUriageNyuuryoku";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.SalesNO } },
                { "@Tennic", new ValuePair { value1 = SqlDbType.TinyInt, value2 = tennic.ToString() } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 売上入力更新処理
        /// UriageNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Sales_Exec(D_Sales_Entity de, DataTable dt, short operationMode)
        {
            string sp = "PRC_UriageNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@SalesNO", SqlDbType.VarChar, de.SalesNO);
            AddParam(command, "@PurchaseNO", SqlDbType.VarChar, de.PurchaseNO);
            AddParam(command, "@BillingNO", SqlDbType.VarChar, de.BillingNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, de.StoreCD);
            AddParam(command, "@SalesDate", SqlDbType.VarChar, de.SalesDate);
            AddParam(command, "@BillingType", SqlDbType.TinyInt, de.BillingType);
            AddParam(command, "@ReturnFLG", SqlDbType.TinyInt, de.ReturnFlg);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, de.StaffCD);
            AddParam(command, "@CustomerCD", SqlDbType.VarChar, de.CustomerCD);
            AddParam(command, "@CustomerName", SqlDbType.VarChar, de.CustomerName);
            AddParam(command, "@CustomerName2", SqlDbType.VarChar, de.CustomerName2);
            AddParam(command, "@BillingCD", SqlDbType.VarChar, de.BillingCD);
            AddParam(command, "@CollectPlanDate", SqlDbType.VarChar, de.CollectPlanDate);
            AddParam(command, "@PaymentPlanDate", SqlDbType.VarChar, de.PaymentPlanDate);

            AddParam(command, "@SalesGaku", SqlDbType.Money, de.SalesGaku);
            //AddParam(command, "@Discount", SqlDbType.Money, de.Discount);
            AddParam(command, "@SalesHontaiGaku", SqlDbType.Money, de.SalesHontaiGaku);
            //AddParam(command, "@SalesHontaiGaku0", SqlDbType.Money, de.SalesHontaiGaku0);
            //AddParam(command, "@SalesHontaiGaku8", SqlDbType.Money, de.SalesHontaiGaku8);
            //AddParam(command, "@SalesHontaiGaku10", SqlDbType.Money, de.SalesHontaiGaku10);
            AddParam(command, "@SalesTax8", SqlDbType.Money, de.SalesTax8);
            AddParam(command, "@SalesTax10", SqlDbType.Money, de.SalesTax10);
            AddParam(command, "@CostGaku", SqlDbType.Money, de.CostGaku);
            AddParam(command, "@ProfitGaku", SqlDbType.Money, de.ProfitGaku);

            //AddParam(command, "@OrderHontaiGaku", SqlDbType.Money, dme.OrderHontaiGaku);
            //AddParam(command, "@OrderTax8", SqlDbType.Money, dme.OrderTax8);
            //AddParam(command, "@OrderTax10", SqlDbType.Money, dme.OrderTax10);
            //AddParam(command, "@OrderGaku", SqlDbType.Money, dme.OrderGaku);
            AddParam(command, "@PaymentMethodCD", SqlDbType.VarChar, de.PaymentMethodCD);
            AddParam(command, "@NouhinsyoComment", SqlDbType.VarChar, de.NouhinsyoComment);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, de.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutSalesNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 500);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                de.SalesNO = outPutParam;

            return ret;
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に入金消込済みの場合、エラー
        /// </summary>
        /// <param name="salesNo"></param>
        /// <returns></returns>
        public DataTable CheckSalesData(D_Sales_Entity dse)
        {
            string sp = "CheckSalesData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.SalesNO } },
                { "@PurchaseNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.PurchaseNO } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.StoreCD} },
            };

            return SelectData(dic, sp);
        }
    }
}
