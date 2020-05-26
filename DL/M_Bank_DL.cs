using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class M_Bank_DL:Base_DL
    {
        public DataTable M_Bank_Search(M_Bank_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Fields", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.FieldsName } },
                { "@BankCDFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankCDFrom } },
                { "@BankCDTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankCDTo } },
                { "@BankName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankName } },
                { "@BankKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankKana } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } },
                { "@SearchType", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.searchType } },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mbe.DeleteFlg } },
                { "@OrderBy", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.OrderBy } },
            };
            UseTransaction = true;
            return SelectData(dic, "M_Bank_Search");
        }

        public DataTable M_Bank_Select(M_Bank_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } }
            };
            return SelectData(dic, "M_Bank_Select");
        }

        public DataTable M_Bank_ChangeDate_Select(M_Bank_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } }
            };
            return SelectData(dic, "M_Bank_ChangeDate_Select");
        }
        public DataTable M_Ginkou_Select(M_Ginkou_Entity mge)
        {

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@GinkoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ginko_CD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mge.ginko_Changedate } }
            };
            return SelectData(dic, "M_Ginkou_Select");

        }
        public bool M_Ginkou_Delete(M_Ginkou_Entity mge)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                { "@Ginko_CD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ginko_CD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mge.ginko_Changedate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ginko_insertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.Program } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.KeyItem } }
            };

            return InsertUpdateDeleteData(dic, "M_Ginkou_Delete");
        }
        public bool M_Ginkou_Insert_Update(M_Ginkou_Entity mge, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 =SqlDbType.VarChar, value2 =  mge.ginko_CD } },
                { "@ChangeDate", new ValuePair { value1 = System.Data.SqlDbType.Date, value2 =  mge.ginko_Changedate } },
                { "@BankName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ginko_Name } },
                { "@BankKana", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.ginko_kananame} },
                { "@Remarks", new ValuePair { value1 = SqlDbType.VarChar, value2 =  mge.ginko_remarks} },
                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 =  mge.ginko_DeleteFlag} },
                { "@UsedFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  mge.ginko_useflag } },
                { "@InsertOperator", new ValuePair { value1 = SqlDbType.VarChar, value2 =  mge.ginko_insertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.Program } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.OperateMode} },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mge.KeyItem} },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } }
            };
            return InsertUpdateDeleteData(dic, "M_Ginkou_Insert_Update");
        }

        public DataTable M_Vendor_Bank_Select(M_Bank_Entity mbe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@BankCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mbe.BankCD} },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mbe.ChangeDate } },
            };
            UseTransaction = true;
            return SelectData(dic, "M_Vendor_Bank_Select");
        }
    }
}
