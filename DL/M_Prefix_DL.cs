using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class M_Prefix_DL : Base_DL
    {
        public DataTable ShowPrefix(M_Prefix_Entity prefix)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.StoreCD });
            dic.Add("@SeqKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = prefix.SeqKBN });
            dic.Add("@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = prefix.ChangeDate });

            UseTransaction = true;
            return SelectData(dic, "M_Prefix_Display");
        }

        public bool M_Prefix_Insert_Update(M_Prefix_Entity prefix,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.StoreCD });
            dic.Add("@SeqKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = prefix.SeqKBN });
            dic.Add("@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = prefix.ChangeDate });
            dic.Add("@Prefix", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.Prefix });
            dic.Add("@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() });
            dic.Add("@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.InsertOperator });
            dic.Add("@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.ProgramID });
            dic.Add("@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.PC });
            dic.Add("@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.ProcessMode });
            dic.Add("@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.Key });

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Prefix_Insert_Update");
        }

        public bool M_Prefix_Delete(M_Prefix_Entity prefix)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.StoreCD } },
                { "@SeqKBN" , new ValuePair { value1 = SqlDbType.TinyInt, value2 = prefix.SeqKBN } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = prefix.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = prefix.Key } }
            };

            return InsertUpdateDeleteData(dic, "M_Prefix_Delete");
        }
    }
}
