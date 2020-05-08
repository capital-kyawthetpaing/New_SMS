using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
    public class D_JANUpdate_DL : Base_DL
    {
        public bool JanCDHenkou_Insert(string xml, L_Log_Entity log_data)
        {
            string sp = "JanCDHenkou_Insert";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = xml } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = log_data.Operator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = log_data.PC } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = log_data.Program } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = log_data.OperateMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = log_data.KeyItem } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic,sp);
        }
    }
}
