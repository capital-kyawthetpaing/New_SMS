using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Nyuukin_Ichihanyou_Entity :Base_Entity      
    {
        public Nyuukin_Ichihanyou_Entity()
        {

        }
        public string paymentstart { get; set; }
        public string paymentend { get; set; }
        public string paymentinputstart { get; set; }
        public string paymentinputend { get; set; }
        public string cbo_store { get; set; }

        public string cbo_store_cd { get; set; }
        public string cbo_torikomi { get; set; }
        public string search_customer { get; set; }
        public string rdb_all { get; set; }
        public string PCID { get; set; }
        public string OPID { get; set; }
        public string KeyItem { get; set; }
        public string PID { get; set; }
    }
}
