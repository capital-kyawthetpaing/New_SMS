using Entity;
using System.Collections.Generic;
using System.Data;

namespace DL
{
    public class M_BankShiten_DL : Base_DL
    {
        public DataTable M_BankShiten_Search(M_BankShiten_Entity mbse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BankCD } },
                { "@BranchCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchCD_From } },
                { "@BranchCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchCD_To } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbse.ChangeDate } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbse.DeleteFlg } },
                { "@BranchName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchName } },
                 {"@KanaName",   new ValuePair{value1=SqlDbType.VarChar,value2=mbse.BranchKana} },
                //{ "@BranchKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchKana } },               
                { "@SearchType", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.searchType } }
            };
            return SelectData(dic, "M_BankShiten_Search");
        }
        public DataTable M_BankShiten_Select(M_BankShiten_Entity mbse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbse.ChangeDate } },
               // { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbse.DeleteFlg } }
            };
            return SelectData(dic, "M_BankShiten_Select");
        }

        public bool M_BankShiten_Insert_Update(M_BankShiten_Entity mbse, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbse.ChangeDate } },
                { "@BranchName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchName } },
                { "@BranchKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchKana } },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.Remarks } },               
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbse.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.InsertOperator } },                
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.Key } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };

            //UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_BankShiten_Insert_Update");
        }

        public bool M_BankShiten_Delete(M_BankShiten_Entity mbse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BankCD } },
                { "@BranchCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.BranchCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbse.ChangeDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbse.Key } }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_BankShiten_Delete");
        }

        public DataTable M_BankShiten_Search(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Fields", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.FieldsName } },
                { "@SoukoCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCDFrom } },
                { "@SoukoCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoCDTo } },
                { "@SoukoName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SoukoName } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.StoreCD } },
                { "@SoukoType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.SoukoType } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.DeleteFlg } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } },
                { "@OrderBy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.OrderBy } },
            };
            return SelectData(dic, "M_Souko_Search");
        }

       
    }
}
