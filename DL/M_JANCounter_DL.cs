using System.Collections.Generic;
using System.Data;
using Entity;

namespace DL
{
    public class M_JANCounter_DL : Base_DL
    {
        public DataTable M_JANCounter_Select(M_JANCounter_Entity mje)
        {
            string sp = "M_JANCounter_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKEY", new ValuePair { value1 = SqlDbType.Int, value2 = mje.MainKEY } },
            };
            return SelectData(dic, sp);
        }


        public bool M_JANCounter_Update(M_JANCounter_Entity mje)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKEY", new ValuePair { value1 = SqlDbType.Int, value2 = mje.MainKEY } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mje.Operator } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_JANCounter_Update");
        }
    }

}
