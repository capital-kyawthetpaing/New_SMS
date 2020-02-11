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
    public class D_Move_DL : Base_DL
    {
        public DataTable D_Move_SelectAll(D_Move_Entity de)
        {
            string sp = "D_Move_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MoveDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MoveDateFrom } },
                { "@MoveDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MoveDateTo } },
                { "@MovePurposeKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.MovePurposeKBN } },
                { "@FromSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.FromSoukoCD } },
                { "@ToSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ToSoukoCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InsertOperator } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_MoveRequest_SelectAll(D_Move_Entity de)
        {
            string sp = "D_MoveRequest_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RequestDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MoveDateFrom } },
                { "@RequestDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MoveDateTo } },
                { "@MovePurposeKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.MovePurposeKBN } },
                { "@FromSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.FromSoukoCD } },
                { "@ToSoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ToSoukoCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InsertOperator } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 移動入力更新処理
        /// ShiireNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_Move_Exec(D_Move_Entity de, DataTable dt, short operationMode)
        {
            string sp = "PRC_ZaikoIdouNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@MoveNO", SqlDbType.VarChar, de.MoveNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, de.StoreCD);
            AddParam(command, "@RequestNO", SqlDbType.VarChar, de.RequestNO);
            AddParam(command, "@MovePurposeKBN", SqlDbType.TinyInt, de.MovePurposeKBN);
            AddParam(command, "@MovePurposeType", SqlDbType.TinyInt, de.MovePurposeType);
            AddParam(command, "@MoveDate", SqlDbType.VarChar, de.MoveDate);
            AddParam(command, "@FromStoreCD", SqlDbType.VarChar, de.FromStoreCD);
            AddParam(command, "@FromSoukoCD", SqlDbType.VarChar, de.FromSoukoCD);
            AddParam(command, "@ToStoreCD", SqlDbType.VarChar, de.ToStoreCD);
            AddParam(command, "@ToSoukoCD", SqlDbType.VarChar, de.ToSoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, de.StaffCD);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, de.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutMoveNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                de.MoveNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 移動入力データ取得処理
        /// ZaikoIdouNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Move_SelectData(D_Move_Entity de, short operationMode)
        {
            string sp = "D_Move_SelectData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@MoveNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.MoveNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 移動入力データ取得処理
        /// ZaikoIdouNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_MoveRequest_SelectData(D_Move_Entity de)
        {
            string sp = "D_MoveRequest_SelectData";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RequestNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.RequestNO } },
            };

            return SelectData(dic, sp);
        }
    } 
}
