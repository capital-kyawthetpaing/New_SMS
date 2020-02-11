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
    public class D_Edi_DL : Base_DL
    {
        public DataTable D_Edi_SelectAll()
        {
            string sp = "D_Edi_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = "" } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_EDIOrderDetails_SelectAll(D_EDIDetail_Entity de)
        {
            string sp = "D_EDIOrderDetails_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@EDIImportNO", new ValuePair { value1 = SqlDbType.Int, value2 = de.EDIImportNO } },
                { "@ErrorKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ErrorKBN } },
                { "@ChkAnswer", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkAnswer.ToString() } },
                { "@ChkNoAnswer", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkNoAnswer.ToString() } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_EDIOrderDetails_SelectForPrint(string ediOrderNO)
        {
            string sp = "D_EDIOrderDetails_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@EDIOrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = ediOrderNO } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// EDI回答納期登録にて使用
        /// </summary>
        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            string sp = "M_MultiPorpose_Update";
            
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mme.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.Key } },
                { "@Num1", new ValuePair { value1 = SqlDbType.Int, value2 = mme.Num1 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.UpdateOperator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.PC } },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        public bool D_EDI_Insert(D_EDI_Entity de, DataTable dt)
        {
            string sp = "D_EDI_Insert";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            
            AddParam(command, "@VendorCD", SqlDbType.VarChar, de.VendorCD);
            AddParam(command, "@ImportFile", SqlDbType.VarChar, de.ImportFile);
            AddParam(command, "@OrderDetailsSu", SqlDbType.Int, de.OrderDetailsSu.ToString());
            AddParam(command, "@ImportDetailsSu", SqlDbType.Int, de.ImportDetailsSu.ToString());
            AddParam(command, "@ErrorSu", SqlDbType.Int, de.ErrorSu.ToString());
         
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, de.UpdateOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, "");

            //OUTパラメータの追加
            string outPutParam = "@OutEDIImportNO";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                de.EDIImportNO = outPutParam;

            return ret;
        }
    }
}
