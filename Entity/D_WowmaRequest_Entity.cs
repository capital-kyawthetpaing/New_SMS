using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
 public  class D_WowmaRequest_Entity : Base_Entity
    {
        public string StoreCD { get; set; }
        public string APIKey { get; set; }
        public string LastUpdatedAfter { get; set; }
        public string LastUpdatedBefore { get; set; }
        public DataTable dtOrderList { get; set; }
        public string OrderListXml { get; set; }
        public string ApplicationID0 { get; set; }
        public string InportSEQRows { get; set; }
        public int Seq { get; set; }

        public DataTable dtXML { get; set; }
        //public string Seq { get; set; }     

    }
}
