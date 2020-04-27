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
    public class D_MoveRequest_DL : Base_DL
    {
        public DataTable D_MoveRequest_SelectAllForShoukai(D_MoveRequest_Entity de)
        {
            string sp = "D_MoveRequest_SelectAllForShoukai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AnswerDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.AnswerDateFrom } },
                { "@AnswerDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.AnswerDateTo } },
                { "@FromStoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.FromStoreCD } },
                { "@AnswerKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.AnswerKBN.ToString() } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InsertOperator } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 移動依頼入力データ取得処理
        /// ZaikoIdouIraiNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_MoveRequest_SelectDataForIdouIrai(D_MoveRequest_Entity de)
        {
            string sp = "D_MoveRequest_SelectDataForIdouIrai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@RequestNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.RequestNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 移動依頼入力更新処理
        /// ZaikoIdouIraiNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <param name="operationMode"></param>
        /// <returns></returns>
        public bool D_MoveRequest_Exec(D_MoveRequest_Entity de, DataTable dt, short operationMode)
        {
            string sp = "PRC_ZaikoIdouIraiNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@RequestNO", SqlDbType.VarChar, de.RequestNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, de.StoreCD);
            AddParam(command, "@MovePurposeKBN", SqlDbType.TinyInt, de.MovePurposeKBN);
            //AddParam(command, "@MovePurposeType", SqlDbType.TinyInt, de.MovePurposeType);
            AddParam(command, "@RequestDate", SqlDbType.VarChar, de.RequestDate);
            AddParam(command, "@FromStoreCD", SqlDbType.VarChar, de.FromStoreCD);
            AddParam(command, "@FromSoukoCD", SqlDbType.VarChar, de.FromSoukoCD);
            AddParam(command, "@ToStoreCD", SqlDbType.VarChar, de.ToStoreCD);
            AddParam(command, "@ToSoukoCD", SqlDbType.VarChar, de.ToSoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, de.StaffCD);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, de.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutRequestNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                de.RequestNO = outPutParam;

            return ret;
        }
    } 
}
