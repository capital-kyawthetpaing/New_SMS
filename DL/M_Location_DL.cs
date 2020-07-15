using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class M_Location_DL : Base_DL
    {
        public DataTable M_Location_Select(M_Souko_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=mse.SoukoCD} },
                   { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mse.ChangeDate } }
               };
            return SelectData(dic, "M_Location_Select");
        }

        public DataTable M_Location_SelectData(M_Location_Entity me)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.SoukoCD} },
                   {"@TanaCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.TanaCD} },
                   { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.ChangeDate } }
               };
            return SelectData(dic, "M_Location_SelectData");
        }


        public DataTable M_LocationTana_Select(M_Location_Entity mle)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=mle.SoukoCD} },
                   {"@TanaCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.TanaCD } }
               };
            return SelectData(dic, "M_LocationTana_Select");
        }

        public DataTable M_Location_DataSelect(D_Stock_Entity dse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@ArrivalStartDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.ArrivalStartDate } },
                   {"@ArrivalEndDate",new ValuePair { value1=SqlDbType.VarChar,value2=dse.ArrivalEndDate} },
                   {"@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.SoukoCD} },
                   {"@Unregister",new ValuePair { value1=SqlDbType.TinyInt,value2=dse.UnregisterFlg} },
                   {"@Register", new ValuePair { value1 = SqlDbType.TinyInt, value2 =dse.RegisterFlg } }
               };
            return SelectData(dic, "M_Location_DataSelect");
        }

        public DataTable M_Location_Search(M_Location_Entity mle)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=mle.SoukoCD} },
                   { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mle.ChangeDate } }
               };
            return SelectData(dic, "M_Location_Search");
        }

        public bool M_Location_InsertUpdate(D_Stock_Entity dse, M_Location_Entity mle)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = dse.xml1 } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.Operator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mle.Key } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_Location_InsertUpdate");
        }
        public DataTable M_Location_SelectAll(M_Location_Entity me)

        {
            string sp = "M_Location_SelectAll";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=me.SoukoCD} },
                   { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = me.ChangeDate } }
            };

            return SelectData(dic, sp);
        }

        public DataTable M_Location_SelectForTanaoroshi(D_InventoryProcessing_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@InventoryDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.InventoryDate } },
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=de.SoukoCD} },
                   {"@TanaCDFrom",new ValuePair { value1=SqlDbType.VarChar,value2=de.FromRackNO} },
                   {"@TanaCDTo",new ValuePair { value1=SqlDbType.VarChar,value2=de.ToRackNO} },
               };
            return SelectData(dic, "M_Location_SelectForTanaoroshi");
        }

    }
}
