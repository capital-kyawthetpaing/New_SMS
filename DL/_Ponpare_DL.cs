using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
namespace DL
{
  public  class _Ponpare_DL : Base_DL
    {
        public _Ponpare_DL()
        {

        }
        public DataTable Allow_Check()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };
            return SelectData(dic, "_Ponpare_Allow_Check");
        }
        public DataTable Ponpare_MAPI_Select()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };
            return SelectData(dic, "_Ponpare_MAPI_DRequest");
        }
        public bool InsertRirekiDetail(string StoreCD, String APIkey,string fromdate, string todate, String xml)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = StoreCD} },
                { "@API_Key", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  APIkey} },

                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.DateTime, value2 =fromdate} },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.DateTime, value2 = todate} },
                //{ "@xmldetail", new ValuePair { value1 = SqlDbType.Xml, value2 =  ame.Xmldetails} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = xml} }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "_Ponpare_InsertRirekiDetail");
        }
        public bool InsertEFGH(_PonpareEntity pet)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = pet.StoreCD} },
                { "@API_Key", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  pet.APIKey} },
                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.DateTime, value2 = pet.fromDate} },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.DateTime, value2 = pet.toDate} },
                { "@xmlJuchuu", new ValuePair { value1 = SqlDbType.Xml, value2 =  pet.Juchuu} },
                { "@xmlJuchuuDetails", new ValuePair { value1 = SqlDbType.Xml, value2 =  pet.JuchuuDetails} },
                { "@xmlCoupon", new ValuePair { value1 = SqlDbType.Xml, value2 =pet.Coupon} },
                { "@xmlEnclose", new ValuePair { value1 = SqlDbType.Xml, value2 = pet.Enclose} },
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "_Ponpare_InsertAllDetails");
        }
    }
}
