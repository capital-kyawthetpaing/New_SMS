using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;


namespace DL
{
   public class Brand_DL:Base_DL
    {
        public DataTable M_BrandSelect(M_Brand_Entity  mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@Brand", new ValuePair {value1 = SqlDbType.VarChar,value2 = mbe.BrandCD} }
            };
            return SelectData(dic,"M_BrandSelect");
        }

        public bool M_Brand_Insert_Update(M_Brand_Entity mbe, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Brand", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BrandCD } },
                { "@BrandName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe .BrandName } },               
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BrandCD+" "+mbe.ChangeDate } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Brand_Insert_Update");
        }

        public bool M_Brand_Delete(M_Brand_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@Brand", new ValuePair {value1 = SqlDbType.VarChar,value2 = mbe.BrandCD} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BrandCD+" "+mbe.ChangeDate } }
            };

            return InsertUpdateDeleteData(dic, "M_Brand_Delete");
        }

    }
}
