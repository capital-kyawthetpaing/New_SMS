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
    public class D_Mitsumori_DL : Base_DL
    {
        public DataTable D_Mitsumori_SelectAll(D_Mitsumori_Entity dme)
        {
            string sp = "D_Mitsumori_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MitsumoriDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriDateFrom } },
                { "@MitsumoriDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriDateTo } },
                { "@MitsumoriInputDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateFrom } },
                { "@MitsumoriInputDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerName} },
                { "@MitsumoriName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriName } },
                { "@JuchuuChanceKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.JuchuuChanceKBN } },
                { "@JuchuuFLG1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dme.JuchuuFLG1 } },
                { "@JuchuuFLG2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dme.JuchuuFLG2 } },
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
        /// 見積書よりデータ抽出時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <returns></returns>
        public DataTable D_Mitsumori_SelectForPrint(D_Mitsumori_Entity dme)
        {
            string sp = "D_Mitsumori_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MitsumoriNO", new ValuePair { value1 =  SqlDbType.VarChar, value2 = dme.MitsumoriNO } },
                { "@MitsumoriDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriDateFrom } },
                { "@MitsumoriDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriDateTo } },
                { "@MitsumoriInputDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateFrom } },
                { "@MitsumoriInputDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerName} },
                { "@MitsumoriName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriName } },
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
        /// 見積入力更新処理
        /// MitsumoriNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Mitsumori_Exec(D_Mitsumori_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc )
        {
            string sp = "PRC_MitsumoriNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command,"@MitsumoriNO", SqlDbType.VarChar, dme.MitsumoriNO);
            AddParam(command,"@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command,"@MitsumoriDate", SqlDbType.VarChar, dme.MitsumoriDate);
            AddParam(command,"@StaffCD", SqlDbType.VarChar, dme.StaffCD);
            AddParam(command,"@CustomerCD", SqlDbType.VarChar, dme.CustomerCD);
            AddParam(command,"@CustomerName", SqlDbType.VarChar, dme.CustomerName);
            AddParam(command,"@CustomerName2", SqlDbType.VarChar, dme.CustomerName2 );
            AddParam(command,"@AliasKBN", SqlDbType.TinyInt, dme.AliasKBN);
            AddParam(command,"@ZipCD1", SqlDbType.VarChar, dme.ZipCD1 );
            AddParam(command,"@ZipCD2", SqlDbType.VarChar, dme.ZipCD2 );
            AddParam(command,"@Address1", SqlDbType.VarChar, dme.Address1);
            AddParam(command,"@Address2", SqlDbType.VarChar, dme.Address2);
            AddParam(command,"@Tel11", SqlDbType.VarChar, dme.Tel11);
            AddParam(command,"@Tel12", SqlDbType.VarChar, dme.Tel12);
            AddParam(command,"@Tel13", SqlDbType.VarChar, dme.Tel13);
            AddParam(command,"@Tel21", SqlDbType.VarChar, dme.Tel21);
            AddParam(command,"@Tel22", SqlDbType.VarChar, dme.Tel22);
            AddParam(command,"@Tel23", SqlDbType.VarChar, dme.Tel23);
            AddParam(command,"@JuchuuChanceKBN", SqlDbType.VarChar, dme.JuchuuChanceKBN);
            AddParam(command,"@MitsumoriName", SqlDbType.VarChar, dme.MitsumoriName);
            AddParam(command,"@DeliveryDate", SqlDbType.VarChar, dme.DeliveryDate);
            AddParam(command,"@PaymentTerms", SqlDbType.VarChar, dme.PaymentTerms);
            AddParam(command,"@DeliveryPlace", SqlDbType.VarChar, dme.DeliveryPlace);
            AddParam(command,"@ValidityPeriod", SqlDbType.VarChar, dme.ValidityPeriod);
            AddParam(command,"@MitsumoriHontaiGaku", SqlDbType.Money, dme.MitsumoriHontaiGaku);
            AddParam(command,"@MitsumoriTax8", SqlDbType.Money, dme.MitsumoriTax8);
            AddParam(command,"@MitsumoriTax10", SqlDbType.Money, dme.MitsumoriTax10);
            AddParam(command,"@MitsumoriGaku", SqlDbType.Money, dme.MitsumoriGaku);
            AddParam(command,"@CostGaku", SqlDbType.Money, dme.CostGaku);
            AddParam(command,"@ProfitGaku", SqlDbType.Money, dme.ProfitGaku);
            AddParam(command,"@RemarksInStore", SqlDbType.VarChar, dme.RemarksInStore);
            AddParam(command,"@RemarksOutStore", SqlDbType.VarChar, dme.RemarksOutStore);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command,"@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command,"@PC", SqlDbType.VarChar, pc);

            //OUTパラメータの追加
            string outPutParam = "@OutMitsumoriNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.MitsumoriNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 見積入力データ取得処理
        /// MitsumoriNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Mitsumori_SelectData(D_Mitsumori_Entity mie, short operationMode)
        {
            string sp = "D_Mitsumori_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@MitsumoriNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.MitsumoriNO } },
            };
            
            return SelectData(dic, sp);
                    }

        public bool D_Mitsumori_Update(D_Mitsumori_Entity dme, DataTable dt, string operatorNm, string pc)
        {
            string sp = "D_Mitsumori_Update";

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

    }
}
