using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
   public class M_Hanyou_DL :Base_DL
    {
        public DataTable M_Hanyou_IDSearch(M_Hanyou_Entity mhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@IDFrom", new ValuePair {value1 = SqlDbType.Int,value2 = mhe.IDFrom} },
                { "@IDTo", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.IDTo } }
            };         
            return SelectData(dic, "M_MultiPurpose_IDSearch");
        }

        public DataTable M_Hanyou_KeySearch(M_Hanyou_Entity mhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ID", new ValuePair {value1 = SqlDbType.Int,value2 = mhe.ID} },
                { "@KeyFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.KeyFrom } },
                { "@KeyTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.KeyTo } }
            };
            return SelectData(dic, "M_MultiPurpose_KeySearch");
        }

        public DataTable M_Hanyou_IDSelect(M_Hanyou_Entity mhe)
        {         
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                  { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.ID } }
            };
            return SelectData(dic, "M_MultiPurpose_IDSelect");
        }

        public DataTable M_Hanyou_KeySelect(M_Hanyou_Entity mhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@ID", new ValuePair {value1 = SqlDbType.Int,value2 = mhe.ID} },
                {"@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Key } }              
            };
            return SelectData(dic, "M_MultiPurpose_KeySelect");
        }
        
        public bool M_Hanyou_Insert_Update(M_Hanyou_Entity mhe,int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Key } },
                //{ "@IDName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.IDName } },
                { "@Text1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Text1 } },
                { "@Text2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Text2 } },
                { "@Text3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Text3 } },
                { "@Text4", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Text4 } },
                { "@Text5", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Text5 } },
                { "@Digital1", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.Digital1 } },
                { "@Digital2", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.Digital2 } },
                { "@Digital3", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.Digital3 } },
                { "@Digital4", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.Digital4 } },
                { "@Digital5", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.Digital5 } },
                { "@Day1", new ValuePair { value1 = SqlDbType.DateTime, value2 = mhe.Day1 } },
                { "@Day2", new ValuePair { value1 = SqlDbType.DateTime, value2 = mhe.Day2 } },
                { "@Day3", new ValuePair { value1 = SqlDbType.DateTime, value2 = mhe.Day3 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ID+" "+mhe.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_MultiPurpose_Insert_Update");
        }

        public bool M_Hanyou_Delete(M_Hanyou_Entity mhe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.ID } },
                { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Key } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.ID+" "+mhe.Key } }
            };

            return InsertUpdateDeleteData(dic, "M_MultiPurpose_Delete");
        }

        //public DataTable M_Hanyou_Select(M_Hanyou_Entity mhe)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        { "@ID", new ValuePair { value1 = SqlDbType.Int, value2 = mhe.ID } },
        //        { "@Key", new ValuePair { value1 = SqlDbType.VarChar, value2 = mhe.Key } }
        //    };
        //    return SelectData(dic, "M_MultiPurpose_KeySelect");
        //}
       
    }
}
