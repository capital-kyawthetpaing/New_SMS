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
    public class TempoShukkaNyuuryoku_DL : Base_DL
    {

        /// <summary>
        /// 店舗レジ出荷売上入力更新処理
        /// TempoShukkaNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dse"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool PRC_TempoShukkaNyuuryoku(D_Sales_Entity dse, DataTable dt, short operationMode, string operatorNm, string pc )
        {
            string sp = "PRC_TempoShukkaNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command, "@JuchuuNO", SqlDbType.VarChar, dse.JuchuuNO);
            AddParam(command, "@SalesNO", SqlDbType.VarChar, dse.SalesNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dse.StoreCD);
            AddParam(command, "@SalesDate", SqlDbType.VarChar, dse.SalesDate);
            AddParam(command, "@FirstCollectPlanDate", SqlDbType.VarChar, dse.FirstCollectPlanDate);
            AddParam(command, "@CustomerCD", SqlDbType.VarChar, dse.CustomerCD);
            AddParam(command, "@BillingType", SqlDbType.TinyInt, dse.BillingType);
            AddParam(command, "@SalesGaku", SqlDbType.Money, dse.SalesGaku);
            AddParam(command, "@SalesTax", SqlDbType.Money, dse.SalesTax);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command,"@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command,"@PC", SqlDbType.VarChar, pc);

            //OUTパラメータの追加
            string outPutParam = "@OutSalesNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dse.SalesNO = outPutParam;

            return ret;
        }

        /// <summary>	
        /// 店舗売上入力更新処理	
        /// TempoUriageNyuuryokuより更新時に使用	
        /// ※店舗レジ出荷売上入力の更新内容とほとんど同じ	
        /// </summary>	
        /// <param name="dse"></param>	
        /// <param name="dt"></param>	
        /// <param name="operationMode"></param>	
        /// <returns></returns>	
        public bool PRC_TempoUriageNyuuryoku(D_Sales_Entity dse, DataTable dt, short operationMode)
        {
            string sp = "PRC_TempoUriageNyuuryoku";
            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dse.StoreCD);
            AddParam(command, "@SalesDate", SqlDbType.VarChar, dse.SalesDate);
            AddParam(command, "@BillingType", SqlDbType.TinyInt, dse.BillingType);
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dse.Operator);
            AddParam(command, "@PC", SqlDbType.VarChar, dse.PC);
            //OUTパラメータの追加	
            string outPutParam = "@OutSalesNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;
            UseTransaction = true;
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dse.SalesNO = outPutParam;
            return ret;
        }
    }
}
