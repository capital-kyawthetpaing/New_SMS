using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Entity;

namespace DL
{
    public class M_Calendar_DL:Base_DL
    {
        public DataTable M_Calendar_Select(string month)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Month", new ValuePair { value1 = SqlDbType.VarChar, value2 =month} },
               
            };
            return SelectData(dic, "M_Calendar_Select");
        }

        public DataTable M_Calendar_SelectDayKBN(string date)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Date", new ValuePair { value1 = SqlDbType.VarChar, value2 =date} },
            };
            return SelectData(dic, "M_Calendar_SelectDayKBN");
        }

        public bool M_Calendar_Insert_Update(M_Calendar_Entity mce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DayOffXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.DayOffXml} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mce.Key } }

            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Calendar_Update");

        }

        public DataTable M_Calendar_SelectCalencarDate(M_Calendar_Entity mce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CalendarDate", new ValuePair { value1 = SqlDbType.Date, value2 = mce.CalendarDate} }
            };
            return SelectData(dic, "M_Calendar_SelectCalencarDate");
        }

        public DataTable GetYoteibi(string KaisyuShiharaiKbn, string CustomerCD, string ChangeDate, string TyohaKbn)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KaisyuShiharaiKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = KaisyuShiharaiKbn} },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = CustomerCD} },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = ChangeDate} },
                { "@TyohaKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = TyohaKbn} }
            };
            return SelectData(dic, "Get_ShihraiYoteibi");
        }

        public DataTable M_Calendar_SelectForFB(M_Calendar_Entity mce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CalendarDate", new ValuePair { value1 = SqlDbType.Date, value2 = mce.CalendarDate} }
            };
            return SelectData(dic, "M_Calendar_SelectForFB");
        }

    }
}
