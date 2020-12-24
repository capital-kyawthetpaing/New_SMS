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
    public class D_Exclusive_DL : Base_DL
    {
        public DataTable D_Exclusive_Select(D_Exclusive_Entity dee)
        {
            string sp = "D_Exclusive_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DataKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dee.DataKBN.ToString() } },
                { "@Number", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Number } },
            };
            return SelectData(dic, sp);
        }

        public bool D_Exclusive_Insert(D_Exclusive_Entity dee)
        {
            string sp = "D_Exclusive_Insert";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DataKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dee.DataKBN.ToString()  } },
                { "@Number", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Number } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Program } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.PC } },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        public bool D_Exclusive_Delete(D_Exclusive_Entity dee)
        {
            string sp = "D_Exclusive_Delete";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DataKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dee.DataKBN.ToString()  } },
                { "@Number", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Number } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        /// <summary>
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Exclusive_Exec(D_Exclusive_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc )
        {
            string sp = "PRC_MitsumoriNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            command.Parameters.Add("@OperateMode", SqlDbType.TinyInt).Value = operationMode;
            //command.Parameters.Add("@GeneralRate", SqlDbType.Decimal).Value = dme.GeneralRate;
            //command.Parameters.Add("@MemberRate", SqlDbType.Decimal).Value = dme.MemberRate;
            //command.Parameters.Add("@ClientRate", SqlDbType.Decimal).Value = dme.ClientRate;
            //command.Parameters.Add("@SaleRate", SqlDbType.Decimal).Value = dme.SaleRate;
            //command.Parameters.Add("@WebRate", SqlDbType.Decimal).Value = dme.WebRate;
            command.Parameters.Add("@Table", SqlDbType.Structured).Value = dt;

            command.Parameters.Add("@DeleteFlg", SqlDbType.TinyInt).Value = dme.DeleteFlg;
            command.Parameters.Add("@UsedFlg", SqlDbType.TinyInt).Value = dme.UsedFlg;
            command.Parameters.Add("@Operator", SqlDbType.VarChar).Value = operatorNm;
            command.Parameters.Add("@PC", SqlDbType.VarChar).Value = pc;

            string outPutParam="";
            UseTransaction = true;
            return InsertUpdateDeleteData(sp,ref outPutParam);
        }

        /// <summary>
        /// </summary>
        public DataTable D_Exclusive_SelectData(D_Exclusive_Entity mie, short operationMode)
        {
            string sp = "D_Exclusive_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                //{ "@MitsumoriNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.MitsumoriNO } },
            };
            
            return SelectData(dic, sp);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dee"></param>
        /// <returns></returns>
        public bool D_Exclusive_DeleteByKBN(D_Exclusive_Entity dee)
        {
            string sp = "D_Exclusive_DeleteByKBN";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Program } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.PC } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }

}
