using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_Brand_DL : Base_DL
    {
        public DataTable M_Brand_Select(M_Brand_Entity mme)
        {
            string sp = "M_Brand_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.BrandCD } },

            };

            return SelectData(dic, sp);
        }
        public DataTable M_Brand_SelectAll(M_Brand_Entity mme)
        {
            string sp = "M_Brand_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DisplayKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mme.DisplayKbn } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.ChangeDate } },
                { "@BrandName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.BrandName } },
                { "@MakerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mme.MakerCD } },

            };
            
            return SelectData(dic, sp);
        }

        public DataTable M_BrandSelect(M_Brand_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@Brand", new ValuePair {value1 = SqlDbType.VarChar,value2 = mbe.BrandCD} }
            };
            return SelectData(dic, "M_BrandSelect");
        }

        public bool M_Brand_Insert_Update(M_Brand_Entity mbe, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Brand", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BrandCD } },
                { "@BrandName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe .BrandName } },
                { "@BrandKana",new ValuePair{value1=SqlDbType.VarChar,value2=mbe.BrandKana} },
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

        /// <summary>
        /// For TenzikaiShouhin 
        /// </summary>
        /// <returns></returns>
        public DataTable M_Brand_SelectAll_NoPara()
        {
            string sp = "M_Brand_SelectAll_NoPara";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };

            return SelectData(dic, sp);
        }
    }
}
