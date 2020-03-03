using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class M_Control_DL : Base_DL
    {
        public DataTable M_Control_RecordCheck(string redcordDate)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@RecordDate", new ValuePair { value1 = SqlDbType.Date, value2 = redcordDate });

            UseTransaction = true;
            return SelectData(dic, "M_Control_RecordCheck");
        }

        public DataTable M_Control_PaymentSelect(M_FiscalYear_Entity mfye)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@InputPossibleStartDate", new ValuePair {value1 = SqlDbType.Date,value2 = mfye.InputPossibleStartDate} },
                {"@InputPossibleEndDate", new ValuePair {value1 = SqlDbType.Date,value2 = mfye.InputPossibleEndDate} }
            };
            return SelectData(dic, "M_Control_PaymentSelect");
        }
        public DataTable M_Control_CheckDate(M_Control_Entity mce)
        {
            string sp = "M_Control_CheckDate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mce.MainKey } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.ChangeDate } }
            };

            return SelectData(dic, sp);
        }
        public DataTable M_Control_CheckDateWithFisicalMonth(M_Control_Entity mce)
        {
            string sp = "M_Control_CheckDateWithFisicalMonth";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mce.MainKey } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.ChangeDate } }
            };
            return SelectData(dic, sp);
        }
        public DataTable M_Control_Select(M_Control_Entity mce)
        {
            string sp = "M_Control_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MainKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mce.MainKey } }
            };

            return SelectData(dic, sp);
        }
    }
}
