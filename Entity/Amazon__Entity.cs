using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Entity
{
   public class Amazon__Entity:Base_Entity
    {
        public string strAccessKeyId { get; set; }
        public  string strSecretKeyId { get; set; }
        public  string strMerchantId { get; set; }
        public  string strMarketplaceId { get; set; }
        public  string strApplicationName { get; set; }
        public  string strApplicationVersion { get; set; }
        public  string strMWSAuthToken { get; set; }
        public  string strServiceURL { get; set; }
        public string StoreCD { get; set; }
        public string APIKey { get; set; }
        public string LastUpdatedAfter { get; set; }
        public string LastUpdatedBefore { get; set; }
        public string Xmldetails { get; set; }
        public string XmlOrder { get; set; }

        public string XmlOrderItems { get; set; }

    }
}
