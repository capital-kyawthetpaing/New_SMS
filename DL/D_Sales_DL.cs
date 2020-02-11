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
        /// 売上入力データ取得処理
        /// UriageNyuuryokuよりデータ抽出時に使用
        /// 売上入力未作成のため納品書で使用するためのストアドとして作成
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

    }
}
