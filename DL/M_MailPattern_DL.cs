using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;


namespace DL
{
   public class M_MailPattern_DL : Base_DL
    {
        public DataTable M_MailPattern_Select(M_MailPattern_Entity mme)
        {
            string sp = "M_MailPattern_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailPatternCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MailPatternCD } },

            };

            return SelectData(dic, sp);
        }
        public DataTable M_MailPattern_SelectAll(M_MailPattern_Entity mme)
        {
            string sp = "M_MailPattern_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailPatternCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MailPatternCD } },
            };

            return SelectData(dic, sp);
        }
    }
}
