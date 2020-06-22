using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_StoreBottunGroup_DL : Base_DL
    {

        public DataTable TempoRegiMasterNyuuryoku_Grid_Select(int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@Mode",new ValuePair {value1=SqlDbType.Int,value2=mode.ToString()} }

            };

            return SelectData(dic, "TempoRegiMasterNyuuryoku_Select");
        }

        public DataTable M_StoreBottunGroup_Select(string StoreCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=StoreCD} }

            };

            return SelectData(dic, "M_StoreBottunGroup_Select");
        }
        public DataTable TempoRegiMasterNyuuryoku_buttonName_Select(string btnname)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@btnName",new ValuePair {value1=SqlDbType.VarChar,value2=btnname} }

            };

            return SelectData(dic, "M_StoreBottunGroup_Select");
        }
        public DataTable M_StoreButtonGroup_SelectAll(M_StoreBottunGroup_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@ProgramKBN",new ValuePair {value1=SqlDbType.TinyInt,value2=mse.ProgramKBN} }

            };
            return SelectData(dic, "M_StoreButtonGroup_SelectAll");
        }
        //public bool M_StoreBottunGroup_Insert_Update(M_StoreBottunGroup_Entity msbg, int mode)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
        //    {
        //         { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=msbg.StoreCD} },
        //         { "@ProgramKBN",new ValuePair {value1=SqlDbType.TinyInt,value2=msbg.ProgramKBN} },
        //         { "@GroupNO",new ValuePair {value1=SqlDbType.TinyInt,value2=msbg.GroupNO} },
        //         { "@BottunName",new ValuePair {value1=SqlDbType.VarChar,value2=msbg.BottunName} },
        //         { "@MasterKBN",new ValuePair {value1=SqlDbType.TinyInt,value2=msbg.MasterKBN} },
        //         { "@InsertOperator",new ValuePair {value1=SqlDbType.VarChar,value2=msbg.Operator} },
        //         { "@UpdateOperator",new ValuePair {value1=SqlDbType.VarChar,value2=msbg.Operator} },
        //         { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } },
        //         { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbg.Operator } },
        //         { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbg.ProgramID } },
        //         { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbg.PC } },
        //         { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbg.ProcessMode } },
        //         { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = msbg.Key } }

        //    };
        //    return InsertUpdateDeleteData(dic, "M_StoreButtonGroup_Insert_Update");
        //}
        

        //public bool M_StoreButton_Insert(M_StoreBottunGroup_Entity msbge)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
        //    {
        //         //{ "@GroupXML",new ValuePair {value1=SqlDbType.VarChar,value2=msbge.GroupXML} },
        //         { "@GroupDetailXML",new ValuePair {value1=SqlDbType.VarChar,value2=msbge.GroupXML} },
        //        {"@StoreCD" ,new ValuePair {value1=SqlDbType.VarChar,value2=msbge.StoreCD}},
        //        {"@MasterKBN",new ValuePair{value1=SqlDbType.TinyInt,value2=msbge.MasterKBN} }
        //    };
        //    //  return InsertUpdateDeleteData(dic, "M_StoreButton_Insert_Update");
        //    return InsertUpdateDeleteData(dic, "M_StoreButtonDetails_Insert_Update1");
        //}


      


        //public int TempoRegiMasterNyuuryoku_cell_Select(string btnname,string groupno)M_StoreButtonDetails_Insert
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
        //    {
        //         { "@btnname",new ValuePair {value1=SqlDbType.VarChar,value2=btnname.ToString()} },
        //         { "@btnname",new ValuePair {value1=SqlDbType.Int,value2=btnname.ToString()} }

        //    };
        //    return SelectData(dic, "TempoRegiMasterNyuuryoku_Select");
        //}
    }
}
