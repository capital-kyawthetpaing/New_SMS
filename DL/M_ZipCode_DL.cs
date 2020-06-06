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
    public class M_ZipCode_DL : Base_DL
    {
        public DataTable M_ZipCode_Select(M_ZipCode_Entity mze)
        {
            string sp = "M_ZipCode_Select";

            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@ZipCD1", SqlDbType.VarChar).Value = mze.ZipCD1;
            //command.Parameters.Add("@ZipCD2", SqlDbType.VarChar).Value = mze.ZipCD2;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ZipCD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mze.ZipCD1 } },
                 { "@ZipCD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mze.ZipCD2 } }
            };

            //return SelectData(sp);
            return SelectData(dic,sp);
            
        }

        public DataTable M_ZipCode_YuubinBangou_Select(M_ZipCode_Entity Zipcode, string Zip1To, string Zip2To)
        {
            string sp = "M_ZipCode_YuubinBangou_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Zip1From", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zipcode.ZipCD1 } },
                { "@Zip2From", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zipcode.ZipCD2 } },
                { "@Zip1To", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zip1To } },
                { "@Zip2To", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zip2To } }
            };

            return SelectData(dic, sp);
        }

        public bool M_ZipCode_Update(M_ZipCode_Entity ZipCode, string Zip1To, string Zip2To, string Xml)
        {
            string sp = "M_ZipCode_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Zip1From", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.ZipCD1 } },
                { "@Zip2From", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.ZipCD2 } },
                { "@Zip1To", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zip1To } },
                { "@Zip2To", new ValuePair { value1 = SqlDbType.VarChar, value2 = Zip2To } },
                { "@Xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = Xml } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.Operator }},
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.ProgramID }},
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.PC }},
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.ProcessMode }},
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = ZipCode.Key }}
            };
            UseTransaction = true;

            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
