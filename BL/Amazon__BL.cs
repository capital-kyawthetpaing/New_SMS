using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;
namespace BL
{
  public  class Amazon__BL : Base_BL
    {
        Amazon__DL adl = new Amazon__DL();
        public Amazon__BL()
        {

        }

        public Amazon__Entity MAPI_DRequest()
        {
            Amazon__Entity ame = new Amazon__Entity() ;
            DataTable dt = adl.MAPI_DRequest();
            ame.strMerchantId = dt.Rows[0]["SellerId"].ToString();
            ame.strMarketplaceId = dt.Rows[0]["MarketplaceId"].ToString();
            ame.strMWSAuthToken = dt.Rows[0]["MWSAuthToke"].ToString();
            ame.strAccessKeyId = dt.Rows[0]["AccessKeyId"].ToString();
            ame.strSecretKeyId = dt.Rows[0]["ServiceSecret"].ToString();
            ame.APIKey = dt.Rows[0]["APIKey"].ToString();
            ame.StoreCD = dt.Rows[0]["StoreCD"].ToString();
            return ame;
        }
        public bool Allow_Check()
        {
            return Convert.ToInt32(adl.Allow_Check().Rows[0]["Status"].ToString()) == 1 ? true : false;
        }
        public DataTable Select_AllOrderByLastRireki()
        {
            return adl.Select_AllOrderByLastRireki();
        }
        public string Amazon_DRequest()
        {
            var dt = adl.Amazon_DRequest();
             return (dt.Rows.Count== 1) ? (dt.Rows[0]["LastUpdatedBefore"].ToString()) : null;
        }

        public bool AmazonAPI_InsertOrderDetails(Amazon__Entity ame)
        {
            return adl.AmazonAPI_InsertOrderDetails(ame);
        }


        public bool AmazonAPI_Insert_NextToken(Amazon__Entity ame)
        {
            return adl.AmazonAPI_Insert_NextToken(ame);
        }
        public bool Amazon_InsertOrderItemDetails(Amazon__Entity OrderItems)
        {
            return adl.Amazon_InsertOrderItemDetails(OrderItems);
        }

    }
}
