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
    public class M_DenominationKBN_DL : Base_DL 
    {

        public DataTable M_DenominationKBN_SelectAll(M_DenominationKBN_Entity me)
        {
            string sp = "M_DenominationKBN_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SystemKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.SystemKBN} }
            };

            return SelectData(dic,sp);
        }
        public DataTable M_DenominationKBN_Select(M_DenominationKBN_Entity me)
        {
            string sp = "M_DenominationKBN_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DenominationCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.DenominationCD} }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_Denomination_cboSelect(M_DenominationKBN_Entity me)
        {
            string sp = "M_Denomination_cboSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SystemKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.SystemKBN} }
            };

            return SelectData(dic, sp);
        }
    }
}
