using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;

namespace DL
{
    public class M_Renban_DL : Base_DL
    {
        public bool M_Renban_Insert_Update(M_Renban_Entity mre, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@PrefixValue",new ValuePair {value1=SqlDbType.VarChar,value2=mre.PrefixValue} },
                { "@Continuous",new ValuePair {value1=SqlDbType.Int,value2=mre.Continuous} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }

            };
            return InsertUpdateDeleteData(dic, "M_Renban_Insert_Update");
        }
        public bool M_Renban_Delete(M_Renban_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@PrefixValue",new ValuePair {value1=SqlDbType.VarChar,value2=mre.PrefixValue} },
                { "@Continuous",new ValuePair {value1=SqlDbType.VarChar,value2=mre.Continuous} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Key } }
            };

            return InsertUpdateDeleteData(dic, "M_Renban_Delete");
        }
        public DataTable M_Renban_Select(M_Renban_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@PrefixValue",new ValuePair {value1=SqlDbType.VarChar,value2=mre.PrefixValue} }

            };

            return SelectData(dic, "M_Renban_Select");
        }

    }

}
