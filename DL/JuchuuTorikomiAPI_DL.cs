using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class JuchuuTorikomiAPI_DL : Base_DL
    {
        public bool JuchuuTorikomiAPI_Insert_Update(D_APIRireki_Entity mre)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Num1",new ValuePair {value1=SqlDbType.Int,value2=mre.ShowMode} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = null } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Key } }

            };
            return InsertUpdateDeleteData(dic, "D_APIRireki_Insert_Update");
        }
        public bool JuchuuTorikomiAPI_D_APIControl_Insert_Update(D_APIRireki_Entity mre)
        {
          Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
          {
            { "@APIKey",new ValuePair { value1 = SqlDbType.Int, value2 = mre.APIKey } },
            { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.Operator } },
            { "@State", new ValuePair { value1 = SqlDbType.VarChar, value2 = mre.State } }
        };
         return InsertUpdateDeleteData(dic, "D_APIControl_Insert_Update");
        }
        public DataTable JuchuuTorikomiAPI_Grid_Select()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();
           
            return SelectData(dic, "D_APIRireki_Grid_Select");
        }
        public DataTable D_APIControl_Select()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            return SelectData(dic, "D_APIControl_Select");
        }
    }
}
