using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_JANCounter_Entity : Base_Entity
    {
        public string MainKEY { get;set;}  //削除
        public string JanCount { get; set; }
        public string UpdatingFlg { get; set; }
        public string BeforeJanCount { get; set; }
    }
}
