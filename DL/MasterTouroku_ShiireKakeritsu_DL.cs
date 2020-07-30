using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class MasterTouroku_ShiireKakeritsu_DL:Base_DL
    {
        public DataTable MasterTouroku_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        {
            string sp = "M_ShiireKakeritsu_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@VendorCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.VendorCD  } },
                {"@StoreCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.StoreCD} }
            };
            return SelectData(dic, sp);
        }
        public bool M_Shiirekakeritsu(M_OrderRate_Entity moe,string delData,string insertData)
        {
            string sp = "M_OrderRate_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@delXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = delData } },
                { "@insertXml",new ValuePair{value1=SqlDbType.VarChar,value2=insertData } },
                { "@VendorCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.VendorCD  } },
                { "@StoreCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.StoreCD  } },
                { "@ChangeDate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.ChangeDate} },
                { "@Rate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.Rate} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.Operator }},
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.ProgramID }},
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.PC }},
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.ProcessMode }},
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.Key }}
            };
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
