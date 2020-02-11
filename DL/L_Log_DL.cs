using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class L_Log_DL : Base_DL
    {
        public bool L_Log_Insert(L_Log_Entity lle)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = lle.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = lle.Program } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = lle.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = lle.OperateMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = lle.KeyItem } }
            };

            return InsertUpdateDeleteData(dic, "L_Log_Insert");
        }



        public bool L_Log_Insert(string[] linfo)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 =linfo[0] } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = linfo[1] } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = linfo[2]} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 =  linfo[3] } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 =linfo[4] } }
            };

            return InsertUpdateDeleteData(dic, "L_Log_Insert");
        }
        //public bool L_Log_Insert_Print(string[] str, DataTable dtlog)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 = str[0] } },
        //        { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 =str[1] } },
        //        { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 =str[2]} },
        //        { "@tablelog", new ValuePair { value1 = SqlDbType.Structured, value2 = dtlog} }
        //    };

        //    return InsertUpdateDeleteData(dic, "L_Log_Insert");
        //}
        public bool L_Log_Insert_Print(string[] str, DataTable dtlog)
        {
            string sp = "D_keihiPrint_Log";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dtlog);
            AddParam(command, "@Operator", SqlDbType.VarChar, str[0]);
            AddParam(command, "@PC", SqlDbType.VarChar, str[1]);
            AddParam(command, "@ProgramID", SqlDbType.VarChar, str[2]);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
    }
}
