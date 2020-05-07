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
    public class HikiateHenkouNyuuryoku_DL : Base_DL
    {
        /// <summary>
        /// ワークテーブル作成用データ取得
        /// </summary>
        /// <param name="hhe"></param>
        /// <returns></returns>
        public DataTable D_Stock_SelectForHikiateZaiko(HikiateHenkouNyuuryoku_Entity hhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                    {"@StoreCD",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.StoreCD} },
                    {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=hhe.AdminNO} },
                    {"@OrderDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderDateFrom} },
                    {"@OrderDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderDateTo} },
                    {"@ArrivalPlanDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalPlanDateFrom} },
                    {"@ArrivalPlanDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalPlanDateTo} },
                    {"@ArrivalDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalDateFrom} },
                    {"@ArrivalDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalDateTo} },
                    {"@OrderCD",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderCD} },
                    {"@OrderNO",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderNO} }
               };
            return SelectData(dic, "D_Stock_SelectForHikiateZaiko");
        }

        public DataTable D_Stock_SelectForHikiateJuchuu(HikiateHenkouNyuuryoku_Entity hhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                    {"@StoreCD",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.StoreCD} },
                    {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=hhe.AdminNO} },
                    {"@OrderDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderDateFrom} },
                    {"@OrderDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderDateTo} },
                    {"@ArrivalPlanDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalPlanDateFrom} },
                    {"@ArrivalPlanDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalPlanDateTo} },
                    {"@ArrivalDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalDateFrom} },
                    {"@ArrivalDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ArrivalDateTo} },
                    {"@OrderCD",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderCD} },
                    {"@OrderNO",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.OrderNO} },
                    {"@JuchuuDateFrom",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuDateFrom} },
                    {"@JuchuuDateTo",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuDateTo} },
                    {"@CustomerCD",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.CustomerCD} },
                    {"@JuchuuNO",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuNO} },
                    {"@ChkNotReserve",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ChkNotReserve} },
                    {"@ChkReserved",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.ChkReserved} },
                    {"@JuchuuKBN1",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuKBN1} },
                    {"@JuchuuKBN2",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuKBN2} },
                    {"@JuchuuKBN3",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuKBN3} },
                    {"@JuchuuKBN4",new ValuePair { value1=SqlDbType.VarChar,value2=hhe.JuchuuKBN4} },
               };
            return SelectData(dic, "D_Stock_SelectForHikiateJuchuu");
        }
        
        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="hhe"></param>
        /// <param name="zdt"></param>
        /// <param name="jdt"></param>
        /// <returns></returns>
        public bool PRC_HikiateHenkouNyuuryoku(HikiateHenkouNyuuryoku_Entity hhe, DataTable zdt, DataTable jdt)
        {
            string sp = "PRC_HikiateHenkouNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@StoreCD", SqlDbType.VarChar, hhe.StoreCD);
            AddParam(command, "@SKUCD", SqlDbType.VarChar, hhe.SKUCD);
            AddParamForDataTable(command, "@ZTable", SqlDbType.Structured, zdt);
            AddParamForDataTable(command, "@RTable", SqlDbType.Structured, jdt);
            AddParam(command, "@Operator", SqlDbType.VarChar, hhe.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, hhe.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutSKUCD";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 30);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                hhe.SKUCD = outPutParam;

            return ret;
        }

        /// <summary>
        /// 引当更新処理
        /// </summary>
        /// <param name="hhe"></param>
        /// <returns></returns>
        public bool ALL_Hikiate(HikiateHenkouNyuuryoku_Entity hhe)
        {
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = hhe.Operator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = hhe.PC } },
                { "@PStoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = hhe.StoreCD } },
                { "@PAdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = hhe.AdminNO } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "ALL_Hikiate");
        }
    }
}
