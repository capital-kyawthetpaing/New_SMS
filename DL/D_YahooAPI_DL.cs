using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using System.Data.SqlClient;

namespace DL
{
   public class D_YahooAPI_DL:Base_DL
    {
        public D_YahooAPI_DL()
        {

        }

        public bool D_APIRireki_D_YahooCount_Insert(D_APIRireki_Entity apiRireki, D_YahooCount_Entity yahooCount)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  apiRireki.StoreCD } },
                { "@APIKey", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  apiRireki.APIKey } },
                { "@Status", new ValuePair { value1 = SqlDbType.TinyInt, value2 = yahooCount.Status.Equals("OK") ? "1": "0"  } },
                { "@Count_NewOrder", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_NewOrder } },
                { "@Count_NewReserve", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_NewReserve } },
                { "@Count_WaitPayment", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_WaitPayment } },
                { "@Count_WaitShipping", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_WaitShipping } },
                { "@Count_Shipping", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Shipping } },
                { "@Count_Reserve", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Reserve } },
                { "@Count_Holding", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Holding } },
                { "@Count_WaitDone", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_WaitDone } },
                { "@Count_Suspect", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Suspect } },
                { "@Count_SettleError", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_SettleError } },
                { "@Count_Refund", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Refund } },
                { "@Count_AutoDone", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_AutoDone } },
                { "@Count_AutoWorking", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_AutoWorking } },
                { "@Count_Release", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_Release } },
                { "@Count_NoPayNumber", new ValuePair { value1 = SqlDbType.Int, value2 = yahooCount.Count_NoPayNumber } }

            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "D_APIRireki_YahooCount_Insert");
        }


        public bool D_APIDetail_YahooList(D_APIRireki_Entity apiRireki, string yahooorder)
        {

            string sp = "D_APIDetail_YahooList";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.NVarChar, value2 = yahooorder} }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        //ImportYahooJuuChuu YJ
        public bool ImportYahooJuuChuu( string YJa)
        {
        
            string sp = "ImportYahooJuuChuu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.VarChar, value2 = YJa} }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        public bool ImportYahooShipping(string YJa)
        {

            string sp = "ImportYahooShipping";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.NVarChar, value2 = YJa} }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        public bool ImportYahooShippingDetail(string YJa)
        {

            string sp = "ImportYahooShippingDetail";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuChuuXml", new ValuePair { value1 = SqlDbType.NVarChar, value2 = YJa} }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        //ImportYahooShippingDetail
    }
}
