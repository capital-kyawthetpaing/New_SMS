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
    public class D_MailAddress_DL : Base_DL
    {
        public DataTable D_MailAddress_SelectAll(D_Mail_Entity de)
        {
            string sp = "D_MailAddress_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MailCounter", new ValuePair { value1 = SqlDbType.Int, value2 = de.MailCounter } },
            };

            return SelectData(dic, sp);
        }
    }
}
