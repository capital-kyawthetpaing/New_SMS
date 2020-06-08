using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Entity
{
    public class _PonpareEntity : Base_Entity
    {
        public _PonpareEntity()
        {


        }

        public string ShopURL { get; set; }
        public string KEY { get; set; }
        public string PASSWORD { get; set; }
        public string StoreCD { get; set; }
        public string APIKey { get; set; }
        public string fromDate { get; set; }
        public string toDate {get; set;}
        public string Juchuu { get; set; }
        public string JuchuuDetails { get; set; }
        public string Coupon { get; set; }
        public string Enclose { get; set; }
    }
}
