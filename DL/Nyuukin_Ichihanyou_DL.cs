using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;
using System.Data.SqlClient;
namespace DL
{
   public class Nyuukin_Ichihanyou_DL :Base_DL
    {
        public Nyuukin_Ichihanyou_DL()
        {

        }

        public DataTable getPrintData(Nyuukin_Ichihanyou_Entity nie)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@paystart", new ValuePair { value1 = SqlDbType.Date, value2 = nie.paymentstart });
            dic.Add("@payend", new ValuePair { value1 = SqlDbType.Date, value2 = nie.paymentend });
            dic.Add("@payinputstart", new ValuePair { value1 = SqlDbType.Date, value2 = nie.paymentinputstart });
            dic.Add("@payinputend", new ValuePair { value1 = SqlDbType.Date, value2 = nie.paymentinputend });
            dic.Add("@StoreName", new ValuePair { value1 = SqlDbType.VarChar, value2 = nie.cbo_store });
            dic.Add("@WebCollectType", new ValuePair { value1 = SqlDbType.VarChar, value2 = nie.cbo_torikomi});
            dic.Add("@CollectCustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = nie.search_customer });
            dic.Add("@Is_All", new ValuePair { value1 = SqlDbType.Int, value2 = nie.rdb_all });//@StoreCD

            dic.Add("@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = nie.cbo_store_cd });//@StoreCD
            UseTransaction = true;
            return SelectData(dic, "D_NyuuKinPrint");
        }

        public bool L_Log_Insert_Print(string[] str, DataTable dtlog)
        {
            string sp = "D_NyuukinIchihanyou_Log";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dtlog);
            AddParam(command, "@Operator", SqlDbType.VarChar, str[0]);
            AddParam(command, "@PC", SqlDbType.VarChar, str[1]);
            AddParam(command, "@ProgramID", SqlDbType.VarChar, str[2]);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
    }
}
