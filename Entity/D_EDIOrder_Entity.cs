using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_EDIOrder_Entity : Base_Entity
    {
        public string EDIOrderNO { get; set; }
        public string StoreCD { get; set; }
        public string OrderDate { get; set; }
        public string VendorCD { get; set; }
        public string RecordKBN { get; set; }
        public string DataKBN { get; set; }
        public string CapitalCD { get; set; }
        public string CapitalName { get; set; }
        public string OrderCD { get; set; }
        public string OrderName { get; set; }
        public string SalesCD { get; set; }
        public string SalesName { get; set; }
        public string DestinationCD { get; set; }
        public string DestinationName { get; set; }
        public string OutputDatetime { get; set; }

        //検索用
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
    }
}
