using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DL
{
    public class D_JANUpdate_DL : Base_DL
    {
        public bool JanCDHenkou_Insert(string xml)
        {
            string sp = "JanCDHenkou_Insert";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = xml } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic,sp);
        }
    }
}
