using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_PayCloseHistory_DL : Base_DL
    {

        public DataTable CheckPayCloseHistory(D_PayCloseHistory_Entity dpe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@PayeeCD", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayeeCD} }  ,
                {"@PayCloseDate", new ValuePair {value1 = SqlDbType.VarChar,value2 = dpe.PayCloseDate} }
            };
            return SelectData(dic, "CheckPayCloseHistory");
        }
        public DataTable Select_PaymentClose(D_PayCloseHistory_Entity dpch_entity, int Type)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@PaymentCloseCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.PaymentCD } },
                 { "@PaymentCloseDate", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpch_entity.PaymentDate } },
                 {"@Type" , new ValuePair{value1=SqlDbType.Int,value2=Type.ToString()} }
            };
            return SelectData(dic, "Select_PaymentClose_ValueCheck");
        }
        public DataTable D_PayClose_Search(D_PayCloseHistory_Entity dpch_entity)
        {

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

                { "@PayeeCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.PaymentCD} },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = dpch_entity.PaymentDate} },

            };
            return SelectData(dic, "D_PayClose_Search");
        }

        //public DataTable Return_Value(D_APIControl_Entity dpch_entity,int Type)
        //{
        //    Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
        //    {
        //        {"@ReferenceDate",new ValuePair{value1=SqlDbType.Date,value2=dpch_entity.pay} }
        //        {"@SlipType",new ValuePair{value1=SqlDbType.Int,value2=dpch_entity.SlipType } },
        //        { "@Type",new ValuePair {value1=SqlDbType.Int,value2=Type.ToString()} }
        //    };

        //}

        public bool Insert_ShiHaRaiShime_PaymentClose(D_PayCloseHistory_Entity dpch_entity, int InsertType)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                    { "@PaymentCloseCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.PaymentCD } },
                    { "@PaymentCloseDate", new ValuePair { value1 = SqlDbType.VarChar, value2 =dpch_entity.PaymentDate } },
                    { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.InsertOperator } },
                    { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.ProgramID } },
                    { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.PC } },
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.StoreCD } },
                    { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.ProcessMode } },
                    { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = dpch_entity.Key } },
                    {"@InsertType" , new ValuePair{value1=SqlDbType.Int,value2=InsertType.ToString()}  }

            };

            return InsertUpdateDeleteData(dic, "Insert_Update_ShiharaiShori");
        }
    }
}
