using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
namespace DL
{
   public class Amazon__DL:Base_DL
    {
        public Amazon__DL() { }
        public DataTable Select_AllOrderByLastRireki()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

            };
            return SelectData(dic, "Amazon_Select_AllOrderByLastRireki");
        }
        public DataTable Allow_Check()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, "Amazon_Allow_Check");
        }
        public DataTable MAPI_DRequest()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, "Amazon_MAPI_DRequest");
        }
        public DataTable Amazon_DRequest()
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
            };
            return SelectData(dic, "Amazon_DRequest");
        }

        public bool AmazonAPI_InsertOrderDetails(Amazon__Entity ame)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  ame.StoreCD} },
                { "@API_Key", new ValuePair { value1 = SqlDbType.TinyInt, value2 = ame.APIKey} },

                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.DateTime, value2 = ame.LastUpdatedBefore} },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.DateTime, value2 =  ame.LastUpdatedAfter} },

                { "@xmldetail", new ValuePair { value1 = SqlDbType.Xml, value2 =  ame.Xmldetails} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = ame.XmlOrder} }
            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "Amazon_API_InsertOrderDetails");
        }

        public bool AmazonAPI_Insert_NextToken(Amazon__Entity ame)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {

                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  ame.StoreCD} },
                { "@API_Key", new ValuePair { value1 = SqlDbType.TinyInt, value2 = ame.APIKey} },

                { "@LastUpdatedBefore", new ValuePair { value1 = SqlDbType.DateTime, value2 = ame.LastUpdatedBefore} },
                { "@LastUpdatedAfter", new ValuePair { value1 = SqlDbType.DateTime, value2 =  ame.LastUpdatedAfter} },

                { "@xmldetail", new ValuePair { value1 = SqlDbType.Xml, value2 =  ame.Xmldetails} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = ame.XmlOrder} }

            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "Amazon_API_Insert_NextToken");

        }
        public bool Amazon_InsertOrderItemDetails(Amazon__Entity ame)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 =  ame.StoreCD} },
                { "@API_Key", new ValuePair { value1 = SqlDbType.TinyInt, value2 = ame.APIKey} },
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = ame.XmlOrderItems} }

            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "Amazon_InsertOrderItemDetails");
        }
        //MAPI_DRequest
    }
}
