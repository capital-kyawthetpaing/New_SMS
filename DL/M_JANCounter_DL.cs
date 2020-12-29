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


        public DataTable M_JANCounter_Update(M_JANCounter_Entity mje)
        {
            string sp = "M_JANCounter_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKEY", new ValuePair { value1 = SqlDbType.Int, value2 = mje.MainKEY } },
                { "@UpdatingFlg", new ValuePair { value1 = SqlDbType.Int, value2 = mje.UpdatingFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mje.Operator } },
            };
            //UseTransaction = true;
            //return InsertUpdateDeleteData(dic, "M_JANCounter_Update");
            return SelectData(dic, sp);
        }
    }

}
