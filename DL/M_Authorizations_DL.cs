using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class M_Authorizations_DL : Base_DL
    {
        public DataTable M_Authorizations_AccessCheck(M_Authorizations_Entity mae)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ProgramID", new ValuePair { value1 = SqlDbType.VarChar, value2 = mae.ProgramID } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mae.StaffCD } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mae.PC } }
            };
            return SelectData(dic, "M_Authorizations_AccessCheck");
        }

        public DataTable M_StoreAuthorizations_Select(M_StoreAuthorizations_Entity msa)
        {
            string sp = "M_StoreAuthorizations_Select";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@StoreAuthorizationsCD", SqlDbType.VarChar).Value = msa.StoreAuthorizationsCD;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreAuthorizationsCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msa.StoreAuthorizationsCD } }
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }
    }
}
